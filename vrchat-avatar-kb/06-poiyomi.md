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

破壊的変更・修正版マッピングは [06-poiyomi-changelog.md](06-poiyomi-changelog.md) に分離。バージョン起因の不具合を切り分ける時だけ参照する。
