# 外部LLM評価レポート: ChatGPT / Gemini(URL渡し方式) (2026-07-02)

## 実施方法

ユーザーが各LLMのWebチャットに以下の形式で12問(レベル1初心者相談)を投稿:

```
https://github.com/exor455/VRC_Unity_tutorial/blob/main/vrchat-avatar-kb/index.md

<相談文>
```

※KB本体はindex.md(目次)のblob URLのみ。バンドル添付なし。

## 結論: **blob URL渡しは両モデルとも機能しなかった**

| | KB到達 | 証拠 |
|---|---|---|
| ChatGPT | ほぼ不使用 | 「そのGitHubのガイド自体は一般的な手順なので」と目次を一瞥して切り捨て。リンク先19ファイルを未読。KB固有の内容・出典が全回答でゼロ |
| Gemini | **完全に不使用** | 冒頭で「リンクの中身を確認することができませんでした」と自己申告(github.comのblobページをフェッチ不可) |

以降の回答はすべて**各モデルの学習知識のみ**。よってこの評価は「KBの品質」ではなく「**KBなしの素のLLMがどの程度戦えるか**」の対照実験となった。

## 採点(レベル1基準: (a)確認質問 (b)原因候補 (c)平易さ (d)出典・捏造なし)

### ChatGPT(5問分の回答を採点): ○0 / △4 / ×1

| 質問 | 判定 | 主な理由 |
|---|---|---|
| 1 アップロード不可 | △ | 問診良好だがKB第一手(バージョン揃え/サイズ/R&W)欠落 |
| 2 服爆発 | × | **「lilToon AvatarTools」という存在しない着せ替えツールを提示** |
| 10 服消えない | △ | 手動Animatorデバッグ偏重。synced/256bit分岐欠落 |
| 11 Unity固まった | △ | 内容は良質(待つ→Library削除)だが一般知識 |
| 12 MMD顔動かない | △ | 概ね正しいが出典なし |

### Gemini(12問): ○0 / △7 / ×5

| 質問 | 判定 | 主な理由 |
|---|---|---|
| 1 アップロード不可 | △ | 内容妥当(日本語ユーザー名の指摘は良い)だがKB第一手欠落 |
| 2 服爆発 | △ | MA推奨は正しいが、手動マトリョーシカ移植を「正しい例」として教える旧式 |
| 3 非対応衣装 | × | **「Kizuna Style」(存在しない)と「AvatarOptimizer=位置調整ツール」(誤)を提示**。もちふぃった～に不到達 |
| 4 笑うと目が変 | × | 「服のEyeボーンをUnityが迷子になって動かす」という誤メカニズム+「Setup OutfitがMA Merge Animatorを勝手に付けて悪さ」という事実誤認 |
| 5 瞬きしない | △ | Eyelids設定の説明は正確。AAO凍結の観点欠落 |
| 6 腕が細いまま | × | **「腕ボーンにMA Bone Proxy(Force Direct)を付ければScale固定できる」は完全な誤用**(Bone Proxyは装着用。正解はMA Scale Adjuster)。初心者を確実に迷子にする |
| 7 全身ピンク | △ | 概ね正しいが、VRChat文脈でURP変換(Fix to URP)を勧めるのはノイズ |
| 8 Questロボット | △ | Show Avatar/Blueprint ID一致は正確。**VQT不到達**で手動Mobile シェーダー差し替えを案内 |
| 9 重い | △ | AAO T&O/Remove Mesh By BlendShape/テクスチャ縮小とKBに近い良回答。実測(APW)欠落 |
| 10 服消えない | × | **「MA Toggle」という存在しないコンポーネント名**(正: MA Object Toggle+MA Menu Item)。Add Componentで検索しても見つからず詰む |
| 11 Unity固まった | △ | 良質(「9割はLibrary破損」は誇張) |
| 12 MMD顔動かない | × | **「AAO Trace And OptimizeがWrite Defaultsの競合を自動修正してくれる」は機能の捏造**+「MMD4Mechanim」をMMD対応化ツールとして誤案内(実際はMMDモデルのUnity取込ツール)。正解(MA 1.12+のMMD対応/MA VRChat Settings)に不到達 |

## 3者比較

| | KB | ○ | △ | × | 捏造の例 |
|---|---|---|---|---|---|
| Claude Sonnet(バンドル参照) | 使用 | **12** | 0 | 0 | なし |
| ChatGPT(URL渡し) | 不使用 | 0 | 4 | 1 | lilToon AvatarTools |
| Gemini(URL渡し) | 不使用 | 0 | 7 | 5 | Kizuna Style / MA Bone Proxyの誤用 / MA Toggle / AAOのWD修正機能 / MMD4Mechanim誤案内 |

## 学び

1. **配布方式の結論**: 目次のblob URL 1本ではKBは届かない(ChatGPTはリンク先を巡回しない、Geminiはblobページ自体を読めない)。外部LLMには**バンドル(単一md)を添付ファイルで渡す**のが唯一確実な方法(`kb-eval/dist/`のキット)
2. **KBの存在意義の実証**: 素のLLMは「それっぽい」回答を量産するが、**存在しないツール名・存在しない機能を自信満々に案内する**(特にMA/AAO等の細部)。「調べればわかることをKBに書く必要があるか」への実地回答: KBのアンカーがないとこのレベルの捏造が混ざる
3. 素のLLMでも一般Unity知識(Library削除、Eyelids設定、ピンク=シェーダー欠落)は概ね正確 → KBは「一般知識の丸写し」より「ツール固有の正確な名前・機能・バージョン・順序」に厚みを置く現方針が正しい

## 次のアクション

- [ ] バンドル添付方式(`vrchat-avatar-kb-bundle.md`+`eval-prompt.md`)でChatGPT/Geminiを再評価し、「渡し方の問題」か「モデルの問題」かを最終切り分け
