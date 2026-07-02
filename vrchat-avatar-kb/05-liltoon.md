# lilToon

- リポジトリ: https://github.com/lilxyzw/lilToon
- ドキュメント: https://lilxyzw.github.io/lilToon/
- パッケージ名: `jp.lilxyzw.liltoon`(VPMリポジトリ: lilxyzw.github.io/vpm-repos)
- 作者: lilxyzw

## 概要

日本圏のVRChatアバターで事実上標準の**多機能トゥーンシェーダー**。Built-in RP / URP / HDRP対応、VRChat以外(ChilloutVR等)でも使用可。アバター改変では「衣装・素体のマテリアル調整」「シェーダー設定の最適化」の両面で関わる。

- Unity要件: リポジトリ上のパッケージは`unity: 2022.3`(2.x系)。1.x系はUnity 2019対応があった
- VRCSDK: 不要(存在すればVRChat固有機能=フォールバック設定・ビルド時最適化が有効化)
- リポジトリ構造: **リポジトリはUnityプロジェクト形式で、パッケージ本体は`Assets/lilToon/`配下**(package.json / CHANGELOG.mdもそこにある)

## 依存関係

- 必須依存なし(単体で動作)
- オプション連携: VRCSDK(フォールバック、ビルド最適化)、NDMF(存在時にApply on Play最適化スキップ設定、lilToon 1.8.4+)、AudioLink、LTCGI、VRC Light Volumes(2.x系は同梱)
- 派生: lilToonLite(軽量版)、lilToonMulti(マルチ機能版)、宝石/ファー/FakeShadow等のバリアント。lilycalInventory等の同作者ツールとは独立

## 対応する改変パターン

シェーダーなので「改変=マテリアル設定+ビルド時最適化」:

- **色調整**: メインカラーHSVG補正、色補正マスク、階調(ノーマル/影/ハイライト)
- **レイヤー**: メインカラー2nd/3rd(デカール、UVモード、Dissolve、距離フェード、アニメーション対応)
- **影**: 影色1st/2nd/3rd、AOマスク、SDF顔影(1.8+)、Shadow SDFのブレンド(1.9+)
- **発光**: Emission 1st/2nd(UVモード、AudioLink、グラデーション)
- **質感**: MatCap 1st/2nd、反射、異方性反射、ラメ(Glitter)、ファー、屈折、宝石
- **輪郭**: アウトライン(マスク、距離幅補正、法線調整マップ)、RimLight、RimShade(1.6+)、バックライト
- **形状系**: UV Tile Discard(1.7+)、IDマスク(頂点ID、1.4+/ビットマップ対応1.5+)、Dither、テッセレーション
- **VRChat固有**: シェーダーフォールバック設定(カスタム可)、**ビルド時マテリアル最適化**(未使用プロパティの定数化・variant削減)、Toon Standardフォールバック(1.10+)
- **ワールド光対応**: VRC Light Volumes(1.10+、2.0で方向対応/同梱)、Adaptive Probe Volumes(2.3+)、LTCGI(1.8+)
- **ユーティリティ**: `Fix Lighting`(メッシュ・マテリアルの一括ライティング設定)、FBXからのセットアップ、プリセット、未使用プロパティ削除、未使用テクスチャ削除

## 改変時の注意点(実装上の癖)

- **ビルド時最適化がマテリアルを書き換える**: VRChatアバタービルド時、lilToonは未使用機能のプロパティを定数化しシェーダーバリアントを削減する。外部ツール(AAO等)が想定するプロパティが消える事故を防ぐため、1.8.0で「**プロパティアニメーションを考慮した最適化API**」が入った。アニメーションでマテリアルプロパティを動かす改変(調光ギミック等)は、lilToon 1.8.0+でないと最適化後に動かなくなることがある
- **`LILTOON_DISABLE_OPTIMIZATION`** シンボルで最適化を強制無効化できる(1.5+)。トラブル切り分けに有効
- NDMF環境ではApply on Play時のシェーダー最適化をスキップ可能(1.8.4+)。実機ビルドとPlay時の見た目差の一因になりうる
- **テストビルド(Build & Test)では最適化しないオプション**あり(1.3.1+)。「アップロードだけ壊れる」時はこの差を疑う
- 1.5.0で`IPreprocessShaders`による最適化を廃止(AssetBundleビルドで頂点データが消える事故対応の系譜。1.5.2でも修正)。現行はマテリアルベースの最適化
- マテリアルのマイグレーション: バージョン更新時に自動マイグレーションが走る(1.5.1で頻度低減、`Assets/lilToon/[Material] Run migration`で手動実行可)。**プリセットやHDR色の保存に関するバグは2.3.3/2.3.4で修正**されており、2.x系は最新パッチ推奨
- シェーダー設定(`lilToonSetting`)はプロジェクト全体で共有され、材質が使う機能に応じて自動スキャン・自動設定される(1.1.4+)。設定ロックも可能。**空のlilToonSetting.jsonでエラーになる問題は2.3.4で修正**
- カスタムシェーダー(lilToonベースの派生)は改行コード・TEXCOORD重複などの互換問題が起きやすい(2.3.x系で複数修正)。派生シェーダー使用アバターの改変時はlilToon本体のバージョンを合わせる

## Quest対応時の注意

- **lilToonはQuest(Android/iOS)のVRChatアバターでは使用不可**(VRChatのモバイル向けシェーダーホワイトリスト外)。Questビルドでは必ずVRChat/Mobile系またはToon Standardへの変換が必要
- [VRCQuestTools](11-vrcquesttools.md)がlilToon→Toon Lit / Toon Standard変換を最も充実してサポート(色・影・Emission・MatCap・AO・デカールのベイク)。**lilToon 2.0対応はVQT 2.11.1+**、VQT次期メジャーはlilToon <1.10を切り捨て予定
- lilToonの`VRCFallback`設定はPC側のフォールバック(セーフティ)用。Quest対応そのものではない点に注意。1.10+でToon Standard(Outline)フォールバックを設定可能
- ワールド側機能(LTCGI / VRC Light Volumes)はPC向け。Quest変換後には反映されない

## 関連ファイルパス

| パス | 役割 |
|---|---|
| `Assets/lilToon/package.json` | パッケージ定義(リポジトリはUnityプロジェクト形式) |
| `Assets/lilToon/CHANGELOG.md` | 変更履歴 |
| `Assets/lilToon/Editor/` | インスペクタ(`lilInspector`)、最適化、マイグレーション処理 |
| `Assets/lilToon/Shader/` | シェーダー本体(.lilblockベースの生成システム) |
| `ProjectSettings/lilToonSetting.json`(プロジェクト側) | シェーダー設定の保存(2.x) |

## よくあるトラブル

- **最適化後に見た目が変わる/ギミックが死ぬ**: プロパティ定数化が原因。lilToon 1.8+へ更新、または`LILTOON_DISABLE_OPTIMIZATION`で切り分け。Dissolveのnoise未割り当てで最適化前後の見た目が変わる問題は2.1.0で修正
- **アップロード時のみ暗い/明るい**: ライティング設定(上限/下限輝度、Monochrome lighting)。`Fix Lighting`で標準化。ワールド依存(Light Volumes/LTCGI)も確認
- **World SDKと同居でビルド失敗**: 1.8.2で修正
- **マテリアルが勝手に変わる**: 自動マイグレーション。バージョン固定するか、更新後に`Run migration`で明示的に揃える
- **AAOとの組み合わせ**: AAOのOptimize Texture/未使用プロパティ削除はlilToon対応だが、lilToon側のバージョン依存修正が多い(AAO 1.8.5: AngelRing、1.8.9: lilToon 1.9対応、1.9.4: AudioLinkマスク)。AAO・lilToon両方を最新パッチに
- **Unity 6000でエラー**: 2.3.2で修正

## 関連ツール

- [VRCQuestTools](11-vrcquesttools.md): Quest用マテリアル変換元として最重要
- [AAO](09-avatar-optimizer.md): テクスチャ最適化・未使用プロパティ削除のlilToon対応
- [TexTransTool](04-textranstool.md): AtlasTextureのlilToonプロパティベイク対応
- [Poiyomi](06-poiyomi.md): 相互移行(Poiyomi 9.3.64にlilToon→Poiyomi変換機能)

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
