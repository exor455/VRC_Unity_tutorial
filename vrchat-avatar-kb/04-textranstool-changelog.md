# TexTransTool バージョン履歴

> 元ファイル: [04-textranstool.md](04-textranstool.md)。バージョン起因の不具合切り分け・破壊的変更の確認時のみ参照。

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
