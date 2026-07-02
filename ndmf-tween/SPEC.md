# NDMF Tween Generator 仕様書(ドラフト)

「開始値・終了値・秒数(+イージング)を指定するだけで、AnimationClipとAnimator遷移をビルド時に自動生成する」NDMF準拠コンポーネント。VRCFury非依存。

- 対象: VRChat Avatars 3.0 / Unity 2022.3
- 前提: NDMF + Modular Avatar(メニュー/パラメータ統合に使用)
- 関連ナレッジ: [vrchat-avatar-kb](../vrchat-avatar-kb/index.md)(特に 00/01/02/06)

> **決定記録 (2026-07-02)**: 仕様確認の質問がUI切断で届かなかったため、以下は推奨案で確定して実装済み。変更希望があれば改訂する。
> ①方針=新規実装(既存ツールの調査結果は§0参照) ②遷移方式=Auto(ハイブリッド。CrossFade/BakedClips両実装) ③MVPトラック=全4タイプ(Blendshape/Material Float/Material Color/Transform)+Delay/Curve Overrideも初版に含めた

---

## 0. 事前調査の結果(ゴール1: 既存ツールの再検証)

依頼時の前提「同種のコンポーネントは存在しない」は**部分的に誤り**。以下の2つが「値+秒数→アニメーション自動生成」を既に提供している:

| ツール | 該当機能 | 方式 | 制約 |
|---|---|---|---|
| **AvatarMenuCreatorForMA** (narazaka) | ON/OFFメニューの「徐々に変化」(v1.2.0〜) | 遷移秒数+項目ごとの変化待機%/変化時間%。BlendShape/シェーダーパラメータ対応。MAコンポーネント/プレハブとして生成 | **線形補間のみ**(v1.11で線形化修正)。メニュー作成ツールの一機能であり、単体トゥイーンコンポーネントではない。Poiyomiロック名への特別対応なし |
| **Flare** (Auros) | Flare Controlの「Interpolate」(秒数指定) | 0→1/1→0をduration秒かけて遷移。「simple dissolve effects」を公式ユースケースに明記。NDMFベース | **独自メニューシステム**(MA Menu Item非統合)。線形のみ。WDはFlareSettingsで別管理。開発は現在低速 |

周辺の類似・非該当:
- **lilycalInventory SmoothChanger** (lilxyzw): Float+Radial Puppetでフレーム間を補間する「手動無段階」方式。秒数指定の自動再生トゥイーンではない(用途が異なる)
- **MAリアクティブ** (Object Toggle/Shape Changer/Material Setter/Swap): 瞬間切替のみ(依頼時認識どおり)
- **Vixen** (Haï): Flareの着想元。現在アクティブな選択肢ではない。Prefabulousにも該当機能なし(依頼時認識どおり)
- **AAC + NDMF Processor**: コードベース(ノーコード要件を満たさない。依頼時認識どおり)
- [awesome-ndmf](https://github.com/Spokeek/awesome-ndmf)の一覧上、上記以外に該当ツールなし

**結論**: 「トグル+秒数のフェード」だけならAvatarMenuCreatorForMAで実現可能。ただし依頼要件のうち
①**イージング(非線形/任意カーブ)**、②**MA Menu Item/MA Parametersへの素直な統合**(独自メニューUIを持たない単機能コンポーネント)、③**Poiyomiロック済みプロパティ名の解決支援**、④**開始値・終了値を明示指定する汎用トゥイーン**(現在値に依存しない)——の4点を同時に満たすものは存在しない。この4点を差別化仕様として新規実装する価値がある(判断は下記「実装方針の決定」参照)。

---

## 1. ポジショニング

- **単機能・合成可能(composable)**: メニューは作らない。パラメータ名を介してMA Menu Item / MA Parametersと接続する「MAリアクティブの時間軸版」
- **非破壊**: NDMF Generatingフェーズで、MAが消費するコンポーネント(Merge Animator / Parameters)を生成するだけ。独自のFX直接改変はしない(KB 01-ndmf.md「Generatingフェーズの想定用途」準拠)
- 名称(仮): パッケージ `dev.exor455.ndmf-tween` / コンポーネント **`Tween Toggle`**(メニュー表示: `NDMF Tween/Tween Toggle`)

## 2. コンポーネント仕様(ユーザーが触る部分)

`TweenToggle` — アバター配下の任意のGameObjectに追加(複数追加可)。

### 共通設定
| フィールド | 型 | 既定 | 説明 |
|---|---|---|---|
| Parameter Name | string | 自動("Tween/<オブジェクト名>") | トリガーとなるBoolパラメータ名。MA Menu Item(Toggle)と一致させる |
| Default On | bool | false | 初期状態(MA Parametersのdefault値に反映) |
| Saved / Synced | bool | true / true | MA Parameters設定に反映(Bool=1bit) |
| Duration (s) | float | 1.0 | トゥイーン秒数(ON方向)。 |
| Reverse Duration (s) | float | -1 | OFF方向の秒数。負値ならDurationと同じ |
| Easing | enum | Linear | Linear / InQuad / OutQuad / InOutQuad / InOutCubic / InOutSine / Custom |
| Custom Curve | AnimationCurve | 0-1線形 | Easing=Custom時の正規化カーブ(t:0→1, v:0→1) |
| Transition Mode | enum | Auto | Auto / CrossFade / BakedClips(§4参照)。Auto=Easing==LinearならCrossFade、それ以外はBakedClips |
| Cancel Blend (s) | float | 0.15 | BakedClips時、トゥイーン中に逆方向へ切り替えた際のクロスフェード秒数 |

### トラック(複数、ReorderableList)
| フィールド | 説明 |
|---|---|
| Target Type | Blendshape / Material Float / Material Color / Transform |
| Target | SkinnedMeshRenderer(Blendshape) / Renderer(Material系) / Transform |
| Property | ピッカーで選択: Blendshape名一覧 / 対象RendererのsharedMaterialsから列挙したプロパティ(マテリアル別グループ表示) / localPosition・localEulerAngles・localScale |
| Start Value / End Value | float(Blendshape 0-100 / Material Float)、Color(Material Color)、Vector3(Transform) |
| Delay (s) | 0 | このトラックの開始遅延(順次演出用)。BakedClipsモードでのみ有効 |
| Curve Override | 任意 | トラック個別のイージング上書き(任意) |

### Inspector支援機能
- **「Set from current」**: 現在のシーン値をStart/Endへ取り込むボタン
- **「Create MA Menu Item」ボタン**: 同一GameObjectに `MA Menu Item`(Toggle型・同名パラメータ)と、`MA Parameters` のエントリをワンクリック生成(エディタ操作として。既存があれば何もしない)。※コンポーネント自体はビルド時にParameters設定を自動注入するため、このボタンは「メニューに出したい人」向けの補助
- **バリデーション表示**: 対象欠落 / プロパティ不在 / Duration<=0 等をHelpBoxで警告

## 3. Poiyomiロック機構への対応(要件④)

背景はKB [06-poiyomi.md](../vrchat-avatar-kb/06-poiyomi.md) 参照(ロック時にAnimated指定必須+Rename Animatedでプロパティ名が`_Prop_<マテリアル名由来サフィックス>`にリネームされる)。

1. **ピッカーは常に「現在のsharedMaterialsの実プロパティ名」を列挙**する。ロック済みマテリアルならリネーム後の名前がそのまま選べる(=最も確実な経路)
2. **ビルド時リマップ(既定ON)**: 指定プロパティが対象マテリアルに存在しない場合、`指定名+"_"`で始まるプロパティを検索。**一意に見つかれば自動リマップ**してInfoログ、複数/ゼロなら警告(候補名を列挙)。これによりアンロック時に設定→後からロックしたケースを救済
3. **Animated未指定の検出(ベストエフォート)**: 対象マテリアルのシェーダー名に"poiyomi"を含み、かつThryEditorのAnimatedタグ(`material.GetTag("<prop>Animated")`)が立っていない場合、Inspector/ビルド時に警告。※タグ仕様は実装時にThryEditorソースで検証し、取れなければ警告文言のみ(ドキュメント誘導)に落とす
4. ドキュメントに「ロック運用の推奨手順」(Animated指定→ロック→ピッカーで選択)を記載

## 4. 生成物仕様(ビルド時)

### 4.1 NDMFプラグイン
- QualifiedName: `dev.exor455.ndmf-tween`
- **Generatingフェーズ**で全処理。MAより先に実行される順序は**フェーズ分離のみで保証**する(MAはGeneratingにパスを持たないため`BeforePlugin`宣言はクロスフェーズ制約になる恐れがあり、宣言しない)
- 手順: アバター配下の全`TweenToggle`を列挙 → 各コンポーネントについて AnimatorController(1レイヤー)+AnimationClip群を生成し `IAssetSaver.SaveAsset` で保存 → 同一GameObjectに `ModularAvatarMergeAnimator`(FX / **Absolute** pathMode / Match Avatar Write Defaults ON / Delete Attached Animator OFF)と `ModularAvatarParameters`(Boolエントリ)を追加 → `TweenToggle`コンポーネントを削除
- 以降のマージ・パス書き換え・WD調整・Merge Armatureによる移動追従は**すべてMAに委譲**(KB 02の処理順: Merge AnimatorはMerge Armatureより前に走り、その後の移動はAnimatorServicesがパス追従)
- クリップのバインディングパスはアバタールート相対で生成(Absoluteモード前提)
- エラー通知: NDMF Console(ErrorReport)+Debugログ。致命的(対象欠落等)はビルド警告に留めスキップ(ビルドは止めない)

### 4.2 Animatorレイヤー構造

**CrossFadeモード**(Easing=Linear時の既定。VRCFury Transition Time / Flare Interpolateと同方式):
```
[Off] (1Fクリップ: 全トラック=Start値)
  --(param==true,  hasExitTime=false, duration=T秒, fixedDuration)--> [On]
[On]  (1Fクリップ: 全トラック=End値)
  --(param==false, hasExitTime=false, duration=T'秒)--> [Off]
遷移は Interruption Source: Current State Then Next State で相互割り込み許可
```
- 長所: 途中でトグルを戻しても現在のブレンド位置から滑らかに逆転。クリップが軽い
- 制約: 線形補間のみ。Delayトラック不可(全トラック同時)

**BakedClipsモード**(イージング/Delay使用時):
```
[Off](1F Start保持) --(param==true)--> [TweenIn](T秒クリップ: Start→End, イージング焼き込み)
[TweenIn] --(exitTime=1.0)--> [On](1F End保持)
[On] --(param==false)--> [TweenOut](T'秒クリップ: End→Start)
[TweenOut] --(exitTime=1.0)--> [Off]
中断遷移: [TweenIn]--(param==false, duration=CancelBlend)-->[TweenOut](逆も同様)
```
- イージングはAnimationCurveをキーとして焼き込み(Custom含む。20サンプル/カーブ+端点タンジェント調整)
- Delayは該当トラックのキー開始をずらす(クリップ全長=max(delay+duration))
- 制約: トゥイーン中の反転は`Cancel Blend`秒のクロスフェードで近似(完全な位置反転はUnity Animatorでは不可能なため仕様として明記)

共通:
- パラメータはBool 1個(Animator側にも同名Boolを追加)
- 生成レイヤー名: `Tween/<パラメータ名>`
- WD: ステート自体はWD値を持つが、MAの`Match Avatar Write Defaults`(1.16.1+で既定ON)に整合させる。全ステートが同一プロパティ集合をアニメーションするため、WD ON/OFFどちらのアバターでも自己完結
- 初期状態: Default On=trueの場合は[On]をデフォルトステートにし、パラメータdefault=1(「ロード直後に遷移アニメが流れる」問題の回避。AvatarMenuCreatorForMA 1.6.1の既知バグと同種の対策)

## 5. 他ツールとの相互作用(KB準拠の検証項目)

| 相手 | 期待動作 | 根拠 |
|---|---|---|
| MA | Generatingで生成→MAが統合。Merge Armature移動後のパス追従もMA/NDMFが処理 | KB 00 §1.2 / 02 |
| AAO | マージ済みアニメーションとして解析されるため、対象Blendshapeの凍結・対象プロパティの削除は自動回避される | KB 09(AAO: Animator Parser) |
| TTT | AtlasTexture等はマテリアルを置換するが、`material._Prop`バインディングはRenderer単位のため影響なし(プロパティ名が変わらない限り) | KB 04(TTT) |
| lilToon | アニメーション対象プロパティの定数化はlilToon 1.8+のアニメーション考慮最適化で回避される。ドキュメントにlilToon 1.8+推奨を明記 | KB 05(lilToon) |
| Poiyomi | §3の通り。Animated指定は原理的にユーザー操作が必要 | KB 06(Poiyomi) |
| VQT(Quest) | マテリアルプロパティのトゥイーンは、Quest変換(ToonLit等)後に対象プロパティが存在せず**無効化される**。Blendshape/Transformトラックは残る。ドキュメントに明記+ビルド時Info | KB 11(VQT) |
| MMDワールド | FXへマージされたレイヤーの扱いはMA 1.12+のMMD処理に従う(必要なら`MA MMD Layer Control`を併用) | KB 02 |

## 6. パッケージ構成

```
ndmf-tween/                     … UPMパッケージルート(git URL + ?path=ndmf-tween で導入可)
  package.json                  … dev.exor455.ndmf-tween / vpmDependencies: ndmf>=1.7, modular-avatar>=1.9
  README.md                     … 使い方・レシピ(ディゾルブ出現、衣装フェード切替)
  SPEC.md                       … 本書
  Runtime/
    dev.exor455.ndmf-tween.runtime.asmdef
    TweenToggle.cs              … コンポーネント(VRCSDKのIEditorOnly実装、ビルドに残らない)
    TweenTrack.cs               … トラック定義(Serializable)
    Easing.cs                   … イージング定義(enum+評価関数)
  Editor/
    dev.exor455.ndmf-tween.editor.asmdef   … (refs: runtime, nadena.dev.ndmf, nadena.dev.modular-avatar.core)
    TweenPlugin.cs              … NDMFプラグイン(Generating)
    TweenAnimatorBuilder.cs     … クリップ/コントローラ生成
    PoiyomiPropertyResolver.cs  … §3のリマップ/警告
    TweenToggleEditor.cs        … カスタムインスペクタ(ピッカー/バリデーション/MA Menu Item生成ボタン)
```

## 7. スコープ

**MVP(M1)**: TweenToggle(共通設定+全4トラックタイプ、Delay除く) / CrossFade+BakedClips両モード / イージング(enum+Custom) / Poiyomiビルド時リマップ / MA統合 / README
**M2**: トラックDelay、Curve Override、MA Menu Item生成ボタン、Poiyomi Animatedタグ検査
**M3(任意)**: NDMFプレビュー対応(Start/End状態のエディタプレビュー)、Int/Float多段トゥイーン(選択式衣装)、「トゥイーン完了時にMA Material Setter等を発火する完了パラメータ」

## 8. 非採用事項(理由付き)

- **FXコントローラの直接編集**: MA Merge Animator経由に一本化(WD/パス/MMD処理の再実装を避ける。制約「独自の破壊的パイプラインを作らない」)
- **パラメータ平滑化(OSCmooth式)による疑似トゥイーン**: 実装複雑・厳密な秒数指定が困難なため不採用。CrossFade/BakedClipsで要件を満たせる
- **GameObjectのactive切替トラック**: トゥイーン不能(bool)。MA Object Toggleの領分
- **マテリアル差し替えトラック**: MA Material Setter/Swapの領分(M3の完了パラメータで連携予定)

## 9. 検証計画

Unity実機(2022.3.22f1 + VRCSDK + NDMF + MA)での手動検証を前提とし、チェックリストをREADMEに同梱:
1. lilToonディゾルブ(`_DissolveParams`系Float)での出現演出
2. Blendshapeトゥイーン(WD ON/OFF両アバター)
3. トゥイーン中の反転操作(両モード)
4. Merge Armature配下のRendererを対象にした場合のパス追従
5. Poiyomiロック済みマテリアル(リネームあり/なし)
6. AAO T&O併用でプロパティ/Blendshapeが保持されること
7. MMDワールド互換(MA VRChat Settings既定)
