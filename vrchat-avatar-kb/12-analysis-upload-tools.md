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
- 導入(VPM): VCC Curated(VCC同梱リスト、独自リポジトリ追加不要)

Unityエディタ内でアバターのアニメーションをプレビュー・編集するツール。Play Modeで**Expression MenuのRadial Menuを再現**し、トグルやパペットを実際の見た目で試せる。ジェスチャー(手形状)の組み合わせテストも可能。

- NDMFのApply on Playと共存する(NDMF適用後のアバターをそのまま操作できる)ため、**MA/AAO/TTT適用結果のメニュー動作確認の標準手段**
- READMEの「Unity 2018/2019」記述は古い(現行はVRCSDK3+Unity 2022環境で使用されている)
- 類似のAv3Emulatorとの使い分け: メニュー操作の確認はGesture Manager、同期やState Behaviourの厳密な検証はAv3Emulator

## Av3Emulator(Lyuma)

- リポジトリ: https://github.com/lyuma/Av3Emulator
- 導入(VPM): VCC Curated(VCC同梱リスト、独自リポジトリ追加不要)

Avatars 3.0のランタイム挙動をUnityの**PlayableGraph APIで再実装**したエミュレータ。Play Modeでアバターにランタイムコンポーネントが付与される。

- 特徴: **非ローカルクローンの生成による同期テスト**(synced/local変数の見え方の違いを検証できる数少ない手段)、Animatorウィンドウでのレイヤーライブ表示、Gesture weight(アナログFist)、Expression Menu、Parameter Driver、Viseme、OSC対応
- **既知の相互作用**(KB [00](00-cross-tool.md)/[06](09-avatar-optimizer.md)参照): NDMFはAv3Emulator検出時に自前のApply on Playの二重適用を抑止する。AAOはAv3Emulator起動時にRead/Write無効メッシュを処理できない場合がある(AAO 1.8.0で大幅改善)
- 「Play時だけ壊れる」系トラブルの切り分けでは、Av3Emulator/Gesture Manager/素のPlayの3通りで比較する

## lilAvatarUtils(lilxyzw)

- リポジトリ: https://github.com/lilxyzw/lilAvatarUtils
- パッケージ名: `jp.lilxyzw.avatar-utils` / 現行 2.1.x(Unity 2022.3、MIT)
- 導入(VPM): lilxyzw.github.io/vpm-repos

アバター分析ウィンドウ(`Tools/lilAvatarUtils`)。軽量化の**現状把握**に使う(実際の削減は[AAO](09-avatar-optimizer.md)/[TTT](04-textranstool.md)の仕事)。

- **テクスチャ一覧**: VRAMサイズ・解像度・圧縮形式を一覧表示し、インポート設定をまとめて変更可能
- **マテリアル/アニメーション一覧**: 使用箇所の逆引き
- **ライティングテスト**: 各種ワールド照明条件での見た目確認
- **セーフティ(シェーダーフォールバック)のシミュレーション**: 2.1.0でToon Standardフォールバック対応(VRChat仕様変更「Unlitフォールバックの打ち切り廃止」にも追従)
- バージョン要点: 2.0.0 (2025-01)でローカライズ対応・メニュー位置変更(`Tools/lilAvatarUtils`)。2.1.1でマテリアルバリアント(親)のスキャン修正

## anatawa12's gist pack / Actual Performance Window(anatawa12)

- リポジトリ: https://github.com/anatawa12/unity-gist-pack
- パッケージ名: `com.anatawa12.gists` / 0.25.x(MIT)
- 導入(VPM): vpm.anatawa12.com

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
