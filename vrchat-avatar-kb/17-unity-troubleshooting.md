# Unity定番トラブル集(ツール以前の問題)

改変ツールの問題に見えて、実際はUnity/プロジェクト管理レベルの問題であるものの切り分け集。NDMF系のトラブルは[00 §3](00-cross-tool.md)が先。

## マテリアルがピンク(マゼンタ)

シェーダーが解決できていないサイン。原因の頻度順:

1. **シェーダー未導入**: lilToon/Poiyomi等がプロジェクトに無い。アバター/衣装の説明書記載のシェーダーを導入
2. **シェーダーのバージョン/エディション違い**: Poiyomi Pro向けマテリアルをToon環境で開いた、大型アップデートでシェーダーパスが変わった等([06](06-poiyomi.md)のバージョン共存の注意)
3. **ロック済みシェーダーの生成物欠落**: Poiyomiのロック済みマテリアルは生成シェーダーが必要。アンロック→再ロックで再生成([06](06-poiyomi.md))
4. render pipeline不一致: URP用アセットをBuilt-in(VRChat)に入れた等。VRChatアバターはBuilt-in RP固定
5. シェーダーキャッシュ破損: `Library/ShaderCache`削除→再起動

## Missing Script(スクリプトが見つからない)

- 原因: コンポーネントの元ツールがプロジェクトに無い(削除した/入れ忘れた)。VRCFury等の独自シリアライズ形式は特に起きやすい([14](14-vrcfury.md))
- 対処: 元ツールを導入して開き直すのが第一。不要と確定しているなら削除(NDMFはビルド開始時にmissing script componentを除去する([01](01-ndmf.md))が、エディタ作業では邪魔になる)
- AAOは未知の`IEditorOnly`コンポーネントに警告を出す。誤検知なら無視リストへ([09](09-avatar-optimizer.md)、1.9+)

## unitypackage と VPM の二重導入・移行事故

- 旧unitypackage版(`Assets/`配下)とVPM版(`Packages/`配下)の**二重導入はGUID/クラス重複の温床**。lilToonの`legacyFolders`(Assets\lilToon)、Poiyomiの`Assets\_PoiyomiShaders`、VQT 2.0のパス移動など、主要ツールはマニフェストで移行情報を持つが、**手動で入れた古いコピーは自前で削除**が必要
- 症状: 「同名クラスが2つ」コンパイルエラー、インスペクタ表示の異常、ビルド時の謎の旧挙動
- 対処: `Assets/`配下の旧ツールフォルダを削除→VCC/ALCOMでResolve
- NDMF 1.2.5は「VRChatテンプレート派生パッケージとのGUID衝突」を修正済み([01](01-ndmf.md))。それでもGUID衝突警告が出る場合は重複インポートを疑う

## プロジェクトが開けない・挙動が全体的におかしい

- **Libraryフォルダ削除→再インポート**が万能薬(時間はかかる)。プロジェクト破損疑いの第一手
- パッケージ解決エラー: VCC/ALCOMでResolve。`Packages/vpm-manifest.json`の手編集ミスも疑う
- **非ASCII(日本語)を含むプロジェクトパス**: VRCSDKの既知バグの温床(NDMF 1.5.0が一部を回避)。プロジェクトは英数字パスに置くのが安全
- Unityバージョン違いで開いた: VRChat公式指定(現行2022.3系)以外で開くとLibraryが壊れることがある。バージョンを戻し、Library削除

## プレハブ・シーンまわり

- **プレハブオーバーライドの意図しないRevert**: シーン初回ロードでオーバーライドが戻る問題はAAO 1.8.3で修正されたケースあり([09](09-avatar-optimizer.md))。類似症状はツールのバージョン確認
- Prefabステージでビルド系操作は不可(VQT 2.4.1がエラーメッセージ化)。**シーンに出してから**作業する
- 「Auto Referenced」問題: 一部ツールはasmdef設定変更でコンパイル時間を最適化している(lilToon 2.0等)。自作スクリプトから参照できない場合はasmdef参照を明示

## アップロードだけ失敗する

チェック順:
1. **サイズ制限**([13](13-vrchat-avatars-basics.md)): PC 200MB/500MB、Android 10MB/40MB。Build & Testでは検査されない点に注意
2. **Read/Write無効メッシュ**→強制Very Poor([13](13-vrchat-avatars-basics.md))
3. Pipeline Manager(Blueprint ID)の不整合: 別アバターのIDが残っている→Detach
4. ビルドコールバック失敗: 「〜PreprocessAvatarCallback reported a failure」はNDMF Console/Consoleの一次エラーを見る([00 §3](00-cross-tool.md))
5. 回線・CDN起因の一時失敗: 時間を置いて再試行

## テクスチャ・モデルインポートの定番

- テクスチャがぼやける: Max Size設定(既定2048)を確認。ただし上げる前にVRAMランク([13](13-vrchat-avatars-basics.md))とのトレードオフを検討
- 法線が壊れる/影が汚い: モデルインポートのNormals設定(Import/Calculate)とBlend Shape Normals(Legacy推奨のケース多し)
- FBX更新で改変が消えた: メッシュ資産の上書きはコンポーネント参照を壊しうる。載せ替えは[Pumkin's](12-analysis-upload-tools.md)のコピー機能か、最初からMAプレハブ設計で

## 関連ページ

- ツール系トラブルフロー: [00-cross-tool.md §3](00-cross-tool.md)
- VRChat側仕様(サイズ・ランク): [13-vrchat-avatars-basics.md](13-vrchat-avatars-basics.md)
