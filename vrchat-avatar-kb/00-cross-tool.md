# ツール横断情報(NDMFエコシステム全体)

> 対象環境: Unity 2022.3.22f1 / VRCSDK3 Avatars(バージョン非依存の記述を基本とし、依存する箇所は明記)
> 情報源: 各ツールのGitHubリポジトリ(README / CHANGELOG / ソースコード)。2026年前半時点のHEADに基づく。
> 対象ツール: NDMF / Modular Avatar (MA) / TexTransTool (TTT) / lilToon / Poiyomi / AAO: Avatar Optimizer / VRCQuestTools (VQT)

---

## 1. NDMFビルドパイプラインと実行順序

### 1.1 VRChat SDKビルドへの介入ポイント

NDMFは VRCSDK の `IVRCSDKPreprocessAvatarCallback` に2つのフックを登録する(ソース: `ndmf/Editor/VRChat/BuildFrameworkPreprocessHook.cs`)。

| フック | callbackOrder | 実行するNDMFフェーズ |
|---|---|---|
| `BuildFrameworkPreprocessHook` | **-11000** | First 〜 Transforming |
| `BuildFrameworkOptimizeHook` | **-1025** (VRChatの `RemoveAvatarEditorOnly`(-1024) の直前) | Optimizing 〜 Last |

この分割により、**NDMF非対応ツール(VRCFury: callbackOrder -10000 など)は「NDMFのTransforming後・Optimizing前」に実行される**。VRCFury等と併用する場合の順序問題はここで理解する。

- Play Mode(Apply on Play)でも同じパイプラインが実行される。Av3Emulator / Gesture Manager が存在する場合はNDMFが二重実行を抑止する。
- NDMF 1.5.0以降、Transformingフェーズ完了時点で未保存アセットがシリアライズされる(VRCFury等の外部フックがアセット参照を壊さないための措置)。

### 1.2 ビルドフェーズ

ドキュメント上の公開フェーズは4つ: **Resolving → Generating → Transforming → Optimizing**。

**ドキュメントとソースの齟齬(ソースが正)**: 実際の`BuildPhase`にはこれに加えて `First` / `PlatformInit` / `Last`(および内部的なFirstChance)が存在し、VRChat向けフックは `First〜Transforming` / `Optimizing〜Last` の範囲で実行される。`PlatformInit` はNDMF 1.13+のマルチプラットフォーム基盤の初期化用。一般のプラグインが使うのは公開4フェーズと考えてよい。

| フェーズ | 想定用途 | 代表的な処理 |
|---|---|---|
| Resolving | 大規模変更前の参照解決・状態取得 | MAのオブジェクト参照解決、アニメーターのクローン、NDMF組み込みの `RemoveEditorOnlyPass`(EditorOnlyオブジェクト削除)、VQTのプラットフォーム別コンポーネント除去 |
| Generating | 後続プラグインが消費するコンポーネントの生成 | VQTのNetwork ID割り当て。MA向けコンポーネントを生成するツールはここ |
| Transforming | 一般的なアバター変換の本体 | MAの主要処理、TTTのテクスチャ改変、VQTのアバター変換(Transformingモード時) |
| Optimizing | 純粋な最適化 | TTTのAtlasTexture等、VQTの変換(既定のAutoモード)、AAOの全最適化、MAのGC |

### 1.3 同一フェーズ内のプラグイン順序決定アルゴリズム

ソース(`ndmf/Editor/API/Solver/PluginResolver.cs`)より:

1. `[assembly: ExportsPlugin(typeof(...))]` で登録された全プラグインを、**プラグイン型のFullName(名前空間+クラス名)の序数(ordinal)順**でソートして登録する。
2. 各プラグインの `Configure()` が宣言した Sequence / Pass と制約(`BeforePlugin` / `AfterPlugin` / `BeforePass` / `AfterPass` / `WaitFor`)を集め、**フェーズごとにトポロジカルソート**する。
3. 制約は「相手プラグインが存在する場合のみ」効く(オプショナル依存)。フェーズをまたぐ制約は例外を投げる。
4. 同一フェーズに複数Sequenceを宣言した場合、Sequence間の順序は保証されない(インターリーブされうる)。

**重要な帰結**:
- 制約が無いプラグイン同士の順序は「クラスのFullNameのアルファベット順」で決まる。AAOはこれを利用し、プラグインクラスをU+FFDC文字で始まる名前空間に置くことで**ほぼ確実に最後へ**ソートされるようにしている(AAO 1.9.0以降。ソース: `AvatarOptimizer/Editor/OptimizerPlugin.cs` のコメント)。
- 「AAOの後に実行したい」ツールは `AfterPlugin("com.anatawa12.avatar-optimizer")` を明示する必要がある。

### 1.4 対象ツールのプラグインIDとフェーズ配置(ソース確定情報)

| ツール | プラグインQualifiedName | Resolving | Generating | Transforming | Optimizing |
|---|---|---|---|---|---|
| Modular Avatar | `nadena.dev.modular-avatar`(+ `nadena.dev.modular-avatar.late-transform-stages`) | 参照解決・アニメータークローン | - | 本体ほぼ全部 / late側はTTT後にFloor Adjuster+MAコンポーネント purge | GC GameObjects |
| TexTransTool | `net.rs64.tex-trans-tool` | プレビュー解除・旧データ検出 | - | Material/UV系6サブフェーズ | AtlasTexture等+コンポーネント purge |
| AAO | `com.anatawa12.avatar-optimizer` | 早期MakeChildren等(RemoveEditorOnlyPassより前) | - | - | 全最適化(最後尾を狙う) |
| VRCQuestTools | `com.github.kurotu.vrc-quest-tools` | ビルドターゲット設定・Platform Remover類(MAより前) | Network ID割り当て | アバター変換(MA/TTT/lilycalInventory後)・MeshFlipper | アバター変換(AAOより前)・テクスチャ形式チェック(AAO後) |
| lilToon | (NDMFプラグインではない) | - | - | - | 独自のビルドコールバックでマテリアル/シェーダー最適化。NDMFがある場合はApply on Play時にスキップ可(1.8.4+) |
| Poiyomi | (NDMFプラグインではない) | - | - | - | ThryEditorのLock機構がVRChatビルドコールバックで動作 |

### 1.5 明示されている順序制約(ソースから抽出)

**TTT** (`TexTransTool/Editor/NDMF/NDMFPlugin.cs`):
- Transforming: `BeforePlugin` → Light Limit Changer(`io.github.azukimochi.light-limit-changer`)、FloorAdjuster(`net.narazaka.vrchat.floor_adjuster`)、Mantis LOD Editor
- Optimizing: `BeforePlugin` → AAO、Mantis LOD、Meshia MeshSimplification、lilNDMFMeshSimplifier、lilycalInventory
- AAO存在時は `NegotiateAAOPass` がTransforming先頭で連携設定を行う

**MA late stages** (`modular-avatar/Editor/PluginDefinition/PluginDefinition.cs`):
- Transforming: `AfterPlugin` → MA本体、**TTT**、marshmallow_PB。ここでFloor Adjuster実行→MAコンポーネント全破棄→ヒューマノイド再バインド(MA 1.17.1でこの構造になった。TTTのDecal等がMAコンポーネント位置に依存するため)

**VQT** (`VRCQuestTools/.../Editor/NDMF/VRCQuestToolsNdmfPlugin.cs`):
- Resolving: `BeforePlugin` → MA、VirtualLens2
- Transforming(変換パス): `AfterPlugin` → TTT、MA、lilycalInventory / MeshFlipperはMantisLODより前、RemoveVertexColorはMantisLODより後
- Optimizing(変換パス): `AfterPlugin` → TTT、posing system converter、lilNDMFMeshSimplifier、Meshia、Overall NDMF Mesh Simplifier / `BeforePlugin` → AAO
- Optimizing(CheckTextureFormatPass): `AfterPlugin` → TTT、**AAO**

### 1.6 実効的な全体実行順(標準構成: MA + TTT + AAO + VQT)

```
[callbackOrder -11000]
  Resolving:    VQT(Platform除去/変換準備) → MA(参照解決/アニメータークローン) → TTT(プレビュー解除) → AAO(早期パス) → NDMF組み込みEditorOnly削除
  Generating:   VQT(NetworkID)
  Transforming: MA本体(パラメータ改名→アニメーター統合→リアクティブ→メニュー→Merge Armature→…)
                → TTT(MaterialModification→BeforeUV→UV→AfterUV→PostProcessing)
                → MA late(Floor Adjuster→MAコンポーネント破棄)
                → VQT変換(Transformingモード時)
[callbackOrder -10000 など: VRCFury等の非NDMFツール]
[callbackOrder -1025]
  Optimizing:   TTT(AtlasTexture等→TTTコンポーネント破棄) → VQT変換(Autoモード既定) → AAO(全最適化) → VQT(テクスチャ形式チェック)
[callbackOrder -1024] VRChatがEditorOnly破棄 → 以降VRCSDK標準処理
```

---

## 2. 互換性マトリクス

○=公式に連携/考慮あり、△=条件付き・注意、×=非互換または非対応。根拠は各ツールのCHANGELOG/ソース。

| | MA | TTT | AAO | lilToon | Poiyomi | VQT |
|---|---|---|---|---|---|---|
| **NDMF** | ○ 同一作者。MAはNDMF同時更新が前提(下記「バージョン組み合わせ」) | ○ 0.4.0以降NDMF必須化。0.10+はNDMF≥1.7 | ○ 1.5.0以降NDMF統合 | △ プラグインではないがNDMFを検知して挙動変更 | × 関知しない | ○ NDMFなしでも動くがNDMF系コンポーネントは無効 |
| **MA** | - | ○ TTTはMA後に実行。MA MaterialSetter/Animationで追加されたマテリアルにもTTTが効く(TTT 0.10+ / NDMF≥1.7)。MA lateパスがTTT後にFloor Adjuster実行 | ○ MAのDelayDisableレイヤーはAAOのオブジェクト追跡を考慮(MA 1.12+)。MAのGCはOptimizingでAAOより前 | ○ 特記事項なし(マテリアル改変なし) | ○ 特記事項なし | △ VQTはMA後に変換。MA Visible Head Accessory / World Fixed ObjectはMA 1.9+なら削除されない。未対応MAバージョンではエラー(VQT側) |
| **TTT** | | - | ○ 双方向連携: TTTはAAOより前(BeforePlugin)。AAOのRemoveMeshBy*領域をアトラス化から除外+UV退避報告(TTT 0.9+のNegotiateAAO、AAO 1.8のAPI)。同一メッシュ対象時の例外はTTT 0.10.9で修正 | ○ AtlasTextureにlilToon専用サポート(HSVG/MatCap等のプロパティベイク)。宝石・Lite系は対象外(誤検出は0.8.0/0.8.3で修正) | △ 専用AtlasShaderSupportなし。_MainTex等の汎用処理は可能だがプロパティベイク非対応 | ○ VQTはTTT後に実行(生成テクスチャを変換対象に含める)。テクスチャ形式チェックもTTT後(VQT 2.6.0) |
| **AAO** | | | - | ○ Optimize Texture / Remove unused propertiesはlilToon対応(1.8+)。lilToon固有の修正多数(アウトラインマスク1.8.1、AngelRing 1.8.5、AudioLinkマスク1.9.4、lilToon 1.9対応は1.8.9) | △ Optimize TextureはPoiyomi非対応(lilToon/Toon Standardのみ)。ロック済みマテリアルの未使用プロパティ削除は動作する | ○ VQT変換はAAOより前、形式チェックはAAO後。**AAO 1.7.0でVQT v1互換を削除→VQT 2.x必須** |
| **lilToon** | | | | - | △ 相互変換ではないがPoiyomi 9.3.64にlilToon→Poiyomi翻訳機能あり | ○ lilToon→Toon Lit/Toon Standard変換が最も充実。lilToon 2.0対応はVQT 2.11.1+。カスタムシェーダー派生も変換可(VQT 2.7.0+) |
| **Poiyomi** | | | | | - | △ Poiyomi→Toon Lit変換(メインカラー/ノーマル陰影/Emission 0-3)。→Toon Standardは実験的(VQT開発版) |

### 既知の非互換・要注意の組み合わせ(NDMF外含む)

- **VRCFury**: NDMFプラグインではないため順序制御不能(常にNDMF Transforming後・Optimizing前)。MA 1.15+はVRCFury < 1.1250.0 と Mesh Cutter / Shape Changer(delete mode)の併用に警告を出す(非互換)。
- **Av3Emulator**: NDMFはAv3Emulator検出時にApply on Playを抑止(二重適用防止)。AAOはAv3Emulator起動時にRead/Write無効メッシュを処理できないケースがある(1.8.0で大幅改善)。
- **AAO + `-nographics` バッチビルド**: AAO 1.8.0以降はGraphicsなし環境で動作しない(テクスチャ処理にGPUが必要)。CIでは`-nographics`を外す。
- **旧TTTセーブデータ**: TTT 1.0.0はv0.8.x以前のセーブデータ読み込みを削除。古いアセットは旧バージョン経由でマイグレーション必須。
- **Unity 2019**: NDMF 1.5.0+/AAO 1.8.0+/VQT 3.x(開発版)/Poiyomi 9.2.43+はUnity 2019サポートを終了済み。Unity 2022.3系を前提とする。
- **プラグイン名の序数ソート**: 独自ツールがAAOより後に動きたい場合、明示的な`AfterPlugin("com.anatawa12.avatar-optimizer")`が必要。かつAAOにコンポーネントを認識させる(Asset Description / 登録API)必要がある場合がある。

### バージョン組み合わせの推奨(2026年前半時点の安定構成)

| ツール | 安定版 | 備考 |
|---|---|---|
| NDMF | 1.14.x | Unity 6000系対応は1.14.0以降 |
| Modular Avatar | 1.17.x | 要NDMF ≥1.11(package.json: `>=1.11.0 <2.0.0-a`) |
| AAO | 1.9.x | 要NDMF `>=1.8.0 <2.0.0`、VRCSDK `>=3.7.0 <3.11.0`(1.9.13でVRCSDK 3.7.0互換を回復) |
| TexTransTool | 1.0.x | 要NDMF ≥1.7、TexTransCore 0.2.x |
| lilToon | 2.3.x | VRC Light Volumes同梱。1.10.xは2022系最終安定の旧系列 |
| Poiyomi | 9.3.x (LTS) | 10.0開発中。9.2系からの移行推奨 |
| VRCQuestTools | 2.11.x | 次期メジャーでVRCSDK<3.9 / lilToon<1.10 / NDMF<1.5 切り捨て予定(Unreleased) |

原則: **MA / NDMF / AAO はセットで最新安定版に揃える**。NDMFだけ古い・新しい状態は最も事故が多い(各ツールがNDMFのバージョンをVersion Definesで検知して機能を切り替えるため)。

---

## 3. 共通トラブルシュートフロー

### 3.1 ビルドエラー/意図しない出力が出たら最初に確認する項目

1. **NDMF Console**(旧Error Report)を開く(`Tools/NDM Framework/`)。エラーの発生プラグインとオブジェクト参照が表示される。日本語ロケール対応。
2. **バージョン整合**: VCCで NDMF / MA / AAO / TTT を全て最新安定版へ。片方だけの更新が原因のコンパイルエラー・API欠落が最頻出(例: NDMF 1.7.2の`IVirtualizedMotion`名前空間移動、NDMF 1.6.1の`IExtensionContext.Owner`削除)。
3. **コンパイルエラーの有無**: Consoleに赤エラーがある状態ではNDMF自体が動かない。VRCSDKなしプロジェクト・World SDK混在プロジェクトでのエラーは各ツールの既知問題(lilToon 1.8.2、MA 1.17.0 #1973等)を確認。
4. **Manual Bake**で出力を検査: アバター右クリック → `Modular Avatar/Manual Bake Avatar`(またはNDMFのManual Bake)。生成物を直接見ると、どのフェーズで壊れたか切り分けられる。
5. **プラグイン単位で無効化**: NDMF 1.7.0+の `Tools/NDM Framework/` にあるプラグイン有効/無効ウィンドウで二分探索する。`Apply on Build`のトグルも同メニューにある(1.5.4+)。
6. **Apply on Play切り分け**: Play時のみ壊れる場合はAv3Emulator/Gesture Managerとの相互作用を疑う。NDMFのApply on Playを切って比較。

### 3.2 症状別の定石

| 症状 | 疑うポイント |
|---|---|
| 服が体を貫通/ボーンが二重 | MA Merge Armatureの対象・プレフィックス設定、Armature名の不一致、Scale Adjusterの有無 |
| トグルが他人に見えない(ローカルのみ動作) | MA Parametersのsynced設定、パラメータ数超過(Int自動値0-255超過はMA 1.18+でビルドエラー化)、Sync Parameter Sequenceの設定不整合 |
| 表情/トグルがMMDワールドで壊れる | MA 1.12+のMMD対応(`MA VRChat Settings`で挙動制御、`MA MMD Layer Control`)。WD OFFレイヤーとの併用警告(1.14+) |
| テクスチャがずれる/デカール位置が違う | 実行順: FloorAdjuster等のTransform移動系がTTTより先に動いていないか(TTT 0.8.2/MA 1.17.1で対策済みだが、他のTransform移動系ツールは要確認) |
| アトラス化後に一部マテリアルが元に戻る | TTTのAtlasTextureとマテリアル差し替え(MA Material Setter/アニメーション)の順序。TTT 0.10+ / NDMF 1.7+ で対応済み。旧版は非対応 |
| ビルド後にメッシュが消える | AAOのRemove Mesh系/自動FreezeBlendShape(∞デルタは1.9.0で修正)、AAOのRemove Zero Sized Polygon(Advanced扱い)。AAO exclusionsに追加して切り分け |
| PhysBoneの挙動が変わった | AAOのMerge PhysBone / Auto Merge PhysBone / EndBone置換(1.9系で多数の修正あり→AAOを最新パッチへ) |
| アニメーションが効かない/プロパティが飛ぶ | パス書き換えの多重適用(NDMF 1.7.7で修正)、AAOのMergeBoneによるパス変更(ビルド前パスでアニメを作る)、lilToonの最適化でアニメ対象プロパティが定数化(lilToon 1.8.0+の外部ツールAPI/アニメーション考慮で改善) |
| アップロード時だけ壊れる(Playでは正常) | Optimizingフェーズ(AAO/TTT Atlas/VQT)以降の問題。Manual Bakeで確認。lilToonは「テストビルドでは最適化しない」オプションの差にも注意 |
| Questビルドで紫/真っ黒 | シェーダーがMobile非対応(lilToon/PoiyomiはQuest不可)→VQTで変換。テクスチャ形式エラーはVQTのCheckTextureFormatの警告を見る |
| プレビューが固まる/Sceneビューがおかしい | NDMFプレビューシステムの既知問題(1.9.4等で修正多数)。NDMFを最新へ、ダメならプレビューをオフ(NDMF Console/設定から) |

### 3.3 レポートを書く/調べるときに使う情報

- NDMFのビルドログ・プラグインシーケンス表示ウィンドウ(実行パス順が見える)
- AAO 1.9.0+の「Bug Report Helper」(`Tools/Avatar Optimizer/`): T&O設定のJSONダンプ含む収集機能
- lilToonのバグレポート生成: `GameObject/lilToon/[Debug] Generate bug report`
- 各ツールのGitHub Issues(既知問題の検索が最速): bdunderscore/modular-avatar, bdunderscore/ndmf, anatawa12/AvatarOptimizer, ReinaS-64892/TexTransTool, lilxyzw/lilToon, poiyomi/PoiyomiToonShader, kurotu/VRCQuestTools

---

## 4. 各ツール詳細への参照

改変ワークフロー順(01基盤 → 03着せる → 04-06見た目 → 07-08動き・メニュー → 09-10軽量化 → 11 Quest → 12検証):

- [01 NDMF](01-ndmf.md) — フレームワーク本体。全ツールの前提
- [02 Modular Avatar](02-modular-avatar.md) — 衣装結合・メニュー/パラメータ・リアクティブ
- [03 衣装フィッティング](03-fitting-tools.md) — もちふぃった～ / EreMorph / KiseteneEx(非対応衣装)
- [04 TexTransTool](04-textranstool.md) — テクスチャ改変・デカール・アトラス化
- [05 lilToon](05-liltoon.md) — シェーダー(日本圏標準)
- [06 Poiyomi Toon Shader](06-poiyomi.md) — シェーダー(英語圏標準)
- [07 表情・ギミック生成](07-expression-gimmick-tools.md) — FaceEmo / CGE / Light Limit Changer / FloorAdjuster等
- [08 周辺エコシステムツール](08-ecosystem-tools.md) — AvatarMenuCreatorForMA / lilycalInventory / Flare等、および順序制約に登場するNDMFプラグインID早見表
- [09 AAO: Avatar Optimizer](09-avatar-optimizer.md) — 自動最適化
- [10 軽量化・変換](10-optimization-conversion-tools.md) — ポリゴン削減・マテリアル変換
- [11 VRCQuestTools](11-vrcquesttools.md) — Quest/Android/iOS対応
- [12 検証・分析・アップロード・環境管理](12-analysis-upload-tools.md) — Gesture Manager / Av3Emulator / vrc-get等
