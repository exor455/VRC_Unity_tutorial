# AAO: Avatar Optimizer

- リポジトリ: https://github.com/anatawa12/AvatarOptimizer
- ドキュメント: https://vpm.anatawa12.com/avatar-optimizer/
- パッケージ名: `com.anatawa12.avatar-optimizer`(VPMリポジトリ: vpm.anatawa12.com)
- 作者: anatawa12

## 概要

**非破壊のアバター自動最適化ツール**。`AAO Trace And Optimize`(T&O)をアバターに1つ付けるだけで、未使用オブジェクト削除・BlendShape凍結・メッシュ/ボーン/PhysBone統合・アニメーター最適化・テクスチャ最適化までを安全側の解析付きで自動実行する。手動最適化用コンポーネント群も提供。

- Unity要件: 2022.3(**1.8.0でUnity 2019サポート廃止**、1.7.xが2019最終)
- VRCSDK: 1.9系は`>=3.7.0 <3.11.0`。VRM(UniVRM)や非VRChatプロジェクトにも対応(1.9.0で非VRCSDKプラットフォーム互換宣言)
- 実行フェーズ: NDMF Resolving(早期パス)+ **Optimizing(本体、意図的に最後尾)**

## 依存関係

- 必須: NDMF(1.9系: `>=1.8.0 <2.0.0`)、`com.unity.nuget.newtonsoft-json`、`com.unity.burst`
- **GPU必須**: 1.8.0以降、`-nographics`環境では動作しない(テクスチャ処理)
- 他ツールへの認識機構: Asset Description(サードパーティコンポーネントの無害宣言)、コンポーネント登録API(`Make your components compatible with AAO`)

## 対応する改変パターン

### 自動最適化(AAO Trace And Optimize)
- 未使用オブジェクト/コンポーネント/ボーンの削除(GC。PhysBone・Contact・Constraint連鎖も解析)
- 未使用BlendShapeの凍結・削除、常時一定値アニメのBlendShape凍結(レイヤーウェイト0〜1も解析、1.9+)
- BlendShape自動統合(1.8+)、メッシュ自動統合(AutoMergeSkinnedMesh、1.7+。BlendShape持ちも1.8+で対象)
- マテリアルスロット自動統合(1.8+)、空サブメッシュ削除
- ボーン自動統合(MergeBone)
- PhysBone最適化: 不要isAnimated解除、同一設定フロアコライダー統合(1.6+)、**PhysBone自動統合**(掴めないPB対象、1.9+)、EndBone→Endpoint Position自動置換(1.9+)
- **Optimize Animator**(1.7+): 意味なしレイヤー/プロパティ削除、AnyState→Entry/Exit変換(1.8+)、Entry/Exit→1D BlendTree変換、Complete Graph→Entry/Exit(1.9+)、BlendTreeのDirect BlendTree統合
- **Optimize Texture**(1.8+): テクスチャアトラス化・VRAM削減(**対応シェーダー: lilToon、Toon Standard系のみ**)、未使用マテリアルプロパティ/テクスチャ削除
- 最適化メトリクスのコンソール出力(1.9+)

### 手動コンポーネント
- **AAO Merge Skinned Mesh**: メッシュ統合(BlendShape改名/統合モード、有効化アニメーションのコピー(1.8+)、マテリアル別アニメ警告)
- **AAO Merge PhysBone**: PhysBone統合(Endpoint Position: Clear/Copy/Override、Limits: Copy/Override/Fix(回転補正、1.8+)、`INetworkID`対応)
- **AAO Freeze BlendShape** / **AAO Rename BlendShape**(1.8+)
- **AAO Remove Mesh By Box(旧In Box)/ By BlendShape / By Mask / By UV Tile**(1.8+。Invertオプションあり、基本メッシュ(非Skinned)も1.9+で対応)
- **AAO Merge Material**(1.9+、lilToon/Toon Standard他対応。**Merge ToonLit Materialの後継**=旧版は非推奨・次期メジャーで削除予定)
- **AAO Max Texture Size**(1.9+)
- **AAO Clear Endpoint Position** / **AAO Make Children** / **AAO Remove Zero Sized Polygon**(Advanced扱い)
- マスクテクスチャエディタ内蔵(1.7.1+、1.9で刷新)

## 改変時の注意点(ソース由来の癖)

- **意図的に「最後」に実行される**(1.9.0+): プラグインクラスをU+FFDC始まりの名前空間に置きFullNameソートで最後尾へ(`Editor/OptimizerPlugin.cs`)。AAOの後に動きたいツールは`AfterPlugin("com.anatawa12.avatar-optimizer")`が必要で、その場合AAOへのコンポーネント登録が別途要ることが多い
- Resolvingフェーズにも早期パスがある: `MakeChildren(early)`、UnusedBonesByReferencesTool互換処理は**EditorOnly削除より前**に実行(`BeforePass(RemoveEditorOnlyPass)`)
- **アニメーションはビルド前パスで作る**: AAOがオブジェクト構造を変えた後のパス(ビルド後パス)を対象にしたアニメは1.7.0以降無効(「Animations animating missing GameObject is removed」)
- T&Oの解析は保守的だが、**Animator/アニメーションの解釈に依存**する。外部ツールがT&O設定を書き換えるケースの診断用に、1.9.14+のBug Report HelperがT&O実効設定JSONを出力する
- Read/Write無効メッシュ: 通常ビルドは処理可能。Av3Emulator起動時のみ制約あり(1.8.0で大幅改善)
- 頂点インデックス依存シェーダーを検知すると自動メッシュ統合をスキップ(1.8+)
- 孤立頂点(どの三角形にも属さない)はバウンズ制御用とみなし保持(1.9+)
- MMD対応: MMD互換BlendShape名は保護される(T&OのMMD World Compatibility設定。Body以外のexclusion併用バグは1.9.2で修正)
- **VRCSDKバージョン適合が厳格**: 新VRCSDKコンポーネント(VRCConstraints 1.7.10-11、VRCPerPlatformOverrides 1.8.11、VRCRaycast 1.9.9〜1.9.10、PB Global Collider 1.9.14)への対応はAAO側の更新が必須。VRCSDK更新直後はAAOのパッチ追従を確認する
- lilToon固有処理が多い(AudioLinkマスク、AngelRing、アウトラインマスク等)→[lilToon](04-liltoon.md)側のバージョンにも依存

## Quest対応時の注意

- AAO自体はAndroid/iOSビルドで動作。**Windows上でAndroidターゲットのASTC生成が失敗するUnity未文書挙動**があり、MaxTextureSizeのエラーは1.9.5で修正済み
- Quest向けはT&O+Optimize Textureの効果が大きい(VRAM/DL容量制限対策)。ただしOptimize TextureのシェーダーサポートはlilToon/Toon Standardのみで、**Quest変換後のToon Lit材質はVQTのテクスチャ処理側が担当**
- 実行順: VQTのアバター変換(Optimizing)より後にAAOが走り、VQTのテクスチャ形式チェックはさらにその後(AAO生成テクスチャも検査される)
- GogoLoco等のfly系: VRCStation由来BoxColliderのアニメが削除される問題は1.8.0で修正(Quest対応アバターで顕在化しやすかった)

## 関連ファイルパス

| パス | 役割 |
|---|---|
| `package.json` | パッケージ定義(NDMF/VRCSDK範囲指定) |
| `Editor/OptimizerPlugin.cs` | NDMFプラグイン定義(U+FFDC名前空間トリック、全パス順序の一次情報) |
| `Editor/Processors/` | 各最適化の実装(TraceAndOptimizes/、AnimatorOptimizer/等) |
| `.docs/content/docs/reference/<component>/` | コンポーネント別公式ドキュメント(Hugo) |
| `CHANGELOG.md` / `CHANGELOG-PRERELEASE.md` | 変更履歴(安定版/プレリリース分離) |
| `ProjectSettings/AvatarOptimizerSettings.asset` | プロジェクト設定(IEditorOnly無視リスト等、1.9+) |

## よくあるトラブル

- **表情/ギミックのBlendShapeが動かない**: 凍結対象の誤判定(実際はAAOのバグか、アニメーター解釈の限界)。T&OのExclusionsに追加して切り分け、最新パッチ確認
- **PhysBone挙動の変化**: Merge/Auto Merge PhysBoneの既知バグ修正が1.9.3〜1.9.13に大量にある(disabled PB巻き込み、humanoidボーン対象、親PBのIgnore Transforms、curve補正等)。**PhysBone関連の異常はまずAAOを最新パッチへ**
- **アイトラッキング停止**: 片目ボーンだけ未使用と判定→両目保持は1.9.13で修正
- **見えないメッシュ/破損**: 無限大デルタBlendShape凍結(1.9.0修正)、ゼロスケールボーンのMergeBone(1.8.13修正)、Z-fighting(透け髪、1.9.15修正)
- **アニメが一部消える**: 「'/'を含むGameObject名」(1.8.7修正)、日本語環境での中国語文字アニメ(1.9.6修正)、Gestureレイヤーのrevert絡みでFX無視(1.9.9修正)
- **ビルドは通るが容量が増えた**: MeshCompression設定の非保持(1.9.0修正)、未使用テクスチャの巻き込み(T&Oの未使用マテリアルプロパティ削除で対処)
- **「unknown IEditorOnly component」警告**: サードパーティコンポーネントが未登録。1.9+ではProject Settingsから無視リストに追加可能。ツール開発者はAsset Description/登録APIで対応

## 関連ツール

- [NDMF](01-ndmf.md): 実行基盤(AnimatorServices/ObjectRegistry/プレビューを深く利用)
- [TexTransTool](03-textranstool.md): TTTが先に実行され、AAOとUV/RemoveMesh領域をネゴシエーション
- [Modular Avatar](02-modular-avatar.md): MA出力を最適化(DelayDisable等はMA 1.12+でAAOフレンドリー化)
- [VRCQuestTools](07-vrcquesttools.md): VQT変換→AAO→VQT形式チェックの順。**AAO 1.7.0+はVQT 2.x必須**

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
