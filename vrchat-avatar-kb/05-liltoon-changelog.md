# lilToon バージョン履歴

> 元ファイル: [05-liltoon.md](05-liltoon.md)。バージョン起因の不具合切り分け・破壊的変更の確認時のみ参照。

## バージョン履歴

(Assets/lilToon/CHANGELOG.mdより。主要バージョン)

### 2.3.x (2025-10〜2026-06)
- 2.3.0: **Adaptive Probe Volumes対応**。AudioLinkMask頂点シェーダー修正、アニメーションデカールのベイク修正(VQT 2.11.4が要求)、Unity 6警告修正
- 2.3.2: Unity 6000.0エラー修正
- 2.3.3/2.3.4: プリセットHDR色保存、テクスチャコピー、カスタムシェーダー改行、空lilToonSetting.json対応などの安定化。2.3.4でGIFパーサー内蔵

### 2.2.x / 2.1.x (2025-07〜08)
- 2.2.0: `_UdonForceSceneLighting`(ワールド側から明るさ調整無効化)。HDRPコンパイルエラー修正
- 2.1.0: **VRC Light Volumesのピクセル単位計算化+リムライト調整プロパティ**、**複数アバタービルド時のシェーダーコンパイル統合API**
- 2.1.6: VRCLV 1.0.0互換

### 2.0.0 (2025-07) — メジャー
- **VRC Light Volumes 2.0.0を同梱**(方向対応)
- **破壊的**: ファーのShrinkモード削除(Subdivisionに統一)、**メッシュ暗号化(Avatar Encryption)機能の削除**
- asmdefのAuto Referenced無効化(コンパイル高速化)

### 1.10.x (2025-05) — 1.x系最終安定
- 1.10.0: **VRC Light Volumes対応**、Toon Standard(Outline)フォールバック追加、Ramp焼き込み、簡易設定UI削除
- 1.10.1: VRCLVパッケージなしでも使用可能に

### 1.9.0 (2025-04)
- Shadow SDFの拡張(Bチャンネル=通常影ブレンド、A=影強度)。インスペクタ大幅高速化。**外部ツールからのマテリアル最適化API呼び出しでのFakeShadowエラー修正**(AAO 1.8.9が対応)

### 1.8.x (2024-10〜12)
- 1.8.0: **外部ツール向け: プロパティアニメーションを考慮した最適化**、SDF顔影、LTCGI、ファーリムライト
- 1.8.2: World SDK併用ビルド失敗修正
- **1.8.4: NDMFのApply on Playでシェーダー最適化をスキップ可能に**

### 1.7.x (2024-01〜04)
- 1.7.0: **UV Tile Discard**追加。1.7.3: ビルド時keyword設定、最適化がIDMaskPriorを壊す問題修正

### 1.5〜1.6 (2023-12)
- 1.6.0: **RimShade**追加、シェーダーキーワード非互換問題の回避
- 1.5.0: `LILTOON_DISABLE_OPTIMIZATION`、**IPreprocessShaders最適化の削除**。1.5.2: AssetBundleビルドで頂点データ消失修正(重要バグ修正)

### 1.4以前 (2021〜2023)
- 1.4.0 (2023-05): 頂点IDマスク、Dither、VRCSDK検知のVersion Defines化
- 1.3.7 (2023-01): **VPM対応**
- 1.3.0 (2022-06): 大規模機能追加、Unity 2017廃止
- 1.0 (2021-07): 初版

### 推奨組み合わせ
- 新規: lilToon 2.3.x + VQT 2.11.x + AAO 1.9.x
- 旧環境維持: lilToon 1.10.3(1.x最終)— ただしVQT次期メジャーで<1.10切り捨て予定のため2.x移行を推奨
- アバター製品が同梱するlilToonバージョンより古いバージョンへ下げない(マテリアルマイグレーション済みデータは後方非互換)
