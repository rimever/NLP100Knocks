using System;
using System.Collections.Generic;
using System.Linq;

namespace Chapter05.Core
{
    /// <summary>
    /// 第5章: 係り受け解析
    /// 夏目漱石の小説『吾輩は猫である』の文章（neko.txt）をCaboChaを使って係り受け解析し，その結果をneko.txt.cabochaというファイルに保存せよ．
    /// このファイルを用いて，以下の問に対応するプログラムを実装せよ．
    /// </summary>
    public class AnswerService
    {
        private readonly CabochaAnalyzer _analyzer = new CabochaAnalyzer();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AnswerService()
        {
            _analyzer.Execute();
        }

        /// <summary>
        /// 41. 係り受け解析結果の読み込み（文節・係り受け）
        /// 40に加えて，文節を表すクラスChunkを実装せよ．
        /// このクラスは形態素（Morphオブジェクト）のリスト（morphs），係り先文節インデックス番号（dst），係り元文節インデックス番号のリスト（srcs）をメンバ変数に持つこととする．
        /// さらに，入力テキストのCaboChaの解析結果を読み込み，１文をChunkオブジェクトのリストとして表現し，8文目の文節の文字列と係り先を表示せよ．第5章の残りの問題では，ここで作ったプログラムを活用せよ．
        /// 
        /// </summary>
        public void Answer41()
        {
            var sentence = _analyzer.Sentences.Skip(7).FirstOrDefault();
            foreach (var chunk in sentence.Chunks)
            {
                string from = string.Join(string.Empty, chunk.Morphs.Select(m => m.Surface));
                string to = string.Empty;
                if (chunk.Dst >= 0)
                {
                    to = string.Join(string.Empty, sentence.Chunks[chunk.Dst].Morphs.Select(m => m.Surface));
                }

                Console.WriteLine($"{from} -> {to}");
            }
        }

        /// <summary>
        ///         40. 係り受け解析結果の読み込み（形態素）
        /// 形態素を表すクラスMorphを実装せよ．このクラスは表層形（surface），基本形（base），品詞（pos），品詞細分類1（pos1）をメンバ変数に持つこととする．さらに，CaboChaの解析結果（neko.txt.cabocha）を読み込み，各文をMorphオブジェクトのリストとして表現し，3文目の形態素列を表示せよ．
        /// 
        /// </summary>
        public void Answer40()
        {
            var sentence = _analyzer.Sentences.Skip(2).FirstOrDefault();
            foreach (var chunk in sentence.Chunks)
            {
                foreach (var morph in chunk.Morphs)
                {
                    Console.WriteLine(
                        $"表層形 = {morph.Surface}, 基本形 = {morph.Base}, 品詞 = {morph.Pos}, 品詞細分類 = {morph.Pos1}");
                }
            }
        }

        /// <summary>
        ///         42. 係り元と係り先の文節の表示
        /// 係り元の文節と係り先の文節のテキストをタブ区切り形式ですべて抽出せよ．ただし，句読点などの記号は出力しないようにせよ．
        /// </summary>
        public void Answer42()
        {
            foreach (var sentence in _analyzer.Sentences)
            {
                foreach (var chunk in sentence.Chunks)
                {
                    var fromChunks = sentence.Chunks.Where(c => chunk.Srcs.Contains(c.Id)).ToList();

                    string now = string.Join(string.Empty,
                        chunk.Morphs.Where(m => m.Pos != Morph.SignPosName).Select(m => m.Surface));
                    foreach (var fromChunk in fromChunks)
                    {
                        string from = string.Join(string.Empty,
                            fromChunk.Morphs.Where(m => m.Pos != Morph.SignPosName).Select(m => m.Surface));
                        string to = string.Empty;
                        if (chunk.Dst >= 0)
                        {
                            to = string.Join(string.Empty,
                                sentence.Chunks[chunk.Dst].Morphs.Where(m => m.Pos != Morph.SignPosName)
                                    .Select(m => m.Surface));
                        }

                        Console.WriteLine($"{from}  {now}  {to}");
                    }
                }
            }
        }

        /// <summary>
        /// 43. 名詞を含む文節が動詞を含む文節に係るものを抽出
        /// 名詞を含む文節が，動詞を含む文節に係るとき，これらをタブ区切り形式で抽出せよ．
        /// ただし，句読点などの記号は出力しないようにせよ．
        /// </summary>
        public void Answer43()
        {
            foreach (var sentence in _analyzer.Sentences)
            {
                foreach (var chunk in sentence.Chunks)
                {
                    if (chunk.Dst == -1)
                    {
                        continue;
                    }

                    if (chunk.Morphs.All(m => m.Pos != "名詞"))
                    {
                        continue;
                    }

                    if (sentence.Chunks[chunk.Dst].Morphs.All(m => m.Pos != "動詞"))
                    {
                        continue;
                    }

                    string from = string.Join(string.Empty,
                        chunk.Morphs.Where(m => m.Pos != Morph.SignPosName).Select(m => m.Surface));
                    string to = string.Join(string.Empty,
                        sentence.Chunks[chunk.Dst].Morphs.Where(m => m.Pos != Morph.SignPosName)
                            .Select(m => m.Surface));
                    Console.WriteLine($"{from} -> {to}");
                }
            }
        }

        /// <summary>
        /// 45. 動詞の格パターンの抽出
        /// 今回用いている文章をコーパスと見なし，日本語の述語が取りうる格を調査したい． 動詞を述語，動詞に係っている文節の助詞を格と考え，述語と格をタブ区切り形式で出力せよ． ただし，出力は以下の仕様を満たすようにせよ．
        /// 
        /// 動詞を含む文節において，最左の動詞の基本形を述語とする
        /// 述語に係る助詞を格とする
        /// 述語に係る助詞（文節）が複数あるときは，すべての助詞をスペース区切りで辞書順に並べる
        /// 「吾輩はここで始めて人間というものを見た」という例文（neko.txt.cabochaの8文目）を考える． この文は「始める」と「見る」の２つの動詞を含み，「始める」に係る文節は「ここで」，「見る」に係る文節は「吾輩は」と「ものを」と解析された場合は，次のような出力になるはずである．
        /// 
        /// 始める  で
        /// 見る    は を
        /// このプログラムの出力をファイルに保存し，以下の事項をUNIXコマンドを用いて確認せよ．
        /// 
        /// コーパス中で頻出する述語と格パターンの組み合わせ
        /// 「する」「見る」「与える」という動詞の格パターン（コーパス中で出現頻度の高い順に並べよ）
        /// </summary>
        public void Answer45()
        {
            foreach (var sentence in _analyzer.Sentences)
            {
                foreach (var chunk in sentence.Chunks)
                {
                    var firstWord = chunk.Morphs.FirstOrDefault(m => m.Pos == "動詞");
                    if (firstWord == null)
                    {
                        continue;
                    }

                    var result = new List<string> {firstWord.Base};
                    foreach (var supportWords in chunk.Srcs.Select(src =>
                        sentence.Chunks[src].Morphs.Where(m => m.Pos == "助詞").Select(m => m.Base)))
                    {
                        result.AddRange(supportWords);
                    }

                    Console.WriteLine(string.Join("\t", result));
                }
            }
        }

        /// <summary>
        ///         46. 動詞の格フレーム情報の抽出
        /// 45のプログラムを改変し，述語と格パターンに続けて項（述語に係っている文節そのもの）をタブ区切り形式で出力せよ．45の仕様に加えて，以下の仕様を満たすようにせよ．
        /// 
        /// 項は述語に係っている文節の単語列とする（末尾の助詞を取り除く必要はない）
        /// 述語に係る文節が複数あるときは，助詞と同一の基準・順序でスペース区切りで並べる
        /// 「吾輩はここで始めて人間というものを見た」という例文（neko.txt.cabochaの8文目）を考える． この文は「始める」と「見る」の２つの動詞を含み，「始める」に係る文節は「ここで」，「見る」に係る文節は「吾輩は」と「ものを」と解析された場合は，次のような出力になるはずである．
        /// 
        /// 始める  で      ここで
        /// 見る    は を   吾輩は ものを
        /// 
        /// </summary>
        public void Answer46()
        {
            foreach (var sentence in _analyzer.Sentences)
            {
                foreach (var chunk in sentence.Chunks)
                {
                    var firstWord = chunk.Morphs.FirstOrDefault(m => m.Pos == "動詞");
                    if (firstWord == null)
                    {
                        continue;
                    }

                    var result = new List<string> {firstWord.Base};
                    foreach (var supportWords in chunk.Srcs.Select(src =>
                        sentence.Chunks[src].Morphs.Where(m => m.Pos == "助詞").Select(m => m.Base)))
                    {
                        result.AddRange(supportWords);
                    }

                    foreach (var text in chunk.Srcs.Select(src =>
                        sentence.Chunks[src]).Where(c => c.Morphs.Any(m => m.Pos == "助詞")).Select(c =>
                        string.Join(string.Empty,
                            c.Morphs.Where(m => m.Pos != Morph.SignPosName).Select(m => m.Surface))))
                    {
                        result.Add(text);
                    }

                    Console.WriteLine(string.Join("\t", result));
                }
            }
        }

        /// <summary>
        /// 47. 機能動詞構文のマイニング
        /// 動詞のヲ格にサ変接続名詞が入っている場合のみに着目したい．46のプログラムを以下の仕様を満たすように改変せよ．
        /// 
        /// 「サ変接続名詞+を（助詞）」で構成される文節が動詞に係る場合のみを対象とする
        /// 述語は「サ変接続名詞+を+動詞の基本形」とし，文節中に複数の動詞があるときは，最左の動詞を用いる
        /// 述語に係る助詞（文節）が複数あるときは，すべての助詞をスペース区切りで辞書順に並べる
        /// 述語に係る文節が複数ある場合は，すべての項をスペース区切りで並べる（助詞の並び順と揃えよ）
        /// 例えば「別段くるにも及ばんさと、主人は手紙に返事をする。」という文から，以下の出力が得られるはずである．
        /// 
        /// 返事をする      と に は        及ばんさと 手紙に 主人は
        /// このプログラムの出力をファイルに保存し，以下の事項をUNIXコマンドを用いて確認せよ．
        /// 
        /// コーパス中で頻出する述語（サ変接続名詞+を+動詞）
        /// コーパス中で頻出する述語と助詞パターン
        /// </summary>
        public void Answer47()
        {
            foreach (var sentence in _analyzer.Sentences)
            {
                foreach (var chunk in sentence.Chunks)
                {
                    for (int i = 0; i < chunk.Morphs.Count - 1; i++)
                    {
                        var first = chunk.Morphs.Skip(i).First();
                        var second = chunk.Morphs.Skip(i + 1).First();
                        if (first.Pos1 == "サ変接続"
                            && second.Base == "を")
                        {
                            var toChunk = sentence.Chunks[chunk.Dst];
                            var to =
                                toChunk.Morphs.FirstOrDefault(m => m.Pos == "動詞");
                            if (to == null)
                            {
                                continue;
                            }

                            string target = $"{first.Surface}{second.Surface}{to.Surface}";
                            var chunks = chunk.Srcs.Union(toChunk.Srcs).Where(s => s != chunk.Id && s != toChunk.Id)
                                .OrderBy(s => s).Where(s => s >= 0).Select(s => sentence.Chunks[s]);
                            var from = chunks.Where(c => c.Morphs.Any(cm => cm.Pos == "助詞"));
                            string text =
                                $"{target}\t{string.Join(" ", from.Select(c => c.Morphs.LastOrDefault(cm => cm.Pos == "助詞").Surface))}\t{string.Join(" ", from.Select(c => string.Join(string.Empty, c.Morphs.Select(cm => cm.Surface))))}";
                            Console.WriteLine(text);
                        }
                    }
                }
            }

        }
        /// <summary>
        ///         48. 名詞から根へのパスの抽出
        /// 文中のすべての名詞を含む文節に対し，その文節から構文木の根に至るパスを抽出せよ． ただし，構文木上のパスは以下の仕様を満たすものとする．
        /// 
        /// 各文節は（表層形の）形態素列で表現する
        /// パスの開始文節から終了文節に至るまで，各文節の表現を"->"で連結する
        /// 「吾輩はここで始めて人間というものを見た」という文（neko.txt.cabochaの8文目）から，次のような出力が得られるはずである．
        /// 
        /// 吾輩は -> 見た
        /// ここで -> 始めて -> 人間という -> ものを -> 見た
        /// 人間という -> ものを -> 見た
        /// ものを -> 見た
        /// 
        /// </summary>
        public void Answer48()
        {
            foreach (var sentence in _analyzer.Sentences)
            {
                foreach (var chunk in sentence.Chunks)
                {
                    if (chunk.Morphs.All(m => m.Pos != "名詞"))
                    {
                        continue;
                    }

                   var list = new List<Chunk> {chunk};
                    while (list.Last().Dst > 0)
                    {
                        var toChunk = sentence.Chunks[list.Last().Dst];
                        list.Add(toChunk);
                    }
                    Console.WriteLine(string.Join(" -> ", list.Select(c => c.Surface)));
                }
            }
        }
    }

    /*
       49. 名詞間の係り受けパスの抽出
       文中のすべての名詞句のペアを結ぶ最短係り受けパスを抽出せよ．ただし，名詞句ペアの文節番号がi
       とj
       （i<j
       ）のとき，係り受けパスは以下の仕様を満たすものとする．
       
       問題48と同様に，パスは開始文節から終了文節に至るまでの各文節の表現（表層形の形態素列）を"->"で連結して表現する
       文節i
       とj
       に含まれる名詞句はそれぞれ，XとYに置換する
       また，係り受けパスの形状は，以下の2通りが考えられる．
       
       文節i
       から構文木の根に至る経路上に文節j
       が存在する場合: 文節i
       から文節j
       のパスを表示
       上記以外で，文節i
       と文節j
       から構文木の根に至る経路上で共通の文節k
       で交わる場合: 文節i
       から文節k
       に至る直前のパスと文節j
       から文節k
       に至る直前までのパス，文節k
       の内容を"|"で連結して表示
       例えば，「吾輩はここで始めて人間というものを見た。」という文（neko.txt.cabochaの8文目）から，次のような出力が得られるはずである．
       
       Xは | Yで -> 始めて -> 人間という -> ものを | 見た
       Xは | Yという -> ものを | 見た
       Xは | Yを | 見た
       Xで -> 始めて -> Y
       Xで -> 始めて -> 人間という -> Y
       Xという -> Y
            */
}