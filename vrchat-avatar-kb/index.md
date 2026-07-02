# VRChatアバター改変 ナレッジベース

VRChatアバターの非破壊改変(NDMFエコシステム)に関するナレッジベース。LLMセッションのRAGソースとして、任意のアバターに対する改変手順の生成・トラブルシュートに使うことを想定している。

## 前提環境

- Unity 2022.3.22f1
- VRCSDK3 Avatars(バージョン更新が頻繁なため記述はバージョン非依存を基本とし、依存する挙動は「どのバージョン以降か」を明記)
- 対象アバターは特定しない(汎用)
- 基本はPC向け。Quest(Android/iOS)対応の制約・注意点は各ツールの「Quest対応時の注意」に記載

## 情報源と鮮度

各ツールのGitHubリポジトリ(README / CHANGELOG / ソースコード / 公式ドキュメント)を直接読解して作成。2026年前半時点のリポジトリHEADに基づく。実行順序・フェーズ構成などパイプラインに関する記述は、各ツールのNDMFプラグイン定義ソースを一次情報とする(ドキュメントと齟齬がある場合はソースを正とし、その旨を明記)。

## ファイル構成

| ファイル | 内容 |
|---|---|
| [00-cross-tool.md](00-cross-tool.md) | **ツール横断情報**: NDMFビルドパイプライン上の全ツールの実行順序と処理フェーズ / 互換性マトリクス / 共通トラブルシュートフロー / 推奨バージョン組み合わせ |
| [01-ndmf.md](01-ndmf.md) | **NDMF** — 非破壊ビルドの基盤フレームワーク(フェーズ、順序解決、プレビュー、AnimatorServices) |
| [02-modular-avatar.md](02-modular-avatar.md) | **Modular Avatar** — 衣装統合(Merge Armature)、アニメーター/メニュー/パラメータ合成、リアクティブコンポーネント、Mesh Cutter |
| [03-textranstool.md](03-textranstool.md) | **TexTransTool** — デカール、レイヤー合成(PSD)、マテリアル改変、テクスチャアトラス化 |
| [04-liltoon.md](04-liltoon.md) | **lilToon** — シェーダー(日本圏標準)。マテリアル調整、ビルド時最適化の癖 |
| [05-poiyomi.md](05-poiyomi.md) | **Poiyomi Toon Shader** — シェーダー(英語圏標準)。ロック機構とAnimated指定 |
| [06-avatar-optimizer.md](06-avatar-optimizer.md) | **AAO: Avatar Optimizer** — Trace And Optimizeによる自動最適化、メッシュ/PhysBone/アニメーター/テクスチャ最適化 |
| [07-vrcquesttools.md](07-vrcquesttools.md) | **VRCQuestTools** — Quest/Android/iOS変換(マテリアルベイク、コンポーネント除去、プラットフォーム出し分け) |

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

- 「実行順序」「どのツールが先か」「フェーズ」→ 00-cross-tool.md §1
- 「AとBの併用で壊れる」→ 00-cross-tool.md §2(互換性マトリクス)+各ツールの「よくあるトラブル」
- 「ビルドエラー」「アップロードだけ壊れる」→ 00-cross-tool.md §3(トラブルシュートフロー)
- 「衣装を着せる」「トグルを作る」→ 02-modular-avatar.md
- 「テクスチャ改変」「アトラス化」→ 03-textranstool.md
- 「軽量化」「Performance Rank改善」→ 06-avatar-optimizer.md(+03のAtlasTexture)
- 「Quest対応」→ 07-vrcquesttools.md +各ツールの「Quest対応時の注意」
- 「このバージョンで何が変わった」→ 各ツールの「バージョン履歴」
