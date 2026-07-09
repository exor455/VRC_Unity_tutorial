# NDMF (Non-Destructive Modular Framework)

- リポジトリ: https://github.com/bdunderscore/ndmf
- ドキュメント: https://ndmf.nadena.dev
- パッケージ名: `nadena.dev.ndmf`(VPMリポジトリ: vpm.nadena.dev)
- 作者: bd_(Modular Avatarと同一作者)

## 概要

VRChatアバター向け**非破壊ビルドプラグインのフレームワーク**。それ自体は改変を行わず、Modular Avatar / AAO / TexTransTool / VRCQuestTools などのプラグインをビルド時(アップロード時・Play Mode突入時・Manual Bake時)に一貫した順序で実行する基盤。

- Unity要件: 2022.3(package.json `unity: 2022.3`)。**1.5.0でUnity 2019サポート廃止**
- VRCSDK要件: package.jsonでは `vrchatVersion: 2022.1.1` 以上相当。VRCSDKなしプロジェクトでも動作可(1.1.1+)
- ユーザーが直接操作するのは主に: NDMF Console、Apply on Play/Build トグル、プラグイン有効/無効ウィンドウ、Manual Bake

## 依存関係

- 依存: `com.unity.modules.animation` のみ(VRCSDKはオプショナル。Version Definesで検知)
- 被依存: MA(`>=1.11 <2.0.0-a`)、AAO(`>=1.8.0 <2.0.0`)、TTT(`>=1.7.0`)、VQT(オプショナル、NDMF系機能に必須)ほか多数
- ChilloutVR / VRM / Resonite(1.8.0+の実験的プラットフォームAPI)にも対応コードがある

## 対応する改変パターン

NDMF自体はフレームワークであり直接の改変機能はほぼ無いが、以下を提供する:

- **ビルドパイプライン**: フェーズ(Resolving/Generating/Transforming/Optimizing)+ 制約解決(詳細は[ツール横断情報](00-cross-tool.md)§1)
- **Apply on Play**: Play Mode突入時の非破壊適用(Av3Emulator検出時は抑止)
- **Manual Bake Avatar**: エディタ上で適用結果のコピーを生成(改変結果の確認・非VRChat用途への書き出し)
- **NDMFプレビューシステム**(1.5.0+): オブジェクトを変更せずレンダリングのみ差し替えるエディタプレビュー基盤。MAのShape Changer、AAOのRemove Mesh系、TTTの各フェーズプレビューが利用
- **エラーレポート/ローカライズ基盤**(1.3.0+): NDMF Console
- **AnimatorServicesContext**(1.7.0+): アニメーターの仮想化(VirtualClip/VirtualAnimatorController)。パス書き換え・パラメータ型調整・プロキシクリップ保護を一元化
- **ParameterProvider**(1.4.0+): Expressionsパラメータ使用量のイントロスペクション
- **ObjectRegistry**: オブジェクト置換の追跡(あるツールが置換したオブジェクトを他ツールが追跡できる)
- (プラグイン開発用の内部API群 — `IAssetSaver` / `PropCache` / `ProfilerScope` 等 — は本KBの対象外。必要時はNDMF公式doc/ソース参照)

## 改変時の注意点(実装上の癖)

- **同一フェーズ内の既定順序はプラグイン型FullNameの序数順**(`PluginResolver.cs`)。順序が重要な場合は必ずBefore/AfterPluginを書く。名前変更は事実上の順序変更になる
- 制約はオプショナル依存: 相手が入っていなければ単に無視される。フェーズをまたぐ制約は例外
- Transforming終了時に未保存アセットがシリアライズされる(1.5.0+)。VRCFury等の非NDMFフックとの互換のため
- VRChatのプロキシアニメーション(proxy_*)はNDMFが保護する。他プラグインがプロキシクリップをクローンした場合、ObjectRegistryに登録されていれば元参照に復元される(1.7.5+)。AnimatorOverrideControllerのプロキシ経由マッピングは1.13.1で修正
- 重複するアニメーターパラメータは「最後の定義」が使われる(1.7.5+、以前はビルド失敗)
- パラメータ型の調整(Harmonize): MAのMerge Animator等で型不一致があるとFloatへ調整されるが、VRC Parameter Driverの挙動保存は1.9.0+(Add mode修正は1.9.1)
- アバタールートが`EditorOnly`タグだと明示エラー(1.14.0+。以前は不可解な挙動)
- テクスチャのmip streaming欠落はVRChatビルドでエラー報告される(1.10.0+)
- 生成メッシュには`Mesh.RecalculateUVDistributionMetrics`が自動適用される(1.5.7+、オプトアウトAPIあり)
- **プレビューシステムはカメラ単位でレンダラーを差し替える**。プレビュー中のシーンビュー選択やドラッグ&ドロップの不具合は1.5.x系で多数修正済み→NDMFは常に最新パッチを使う

## Quest対応時の注意

- NDMF自体はプラットフォーム非依存。ビルドターゲット切替(PC⇔Android)自体には関与しない
- 1.8.0+のプラットフォームAPI(`RunsOnPlatforms`)により、プラグインがプラットフォーム別にパスを有効化できる(VQTがこれとは別に独自のビルドターゲット概念を持つ点に注意)
- Sync Parameter Sequence(MA)等のクロスプラットフォーム同期機能はNDMFのパラメータイントロスペクションに依存

## 関連ファイルパス

| パス | 役割 |
|---|---|
| `package.json` | パッケージ定義(リポジトリルート=パッケージ) |
| `Editor/API/BuildContext.cs` | ビルドコンテキスト(AvatarRootObject等) |
| `Editor/API/Fluent/Plugin.cs` | `Plugin<T>`基底クラス、`InPhase()` |
| `Editor/API/Solver/PluginResolver.cs` | プラグイン列挙・順序解決(FullName序数ソート+トポソート) |
| `Editor/VRChat/BuildFrameworkPreprocessHook.cs` | VRCSDKフック(-11000 / -1025) |
| `Editor/API/RemoveEditorOnlyPass`(builtin) | Resolving組み込みのEditorOnly削除 |
| `docfx~/` | 公式ドキュメントソース(execution-model.md / extension-context.md / best-practices.md / versioning-policy.md) |
| `CHANGELOG.md` | 変更履歴(プレリリースはCHANGELOG-PRERELEASE.md) |

## よくあるトラブル

- **「Duplicate pass with qualified name」**: 同名パスの二重登録。プラグイン開発時のミス、または同一プラグインの二重インポート(Assets+Packages)
- **順序が期待と違う**: 制約なしプラグインはFullName順。プラグインシーケンス表示ウィンドウで実際の順序を確認
- **Apply on Playが効かない**: Av3Emulator併存(意図的抑止)、`Apply on Play`設定、複数シーンロード時の既知問題(1.2.3で修正)
- **ビルドは通るがgit diffにプロキシクリップ変更が出る**: 1.8.0(#664)で修正
- **「Unreachable code reached???」**: 1.7.2で修正されたアニメーター処理の既知バグ
- **エディタフリーズ/性能劣化**: 1.6.3〜1.6.5でスレッド安全性・性能修正。preview関連のスタックは1.9.4で修正
- **エディタスクリプトからの`Camera.Render()`でNDMFプレビューが乗らない(`PreviewSession`常にnull)**: `NDMFPreview._globalPreviewSession` / `PreviewSession.ForCamera(cam)` が手動の`Camera.Render()`呼び出し時に常にnullを返す。原因はNDMFプレビューセッションがSceneViewのカメラレンダリングイベント(`Camera.onPreCull`等)を起点として初めて生成されるため。`NDMFPreview.Init()` / `NDMFPreviewSceneManager.Init()` / `ComputeContext.FlushInvalidates()`を事前に呼んでも解消しない(エディタアップデートループ外での実行制約による)。(実測: Unity 2022.3.22f1 / NDMF 1.14系 / VRCSDK3、2026-07)  
  **代替手順(シーンを変更せず視覚確認する場合)**:  
  1. 表示したいSkinnedMeshRendererを`BakeMesh(mesh, useScale:true)`でベイクする  
  2. `Graphics.DrawMesh(baked, smr.localToWorldMatrix, material, layer, camera, submeshIdx)`を`camera.Render()`直前に呼んで描画に注入する  
  3. 非表示にするRendererは`renderer.enabled`を一時falseにしてRender後に復元する(`activeSelf`は変更しない)  
  4. ON/OFFのリストはMA Object Toggleコンポーネントの`m_objects`から読んで駆動する(実設定との食い違い防止)

## 関連ツール

- 全NDMFプラグインの前提。特に [Modular Avatar](02-modular-avatar.md)(同一作者・同時リリースが多い)
- [AAO](09-avatar-optimizer.md) はNDMFのAnimatorServices/ObjectRegistry/プレビューを深く利用
- 非NDMFツール(VRCFury等)との順序関係は[ツール横断情報](00-cross-tool.md)§1.1

## バージョン履歴

破壊的変更・修正版マッピングは [01-ndmf-changelog.md](01-ndmf-changelog.md) に分離。バージョン起因の不具合を切り分ける時だけ参照する。
