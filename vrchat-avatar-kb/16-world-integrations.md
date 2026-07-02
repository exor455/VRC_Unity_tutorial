# ワールド連携機能(AudioLink / LTCGI / VRC Light Volumes)

ワールド側が提供する仕組みに、**アバター側のシェーダーが反応する**タイプの機能群。改変では「シェーダーの対応機能をONにする+対応ワールドで確認する」が基本で、アバター単体では動作確認できない点が共通のハマりどころ。

> 情報源: 各リポジトリREADME(2026年前半時点)+lilToon/PoiyomiのCHANGELOG(KB 05/06)。

## 共通の基礎知識

- これらは**PC向け機能**が中心(Questのアバターシェーダーは対応不可。ワールド側のQuest対応は各プロジェクト参照)
- 「自分のPCでは光るのにフレンドには見えない」系は、相手のセーフティ(シェーダーOFF→フォールバック描画)が原因のことが多い([13](13-vrchat-avatars-basics.md)のセーフティ節)
- 非対応ワールドでは単に反応しない(AudioLink等は非対応時の見た目を設定できる機能がシェーダー側にある)

---

## AudioLink

- リポジトリ: https://github.com/llealloo/audiolink(VPM配布)
- 現行: 2.x(2.0.0は2024-09)

**ワールド内の音声を解析し、周波数帯ごとのリアクティブデータをシェーダー/Udonに公開する**システム。データはグローバルな`_AudioTexture`として全アバターのシェーダーから参照できる。

- アバター側: lilToon / PoiyomiのAudioLink機能(Emission連動等)を有効化するだけ。対応ワールドで自動的に反応する
- エディタでのテスト: AudioLinkパッケージをアバタープロジェクトに入れると、エディタ内でテスト用のAudioLink環境を再生できる(アップロード物には含めない=ワールド側機能)
- 改変時の注意: **AAOのテクスチャ最適化とlilToonのAudioLinkマスクの組み合わせ問題はAAO 1.9.4で修正済み**([09](09-avatar-optimizer.md))。AudioLink演出が最適化後に壊れたらAAOのバージョンを確認
- 2.0.0の変更(README): コントローラ同期方法の調整、デュアルモノ音源対応、BlendShape駆動ユーティリティ(AudioReactiveBlendshapes)追加

## LTCGI

- リポジトリ: https://github.com/PiMaker/ltcgi(VPM: vpm.pimaker.at)
- ライセンス: 無料だが**Attribution(クレジット表記)必須**

**Linearly Transformed Cosineアルゴリズムによるリアルタイムエリアライト**(スクリーンの光が部屋とアバターを照らす等)。ワールド側に仕込む機能で、アバターは対応シェーダーであれば照らされる。

- アバター側: **lilToonは1.8.0でLTCGI対応**([05](05-liltoon.md))。シェーダー設定でLTCGIを有効化したマテリアルのみ反応する
- lilToonの既知修正: LTCGIの減衰・特定条件で動かない問題は1.8.4、AudioLinkとLTCGI併用時のワールドプロジェクトでのシェーダーエラーは2.1.5で修正
- 古いGPU(GTX9/10系)+LTCGI環境の問題はシェーダー側で修正歴あり(lilToon 2.1.2)

## VRC Light Volumes(VRCLV)

- リポジトリ: https://github.com/REDSIM/VRCLightVolumes
- 作者: REDSIM

**ボクセルベースのLight Probes代替**。ワールドのベイク照明を高品質・軽量にアバターへ適用する次世代ライティング。2025年以降、対応ワールド・対応シェーダーが急速に普及した。

- アバター側: **lilToonは1.10.0でVRCLV対応、2.0.0でVRCLV 2.0(方向対応)を同梱**、2.1.0でピクセル単位計算+専用リムライト設定([05](05-liltoon.md))。Poiyomiも9.x系で対応(バージョン別詳細はリポジトリの[Compatible Shaders](https://github.com/REDSIM/VRCLightVolumes/blob/main/Documentation/CompatibleShaders.md)を参照)
- lilToon 2.2.0の`_UdonForceSceneLighting`のように、ワールド側からアバターの明るさ調整を制御する連携も進んでいる
- 改変時の注意: VRCLV対応前のシェーダーバージョンだと対応ワールドで見た目が沈む/浮くことがある。**「特定ワールドでだけ暗い・明るい」系の相談ではシェーダーのVRCLV対応バージョンをまず確認**
- Light Limit Changer([07](07-expression-gimmick-tools.md))との関係: LLCは明るさの上下限操作、VRCLVは光源情報の供給で役割が異なる。併用可

## 関連ページ

- シェーダー側の対応履歴: [05-liltoon.md](05-liltoon.md) / [06-poiyomi.md](06-poiyomi.md)
- 明るさ調整メニュー: [07-expression-gimmick-tools.md](07-expression-gimmick-tools.md)(Light Limit Changer)
