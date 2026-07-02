# NDMF Tween Generator

「開始値・終了値・秒数(+イージング)」を指定するだけで、AnimationClipとAnimator遷移をビルド時に自動生成するNDMFコンポーネント。VRCFury非依存。メニュー/パラメータ統合はModular Avatarに委譲する。

- 対象: VRChat Avatars 3.0 / Unity 2022.3
- 依存: [NDMF](https://github.com/bdunderscore/ndmf) >=1.7、[Modular Avatar](https://modular-avatar.nadena.dev) >=1.12、VRCSDK Avatars
- 仕様の詳細・設計判断: [SPEC.md](SPEC.md)

## インストール

VCC管理プロジェクトにNDMF/MA導入済みの前提で、以下のいずれか:

- UPM(git URL): `https://github.com/exor455/VRC_Unity_tutorial.git?path=ndmf-tween`
- または本フォルダ(`ndmf-tween/`)をプロジェクトの `Packages/` 配下へコピー

## 使い方(クイックスタート: ディゾルブ出現)

1. アバター配下の任意のGameObject(衣装のルート等)に **`NDMF Tween/NT Tween Toggle`** を追加
2. `Parameter Name` に任意のBoolパラメータ名(例: `Outfit/Dissolve`)を設定
3. `Add Track` → Type: **Material Float** → 衣装のRendererを指定 → `▾` ボタンでプロパティを選択(例: lilToonのディゾルブ進行度)→ Start `0` / End `1`
4. `Duration` に秒数(例: `2`)、`Easing` を選択(例: `InOutQuad`)
5. 同じ(または任意の)GameObjectに **MA Menu Item**(Type: Toggle)を追加し、Parameterに手順2と同じ名前を設定
6. アップロード or Play(Apply on Play)。メニューのトグルON→2秒かけてStart→End、OFF→逆再生

パラメータのExpression Parameters登録(Bool/synced/saved/default)は本コンポーネントがビルド時にMA Parametersとして自動注入するため、**手動でのパラメータ登録は不要**(MA Menu Item側はパラメータ名の一致のみ必要)。

## コンポーネント設定

| 項目 | 説明 |
|---|---|
| Parameter Name | トリガーのBoolパラメータ名。空なら `Tween/<オブジェクト名>` |
| Default On / Saved / Synced | Expression Parametersの初期値/保存/同期設定(MA Parametersに反映) |
| Duration (s) / Reverse Duration (s) | ON方向/OFF方向の秒数(Reverse負値=Durationと同じ) |
| Easing / Custom Curve | Linear / In・Out・InOutQuad / InOutCubic / InOutSine / Custom(0-1正規化カーブ) |
| Transition Mode | Auto(推奨)/ CrossFade / BakedClips(下記) |
| Cancel Blend (s) | BakedClips時、トゥイーン中に反転した際のクロスフェード秒数 |
| Poiyomi Auto Remap | プロパティ不在時、リネーム済み候補が一意なら自動リマップ |

### トラック

| Type | 対象 | 値 |
|---|---|---|
| Blendshape | SkinnedMeshRenderer + Blendshape名 | float (0-100) |
| Material Float | Renderer + プロパティ名(全マテリアルスロットに適用)。`_Prop.x` のようにベクトル成分も手入力可 | float |
| Material Color | Renderer + Colorプロパティ名 | Color (HDR可、RGBA 4カーブ) |
| Transform | Transform + localPosition / localEulerAngles / localScale | Vector3 |

各トラックに `Delay (s)`(開始遅延。BakedClips時のみ有効)と `Curve Override` を設定可能。

### Transition Mode の挙動

- **CrossFade**(Auto時: Easing=Linearかつ Delay/カーブ上書きなし): Start値/End値を持つ2つの1フレームステート間を、Animator遷移のduration(秒)で補間。**トゥイーン途中の反転が完全に滑らか**。線形補間のみ
- **BakedClips**(Auto時: イージング等使用時): イージングを焼き込んだ遷移クリップを生成(Off→TweenIn→On→TweenOut)。途中反転は `Cancel Blend` 秒のクロスフェードで近似

## 生成物(ビルド時の動作)

NDMFの**Generating**フェーズで、各`NT Tween Toggle`につき:

1. AnimatorController(1レイヤー)+AnimationClip群を生成
2. 子GameObjectに `MA Merge Animator`(FX / Absolute / Match Avatar Write Defaults ON)と `MA Parameters`(Boolエントリ)を追加
3. `NT Tween Toggle` コンポーネント自体を削除

以降のFX統合・パス書き換え(Merge Armature移動追従)・WD調整・MMD対応はModular Avatarが処理する。FXを直接書き換えないため、既存NDMF/MAエコシステムと干渉しない。

## Poiyomi(ロック機構)との併用

- ロック時にアニメーションさせるプロパティは、事前にPoiyomiインスペクタで**Animated指定**が必要(仕様上ユーザー操作が必須)
- 「Rename Animated」でプロパティ名がリネームされた場合: プロパティピッカー(`▾`)は現在のマテリアルの実プロパティ名を列挙するため、**ロック後のマテリアルからピッカーで選び直すのが確実**。旧名のままでも、リネーム先が一意に特定できる場合はビルド時に自動リマップされる(Poiyomi Auto Remap)
- 未ロックPoiyomiマテリアルでAnimatedタグが確認できない場合、Inspector/ビルドログに警告を出す(ベストエフォート)

## 他ツールとの関係

- **lilToon**: ビルド時最適化がアニメーション対象プロパティを考慮するのはlilToon 1.8.0以降。それ未満では最適化で定数化されトゥイーンが無効になりうる
- **AAO**: マージ済みアニメーションとして解析されるため、対象Blendshapeの凍結や対象プロパティの削除は自動回避される
- **VRCQuestTools(Quest変換)**: マテリアル変換後のシェーダーに同名プロパティが無い場合、Material系トラックは無効になる(Blendshape/Transformトラックは残る)
- **MMDワールド**: FXに統合されたレイヤーの扱いはMA 1.12+のMMD互換処理に従う

## 実機検証チェックリスト(未実施)

本パッケージは実機Unity(2022.3.22f1 + VRCSDK + NDMF + MA)での検証が必要:

1. コンパイル確認(下記「要確認ポイント」)
2. lilToonディゾルブ系Floatプロパティでの出現演出(CrossFade/BakedClips両方)
3. Blendshapeトゥイーン(WD ON / OFF両方のアバター)
4. トゥイーン中の反転操作
5. Merge Armature配下のRendererを対象にした場合のパス追従
6. Poiyomiロック済みマテリアル(リネームあり/なし)での動作とAuto Remap
7. AAO T&O併用時に対象プロパティ/Blendshapeが保持されること
8. MMDワールドでの動作(MA VRChat Settings既定)

### 要確認ポイント(コンパイル未検証の外部API)

- `BuildContext.AssetSaver`(NDMF 1.6+のIAssetSaver)のプロパティ名
- `ParameterConfig.hasExplicitDefaultValue` フィールド名(MA 1.12+想定)
- `localEulerAnglesRaw` バインディング(Transform回転トラック)

いずれも問題があれば該当行のみの修正で済む設計にしてある。

## ライセンス

未定(リポジトリ方針に従う)。
