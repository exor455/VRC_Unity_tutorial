# TexTransTool (TTT)

- リポジトリ: https://github.com/ReinaS-64892/TexTransTool
- ドキュメント: https://ttt.rs64.net
- パッケージ名: `net.rs64.tex-trans-tool`(VPMリポジトリ: vpm.rs64.net)
- 作者: Reina_Sakiria

## 概要

**テクスチャの非破壊改変**に特化したツール。デカール貼り付け、レイヤー合成(PSD対応)、マテリアル改変、テクスチャアトラス化を、元アセットを変更せずビルド時(NDMF)に適用する。CHANGELOGは日本語で管理されている。

- Unity要件: 2022.3(v0.5.0で最小2022.3に変更)
- VRCSDK: 直接依存なし(VRChat以外でも動く)。VRChat Avatar SDK存在時にNDMF不在だと警告
- 実行バックエンド: Unity標準+ComputeShader(v0.9+)。オプションでTTCE-Wgpu(GPUバックエンド)

## 依存関係

- 必須: NDMF(v1.0系は`>=1.7.0`)、**TexTransCore**(`net.rs64.tex-trans-core` — v0.10.0でコア部分が別パッケージに分離)、`com.unity.burst`、`com.unity.mathematics`
- オプション: AAO(存在時に`CONTAINS_AAO`デファインで連携パスが有効化)、TTCE-Wgpu

## 対応する改変パターン

### デカール(テクスチャへの貼り付け)
- **SimpleDecal**: 平面投影デカール(複数レンダラー対応、Island Selector連携、Auto選択モード(v0.9+)、GrabDecal(実験的))
- **SingleGradationDecal**: グラデーションデカール(v0.9でStable化)
- **CylindricalCurveDecal 等**: 円筒/カーブ系デカール(実験的)

### レイヤー合成・テクスチャ編集
- **MultiLayerImageCanvas**: 画像編集ソフト的なレイヤー合成(RasterLayer / LayerFolder / SolidLayer)
- **TTT PSD Importer**: PSDをレイヤー構造ごとインポートしてCanvasとして利用
- **TextureBlender**: 既存テクスチャへの合成(v0.9でStable化)。合成モードは通常/比較(明・暗)/PinLight/HardMix/AdditionGlow/Dissolve等、画像編集ソフト準拠多数(v0.6で大幅追加・計算式修正)
- **MaterialModifier / MaterialOverrideTransfer**: マテリアル設定の改変(MaterialModificationフェーズ、v0.9+)

### アトラス化・最適化
- **AtlasTexture**: 複数マテリアルのテクスチャを1枚にアトラス化(UV再配置)。lilToon専用サポート(プロパティベイク: `_MainTexHSVG`、MatCap、Emission等)、Island統合(v0.10+)、TextureFineTuning(Resize / Compress / MipMap / ReferenceCopy / Remove / ColorSpace)、SizePriority、複数マテリアル→1マテリアル統合(MergeReference / AllMaterialMergeReference)
- **UVtoIsland / IslandSelector**(Box/Sphere/Pin/AND/OR/NOT/XOR): 処理対象アイランドの選択

### その他
- テクスチャ圧縮の一括制御(プラットフォーム別フォーマット、v1.0.1でiOS対応修正)
- NDMFプレビュー: フェーズ単位でON/OFF可能なリアルタイムプレビュー(v0.8+)

## 改変時の注意点(ソース由来の癖)

- **TTT独自の6フェーズ**(`Editor/NDMF/NDMFPlugin.cs`、NDMF Transforming内で順に実行): `MaterialModification → BeforeUVModification → UVModification → AfterUVModification → PostProcessing → UnDefined`。AtlasTexture等の重い最適化はNDMF **Optimizing**フェーズの`OptimizingPass`で実行される
- コンポーネントの実行順は、`PhaseDefine`配下でなければ**ヒエラルキーの上から順**(v0.10でTexTransGroup削除に伴い確定した仕様)
- **コンポーネントを入れ子にすると入れ子側は動作しない**(v0.9+)
- 順序制約(ソース): Transformingでは Light Limit Changer / FloorAdjuster / MantisLOD より前。Optimizingでは AAO / MantisLOD / Meshia / lilNDMFMeshSimplifier / lilycalInventory より前
- **AAO連携(NegotiateAAOPass、AAO存在時のみ)**: AAOのRemove MeshBy*で消える領域をアトラス化から除外し、UVをAAOに報告して整合を取る(TTT 0.9+/AAO 1.8+のAPI連携)
- **MA Material SetterやAnimationで追加されるマテリアル**にもほぼ全コンポーネントが効く(v0.10+、NDMF≥1.7必須)。旧版では差し替え後マテリアルに改変が乗らない
- プレビュー状態のままアップロードされる事故は`PreviewCancelerPass`(Resolvingフェーズ)で防止される(v0.5.0+)
- 生成テクスチャはisReadable無効+StreamingMipmap有効で出力(v0.5.0+)。TTT生成テクスチャを他ツールが置換した場合、未圧縮ならTTTが圧縮を代行する(v0.8.7+)
- 黒に近い階調がLinear一時保存で失われる問題はv0.10.4で修正(それ以前のバージョンでは暗部の色調変化に注意)
- 非正方形テクスチャのPadding計算はv0.10.0で修正
- v1.0.0でv0.8.x以前のセーブデータを完全削除。**古いprefab/シーンはv0.9〜v0.10経由でマイグレーションしてから**v1.0へ

## Quest対応時の注意

- TTT自体はAndroid/iOSビルドで動作する(v0.8.0でAndroidコンパイルエラー修正、v1.0.1でiOSのフォーマット自動選択修正)
- TextureFineTuningのCompressはビルドターゲットに応じたフォーマット(ASTC等)を選ぶ。未知のターゲットで例外が出た問題はv0.10.5で修正
- Quest向けはVRAM削減が特に重要なので、AtlasTexture+Resize+Compressの組み合わせが定石。VQTと併用する場合、**VQTのテクスチャ変換・形式チェックはTTTの後に走る**ため、TTTの出力がそのまま変換対象になる
- MaxTextureSize系の他ツール(AAO)との重複設定に注意(どちらか一方で管理する)

## 関連ファイルパス

| パス | 役割 |
|---|---|
| `package.json` | パッケージ定義(TexTransCore依存) |
| `Editor/NDMF/NDMFPlugin.cs` | NDMFプラグイン定義(フェーズ構成・順序制約の一次情報) |
| `Editor/NDMF/`(AAO連携: `NegotiateAAOPass`等) | AAO API連携(`CONTAINS_AAO`時) |
| `Runtime/`(TexTransBehavior系) | 各コンポーネント定義 |
| `CHANGELOG.md` | 日本語の変更履歴(最重要資料) |

## よくあるトラブル

- **デカール/アトラスの位置ずれ**: Transform移動系ツール(FloorAdjuster等)との順序。TTT 0.8.2+/MA 1.17.1+では対策済み。それ以外の移動系ツールとの併用時は要検証
- **アトラス化後にマテリアルが差し替わらない/戻る**: マテリアル差し替え(アニメ/MA)との順序問題→TTT 0.10+とNDMF 1.7+へ更新
- **AtlasTexture+AAO Remove Meshで例外**: v0.10.9で修正
- **lilycalInventoryより後に動いてエラー**: v0.10.6で修正(BeforePlugin追加)
- **lilToon Lite/宝石シェーダーで例外**: 誤ってlilToon扱いする問題はv0.8.0/v0.8.3で修正
- **GTX9/10系(DX11)で動作しない**: v0.10.0で修正
- **プレビューだけ表示が変**: ShaderKeywordsコピー漏れ(v0.9.3修正)、フェーズ単位プレビューのON/OFF確認
- **メモリリーク**: MeshData解放漏れ(v0.10.0修正)、TextureBlender(v1.0.2修正)。長時間エディタでVRAM/RAMが膨らむ場合は最新パッチへ

## 関連ツール

- [AAO](06-avatar-optimizer.md): 双方向連携(UV/RemoveMesh領域のネゴシエーション)。実行はTTTが先
- [Modular Avatar](02-modular-avatar.md): MAの後に実行。MA由来のマテリアル差し替えを認識
- [lilToon](04-liltoon.md): AtlasTextureのプロパティベイク対応シェーダー
- [VRCQuestTools](07-vrcquesttools.md): TTTの生成テクスチャを変換対象にするため常にTTT後

## バージョン履歴

(CHANGELOG.md準拠。日本語原文。主要バージョンのみ)

### v1.0.x (2025-06〜2026-06)
- **v1.0.0 (2025-06)**: 安定版化。マイグレーション未実施コンポーネントへの警告追加。**v0.8.x以前のセーブデータ削除(破壊的)** — 旧データは旧バージョン経由で移行
- v1.0.1 (2025-12): iOSビルドターゲットでのフォーマット選択修正
- v1.0.2 (2026-06): TextureBlenderのメモリリーク修正

### v0.10.x (2025-05〜06)
- **v0.10.0**: **TexTransCore分離(依存追加)**、NDMF≥1.7.0へ、**Animation/MA MaterialSetterで追加されたマテリアルへの対応**、AtlasTextureの大幅改修(MergeMaterial→AllMaterialMergeReference、BakeProperty削除、MipMap刷新、Island統合)、**TexTransGroup削除**(ヒエラルキー順実行に一本化)、Relative選択モード削除、HighQualityPadding削除、DX11旧GPU修正
- v0.10.4: 暗部階調がLinear保存で失われる問題修正、メッシュ軽量化系より後に動く問題修正
- v0.10.6: lilycalInventoryとの順序修正。v0.10.9: AAO RemoveMeshBy*併用時の例外修正

### v0.9.x (2025-02〜03)
- **v0.9.0**: VRAM使用量削減、**AAO API連携**(RemoveMeshBy*領域の除外+UV退避報告)、デカールのComputeShader化(高品質パディング復活)、SimpleDecalのAuto選択モード、MaterialModification/PostProcessingフェーズ追加、多数の実験的機能をStable化(SingleGradationDecal、IslandSelector系、TextureBlender)、MultiRendererMode削除(標準で複数対応)

### v0.8.x (2024-09〜2025-01)
- **v0.8.0**: **NDMF-Preview対応**(NDMF 1.5+で従来プレビューがフェーズ単位トグルに)、NDMF対応バージョン>=1.5.0へ、AtlasTextureの多数改善(MipMapのアルファ考慮生成、BackGroundColor、再配置効率改善)
- v0.8.2: **FloorAdjusterより先に動くよう修正**(デカールずれ対策)。v0.8.7: 他ツールがTTT生成テクスチャを置換した場合の圧縮代行。v0.8.8: NDMF 1.6のAssetSaver対応。v0.8.10/v0.8.13: AAO連携のバックポートと修正。v0.8.11: MantisLODより先に動くよう修正

### v0.7.x (2024-05〜07)
- **v0.7.0**: **OptimizingフェーズがNDMF Optimizeフェーズで実行されるように**、NDMF ObjectRegistryによる置換追跡、AtlasTextureのTiling逆補正、SizeOffset→SizePriority(破壊的)、VRCSDKありNDMFなし環境への警告

### v0.6.x (2024-03〜04)
- v0.6.0: 合成モード大量追加と計算式修正(Hue/Saturation等が画像編集ソフト準拠に)、AtlasTextureの再配置改善(**Padding値の意味がテクスチャスケール→UVスケールに破壊的変更**)、TextureFineTuningの保存形式変更(破壊的)

### v0.5.x (2024-01〜02)
- **v0.5.0**: **MultiLayerImageCanvas / PSDインポータ追加**、`PreviewCancelerPass`(プレビュー状態アップロード防止)、**最小Unityを2022.3に**、VRCAvatarCallBackToProcessAvatar削除(NDMF一本化)、生成テクスチャのisReadable無効化+StreamingMipmap有効化

### v0.4.x (2023-10〜2024-01)
- **v0.4.0: NDMF対応**。Phase/PhaseDefinition導入、MaterialModifier追加、lilToonの宝石/ファーテクスチャをアトラス化対象に追加

### 推奨組み合わせ
- TTT 1.0.x + NDMF 1.13〜1.14 + AAO 1.9.x(AAO連携が最も安定する構成)
- v0.9以前とv1.0系の混在プロジェクト移行は必ず v0.9/0.10 を経由
