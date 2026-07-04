# VRChatアバター改変 ナレッジベース

VRChatアバターの非破壊改変(NDMFエコシステム)に関するナレッジベース。LLMセッションのRAGソースとして、任意のアバターに対する改変手順の生成・トラブルシュートに使うことを想定している。

**このファイルの直リンク(LLMに貼るとき用)**:
```
https://raw.githubusercontent.com/exor455/VRC_Unity_tutorial/main/vrchat-avatar-kb/index.md
```

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

- **「オブジェクト/アクセサリをボーンに付けたい」「シーンをスクリプト/エージェントで直接操作する」→ まず00-cross-tool.md §0(非破壊の絶対原則)。直接ボーン配下へReparentするのはアンチパターン、MA Bone Proxyを使う**
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
