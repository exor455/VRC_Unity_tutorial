# VRCQuestTools (VQT)

- リポジトリ: https://github.com/kurotu/VRCQuestTools
- ドキュメント: https://kurotu.github.io/VRCQuestTools/
- パッケージ名: `com.github.kurotu.vrc-quest-tools`(VPMリポジトリ: kurotu.github.io/vpm-repos)
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

破壊的変更・修正版マッピングは [11-vrcquesttools-changelog.md](11-vrcquesttools-changelog.md) に分離。バージョン起因の不具合を切り分ける時だけ参照する。

## スクリプト実行ノート(自動化用・VQT 2.11.7 / Unity 2022.3 実測)

**手動GUI変換をスクリプトから呼ぶとハングする問題と回避策。** 通常の手動変換利用者には不要。スクリプト・エージェントからQuest変換を自動実行する場合のみ参照する。

### GUIエントリポイントはヘッドレス環境で使えない

`Tools/VRCQuestTools/Convert Avatar for Android` メニューの `OnClickConvertButton` は、対象アバターにUnity Constraintが含まれる場合に `EditorUtility.DisplayDialog` で確認ダイアログを表示する。ヘッドレス/バッチ実行環境(Unityをバッチモードで起動した場合や、AIBridgeのようなエディタ拡張から同期的に呼ぶ場合)ではこのダイアログがハングの原因になる。

### 内部APIを直呼びしてダイアログを回避する

スクリプトから変換する場合は、ダイアログを発生させない内部APIを直接呼ぶ(実測):

```csharp
// 1. 変換設定コンポーネントをアバターに追加
//    AvatarDescriptor は AddComponent 時に自動設定される
var settings = avatarGameObject.AddComponent<KRT.VRCQuestTools.Models.Unity.AvatarConverterSettings>();

// 2. ProgressCallback(内部クラス)の空インスタンスを生成し、各デリゲートを NoOp で埋める
//    ※ null を渡すと ConvertMaterialsForAndroid(AvatarConverter.cs 410行付近)で
//      NullReferenceException が発生する(progressCallback?.onTextureProgress(...) ではなく
//      直接呼びのため)
var converterType = typeof(KRT.VRCQuestTools.VRCQuestTools).Assembly
    .GetType("KRT.VRCQuestTools.AvatarConverter");
var callbackType = converterType.GetNestedType(
    "ProgressCallback", System.Reflection.BindingFlags.NonPublic);
var callback = System.Activator.CreateInstance(callbackType, nonPublic: true);
// 各デリゲートフィールド(onTextureProgress 等)を NoOp デリゲートで埋める
foreach (var field in callbackType.GetFields(
    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic |
    System.Reflection.BindingFlags.Public))
{
    if (typeof(System.Delegate).IsAssignableFrom(field.FieldType))
    {
        var noop = System.Delegate.CreateDelegate(
            field.FieldType,
            typeof(YourNoOpClass).GetMethod("NoOp", /* matching signature */));
        field.SetValue(callback, noop);
    }
}

// 3. 変換実行
var componentRemover = /* ComponentRemover インスタンス */;
var convertMethod = converterType.GetMethod("ConvertForQuest",
    System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
convertMethod.Invoke(null, new object[] {
    settings, componentRemover, false, outputPath, callback
});
```

**注意事項:**
- 型名・メソッドシグネチャ・フィールド名はすべて `internal` であり、VQTのメジャー更新で変更される可能性がある。使用前にソース(`Editor/AvatarConverter.cs`)を確認すること
- 変換後、`Selection.activeGameObject` がQuest複製のGameObjectを指す。複製の参照取得手段として利用できる(実測)
- `Remove Avatar Dynamics` オプションはデフォルト無効(`removeAvatarDynamics=false`)。スクリプトから有効化する場合は `AvatarConverterSettings` の該当フィールドを設定する(実測)
