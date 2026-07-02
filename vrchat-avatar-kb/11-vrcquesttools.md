# VRCQuestTools (VQT)

- リポジトリ: https://github.com/kurotu/VRCQuestTools
- ドキュメント: https://kurotu.github.io/VRCQuestTools/
- パッケージ名: `com.github.kurotu.vrc-quest-tools`
- 作者: kurotu

## 概要

PC向けアバターを**Quest/Android/iOS(モバイル)対応へ変換**するツール。マテリアルのモバイル対応シェーダーへの変換(テクスチャベイク付き)、非対応コンポーネント除去、Avatar Dynamics削減、プラットフォーム別出し分けを、破壊的(複製生成)にも**非破壊(NDMFビルド時)**にも実行できる。

- Unity要件: 2022.3(次期メジャーでUnity 2019サポート削除がUnreleasedに記載。2.x現行は2019コードも残存)
- VRCSDK要件: 2.0.0で3.3.0+。次期メジャーで**3.9.0未満を切り捨て予定**
- リポジトリ構造: Unityプロジェクト形式。パッケージ本体は`Packages/com.github.kurotu.vrc-quest-tools/`

## 依存関係

- 必須依存なし(VRCSDKは前提)
- オプション: **NDMF**(非破壊変換・`[NDMF]`表記の機能全般に必須。次期メジャーでNDMF <1.5切り捨て)、Modular Avatar(Convert Constraints連携、Setup Avatar for Mobile)、lilToon/Poiyomi(変換元シェーダー)、FinalIK/VirtualLens2(専用対応)
- AAOとの互換: AAO 1.5.7+/VQT 2.x(AAO 1.7.0でVQT 1.x互換削除)

## 対応する改変パターン

### マテリアル/テクスチャ変換
- **lilToon → Toon Lit / MatCap Lit / Toon Standard(VQT 2.10+、要VRCSDK 3.8.1+)**: 色・影・Emission・MatCap・AO・デカール等をテクスチャへベイク。lilToonカスタムシェーダー派生も対象(2.7+)。lilToon 2.0対応は2.11.1+
- **Poiyomi → Toon Lit**(メインカラー/ノーマル陰影/Emission 0-3)。→Toon Standardは実験的(開発版)
- Material Replacement(任意マテリアルへの手動差し替え)、マテリアル個別の変換設定
- アニメーション/AnimatorController/BlendTree/AnimatorOverrideControllerの変換追従(マテリアル差し替えアニメも変換)
- テクスチャ圧縮形式・最大サイズ制御、メニューアイコンのリサイズ/圧縮(`VQT Menu Icon Resizer`)

### コンポーネント/構造
- 非対応コンポーネントの除去(Constraint類はVRCConstraintsへの変換誘導=MA Convert Constraints連携、2.10+)
- **VQT Platform Component Remover / Platform GameObject Remover**: ビルドプラットフォーム別の出し分け(NDMF必須)
- **VQT Platform Target Settings**: 対象プラットフォームの明示
- **VQT Network ID Assigner**: PC/Quest間のPhysBone同期用Network ID割り当て(2.5+はNDMFなしでも動作)
- **VQT Mesh Flipper**(2.6+): メッシュ反転/両面化(ポリゴン削減前/後のフェーズ選択可、2.9+)
- **VQT Avatar Converter Settings**: 変換設定をアバターに保存し、**NDMFビルド時に非破壊変換**(2.3+)。NDMFフェーズ選択(Transforming/Optimizing/Auto)。Autoは既定Optimizing、VRCFury存在時はTransforming(2.10+)
- **VQT Fallback Avatar**(開発版): アップロード後の自動フォールバック設定
- Avatar Dynamics(PhysBone等)の選択削減(パフォーマンスランク推定付き)、`Remove Avatar Dynamics`オプション(2.9+)

### 検証系
- テクスチャ形式チェック(Androidで未対応形式の警告。AAO後に実行)、Unity Settings for Mobile、バリデーション自動化(2.11+で設定可)

## 改変時の注意点(ソース由来の癖)

- **NDMFフェーズ配置**(`Editor/NDMF/VRCQuestToolsNdmfPlugin.cs`): Resolvingで`BeforePlugin` MA/VirtualLens2(プラットフォーム別除去はMAより前に済ませる)。変換パスはTransforming/Optimizing両方に存在し、コンポーネント設定でどちらで動くか決まる。いずれも`AfterPlugin` TTT/MA/lilycalInventory、Optimizing側は`BeforePlugin` AAO
- **変換フェーズの選択が互換性の要**: VRCFury等の非NDMFツール(Transforming後に実行)が生成するマテリアルを変換したい場合はOptimizing、逆にVRCFuryがVQT変換後の状態を前提とするならTransforming。2.10+のAutoが妥当な既定
- **非破壊変換はビルドターゲットがAndroid/iOSのときに発動**する(PC向けビルドでは変換パスはスキップ)。「[NDMF] Build and Test for PC with Android Settings」でPC上での見た目確認が可能
- 手動変換(破壊的)は複製を生成(出力先`Assets/VRCQuestToolsOutput`、2.0+)。元アバターは変更されない
- テクスチャベイクはlilToonのプロパティ実装に強く依存: lilToon側の更新でベイク不具合が出やすい(2.11.4はlilToon 2.3.0+が必要な修正を含む)。**lilToonとVQTはセットで更新**
- MA連携: `MA Visible Head Accessory`/`MA World Fixed Object`はMA 1.9+なら削除しない(2.4.0+)。未対応MAバージョン使用時はエラー(開発版)。`Setup Avatar for Mobile`はMA Sync Parameter Sequence(PrimaryPlatform=PC)等を自動構成
- 2.0.0でインポートパスが`Assets/KRT/VRCQuestTools`→`Packages/com.github.kurotu.vrc-quest-tools`に変更(1.x→2.x移行時は旧フォルダ削除)

## Quest対応時の注意(このツール自体がQuest対応ツール)

VRChatのモバイル制約の実装対象:
- シェーダー: VRChat/Mobile系+Toon Standard(2.9.2+でVRCSDKのAndroidホワイトリスト参照)
- テクスチャ: ASTC推奨(形式チェックパスが検査)、最大サイズ制御
- コンポーネント: Constraint(Unity製)、Cloth、Camera、Light、AudioSource等は非対応→除去/変換
- パフォーマンスランク: モバイルはVery Poorでアップロード不可(Fallback運用)。Avatar Dynamics上限(PhysBone数等)は専用削減UIで対応
- PC/Quest同一Blueprint IDでのアップロード時、**パラメータ順序の一致**(MA Sync Parameter Sequence)と**PhysBoneのNetwork ID一致**(VQT Network ID Assigner)が同期ずれ防止の要

## 関連ファイルパス

| パス | 役割 |
|---|---|
| `Packages/com.github.kurotu.vrc-quest-tools/package.json` | パッケージ定義 |
| `.../Editor/VRCQuestTools.cs` | 本体定義(バージョン定数等) |
| `.../Editor/NDMF/VRCQuestToolsNdmfPlugin.cs` | NDMFプラグイン定義(順序制約の一次情報) |
| `.../Runtime/Components/` | VQTコンポーネント群 |
| `Website/` | ドキュメントソース(Docusaurus) |
| `CHANGELOG.md` | 変更履歴 |

## よくあるトラブル

- **変換後の見た目が暗い/発光しない**: lilToon/PoiyomiのEmission系変換の既知修正(2.11.2/2.11.3/2.11.7)。UVタイリング未保持は2.11.6修正。→VQT最新パッチへ
- **lilToon 2.xで変換失敗**: VQT 2.11.1未満は非対応
- **テクスチャ形式警告が大量に出る**: Windowsビルドターゲットでのチェックは2.11+で既定OFF。Android時の警告は実際に直す(ASTC化)
- **VRCFuryのギミックが変換されない**: 変換フェーズをOptimizing(またはAuto)にする
- **PC/Quest間で同期が壊れる**: Network ID未割り当て(VQT Network ID Assigner)+パラメータ順序(MA Sync Parameter Sequence)を両方確認
- **GoGo Loco入りFXの変換エラー**: 2.5.3で修正(サブステートマシン対応)
- **prefabステージで変換できない**: 仕様(2.4.1でエラーメッセージ化)

## 関連ツール

- [Modular Avatar](02-modular-avatar.md): Convert Constraints / Sync Parameter Sequence / Setup Avatar for Mobileで密連携
- [AAO](09-avatar-optimizer.md): VQT変換→AAO最適化→VQT形式チェックの順で協調
- [TexTransTool](04-textranstool.md): TTT出力を変換対象に含めるため常にTTT後
- [lilToon](05-liltoon.md) / [Poiyomi](06-poiyomi.md): 変換元シェーダー

## バージョン履歴

(CHANGELOG.mdより。主要バージョン)

### 3.0系(Unreleased)
- **削除予定(破壊的)**: Unity 2019、VRCSDK <3.9.0、lilToon <1.10.0、NDMF <1.5.0、VQT Avatar Builderウィンドウ(VRCSDKコントロールパネル直アップロードへ)
- 追加予定: Toon Standard変換の拡充(shadow ramp生成、機能選択、マスク/MatCap解像度制御)、Poiyomi→Toon Standard(実験的)、パーティクル/Trail/Line系の変換、`VQT Fallback Avatar`、NDMFプレビュー(マテリアル変換・頂点カラー除去)、ソーステクスチャのプラットフォーム別オーバーライド継承
- 変更予定: 既定変換先をToon Standardへ、`Convert Avatar for Mobile`→`Setup Avatar for Mobile`改名(MA/AAO考慮のセットアップ)、Network ID自動付与の廃止(明示オプション化)

### 2.11.x (2025-06〜2026-06)
- 2.11.0: 設定メニュー(Validation Automator、Windows時の形式チェック既定OFF)
- **2.11.1: lilToon 2.0対応**。2.11.4: lilToon Main 2nd/3rdのUV0以外/アニメーションデカールのベイク修正(**要lilToon 2.3.0+**)。2.11.5〜2.11.7: NDMF共有黒テクスチャ、UVタイリング保持、Poiyomi→ToonLitのEmission修正

### 2.10.x (2025-05)
- **2.10.0: Toon Standard変換導入(要VRCSDK 3.8.1+、lilToon 1.10+のみ)**、MA Convert Constraints統合、**NDMFフェーズAuto追加(既定。VRCFury検知でTransforming)**、Overall NDMF Mesh Simplifierを順序制約に追加、Material Conversion Settings等の実験的status解除

### 2.9.x (2025-04〜05)
- 2.9.0: Android Build Supportチェック、`Remove Avatar Dynamics`、`VQT Material Conversion Settings`、Mesh Flipperのフェーズ選択+プレビュー。2.9.2: **VRCSDKのAndroidシェーダーホワイトリスト参照(Toon Standard許可)**

### 2.7〜2.8 (2025-02〜04)
- 2.7.0: **VQT Material Swap**(実験的)、**VQT Menu Icon Resizer**、lilToonカスタムシェーダー変換、Mesh Flipperのマスク制御、MantisLODより前に実行する制約追加
- 2.8.0: テクスチャ生成の高速化+キャッシュ、生成テクスチャのRead/Write無効化。2.8.3: 著作権同意確認(VRCSDK 3.8.1系アップロード対応)

### 2.5〜2.6 (2024-09〜2025-01)
- 2.5.0: **NDMF変換フェーズ選択(Transforming/Optimizing)導入、既定をTransformingへ**(2.4.3の変更を差し戻し)、.poベースのローカライズ
- 2.6.0: **VQT Mesh Flipper**(実験的)、`[NDMF] Manual Bake with Android Settings`、**テクスチャ形式チェックをTTT後に**、**lilycalInventory後に変換するよう変更**、MA Convert Constraints提案ダイアログ

### 2.3〜2.4 (2024-05〜08)
- **2.3.0: NDMFによる非破壊変換導入**(`VQT Avatar Converter Settings`)、Platform Target Settings、Network ID Assigner
- 2.4.0: **iOS対応**、MA VHA/WFOをMA 1.9+で削除しない変更、VRCSDK constraints系のコンパイル修正
- **2.4.3: 変換をOptimizingへ移動(TTT相互運用)** → 2.5.0で既定Transformingに再変更(履歴として重要: この間のバージョンは挙動が異なる)

### 2.0〜2.2 (2024-01〜04)
- **2.0.0 (2024-01)**: メジャー刷新。`VQT Avatar Converter Settings`コンポーネント化、MatCap Lit/Material Replacement変換、**Poiyomi→Toon Lit変換追加**、NDMF生成の非対応コンポーネント除去、**VRCSDK 3.3.0+必須**、パッケージ配置変更(`Packages/`へ)、出力先変更、**AAOより前に除去処理を行う順序変更**、FinalIK/VirtualLens2対応統合
- 2.1.0/2.2.0: Platform Component Remover / Platform GameObject Remover追加(NDMF必須)

### 1.x (〜2023-12)
- 1.14.0: Unity 2022対応。1.9.0 (2023-01): VPM対応、MA Merge AnimatorのAnimator変換対応。1.x系は破壊的変換のみ(NDMF機能なし)。**AAO 1.7.0以降とは非互換**

### 推奨組み合わせ
- VQT 2.11.x + lilToon 2.3.x + MA 1.17.x + AAO 1.9.x + NDMF 1.13〜1.14 + VRCSDK 3.8.1以上(Toon Standard変換を使う場合)
- Toon Lit変換のみなら VRCSDK 3.7系でも可(ただしAAO側の下限に注意)
