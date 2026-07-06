# NDMF バージョン履歴

> 元ファイル: [01-ndmf.md](01-ndmf.md)。バージョン起因の不具合切り分け・破壊的変更の確認時のみ参照。

## バージョン履歴

主要バージョンと、改変作業・他ツール互換に影響する変更(CHANGELOG.mdより。パッチは重要なもののみ):

### 2.x(未リリース)
- 1.6.1告知: 1.7.0以降、全インターフェースにsentinelデフォルトメソッドを追加。古いC#でビルドされた下流パッケージは非対応化

### 1.14.0 (2026-06)
- Unity 6000.x互換修正。アバタールートEditorOnly時の明示エラー。`VirtualAnimatorController.SetParameter`追加

### 1.13.x (2026-05)
- 1.13.0: `RenderGroup.WithData<T>(T)`非推奨化(equality規則の問題)。IRenderFilter多重登録修正
- 1.13.1: AnimatorOverrideControllerのプロキシアニメマッピングが他ツールの直接操作で無視される問題を修正

### 1.12.0 (2026-05)
- `VRCRaycast`のパラメータイントロスペクション対応(VRCSDK 3.10.3+の新コンポーネント)。`VirtualControllerContext.Controllers`のKeys/Values空返却バグ修正

### 1.11.0 (2026-02) / 1.10.x (2025-12)
- 1.10.0: `PreviewSession`公開、`PropCache`公開、`AvatarProcessor.ManualProcessAvatar()`(プラットフォーム対応の手動ビルド)、mip streaming欠落のエラー報告、`GetAvatarRoots`がアクティブなルートのみ返すよう変更

### 1.9.x (2025-09)
- **パラメータ型変更時のVRC Parameter Driver挙動保存**(1.9.0 #693。AAOのEntryExit→BlendTree最適化と連動)。`VirtualAnimatorController.Parameters`の値のin-place変更が非推奨化(将来破壊予定)
- 1.9.1: Add modeのParameter Driver修正。1.9.2-1.9.4: プレビュー安定化

### 1.8.x (2025-07)
- **マルチプラットフォームAPI**: `[RunsOnPlatforms]`/`[RunsOnAllPlatforms]`、`WellKnownPlatforms`、ポータブルコンポーネント群(Avatar Root/Viewpoint/Viseme/Dynamic Bone)。Resonite対応基盤
- `CompatibleWithContext`属性追加。1.8.2: blendshape以外のviseme処理修正

### 1.7.x (2025-04〜05)
- **1.7.0: AnimatorServicesContext導入**(VirtualClip等)。プラグイン無効化ウィンドウ。`INDMFEditorOnlyComponent`
- **1.7.2: `IVirtualizedMotion`を`nadena.dev.ndmf.animator`へ移動(semver破壊)**
- 1.7.5: 重複パラメータ許容化、プロキシクリップ復元。1.7.6: AnimatorLayerControl喪失修正。1.7.7: 重複レイヤーエントリ修正、パス多重書き換え時の削除修正(MA 1.12.5が要求)

### 1.6.x (2024-11〜2025-02)
- 1.6.0: `IAssetSaver`/`SerializationScope`(アセット分割保存)、`DependsOnContext`
- **1.6.1: `IExtensionContext.Owner`を削除(意図的なsemver違反)**
- 1.6.2: 全アセットロードによる性能問題修正

### 1.5.x (2024-09〜11)
- **1.5.0: プレビューシステム導入。Unity 2019廃止**。パス/プラグイン単位のプレビューON/OFF UI。ChilloutVR修正(hai-vr提供)。非ASCIIプロジェクトパスのVRCSDKバグ回避
- 1.5.4: `Apply on Build`メニュー追加。1.5.7: RecalculateUVDistributionMetrics自動適用(streaming mipmaps互換)

### 1.4.x (2024-03〜05)
- 1.4.0: パラメータイントロスペクションAPI、PhysBonesプレフィックスのSubParameters
- 1.4.1: MA併用時のManual Bakeメニュー、`__Generated`フォルダ扱い変更

### 1.3.x (2024-01〜03)
- 1.3.0: ローカライズ/エラーレポート枠組み、オブジェクト置換記録API。**VRCFury互換のためフック処理順を調整**
- 1.3.2: フック処理ロジックをVRCSDK準拠に変更(VRCF等の外部フック互換性改善)

### 1.2.x以前 (2023)
- 1.2.5: VRChatテンプレート派生パッケージとのGUID衝突修正
- 1.0.1: **Optimizingフェーズを-1025へ移動**(VRChatのIEditorOnly破棄(-1024)より前に実行するため)
- 1.0.0 (2023-09): 初回安定版

### 推奨組み合わせ
- MA 1.17.x ⇔ NDMF 1.13〜1.14 / AAO 1.9.x ⇔ NDMF ≥1.8 / TTT 1.0.x ⇔ NDMF ≥1.7
- NDMF更新時はMA・AAO・TTTも同時に更新する(単独更新はAPI欠落リスク)
