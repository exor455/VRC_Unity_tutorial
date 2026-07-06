# GoGo Loco(移動システム)

- リポジトリ: https://github.com/franada/gogoloco
- 配布: Booth(franada)/ GitHub
- ドキュメント: README記載のcraft.doリンク / VRChatグループ・Discordあり

## 概要

デフォルトのロコモーション(移動アニメーション)を置き換える**定番の移動システムプレハブ**。導入率が非常に高く、「改変対象のアバターに最初から入っている」ことが多いため、直接使わなくても挙動を知っておく必要がある。

READMEより(一次情報):
- 足の動きやジャンプアニメーションを個別にOFFにできる多数のトグル
- **ポーズ切替でフルボディトラッキング風の姿勢を再現**(座る・寝転がるを任意の場所で)
- ビルトインの**プレイスペース上下移動**(小柄なアバター向け)
- ゲームワールド向けの高度な移動を有効化する「game loco」トグル
- **同期メモリ使用量: 16/256 bit**(+飛行/ダッシュ等のExtraで+1 bit)。PC/Quest両対応

## 仕組み(改変との関わり)

- **Base / Action / Sitting 等のPlayable Layerを差し替える**([13](13-vrchat-avatars-basics.md)のレイヤー構造参照)。FX中心のMA系改変とはレイヤーが分かれるため共存しやすいが、**移動系を触る他ギミックとは競合**する(移動システムは1つに統一)
- Expression Menuに専用メニュー(アイコン)を追加する。ライセンス条件として**販売モデルに組み込む場合もメインアイコンは残す**ことがREADMEで明示されている

## 改変ツールとの相互作用

| 相手 | 内容 |
|---|---|
| MA | メニュー統合自体は共存可。GoGo Locoのメニュー/パラメータ(16bit)をMA Parametersの容量計算に含めて設計する |
| AAO | **AAO 1.8.0で「VRCStation由来BoxColliderのアニメーション削除がGoGo Loco等の飛行系アバターを壊す」問題が修正済み**([09 バージョン履歴](09-avatar-optimizer-changelog.md))。飛行・姿勢が壊れたらまずAAOのバージョン確認 |
| Quest/VQT | GoGo Loco自体はPC/Quest両対応(README)。VQT変換はFX系が主対象でBase/Action層の置き換えはそのまま通る |
| MMDワールド | MMD処理はFXレイヤーの話なのでGoGo Locoとは概ね独立(MA側の対応に従う) |

## 導入・更新時の注意

- バージョンにより内部構成(レイヤー名・パラメータ)が変わるため、**アバター付属の古いGoGo Locoを手動で新しいものへ差し替える際は、旧バージョンの残骸(レイヤー・パラメータ・メニュー)を除去してから**導入する
- 「Beyond」等のエディション/追加機能はBooth配布物のバリエーションとして提供される(+1bit等の同期コスト差)。導入物がどれかを最初に確認する
- 改変後に移動がおかしい場合の切り分け: Gesture Manager/Av3Emulator([12](12-analysis-upload-tools.md))でBase/Actionレイヤーの状態遷移を見る→AAOのAnimator最適化を一時無効化して比較

## 関連ページ

- Playable Layerの基礎: [13-vrchat-avatars-basics.md](13-vrchat-avatars-basics.md)
- パラメータ容量: [13](13-vrchat-avatars-basics.md)(256bit)+[02](02-modular-avatar.md)(MA Parameters)
