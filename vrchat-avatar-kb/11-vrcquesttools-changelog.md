# VRCQuestTools バージョン履歴

> 元ファイル: [11-vrcquesttools.md](11-vrcquesttools.md)。バージョン起因の不具合切り分け・破壊的変更の確認時のみ参照。

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
