# AAO: Avatar Optimizer バージョン履歴

> 元ファイル: [09-avatar-optimizer.md](09-avatar-optimizer.md)。バージョン起因の不具合切り分け・破壊的変更の確認時のみ参照。

## バージョン履歴

(CHANGELOG.mdより。パッチは互換性・重要修正のみ)

### 1.10(Unreleased)
- テクスチャ使用量推定をVRCSDK依存から自前実装へ変更予定(RenderTexture/CubeMap計算の正確化)

### 1.9.x (2026-02〜)
- **1.9.0 (2026-02)**: **NDMFパイプラインの最後尾で実行するよう変更(プラグインクラス名の非ASCII化=内部API変更)**、非VRCSDKプラットフォーム互換宣言、**Merge Material追加(Merge ToonLit Material非推奨化・次期メジャー削除予告)**、Max Texture Size追加、EndBone→Endpoint自動置換、PhysBone自動統合、Complete Graph→Entry/Exit最適化、未使用テクスチャ削除、Optimize Textureの非一様使用対応、基本メッシュのRemove Mesh対応、最適化メトリクス、Bug Report Helper、IEditorOnly無視のProject Settings
- 互換性系パッチ: 1.9.5(Android ASTC on Windows)、1.9.9/1.9.10(VRCSDK 3.10.3 VRCRaycast)、1.9.13(**VRCSDK 3.7.0互換回復**=1.9.11/12は実質3.8.0+要求、EyeLook両目保持)、1.9.14(VRCSDK 3.10.4 PB Global Collider)、1.9.15(AutoMergeSMRのレイヤー選択、MergeBoneのZ-fighting)
- PhysBone修正群: 1.9.3/1.9.6/1.9.7/1.9.8/1.9.11/1.9.12/1.9.13(併用アバターでPB異常が出たらまずここ)

### 1.8.x (2024-11〜2025-12)
- **1.8.0 (2024-11)**: **Unity 2019廃止**。**Optimize Texture導入(lilToonのみ)**、AnyState→Entry/Exit、Rename BlendShape、Remove Mesh By UV Tile、未使用マテリアルプロパティ自動削除、BlendShape自動統合・BlendShape付きメッシュの自動統合、Merge SkinnedMeshのBlendShape対応(改名モード)、NDMFプレビューへ移行、**-nographics非対応化**、zh-cn→zh-hansロケール変更(NDMFツール互換)、TTTとのRemoveMeshByMask互換改善
- 1.8.1: lilToonアウトラインマスク破損修正。1.8.5: lilToon AngelRing修正。1.8.9: **lilToon 1.9対応**。1.8.11: **VRCSDK 3.8.1対応(VRCPerPlatformOverrides / Toon Standard(Outline)のOptimize Texture対応)、NDMF 1.8のNDMFAvatarRoot対応、VRCFuryTest対応**。1.8.14: VRCSDK 3.9互換宣言。1.8.15: **VRCSDK 3.10対応**。1.8.16: streaming mipmap設定コピー

### 1.7.x (2024-04〜10)
- **1.7.0 (2024-04)**: **Optimize Animator導入**(Entry/Exit→BlendTree等)、**Asset Description導入**、Automatic Merge Skinned Mesh、Remove Mesh By Mask、コンポーネントAPI公開、CL4EE依存廃止(NDMFローカライズへ)、**最小VRCSDK 3.3.0**、**VRCQuestTools v1互換の削除(VQT 2.x必須)**、ビルド後パス対象アニメの無効化(挙動変更)
- 1.7.10/1.7.11: **VRCConstraints対応(VRCSDK 3.7.0)**。1.7.13: 2019系最終安定

### 1.6.x (2023-11〜2024-04)
- 1.6.0: コンポーネント登録の公開API、メッシュON/OFF連動のPhysBone自動無効化、Remove Zero Sized Polygon、UniVRM対応、FloorAdjuster等Transform移動系との互換修正
- 1.6.5: エラーレポートをNDMF APIへ移行、表示名「AAO: Avatar Optimizer」に。1.6.9: NDMF 1.4.0でContextHolderが未知コンポーネント化する問題修正

### 1.5.x (2023-10〜11)
- **1.5.0: NDMF統合**(独自ApplyOnPlay廃止)、新GCアルゴリズム(Remove Unused Objectsがコンポーネント/ボーンも削除、UnusedBonesByReferenceTool非推奨化)、Unity 2022クラッシュ回避
- 1.5.7: **VRCQuestTools互換追加**

### 1.4以前 (2023)
- 1.4.0: Advanced Animator Parser(レイヤー解析でBlendShape凍結精度向上)、マルチフレームBlendShape対応
- 1.3.0: 「Automatic Configuration」→「Trace And Optimize」改名、コンポーネントに`AAO`プレフィックス
- 1.1.0: MergeToonLitのテクスチャ形式変更(ARGB32→ASTC/DXT5、軽微な破壊的変更)
- 1.0.0 (2023-06): v0.4からの移行(0.3以前のデータは読めない)

### 推奨組み合わせ
- AAO 1.9.x + NDMF 1.13〜1.14 + VRCSDK 3.8〜3.10(3.7.0は1.9.13+で可) + TTT 1.0.x + MA 1.17.x
- VRCSDKを更新したらAAOのパッチも必ず確認(新コンポーネント対応はAAO側更新が必要)
