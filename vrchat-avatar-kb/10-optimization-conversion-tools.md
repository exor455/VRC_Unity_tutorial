# 軽量化・変換ツール(メッシュ削減・マテリアル変換)

[AAO](09-avatar-optimizer.md)/[TTT](04-textranstool.md)/[VQT](11-vrcquesttools.md)を補完する、ポリゴン削減とマテリアル統一の専門ツール群。

> 情報源: 各リポジトリのREADME/package.json(2026年前半時点)+主要ツールのプラグイン定義に現れる順序制約。

## 目的別早見表

| 目的 | 第一候補 | 備考 |
|---|---|---|
| ポリゴン削減(無料・現行) | Meshia Mesh Simplification | lilNDMFMeshSimplifierの公式後継 |
| ポリゴン削減(品質重視・有償) | NDMF Mantis LOD Editor | Mantis LOD Editor Pro(Asset Store)が別途必要 |
| アバター全体を目標ポリゴン数へ一括削減 | Overall NDMF Mesh Simplifier | |
| 他シェーダー→lilToonへ統一 | lilMaterialConverter | UTS2対応 |
| メッシュ統合・未使用削除・テクスチャ最適化 | [AAO](09-avatar-optimizer.md) | 削減率よりも「無駄の除去」 |
| テクスチャアトラス化 | [TTT](04-textranstool.md) AtlasTexture | |
| Quest向けマテリアル変換 | [VQT](11-vrcquesttools.md) | |

## ポリゴン削減系に共通する実行順序の知識

主要ツールのソース上の制約(KB [00 §1.5](00-cross-tool.md)、[08 早見表](08-ecosystem-tools.md))から:

- **TTTはポリゴン削減系より前**に実行される(UV/マスクが変わる前にテクスチャ処理を終えるため。TTT 0.8.11/0.10.4で順序修正の履歴)
- **VQTのMesh Flipperは削減前/削減後を選択可能**(BeforePolygonReduction/AfterPolygonReduction)。VQTのアバター変換(Optimizing)は削減系の後
- **AAOは全削減系より後**(最後尾)。削減後のメッシュに対して未使用削除・統合が走る
- 削減系ツール同士の併用は原則避ける(同一メッシュへの多重削減は品質劣化の元)

---

## Meshia Mesh Simplification(Ram.Type-0)

- リポジトリ: https://github.com/RamType0/Meshia.MeshSimplification
- パッケージ名: `com.ramtype0.meshia.mesh-simplification` / 3.x(Unity 2022.3)
- NDMFプラグインID: `Meshia.MeshSimplification.Ndmf.Editor.NdmfPlugin`

**Burst/Job System実装の高速・非同期メッシュ簡略化**ツール/ライブラリ。エディタでもランタイムでも実行可能で、NDMF統合コンポーネントによる非破壊削減が改変用途の主線。**lilNDMFMeshSimplifierの公式後継**(lil側READMEが移行を明示)。

- 依存: Burst / Collections / Mathematics、CL4EE(anatawa12のローカライズライブラリ)
- コンポーネントで対象メッシュと削減率(目標)を指定 → ビルド時に適用。NDMFプレビュー対応
- 順序: TTTより後・VQT変換より前(TTT/VQT両方が制約を宣言済み)

## lilNDMFMeshSimplifier(lilxyzw)【開発終了】

- リポジトリ: https://github.com/lilxyzw/lilNDMFMeshSimplifier
- NDMFプラグインID: `jp.lilxyzw.ndmfmeshsimplifier.NDMF.NDMFPlugin`

UnityMeshSimplifierをNDMFプラグイン化したもの。quality値(例: 0.5で約半分のポリゴン)を設定するだけの簡易操作。**プロジェクトは終了済みで、READMEがMeshia Mesh Simplificationへの移行を明示**。既存アバターで見かけた場合の知識として記録(TTT/VQTの順序制約リストには互換のため残っている)。

## NDMF Mantis LOD Editor(hitsub)

- リポジトリ: https://github.com/hitsub/ndmf-mantis-lod-editor
- 配布: https://booth.pm/ja/items/5409262(ツール自体は無料)
- NDMFプラグインID: `MantisLODEditor.ndmf`

Asset Storeの**有償ポリゴン削減アセット「Mantis LOD Editor Professional Edition」を非破壊(NDMF)化するラッパー**。本体アセットの購入が前提。品質評価の高いMantisの削減アルゴリズムを非破壊で使える点が価値。

- **Transformingフェーズで実行される**(Optimizingではない)。このため他ツールとの順序は「TTTの後・MA lateの前後」の文脈で考える必要がある(リポジトリIssue #10でフェーズ選定の議論あり)
- VQTはMesh Flipper(削減前)→Mantis→RemoveVertexColor(削減後)の順序制約を宣言(頂点カラーを削減制御に使うため)

## Overall NDMF Mesh Simplifier(aoyon)

- NDMFプラグインID: `com.aoyon.overall-ndmf-mesh-simplifier`

アバター**全体を一括で目標値まで削減**するアプローチのNDMFツール。メッシュ個別指定ではなく全体最適で削減したい場合の選択肢。VQTが順序制約(変換より前)を宣言している。詳細仕様はリポジトリを参照(本KBでは順序関係の記録が主)。

## lilMaterialConverter(lilxyzw)

- リポジトリ: https://github.com/lilxyzw/lilMaterialConverter

マテリアルを右クリック→`lilToon/Convert material to lilToon`で**他シェーダーのマテリアルをlilToonへ変換**するツール。対応表(README)は現状 **Unity-Chan Toon Shader 2.0(UTS2)→lilToon**。

- 用途: 古いUTS2ベースのアバター/衣装をlilToonに統一し、その後の最適化([AAO](09-avatar-optimizer.md)のOptimize Texture、[TTT](04-textranstool.md)のアトラス化、[VQT](11-vrcquesttools.md)のQuest変換)をlilToon前提のパイプラインに載せる下準備
- 破壊的(マテリアルを直接書き換える)なので**変換前にバックアップ**をREADMEが明示
- Poiyomi→lilToonは対象外(逆方向のlilToon→PoiyomiはPoiyomi 9.3.64+が持つ。KB [Poiyomi](06-poiyomi.md))

---

## 選定ガイド(軽量化の全体戦略)

1. **現状把握**: lilAvatarUtils(テクスチャ/VRAM)+ Actual Performance Window(ランク実測)→ [12 検証・分析](12-analysis-upload-tools.md)
2. **無駄の除去**: AAO Trace And Optimize(未使用削除・統合・アニメーター最適化)
3. **テクスチャ**: TTT AtlasTexture / AAO Optimize Texture(lilToon系)
4. **ポリゴン**: 必要な場合のみ Meshia(無料)または Mantis NDMF(有償・高品質)
5. **Quest**: VQTで変換 → AAO → VQT形式チェック(順序はKB [00 §1.6](00-cross-tool.md))
