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
- 導入(VPM): suzuryg.github.io/vpm-repos

**表情作成・設定の統合ツール**(日本圏で最も普及した表情システムの一つ)。ハンドジェスチャーとExpression Menuを組み合わせた表情切り替えを、専用GUIで表情パターン(モード)単位に構築する。

- できること: ジェスチャー組み合わせへの表情割り当て、メニューからの表情モード切替/表情固定、まばたき・リップシンク干渉の制御、表情プレビュー、既存表情アニメの取り込み
- **Modular Avatarと組み合わせて非破壊にFXへ統合する構成が標準**(生成物はMA経由でマージされる)
- 注意: 表情はFXレイヤーを大きく占有するため、[AAO](09-avatar-optimizer.md)のOptimize Animatorとの併用で最適化はAAO側に任せる。MMDワールド互換はMA側の仕組み(KB [02](02-modular-avatar.md))に依存
- **「アバターに適用」を押してもGesture Managerで確認すると元に戻る**(公式チュートリアルに既知の注意事項として記載): Gesture Manager/MAのバージョンの組み合わせにより、プレビューに表情メニューが反映されないことがある。対処: ①Gesture Managerのオブジェクトを**選択していない状態**でPlayを押す ②Playモードに入ってから改めてGesture Managerのオブジェクトを選択する ③Gameタブで反映されなければSceneタブで確認 ④Action(AFK)レイヤーのAFKチェックボックスがONだと無操作モーション優先で表情が隠れることがあるのでオフにする。それでも直らなければGesture Manager/MAを両方最新版へ

## ComboGestureExpressions(Haï)

- リポジトリ: https://github.com/hai-vr/combo-gesture-expressions-av3
- ドキュメント: https://docs.hai-vr.dev/docs/products/combo-gesture-expressions
- 導入(VPM): hai-vr.github.io/vpm-listing

英語圏定番の表情ツール。ジェスチャー組み合わせに表情を割り当てるのに加え、**Contacts / PhysBones / OSC / コントローラートリガー圧**で表情をブレンドできるのが特徴。

- 「まばたき付き表情で目を閉じたときの瞬き干渉防止」「トリガー圧アニメーションの外部視点補正(corrections)」など、細部の作り込みが強み
- V3でVCC対応。Animator As Code(同作者)の系譜で、生成されるレイヤーは高度なblend tree構成
- FaceEmoとの使い分け: 日本語UI・モード管理重視ならFaceEmo、Avatar Dynamics連動やアナログ表現重視ならCGE

## Light Limit Changer(Azukimochi)

- リポジトリ: https://github.com/Azukimochi/LightLimitChangerForMA
- ドキュメント: https://azukimochi.github.io/LLC-Docs/
- パッケージ名: `io.github.azukimochi.light-limit-changer` / 1.14.x
- NDMFプラグインID: `io.github.azukimochi.light-limit-changer`
- 導入(VPM): azukimochi.github.io/vpm-repos

**シェーダーの明るさ下限/上限を操作するアニメーションとメニューを自動生成**するツール。ワールドの照明が暗すぎる/明るすぎる場合にユーザー自身がメニューで調整できるようにする、日本圏でほぼ標準装備のギミック。

- 対応シェーダー: **lilToon / Poiyomi / Sunao**(現行)
- 依存: VRCSDK >=3.2、**Modular Avatar ^1.9.9、NDMF ^1.4**。MAプレハブ形式で生成されるため非破壊でFXを直接変更しない
- **実行順序(重要)**: TTTがLLCより先に実行される制約を宣言している(`BeforePlugin`)。テクスチャ/マテラルを差し替えるツールとの順序問題が既知のため、LLC関連の明るさ異常はまずツール群のバージョンを最新に揃える
- Poiyomi使用時の注意: 明るさ系プロパティがロックでAnimated指定されている必要がある(LLCのセットアップが面倒を見るが、手動ロック運用時はKB [Poiyomi](06-poiyomi.md)の原則が適用される)

## FloorAdjuster(narazaka)

- リポジトリ: https://github.com/Narazaka/FloorAdjuster
- パッケージ名: `net.narazaka.vrchat.floor-adjuster`
- NDMFプラグインID: `net.narazaka.vrchat.floor_adjuster`
- 導入(VPM): vpm.narazaka.net

**アバターの上下位置(接地)を非破壊に調整**するツール。ヒール靴などで浮く/沈むアバターの定番修正手段。MA互換。現行は「by skeleton(新方式)」セットアップが推奨。

- **実行順序の履歴(重要)**: Transformを動かすため、TTT(デカール位置)と衝突した過去がある → TTT 0.8.2で「TTTがFloorAdjusterより先に実行」に修正済み
- **MA 1.17.0以降はMA本体に`MA Floor Adjuster`が内蔵**され、MA側はTTT等の後に実行するよう順序制御されている(KB [02](02-modular-avatar.md))。新規はMA内蔵版、既存アバターはnarazaka版が残っていても問題ない(両方入れるのは避ける)

## Avatars 3.0 Manager(VRLabs)

- リポジトリ: https://github.com/VRLabs/Avatars-3.0-Manager
- パッケージ名: `dev.vrlabs.av3manager`
- 導入(VPM): VCC Curated(VCC同梱リスト、独自リポジトリ追加不要)

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
- **「FaceEmoで適用したのにGesture Managerだと元に戻る/反映されない」**: FaceEmo公式の既知事項(上記FaceEmo節を参照)。Gesture Managerを選択せずにPlay→Play後に選択、の順で回避

## 関連ページ

- メニュー/トグル生成(AMCFMA / lilycalInventory / Flare)と**トゥイーン・フェード比較表**: [08-ecosystem-tools.md](08-ecosystem-tools.md)
- 検証・分析・アップロード: [12-analysis-upload-tools.md](12-analysis-upload-tools.md)
- 軽量化・変換: [10-optimization-conversion-tools.md](10-optimization-conversion-tools.md)
