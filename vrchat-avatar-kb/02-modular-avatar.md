# Modular Avatar (MA)

- リポジトリ: https://github.com/bdunderscore/modular-avatar
- ドキュメント: https://modular-avatar.nadena.dev (https://m-a.nadena.dev)
- パッケージ名: `nadena.dev.modular-avatar`
- 作者: bd_(NDMFと同一作者)

## 概要

**コンポーネントをドラッグ&ドロップで組み合わせてアバターを非破壊に組み立てる**ツールキット。衣装のアーマチュア統合、アニメーター/メニュー/パラメータの合成、リアクティブ(トグル)生成が中核。処理は全てNDMFプラグインとしてビルド時に実行される。

- Unity要件: 2022.3(1.13.x以前は2019系もサポートしていた時期あり。現行は2022専用)
- VRCSDK: Avatars 3.0前提の機能が多いが、**1.13.0以降は非VRChatプラットフォームを実験的サポート**(VRCSDKなしでもコンパイル可)。1.15.0で`com.vrchat.avatars`への強依存を除去
- 配布: VPM(vpm.nadena.dev)。**unitypackage配布は非推奨経路**

## 依存関係

- 必須: NDMF(1.17.xは `>=1.11.0 <2.0.0-a`)、`com.unity.nuget.newtonsoft-json`
- VRCSDK Avatars: オプショナル(`MA_VRCSDK3_AVATARS`デファインで機能が切り替わる)
- 衣装側アセットがMAコンポーネント入りプレハブを同梱する形での再配布を想定(MITライセンス)

## 対応する改変パターン

コンポーネント単位で列挙(ビルド時に全て解決され、MAコンポーネントは最終的に削除される):

### 衣装・オブジェクト結合
- **MA Merge Armature**: 衣装アーマチュアをアバター本体へ統合(ボーン名ヒューリスティックマッピング、Prefix/Suffix除去)。PhysBone付きヒューマノイドボーンの統合は条件付き可(1.12+)
- **MA Bone Proxy**: オブジェクトを指定ボーン配下へ移動(アクセサリ装着)。ターゲットに他のBone Proxy/Merge Armature配下も指定可(1.16+)、Match scaleオプション(1.17+)
- **MA Mesh Settings**: Anchor Override / Boundsをアバター単位で統一設定
- **MA Replace Object**: オブジェクトを別オブジェクトで置換(素体メッシュ差し替え等)
- **MA Move Independently**: 親子関係を保ったまま独立移動(編集時支援)
- **MA Scale Adjuster**: ボーンスケールの調整(ヒューマノイドボーン長も対応、1.15+)
- **MA World Fixed Object**: ワールド固定オブジェクト(1.12+でVRCParentConstraint使用→Android対応)
- **MA Visible Head Accessory**: 一人称視点で自分の頭部アクセサリを可視化
- **MA World Scale Object**: ワールドスケール追従(1.12+)

### アニメーター/表情
- **MA Merge Animator**: アニメーターコントローラーをPlayable Layer(FX等)へ統合。相対/絶対パスモード、Write Defaults一致オプション、**既存アニメーターの置換**(1.12+)
- **MA Merge Motion (Blend Tree)**: BlendTree/AnimationClipをDirect Blend Treeとして統合(旧名Merge Blend Tree、1.12で改名)
- **MA Blendshape Sync**: 服と素体のBlendShape値を同期(カーブリマップ対応は1.18+)
- **MA MMD Layer Control**: MMDワールド互換挙動のレイヤー単位制御(1.12+)
- **MA Convert Constraints**: Unity Constraint→VRC Constraintへビルド時変換(Quest対応にも有効)

### メニュー/パラメータ
- **MA Menu Installer / Menu Item / Menu Group**: Expressions Menuの合成・オブジェクトベースのメニュー編集。アイコン自動圧縮(iOS対応修正1.12+)
- **MA Parameters**: Expression Parametersの追加・自動リネーム(衝突回避)・値インポート
- **MA Rename Collision Tags**: Contact系のcollisionTagsを一意名へリネーム(1.13+)
- **MA Sync Parameter Sequence**: PC/Quest等プラットフォーム間でパラメータ順序を同期(1.16+でUnity Library配下に自動保存、プライマリ→セカンダリ同期に変更)
- **MA VRChat Settings**: MMD対応等のVRChat固有挙動の設定(1.12+)

### リアクティブコンポーネント(1.8系〜、ビルド時にアニメーター生成)
- **MA Object Toggle**: オブジェクトのON/OFFトグル
- **MA Shape Changer**: BlendShape値の変更/メッシュ削除(Delete mode)
- **MA Material Setter**: マテリアル差し替え
- **MA Material Swap**: マテリアル一括スワップ(1.13+)
- **Menu Item連動**: メニュー項目のトグルとリアクティブの自動接続、自動パラメータ割当

### メッシュ加工
- **MA Mesh Cutter**(1.14+): 頂点フィルタ(By Bone / By Blendshape / By Axis / By Mask / By UV Tile(1.18+))によるメッシュ部分削除・トグル
- **MA Remove Vertex Color**: 頂点カラー削除

### その他
- **MA Platform Filter**(1.13+): プラットフォーム別にオブジェクトを除外
- **MA Global Collider**(1.15+): VRC標準コライダーの付け替え
- **MA Floor Adjuster**(1.17+): 接地高さ調整(narazaka版FloorAdjusterのMA内蔵版)
- **MA PhysBone Blocker**: 子オブジェクトへのPhysBone影響を遮断
- **MA Fit Preview**(1.15+): 編集モードでPhysBone付きポーズプレビュー
- GCによる未使用オブジェクト削除(Optimizingフェーズ、アニメ参照は1.15+で保持)

## 改変時の注意点(ソース由来の癖)

- **パス順序**(`Editor/PluginDefinition/PluginDefinition.cs`): Rename Parameters → Merge Blend Tree → Merge Animator → Reactive Components → Menu Install → Merge Armature → Bone Proxy → … の順。**Menu InstallerはMerge Armatureより前**に処理される(Merge ArmatureがMenu Installer入りオブジェクトを破壊しうるため。ソースコメントにTODOあり)
- **Merge Armatureの前にBoneProxyPrepassがターゲットを解決**する。Merge Armatureで動く前提のBone Proxyも動作するが、1.16.0→1.16.1で「Merge Armature配下のBone Proxy」のリグレッションがあった(常に最新パッチ推奨)
- リアクティブコンポーネントは`ReadablePropertyExtension`必須のシーケンスで一括実行され、プレビューはShape Changer/Mesh Deleter/Object Switcher/Material Setterの4フィルタ
- **EditorOnlyタグのオブジェクト**: MAコンポーネントのみ除去し、オブジェクト自体はPlay Modeでは残す(`ClearEditorOnlyTags`)。ビルドではNDMF/VRChatが削除
- Merge AnimatorのWD一致ロジックは1.12系で頻繁に変化した(1.12.3/1.12.4/1.13.0)。単一ステートBlendTreeレイヤーはWD ONに強制される等。WD混在アセットの互換問題はまずMAのバージョンを疑う
- パラメータ自動リネームは1.12.0からオブジェクトパスベースの名前になった(Sync Parameter Sequence互換性向上。**更新時はSyncedParamsアセットを空にして全プラットフォーム再アップロード推奨**)
- 静的(常時ON)リアクティブの優先度がFXより下だったのは1.14.0でバグ修正 → 既存ギミックの挙動が変わる可能性が明記されている
- Shape ChangerのSet/Deleteの上書き規則は1.13で変わり1.14で再逆転(1.13.xの挙動は事故扱い)
- Int自動パラメータ値が0-255を超えるとビルドエラー(1.18+。以前は黙って範囲外を割り当て)
- Merge Armature中のヒューマノイドボーン+PhysBoneは「子ヒューマノイドボーンが全PhysBoneから除外されている場合」のみ統合可(1.12+)
- MAのGCはアニメーションから参照されるオブジェクトを削除しない(1.15+)。それ以前は削除されることがあった

## Quest対応時の注意

- MA自体はQuestビルドでも同様に動作する(プラットフォーム非依存の変換)
- **MA Platform Filter**でQuest専用/PC専用オブジェクトを出し分け可能
- **MA Sync Parameter Sequence**はPC/Quest間のパラメータ不一致(同期ずれ)対策の中核。1.16+でプラットフォーム間の内容一致を強制
- World Fixed Objectは1.12+でVRCParentConstraint実装になりAndroidビルド可
- メニューアイコン自動圧縮はiOSビルド対応(1.12.0で修正)
- VQTとの関係: VQTはMA処理後のアバターを変換する。`MA Visible Head Accessory`/`MA World Fixed Object`はMA 1.9+ならVQTに削除されない([VRCQuestTools](11-vrcquesttools.md)参照)

## 関連ファイルパス

| パス | 役割 |
|---|---|
| `package.json` | リポジトリルート=パッケージ本体(1.18系: vpmDependencies ndmf >=1.11) |
| `Editor/PluginDefinition/PluginDefinition.cs` | NDMFプラグイン定義(全パス順序の一次情報) |
| `Editor/`(各Processor/Hook) | Merge Armature等の実装(`MergeArmatureHook`, `MenuInstallHook`, `RenameParametersHook`等) |
| `Runtime/` | コンポーネント定義(`ModularAvatarMergeArmature`等、`AvatarTagComponent`派生) |
| `docs~/` | 公式ドキュメント(Docusaurus)ソース |
| `CHANGELOG.md` | 1.12.0以降の変更履歴(それ以前はGitHub Releases) |

## よくあるトラブル

- **服が飛ぶ/ボーンが吹き飛ぶ**: Merge Armatureのボーン対応失敗。プレフィックス設定、素体と衣装のボーン名、スケール(Scale Adjusterの併用)を確認
- **メニューが出ない**: Menu Installerのインストール先、パラメータ未定義(MA Parametersの自動リネームとの絡み)。パラメータがMA Parameters定義済みでもメニュー値が0になる問題は1.15.1で修正
- **MMDワールドで表情停止**: 1.12+のMMD処理を理解する(`MA VRChat Settings`で無効化可)。WD OFFステートのあるレイヤーへのMMD Layer Controlは警告(1.14+)。FXレイヤー1枚目が無効化される問題は1.18系で修正
- **Play Audio(Animator Play Audio)のパスずれ**: 相対/絶対パスの解決規則が1.12.5で確定(絶対パス設定+相対Merge時、対象不在なら絶対として扱う)
- **1.16.0でBone Proxy + Merge Armatureが壊れた**: 1.16.1で修正済み
- **Visible Head Accessoryでメッシュ破損**: 1.17系(#2019)で修正。頭部直下root boneのメッシュはHead Chop絡みの既知問題が多い(1.13.3/1.13.4)
- **ビルドは成功するがトグルが他人から見えない**: パラメータsynced設定とExpression Parameters容量超過を確認

## 関連ツール

- [NDMF](01-ndmf.md): 実行基盤(同一作者、同時更新推奨)
- [TexTransTool](04-textranstool.md): MAの後に実行。MA Material Setter/Swapで差し替えたマテリアルへもTTTが効く(TTT 0.10+)
- [AAO](09-avatar-optimizer.md): MAの生成物(DelayDisableレイヤー等)を認識して最適化
- [VRCQuestTools](11-vrcquesttools.md): MA処理後にQuest変換。Setup Avatar for MobileはMA Sync Parameter Sequence等を自動追加

## バージョン履歴

(CHANGELOG.mdは1.12.0以降。それ以前はGitHub Releases参照。日付はリリース日)

### 1.18(Unreleased→2026年後半見込み)
- 追加: Blendshape Syncのカーブリマップ、Vertex Filter By UV Tile、By Maskの代替UV、Sync Parameter Sequenceの誤設定警告、**Int自動値の0-255超過をビルドエラー化**
- 修正: Unity 6000.0互換、MMDワールドでFX第1レイヤー無効化、Merge Armatureのメモリデフラグ後の追跡喪失

### 1.17.x (2026-05)
- 1.17.0: **MA Floor Adjuster追加**、VRCSDKのメニューエディタハングをパッチ、Bone ProxyのMatch scale、`VRCRaycast`対応(VRCSDK 3.10.3+)
- 1.17.1: **Floor AdjusterをTTT等の後に実行するよう順序変更**(late-transform-stagesプラグイン新設。互換性重要)

### 1.16.x (2026-02)
- 1.16.0: Bone Proxyのターゲット拡張、**Sync Parameter SequenceがUnity Library保存+プライマリ→セカンダリ同期に変更**(パラメータアセット不要化)、TextMeshProポストプロセッサ抑止(ビルド高速化)、MMDワールド関連修正多数
- 1.16.1: **Merge Animator「Match Avatar Write Defaults」既定ON化**。Bone Proxy+Merge Armatureリグレッション修正
- 1.16.2: 非定数カーブ検査の誤検知修正

### 1.15.x (2025-12)
- 1.15.0: **MA Fit Preview**、**MA Global Collider**、`com.vrchat.avatars`依存除去、VRCFury<1.1250.0+Mesh Cutter/Shape Changer(delete)の非互換警告、GCがアニメ参照オブジェクトを保持するよう変更
- 1.15.1: MA Parameters定義パラメータのメニュー値0問題、非ヒューマノイドでのScale Adjusterエラー修正

### 1.14.x (2025-09)
- 1.14.0: **MA Mesh Cutter追加**(頂点フィルタBy Bone/Blendshape/Axis/Mask)、`GetBonesMapping` API公開。**静的リアクティブの優先度バグ修正(挙動変化の可能性明記)**、Shape ChangerのSet/Delete上書き規則を1.12挙動へ復帰、Head Chop過剰生成修正、リアクティブ初期状態を非VRChatプラットフォームでも適用
- 1.14.1〜1.14.3: genericアバターアップロード失敗、パラメータFloat化時のDriver挙動、Vertex Filter By Axisのプレビュー修正

### 1.13.x (2025-07〜08)
- 1.13.0: **非VRCプラットフォーム実験的対応**、**MA Material Swap**、**MA Platform Filter**、**MA Rename Collision Tags**、Shape Changerの完全削除化(アニメ中でも)、`ModularAvatarMenuItem`のVRCSDK非依存API(旧APIは2.0で削除予告)
- 1.13.1: Blendshape Syncのシーン常時更新/安全設定ブロック時の削除形状未適用修正。1.13.2: Shape Changerによる特定ワールドでのVRChatクラッシュ修正。1.13.3/1.13.4: 頭部配下root boneのfirst person不可視問題修正

### 1.12.x (2025-04)
- 1.12.0: CHANGELOG導入。**Merge Animatorの既存アニメーター置換**、World Scale Object、MA MMD Layer Control、**MMDワールド互換処理の導入**(Merge Blend Tree/リアクティブとの互換修正、MA VRChat Settingsで無効化可)、**Merge Blend Tree→Merge Motionへ改名**(クリップ統合対応)、新NDMF API(IVirtualizeMotion等)採用、**パラメータ自動リネームがパスベースに**(Sync Parameter Sequence利用者は再アップロード推奨)、World FixedのVRCParentConstraint化(Android対応)、アイコン圧縮のiOS対応
- 1.12.1〜1.12.5: 新規プロジェクトでのコンパイルエラー、Merge Motion+Rename Parameters、WD調整の互換修正、NDMF連動修正(重複レイヤー等)。**1.12.5: Play Audioの絶対/相対パス解決規則確定**

### 1.11以前(GitHub Releases。主要トピックのみ)
- 1.11 (2025-02): Merge Animator相対パスモードの内部改善、NDMF 1.6系対応
- 1.10 (2024-12): NDMF AnimatorServices移行準備、安定性改善
- 1.9 (2024-09): NDMFプレビュー対応強化(Shape Changer等)、VQTがVHA/WFOを削除しなくなる基準バージョン
- 1.8 (2024-06): **リアクティブコンポーネント(Object Toggle / Shape Changer / Material Setter)導入**
- 1.7以前 (2023〜2024-05): NDMF移行(1.6で完了)、Merge Armature/Menu/Parametersの基礎確立

### 推奨組み合わせ
- MA 1.17.x + NDMF 1.13〜1.14 + AAO 1.9.x + TTT 1.0.x(2026年前半の標準構成)
- MAとNDMFは同一作者のため**同時に更新**するのが原則。MAのみ更新してNDMFが古いとコンパイルエラーになりうる(vpmDependenciesで下限は保証されるがVCC以外の導入経路では要注意)
