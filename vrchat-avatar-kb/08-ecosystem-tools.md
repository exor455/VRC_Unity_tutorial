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
- 関連ツール: [lilToon](04-liltoon.md)(同一作者)、[TexTransTool](03-textranstool.md)/[VRCQuestTools](07-vrcquesttools.md)(順序制約あり)

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
