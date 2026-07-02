# KB回答品質評価: 質問セット v1

RAG利用を想定した評価用質問(14問)と採点基準。回答エージェントには質問のみを渡し、`vrchat-avatar-kb/` 以外の知識使用を禁止する。

採点: ○=キーポイントを全て含み誤りなし / △=一部欠落または曖昧 / ×=誤り・不明を含む、または「記載なし」誤答

| # | 質問(要約) | 期待するキーポイント | 期待出典 |
|---|---|---|---|
| 1 | 4ツール導入時のOptimizing内実行順 | TTT(Atlas等)→VQT変換(Auto既定)→AAO→VQT形式チェック | 00 §1.6 |
| 2 | AAOが最後に実行される仕組み | FullName序数ソート+U+FFDC始まりの名前空間(1.9.0+)。後に動くにはAfterPlugin明示 | 00 §1.3 / 09 |
| 3 | lilToonアニメギミックがアップロード後だけ死ぬ | ビルド時最適化のプロパティ定数化。lilToon 1.8.0+でアニメ考慮。切り分けはLILTOON_DISABLE_OPTIMIZATION、テストビルド非最適化オプションとの差 | 05 |
| 4 | Poiyomiロック済みでアニメが効かない | Animated指定必須。Rename Animated時はプロパティ名がマテリアル固有名に変わる→クリップ側も一致必要。アンロック→編集→再ロック | 06 |
| 5 | PCでVery Poorになるトライアングル数 | 70,000超(Good〜Poorの上限が同値) | 13 |
| 6 | PC/Quest同期ずれ対策 | MA Sync Parameter Sequence(パラメータ順序)+VQT Network ID Assigner(PhysBone ID) | 02 / 11 |
| 7 | Fury無しの値+秒数フェード | AvatarMenuCreatorForMA「徐々に変化」(線形のみ)/Flare Interpolate(独自メニュー・線形) | 08 |
| 8 | もちふぃった～変換後に動かない可能性 | 衣装のアニメーション・複雑なギミック(公式明記)。PhysBone系は概ね引き継ぎ | 03 |
| 9 | TTT Atlas×AAO RemoveMesh連携 | NegotiateAAOPass: 削除領域をアトラス除外+UV退避報告(TTT 0.9+/AAO 1.8 API)。同一メッシュ例外は0.10.9修正 | 04 / 09 |
| 10 | Av3Emulator時のAAOエラー | Read/Write無効メッシュ(1.8.0で大幅改善、Av3Emu起動時のみ残存) | 09 / 12 |
| 11 | FloorAdjuster×TTTデカールずれ | Transform移動系との順序問題→TTT 0.8.2でTTT先行に修正。MA 1.17+の内蔵版もTTT後(late-transform-stages) | 07 / 00 / 02 |
| 12 | GoGo Locoの同期bit数 | 16/256bit(+Extra飛行系で+1bit) | 15 |
| 13 | Androidサイズ制限 | DL 10MB / 非圧縮40MB(SDK 3.5.2+で検査。Build & Testは非検査) | 13 |
| 14 | NDMFからVRCFuryの順序制御 | 不可。VRCFuryは非NDMF(-10000固定)で常にTransforming後・Optimizing前 | 14 / 00 §1.1 |

## 運用メモ

- 回答エージェント: Claude Code の Agent tool(model: sonnet)。KBディレクトリのみ参照可、外部知識禁止、出典必須の制約付きプロンプト
- 採点者: メインセッション(本rubricと突き合わせ)
- 結果は `kb-eval/report-<日付>.md` に記録し、×/△はKB本体の改善(追記・見出し調整)につなげる
