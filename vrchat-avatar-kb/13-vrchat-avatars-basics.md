# VRChat / Avatars 3.0 基礎リファレンス

ツール知識の土台となるVRChat側の仕様。数値は**VRChat公式Creator Docs(github.com/vrchat-community/creator-docs)のソースから転記**(2026年前半時点。VRChatの仕様変更で変わりうるため、判断が際どい場合は[公式](https://creators.vrchat.com/avatars/avatar-performance-ranking-system/)で最新を確認)。

## Playable Layers(アニメーターの層構造)

Avatars 3.0のアバターは複数のAnimator Controllerを層として持つ:

| レイヤー | 役割 | 改変ツールとの関係 |
|---|---|---|
| Base | ロコモーション(歩行・ジャンプ) | GoGo Loco等の移動システムが差し替える対象([15](15-gogo-loco.md)) |
| Additive | 呼吸等の加算アニメーション | 通常触らない |
| Gesture | 手指・ジェスチャー | 表情ツールが手形状を参照([07](07-expression-gimmick-tools.md)) |
| Action | エモート・AFK | GoGo Loco等が拡張 |
| **FX** | マテリアル・BlendShape・トグル等の見た目全般 | **改変ツールの主戦場**。MA Merge Animator等はここへ統合するのが基本 |
| Sitting / TPose / IKPose | 着席・キャリブレーション用 | 通常触らない |

- FX以外のレイヤーではマテリアル等の見た目プロパティを動かさないのが原則(FXはヒューマノイドボーンを動かさない)
- MMDワールドはFXレイヤー1〜2を無効化して独自表情を流す→MAのMMD対応([02](02-modular-avatar.md))の背景

## Expression Parameters(同期パラメータ)

- **同期メモリ上限: 256 bit**(VRCSDKの`VRCExpressionParameters.MAX_PARAMETER_COST`)。Bool=1bit、Int=8bit、Float=8bit
- synced(全員に同期)/ local only(自分のみ)、saved(ワールド間で保持)、default値を持つ
- 改変ツールとの関係:
  - [MA Parameters](02-modular-avatar.md)が衝突回避のリネーム・使用量集計(MA Informationで超過表示)を担う
  - ギミック導入時は「そのギミックが何bit使うか」を確認する(例: [GoGo Loco](15-gogo-loco.md)は16/256bit+拡張1bit)
  - lilycalInventoryはInt圧縮等のパラメータ最適化を内蔵([08](08-ecosystem-tools.md))
- 主な組み込みパラメータ(FXから参照可能): `IsLocal` / `GestureLeft(Right)` / `GestureLeftWeight` / `Viseme` / `TrackingType` / `VRMode` / `MuteSelf` / `AFK` / `Seated` など(全一覧は公式animator-parameters参照)

### OSC(参考)

Expression ParametersはOSCで外部から読み書きできる(`/avatar/parameters/<名前>`)。OSCギミック改変時の注意: AAOはOSC使用パラメータを削除しうるため、**Asset DescriptionでOSC使用を宣言する仕組み**がある([09](09-avatar-optimizer.md)、AAO 1.8.0+)。

## Performance Rank(公式表の転記)

- ランクは Excellent / Good / Medium / Poor / **Very Poor(上限なし)** の5段階。**どれか1項目でも超えたら次のランクに落ちる**
- **無効化中のGameObject/Componentもすべてカウントされる**(トグルで隠してもランクは変わらない→ビルド時に削除する[AAO](09-avatar-optimizer.md)等が有効な理由)
- **Read/Write無効のメッシュが1つでもあるとTriangles計測不能で強制Very Poor**(SDKが警告)

### PC Limits

| 項目 | Excellent | Good | Medium | Poor |
|---|---|---|---|---|
| Triangles | 32,000 | 70,000 | 70,000 | 70,000 |
| Texture Memory | 40 MB | 75 MB | 110 MB | 150 MB |
| Skinned Meshes | 1 | 2 | 8 | 16 |
| Basic Meshes | 4 | 8 | 16 | 24 |
| Material Slots | 4 | 8 | 16 | 32 |
| PhysBones Components | 4 | 8 | 16 | 32 |
| PB Affected Transforms | 16 | 64 | 128 | 256 |
| PhysBones Colliders | 4 | 8 | 16 | 32 |
| PB Collision Check Count | 32 | 128 | 256 | 512 |
| Contacts | 8 | 16 | 24 | 32 |
| Constraint Count | 100 | 250 | 300 | 350 |
| Constraint Depth | 20 | 50 | 80 | 100 |
| Animators | 1 | 4 | 16 | 32 |
| Bones | 75 | 150 | 256 | 400 |
| Lights | 0 | 0 | 0 | 1 |
| Particle Systems | 0 | 4 | 8 | 16 |
| Total Particles Active | 0 | 300 | 1000 | 2500 |
| Trail / Line Renderers | 各1 | 各2 | 各4 | 各8 |
| Cloths | 0 | 1 | 1 | 1 |
| Audio Sources | 1 | 4 | 8 | 8 |

- **PCのTrianglesはGood以上=70,000が実質上限**(超えたら即Very Poor)
- PCの既定「Minimum Displayed Performance Rank」は**Very Poor=全員表示**。ユーザーがMedium/Poorに絞ると、超過項目に応じて「コンポーネント除去」または「Fallback置換」(Triangles/Texture Memory/メッシュ数/マテリアル数/Bones超過はFallback置換=丸ごと差し替え)

### Mobile(Quest/Android/iOS) Limits

| 項目 | Excellent | Good | Medium | Poor |
|---|---|---|---|---|
| Triangles | 7,500 | 10,000 | 15,000 | 20,000 |
| Texture Memory | 10 MB | 18 MB | 25 MB | 40 MB |
| Skinned Meshes | 1 | 1 | 2 | 2 |
| Basic Meshes | 1 | 1 | 2 | 2 |
| Material Slots | 1 | 1 | 2 | 4 |
| Animators | 1 | 1 | 1 | 2 |
| Bones | 75 | 90 | 150 | 150 |
| PhysBones Components | 0 | 4 | 6 | 8 |
| PB Affected Transforms | 0 | 16 | 32 | 64 |
| PhysBones Colliders | 0 | 4 | 8 | 16 |
| PB Collision Check Count | 0 | 16 | 32 | 64 |
| Contacts | 2 | 4 | 8 | 16 |
| Constraint Count | 30 | 60 | 120 | 150 |
| Constraint Depth | 5 | 15 | 35 | 50 |
| Particle Systems | 0 | 0 | 0 | 2 |

- Mobileの既定ブロックは**Medium**(Poor以下は非表示。ユーザーはPoorまで緩和可能だがVery Poor表示設定は存在しない。個別の「Show Avatar」でのみ表示可 — **この救済は将来削除される可能性を公式が明言**)
- **ハードキャップ(Show Avatarでも回避不可)**: PhysBone 8個 / 影響Transform 64 / コライダー16 / 衝突チェック64 / Contacts 16 / Constraint 150・深さ50 — 超過すると**該当コンポーネントが全て除去される**
- Mobileでは Lights / Cloth / 物理コライダー / Rigidbody / **Audio Source** は常に無効(存在しても0扱い)
- Raycastsは全プラットフォーム共通で1アバター80個のハード上限

## アバターサイズ制限(公式表の転記)

| プラットフォーム | ダウンロードサイズ(圧縮後) | 非圧縮サイズ |
|---|---|---|
| PC | 200 MB | 500 MB |
| Android(Quest等) | 10 MB | 40 MB |

- SDKがビルド時に検査(Android非圧縮はSDK 3.5.2+、PC制限は3.7.0+で強制。それ未満のSDKだとアップロードがサーバー側で失敗する)
- Build & Testでは制限が適用されない(=テストで通ってもアップロードで弾かれることがある)

## セーフティ / フォールバックの2系統(混同注意)

1. **アバターFallback**: Performance Rankブロックやセーフティで本体が表示されないとき、代わりに表示される軽量アバター。自分で設定でき(Fallback対応アバターをFallbackに指定)、未設定ならロボット等のデフォルト。VQTの`VQT Fallback Avatar`はこの自動設定を支援([11](11-vrcquesttools.md))
2. **シェーダーFallback**: セーフティで「Shaders」がオフにされた相手には、マテリアルの`VRCFallback`タグに基づく標準シェーダーで描画される。lilToon/Poiyomiはフォールバック設定UIを持ち([05](05-liltoon.md)/[06](06-poiyomi.md))、Toon Standard(Outline)フォールバックも指定可能(lilToon 1.10+)。**見え方の事前確認はlilAvatarUtilsのフォールバックシミュレーション**([12](12-analysis-upload-tools.md))

## ビルドコールバック(改変ツールの入口)

VRCSDKは`IVRCSDKPreprocessAvatarCallback`をcallbackOrder順に実行する。主要な順序(ソース確認済みの値は[00 §1.1](00-cross-tool.md)):

| order | 実行者 |
|---|---|
| -11000 | NDMF前段(First〜Transforming: MA/TTT等) |
| -10000 | VRCFury([14](14-vrcfury.md)) |
| -1025 | NDMF後段(Optimizing〜: AAO/VQT等) |
| -1024 | VRChatのEditorOnly破棄(`RemoveAvatarEditorOnly`) |
| 0〜 | VRCSDK標準処理 |

## 関連ページ

- 実行順序の詳細: [00-cross-tool.md](00-cross-tool.md) / ランク改善の実務: [09](09-avatar-optimizer.md)・[10](10-optimization-conversion-tools.md) / Quest対応: [11](11-vrcquesttools.md) / ランク実測: [12](12-analysis-upload-tools.md)のActual Performance Window
