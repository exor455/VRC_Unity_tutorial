# VRCFury(防御的ナレッジ)

- リポジトリ: https://github.com/VRCFury/VRCFury
- 公式サイト: https://vrcfury.com

> **このKBの方針ではVRCFuryは使用しない**(NDMF/MA系で統一)。ただし**導入済みアセット・配布ギミック・他人のアバターに極めて高頻度で含まれる**ため、トラブルシュートのための知識としてこのページを置く。ソース裏付けは限定的で、既知の相互作用は本KB内でソース確認済みの事実(MA/AAO/VQT側の記述)を根拠とする。

## 概要

英語圏で最大手の非破壊改変ツール。**NDMFベースではない**独自実装で、VRCSDKのビルドコールバック(**callbackOrder -10000**)とPlay Mode時の適用で動作する。

主な機能(代表的なもの):
- **Toggle**: トグル生成。**Transition Time(値+秒数のトゥイーン)**を持つ(KB [08の比較表](08-ecosystem-tools.md)参照)
- **Full Controller**: 既存のアニメーター/メニュー/パラメータ一式をアバターへ合成(MAのMerge Animator+Menu Installer+Parametersに相当)
- **Armature Link**: 衣装アーマチュアの結合(MA Merge Armature相当)
- その他: 各種Fixes(Anchor Override統一等)、ギミック向けコンポーネント多数

## 運用上の特性(トラブルの源泉)

- **バージョン固定が事実上できない**: ローリングリリース+自動アップデートの思想で、「特定バージョンで止めて安定運用」がしづらい。再現性が必要なプロジェクトでNDMF系と思想が衝突する主因
- **NDMFの順序制御が及ばない**: 常に「NDMF Transforming(-11000)の後、NDMF Optimizing(-1025)の前」で実行される([00 §1.1](00-cross-tool.md))。NDMF側から`BeforePlugin`/`AfterPlugin`で位置調整することは不可能
- コンポーネントのシリアライズが独自形式で、VRCFury未導入プロジェクトに持ち込むと不明コンポーネント化しやすい

## 既知の相互作用(本KBでソース確認済みのもの)

| 相手 | 内容 |
|---|---|
| NDMF | NDMF 1.3.0/1.3.2/1.5.0でVRCFury互換のためのフック順序調整・Transforming後のアセットシリアライズが入っている([01](01-ndmf.md)) |
| MA | **MA 1.15+はVRCFury < 1.1250.0 と Mesh Cutter / Shape Changer(delete mode)の併用に非互換警告**を出す([02](02-modular-avatar.md)) |
| AAO | AAO 1.8.11で`VRCFuryTest`対応。順序の議論はAAOリポジトリDiscussion #860が公式見解(併用は概ね動作するが、AAOがVRCFury生成物を解析するのはVRCFuryが先に走った場合のみ=Optimizing側のAAOは解析可能) |
| VQT | **VQT 2.10+の変換フェーズ「Auto」はVRCFury検出時にTransformingを選ぶ**(VRCFury生成マテリアルを変換対象にするため)([11](11-vrcquesttools.md)) |

## 併用時の定石(他人のアバター/VRCFury入りアセットを触る場合)

1. **同じ役割を二重管理しない**: 衣装結合はMA Merge ArmatureかVRCFury Armature Linkの**どちらか一方**に統一。トグルも同様
2. VRCFury入りギミックをそのまま使う場合、**NDMF系ツールの生成物との順序は変えられない**ことを前提に設計する(VRCFuryのToggleがNDMF Optimizing適用後の状態を参照することはない、等)
3. ビルド結果の検証はManual BakeではなくPlay Mode/アップロードで行う(VRCFuryはNDMFのManual Bakeに乗らない)
4. 削除する場合はパッケージ削除後に残存コンポーネント(missing script)を掃除([17](17-unity-troubleshooting.md))
5. MAとVRCFuryの機能対応表を意識して移行する: Armature Link→Merge Armature、Full Controller→Merge Animator+Menu Installer+Parameters、Toggle(Transition Time)→[08の比較表](08-ecosystem-tools.md)の代替(AvatarMenuCreatorForMA等)

## 関連ページ

- 実行順序の全体像: [00-cross-tool.md §1.1](00-cross-tool.md)
- トゥイーン機能の代替: [08-ecosystem-tools.md](08-ecosystem-tools.md)
