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
- **MA Bone Proxy**: オブジェクトを指定ボーン配下へ**非破壊に**移動(アクセサリ装着)。Inspectorで **Target**(付けたいボーンをドラッグ&ドロップ)と **Attachment**(位置+回転一致/現在の位置か回転だけ保つ 等)を設定するだけ。ターゲットに他のBone Proxy/Merge Armature配下も指定可(1.16+)。**オブジェクト自体はシーン上のどこに置いてもよく(直接ボーンの子にする必要はない)**、実際の付け替えはビルド時に行われる。**Hierarchyで直接ボーンの子にドラッグして付け替えるのは非破壊原則違反**(KB [00 §0](00-cross-tool.md)参照)
  - **Match Scale**(1.17+): オブジェクト自身のスケールを、装着先ボーンのスケールに**追従させ続ける**オプション。ONにすると、そのオブジェクトに手動で設定したlocalScaleはボーンのスケールで**エディタ上でも上書きされ続ける**(値を変えてもすぐ戻る)。
    - ONにすべき場合: 体格が異なるアバターへ使い回す衣装・小物など、装着先アバターのスケール変化に追従させたい場合
    - OFFにすべき場合: プリミティブ等、オブジェクト側で意図したスケールを直接指定・固定したい場合
    - **上書きは設定を変えた直後ではなく、MAのエディタ処理が走った後に起きる**ため、変更直後の見た目確認では気付きにくい
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
- **`ModularAvatarObjectToggle` のシリアライズフィールド名(エディタスクリプト操作時)**: SerializedObject 経由でトグル対象リストを読む際、`FindProperty("Objects")` / `FindProperty("objects")` / `FindProperty("m_Objects")` はいずれもヒットしない(実測)。正しい名前は **`m_objects`(小文字o)**。各要素のサブプロパティ構造は `m_objects[i].Object.referencePath`(string: アバタールートからの相対パス)と `m_objects[i].Active`(bool)。同列に `m_inverted`(bool)もある。エントリ追加は `arraySize` をインクリメントし `referencePath` と `Active` を設定後、`ApplyModifiedProperties` を呼ぶ。環境: Modular Avatar (nadena.dev.modular_avatar.core) / Unity 2022.3.22f1 / VRCSDK3プロジェクト、2026-07-05実測確認。
  - **`referencePath` と `targetObject` は必ず両方設定する(実測)**: MAは `m_objects[i].Object.targetObject`(UnityネイティブのGameObject参照、objectReferenceValue)を正として管理し、`referencePath` はドメインリロード時に `targetObject` のGameObject名から再生成される。そのため `referencePath` のみ SerializedObject で書き換えると直後の読み返しは正しいが**ドメインリロード後に静かに巻き戻る**。`InsertArrayElementAtIndex` で前要素を複製した場合は `targetObject` が複製元のままクローンされるため、`referencePath` だけ上書きしてもリロード後は複製元と同じ対象に戻る。スクリプトから編集する際は `referencePath`(string)と `targetObject`(objectReferenceValue)の**両方**を必ず設定すること。検証は同一ジョブ内の読み返しでは不十分で、**ドメインリロードを跨いだ再ダンプ**で永続性を確認する。(実測: Unity 2022.3.22f1 / MA 1.17.1 / NDMF 1.14.0)
- **`ModularAvatarMaterialSetter` のシリアライズフィールド名(エディタスクリプト操作時)**: SerializedObject 経由でマテリアル差し替えリストを操作する際の構造は以下の通り(実測: Unity 2022.3.22f1 / MA 1.17.1 / NDMF 1.14.0)。
  - フィールド名は **`m_objects`**。型は `List<MaterialSwitchObject>`
  - 各要素のサブプロパティ:
    - `m_objects[i].Object`(型: `AvatarObjectReference`, SerializedPropertyType.Generic) — `FindPropertyRelative("Object")` で取得し、さらに `FindPropertyRelative("referencePath")`(string: アバタールートからの相対Transform名パス)でアクセスする。`objectReferenceValue` では取れない点に注意
    - `m_objects[i].Material`(Material参照: `objectReferenceValue` で設定)
    - `m_objects[i].MaterialIndex`(int: 対象マテリアルスロット番号)
  - `Object.targetObject`(GameObjectへのObjectReference)は **bake時には不要**。`referencePath` だけで動作する(MA Object Toggle の `targetObject` 必須ルールとは異なる)
  - エントリ追加は `arraySize` をインクリメントし `InsertArrayElementAtIndex` 後、`referencePath`・`Material`・`MaterialIndex` を設定して `ApplyModifiedProperties` を呼ぶ

### MA Blendshape Sync スクリプト設定ノート

エディタ C# から `ModularAvatarBlendshapeSync` を設定する際の実測知見(Unity 2022.3.22f1 / MA 1.17.1 / NDMF 1.14.0、まめひなたで検証)。

#### Bindings のフィールド構造

`BlendshapeBinding` の各フィールドは以下の通り(いずれも public):

| フィールド | 型 | 内容 |
|---|---|---|
| `ReferenceMesh` | `AvatarObjectReference` | 同期元メッシュを持つ GameObject |
| `Blendshape` | string | 同期元(素体)の BlendShape 名 |
| `LocalBlendshape` | string | 同期先(衣装)の BlendShape 名 |

コンポーネント側の `Bindings`(`List<BlendshapeBinding>`)も public で直接追加可能。

#### AvatarObjectReference.targetObject は private — Reflection で設定する(実測)

`AvatarObjectReference.referencePath`(string)は public で直接代入できる。しかし `targetObject`(GameObject)は **private フィールド**であり、直接アクセスは CS1061 コンパイルエラーになる。

解決: Reflection を使う。

```csharp
var fi = typeof(AvatarObjectReference).GetField("targetObject",
    BindingFlags.NonPublic | BindingFlags.Instance);
fi.SetValue(binding.ReferenceMesh, bodyGameObject);
```

`referencePath` と `targetObject` の**両方**を設定すること。`referencePath` のみだとドメインリロード後に `targetObject` 側から再生成されて巻き戻る事故がある(MA Object Toggle の `m_objects` における既知挙動と同根。詳細は「改変時の注意点 §m_objects」参照)。

#### 同期反映タイミング — 同一フレーム内では同期先 weight は更新されない(実測)

`ModularAvatarBlendshapeSync` を AddComponent して Bindings を設定し、同期元(素体)の BlendShape weight を `SetBlendShapeWeight(100)` で変更しても、**同一フレーム(同一スクリプト実行)内では同期先(衣装)の weight は 0 のまま**になる。

- 実測: 次のジョブ実行時(ドメインリロード後)には同期先が 100 に追従していた。NDMF Manual Bake は不要。
- 解釈: 同期はMAのエディタ側処理で行われ、スクリプトから値を設定した同一フレーム内では発動しない。「設定直後に読み戻して 0 だから壊れている」と誤判定しないこと。
- GUI操作(Inspector のスライダー)では即時追従して見えるため、この罠は**スクリプト自動化時のみ顕在化する**。

#### 衣装に対応 BlendShape が無い場合の対処

衣装側に素体と対応する BlendShape が無い場合、`Mesh.AddBlendShapeFrame` でメッシュコピーに同名 BS を生成してから同期させる手が使える。元 FBX メッシュは直接変更せず `Object.Instantiate` したコピーに追加して `sharedMesh` を差し替える。既存 BS は読み出して再追加で保全する。

#### 素体ごとの BS 命名差(参照)

素体によっては「同名BSが同期元/先にある」前提が成立しない(独自命名・セパレータ専用BS等)。名前が違う場合も `LocalBlendshape` に同期先の実BS名を明示すれば同期は成立する。素体設計の分類と改変前の判定手順は [20 素体差リファレンス](20-avatar-differences.md)。

### メニュー付きトグルの組み立て骨格

`MA Menu Installer` / `MA Menu Item` / `MA Menu Group` / `MA Object Toggle` の存在は「対応する改変パターン」に列挙されているが、それらをどう配置するとメニュー付きトグルが成立するかの骨格は別途示す必要がある。以下2パターンを示す(実測: Unity 2022.3.22f1 / MA 1.17.1 / NDMF 1.14.0、NDMF Manual Bakeで動作確認)。

**最小構成(単一GO平置き)**: 1つの空GOに `MA Menu Installer` + `MA Menu Item`(type=Toggle) + `MA Object Toggle` を載せる。Menu Installer の `installTargetMenu = null` でアバタールートの Expressions メニューへ直接挿入される。同一GO上の Menu Item(Toggle)と Object Toggle が**自動結線・自動パラメータ割当**される。単発トグルはこれで足りる。Bakeで「ルートメニュー項目 + 自動 Bool パラメータ + FX の activeSelf 切替結線」が生成されることを確認。

**定番構成(フォルダ=親子2階層)**: 親GOに `MA Menu Installer` + `MA Menu Group`、子GO群に各 `MA Menu Item`(Toggle) + リアクティブコンポーネント(オブジェクトON/OFFなら `MA Object Toggle`、マテリアル切替なら `MA Material Setter` 等、用途で差し替え)を載せる。`MA Menu Group` が「子オブジェクト群=メニュー項目」を成立させる箱の役割を担い、メニュー上でフォルダにまとまる。日本圏で最もよく使われる形。

**Menu Item の主なシリアライズフィールド(スクリプト操作時の参考)**:

| フィールド | 型 | 備考 |
|---|---|---|
| `Control.name` | string | メニュー表示名候補(後述の落とし穴あり) |
| `Control.type` | enum | Toggle = index 1 |
| `Control.parameter.name` | string | 空なら自動割当(GO名から生成) |
| `Control.value` | float | Toggle は通常 1.0 |
| `isSynced` / `isSaved` / `isDefault` | bool | 自動割当パラメータの属性 |
| `automaticValue` | bool | ON のとき value を自動決定 |

`installTargetMenu`(Menu Installer フィールド): null = アバタールートメニューへ挿入。

**落とし穴 — 自動パラメータ割当の名前は `Control.name` ではなく GO 名から生成される(実測)**:
`Control.parameter.name` を空にして自動割当にすると、生成される同期パラメータ名は `__MA/AutoParam/<GameObject名>$<hash>`(Bool)になり、**メニュー項目の表示名も GO 名が採用される**(`Control.name` ではない)。例: GO 名 = "ExampleToggle"・`Control.name` = "Example" のとき、パラメータ名・表示名ともに "ExampleToggle" になる。狙った表示名・パラメータ名にしたい場合は、GO 名をその名前に合わせるか `Control.parameter.name` を明示設定する。

**定番構成(親子2階層)でも MA Material Setter を含む全リアクティブは動作する(実測: Unity 2022.3.22f1 / MA 1.17.1 / NDMF 1.14.0)**: 親GO に `MA Menu Installer` + `MA Menu Group`、子GO に `MA Menu Item`(Toggle) + `MA Material Setter` を置く2階層構成でも、単一GO平置き構成と**同一の FX 結線・自動パラメータ・メニュー項目が bake 生成される**。`MA Menu Group` はメニュー構造の「箱」にすぎず、リアクティブ(Material Setter / Object Toggle 等)は同一GO上の Menu Item と自動結線されるため、**メニューの入れ子とリアクティブ種別は独立。どの構成でもリアクティブは動作する**。
過去に「2階層は動かない」と誤認した原因は Menu Group の制約ではなく次の2点: (a) `AvatarProcessor.ProcessAvatarUI()` に `Instantiate` した clone を渡して `(Clone)(Clone)` を生成し間違ったオブジェクトを検査した計測ミス、(b) FX に motion が null の State が含まれていた場合の bake 例外([01 NDMF §スクリプト実行ノート](01-ndmf.md)参照)。

### メニュー編集のGUIショートカット(Create Toggle / Extract Menu)

メニュー項目の生成・整頓を最速で行う2つのUnityエディタメニュー操作。対象: MA 1.17.1 / Unity 2022.3.22f1 / VRCSDK3 Avatars。

**出典**: 公式doc [Simple Object Toggle tutorial](https://modular-avatar.nadena.dev/docs/tutorials/object_toggle) / [Edit menus tutorial](https://modular-avatar.nadena.dev/docs/tutorials/menu)。以下の非自明な挙動はソース `Editor/Inspector/Menu/ToggleCreatorShortcut.cs` / `Editor/Menu/MenuExtractor.cs` で確認。

#### Create Toggle(右クリック最速生成)

Hierarchyで対象GOを右クリック → `GameObject/Modular Avatar/Create Toggle for Selection`。

- 生成物: 選択オブジェクトを対象にした Object Toggle + Menu Item を含む空GOがアバター配下に生成される。単体選択時は Menu Installer 付き。複数選択時はサブメニューを自動生成して子としてまとめ、親側に Installer を付け子トグルには付けない(MA #1437以降)
- `Create Toggle`(選択対象なし版)は空の "New Toggle" GO(MA Menu Item + MA Menu Installer + 空の MA Object Toggle)を生成する

**★非自明な罠 — 生成時の activeSelf で ON/OFF の意味が決まる(ソース: `ToggleCreatorShortcut.cs`)**:
`Create Toggle for Selection` は**対象オブジェクトの現在の `activeSelf` を読み、Object Toggle の Active をその反対値に設定する**(`Active = !selected.activeSelf`)。生成GOの名前も `activeSelf=true` なら "{name} OFF"、`activeSelf=false` なら "{name} ON" になる。
→ **トグルを作る前に対象を「意図したデフォルト表示状態」にしておくこと**。作成時の activeSelf が狙いと逆だと、生成されるトグルのON/OFFの意味が反転し、期待と逆の挙動になる。

**位置づけ**: 単発トグルの最速手段。ただし単体ごとに Installer 付き GO を量産するため構造が散らかりやすい。精密・大量・スクリプト運用では「### メニュー付きトグルの組み立て骨格」に示す定番構成を使う。

#### Extract Menu(既存 Expressions メニューをオブジェクト化)

アバタールートを選択 → `GameObject/Modular Avatar/Extract Menu`。

- 生成物: "Avatar Menu" GO が追加され、`MA Menu Installer` + `MA Menu Group` を持つ。元メニューのトップ階層の各コントロールが子GOの `MA Menu Item` になる
- サブメニューは各 Menu Item / Menu Installer インスペクタの「オブジェクトに展開(extract to objects)」ボタンで1階層ずつ再帰的に展開できる
- 展開後はヒエラルキー上のドラッグ&ドロップでメニュー項目を移動・再編できる(既存 EXmenu の整理・組み替えに有利)

**★「破壊的」の正体 — expressionsMenu がプレースホルダーに差し替わる(ソース: `MenuExtractor.cs`)**:
展開時に**アバターの `VRCAvatarDescriptor.expressionsMenu` を空のプレースホルダーアセット(元アセット名 + " placeholder.asset")に差し替える**。元のメニューアセット自体は削除されないが、アバターからの参照が外れ、以後メニューはMAオブジェクト側が単一の正になる。VRCSDKがパラメータ定義時にメニューアセットの存在を要求するためダミーを噛ませる仕様。元アセットが Packages 配下の場合プレースホルダーは Assets ルートに作られる。**元に戻すには `expressionsMenu` を元アセットに指し直す。**

**用途**: 既存アバターのメニュー構成をMAの非破壊オブジェクト編集へ載せ替えて再編するときに使う破壊的操作。

### 衣装トグルのOFFリスト構築(参照)

OFFリスト(既定衣装の取りこぼし)をスキンウェイトから機械判定する「部位シグネチャ」手順は [20 素体差リファレンス](20-avatar-differences.md) 参照。

### NDMF Manual Bake によるトグル結線の検証

メニュー→パラメータ→リアクティブの結線が実際に機能するかは、NDMF Manual Bake の生成物を読んで確認する(NDMFプレビューはスクリプトから発火できないため)。スクリプトからの呼び方・Cloneの取り扱い・例外の切り分けは [01 NDMF §スクリプト実行ノート](01-ndmf.md)。生成された `<アバター名>(Clone)` で以下を読む:

- `VRCExpressionParameters`: 生成パラメータ名・型・synced/saved フラグ
- `VRCExpressionsMenu`: メニュー項目とそのパラメータ名が VRCExpressionParameters と一致するか
- FX `AnimatorController`: activeSelf 切替ステートが存在するか

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
- **「スケールを変えても勝手に元に戻る」**(ユーザー語彙): そのオブジェクトにBone Proxyが付いていて**Match Scale ON**になっていないか確認。ONだとlocalScaleがボーンのスケールで上書きされ続ける。固定したいスケールがあるならMatch ScaleをOFFにしてから設定し直す
- **スクリプトで書き換えたトグル対象がドメインリロード後に元に戻る**: `m_objects[i].Object.referencePath` を SerializedObject 経由で書き換えてもドメインリロード後に `targetObject`(GameObjectへのUnityネイティブ参照)からパスが再生成されて巻き戻る。`InsertArrayElementAtIndex` による複製は `targetObject` が複製元のままクローンされるため特に注意。`referencePath` と `targetObject`(objectReferenceValue)の**両方**を設定すること。同一ジョブ内の読み返しでは検証不十分で、ドメインリロード後の再ダンプで永続性を確認する。詳細は「改変時の注意点 §m_objects」参照。(実測: Unity 2022.3.22f1 / MA 1.17.1 / NDMF 1.14.0)
- **NDMF Manual Bake またはビルドが途中で例外停止する(`VirtualClip.Commit` 等)**: 詳細手順は [01 NDMF §スクリプト実行ノート](01-ndmf.md)。
- **スクリプトで Blendshape Sync を設定したが同期先 weight が 0 のまま**: 同一フレーム(同一ジョブ)内では同期が発動しない仕様。次のジョブ実行(ドメインリロード後)で追従しているかを確認する。NDMF Manual Bake は不要。詳細は「MA Blendshape Sync スクリプト設定ノート §同期反映タイミング」参照。(実測: Unity 2022.3.22f1 / MA 1.17.1 / NDMF 1.14.0)
- **スクリプトで `AvatarObjectReference.targetObject` に直接代入しようとした**: CS1061 コンパイルエラーになる。`targetObject` は private フィールドのため Reflection で設定する。`referencePath`(public string)だけでは足りず両方の設定が必要。詳細は「MA Blendshape Sync スクリプト設定ノート §AvatarObjectReference.targetObject は private」参照。
- **衣装を別素体のアバターに着せたが表情・体型が反応しない(BS同期無反応)**: 素体間でBS命名が互換でない可能性。両者のBS一覧を突き合わせ、名前が違う場合は同期先BS名を明示指定する(MA Blendshape Sync の `LocalBlendshape` フィールドで同期先の実際の BS 名を明示指定)。詳細は [20 素体差リファレンス](20-avatar-differences.md)。

## 関連ツール

- [NDMF](01-ndmf.md): 実行基盤(同一作者、同時更新推奨)
- [TexTransTool](04-textranstool.md): MAの後に実行。MA Material Setter/Swapで差し替えたマテリアルへもTTTが効く(TTT 0.10+)
- [AAO](09-avatar-optimizer.md): MAの生成物(DelayDisableレイヤー等)を認識して最適化
- [VRCQuestTools](11-vrcquesttools.md): MA処理後にQuest変換。Setup Avatar for MobileはMA Sync Parameter Sequence等を自動追加

## バージョン履歴

破壊的変更・修正版マッピングは [02-modular-avatar-changelog.md](02-modular-avatar-changelog.md) に分離。バージョン起因の不具合を切り分ける時だけ参照する。
