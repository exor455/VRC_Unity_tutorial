# Modular Avatar バージョン履歴

> 元ファイル: [02-modular-avatar.md](02-modular-avatar.md)。バージョン起因の不具合切り分け・破壊的変更の確認時のみ参照。

## バージョン履歴

(CHANGELOG.mdは1.12.0以降。それ以前はGitHub Releases参照。日付はリリース日)

### 1.18(Unreleased→2026年後半見込み)
- 追加: Blendshape Syncのカーブリマップ、Vertex Filter By UV Tile、By Maskの代替UV、Sync Parameter Sequenceの誤設定警告、**Int自動値の0-255超過をビルドエラー化**
- 修正: Unity 6000.0互換、MMDワールドでFX第1レイヤー無効化、Merge Armatureのメモリデフラグ後の追跡喪失

### 1.17.x (2026-05)
- 1.17.0: **MA Floor Adjuster追加**、VRCSDKのメニューエディタハングをパッチ、Bone ProxyのMatch scale、`VRCRaycast`対応(VRCSDK 3.10.3+)
- 1.17.1: **Floor AdjusterをTTT等の後に実行するよう順序変更**(late-transform-stagesプラグイン新設。互換性重要)

### 1.16.x (2026-02)
- 1.16.0: Bone Proxyのターゲット拡張、**Sync Parameter SequenceがUnity Library保存+プライマリ→セカンダリ同期に変更**(パラメータアセット不要化)、TextMeshProポストプロセッサ抑止(ビルド高速化)、MMDワールド関連修正多数
- 1.16.1: **Merge Animator「Match Avatar Write Defaults」既定ON化**。Bone Proxy+Merge Armatureリグレッション修正
- 1.16.2: 非定数カーブ検査の誤検知修正

### 1.15.x (2025-12)
- 1.15.0: **MA Fit Preview**、**MA Global Collider**、`com.vrchat.avatars`依存除去、VRCFury<1.1250.0+Mesh Cutter/Shape Changer(delete)の非互換警告、GCがアニメ参照オブジェクトを保持するよう変更
- 1.15.1: MA Parameters定義パラメータのメニュー値0問題、非ヒューマノイドでのScale Adjusterエラー修正

### 1.14.x (2025-09)
- 1.14.0: **MA Mesh Cutter追加**(頂点フィルタBy Bone/Blendshape/Axis/Mask)、`GetBonesMapping` API公開。**静的リアクティブの優先度バグ修正(挙動変化の可能性明記)**、Shape ChangerのSet/Delete上書き規則を1.12挙動へ復帰、Head Chop過剰生成修正、リアクティブ初期状態を非VRChatプラットフォームでも適用
- 1.14.1〜1.14.3: genericアバターアップロード失敗、パラメータFloat化時のDriver挙動、Vertex Filter By Axisのプレビュー修正

### 1.13.x (2025-07〜08)
- 1.13.0: **非VRCプラットフォーム実験的対応**、**MA Material Swap**、**MA Platform Filter**、**MA Rename Collision Tags**、Shape Changerの完全削除化(アニメ中でも)、`ModularAvatarMenuItem`のVRCSDK非依存API(旧APIは2.0で削除予告)
- 1.13.1: Blendshape Syncのシーン常時更新/安全設定ブロック時の削除形状未適用修正。1.13.2: Shape Changerによる特定ワールドでのVRChatクラッシュ修正。1.13.3/1.13.4: 頭部配下root boneのfirst person不可視問題修正

### 1.12.x (2025-04)
- 1.12.0: CHANGELOG導入。**Merge Animatorの既存アニメーター置換**、World Scale Object、MA MMD Layer Control、**MMDワールド互換処理の導入**(Merge Blend Tree/リアクティブとの互換修正、MA VRChat Settingsで無効化可)、**Merge Blend Tree→Merge Motionへ改名**(クリップ統合対応)、新NDMF API(IVirtualizeMotion等)採用、**パラメータ自動リネームがパスベースに**(Sync Parameter Sequence利用者は再アップロード推奨)、World FixedのVRCParentConstraint化(Android対応)、アイコン圧縮のiOS対応
- 1.12.1〜1.12.5: 新規プロジェクトでのコンパイルエラー、Merge Motion+Rename Parameters、WD調整の互換修正、NDMF連動修正(重複レイヤー等)。**1.12.5: Play Audioの絶対/相対パス解決規則確定**

### 1.11以前(GitHub Releases。主要トピックのみ)
- 1.11 (2025-02): Merge Animator相対パスモードの内部改善、NDMF 1.6系対応
- 1.10 (2024-12): NDMF AnimatorServices移行準備、安定性改善
- 1.9 (2024-09): NDMFプレビュー対応強化(Shape Changer等)、VQTがVHA/WFOを削除しなくなる基準バージョン
- 1.8 (2024-06): **リアクティブコンポーネント(Object Toggle / Shape Changer / Material Setter)導入**
- 1.7以前 (2023〜2024-05): NDMF移行(1.6で完了)、Merge Armature/Menu/Parametersの基礎確立

### 推奨組み合わせ
- MA 1.17.x + NDMF 1.13〜1.14 + AAO 1.9.x + TTT 1.0.x(2026年前半の標準構成)
- MAとNDMFは同一作者のため**同時に更新**するのが原則。MAのみ更新してNDMFが古いとコンパイルエラーになりうる(vpmDependenciesで下限は保証されるがVCC以外の導入経路では要注意)
