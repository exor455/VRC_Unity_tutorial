# VRChatアバター改変ナレッジベース(単一ファイル版)

> vrchat-avatar-kb/ の全20ファイルを結合したもの(2026-07-02時点)。本文中の `XX-....md` へのリンクは、このファイル内の対応する「FILE: XX-....md」セクションを指す。


======================================================================
# FILE: index.md
======================================================================

# VRChatアバター改変 ナレッジベース

VRChatアバターの非破壊改変(NDMFエコシステム)に関するナレッジベース。LLMセッションのRAGソースとして、任意のアバターに対する改変手順の生成・トラブルシュートに使うことを想定している。

## 前提環境

- Unity 2022.3.22f1
- VRCSDK3 Avatars(バージョン更新が頻繁なため記述はバージョン非依存を基本とし、依存する挙動は「どのバージョン以降か」を明記)
- 対象アバターは特定しない(汎用)
- 基本はPC向け。Quest(Android/iOS)対応の制約・注意点は各ツールの「Quest対応時の注意」に記載

## 情報源と鮮度

各ツールのGitHubリポジトリ(README / CHANGELOG / ソースコード / 公式ドキュメント)を直接読解して作成。2026年前半時点のリポジトリHEADに基づく。実行順序・フェーズ構成などパイプラインに関する記述は、各ツールのNDMFプラグイン定義ソースを一次情報とする(ドキュメントと齟齬がある場合はソースを正とし、その旨を明記)。

## ファイル構成(改変ワークフロー順)

**基盤 → 着せる → 見た目 → 動き・メニュー → 軽量化 → Quest → 検証** の順に並んでいる。

| ファイル | 内容 |
|---|---|
| [19-triage-guide.md](19-triage-guide.md) | 【**入口**】**問診ガイド** — 「なんか壊れた」「服着せたい」「表情バグった」等の**曖昧な相談を受けたらまずここ**。確認質問の仕方、ユーザーの言葉→技術用語の対訳表、症状別フロー |
| [00-cross-tool.md](00-cross-tool.md) | **ツール横断情報**: NDMFビルドパイプライン上の全ツールの実行順序と処理フェーズ / 互換性マトリクス / 共通トラブルシュートフロー / 推奨バージョン組み合わせ |
| [01-ndmf.md](01-ndmf.md) | 【基盤】**NDMF** — 非破壊ビルドの基盤フレームワーク(フェーズ、順序解決、プレビュー、AnimatorServices) |
| [02-modular-avatar.md](02-modular-avatar.md) | 【基盤】**Modular Avatar** — 衣装統合(Merge Armature)、アニメーター/メニュー/パラメータ合成、リアクティブコンポーネント、Mesh Cutter |
| [03-fitting-tools.md](03-fitting-tools.md) | 【着せる】**衣装フィッティング(非対応衣装)** — もちふぃった～(MochiFitter) / EreMorph / KiseteneEx、非対応衣装ワークフロー |
| [04-textranstool.md](04-textranstool.md) | 【見た目】**TexTransTool** — デカール、レイヤー合成(PSD)、マテリアル改変、テクスチャアトラス化 |
| [05-liltoon.md](05-liltoon.md) | 【見た目】**lilToon** — シェーダー(日本圏標準)。マテリアル調整、ビルド時最適化の癖 |
| [06-poiyomi.md](06-poiyomi.md) | 【見た目】**Poiyomi Toon Shader** — シェーダー(英語圏標準)。ロック機構とAnimated指定 |
| [07-expression-gimmick-tools.md](07-expression-gimmick-tools.md) | 【動き】**表情・ギミック生成** — FaceEmo / ComboGestureExpressions / Light Limit Changer / FloorAdjuster / VRLabs Avatars 3.0 Manager、有償定番ギミックのメモ |
| [08-ecosystem-tools.md](08-ecosystem-tools.md) | 【動き】**周辺エコシステム(メニュー/トグル生成)** — AvatarMenuCreatorForMA / lilycalInventory / Flare / Vixen・AAC、トゥイーン・フェード機能の有無比較、NDMFプラグインID早見表 |
| [09-avatar-optimizer.md](09-avatar-optimizer.md) | 【軽量化】**AAO: Avatar Optimizer** — Trace And Optimizeによる自動最適化、メッシュ/PhysBone/アニメーター/テクスチャ最適化 |
| [10-optimization-conversion-tools.md](10-optimization-conversion-tools.md) | 【軽量化】**軽量化・変換** — Meshia Mesh Simplification / lilNDMFMeshSimplifier(終了) / NDMF Mantis LOD Editor / Overall NDMF Mesh Simplifier / lilMaterialConverter、軽量化の全体戦略 |
| [11-vrcquesttools.md](11-vrcquesttools.md) | 【Quest】**VRCQuestTools** — Quest/Android/iOS変換(マテリアルベイク、コンポーネント除去、プラットフォーム出し分け) |
| [12-analysis-upload-tools.md](12-analysis-upload-tools.md) | 【検証】**検証・分析・アップロード・環境管理** — Gesture Manager / Av3Emulator / lilAvatarUtils / Actual Performance Window(gist pack) / Continuous Avatar Uploader / vrc-get・ALCOM / VPMPackageAutoInstaller / Pumkin's / lilEditorToolbox |
| [13-vrchat-avatars-basics.md](13-vrchat-avatars-basics.md) | 【リファレンス】**VRChat/Avatars 3.0基礎** — Playable Layers / Expression Parameters(256bit) / **Performance Rank公式テーブル(PC/Mobile)** / アバターサイズ制限 / セーフティ・フォールバック / ビルドコールバック順 |
| [14-vrcfury.md](14-vrcfury.md) | 【リファレンス】**VRCFury(防御的ナレッジ)** — 本KBでは不使用。導入済みアセット対応のための挙動・既知の相互作用・併用定石 |
| [15-gogo-loco.md](15-gogo-loco.md) | 【リファレンス】**GoGo Loco** — 移動システムの定番。レイヤー差し替え・16bit同期・AAOとの既知問題 |
| [16-world-integrations.md](16-world-integrations.md) | 【リファレンス】**ワールド連携** — AudioLink / LTCGI / VRC Light Volumes とシェーダー対応バージョン |
| [17-unity-troubleshooting.md](17-unity-troubleshooting.md) | 【リファレンス】**Unity定番トラブル** — ピンクマテリアル / Missing Script / unitypackage×VPM二重導入 / アップロード失敗の切り分け |
| [18-license-notes.md](18-license-notes.md) | 【リファレンス】**ライセンス実務メモ** — アバター規約の確認項目 / ツール・シェーダーのライセンス早見 / 受け渡しチェックリスト |

## 各ツールファイルの共通構成

1. **概要** — 何をするツールか、Unity/VRCSDKバージョン要件
2. **依存関係** — 他ツール・アセットとの前提関係
3. **対応する改変パターン** — そのツールで可能な改変の網羅列挙
4. **改変時の注意点** — ソースコード解析に基づく実装上の癖・制約・相性
5. **Quest対応時の注意** — PC→Quest移行時の挙動・出力の変化
6. **関連ファイルパス** — 主要スクリプト・設定ファイルのパスと役割
7. **よくあるトラブル** — 既知の落とし穴と対処
8. **関連ツール** — 相互参照
9. **バージョン履歴** — 破壊的変更 / 他ツール互換に影響する変更 / 重要バグ修正 / 推奨組み合わせ

## 典型的な参照パターン(RAG利用時のヒント)

**相談者は初心者が前提。質問が曖昧・症状ベース(「なんか壊れた」「〜したい」)なら、個別ファイルより先に必ず [19-triage-guide.md](19-triage-guide.md) を読み、確認質問→原因候補の順で対応する。** 以下は質問が具体的な場合の直行先:

- 「実行順序」「どのツールが先か」「フェーズ」→ 00-cross-tool.md §1
- 「AとBの併用で壊れる」→ 00-cross-tool.md §2(互換性マトリクス)+各ツールの「よくあるトラブル」
- 「ビルドエラー」「アップロードだけ壊れる」→ 00-cross-tool.md §3(トラブルシュートフロー)
- 「衣装を着せる」「トグルを作る」→ 02-modular-avatar.md
- 「テクスチャ改変」「アトラス化」→ 04-textranstool.md
- 「軽量化」「Performance Rank改善」→ 09-avatar-optimizer.md(+04-textranstool.mdのAtlasTexture)
- 「Quest対応」→ 11-vrcquesttools.md +各ツールの「Quest対応時の注意」
- 「このバージョンで何が変わった」→ 各ツールの「バージョン履歴」
- 「メニュー/トグルをノーコードで作る」「フェード・トゥイーン」→ 08-ecosystem-tools.md
- 「知らないプラグインIDが順序制約に出てきた」→ 08-ecosystem-tools.md(プラグインID早見表)+ 07(有償ギミック)
- 「Unity内でメニューを試したい」「同期のテスト」「ランク実測」→ 12-analysis-upload-tools.md
- 「ポリゴン削減」「シェーダー統一」「軽量化の進め方」→ 10-optimization-conversion-tools.md
- 「表情システムを作る」「明るさ調整メニュー」「接地調整」→ 07-expression-gimmick-tools.md
- 「非対応衣装を着せたい」「貫通を直したい」→ 03-fitting-tools.md
- 「ランクの基準値」「パラメータ何bit」「サイズ制限」「フォールバック」→ 13-vrchat-avatars-basics.md
- 「VRCFury入りのアセット/アバターを扱う」→ 14-vrcfury.md
- 「移動システム」「飛行」「座りポーズ」→ 15-gogo-loco.md
- 「AudioLinkで光らせたい」「特定ワールドだけ暗い」→ 16-world-integrations.md
- 「マテリアルがピンク」「Missing Script」「アップロードだけ失敗」→ 17-unity-troubleshooting.md
- 「改変データを渡していい?」「同梱していい?」→ 18-license-notes.md


======================================================================
# FILE: 00-cross-tool.md
======================================================================

# ツール横断情報(NDMFエコシステム全体)

> 対象環境: Unity 2022.3.22f1 / VRCSDK3 Avatars(バージョン非依存の記述を基本とし、依存する箇所は明記)
> 情報源: 各ツールのGitHubリポジトリ(README / CHANGELOG / ソースコード)。2026年前半時点のHEADに基づく。
> 対象ツール: NDMF / Modular Avatar (MA) / TexTransTool (TTT) / lilToon / Poiyomi / AAO: Avatar Optimizer / VRCQuestTools (VQT)

---

## 1. NDMFビルドパイプラインと実行順序

### 1.1 VRChat SDKビルドへの介入ポイント

NDMFは VRCSDK の `IVRCSDKPreprocessAvatarCallback` に2つのフックを登録する(ソース: `ndmf/Editor/VRChat/BuildFrameworkPreprocessHook.cs`)。

| フック | callbackOrder | 実行するNDMFフェーズ |
|---|---|---|
| `BuildFrameworkPreprocessHook` | **-11000** | First 〜 Transforming |
| `BuildFrameworkOptimizeHook` | **-1025** (VRChatの `RemoveAvatarEditorOnly`(-1024) の直前) | Optimizing 〜 Last |

この分割により、**NDMF非対応ツール(VRCFury: callbackOrder -10000 など)は「NDMFのTransforming後・Optimizing前」に実行される**。VRCFury等と併用する場合の順序問題はここで理解する。

- Play Mode(Apply on Play)でも同じパイプラインが実行される。Av3Emulator / Gesture Manager が存在する場合はNDMFが二重実行を抑止する。
- NDMF 1.5.0以降、Transformingフェーズ完了時点で未保存アセットがシリアライズされる(VRCFury等の外部フックがアセット参照を壊さないための措置)。

### 1.2 ビルドフェーズ

ドキュメント上の公開フェーズは4つ: **Resolving → Generating → Transforming → Optimizing**。

**ドキュメントとソースの齟齬(ソースが正)**: 実際の`BuildPhase`にはこれに加えて `First` / `PlatformInit` / `Last`(および内部的なFirstChance)が存在し、VRChat向けフックは `First〜Transforming` / `Optimizing〜Last` の範囲で実行される。`PlatformInit` はNDMF 1.13+のマルチプラットフォーム基盤の初期化用。一般のプラグインが使うのは公開4フェーズと考えてよい。

| フェーズ | 想定用途 | 代表的な処理 |
|---|---|---|
| Resolving | 大規模変更前の参照解決・状態取得 | MAのオブジェクト参照解決、アニメーターのクローン、NDMF組み込みの `RemoveEditorOnlyPass`(EditorOnlyオブジェクト削除)、VQTのプラットフォーム別コンポーネント除去 |
| Generating | 後続プラグインが消費するコンポーネントの生成 | VQTのNetwork ID割り当て。MA向けコンポーネントを生成するツールはここ |
| Transforming | 一般的なアバター変換の本体 | MAの主要処理、TTTのテクスチャ改変、VQTのアバター変換(Transformingモード時) |
| Optimizing | 純粋な最適化 | TTTのAtlasTexture等、VQTの変換(既定のAutoモード)、AAOの全最適化、MAのGC |

### 1.3 同一フェーズ内のプラグイン順序決定アルゴリズム

ソース(`ndmf/Editor/API/Solver/PluginResolver.cs`)より:

1. `[assembly: ExportsPlugin(typeof(...))]` で登録された全プラグインを、**プラグイン型のFullName(名前空間+クラス名)の序数(ordinal)順**でソートして登録する。
2. 各プラグインの `Configure()` が宣言した Sequence / Pass と制約(`BeforePlugin` / `AfterPlugin` / `BeforePass` / `AfterPass` / `WaitFor`)を集め、**フェーズごとにトポロジカルソート**する。
3. 制約は「相手プラグインが存在する場合のみ」効く(オプショナル依存)。フェーズをまたぐ制約は例外を投げる。
4. 同一フェーズに複数Sequenceを宣言した場合、Sequence間の順序は保証されない(インターリーブされうる)。

**重要な帰結**:
- 制約が無いプラグイン同士の順序は「クラスのFullNameのアルファベット順」で決まる。AAOはこれを利用し、プラグインクラスをU+FFDC文字で始まる名前空間に置くことで**ほぼ確実に最後へ**ソートされるようにしている(AAO 1.9.0以降。ソース: `AvatarOptimizer/Editor/OptimizerPlugin.cs` のコメント)。
- 「AAOの後に実行したい」ツールは `AfterPlugin("com.anatawa12.avatar-optimizer")` を明示する必要がある。

### 1.4 対象ツールのプラグインIDとフェーズ配置(ソース確定情報)

| ツール | プラグインQualifiedName | Resolving | Generating | Transforming | Optimizing |
|---|---|---|---|---|---|
| Modular Avatar | `nadena.dev.modular-avatar`(+ `nadena.dev.modular-avatar.late-transform-stages`) | 参照解決・アニメータークローン | - | 本体ほぼ全部 / late側はTTT後にFloor Adjuster+MAコンポーネント purge | GC GameObjects |
| TexTransTool | `net.rs64.tex-trans-tool` | プレビュー解除・旧データ検出 | - | Material/UV系6サブフェーズ | AtlasTexture等+コンポーネント purge |
| AAO | `com.anatawa12.avatar-optimizer` | 早期MakeChildren等(RemoveEditorOnlyPassより前) | - | - | 全最適化(最後尾を狙う) |
| VRCQuestTools | `com.github.kurotu.vrc-quest-tools` | ビルドターゲット設定・Platform Remover類(MAより前) | Network ID割り当て | アバター変換(MA/TTT/lilycalInventory後)・MeshFlipper | アバター変換(AAOより前)・テクスチャ形式チェック(AAO後) |
| lilToon | (NDMFプラグインではない) | - | - | - | 独自のビルドコールバックでマテリアル/シェーダー最適化。NDMFがある場合はApply on Play時にスキップ可(1.8.4+) |
| Poiyomi | (NDMFプラグインではない) | - | - | - | ThryEditorのLock機構がVRChatビルドコールバックで動作 |

### 1.5 明示されている順序制約(ソースから抽出)

**TTT** (`TexTransTool/Editor/NDMF/NDMFPlugin.cs`):
- Transforming: `BeforePlugin` → Light Limit Changer(`io.github.azukimochi.light-limit-changer`)、FloorAdjuster(`net.narazaka.vrchat.floor_adjuster`)、Mantis LOD Editor
- Optimizing: `BeforePlugin` → AAO、Mantis LOD、Meshia MeshSimplification、lilNDMFMeshSimplifier、lilycalInventory
- AAO存在時は `NegotiateAAOPass` がTransforming先頭で連携設定を行う

**MA late stages** (`modular-avatar/Editor/PluginDefinition/PluginDefinition.cs`):
- Transforming: `AfterPlugin` → MA本体、**TTT**、marshmallow_PB。ここでFloor Adjuster実行→MAコンポーネント全破棄→ヒューマノイド再バインド(MA 1.17.1でこの構造になった。TTTのDecal等がMAコンポーネント位置に依存するため)

**VQT** (`VRCQuestTools/.../Editor/NDMF/VRCQuestToolsNdmfPlugin.cs`):
- Resolving: `BeforePlugin` → MA、VirtualLens2
- Transforming(変換パス): `AfterPlugin` → TTT、MA、lilycalInventory / MeshFlipperはMantisLODより前、RemoveVertexColorはMantisLODより後
- Optimizing(変換パス): `AfterPlugin` → TTT、posing system converter、lilNDMFMeshSimplifier、Meshia、Overall NDMF Mesh Simplifier / `BeforePlugin` → AAO
- Optimizing(CheckTextureFormatPass): `AfterPlugin` → TTT、**AAO**

### 1.6 実効的な全体実行順(標準構成: MA + TTT + AAO + VQT)

```
[callbackOrder -11000]
  Resolving:    VQT(Platform除去/変換準備) → MA(参照解決/アニメータークローン) → TTT(プレビュー解除) → AAO(早期パス) → NDMF組み込みEditorOnly削除
  Generating:   VQT(NetworkID)
  Transforming: MA本体(パラメータ改名→アニメーター統合→リアクティブ→メニュー→Merge Armature→…)
                → TTT(MaterialModification→BeforeUV→UV→AfterUV→PostProcessing)
                → MA late(Floor Adjuster→MAコンポーネント破棄)
                → VQT変換(Transformingモード時)
[callbackOrder -10000 など: VRCFury等の非NDMFツール]
[callbackOrder -1025]
  Optimizing:   MA(GC GameObjects) → TTT(AtlasTexture等→TTTコンポーネント破棄) → VQT変換(Autoモード既定) → AAO(全最適化) → VQT(テクスチャ形式チェック)
                ※MA GCの位置は明示制約がなくFullName序数順による(nadena.dev... < net.rs64...)。TTT/VQT/AAO間はソース上の制約で確定
[callbackOrder -1024] VRChatがEditorOnly破棄 → 以降VRCSDK標準処理
```

---

## 2. 互換性マトリクス

○=公式に連携/考慮あり、△=条件付き・注意、×=非互換または非対応。根拠は各ツールのCHANGELOG/ソース。

| | MA | TTT | AAO | lilToon | Poiyomi | VQT |
|---|---|---|---|---|---|---|
| **NDMF** | ○ 同一作者。MAはNDMF同時更新が前提(下記「バージョン組み合わせ」) | ○ 0.4.0以降NDMF必須化。0.10+はNDMF≥1.7 | ○ 1.5.0以降NDMF統合 | △ プラグインではないがNDMFを検知して挙動変更 | × 関知しない | ○ NDMFなしでも動くがNDMF系コンポーネントは無効 |
| **MA** | - | ○ TTTはMA後に実行。MA MaterialSetter/Animationで追加されたマテリアルにもTTTが効く(TTT 0.10+ / NDMF≥1.7)。MA lateパスがTTT後にFloor Adjuster実行 | ○ MAのDelayDisableレイヤーはAAOのオブジェクト追跡を考慮(MA 1.12+)。MAのGCはOptimizingでAAOより前 | ○ 特記事項なし(マテリアル改変なし) | ○ 特記事項なし | △ VQTはMA後に変換。MA Visible Head Accessory / World Fixed ObjectはMA 1.9+なら削除されない。未対応MAバージョンではエラー(VQT側) |
| **TTT** | | - | ○ 双方向連携: TTTはAAOより前(BeforePlugin)。AAOのRemoveMeshBy*領域をアトラス化から除外+UV退避報告(TTT 0.9+のNegotiateAAO、AAO 1.8のAPI)。同一メッシュ対象時の例外はTTT 0.10.9で修正 | ○ AtlasTextureにlilToon専用サポート(HSVG/MatCap等のプロパティベイク)。宝石・Lite系は対象外(誤検出は0.8.0/0.8.3で修正) | △ 専用AtlasShaderSupportなし。_MainTex等の汎用処理は可能だがプロパティベイク非対応 | ○ VQTはTTT後に実行(生成テクスチャを変換対象に含める)。テクスチャ形式チェックもTTT後(VQT 2.6.0) |
| **AAO** | | | - | ○ Optimize Texture / Remove unused propertiesはlilToon対応(1.8+)。lilToon固有の修正多数(アウトラインマスク1.8.1、AngelRing 1.8.5、AudioLinkマスク1.9.4、lilToon 1.9対応は1.8.9) | △ Optimize TextureはPoiyomi非対応(lilToon/Toon Standardのみ)。ロック済みマテリアルの未使用プロパティ削除は動作する | ○ VQT変換はAAOより前、形式チェックはAAO後。**AAO 1.7.0でVQT v1互換を削除→VQT 2.x必須** |
| **lilToon** | | | | - | △ 相互変換ではないがPoiyomi 9.3.64にlilToon→Poiyomi翻訳機能あり | ○ lilToon→Toon Lit/Toon Standard変換が最も充実。lilToon 2.0対応はVQT 2.11.1+。カスタムシェーダー派生も変換可(VQT 2.7.0+) |
| **Poiyomi** | | | | | - | △ Poiyomi→Toon Lit変換(メインカラー/ノーマル陰影/Emission 0-3)。→Toon Standardは実験的(VQT開発版) |

### 既知の非互換・要注意の組み合わせ(NDMF外含む)

- **VRCFury**: NDMFプラグインではないため順序制御不能(常にNDMF Transforming後・Optimizing前)。MA 1.15+はVRCFury < 1.1250.0 と Mesh Cutter / Shape Changer(delete mode)の併用に警告を出す(非互換)。
- **Av3Emulator**: NDMFはAv3Emulator検出時にApply on Playを抑止(二重適用防止)。AAOはAv3Emulator起動時にRead/Write無効メッシュを処理できないケースがある(1.8.0で大幅改善)。
- **AAO + `-nographics` バッチビルド**: AAO 1.8.0以降はGraphicsなし環境で動作しない(テクスチャ処理にGPUが必要)。CIでは`-nographics`を外す。
- **旧TTTセーブデータ**: TTT 1.0.0はv0.8.x以前のセーブデータ読み込みを削除。古いアセットは旧バージョン経由でマイグレーション必須。
- **Unity 2019**: NDMF 1.5.0+/AAO 1.8.0+/VQT 3.x(開発版)/Poiyomi 9.2.43+はUnity 2019サポートを終了済み。Unity 2022.3系を前提とする。
- **プラグイン名の序数ソート**: 独自ツールがAAOより後に動きたい場合、明示的な`AfterPlugin("com.anatawa12.avatar-optimizer")`が必要。かつAAOにコンポーネントを認識させる(Asset Description / 登録API)必要がある場合がある。

### バージョン組み合わせの推奨(2026年前半時点の安定構成)

| ツール | 安定版 | 備考 |
|---|---|---|
| NDMF | 1.14.x | Unity 6000系対応は1.14.0以降 |
| Modular Avatar | 1.17.x | 要NDMF ≥1.11(package.json: `>=1.11.0 <2.0.0-a`) |
| AAO | 1.9.x | 要NDMF `>=1.8.0 <2.0.0`、VRCSDK `>=3.7.0 <3.11.0`(1.9.13でVRCSDK 3.7.0互換を回復) |
| TexTransTool | 1.0.x | 要NDMF ≥1.7、TexTransCore 0.2.x |
| lilToon | 2.3.x | VRC Light Volumes同梱。1.10.xは2022系最終安定の旧系列 |
| Poiyomi | 9.3.x (LTS) | 10.0開発中。9.2系からの移行推奨 |
| VRCQuestTools | 2.11.x | 次期メジャーでVRCSDK<3.9 / lilToon<1.10 / NDMF<1.5 切り捨て予定(Unreleased) |

原則: **MA / NDMF / AAO はセットで最新安定版に揃える**。NDMFだけ古い・新しい状態は最も事故が多い(各ツールがNDMFのバージョンをVersion Definesで検知して機能を切り替えるため)。

---

## 3. 共通トラブルシュートフロー

### 3.1 ビルドエラー/意図しない出力が出たら最初に確認する項目

1. **NDMF Console**(旧Error Report)を開く(`Tools/NDM Framework/`)。エラーの発生プラグインとオブジェクト参照が表示される。日本語ロケール対応。
2. **バージョン整合**: VCCで NDMF / MA / AAO / TTT を全て最新安定版へ。片方だけの更新が原因のコンパイルエラー・API欠落が最頻出(例: NDMF 1.7.2の`IVirtualizedMotion`名前空間移動、NDMF 1.6.1の`IExtensionContext.Owner`削除)。
3. **コンパイルエラーの有無**: Consoleに赤エラーがある状態ではNDMF自体が動かない。VRCSDKなしプロジェクト・World SDK混在プロジェクトでのエラーは各ツールの既知問題(lilToon 1.8.2、MA 1.17.0 #1973等)を確認。
4. **Manual Bake**で出力を検査: アバター右クリック → `Modular Avatar/Manual Bake Avatar`(またはNDMFのManual Bake)。生成物を直接見ると、どのフェーズで壊れたか切り分けられる。
5. **プラグイン単位で無効化**: NDMF 1.7.0+の `Tools/NDM Framework/` にあるプラグイン有効/無効ウィンドウで二分探索する。`Apply on Build`のトグルも同メニューにある(1.5.4+)。
6. **Apply on Play切り分け**: Play時のみ壊れる場合はAv3Emulator/Gesture Managerとの相互作用を疑う。NDMFのApply on Playを切って比較。

### 3.2 症状別の定石

| 症状 | 疑うポイント |
|---|---|
| 服が体を貫通/ボーンが二重 | MA Merge Armatureの対象・プレフィックス設定、Armature名の不一致、Scale Adjusterの有無 |
| トグルが他人に見えない(ローカルのみ動作) | MA Parametersのsynced設定、パラメータ数超過(Int自動値0-255超過はMA 1.18+でビルドエラー化)、Sync Parameter Sequenceの設定不整合 |
| 表情/トグルがMMDワールドで壊れる | MA 1.12+のMMD対応(`MA VRChat Settings`で挙動制御、`MA MMD Layer Control`)。WD OFFレイヤーとの併用警告(1.14+) |
| テクスチャがずれる/デカール位置が違う | 実行順: FloorAdjuster等のTransform移動系がTTTより先に動いていないか(TTT 0.8.2/MA 1.17.1で対策済みだが、他のTransform移動系ツールは要確認) |
| アトラス化後に一部マテリアルが元に戻る | TTTのAtlasTextureとマテリアル差し替え(MA Material Setter/アニメーション)の順序。TTT 0.10+ / NDMF 1.7+ で対応済み。旧版は非対応 |
| ビルド後にメッシュが消える | AAOのRemove Mesh系/自動FreezeBlendShape(∞デルタは1.9.0で修正)、AAOのRemove Zero Sized Polygon(Advanced扱い)。AAO exclusionsに追加して切り分け |
| PhysBoneの挙動が変わった | AAOのMerge PhysBone / Auto Merge PhysBone / EndBone置換(1.9系で多数の修正あり→AAOを最新パッチへ) |
| アニメーションが効かない/プロパティが飛ぶ | パス書き換えの多重適用(NDMF 1.7.7で修正)、AAOのMergeBoneによるパス変更(ビルド前パスでアニメを作る)、lilToonの最適化でアニメ対象プロパティが定数化(lilToon 1.8.0+の外部ツールAPI/アニメーション考慮で改善) |
| アップロード時だけ壊れる(Playでは正常) | Optimizingフェーズ(AAO/TTT Atlas/VQT)以降の問題。Manual Bakeで確認。lilToonは「テストビルドでは最適化しない」オプションの差にも注意 |
| Questビルドで紫/真っ黒 | シェーダーがMobile非対応(lilToon/PoiyomiはQuest不可)→VQTで変換。テクスチャ形式エラーはVQTのCheckTextureFormatの警告を見る |
| プレビューが固まる/Sceneビューがおかしい | NDMFプレビューシステムの既知問題(1.9.4等で修正多数)。NDMFを最新へ、ダメならプレビューをオフ(NDMF Console/設定から) |

### 3.3 レポートを書く/調べるときに使う情報

- NDMFのビルドログ・プラグインシーケンス表示ウィンドウ(実行パス順が見える)
- AAO 1.9.0+の「Bug Report Helper」(`Tools/Avatar Optimizer/`): T&O設定のJSONダンプ含む収集機能
- lilToonのバグレポート生成: `GameObject/lilToon/[Debug] Generate bug report`
- 各ツールのGitHub Issues(既知問題の検索が最速): bdunderscore/modular-avatar, bdunderscore/ndmf, anatawa12/AvatarOptimizer, ReinaS-64892/TexTransTool, lilxyzw/lilToon, poiyomi/PoiyomiToonShader, kurotu/VRCQuestTools

---

## 4. 各ツール詳細への参照

改変ワークフロー順(01基盤 → 03着せる → 04-06見た目 → 07-08動き・メニュー → 09-10軽量化 → 11 Quest → 12検証):

- [01 NDMF](01-ndmf.md) — フレームワーク本体。全ツールの前提
- [02 Modular Avatar](02-modular-avatar.md) — 衣装結合・メニュー/パラメータ・リアクティブ
- [03 衣装フィッティング](03-fitting-tools.md) — もちふぃった～ / EreMorph / KiseteneEx(非対応衣装)
- [04 TexTransTool](04-textranstool.md) — テクスチャ改変・デカール・アトラス化
- [05 lilToon](05-liltoon.md) — シェーダー(日本圏標準)
- [06 Poiyomi Toon Shader](06-poiyomi.md) — シェーダー(英語圏標準)
- [07 表情・ギミック生成](07-expression-gimmick-tools.md) — FaceEmo / CGE / Light Limit Changer / FloorAdjuster等
- [08 周辺エコシステムツール](08-ecosystem-tools.md) — AvatarMenuCreatorForMA / lilycalInventory / Flare等、および順序制約に登場するNDMFプラグインID早見表
- [09 AAO: Avatar Optimizer](09-avatar-optimizer.md) — 自動最適化
- [10 軽量化・変換](10-optimization-conversion-tools.md) — ポリゴン削減・マテリアル変換
- [11 VRCQuestTools](11-vrcquesttools.md) — Quest/Android/iOS対応
- [12 検証・分析・アップロード・環境管理](12-analysis-upload-tools.md) — Gesture Manager / Av3Emulator / vrc-get等

リファレンス(13〜18):

- [13 VRChat/Avatars 3.0基礎](13-vrchat-avatars-basics.md) — Performance Rank公式テーブル / パラメータ256bit / サイズ制限 / フォールバック / コールバック順
- [14 VRCFury(防御的ナレッジ)](14-vrcfury.md) / [15 GoGo Loco](15-gogo-loco.md) / [16 ワールド連携(AudioLink/LTCGI/VRCLV)](16-world-integrations.md) / [17 Unity定番トラブル](17-unity-troubleshooting.md) / [18 ライセンス実務メモ](18-license-notes.md)


======================================================================
# FILE: 01-ndmf.md
======================================================================

# NDMF (Non-Destructive Modular Framework)

- リポジトリ: https://github.com/bdunderscore/ndmf
- ドキュメント: https://ndmf.nadena.dev
- パッケージ名: `nadena.dev.ndmf`
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
- **開発者向け付帯機能**: IAssetSaver(1.6.0+)、PropCache(1.10.0+)、ProfilerScope、プラットフォームAPI(1.8.0+ `[RunsOnPlatforms]`)

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

## 関連ツール

- 全NDMFプラグインの前提。特に [Modular Avatar](02-modular-avatar.md)(同一作者・同時リリースが多い)
- [AAO](09-avatar-optimizer.md) はNDMFのAnimatorServices/ObjectRegistry/プレビューを深く利用
- 非NDMFツール(VRCFury等)との順序関係は[ツール横断情報](00-cross-tool.md)§1.1

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


======================================================================
# FILE: 02-modular-avatar.md
======================================================================

# Modular Avatar (MA)

- リポジトリ: https://github.com/bdunderscore/modular-avatar
- ドキュメント: https://modular-avatar.nadena.dev (https://m-a.nadena.dev)
- パッケージ名: `nadena.dev.modular-avatar`
- 作者: bd_(NDMFと同一作者)

## 概要

**コンポーネントをドラッグ&ドロップで組み合わせてアバターを非破壊に組み立てる**ツールキット。衣装のアーマチュア統合、アニメーター/メニュー/パラメータの合成、リアクティブ(トグル)生成が中核。処理は全てNDMFプラグインとしてビルド時に実行される。

- Unity要件: 2022.3(1.13.x以前は2019系もサポートしていた時期あり。現行は2022専用)
- VRCSDK: Avatars 3.0前提の機能が多いが、**1.13.0以降は非VRChatプラットフォームを実験的サポート**(VRCSDKなしでもコンパイル可)。1.15.0で`com.vrchat.avatars`への強依存を除去
- 配布: VPM(vpm.nadena.dev)。**unitypackage配布は非推奨経路**

## 依存関係

- 必須: NDMF(1.17.xは `>=1.11.0 <2.0.0-a`)、`com.unity.nuget.newtonsoft-json`
- VRCSDK Avatars: オプショナル(`MA_VRCSDK3_AVATARS`デファインで機能が切り替わる)
- 衣装側アセットがMAコンポーネント入りプレハブを同梱する形での再配布を想定(MITライセンス)

## 対応する改変パターン

コンポーネント単位で列挙(ビルド時に全て解決され、MAコンポーネントは最終的に削除される):

### 衣装・オブジェクト結合
- **MA Merge Armature**: 衣装アーマチュアをアバター本体へ統合(ボーン名ヒューリスティックマッピング、Prefix/Suffix除去)。PhysBone付きヒューマノイドボーンの統合は条件付き可(1.12+)
- **MA Bone Proxy**: オブジェクトを指定ボーン配下へ移動(アクセサリ装着)。ターゲットに他のBone Proxy/Merge Armature配下も指定可(1.16+)、Match scaleオプション(1.17+)
- **MA Mesh Settings**: Anchor Override / Boundsをアバター単位で統一設定
- **MA Replace Object**: オブジェクトを別オブジェクトで置換(素体メッシュ差し替え等)
- **MA Move Independently**: 親子関係を保ったまま独立移動(編集時支援)
- **MA Scale Adjuster**: ボーンスケールの調整(ヒューマノイドボーン長も対応、1.15+)
- **MA World Fixed Object**: ワールド固定オブジェクト(1.12+でVRCParentConstraint使用→Android対応)
- **MA Visible Head Accessory**: 一人称視点で自分の頭部アクセサリを可視化
- **MA World Scale Object**: ワールドスケール追従(1.12+)

### アニメーター/表情
- **MA Merge Animator**: アニメーターコントローラーをPlayable Layer(FX等)へ統合。相対/絶対パスモード、Write Defaults一致オプション、**既存アニメーターの置換**(1.12+)
- **MA Merge Motion (Blend Tree)**: BlendTree/AnimationClipをDirect Blend Treeとして統合(旧名Merge Blend Tree、1.12で改名)
- **MA Blendshape Sync**: 服と素体のBlendShape値を同期(カーブリマップ対応は1.18+)
- **MA MMD Layer Control**: MMDワールド互換挙動のレイヤー単位制御(1.12+)
- **MA Convert Constraints**: Unity Constraint→VRC Constraintへビルド時変換(Quest対応にも有効)

### メニュー/パラメータ
- **MA Menu Installer / Menu Item / Menu Group**: Expressions Menuの合成・オブジェクトベースのメニュー編集。アイコン自動圧縮(iOS対応修正1.12+)
- **MA Parameters**: Expression Parametersの追加・自動リネーム(衝突回避)・値インポート
- **MA Rename Collision Tags**: Contact系のcollisionTagsを一意名へリネーム(1.13+)
- **MA Sync Parameter Sequence**: PC/Quest等プラットフォーム間でパラメータ順序を同期(1.16+でUnity Library配下に自動保存、プライマリ→セカンダリ同期に変更)
- **MA VRChat Settings**: MMD対応等のVRChat固有挙動の設定(1.12+)

### リアクティブコンポーネント(1.8系〜、ビルド時にアニメーター生成)
- **MA Object Toggle**: オブジェクトのON/OFFトグル
- **MA Shape Changer**: BlendShape値の変更/メッシュ削除(Delete mode)
- **MA Material Setter**: マテリアル差し替え
- **MA Material Swap**: マテリアル一括スワップ(1.13+)
- **Menu Item連動**: メニュー項目のトグルとリアクティブの自動接続、自動パラメータ割当

### メッシュ加工
- **MA Mesh Cutter**(1.14+): 頂点フィルタ(By Bone / By Blendshape / By Axis / By Mask / By UV Tile(1.18+))によるメッシュ部分削除・トグル
- **MA Remove Vertex Color**: 頂点カラー削除

### その他
- **MA Platform Filter**(1.13+): プラットフォーム別にオブジェクトを除外
- **MA Global Collider**(1.15+): VRC標準コライダーの付け替え
- **MA Floor Adjuster**(1.17+): 接地高さ調整(narazaka版FloorAdjusterのMA内蔵版)
- **MA PhysBone Blocker**: 子オブジェクトへのPhysBone影響を遮断
- **MA Fit Preview**(1.15+): 編集モードでPhysBone付きポーズプレビュー
- GCによる未使用オブジェクト削除(Optimizingフェーズ、アニメ参照は1.15+で保持)

## 改変時の注意点(ソース由来の癖)

- **パス順序**(`Editor/PluginDefinition/PluginDefinition.cs`): Rename Parameters → Merge Blend Tree → Merge Animator → Reactive Components → Menu Install → Merge Armature → Bone Proxy → … の順。**Menu InstallerはMerge Armatureより前**に処理される(Merge ArmatureがMenu Installer入りオブジェクトを破壊しうるため。ソースコメントにTODOあり)
- **Merge Armatureの前にBoneProxyPrepassがターゲットを解決**する。Merge Armatureで動く前提のBone Proxyも動作するが、1.16.0→1.16.1で「Merge Armature配下のBone Proxy」のリグレッションがあった(常に最新パッチ推奨)
- リアクティブコンポーネントは`ReadablePropertyExtension`必須のシーケンスで一括実行され、プレビューはShape Changer/Mesh Deleter/Object Switcher/Material Setterの4フィルタ
- **EditorOnlyタグのオブジェクト**: MAコンポーネントのみ除去し、オブジェクト自体はPlay Modeでは残す(`ClearEditorOnlyTags`)。ビルドではNDMF/VRChatが削除
- Merge AnimatorのWD一致ロジックは1.12系で頻繁に変化した(1.12.3/1.12.4/1.13.0)。単一ステートBlendTreeレイヤーはWD ONに強制される等。WD混在アセットの互換問題はまずMAのバージョンを疑う
- パラメータ自動リネームは1.12.0からオブジェクトパスベースの名前になった(Sync Parameter Sequence互換性向上。**更新時はSyncedParamsアセットを空にして全プラットフォーム再アップロード推奨**)
- 静的(常時ON)リアクティブの優先度がFXより下だったのは1.14.0でバグ修正 → 既存ギミックの挙動が変わる可能性が明記されている
- Shape ChangerのSet/Deleteの上書き規則は1.13で変わり1.14で再逆転(1.13.xの挙動は事故扱い)
- Int自動パラメータ値が0-255を超えるとビルドエラー(1.18+。以前は黙って範囲外を割り当て)
- Merge Armature中のヒューマノイドボーン+PhysBoneは「子ヒューマノイドボーンが全PhysBoneから除外されている場合」のみ統合可(1.12+)
- MAのGCはアニメーションから参照されるオブジェクトを削除しない(1.15+)。それ以前は削除されることがあった

## Quest対応時の注意

- MA自体はQuestビルドでも同様に動作する(プラットフォーム非依存の変換)
- **MA Platform Filter**でQuest専用/PC専用オブジェクトを出し分け可能
- **MA Sync Parameter Sequence**はPC/Quest間のパラメータ不一致(同期ずれ)対策の中核。1.16+でプラットフォーム間の内容一致を強制
- World Fixed Objectは1.12+でVRCParentConstraint実装になりAndroidビルド可
- メニューアイコン自動圧縮はiOSビルド対応(1.12.0で修正)
- VQTとの関係: VQTはMA処理後のアバターを変換する。`MA Visible Head Accessory`/`MA World Fixed Object`はMA 1.9+ならVQTに削除されない([VRCQuestTools](11-vrcquesttools.md)参照)

## 関連ファイルパス

| パス | 役割 |
|---|---|
| `package.json` | リポジトリルート=パッケージ本体(1.18系: vpmDependencies ndmf >=1.11) |
| `Editor/PluginDefinition/PluginDefinition.cs` | NDMFプラグイン定義(全パス順序の一次情報) |
| `Editor/`(各Processor/Hook) | Merge Armature等の実装(`MergeArmatureHook`, `MenuInstallHook`, `RenameParametersHook`等) |
| `Runtime/` | コンポーネント定義(`ModularAvatarMergeArmature`等、`AvatarTagComponent`派生) |
| `docs~/` | 公式ドキュメント(Docusaurus)ソース |
| `CHANGELOG.md` | 1.12.0以降の変更履歴(それ以前はGitHub Releases) |

## よくあるトラブル

- **服が飛ぶ/ボーンが吹き飛ぶ**: Merge Armatureのボーン対応失敗。プレフィックス設定、素体と衣装のボーン名、スケール(Scale Adjusterの併用)を確認
- **メニューが出ない**: Menu Installerのインストール先、パラメータ未定義(MA Parametersの自動リネームとの絡み)。パラメータがMA Parameters定義済みでもメニュー値が0になる問題は1.15.1で修正
- **MMDワールドで表情停止**: 1.12+のMMD処理を理解する(`MA VRChat Settings`で無効化可)。WD OFFステートのあるレイヤーへのMMD Layer Controlは警告(1.14+)。FXレイヤー1枚目が無効化される問題は1.18系で修正
- **Play Audio(Animator Play Audio)のパスずれ**: 相対/絶対パスの解決規則が1.12.5で確定(絶対パス設定+相対Merge時、対象不在なら絶対として扱う)
- **1.16.0でBone Proxy + Merge Armatureが壊れた**: 1.16.1で修正済み
- **Visible Head Accessoryでメッシュ破損**: 1.17系(#2019)で修正。頭部直下root boneのメッシュはHead Chop絡みの既知問題が多い(1.13.3/1.13.4)
- **ビルドは成功するがトグルが他人から見えない**: パラメータsynced設定とExpression Parameters容量超過を確認
- **「ボタン押しても消えない」「トグルが反応しない」**(ユーザー語彙): 自分にも効かない→Menu Itemのパラメータ名とリアクティブ/アニメの接続を確認(Setup Outfitやり直しが早い)。自分には効くが他人に見えない→上記synced/容量。問診は[19 §G](19-triage-guide.md)

## 関連ツール

- [NDMF](01-ndmf.md): 実行基盤(同一作者、同時更新推奨)
- [TexTransTool](04-textranstool.md): MAの後に実行。MA Material Setter/Swapで差し替えたマテリアルへもTTTが効く(TTT 0.10+)
- [AAO](09-avatar-optimizer.md): MAの生成物(DelayDisableレイヤー等)を認識して最適化
- [VRCQuestTools](11-vrcquesttools.md): MA処理後にQuest変換。Setup Avatar for MobileはMA Sync Parameter Sequence等を自動追加

## バージョン履歴

(CHANGELOG.mdは1.12.0以降。それ以前はGitHub Releases参照。日付はリリース日)

### 1.18(Unreleased→2026年後半見込み)
- 追加: Blendshape Syncのカーブリマップ、Vertex Filter By UV Tile、By Maskの代替UV、Sync Parameter Sequenceの誤設定警告、**Int自動値の0-255超過をビルドエラー化**
- 修正: Unity 6000.0互換、MMDワールドでFX第1レイヤー無効化、Merge Armatureのメモリデフラグ後の追跡喪失

### 1.17.x (2026-05)
- 1.17.0: **MA Floor Adjuster追加**、VRCSDKのメニューエディタハングをパッチ、Bone ProxyのMatch scale、`VRCRaycast`対応(VRCSDK 3.10.3+)
- 1.17.1: **Floor AdjusterをTTT等の後に実行するよう順序変更**(late-transform-stagesプラグイン新設。互換性重要)

### 1.16.x (2026-02)
- 1.16.0: Bone Proxyのターゲット拡張、**Sync Parameter SequenceがUnity Library保存+プライマリ→セカンダリ同期に変更**(パラメータアセット不要化)、TextMeshProポストプロセッサ抑止(ビルド高速化)、MMDワールド関連修正多数
- 1.16.1: **Merge Animator「Match Avatar Write Defaults」既定ON化**。Bone Proxy+Merge Armatureリグレッション修正
- 1.16.2: 非定数カーブ検査の誤検知修正

### 1.15.x (2025-12)
- 1.15.0: **MA Fit Preview**、**MA Global Collider**、`com.vrchat.avatars`依存除去、VRCFury<1.1250.0+Mesh Cutter/Shape Changer(delete)の非互換警告、GCがアニメ参照オブジェクトを保持するよう変更
- 1.15.1: MA Parameters定義パラメータのメニュー値0問題、非ヒューマノイドでのScale Adjusterエラー修正

### 1.14.x (2025-09)
- 1.14.0: **MA Mesh Cutter追加**(頂点フィルタBy Bone/Blendshape/Axis/Mask)、`GetBonesMapping` API公開。**静的リアクティブの優先度バグ修正(挙動変化の可能性明記)**、Shape ChangerのSet/Delete上書き規則を1.12挙動へ復帰、Head Chop過剰生成修正、リアクティブ初期状態を非VRChatプラットフォームでも適用
- 1.14.1〜1.14.3: genericアバターアップロード失敗、パラメータFloat化時のDriver挙動、Vertex Filter By Axisのプレビュー修正

### 1.13.x (2025-07〜08)
- 1.13.0: **非VRCプラットフォーム実験的対応**、**MA Material Swap**、**MA Platform Filter**、**MA Rename Collision Tags**、Shape Changerの完全削除化(アニメ中でも)、`ModularAvatarMenuItem`のVRCSDK非依存API(旧APIは2.0で削除予告)
- 1.13.1: Blendshape Syncのシーン常時更新/安全設定ブロック時の削除形状未適用修正。1.13.2: Shape Changerによる特定ワールドでのVRChatクラッシュ修正。1.13.3/1.13.4: 頭部配下root boneのfirst person不可視問題修正

### 1.12.x (2025-04)
- 1.12.0: CHANGELOG導入。**Merge Animatorの既存アニメーター置換**、World Scale Object、MA MMD Layer Control、**MMDワールド互換処理の導入**(Merge Blend Tree/リアクティブとの互換修正、MA VRChat Settingsで無効化可)、**Merge Blend Tree→Merge Motionへ改名**(クリップ統合対応)、新NDMF API(IVirtualizeMotion等)採用、**パラメータ自動リネームがパスベースに**(Sync Parameter Sequence利用者は再アップロード推奨)、World FixedのVRCParentConstraint化(Android対応)、アイコン圧縮のiOS対応
- 1.12.1〜1.12.5: 新規プロジェクトでのコンパイルエラー、Merge Motion+Rename Parameters、WD調整の互換修正、NDMF連動修正(重複レイヤー等)。**1.12.5: Play Audioの絶対/相対パス解決規則確定**

### 1.11以前(GitHub Releases。主要トピックのみ)
- 1.11 (2025-02): Merge Animator相対パスモードの内部改善、NDMF 1.6系対応
- 1.10 (2024-12): NDMF AnimatorServices移行準備、安定性改善
- 1.9 (2024-09): NDMFプレビュー対応強化(Shape Changer等)、VQTがVHA/WFOを削除しなくなる基準バージョン
- 1.8 (2024-06): **リアクティブコンポーネント(Object Toggle / Shape Changer / Material Setter)導入**
- 1.7以前 (2023〜2024-05): NDMF移行(1.6で完了)、Merge Armature/Menu/Parametersの基礎確立

### 推奨組み合わせ
- MA 1.17.x + NDMF 1.13〜1.14 + AAO 1.9.x + TTT 1.0.x(2026年前半の標準構成)
- MAとNDMFは同一作者のため**同時に更新**するのが原則。MAのみ更新してNDMFが古いとコンパイルエラーになりうる(vpmDependenciesで下限は保証されるがVCC以外の導入経路では要注意)


======================================================================
# FILE: 03-fitting-tools.md
======================================================================

# 衣装フィッティングツール(非対応衣装の着せ替え)

「対応衣装を着せる」だけなら[Modular Avatar](02-modular-avatar.md)(Merge Armature)で完結する。このページは**非対応衣装**(別アバター向けに作られた衣装)を着せるためのツール群を扱う。

> 情報源の注記: もちふぃった～/EreMorphは**有償ツールでソース非公開**のため、本ページはBooth公式ページ・公式ドキュメント・コミュニティ解説(2026年前半時点)に基づく。主要7ツールのようなソースコード裏付けはない点に留意。仕様は更新で変わりやすいので、作業前にBoothの更新履歴を確認すること。

## 目的別早見表

| 状況 | 使うもの |
|---|---|
| 対応衣装を着せる | [MA](02-modular-avatar.md)のみ(Merge Armature) |
| **非対応衣装を体型に合わせて変換して着せる** | **もちふぃった～**(+プロファイル) |
| 変換後の貫通・形状崩れを微修正 | EreMorph(非破壊)/ Blender(破壊的) |
| 衣装の位置・スケールの手動微調整 | KiseteneEx、MA Scale Adjuster等 |

---

## もちふぃった～(MochiFitter)

- 作者: おもちのびる(Nine Gates)
- 配布: https://booth.pm/ja/items/7657840(有償: 2,500円)
- 種別: Unityエディタ拡張(`Tools/MochiFitter`)

### 概要

**別アバター向けの衣装を、対象アバターの体型に合わせて変換(リフィット)する**ツール。2025年末〜2026年にかけて「アバター改変の革命」として急速に普及した。従来Blenderで手作業していた非対応衣装の着せ付けを、Unity内でほぼ自動化する。

### 仕組み(プロファイル方式)

変換には3つの材料が必要:

1. **変換元アバターのプロファイル**(衣装が本来対応しているアバター)
2. **変換先アバターのプロファイル**(着せたいアバター)
3. **変換先アバター本体**

プロファイルは「アバターごとの変換に必要な情報(体型・ベースポーズ等)を含んだファイル群」で、以下から入手する:

- **本体同梱**: Nine Gates「ベリル」、ポンデロニウム研究所「しなの」「桔梗」、STUDIO JINGO「マヌカ」(同梱内容は更新で変わるためBoothページで確認)
- **アバター販売ショップの配布**: 対応が進んでおり、Boothで「もちふぃった」で検索
- **自作**: プロファイル作成機能あり。ただし**ベースポーズの保存ミスや設定ファイルの誤りで変換が壊れる**ことがコミュニティで多数報告されており、作成後のテスト変換が必須とされる

### 導入と使い方の流れ

1. Boothから購入・展開し、unitypackageをプロジェクトにインポート(使用するプロファイルも併せて)
2. `Tools/MochiFitter`でウィンドウを開き、初回は**「Download & Install」で追加コンポーネントを取得**(ネットワーク接続が必要)
3. 変換元/変換先プロファイルと衣装を指定して変換を実行
4. 変換された衣装を通常の対応衣装と同様に**MA(Merge Armature)で着せる**
5. 貫通・形状崩れが残る箇所はEreMorph(下記)やBlendShapeで微修正

### 注意点

- **変換は破壊的(新しいメッシュ/プレハブの生成)**。非破壊なのは着せ付け以降(MA)と修正(EreMorph)の部分。元衣装のプレハブは保持しておく
- **衣装のアニメーション・複雑なギミックは変換後に動かなくなる可能性**(公式が明記)。PhysBone類は概ね引き継がれるが、衣装専用ギミックは要検証
- 素体より衣装のメッシュ密度が低いと貫通しやすい → **メッシュ自動細分化オプション**で対処可能(ポリゴン数は増える)
- 変換品質は**プロファイルの品質に強く依存**。結果がおかしい場合はまずプロファイル側(ベースポーズ・設定)を疑う
- ライセンス: 変換した衣装の扱いは元衣装の利用規約に従う(改変物の扱い)。プロファイル配布の可否もアバター側規約に依存

### Quest対応時の注意

- 変換後の衣装は通常の衣装と同じ扱い。細分化オプションでポリゴンが増えている場合があるため、[AAO](09-avatar-optimizer.md)/[Meshia等](10-optimization-conversion-tools.md)での削減と[VQT](11-vrcquesttools.md)変換を通常どおり適用する

### 関連ツール

- [Modular Avatar](02-modular-avatar.md): 変換後の着せ付け
- EreMorph(下記): 変換後の貫通修正
- [AAO](09-avatar-optimizer.md)/[TTT](04-textranstool.md): 変換・着せ付け後の最適化

---

## EreMorph(えれもーふ)

- 作者: えれのあ(えれにゃんこどっとしょっぷ)
- 配布: https://booth.pm/ja/items/7552621(有償)
- 公式ドキュメント: https://eremorph.erenoa.com/

### 概要

**Unity上で直接メッシュを編集できるツール**。「Blender不要で貫通をつまんで直す」が売り。もちふぃった～変換後の仕上げとして定番の組み合わせ。

### できること

- 減衰(フォールオフ)付きの頂点移動、対称編集
- 編集結果を**メッシュとして出力 or BlendShapeとして出力**
- **NDMF対応 = 非破壊適用が可能**(元メッシュを書き換えずビルド時に反映)
- 既存シェイプキーのインポート、法線再計算、頂点ウェイトのリマップ
- Mesh / SkinnedMesh両対応。汎用髪・アクセサリの位置/カーブ調整、ワールド用オブジェクト調整にも使える

### 注意点

- NDMF対応ツールなので実行順序の文脈に乗る(順序制約の詳細は公式ドキュメント参照。本KBでは未検証)
- BlendShape出力を選べば「着衣時のみ形状変更」のような使い方ができ、[MA Shape Changer](02-modular-avatar.md)等との連携がしやすい

---

## KiseteneEx(Satania)

衣装の位置・回転・スケールをボーン単位で調整する老舗の着せ付け補助ツール。**AAOが1.5.4で明示的に互換対応を追加**した実績があり(KB [AAO](09-avatar-optimizer.md)バージョン履歴)、NDMFエコシステムでの使用実績が長い。もちふぃった～登場後は「変換まではいらない、位置合わせだけしたい」ケース向け。

---

## 使い分けの整理

1. **対応衣装** → MAで着せるだけ(このページの出番なし)
2. **非対応衣装(プロファイルあり)** → もちふぃった～で変換 → MAで着せる → EreMorphで微修正
3. **非対応衣装(プロファイルなし)** → プロファイル自作(テスト必須)or 従来手法(Blenderリトポ/手動フィッティング、KiseteneEx)
4. 仕上げの最適化はいつも通り: [AAO](09-avatar-optimizer.md) → 必要なら[ポリゴン削減](10-optimization-conversion-tools.md) → [Quest対応](11-vrcquesttools.md)


======================================================================
# FILE: 04-textranstool.md
======================================================================

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

- [AAO](09-avatar-optimizer.md): 双方向連携(UV/RemoveMesh領域のネゴシエーション)。実行はTTTが先
- [Modular Avatar](02-modular-avatar.md): MAの後に実行。MA由来のマテリアル差し替えを認識
- [lilToon](05-liltoon.md): AtlasTextureのプロパティベイク対応シェーダー
- [VRCQuestTools](11-vrcquesttools.md): TTTの生成テクスチャを変換対象にするため常にTTT後

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


======================================================================
# FILE: 05-liltoon.md
======================================================================

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


======================================================================
# FILE: 06-poiyomi.md
======================================================================

# Poiyomi Toon Shader

- リポジトリ: https://github.com/poiyomi/PoiyomiToonShader
- ドキュメント: https://www.poiyomi.com (ソース: https://github.com/poiyomi/PoiyomiDocs)
- 変更履歴: https://www.poiyomi.com/changelog
- パッケージ名: `com.poiyomi.toon`
- 作者: Poiyomi

## 概要

英語圏のVRChatアバターで標準的な**多機能トゥーンシェーダー**。lilToonと双璧。無料版(Toon)とPatreon限定のPro版(`com.poiyomi.pro`)がある。**ThryEditor**ベースの高機能インスペクタと、**シェーダーロック(Lock/Optimize)機構**が最大の特徴。

- Unity要件: package.jsonの表記は`2019.4`だが、**changelog上は9.2.43でUnity 2019サポートを終了**(2021.3/2022.3推奨)。マニフェストと実態に齟齬があり、実態(changelog)が正
- VRCSDK: 不要(VRChat向け機能はVRCSDK検知で有効化)
- 配布: VPM/unitypackage/GitHub。`legacyFolders: Assets\_PoiyomiShaders`(旧配置からの移行定義)

## 依存関係

- 必須依存なし。ThryEditor(インスペクタ/オプティマイザ)はパッケージに同梱
- オプション連携: AudioLink、LTCGI、GrabPass系機能(PC専用)、VRChatフォールバック設定
- Pro版はToon版の上位互換(同一マテリアルデータ)。`legacyPackages: com.poiyomi.pro`(Toon側から見た移行定義)

## 対応する改変パターン

- **色調**: メインカラー、色調補正(HSV等)、グラデーション、AudioLink連動
- **レイヤー**: デカール(複数)、詳細テクスチャ、UVモード多数(パノスフィア等)
- **陰影**: 複数ライティングモード(Toon/Realistic系)、シャドウマスク、AO、SSS風表現
- **発光**: Emission複数スロット(0-3)、AudioLink、Glow in the Dark
- **質感**: MatCap複数、Metallic/Smoothness、Clear Coat、Anisotropy、Glitter/Sparkle、Iridescence
- **輪郭/縁**: Outline(Z Offset制御。旧Cam Z Offsetは廃止・**破壊的変更**)、Rim Lighting
- **特殊効果**: Dissolve、Flipbook、Video、Mirror検知、Touch/Distance反応、Grabpass Blur(Pro)
- **VRChat固有**: シェーダーフォールバック設定、**Lock機構によるビルド時最適化**(下記)
- **移行**: lilToon→Poiyomiのマテリアル変換機能(9.3.64+)

## 改変時の注意点(実装上の癖)

- **ロック(Lock In)機構が中核**: ThryEditorのShader Optimizerが、マテリアルごとに未使用機能を除去した専用シェーダーを生成する(プロパティは定数化)。**アップロード時に自動ロック**(Lock Material on Upload)されるため、「エディタで動くがアップロードで壊れる」問題の多くはロックが原因
- **アニメーションするプロパティは事前に「Animated」指定が必須**: ロック時、Animated指定のないプロパティは定数化されアニメーションが効かなくなる。さらに「Rename Animated」を使うとプロパティ名がマテリアル固有名にリネームされるため、**アニメーションクリップ側のプロパティ名も一致させる必要がある**。ギミック改変で最頻出の落とし穴
- ロック済みマテリアルのシェーダーは生成物(プロジェクト内に出力)なので、**ロック状態のままマテリアル設定を変更しても反映されない**。改変時はアンロック→編集→再ロック
- バージョン更新はマテリアルの自動アップグレードを伴う。**大型更新(7.3→8.x→9.x)ではマテリアルの見た目が変わることがある**ため、製品アバター付属のバージョンから安易に上げない・混在させない(シェーダー名が`.poiyomi/Poiyomi 8.1/...`のようにバージョンを含むため、複数バージョン共存は可能だが管理が煩雑)
- Pro版とToon版の混在: 同一機能でもシェーダーパスが異なる。配布アバターを扱う際はどちらでセットアップされたか確認
- AAOとの関係: AAOのOptimize Texture(アトラス化)は**Poiyomi非対応**(lilToon/Toon Standardのみ)。未使用プロパティ削除はロック済みシェーダーに対しても安全に動作する
- TexTransToolのAtlasTextureにはPoiyomi専用のプロパティベイク対応がない(汎用`_MainTex`処理のみ)。色調補正やデカールをテクスチャに焼き込む用途はロック機構側で行うか手動ベイク

## Quest対応時の注意

- **PoiyomiはQuest(Android/iOS)のVRChatアバターでは使用不可**(モバイル向けホワイトリスト外)
- [VRCQuestTools](11-vrcquesttools.md)のPoiyomi→Toon Lit変換が利用可能(対応: メインカラー、ノーマルマップ由来の陰影、Emission 0-3。VQT 2.0.0+)。Poiyomi→Toon Standard変換はVQT開発版の実験的機能
- Poiyomi→ToonLit変換のEmission不具合はVQT 2.11.7、メインテクスチャのUVタイリング保持はVQT 2.11.6で修正(VQT側を最新に)
- GrabPass系機能(Blur等)はQuest以前にPC以外で全滅する点に注意(ワールド制約)

## 関連ファイルパス

| パス | 役割 |
|---|---|
| `package.json` | パッケージ定義(リポジトリルート=`com.poiyomi.toon`) |
| `Shaders/`配下 | シェーダー本体(バージョン名を含むシェーダーパス) |
| `Scripts/`(ThryEditor同梱) | インスペクタ・Shader Optimizer(ロック機構)・アップロード時フック |
| 生成物: プロジェクト内のOptimizedShaders系フォルダ | ロック時に生成される最適化済みシェーダー |
| https://github.com/poiyomi/PoiyomiDocs | ドキュメントソース(Docusaurus) |

(注: リポジトリ内の正確なフォルダ構成はバージョンで変動が大きい。上記は役割ベースの整理)

## よくあるトラブル

- **アップロード後にアニメーションが効かない**: プロパティのAnimated未指定+自動ロック。該当プロパティをAnimatedにして再アップロード
- **Renameしたのにアニメが効かない**: Rename Animated使用時はアニメーションクリップのプロパティパスがマテリアル固有名(`_Prop_MaterialName`形式)になっているか確認
- **ロック時にシェーダーコンパイルエラー**: 機能の組み合わせ依存。まずPoiyomiを同一メジャーの最新へ。ロックを全解除→一括再ロック(Thryのツールメニュー)で直ることが多い
- **見た目が急に変わった**: バージョン更新による自動マテリアルアップグレード、またはOutline Z Offset等の破壊的変更(9.2系)。changelog確認
- **他人の環境で真っピンク**: シェーダー未導入またはバージョン違い。配布時はロック済みシェーダー同梱かバージョン明記
- **AAOで最適化したらPoiyomiだけアトラス化されない**: 仕様(AAOのOptimize TextureはPoiyomi非対応)

## 関連ツール

- [VRCQuestTools](11-vrcquesttools.md): Quest変換(→Toon Lit / 実験的にToon Standard)
- [AAO](09-avatar-optimizer.md): 未使用プロパティ削除は有効、テクスチャアトラス化は非対応
- [lilToon](05-liltoon.md): 相互移行(lilToon→Poiyomi変換は9.3.64+)
- ThryEditor(同梱): ロック機構の実体

## バージョン履歴

一次情報はwww.poiyomi.com/changelog(GitHub Releasesはミラー)。リポジトリ内にCHANGELOG.mdは無い。以下は要点(★=確度高、☆=changelog個別確認推奨):

### 9.3.x — 現行LTS (2025-11〜)
- ★ 9.3系は**LTS**(10.0開発中のため最終安定系列)。9.3.64 (2026-01): lilToon→Poiyomi変換にFakeShadow対応追加
- ★ 現行バージョン: 9.3.64(package.json確認値)

### 9.2.x (2025)
- ★ **9.2.43でUnity 2019サポート終了**(2021.3/2022.3推奨)
- ★ **破壊的変更: OutlinesのCam Z OffsetをOutline Z Offsetへ刷新**(計算式変更。既存マテリアルのアウトライン見た目に影響)
- 9.2.74 (2025-09) が9.2系の後期安定版(Pro 9.3.44と対応)

### 9.0〜9.1 (2024〜2025)
- ☆ 8.x系からの大型更新。ライティング/機能の再編とマテリアルアップグレードを伴う。詳細は公式changelogの各項を参照

### 8.x (2022〜2024)
- ☆ 8.0で7.3系から大規模刷新(マテリアルアップグレード必須)。8.1が長期の主流版で、多くの配布アバターが同梱。VQT 2.0.0(2024-01)のPoiyomi変換はこの世代を対象に実装
- ★ Pro版がPatreon配布として並行提供(`com.poiyomi.pro`)

### 7.3.x (2021) — レガシー
- ★ `_PoiyomiShaders`(Assets配置)時代の広く出回った無料版。7.3.50同梱の配布アバターが今も多い。**7.3→8+はマテリアル互換がなく、変換ツール経由の移行が必要**

### 10.0 (開発中)
- ★ 2026年時点で開発中とアナウンス

### 推奨組み合わせ
- Poiyomi 9.3.x (LTS) + Unity 2022.3 + VQT 2.11.x(Quest変換する場合)
- 配布アバターが7.3/8.x同梱の場合: そのバージョンのまま使うか、意図を持って9.xへ移行(見た目検証必須)。プロジェクト内に複数メジャーの共存は可能だが、同一マテリアルへの適用バージョンは統一する


======================================================================
# FILE: 07-expression-gimmick-tools.md
======================================================================

# 表情・ギミック生成ツール

表情システム・定番ギミックを生成するツール群。メニュー/トグル生成の汎用ツール(AvatarMenuCreatorForMA / lilycalInventory / Flare)は[08-ecosystem-tools.md](08-ecosystem-tools.md)を参照。

> 情報源: 各リポジトリのREADME/package.json(2026年前半時点)。

## 目的別早見表

| 目的 | ツール |
|---|---|
| ジェスチャー×メニューの表情システムを丸ごと構築 | FaceEmo |
| ジェスチャー表情+Contacts/PhysBone/OSC連動(英語圏定番) | ComboGestureExpressions |
| アバターの明るさ調整メニューを自動生成 | Light Limit Changer |
| アバターの接地高さを非破壊調整 | FloorAdjuster(またはMA 1.17+内蔵) |
| Playable Layer/パラメータの手動マージ(MA不使用時) | VRLabs Avatars 3.0 Manager |

---

## FaceEmo(suzuryg)

- リポジトリ: https://github.com/suzuryg/face-emo
- ドキュメント: https://suzuryg.github.io/face-emo/
- パッケージ名: `jp.suzuryg.face-emo` / 1.7.x(Unity 2022.3)

**表情作成・設定の統合ツール**(日本圏で最も普及した表情システムの一つ)。ハンドジェスチャーとExpression Menuを組み合わせた表情切り替えを、専用GUIで表情パターン(モード)単位に構築する。

- できること: ジェスチャー組み合わせへの表情割り当て、メニューからの表情モード切替/表情固定、まばたき・リップシンク干渉の制御、表情プレビュー、既存表情アニメの取り込み
- **Modular Avatarと組み合わせて非破壊にFXへ統合する構成が標準**(生成物はMA経由でマージされる)
- 注意: 表情はFXレイヤーを大きく占有するため、[AAO](09-avatar-optimizer.md)のOptimize Animatorとの併用で最適化はAAO側に任せる。MMDワールド互換はMA側の仕組み(KB [02](02-modular-avatar.md))に依存

## ComboGestureExpressions(Haï)

- リポジトリ: https://github.com/hai-vr/combo-gesture-expressions-av3
- ドキュメント: https://docs.hai-vr.dev/docs/products/combo-gesture-expressions

英語圏定番の表情ツール。ジェスチャー組み合わせに表情を割り当てるのに加え、**Contacts / PhysBones / OSC / コントローラートリガー圧**で表情をブレンドできるのが特徴。

- 「まばたき付き表情で目を閉じたときの瞬き干渉防止」「トリガー圧アニメーションの外部視点補正(corrections)」など、細部の作り込みが強み
- V3でVCC対応。Animator As Code(同作者)の系譜で、生成されるレイヤーは高度なblend tree構成
- FaceEmoとの使い分け: 日本語UI・モード管理重視ならFaceEmo、Avatar Dynamics連動やアナログ表現重視ならCGE

## Light Limit Changer(Azukimochi)

- リポジトリ: https://github.com/Azukimochi/LightLimitChangerForMA
- ドキュメント: https://azukimochi.github.io/LLC-Docs/
- パッケージ名: `io.github.azukimochi.light-limit-changer` / 1.14.x
- NDMFプラグインID: `io.github.azukimochi.light-limit-changer`

**シェーダーの明るさ下限/上限を操作するアニメーションとメニューを自動生成**するツール。ワールドの照明が暗すぎる/明るすぎる場合にユーザー自身がメニューで調整できるようにする、日本圏でほぼ標準装備のギミック。

- 対応シェーダー: **lilToon / Poiyomi / Sunao**(現行)
- 依存: VRCSDK >=3.2、**Modular Avatar ^1.9.9、NDMF ^1.4**。MAプレハブ形式で生成されるため非破壊でFXを直接変更しない
- **実行順序(重要)**: TTTがLLCより先に実行される制約を宣言している(`BeforePlugin`)。テクスチャ/マテラルを差し替えるツールとの順序問題が既知のため、LLC関連の明るさ異常はまずツール群のバージョンを最新に揃える
- Poiyomi使用時の注意: 明るさ系プロパティがロックでAnimated指定されている必要がある(LLCのセットアップが面倒を見るが、手動ロック運用時はKB [Poiyomi](06-poiyomi.md)の原則が適用される)

## FloorAdjuster(narazaka)

- リポジトリ: https://github.com/Narazaka/FloorAdjuster
- パッケージ名: `net.narazaka.vrchat.floor-adjuster`
- NDMFプラグインID: `net.narazaka.vrchat.floor_adjuster`

**アバターの上下位置(接地)を非破壊に調整**するツール。ヒール靴などで浮く/沈むアバターの定番修正手段。MA互換。現行は「by skeleton(新方式)」セットアップが推奨。

- **実行順序の履歴(重要)**: Transformを動かすため、TTT(デカール位置)と衝突した過去がある → TTT 0.8.2で「TTTがFloorAdjusterより先に実行」に修正済み
- **MA 1.17.0以降はMA本体に`MA Floor Adjuster`が内蔵**され、MA側はTTT等の後に実行するよう順序制御されている(KB [02](02-modular-avatar.md))。新規はMA内蔵版、既存アバターはnarazaka版が残っていても問題ない(両方入れるのは避ける)

## Avatars 3.0 Manager(VRLabs)

- リポジトリ: https://github.com/VRLabs/Avatars-3.0-Manager
- パッケージ名: `dev.vrlabs.av3manager`

**Playable Layerとパラメータを手動マージ**する老舗ツール。「FXレイヤーに別のコントローラーの中身を統合する」「パラメータをコピーする」操作をGUIで行う。

- 位置づけ: MA以前の標準。**MAのMerge Animatorが同じ役割を非破壊で担う**ため、現在はMA非対応プレハブの組み込みや、非破壊化されていない旧式ギミックの導入で使う
- 破壊的(コントローラーアセットを直接編集)なので、実行前にFXの複製を取るのが定石
- VRLabsは他にも多数のギミックプレハブ(World Constraint系等)を配布しており、その導入手段としてもA3Mが前提になっていることがある(近年のVRLabsプレハブはVCC/MA対応が進行)

---

## 有償定番ギミックのメモ(順序制約に登場するもの)

本KBの主要ツールのソースに順序制約として名前が出る有償/外部ギミック。導入時は各ツールの互換情報を確認:

| 製品 | NDMFプラグインID | 判明している順序関係 |
|---|---|---|
| VirtualLens2(logilabo) — カメラギミック | `dev.logilabo.virtuallens2.apply-non-destructive` | VQTがResolvingで先に設定を構成(VQT宣言) |
| ましゅまろPB(わたあめ屋) — PhysBone変形ギミック | `wataameya.marshmallow_PB.ndmf` | MAのlateステージ(Floor Adjuster等)より前(MA宣言) |
| ポージングシステム(ゆにさきスタジオ) | `jp.unisakistudio.posingsystemeditor.posingsystemconverter` | VQT変換より前(VQT宣言) |

## よくあるトラブル(表情まわり)

ユーザーの言い方: 「表情バグった」「笑うと目が変」「瞬きできない」「顔が動かない」(問診は[19 §D](19-triage-guide.md))

- **表情が混ざる・特定の表情で目が壊れる**: **表情システムの二重導入**(アバター元来の表情+後入れのFaceEmo/CGE等)が最頻出。どちらか一方に統一する
- **瞬きしなくなった**: ①表情メニューの固定解除 ②Avatar DescriptorのEyelids設定(改変で顔メッシュ名/シェイプキー名を変えると壊れる) ③表情アニメがまばたきシェイプを常時上書き ④AAOの自動凍結(Exclusionsで切り分け)の順で確認([13](13-vrchat-avatars-basics.md)/[09](09-avatar-optimizer.md))
- **MMDワールドで表情停止**: 壊れていない可能性が高い(仕様に近い)。[02のMMD対応](02-modular-avatar.md)参照
- **表情ツール導入後に別のギミックが壊れた**: FXレイヤー構成の変更が原因のことが多い。導入ツールを一時無効化して切り分け

## 関連ページ

- メニュー/トグル生成(AMCFMA / lilycalInventory / Flare)と**トゥイーン・フェード比較表**: [08-ecosystem-tools.md](08-ecosystem-tools.md)
- 検証・分析・アップロード: [12-analysis-upload-tools.md](12-analysis-upload-tools.md)
- 軽量化・変換: [10-optimization-conversion-tools.md](10-optimization-conversion-tools.md)


======================================================================
# FILE: 08-ecosystem-tools.md
======================================================================

# 周辺エコシステムツール(トグル/メニュー生成・その他NDMFプラグイン)

主要7ツール([index](index.md)参照)以外で、アバター改変の現場によく登場するNDMF系ツールのナレッジ。特に「メニュー/トグル自動生成」系と、実行順序制約に頻出するプラグインを扱う。

> 情報源: 各リポジトリのREADME/CHANGELOG(2026年前半時点)、公式ドキュメント、および主要7ツールのプラグイン定義ソースに現れる順序制約。

---

## トゥイーン/フェード機能の有無(調査結論の早見表)

「開始値・終了値・秒数を指定してアニメーションを自動生成する」機能の有無(2026-07時点):

| ツール | 値+秒数のフェード | イージング | メニュー統合 | 備考 |
|---|---|---|---|---|
| AvatarMenuCreatorForMA | ○「徐々に変化」(v1.2.0〜) | ×(線形のみ) | MAコンポーネント生成 | 変化待機/変化時間を項目ごとに%指定可 |
| Flare | ○「Interpolate」(秒数) | ×(線形のみ) | 独自メニューシステム | 公式がディゾルブ用途を明記 |
| lilycalInventory | ×(SmoothChangerは手動無段階) | - | 独自(LI)コンポーネント | Radial Puppetでフレーム間補間 |
| MAリアクティブ | ×(瞬間切替のみ) | - | MAネイティブ | Object Toggle/Shape Changer/Material Setter/Swap |
| VRCFury Toggle | ○ Transition Time | - | 独自 | 非NDMF。順序制御不能(KB 00 §1.1) |
| AAC(コード) | 自由に実装可 | 自由 | - | ノーコードではない |

---

## AvatarMenuCreatorForMA(narazaka)

- リポジトリ: https://github.com/Narazaka/AvatarMenuCreaterForMA
- ドキュメント: https://avatar-menu-creator-for-ma.vrchat.narazaka.net
- パッケージ名: `net.narazaka.vrchat.avatar-menu-creater-for-ma`(VPM: vpm.narazaka.net)

### 概要
Modular Avatarでアバターメニューを構成するための補助ツール。「メニュー1項目=MAの1コンポーネント(または1プレハブ)」として生成する。NDMF(MA 1.8.0以降)による非破壊動作に対応(MA 1.7.7以前はプレハブ生成のみ)。

### 対応する改変パターン
- **ON/OFFメニュー(Bool Toggle)**: オブジェクト・BlendShape・シェーダーパラメータのトグル
- **選択式メニュー(Int複数Toggle)**: 上記+マテリアル差し替えの選択制御
- **無段階調整(Float Radial Puppet)**: BlendShape・シェーダーパラメータ
- **「徐々に変化」(v1.2.0〜)**: ON/OFFメニューに遷移秒数を指定してフェード生成。項目ごとに「変化待機」「変化時間」(%)を設定でき、順次演出が可能。GameObjectにも変化待機を設定可(v1.37+、MAリアクティブ連携用)
- 複数メニューの一括生成、設定値の抽出/書き戻し、AvatarParametersDriver連携

### 注意点・既知の癖
- 「徐々に変化」の補間は**線形固定**(v1.11.0で「線形に値が推移するように」修正)。イージングは不可
- 遷移時間指定+デフォルト以外の変数状態でのロード時に遷移アニメが再生される問題はv1.6.1で修正(古いバージョンに注意)
- Poiyomiロック済みプロパティ名の解決支援は無い(実プロパティ名を自分で指定する)
- アクティブに更新が続いている(v1.38系、Unity 2022対応、VRCSDK 3.9系対応)

### 関連ツール
[Modular Avatar](02-modular-avatar.md)(生成対象)、lilycalInventory(同ジャンルの代替)

---

## lilycalInventory(lilxyzw)

- リポジトリ: https://github.com/lilxyzw/lilycalInventory
- ドキュメント: https://lilxyzw.github.io/lilycalInventory/
- パッケージ名: `jp.lilxyzw.lilycalinventory`
- NDMFプラグインID: `jp.lilxyzw.lilycalinventory`(TTT/VQTの順序制約に登場)

### 概要
lilToon作者によるノーコードのメニュー/トグル生成ツール。コンポーネントを付けるだけでビルド時にアニメーション・メニュー・パラメータを自動生成する(追加アセットを生成しない)。

### 主要コンポーネント
- **LI Prop / LI ItemToggler**: オブジェクトのON/OFFトグル(Bool)
- **LI CostumeChanger**: Int値による衣装切替(登録衣装をAnimationClip+Animator Stateに変換、瞬間切替)。AutoDresser統合(1.1.0+)
- **LI SmoothChanger**: Float+Radial Puppetの無段階調整。登録フレーム間をBlendTreeで補間する**手動操作方式**(秒数指定の自動トゥイーンではない)
- **LI AutoDresser**: 着ている衣装の自動検出切替
- **LI Preset**(1.1.0+)、**LI MenuFolder**、**LI AutoFixMeshSettings**(1.1.0+、Anchor/Bounds自動統一)

### 注意点・既知の癖
- 実行順: **TexTransToolはlilycalInventoryより前に動く**(TTT 0.10.6でBeforePlugin追加。それ以前はAtlasTexture後動作でエラーの既知問題)。**VQTの変換はlilycalInventoryより後**(VQT 2.6.0+)
- Intパラメータの圧縮(1.2.0+)などパラメータ最適化を内蔵
- メニュー名のスラッシュでフォルダ自動生成、同名サブメニュー統合(1.2.0+)

### バージョン要点
- 1.0.0 (2024-08) 安定版 / 1.4.x (2024-09〜10) 安定期 / 1.5.x (2025-09) 現行
- 関連ツール: [lilToon](05-liltoon.md)(同一作者)、[TexTransTool](04-textranstool.md)/[VRCQuestTools](11-vrcquesttools.md)(順序制約あり)

---

## Flare(Auros)

- リポジトリ: https://github.com/Auros/Flare(MIT)
- ドキュメント: https://docs.auros.nexus/vrchat/flare/

### 概要
NDMFベースの非破壊トグル作成ツール。Vixen(Haï)に着想を得ており、アニメーションユーティリティはMAから流用(MIT)。**独自のメニューシステム**を持つ(MA Menu Item非統合)。

### 特徴
- **Flare Control**: トグル/メニュー制御の中核。**「Interpolate」に秒数を指定すると0→1/1→0をその時間かけて遷移**させられる(公式ドキュメントが「smooth transitions, simple dissolve effects, gradual activations」を明記)。補間は線形
- **FlareSettings**: 生成するAnimatorステートのWrite Defaultsモードを制御(タグシステムとduration付きコントロールが対象)
- タグシステムによるコントロールのグループ化

### 注意点
- MAのメニュー/パラメータ体系とは別系統のため、MA中心のワークフローに混ぜる場合はメニュー構成が二重管理になる
- 開発ペースは緩やか(採用時はリポジトリの活動状況を確認)

---

## Vixen / Prefabulous / Animator As Code(Haï)

- **Vixen**: https://github.com/hai-vr 系のトグルツール。Flareの着想元。現在アクティブな選択肢ではない
- **Prefabulous**: Haï氏のNDMFユーティリティ集。「値+秒数のトゥイーン生成」機能は現状含まれない(2026-07時点)
- **Animator As Code (AAC) V1 + NDMF Processor**: C#でAnimator Layerを宣言的に生成するライブラリ。NDMFプラグインとして動かすテンプレートあり。**コードを書く前提**のため、ノーコード用途には不適だが、複雑な独自ギミックの再利用可能化には最有力

---

## NDMFプラグインID早見表(実行順序制約に登場するツール)

主要7ツールのプラグイン定義ソース(KB 00 §1.5)に現れた外部プラグインID。順序トラブルの調査時に参照:

| プラグインID | ツール | 判明している順序関係(宣言元) |
|---|---|---|
| `jp.lilxyzw.lilycalinventory` | lilycalInventory | TTTより後(TTT宣言)、VQT変換より前(VQT宣言) |
| `io.github.azukimochi.light-limit-changer` | Light Limit Changer | TTT(Transforming)より後(TTT宣言) |
| `net.narazaka.vrchat.floor_adjuster` | FloorAdjuster(narazaka) | TTTより後(TTT宣言)。MA 1.17+は内蔵Floor Adjusterを同様にTTT後へ配置 |
| `MantisLODEditor.ndmf` | NDMF Mantis LOD Editor(ポリゴン削減) | TTTより後、VQT MeshFlipperより後・RemoveVertexColorより前(TTT/VQT宣言) |
| `Meshia.MeshSimplification.Ndmf.Editor.NdmfPlugin` | Meshia Mesh Simplification | TTTより後、VQT変換より前(両者宣言) |
| `jp.lilxyzw.ndmfmeshsimplifier.NDMF.NDMFPlugin` | lilNDMFMeshSimplifier | 同上 |
| `com.aoyon.overall-ndmf-mesh-simplifier` | Overall NDMF Mesh Simplifier | VQT変換より前(VQT宣言) |
| `dev.logilabo.virtuallens2.apply-non-destructive` | VirtualLens2 | VQT Resolvingより後(VQT宣言: VQTが先に設定を構成) |
| `jp.unisakistudio.posingsystemeditor.posingsystemconverter` | ポージングシステム変換 | VQT変換より前(VQT宣言: 生成メニューを変換対象に含める) |
| `wataameya.marshmallow_PB.ndmf` | ましゅまろPB | MA late-transform-stagesより前(MA宣言) |
| `nadena.dev.modular-avatar` / `net.rs64.tex-trans-tool` / `com.anatawa12.avatar-optimizer` / `com.github.kurotu.vrc-quest-tools` | 主要ツール | KB [00-cross-tool.md](00-cross-tool.md) §1.4〜1.6参照 |

**使い方**: 「AツールとBツールの順序がおかしい」と疑ったら、まずこの表とKB 00 §1.5で既知の制約を確認し、無ければNDMFのプラグインシーケンス表示ウィンドウで実際の順序を見る。


======================================================================
# FILE: 09-avatar-optimizer.md
======================================================================

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
- lilToon固有処理が多い(AudioLinkマスク、AngelRing、アウトラインマスク等)→[lilToon](05-liltoon.md)側のバージョンにも依存

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
- [TexTransTool](04-textranstool.md): TTTが先に実行され、AAOとUV/RemoveMesh領域をネゴシエーション
- [Modular Avatar](02-modular-avatar.md): MA出力を最適化(DelayDisable等はMA 1.12+でAAOフレンドリー化)
- [VRCQuestTools](11-vrcquesttools.md): VQT変換→AAO→VQT形式チェックの順。**AAO 1.7.0+はVQT 2.x必須**

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


======================================================================
# FILE: 10-optimization-conversion-tools.md
======================================================================

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


======================================================================
# FILE: 11-vrcquesttools.md
======================================================================

# VRCQuestTools (VQT)

- リポジトリ: https://github.com/kurotu/VRCQuestTools
- ドキュメント: https://kurotu.github.io/VRCQuestTools/
- パッケージ名: `com.github.kurotu.vrc-quest-tools`
- 作者: kurotu

## 概要

PC向けアバターを**Quest/Android/iOS(モバイル)対応へ変換**するツール。マテリアルのモバイル対応シェーダーへの変換(テクスチャベイク付き)、非対応コンポーネント除去、Avatar Dynamics削減、プラットフォーム別出し分けを、破壊的(複製生成)にも**非破壊(NDMFビルド時)**にも実行できる。

- Unity要件: 2022.3(次期メジャーでUnity 2019サポート削除がUnreleasedに記載。2.x現行は2019コードも残存)
- VRCSDK要件: 2.0.0で3.3.0+。次期メジャーで**3.9.0未満を切り捨て予定**
- リポジトリ構造: Unityプロジェクト形式。パッケージ本体は`Packages/com.github.kurotu.vrc-quest-tools/`

## 依存関係

- 必須依存なし(VRCSDKは前提)
- オプション: **NDMF**(非破壊変換・`[NDMF]`表記の機能全般に必須。次期メジャーでNDMF <1.5切り捨て)、Modular Avatar(Convert Constraints連携、Setup Avatar for Mobile)、lilToon/Poiyomi(変換元シェーダー)、FinalIK/VirtualLens2(専用対応)
- AAOとの互換: AAO 1.5.7+/VQT 2.x(AAO 1.7.0でVQT 1.x互換削除)

## 対応する改変パターン

### マテリアル/テクスチャ変換
- **lilToon → Toon Lit / MatCap Lit / Toon Standard(VQT 2.10+、要VRCSDK 3.8.1+)**: 色・影・Emission・MatCap・AO・デカール等をテクスチャへベイク。lilToonカスタムシェーダー派生も対象(2.7+)。lilToon 2.0対応は2.11.1+
- **Poiyomi → Toon Lit**(メインカラー/ノーマル陰影/Emission 0-3)。→Toon Standardは実験的(開発版)
- Material Replacement(任意マテリアルへの手動差し替え)、マテリアル個別の変換設定
- アニメーション/AnimatorController/BlendTree/AnimatorOverrideControllerの変換追従(マテリアル差し替えアニメも変換)
- テクスチャ圧縮形式・最大サイズ制御、メニューアイコンのリサイズ/圧縮(`VQT Menu Icon Resizer`)

### コンポーネント/構造
- 非対応コンポーネントの除去(Constraint類はVRCConstraintsへの変換誘導=MA Convert Constraints連携、2.10+)
- **VQT Platform Component Remover / Platform GameObject Remover**: ビルドプラットフォーム別の出し分け(NDMF必須)
- **VQT Platform Target Settings**: 対象プラットフォームの明示
- **VQT Network ID Assigner**: PC/Quest間のPhysBone同期用Network ID割り当て(2.5+はNDMFなしでも動作)
- **VQT Mesh Flipper**(2.6+): メッシュ反転/両面化(ポリゴン削減前/後のフェーズ選択可、2.9+)
- **VQT Avatar Converter Settings**: 変換設定をアバターに保存し、**NDMFビルド時に非破壊変換**(2.3+)。NDMFフェーズ選択(Transforming/Optimizing/Auto)。Autoは既定Optimizing、VRCFury存在時はTransforming(2.10+)
- **VQT Fallback Avatar**(開発版): アップロード後の自動フォールバック設定
- Avatar Dynamics(PhysBone等)の選択削減(パフォーマンスランク推定付き)、`Remove Avatar Dynamics`オプション(2.9+)

### 検証系
- テクスチャ形式チェック(Androidで未対応形式の警告。AAO後に実行)、Unity Settings for Mobile、バリデーション自動化(2.11+で設定可)

## 改変時の注意点(ソース由来の癖)

- **NDMFフェーズ配置**(`Editor/NDMF/VRCQuestToolsNdmfPlugin.cs`): Resolvingで`BeforePlugin` MA/VirtualLens2(プラットフォーム別除去はMAより前に済ませる)。変換パスはTransforming/Optimizing両方に存在し、コンポーネント設定でどちらで動くか決まる。いずれも`AfterPlugin` TTT/MA/lilycalInventory、Optimizing側は`BeforePlugin` AAO
- **変換フェーズの選択が互換性の要**: VRCFury等の非NDMFツール(Transforming後に実行)が生成するマテリアルを変換したい場合はOptimizing、逆にVRCFuryがVQT変換後の状態を前提とするならTransforming。2.10+のAutoが妥当な既定
- **非破壊変換はビルドターゲットがAndroid/iOSのときに発動**する(PC向けビルドでは変換パスはスキップ)。「[NDMF] Build and Test for PC with Android Settings」でPC上での見た目確認が可能
- 手動変換(破壊的)は複製を生成(出力先`Assets/VRCQuestToolsOutput`、2.0+)。元アバターは変更されない
- テクスチャベイクはlilToonのプロパティ実装に強く依存: lilToon側の更新でベイク不具合が出やすい(2.11.4はlilToon 2.3.0+が必要な修正を含む)。**lilToonとVQTはセットで更新**
- MA連携: `MA Visible Head Accessory`/`MA World Fixed Object`はMA 1.9+なら削除しない(2.4.0+)。未対応MAバージョン使用時はエラー(開発版)。`Setup Avatar for Mobile`はMA Sync Parameter Sequence(PrimaryPlatform=PC)等を自動構成
- 2.0.0でインポートパスが`Assets/KRT/VRCQuestTools`→`Packages/com.github.kurotu.vrc-quest-tools`に変更(1.x→2.x移行時は旧フォルダ削除)

## Quest対応時の注意(このツール自体がQuest対応ツール)

VRChatのモバイル制約の実装対象:
- シェーダー: VRChat/Mobile系+Toon Standard(2.9.2+でVRCSDKのAndroidホワイトリスト参照)
- テクスチャ: ASTC推奨(形式チェックパスが検査)、最大サイズ制御
- コンポーネント: Constraint(Unity製)、Cloth、Camera、Light、AudioSource等は非対応→除去/変換
- パフォーマンスランク: モバイルはVery Poorでアップロード不可(Fallback運用)。Avatar Dynamics上限(PhysBone数等)は専用削減UIで対応
- PC/Quest同一Blueprint IDでのアップロード時、**パラメータ順序の一致**(MA Sync Parameter Sequence)と**PhysBoneのNetwork ID一致**(VQT Network ID Assigner)が同期ずれ防止の要

## 関連ファイルパス

| パス | 役割 |
|---|---|
| `Packages/com.github.kurotu.vrc-quest-tools/package.json` | パッケージ定義 |
| `.../Editor/VRCQuestTools.cs` | 本体定義(バージョン定数等) |
| `.../Editor/NDMF/VRCQuestToolsNdmfPlugin.cs` | NDMFプラグイン定義(順序制約の一次情報) |
| `.../Runtime/Components/` | VQTコンポーネント群 |
| `Website/` | ドキュメントソース(Docusaurus) |
| `CHANGELOG.md` | 変更履歴 |

## よくあるトラブル

- **変換後の見た目が暗い/発光しない**: lilToon/PoiyomiのEmission系変換の既知修正(2.11.2/2.11.3/2.11.7)。UVタイリング未保持は2.11.6修正。→VQT最新パッチへ
- **lilToon 2.xで変換失敗**: VQT 2.11.1未満は非対応
- **テクスチャ形式警告が大量に出る**: Windowsビルドターゲットでのチェックは2.11+で既定OFF。Android時の警告は実際に直す(ASTC化)
- **VRCFuryのギミックが変換されない**: 変換フェーズをOptimizing(またはAuto)にする
- **PC/Quest間で同期が壊れる**: Network ID未割り当て(VQT Network ID Assigner)+パラメータ順序(MA Sync Parameter Sequence)を両方確認
- **GoGo Loco入りFXの変換エラー**: 2.5.3で修正(サブステートマシン対応)
- **prefabステージで変換できない**: 仕様(2.4.1でエラーメッセージ化)

## 関連ツール

- [Modular Avatar](02-modular-avatar.md): Convert Constraints / Sync Parameter Sequence / Setup Avatar for Mobileで密連携
- [AAO](09-avatar-optimizer.md): VQT変換→AAO最適化→VQT形式チェックの順で協調
- [TexTransTool](04-textranstool.md): TTT出力を変換対象に含めるため常にTTT後
- [lilToon](05-liltoon.md) / [Poiyomi](06-poiyomi.md): 変換元シェーダー

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


======================================================================
# FILE: 12-analysis-upload-tools.md
======================================================================

# 検証・分析・アップロード・環境管理ツール

アバター改変の「作る」以外の工程(テスト、分析、アップロード、パッケージ管理)を支えるツール群。

> 情報源: 各リポジトリのREADME/CHANGELOG/package.json(2026年前半時点)。

## 目的別早見表

| 目的 | ツール |
|---|---|
| Unity内でメニュー・ジェスチャーを試す | Gesture Manager |
| Avatars 3.0の挙動を忠実にエミュレート(同期・OSC含む) | Av3Emulator |
| テクスチャ/VRAM/マテリアルの分析・一括変更 | lilAvatarUtils |
| Performance Rankをアップロード前に確認 | Actual Performance Window(gist pack内) |
| 複数アバターの連続アップロード | Continuous Avatar Uploader |
| VCCの代替・CLI/高速なパッケージ管理 | vrc-get / ALCOM |
| Booth配布物の「VCC用インストーラー」の正体 | VPMPackageAutoInstaller |
| コンポーネントの一括コピー(再セットアップ) | Pumkin's Avatar Tools |
| Hierarchy/Projectビューの拡張 | lilEditorToolbox |

---

## Gesture Manager(BlackStartx)

- リポジトリ: https://github.com/BlackStartx/VRC-Gesture-Manager
- 導入: VCC公式(Curated)パッケージ

Unityエディタ内でアバターのアニメーションをプレビュー・編集するツール。Play Modeで**Expression MenuのRadial Menuを再現**し、トグルやパペットを実際の見た目で試せる。ジェスチャー(手形状)の組み合わせテストも可能。

- NDMFのApply on Playと共存する(NDMF適用後のアバターをそのまま操作できる)ため、**MA/AAO/TTT適用結果のメニュー動作確認の標準手段**
- READMEの「Unity 2018/2019」記述は古い(現行はVRCSDK3+Unity 2022環境で使用されている)
- 類似のAv3Emulatorとの使い分け: メニュー操作の確認はGesture Manager、同期やState Behaviourの厳密な検証はAv3Emulator

## Av3Emulator(Lyuma)

- リポジトリ: https://github.com/lyuma/Av3Emulator

Avatars 3.0のランタイム挙動をUnityの**PlayableGraph APIで再実装**したエミュレータ。Play Modeでアバターにランタイムコンポーネントが付与される。

- 特徴: **非ローカルクローンの生成による同期テスト**(synced/local変数の見え方の違いを検証できる数少ない手段)、Animatorウィンドウでのレイヤーライブ表示、Gesture weight(アナログFist)、Expression Menu、Parameter Driver、Viseme、OSC対応
- **既知の相互作用**(KB [00](00-cross-tool.md)/[06](09-avatar-optimizer.md)参照): NDMFはAv3Emulator検出時に自前のApply on Playの二重適用を抑止する。AAOはAv3Emulator起動時にRead/Write無効メッシュを処理できない場合がある(AAO 1.8.0で大幅改善)
- 「Play時だけ壊れる」系トラブルの切り分けでは、Av3Emulator/Gesture Manager/素のPlayの3通りで比較する

## lilAvatarUtils(lilxyzw)

- リポジトリ: https://github.com/lilxyzw/lilAvatarUtils
- パッケージ名: `jp.lilxyzw.avatar-utils` / 現行 2.1.x(Unity 2022.3、MIT)

アバター分析ウィンドウ(`Tools/lilAvatarUtils`)。軽量化の**現状把握**に使う(実際の削減は[AAO](09-avatar-optimizer.md)/[TTT](04-textranstool.md)の仕事)。

- **テクスチャ一覧**: VRAMサイズ・解像度・圧縮形式を一覧表示し、インポート設定をまとめて変更可能
- **マテリアル/アニメーション一覧**: 使用箇所の逆引き
- **ライティングテスト**: 各種ワールド照明条件での見た目確認
- **セーフティ(シェーダーフォールバック)のシミュレーション**: 2.1.0でToon Standardフォールバック対応(VRChat仕様変更「Unlitフォールバックの打ち切り廃止」にも追従)
- バージョン要点: 2.0.0 (2025-01)でローカライズ対応・メニュー位置変更(`Tools/lilAvatarUtils`)。2.1.1でマテリアルバリアント(親)のスキャン修正

## anatawa12's gist pack / Actual Performance Window(anatawa12)

- リポジトリ: https://github.com/anatawa12/unity-gist-pack
- パッケージ名: `com.anatawa12.gists` / 0.25.x(MIT)

anatawa12製の小物ツール集。`Tools/anatawa12's gist selector`で**使いたいgistだけを個別に有効化**する方式。バージョン規則が特殊(x=gist追加、y=更新。**1.0には永遠に到達しない=個々のgistは小さな破壊的変更をしうる**とREADMEに明記)。

- 代表格 **Actual Performance Window**: Play Mode時/ビルド直後に**最適化適用後の実際のPerformance Rank数値**を表示する。AAO等の非破壊最適化はエディタ上の静的な数値に反映されないため、**軽量化の効果測定はこれが事実上の標準**
- AAOと同じVPMリポジトリ(vpm.anatawa12.com)で配布されるため、AAO導入済みなら追加が容易

## Continuous Avatar Uploader(anatawa12)

- リポジトリ: https://github.com/anatawa12/ContinuousAvatarUploader

複数アバターを**連続自動アップロード**するツール。説明文へのバージョン番号自動付与(`(v1)`→`(v2)`)、gitリポジトリ内ならアップロード時の自動タグ付けに対応。PC/Quest両対応を多数のアバターで維持する運用(VQTと併用)で効果大。

## vrc-get / ALCOM(anatawa12ほか)

- リポジトリ: https://github.com/vrc-get/vrc-get

VRChat Package Manager(VPM)の**オープンソースCLIクライアント**。Windows/Linux/macOS対応。公式VCCより高速で、公式vpmコマンドにない機能も提供する(コミュニティ開発であり、VRChat公式ではない)。

- **ALCOM**(vrc-get-gui): vrc-getベースのGUI版で、**VCCの実用的な代替**。VCCが不安定な環境・macOS/Linux環境での定番
- 本ナレッジの各ツールはすべてVPMリポジトリ経由の導入が基本なので、環境管理はVCC/ALCOMのどちらでも同じ手順が通じる

## VPMPackageAutoInstaller(anatawa12)

- リポジトリ: https://github.com/anatawa12/VPMPackageAutoInstaller

「unitypackageをインポートするとVPMパッケージがインストールされる」**インストーラーを作成する**ツール(vrc-getのC#再実装ベース)。narazaka系ツール等の「VCC用インストーラーunitypackage」はこの仕組み。**Booth配布物のインストーラーの挙動を理解する**うえで押さえておくとトラブル対応が楽になる(実体はVPMリポジトリ追加+パッケージ追加)。

## Pumkin's Avatar Tools(rurre)

- リポジトリ: https://github.com/rurre/PumkinsAvatarTools

老舗のセットアップ支援ツール。中核は**コンポーネントコピー**(アバターを再インポート/別素体に載せ替える際、PhysBone・Collider・Descriptor設定等を一括コピー)とサムネイル補助。

- 注意: メンテナンスは緩やか(Wikiも長期未更新とREADMEに明記)。MA/NDMF以前の世代のツールだが、「FBX更新で作り直し」のような破壊的作業では今も現役
- 非破壊ワークフローでの代替: 载せ替え用途はMA(プレハブ化)で設計する方が今風

## lilEditorToolbox(lilxyzw)

- リポジトリ: https://github.com/lilxyzw/lilEditorToolbox
- パッケージ名: `jp.lilxyzw.editortoolbox` / 2.0.x(Unity 2022.3、MIT)

Unityエディタ全般のQoL拡張集。`Edit/Preferences/lilEditorToolbox`で機能を個別に有効化。

- Hierarchy拡張(オブジェクトON/OFF、コンポーネント、タグ/レイヤー表示)、Project拡張(拡張子・プレハブ情報表示)、ツールバー拡張(アセンブリロックボタン等)など
- 改変作業の直接ツールではないが、大規模アバターのHierarchy把握が楽になる

---

## 関連ページ

- 実行順序・互換性の基礎: [00-cross-tool.md](00-cross-tool.md)
- メニュー/トグル生成系: [08-ecosystem-tools.md](08-ecosystem-tools.md)
- 軽量化・変換系: [10-optimization-conversion-tools.md](10-optimization-conversion-tools.md)
- 表情・ギミック系: [07-expression-gimmick-tools.md](07-expression-gimmick-tools.md)


======================================================================
# FILE: 13-vrchat-avatars-basics.md
======================================================================

# VRChat / Avatars 3.0 基礎リファレンス

ツール知識の土台となるVRChat側の仕様。数値は**VRChat公式Creator Docs(github.com/vrchat-community/creator-docs)のソースから転記**(2026年前半時点。VRChatの仕様変更で変わりうるため、判断が際どい場合は[公式](https://creators.vrchat.com/avatars/avatar-performance-ranking-system/)で最新を確認)。

## Playable Layers(アニメーターの層構造)

Avatars 3.0のアバターは複数のAnimator Controllerを層として持つ:

| レイヤー | 役割 | 改変ツールとの関係 |
|---|---|---|
| Base | ロコモーション(歩行・ジャンプ) | GoGo Loco等の移動システムが差し替える対象([15](15-gogo-loco.md)) |
| Additive | 呼吸等の加算アニメーション | 通常触らない |
| Gesture | 手指・ジェスチャー | 表情ツールが手形状を参照([07](07-expression-gimmick-tools.md)) |
| Action | エモート・AFK | GoGo Loco等が拡張 |
| **FX** | マテリアル・BlendShape・トグル等の見た目全般 | **改変ツールの主戦場**。MA Merge Animator等はここへ統合するのが基本 |
| Sitting / TPose / IKPose | 着席・キャリブレーション用 | 通常触らない |

- FX以外のレイヤーではマテリアル等の見た目プロパティを動かさないのが原則(FXはヒューマノイドボーンを動かさない)
- MMDワールドはFXレイヤー1〜2を無効化して独自表情を流す→MAのMMD対応([02](02-modular-avatar.md))の背景

## まばたき・視線(VRC Avatar Descriptor)

まばたき(自動瞬き)と視線は**FXではなくVRC Avatar DescriptorのEye Look / Eyelids設定**で動く:

- Eyelids: 主流は「Blendshapes」方式(対象の顔メッシュ+Blink用シェイプキーを指定)。**改変で顔メッシュの名前やシェイプキー名を変更/削除すると壊れる**
- 表情アニメーションが同じシェイプキーを常時上書きしていても瞬きは止まる(WD ONの焼き込み表情が典型)
- 最適化ツールによるシェイプキー凍結でも壊れうる([09](09-avatar-optimizer.md)のExclusionsで切り分け)
- 「瞬きできない」の問診フローは[19 §D-3](19-triage-guide.md)

## Expression Parameters(同期パラメータ)

- **同期メモリ上限: 256 bit**(VRCSDKの`VRCExpressionParameters.MAX_PARAMETER_COST`)。Bool=1bit、Int=8bit、Float=8bit
- synced(全員に同期)/ local only(自分のみ)、saved(ワールド間で保持)、default値を持つ
- 改変ツールとの関係:
  - [MA Parameters](02-modular-avatar.md)が衝突回避のリネーム・使用量集計(MA Informationで超過表示)を担う
  - ギミック導入時は「そのギミックが何bit使うか」を確認する(例: [GoGo Loco](15-gogo-loco.md)は16/256bit+拡張1bit)
  - lilycalInventoryはInt圧縮等のパラメータ最適化を内蔵([08](08-ecosystem-tools.md))
- 主な組み込みパラメータ(FXから参照可能): `IsLocal` / `GestureLeft(Right)` / `GestureLeftWeight` / `Viseme` / `TrackingType` / `VRMode` / `MuteSelf` / `AFK` / `Seated` など(全一覧は公式animator-parameters参照)

### OSC(参考)

Expression ParametersはOSCで外部から読み書きできる(`/avatar/parameters/<名前>`)。OSCギミック改変時の注意: AAOはOSC使用パラメータを削除しうるため、**Asset DescriptionでOSC使用を宣言する仕組み**がある([09](09-avatar-optimizer.md)、AAO 1.8.0+)。

## Performance Rank(公式表の転記)

- ランクは Excellent / Good / Medium / Poor / **Very Poor(上限なし)** の5段階。**どれか1項目でも超えたら次のランクに落ちる**
- **無効化中のGameObject/Componentもすべてカウントされる**(トグルで隠してもランクは変わらない→ビルド時に削除する[AAO](09-avatar-optimizer.md)等が有効な理由)
- **Read/Write無効のメッシュが1つでもあるとTriangles計測不能で強制Very Poor**(SDKが警告)

### PC Limits

| 項目 | Excellent | Good | Medium | Poor |
|---|---|---|---|---|
| Triangles | 32,000 | 70,000 | 70,000 | 70,000 |
| Texture Memory | 40 MB | 75 MB | 110 MB | 150 MB |
| Skinned Meshes | 1 | 2 | 8 | 16 |
| Basic Meshes | 4 | 8 | 16 | 24 |
| Material Slots | 4 | 8 | 16 | 32 |
| PhysBones Components | 4 | 8 | 16 | 32 |
| PB Affected Transforms | 16 | 64 | 128 | 256 |
| PhysBones Colliders | 4 | 8 | 16 | 32 |
| PB Collision Check Count | 32 | 128 | 256 | 512 |
| Contacts | 8 | 16 | 24 | 32 |
| Constraint Count | 100 | 250 | 300 | 350 |
| Constraint Depth | 20 | 50 | 80 | 100 |
| Animators | 1 | 4 | 16 | 32 |
| Bones | 75 | 150 | 256 | 400 |
| Lights | 0 | 0 | 0 | 1 |
| Particle Systems | 0 | 4 | 8 | 16 |
| Total Particles Active | 0 | 300 | 1000 | 2500 |
| Trail / Line Renderers | 各1 | 各2 | 各4 | 各8 |
| Cloths | 0 | 1 | 1 | 1 |
| Audio Sources | 1 | 4 | 8 | 8 |

- **PCのTrianglesはGood以上=70,000が実質上限**(超えたら即Very Poor)
- PCの既定「Minimum Displayed Performance Rank」は**Very Poor=全員表示**。ユーザーがMedium/Poorに絞ると、超過項目に応じて「コンポーネント除去」または「Fallback置換」(Triangles/Texture Memory/メッシュ数/マテリアル数/Bones超過はFallback置換=丸ごと差し替え)

### Mobile(Quest/Android/iOS) Limits

| 項目 | Excellent | Good | Medium | Poor |
|---|---|---|---|---|
| Triangles | 7,500 | 10,000 | 15,000 | 20,000 |
| Texture Memory | 10 MB | 18 MB | 25 MB | 40 MB |
| Skinned Meshes | 1 | 1 | 2 | 2 |
| Basic Meshes | 1 | 1 | 2 | 2 |
| Material Slots | 1 | 1 | 2 | 4 |
| Animators | 1 | 1 | 1 | 2 |
| Bones | 75 | 90 | 150 | 150 |
| PhysBones Components | 0 | 4 | 6 | 8 |
| PB Affected Transforms | 0 | 16 | 32 | 64 |
| PhysBones Colliders | 0 | 4 | 8 | 16 |
| PB Collision Check Count | 0 | 16 | 32 | 64 |
| Contacts | 2 | 4 | 8 | 16 |
| Constraint Count | 30 | 60 | 120 | 150 |
| Constraint Depth | 5 | 15 | 35 | 50 |
| Particle Systems | 0 | 0 | 0 | 2 |

- Mobileの既定ブロックは**Medium**(Poor以下は非表示。ユーザーはPoorまで緩和可能だがVery Poor表示設定は存在しない。個別の「Show Avatar」でのみ表示可 — **この救済は将来削除される可能性を公式が明言**)
- **ハードキャップ(Show Avatarでも回避不可)**: PhysBone 8個 / 影響Transform 64 / コライダー16 / 衝突チェック64 / Contacts 16 / Constraint 150・深さ50 — 超過すると**該当コンポーネントが全て除去される**
- Mobileでは Lights / Cloth / 物理コライダー / Rigidbody / **Audio Source** は常に無効(存在しても0扱い)
- Raycastsは全プラットフォーム共通で1アバター80個のハード上限

## アバターサイズ制限(公式表の転記)

| プラットフォーム | ダウンロードサイズ(圧縮後) | 非圧縮サイズ |
|---|---|---|
| PC | 200 MB | 500 MB |
| Android(Quest等) | 10 MB | 40 MB |

- SDKがビルド時に検査(Android非圧縮はSDK 3.5.2+、PC制限は3.7.0+で強制。それ未満のSDKだとアップロードがサーバー側で失敗する)
- Build & Testでは制限が適用されない(=テストで通ってもアップロードで弾かれることがある)

## セーフティ / フォールバックの2系統(混同注意)

1. **アバターFallback**: Performance Rankブロックやセーフティで本体が表示されないとき、代わりに表示される軽量アバター。自分で設定でき(Fallback対応アバターをFallbackに指定)、未設定ならロボット等のデフォルト。VQTの`VQT Fallback Avatar`はこの自動設定を支援([11](11-vrcquesttools.md))
2. **シェーダーFallback**: セーフティで「Shaders」がオフにされた相手には、マテリアルの`VRCFallback`タグに基づく標準シェーダーで描画される。lilToon/Poiyomiはフォールバック設定UIを持ち([05](05-liltoon.md)/[06](06-poiyomi.md))、Toon Standard(Outline)フォールバックも指定可能(lilToon 1.10+)。**見え方の事前確認はlilAvatarUtilsのフォールバックシミュレーション**([12](12-analysis-upload-tools.md))

## ビルドコールバック(改変ツールの入口)

VRCSDKは`IVRCSDKPreprocessAvatarCallback`をcallbackOrder順に実行する。主要な順序(ソース確認済みの値は[00 §1.1](00-cross-tool.md)):

| order | 実行者 |
|---|---|
| -11000 | NDMF前段(First〜Transforming: MA/TTT等) |
| -10000 | VRCFury([14](14-vrcfury.md)) |
| -1025 | NDMF後段(Optimizing〜: AAO/VQT等) |
| -1024 | VRChatのEditorOnly破棄(`RemoveAvatarEditorOnly`) |
| 0〜 | VRCSDK標準処理 |

## 関連ページ

- 実行順序の詳細: [00-cross-tool.md](00-cross-tool.md) / ランク改善の実務: [09](09-avatar-optimizer.md)・[10](10-optimization-conversion-tools.md) / Quest対応: [11](11-vrcquesttools.md) / ランク実測: [12](12-analysis-upload-tools.md)のActual Performance Window


======================================================================
# FILE: 14-vrcfury.md
======================================================================

# VRCFury(防御的ナレッジ)

- リポジトリ: https://github.com/VRCFury/VRCFury
- 公式サイト: https://vrcfury.com

> **このKBの方針ではVRCFuryは使用しない**(NDMF/MA系で統一)。ただし**導入済みアセット・配布ギミック・他人のアバターに極めて高頻度で含まれる**ため、トラブルシュートのための知識としてこのページを置く。ソース裏付けは限定的で、既知の相互作用は本KB内でソース確認済みの事実(MA/AAO/VQT側の記述)を根拠とする。

## 概要

英語圏で最大手の非破壊改変ツール。**NDMFベースではない**独自実装で、VRCSDKのビルドコールバック(**callbackOrder -10000**)とPlay Mode時の適用で動作する。

主な機能(代表的なもの):
- **Toggle**: トグル生成。**Transition Time(値+秒数のトゥイーン)**を持つ(KB [08の比較表](08-ecosystem-tools.md)参照)
- **Full Controller**: 既存のアニメーター/メニュー/パラメータ一式をアバターへ合成(MAのMerge Animator+Menu Installer+Parametersに相当)
- **Armature Link**: 衣装アーマチュアの結合(MA Merge Armature相当)
- その他: 各種Fixes(Anchor Override統一等)、ギミック向けコンポーネント多数

## 運用上の特性(トラブルの源泉)

- **バージョン固定が事実上できない**: ローリングリリース+自動アップデートの思想で、「特定バージョンで止めて安定運用」がしづらい。再現性が必要なプロジェクトでNDMF系と思想が衝突する主因
- **NDMFの順序制御が及ばない**: 常に「NDMF Transforming(-11000)の後、NDMF Optimizing(-1025)の前」で実行される([00 §1.1](00-cross-tool.md))。NDMF側から`BeforePlugin`/`AfterPlugin`で位置調整することは不可能
- コンポーネントのシリアライズが独自形式で、VRCFury未導入プロジェクトに持ち込むと不明コンポーネント化しやすい

## 既知の相互作用(本KBでソース確認済みのもの)

| 相手 | 内容 |
|---|---|
| NDMF | NDMF 1.3.0/1.3.2/1.5.0でVRCFury互換のためのフック順序調整・Transforming後のアセットシリアライズが入っている([01](01-ndmf.md)) |
| MA | **MA 1.15+はVRCFury < 1.1250.0 と Mesh Cutter / Shape Changer(delete mode)の併用に非互換警告**を出す([02](02-modular-avatar.md)) |
| AAO | AAO 1.8.11で`VRCFuryTest`対応。順序の議論はAAOリポジトリDiscussion #860が公式見解(併用は概ね動作するが、AAOがVRCFury生成物を解析するのはVRCFuryが先に走った場合のみ=Optimizing側のAAOは解析可能) |
| VQT | **VQT 2.10+の変換フェーズ「Auto」はVRCFury検出時にTransformingを選ぶ**(VRCFury生成マテリアルを変換対象にするため)([11](11-vrcquesttools.md)) |

## 併用時の定石(他人のアバター/VRCFury入りアセットを触る場合)

1. **同じ役割を二重管理しない**: 衣装結合はMA Merge ArmatureかVRCFury Armature Linkの**どちらか一方**に統一。トグルも同様
2. VRCFury入りギミックをそのまま使う場合、**NDMF系ツールの生成物との順序は変えられない**ことを前提に設計する(VRCFuryのToggleがNDMF Optimizing適用後の状態を参照することはない、等)
3. ビルド結果の検証はManual BakeではなくPlay Mode/アップロードで行う(VRCFuryはNDMFのManual Bakeに乗らない)
4. 削除する場合はパッケージ削除後に残存コンポーネント(missing script)を掃除([17](17-unity-troubleshooting.md))
5. MAとVRCFuryの機能対応表を意識して移行する: Armature Link→Merge Armature、Full Controller→Merge Animator+Menu Installer+Parameters、Toggle(Transition Time)→[08の比較表](08-ecosystem-tools.md)の代替(AvatarMenuCreatorForMA等)

## 関連ページ

- 実行順序の全体像: [00-cross-tool.md §1.1](00-cross-tool.md)
- トゥイーン機能の代替: [08-ecosystem-tools.md](08-ecosystem-tools.md)


======================================================================
# FILE: 15-gogo-loco.md
======================================================================

# GoGo Loco(移動システム)

- リポジトリ: https://github.com/franada/gogoloco
- 配布: Booth(franada)/ GitHub
- ドキュメント: README記載のcraft.doリンク / VRChatグループ・Discordあり

## 概要

デフォルトのロコモーション(移動アニメーション)を置き換える**定番の移動システムプレハブ**。導入率が非常に高く、「改変対象のアバターに最初から入っている」ことが多いため、直接使わなくても挙動を知っておく必要がある。

READMEより(一次情報):
- 足の動きやジャンプアニメーションを個別にOFFにできる多数のトグル
- **ポーズ切替でフルボディトラッキング風の姿勢を再現**(座る・寝転がるを任意の場所で)
- ビルトインの**プレイスペース上下移動**(小柄なアバター向け)
- ゲームワールド向けの高度な移動を有効化する「game loco」トグル
- **同期メモリ使用量: 16/256 bit**(+飛行/ダッシュ等のExtraで+1 bit)。PC/Quest両対応

## 仕組み(改変との関わり)

- **Base / Action / Sitting 等のPlayable Layerを差し替える**([13](13-vrchat-avatars-basics.md)のレイヤー構造参照)。FX中心のMA系改変とはレイヤーが分かれるため共存しやすいが、**移動系を触る他ギミックとは競合**する(移動システムは1つに統一)
- Expression Menuに専用メニュー(アイコン)を追加する。ライセンス条件として**販売モデルに組み込む場合もメインアイコンは残す**ことがREADMEで明示されている

## 改変ツールとの相互作用

| 相手 | 内容 |
|---|---|
| MA | メニュー統合自体は共存可。GoGo Locoのメニュー/パラメータ(16bit)をMA Parametersの容量計算に含めて設計する |
| AAO | **AAO 1.8.0で「VRCStation由来BoxColliderのアニメーション削除がGoGo Loco等の飛行系アバターを壊す」問題が修正済み**([09](09-avatar-optimizer.md)バージョン履歴)。飛行・姿勢が壊れたらまずAAOのバージョン確認 |
| Quest/VQT | GoGo Loco自体はPC/Quest両対応(README)。VQT変換はFX系が主対象でBase/Action層の置き換えはそのまま通る |
| MMDワールド | MMD処理はFXレイヤーの話なのでGoGo Locoとは概ね独立(MA側の対応に従う) |

## 導入・更新時の注意

- バージョンにより内部構成(レイヤー名・パラメータ)が変わるため、**アバター付属の古いGoGo Locoを手動で新しいものへ差し替える際は、旧バージョンの残骸(レイヤー・パラメータ・メニュー)を除去してから**導入する
- 「Beyond」等のエディション/追加機能はBooth配布物のバリエーションとして提供される(+1bit等の同期コスト差)。導入物がどれかを最初に確認する
- 改変後に移動がおかしい場合の切り分け: Gesture Manager/Av3Emulator([12](12-analysis-upload-tools.md))でBase/Actionレイヤーの状態遷移を見る→AAOのAnimator最適化を一時無効化して比較

## 関連ページ

- Playable Layerの基礎: [13-vrchat-avatars-basics.md](13-vrchat-avatars-basics.md)
- パラメータ容量: [13](13-vrchat-avatars-basics.md)(256bit)+[02](02-modular-avatar.md)(MA Parameters)


======================================================================
# FILE: 16-world-integrations.md
======================================================================

# ワールド連携機能(AudioLink / LTCGI / VRC Light Volumes)

ワールド側が提供する仕組みに、**アバター側のシェーダーが反応する**タイプの機能群。改変では「シェーダーの対応機能をONにする+対応ワールドで確認する」が基本で、アバター単体では動作確認できない点が共通のハマりどころ。

> 情報源: 各リポジトリREADME(2026年前半時点)+lilToon/PoiyomiのCHANGELOG(KB 05/06)。

## 共通の基礎知識

- これらは**PC向け機能**が中心(Questのアバターシェーダーは対応不可。ワールド側のQuest対応は各プロジェクト参照)
- 「自分のPCでは光るのにフレンドには見えない」系は、相手のセーフティ(シェーダーOFF→フォールバック描画)が原因のことが多い([13](13-vrchat-avatars-basics.md)のセーフティ節)
- 非対応ワールドでは単に反応しない(AudioLink等は非対応時の見た目を設定できる機能がシェーダー側にある)

---

## AudioLink

- リポジトリ: https://github.com/llealloo/audiolink(VPM配布)
- 現行: 2.x(2.0.0は2024-09)

**ワールド内の音声を解析し、周波数帯ごとのリアクティブデータをシェーダー/Udonに公開する**システム。データはグローバルな`_AudioTexture`として全アバターのシェーダーから参照できる。

- アバター側: lilToon / PoiyomiのAudioLink機能(Emission連動等)を有効化するだけ。対応ワールドで自動的に反応する
- エディタでのテスト: AudioLinkパッケージをアバタープロジェクトに入れると、エディタ内でテスト用のAudioLink環境を再生できる(アップロード物には含めない=ワールド側機能)
- 改変時の注意: **AAOのテクスチャ最適化とlilToonのAudioLinkマスクの組み合わせ問題はAAO 1.9.4で修正済み**([09](09-avatar-optimizer.md))。AudioLink演出が最適化後に壊れたらAAOのバージョンを確認
- 2.0.0の変更(README): コントローラ同期方法の調整、デュアルモノ音源対応、BlendShape駆動ユーティリティ(AudioReactiveBlendshapes)追加

## LTCGI

- リポジトリ: https://github.com/PiMaker/ltcgi(VPM: vpm.pimaker.at)
- ライセンス: 無料だが**Attribution(クレジット表記)必須**

**Linearly Transformed Cosineアルゴリズムによるリアルタイムエリアライト**(スクリーンの光が部屋とアバターを照らす等)。ワールド側に仕込む機能で、アバターは対応シェーダーであれば照らされる。

- アバター側: **lilToonは1.8.0でLTCGI対応**([05](05-liltoon.md))。シェーダー設定でLTCGIを有効化したマテリアルのみ反応する
- lilToonの既知修正: LTCGIの減衰・特定条件で動かない問題は1.8.4、AudioLinkとLTCGI併用時のワールドプロジェクトでのシェーダーエラーは2.1.5で修正
- 古いGPU(GTX9/10系)+LTCGI環境の問題はシェーダー側で修正歴あり(lilToon 2.1.2)

## VRC Light Volumes(VRCLV)

- リポジトリ: https://github.com/REDSIM/VRCLightVolumes
- 作者: REDSIM

**ボクセルベースのLight Probes代替**。ワールドのベイク照明を高品質・軽量にアバターへ適用する次世代ライティング。2025年以降、対応ワールド・対応シェーダーが急速に普及した。

- アバター側: **lilToonは1.10.0でVRCLV対応、2.0.0でVRCLV 2.0(方向対応)を同梱**、2.1.0でピクセル単位計算+専用リムライト設定([05](05-liltoon.md))。Poiyomiも9.x系で対応(バージョン別詳細はリポジトリの[Compatible Shaders](https://github.com/REDSIM/VRCLightVolumes/blob/main/Documentation/CompatibleShaders.md)を参照)
- lilToon 2.2.0の`_UdonForceSceneLighting`のように、ワールド側からアバターの明るさ調整を制御する連携も進んでいる
- 改変時の注意: VRCLV対応前のシェーダーバージョンだと対応ワールドで見た目が沈む/浮くことがある。**「特定ワールドでだけ暗い・明るい」系の相談ではシェーダーのVRCLV対応バージョンをまず確認**
- Light Limit Changer([07](07-expression-gimmick-tools.md))との関係: LLCは明るさの上下限操作、VRCLVは光源情報の供給で役割が異なる。併用可

## 関連ページ

- シェーダー側の対応履歴: [05-liltoon.md](05-liltoon.md) / [06-poiyomi.md](06-poiyomi.md)
- 明るさ調整メニュー: [07-expression-gimmick-tools.md](07-expression-gimmick-tools.md)(Light Limit Changer)


======================================================================
# FILE: 17-unity-troubleshooting.md
======================================================================

# Unity定番トラブル集(ツール以前の問題)

改変ツールの問題に見えて、実際はUnity/プロジェクト管理レベルの問題であるものの切り分け集。NDMF系のトラブルは[00 §3](00-cross-tool.md)が先。

## マテリアルがピンク(マゼンタ)

シェーダーが解決できていないサイン。原因の頻度順:

1. **シェーダー未導入**: lilToon/Poiyomi等がプロジェクトに無い。アバター/衣装の説明書記載のシェーダーを導入
2. **シェーダーのバージョン/エディション違い**: Poiyomi Pro向けマテリアルをToon環境で開いた、大型アップデートでシェーダーパスが変わった等([06](06-poiyomi.md)のバージョン共存の注意)
3. **ロック済みシェーダーの生成物欠落**: Poiyomiのロック済みマテリアルは生成シェーダーが必要。アンロック→再ロックで再生成([06](06-poiyomi.md))
4. render pipeline不一致: URP用アセットをBuilt-in(VRChat)に入れた等。VRChatアバターはBuilt-in RP固定
5. シェーダーキャッシュ破損: `Library/ShaderCache`削除→再起動

## Missing Script(スクリプトが見つからない)

- 原因: コンポーネントの元ツールがプロジェクトに無い(削除した/入れ忘れた)。VRCFury等の独自シリアライズ形式は特に起きやすい([14](14-vrcfury.md))
- 対処: 元ツールを導入して開き直すのが第一。不要と確定しているなら削除(NDMFはビルド開始時にmissing script componentを除去する([01](01-ndmf.md))が、エディタ作業では邪魔になる)
- AAOは未知の`IEditorOnly`コンポーネントに警告を出す。誤検知なら無視リストへ([09](09-avatar-optimizer.md)、1.9+)

## unitypackage と VPM の二重導入・移行事故

- 旧unitypackage版(`Assets/`配下)とVPM版(`Packages/`配下)の**二重導入はGUID/クラス重複の温床**。lilToonの`legacyFolders`(Assets\lilToon)、Poiyomiの`Assets\_PoiyomiShaders`、VQT 2.0のパス移動など、主要ツールはマニフェストで移行情報を持つが、**手動で入れた古いコピーは自前で削除**が必要
- 症状: 「同名クラスが2つ」コンパイルエラー、インスペクタ表示の異常、ビルド時の謎の旧挙動
- 対処: `Assets/`配下の旧ツールフォルダを削除→VCC/ALCOMでResolve
- NDMF 1.2.5は「VRChatテンプレート派生パッケージとのGUID衝突」を修正済み([01](01-ndmf.md))。それでもGUID衝突警告が出る場合は重複インポートを疑う

## プロジェクトが開けない・挙動が全体的におかしい

- **Libraryフォルダ削除→再インポート**が万能薬(時間はかかる)。プロジェクト破損疑いの第一手
- パッケージ解決エラー: VCC/ALCOMでResolve。`Packages/vpm-manifest.json`の手編集ミスも疑う
- **非ASCII(日本語)を含むプロジェクトパス**: VRCSDKの既知バグの温床(NDMF 1.5.0が一部を回避)。プロジェクトは英数字パスに置くのが安全
- Unityバージョン違いで開いた: VRChat公式指定(現行2022.3系)以外で開くとLibraryが壊れることがある。バージョンを戻し、Library削除

## プレハブ・シーンまわり

- **プレハブオーバーライドの意図しないRevert**: シーン初回ロードでオーバーライドが戻る問題はAAO 1.8.3で修正されたケースあり([09](09-avatar-optimizer.md))。類似症状はツールのバージョン確認
- Prefabステージでビルド系操作は不可(VQT 2.4.1がエラーメッセージ化)。**シーンに出してから**作業する
- 「Auto Referenced」問題: 一部ツールはasmdef設定変更でコンパイル時間を最適化している(lilToon 2.0等)。自作スクリプトから参照できない場合はasmdef参照を明示

## アップロードだけ失敗する

チェック順:
1. **サイズ制限**([13](13-vrchat-avatars-basics.md)): PC 200MB/500MB、Android 10MB/40MB。Build & Testでは検査されない点に注意
2. **Read/Write無効メッシュ**→強制Very Poor([13](13-vrchat-avatars-basics.md))
3. Pipeline Manager(Blueprint ID)の不整合: 別アバターのIDが残っている→Detach
4. ビルドコールバック失敗: 「〜PreprocessAvatarCallback reported a failure」はNDMF Console/Consoleの一次エラーを見る([00 §3](00-cross-tool.md))
5. 回線・CDN起因の一時失敗: 時間を置いて再試行

## テクスチャ・モデルインポートの定番

- テクスチャがぼやける: Max Size設定(既定2048)を確認。ただし上げる前にVRAMランク([13](13-vrchat-avatars-basics.md))とのトレードオフを検討
- 法線が壊れる/影が汚い: モデルインポートのNormals設定(Import/Calculate)とBlend Shape Normals(Legacy推奨のケース多し)
- FBX更新で改変が消えた: メッシュ資産の上書きはコンポーネント参照を壊しうる。載せ替えは[Pumkin's](12-analysis-upload-tools.md)のコピー機能か、最初からMAプレハブ設計で

## 関連ページ

- ツール系トラブルフロー: [00-cross-tool.md §3](00-cross-tool.md)
- VRChat側仕様(サイズ・ランク): [13-vrchat-avatars-basics.md](13-vrchat-avatars-basics.md)


======================================================================
# FILE: 18-license-notes.md
======================================================================

# ライセンス・規約の実務メモ

> **注意**: 法的助言ではなく、改変作業時に確認すべきポイントの実務メモ。個別の判断は必ず各アセットの規約原文を確認すること。規約は改定されるため、購入時期の規約と現行規約の両方に注意。

## アバター・衣装規約でまず確認する項目

| 確認項目 | 関わる作業 |
|---|---|
| 改変の可否・範囲 | ほぼ全ての改変作業 |
| **他モデルへのパーツ流用可否** | 非対応衣装の変換([03](03-fitting-tools.md))、パーツ移植。**もちふぃった～での変換も「流用」に該当しうる** |
| 改変データの受け渡し(改変代行・共同作業) | 依頼改変、フレンドへの譲渡。多くの規約で「双方が正規購入者なら可」形式 |
| 改変後アバターのアップロード条件 | ペデスタル公開可否、パブリックアバター化の可否 |
| 同梱アセット(シェーダー等)の再配布条件 | 改変済みデータの配布 |

- 日本圏のBooth製品は**VN3ライセンス**(vn3.org)形式の規約が多い。条項番号ベースで「改変」「他モデルへの使用」「共同作業」の可否が一覧化されているので、該当条項を読む
- **もちふぃった～のプロファイル**: アバター側規約の「解析・派生データ配布」の扱いに依存する。ショップ公式配布のプロファイルを使うのが最も安全([03](03-fitting-tools.md))

## ツール・シェーダーのライセンス早見

| 種別 | 代表 | 要点 |
|---|---|---|
| OSS(MIT等) | NDMF / MA / AAO / TTT / lilToon / lilAvatarUtils / Flare / AudioLink / Meshia / gist pack | 改変・再配布可(表記条件はライセンス文書に従う)。**アバター製品への同梱も一般に可** |
| OSS(条件付き) | LTCGI | 無料だが**Attribution必須**([16](16-world-integrations.md)) |
| 無料+有償本体 | NDMF Mantis LOD Editor | ラッパーは無料、Mantis LOD Editor Pro本体はAsset Storeライセンス(再配布不可)([10](10-optimization-conversion-tools.md)) |
| Booth有償 | もちふぃった～ / EreMorph / VirtualLens2 / ましゅまろPB / GoGo Loco(無償版あり) | **ツール本体の再配布不可**が基本。「ツールの出力物」の扱いは各規約で異なる(GoGo LocoはREADMEで「販売モデルへの同梱可・メニューアイコン維持」を明示) |
| Patreon限定 | Poiyomi Pro | **再配布不可**。Pro向けマテリアルを含むデータの受け渡しに注意([06](06-poiyomi.md)) |
| Asset Store | Mantis LOD Editor / FinalIK等 | シートライセンス。プロジェクト共有・改変代行時に相手側の所持が必要になるケースが多い |

## 改変データを配布・受け渡しする時のチェックリスト

1. アバター/衣装本体: 双方が正規購入者か。規約の受け渡し条項に適合するか
2. 有償ツール本体を同梱していないか(もちふぃった～/EreMorph等は**成果物のみ**渡す)
3. シェーダー: lilToonはMITで同梱可。**Poiyomi Proは不可**(Toon版に差し替えるかロック済み生成物の扱いを規約確認)
4. 有償ギミック(VirtualLens2等): 相手も購入者であることが前提の規約が多い
5. フォント・テクスチャ素材等の第三者素材の規約

## アップロード物に「含まれない」ものの整理(誤解しやすい)

- **エディタ専用ツール(NDMF/MA/AAO/TTT/VQT等)はアバターのアセットバンドルに含まれない**(コンポーネントはビルド時に消える)。→ツールのライセンスがアップロード物に波及することは基本ない
- 含まれるのは: メッシュ・テクスチャ・マテリアル・シェーダー(ロック生成物含む)・アニメーション・音声。配布/公開の判断はこちらの規約で行う

## 関連ページ

- 非対応衣装ワークフロー: [03-fitting-tools.md](03-fitting-tools.md)
- Poiyomiのエディション: [06-poiyomi.md](06-poiyomi.md)


======================================================================
# FILE: 19-triage-guide.md
======================================================================

# 問診ガイド(曖昧な相談の入口)【RAGはまずここを読む】

このKBの利用者(相談者)は**改変の初心者**が前提。「なんか知らんけどエラー吐いた」「服着せたい」「表情バグった」レベルの相談から、**確認質問で状況を絞り込み→原因候補→平易な言葉で対処を案内**するためのガイド。

## 回答時の原則(LLM向け)

1. **確認質問は一度に最大3つ、できるだけ選択肢形式で**(「〜ですか?それとも〜?」)。質問攻めにしない
2. 最初に確定させるのは**「どの段階で起きたか」**: ①Unityで編集中 ②Playボタンを押したとき ③アップロード中 ④VRChatの中。これで原因の半分は絞れる
3. 次に**「直前に何をしたか」**: 何かを入れた/更新した/消した直後のトラブルが大半。「最後にやった操作」を聞く
4. エラーがある場合は**赤い文字の最初の1行**(またはスクショ)をもらう。Unityの下部Console、もしくはビルド時に出るNDMF Consoleウィンドウ
5. 回答は**番号付きの手順**で。専門用語には一言説明を添える(例:「FX(=表情やトグルを動かすアニメーション設定)」)
6. **破壊的な操作(削除・上書き)を勧める前にバックアップ(プロジェクトのコピー)を必ず案内**
7. 決めつけない。原因候補が複数あるときは「一番ありそうな順」に1つずつ試してもらう

## ユーザーの言葉 → 技術的な意味(対訳表)

| ユーザーの言い方 | 技術的に確認すべきこと | 主な参照 |
|---|---|---|
| 「落ちた」 | Unityが強制終了? VRChatがクラッシュ? アップロードが失敗? | [17](17-unity-troubleshooting.md) |
| 「止まった」「固まった」 | Unityフリーズ(応答なし)? ビルドの進捗が進まない? 読込中のまま? | [17](17-unity-troubleshooting.md) |
| 「エラー吐いた」 | Console(下のログ)の赤字? ポップアップ? NDMF Console? VRChat内の表示? | [00 §3](00-cross-tool.md) / [17](17-unity-troubleshooting.md) |
| 「服着せたい」 | そのアバター**対応**の衣装? 別アバター用(**非対応**)? | [02](02-modular-avatar.md) / [03](03-fitting-tools.md) |
| 「服が爆発した」「服が飛んでった」 | Merge Armature(衣装結合)のボーン対応失敗 | [02](02-modular-avatar.md) |
| 「表情バグった」 | 動かない? 固まる? 混ざる? 特定ワールドだけ? | 本ページ§D |
| 「瞬きできない」 | まばたき(自動)が止まった | 本ページ§D-3 |
| 「Unityだと正常なのにVRCだと変」 | **ビルド時ツールかアニメーターによる上書き**。再現条件の切り分けへ | 本ページ§E |
| 「ピンク/紫になった」 | シェーダー欠落 | [17](17-unity-troubleshooting.md) |
| 「真っ黒/真っ白/光りすぎ」 | ライティング設定 or ワールド側 | [05](05-liltoon.md) / [16](16-world-integrations.md) / [07](07-expression-gimmick-tools.md) |
| 「Questの人からロボットに見える」 | フォールバック表示(Quest版未アップロード or ランク超過) | 本ページ§H |
| 「マークが赤い」「重いって言われた」 | Performance Rank | [13](13-vrchat-avatars-basics.md) / [09](09-avatar-optimizer.md) |
| 「ボタン押しても反応しない」 | メニュー/パラメータ/同期の問題 | 本ページ§G |

---

## 症状別問診フロー

### A. 「なんか知らんけどエラー吐いた/止まった/落ちた」

**確認質問**: ①いつ?(Unity起動時/編集中/Play押した時/アップロード時) ②直前に何を入れた・更新した? ③赤いエラーの最初の1行は?(スクショでOK)

| 状況 | ありそうな原因(順) | 参照 |
|---|---|---|
| 何かを導入・更新した直後 | ツールのバージョン不整合(片方だけ更新)→**VCC/ALCOMで全部最新に揃える**が第一手 | [00 §3.1](00-cross-tool.md) |
| アップロード時だけ失敗 | サイズ超過(Quest 10MB等)/Read-Write設定/ID不整合 | [17](17-unity-troubleshooting.md)「アップロードだけ失敗」 |
| Unity起動時から赤字 | 二重導入(unitypackageとVCC両方で入れた)/旧フォルダ残り | [17](17-unity-troubleshooting.md) |
| Unityが固まる・落ちる | Library破損→削除して再インポート(時間がかかるだけで安全) | [17](17-unity-troubleshooting.md) |

**注意**: 「コンパイルエラーが1個でもあると、非破壊ツール全部が動かなくなる」ことを平易に伝える(「赤いエラーが残ってると他も全部止まります」)。

### B. 「服を着せたい」

**確認質問**: ①その服は着せたいアバター用として売られてたもの?(商品ページに対応アバター名がある?) ②アバターと服の名前は?

- **対応衣装** → [MA](02-modular-avatar.md)で完結: 服のプレハブをアバターの中に入れて「Setup Outfit」(右クリック→Modular Avatar)。ほとんどの現行衣装はこれだけ
- **非対応衣装** → [もちふぃった～](03-fitting-tools.md)(有償2,500円)+両方のアバターの「プロファイル」が必要。プロファイルが無い場合は難易度が跳ね上がることを正直に伝える
- 着せた後の貫通・はみ出し → 服に入っている「シュリンク(縮める)」系のシェイプキー活用 → 無ければ[EreMorph](03-fitting-tools.md)か[MA Mesh Cutter](02-modular-avatar.md)で素体側を消す

### C. 「服がおかしい(爆発/変な位置/ついてこない)」

**確認質問**: ①服を入れた手順は?(Setup Outfit使った?手でドラッグしただけ?) ②アバターを動かすと服だけ残る?一緒に動く?

| 症状 | 原因候補 | 参照 |
|---|---|---|
| 爆発・ぐちゃぐちゃ | Merge Armatureのボーン対応失敗(服が別アバター用/ボーン名不一致) | [02](02-modular-avatar.md)よくあるトラブル |
| 服が地面に残る・ついてこない | Merge Armature未設定(ただ入れただけ) → Setup Outfitをやり直す | [02](02-modular-avatar.md) |
| 位置・サイズが微妙にずれる | アバター側を改変済み(体型調整後)→ Scale Adjuster/[フィッティング](03-fitting-tools.md) | [02](02-modular-avatar.md)/[03](03-fitting-tools.md) |
| 動くと貫通する | シュリンクキー未設定/PhysBoneの揺れ違い | [02](02-modular-avatar.md) |

### D. 「表情がおかしい」

**確認質問**: ①どうおかしい?(動かない/1つの表情で固まった/混ざって崩れる/まばたきしない) ②いつから?(何かを入れた直後?) ③特定のワールドだけ?(MMD系ワールド?)

1. **表情が全く動かない・固まった**: FX(表情のアニメーション設定)が壊れたか、Write Defaults(WD)設定の混在。表情ツール(FaceEmo等)や大きなギミックを入れた直後なら、それを一旦抜いて確認 → [07](07-expression-gimmick-tools.md)/[02](02-modular-avatar.md)
2. **特定ワールド(MMD/ダンス系)だけ死ぬ**: 仕様に近い既知問題。MMDワールドはFXの一部を乗っ取る → MA 1.12+のMMD対応と`MA VRChat Settings`で挙動制御 → [02](02-modular-avatar.md)
3. **瞬きしなくなった**:
   - 表情メニューで表情を固定していないか(まず表情リセット)
   - まばたきは「アバター設定(VRC Avatar Descriptor)のEyelids」で動く。改変でFace側のメッシュ名やシェイプキー名を変えた/消したら壊れる
   - 表情アニメが目のシェイプキーを**常時**上書きしている(WD ONで全シェイプを0で焼き込んだ表情アニメが典型)
   - 最適化ツール(AAO)がまばたき用シェイプキーを固定した可能性 → AAOのExclusionsに顔メッシュを入れて切り分け → [09](09-avatar-optimizer.md)
4. **笑うと目が開かない等、表情が混ざる**: 複数の表情システムの二重導入(元の表情+FaceEmo等)。どちらか一方に統一 → [07](07-expression-gimmick-tools.md)

### E. 「Unityでは正常なのに、VRChatに持っていくと変になる」(例: 腕を細くしたのに戻ってる)

これは**このKBで最頻出の相談型**。Unityのシーン上の見た目と、VRChat内の見た目は「ビルド時ツール(非破壊改変)」と「アニメーション」を通った後の姿なので一致しないことがある。

**確認質問**: ①何が戻る?(体型のシェイプキー/服/色) ②UnityでPlayボタンを押した状態でも戻る? ③最適化ツール(AAOなど)入れてる?

**切り分けの黄金ルート**(相手にそのまま案内できる):
1. UnityでPlayを押す(=非破壊ツールが全部適用された状態)。**Playで再現するなら**ビルド時ツールか改変設定の問題 → 2へ。**Playでは正常でVRChat内だけおかしいなら**ワールド/セーフティ/Quest系 → §F/§Hへ
2. 「腕を細くした」の方法を確認:
   - **シーンでシェイプキーの数値を変えただけ** → アバターの表情/リセットアニメ(WD ONや初期化アニメ)がそのシェイプキーを**毎フレーム0に上書き**している可能性が最有力。対処: そのシェイプキー名がFX内のアニメに含まれていないか確認、または「シェイプキーを固定する」方法(AAO Freeze BlendShapeやMAの仕組み)で**値を焼き込む** → [09](09-avatar-optimizer.md)/[02](02-modular-avatar.md)
   - **AAO(Trace and Optimize)を入れている** → 自動シェイプキー凍結が「アニメで動く値」を基準に固定した可能性。AAOのExclusionsで顔/体メッシュを除外して再確認 → [09](09-avatar-optimizer.md)
3. **服だけ体型に追従しない**(素体は細いのに服が太い→貫通): 服側の同名シェイプキーが動いていない → [MA Blendshape Sync](02-modular-avatar.md)を素体→服に設定
4. 色・材質が変わる系は§Fへ

### F. 「色・材質がおかしい」

**確認質問**: ①どうおかしい?(ピンク/真っ黒/白飛び/テカる/特定ワールドだけ暗い) ②Unity上でもおかしい?VRChat内だけ?

| 症状 | 原因候補 | 参照 |
|---|---|---|
| ピンク/紫 | シェーダー未導入・バージョン違い(lilToon/Poiyomi入れ直し) | [17](17-unity-troubleshooting.md) |
| アップロード後だけ色や効果が消える | シェーダーのビルド時最適化(lilToonは1.8未満で顕著/Poiyomiはロック時のAnimated未指定) | [05](05-liltoon.md)/[06](06-poiyomi.md) |
| 特定ワールドだけ暗い/明るい | ワールドの照明仕様(Light Volumes等)/シェーダーが古い → 明るさ調整メニュー(Light Limit Changer)導入も提案 | [16](16-world-integrations.md)/[07](07-expression-gimmick-tools.md) |
| 他人から見ると簡素な見た目 | 相手のセーフティでシェーダーOFF(フォールバック表示)。自分では直せない部分もあると説明 | [13](13-vrchat-avatars-basics.md) |

### G. 「メニュー/トグルがおかしい」

**確認質問**: ①メニュー自体が出ない?押しても反応しない?反応するけど他人に見えない? ②そのトグルは何で作った?(衣装付属/自作/ツール名)

- メニューが出ない → MA Menu Installerの設定/導入手順 → [02](02-modular-avatar.md)
- 押しても反応しない → パラメータ名の不一致/FXに届いていない → [02](02-modular-avatar.md)
- **自分には効くが他人に見えない** → パラメータの「同期(synced)」設定と**容量(256bit)超過** → [13](13-vrchat-avatars-basics.md)/[02](02-modular-avatar.md)
- PCでは動くのにQuestで壊れる → §H

### H. 「Quest(スマホ)の人から見えない/ロボットになる」

**確認質問**: ①そのアバター、Quest用にもアップロードした?(PC用とQuest用は**別々にアップロードが必要**) ②アップロードしたなら、ランク表示は何色だった?

1. **Quest版を未アップロード** → それが原因。Quest対応には[VQT](11-vrcquesttools.md)で変換が必要(lilToon/PoiyomiはQuestでは使えないため)
2. アップロード済みでも**Very Poor**だとQuest側では原則表示されない(相手が個別に「Show Avatar」しない限り) → ランク基準は[13](13-vrchat-avatars-basics.md)、削減は[09](09-avatar-optimizer.md)/[10](10-optimization-conversion-tools.md)
3. PC/Quest両方入れたのに**動きや表情がズレる** → 同期ずれ: [MA Sync Parameter Sequence](02-modular-avatar.md)+[VQT Network ID Assigner](11-vrcquesttools.md)

### I. 「重いって言われた/マークが赤い」

**確認質問**: ①誰に言われた?(フレンド/イベント主催) ②今のランク色は?(アバターメニューで見える)

1. まず現状把握: [Actual Performance Window](12-analysis-upload-tools.md)でビルド後の実測値、[lilAvatarUtils](12-analysis-upload-tools.md)でテクスチャ(VRAM)を見る
2. 一番効くのは大抵**テクスチャ(VRAM)とメッシュ数**。[AAO Trace and Optimize](09-avatar-optimizer.md)を入れるだけでかなり下がる(非破壊なので安全)と案内
3. 詳細な進め方は[10の全体戦略](10-optimization-conversion-tools.md)

---

## 問診でも絞れなかったとき

- Console/NDMF Consoleの**エラー全文**をもらう → エラー文言でKB内検索([00 §3](00-cross-tool.md)の症状表、各ツールの「よくあるトラブル」)
- 「入れているツールと大体のバージョン」を列挙してもらう → [00 §2](00-cross-tool.md)の互換表・推奨組み合わせと突き合わせ
- 最終手段の定型: プロジェクトをコピー(バックアップ)→ VCC/ALCOMで主要ツールを全部最新へ → それでもダメならツールを1つずつ無効化([00 §3.1](00-cross-tool.md)の切り分け手順)

