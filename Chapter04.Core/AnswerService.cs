using System;
using System.Collections.Generic;
using System.Linq;

namespace Chapter04.Core
{
    /// <summary>
    /// 第4章: 形態素解析
    /// 夏目漱石の小説『吾輩は猫である』の文章（neko.txt）をMeCabを使って形態素解析し，その結果をneko.txt.mecabというファイルに保存せよ．
    /// このファイルを用いて，以下の問に対応するプログラムを実装せよ．
    /// なお，問題37, 38, 39はmatplotlibもしくはGnuplotを用いるとよい．
    /// </summary>
    public class AnswerService
    {

        private readonly MorphologicalAnalyzer _analyzer = new MorphologicalAnalyzer();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AnswerService()
        {
            _analyzer.Execute();
        }

        /// <summary>
        /// 30. 形態素解析結果の読み込み
        /// 形態素解析結果（neko.txt.mecab）を読み込むプログラムを実装せよ．
        /// ただし，各形態素は表層形（surface），基本形（base），品詞（pos），品詞細分類1（pos1）をキーとするマッピング型に格納し，1文を形態素（マッピング型）のリストとして表現せよ．
        /// 第4章の残りの問題では，ここで作ったプログラムを活用せよ．
        /// </summary>
        /// <remarks>
        /// 問題ではマッピング型と表しているが、ここではオブジェクトとして扱っている。
        /// </remarks>
        public void Answer30()
        {
            foreach (var word in _analyzer.EnumerableWords())
            {
                Console.WriteLine($"表層形 = {word.Surface}, 基本形 = {word.Base}, 品詞 = {word.Pos}, 品詞細分類1 = {word.Pos1}");
            }
        }

        /// <summary>
        /// 31. 動詞
        /// 動詞の表層形をすべて抽出せよ．
        /// </summary>
        public void Answer31()
        {
            foreach (var word in _analyzer.EnumerableWords())
            {
                if (word.Pos != "動詞")
                {
                    continue;
                }

                Console.WriteLine($"{word.Surface}");
            }
        }

        /// <summary>
        /// 32. 動詞の原形
        /// 動詞の原形をすべて抽出せよ．
        /// </summary>
        public void Answer32()
        {
            foreach (var word in _analyzer.EnumerableWords())
            {
                if (word.Pos != "動詞")
                {
                    continue;
                }

                Console.WriteLine($"{word.Base}");
            }
        }

        /// <summary>
        /// 33. サ変名詞
        /// サ変接続の名詞をすべて抽出せよ．
        /// </summary>
        public void Answer33()
        {
            foreach (var word in _analyzer.EnumerableWords())
            {
                if (word.Pos1 != "サ変接続")
                {
                    continue;
                }

                Console.WriteLine($"表層形 = {word.Surface}, 基本形 = {word.Base}, 品詞 = {word.Pos}, 品詞細分類1 = {word.Pos1}");
            }
        }
        /// <summary>
        /// 34. 「AのB」
        /// 2つの名詞が「の」で連結されている名詞句を抽出せよ．
        /// </summary>
        public void Answer34()
        {
            IList<Word> combinationWords = new List<Word>();
            foreach (var word in _analyzer.EnumerableWords())
            {
                if (combinationWords.Count == 0)
                {
                    if (word.Pos == "名詞")
                    {
                        combinationWords.Add(word);
                    }
                }
                else if (combinationWords.Count == 1)
                {
                    if (word.Base == "の")
                    {
                        combinationWords.Add(word);
                    }
                    else
                    {
                        combinationWords.Clear();
                    }
                }
                else if (combinationWords.Count == 2)
                {
                    if (word.Pos == "名詞")
                    {
                        Console.WriteLine($"{combinationWords[0].Surface}{combinationWords[1].Surface}{word.Surface}");
                    }
                    combinationWords.Clear();
                }
            }
        }
        /// <summary>
        /// 35. 名詞の連接
        /// 名詞の連接（連続して出現する名詞）を最長一致で抽出せよ．
        /// </summary>
        public void Answer35()
        {
            IList<Word> max = new List<Word>();
            IList<Word> now = new List<Word>();
            foreach (var word in _analyzer.EnumerableWords())
            {
                bool isCombo = word.Pos == "名詞";
                if (isCombo)
                {
                    now.Add(word);
                }
                if (now.Count > max.Count)
                {
                    max = new List<Word>(now.Select(s => s));
                }
                if (!isCombo)
                {
                    now.Clear();
                }
            }

            Console.WriteLine($"{string.Join(string.Empty, max.Select(s => s.Surface))}:{max.Count}回");
        }
        /// <summary>
        /// 36. 単語の出現頻度
        /// 文章中に出現する単語とその出現頻度を求め，出現頻度の高い順に並べよ．
        /// </summary>
        public void Answer36()
        {
            IDictionary<string, List<Word>> result = _analyzer.GetGroupByWord();

            foreach (var pair in result.OrderByDescending(pair => pair.Value.Count))
            {
                var first = pair.Value.FirstOrDefault();
                Console.WriteLine($"{first.Base} {first.Pos} {first.Pos1} {pair.Value.Count}回");
            }
        }



        /// <summary>
        /// 37. 頻度上位10語
        /// 出現頻度が高い10語とその出現頻度をグラフ（例えば棒グラフなど）で表示せよ．
        /// </summary>
        public void Answer37()
        {
            using (FormQ37 form = new FormQ37(_analyzer))
            {
                form.ShowDialog();
            }
        }
        /// <summary>
        /// 38. ヒストグラム
        /// 単語の出現頻度のヒストグラム（横軸に出現頻度，縦軸に出現頻度をとる単語の種類数を棒グラフで表したもの）を描け．
        /// </summary>
        public void Answer38()
        {
            using (FormQ38 form = new FormQ38(_analyzer))
            {
                form.ShowDialog();
            }
        }
        /// <summary>
        /// 39. Zipfの法則
        /// 単語の出現頻度順位を横軸，その出現頻度を縦軸として，両対数グラフをプロットせよ．
        /// </summary>
        public void Answer39()
        {
            using (FormQ39 form = new FormQ39(_analyzer))
            {
                form.ShowDialog();
            }
        }
    }
}