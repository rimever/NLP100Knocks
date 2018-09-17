using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Chapter01.Core
{
    /// <summary>
    /// 回答を行うクラスです。
    /// </summary>
    public class AnswerService
    {
        /// <summary>
        /// 00. 文字列の逆順
        /// 文字列"stressed"の文字を逆に（末尾から先頭に向かって）並べた文字列を得よ．
        /// </summary>
        public void Answer00()
        {
            string text = "stressed";
            Console.WriteLine(string.Join("", text.Reverse()));
        }

        /// <summary>
        /// 01. 「パタトクカシーー」
        /// 「パタトクカシーー」という文字列の1,3,5,7文字目を取り出して連結した文字列を得よ
        /// </summary>
        public void Answer01()
        {
            string text = "パタトクカシーー";
            int[] index = {1, 3, 5, 7};
            StringBuilder result = new StringBuilder();
            foreach (var item in index)
            {
                result.Append(text[item - 1]);
            }

            Console.WriteLine(result.ToString());
        }

        /// <summary>
        /// 「パトカー」＋「タクシー」の文字を先頭から交互に連結して文字列「パタトクカシーー」を得よ．
        /// </summary>
        public void Answer02()
        {
            string result = MergeText("パトカー", "タクシー");
            Debug.Assert(result == "パタトクカシーー", $"{result} == パタトクカシーー");
            Console.WriteLine(result);
        }

        /// <summary>
        /// ２つの文字列を交互に連結して文字列を生成します。
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static string MergeText(string a, string b)
        {
            return string.Join(String.Empty, EnumerableCrossChar(a, b));
        }

        /// <summary>
        /// 交互に文字列を列挙します。
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static IEnumerable<char> EnumerableCrossChar(string a, string b)
        {
            for (int i = 0; i < Math.Max(a.Length, b.Length); i++)
            {
                if (i < a.Length)
                {
                    yield return a[i];
                }

                if (i < b.Length)
                {
                    yield return b[i];
                }
            }
        }

        /// <summary>
        /// "Now I need a drink, alcoholic of course, after the heavy lectures involving quantum mechanics."という文を単語に分解し，各単語の（アルファベットの）文字数を先頭から出現順に並べたリストを作成せよ．
        /// </summary>
        public void Answer03()
        {
            var result =
                EnumerableWordLength(
                        "Now I need a drink, alcoholic of course, after the heavy lectures involving quantum mechanics.")
                    .ToList();
            Console.WriteLine(string.Join("", result.Select(i => i.ToString())));
        }

        private static IEnumerable<int> EnumerableWordLength(string text)
        {
            if (text.EndsWith("."))
            {
                text = text.Substring(0, text.Length - 1);
            }

            var words = text.Split(' ', ',');
            foreach (var word in words)
            {
                yield return word.Length;
            }
        }

        /// <summary>
        /// "Hi He Lied Because Boron Could Not Oxidize Fluorine. New Nations Might Also Sign Peace Security Clause. Arthur King Can."という文を単語に分解し，1, 5, 6, 7, 8, 9, 15, 16, 19番目の単語は先頭の1文字，それ以外の単語は先頭に2文字を取り出し，取り出した文字列から単語の位置（先頭から何番目の単語か）への連想配列（辞書型もしくはマップ型）を作成せよ．
        /// </summary>
        public void Answer04()
        {
            string text =
                "Hi He Lied Because Boron Could Not Oxidize Fluorine. New Nations Might Also Sign Peace Security Clause. Arthur King Can.";
            var oneCharIndex = new[] {1, 5, 6, 7, 8, 9, 15, 16, 19};
            var result = CreateWordHash(text, oneCharIndex);
            Console.WriteLine(string.Join(",", result.Select(pair => $"{pair.Key}=>{pair.Value}")));
        }

        private static IDictionary<string, int> CreateWordHash(string text, int[] oneCharIndex)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            if (text.EndsWith("."))
            {
                text = text.Substring(0, text.Length);
            }

            var words = text.Split(' ');
            foreach (var item in words.Select((value, index) => new {value, index}))
            {
                int length = 2;
                var wordNo = item.index + 1;
                if (oneCharIndex.Contains(wordNo))
                {
                    length = 1;
                }

                result.Add(item.value.Substring(0, length), wordNo);
            }

            return result;
        }


        public void Answer05()
        {
            string text = "I am an NLPer";
            Console.WriteLine("文字bi-gram");
            var result = CreateTextNgram(text, 2);
            Console.WriteLine(string.Join(" ", result));
            Console.WriteLine("単語bi-gram");
            var wordNgram = CreateWordNgram(text, 2);
            var wordNgramResult = "";
            foreach (var ngram in wordNgram)
            {
                string value = $"[{string.Join(",", ngram)}]";
                wordNgramResult += value + " ";
            }

            Console.WriteLine(wordNgramResult);
        }

        private static IEnumerable<IEnumerable<string>> CreateWordNgram(string text, int v)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            if (text.EndsWith("."))
            {
                text = text.Substring(0, text.Length);
            }

            var words = text.Split(' ');
            for (int i = 0; i <= words.Length - v; i++)
            {
                yield return words.Skip(i).Take(v);
            }
        }

        private static IList<string> CreateTextNgram(string text, int v)
        {
            if (text.EndsWith("."))
            {
                text = text.Substring(0, text.Length);
            }

            text = text.Replace(" ", string.Empty);
            var result = new List<string>();
            for (int i = 0; i < text.Length - v; i++)
            {
                result.Add(text.Substring(i, v));
            }

            return result;
        }

        /// <summary>
        /// "paraparaparadise"と"paragraph"に含まれる文字bi-gramの集合を，それぞれ, XとYとして求め，XとYの和集合，積集合，差集合を求めよ．さらに，'se'というbi-gramがXおよびYに含まれるかどうかを調べよ．
        /// </summary>
        public void Answer06()
        {
            var textA = "paraparaparadise";
            var textB = "paragraph";
            var a = CreateTextNgram(textA, 2);
            var b = CreateTextNgram(textB, 2);
            Console.WriteLine("和集合");
            Console.WriteLine(string.Join(" ", a.Union(b)));
            Console.WriteLine("差集合");
            Console.WriteLine(string.Join(" ", a.Intersect(b)));
            Console.WriteLine("seという単語が含まれるか？");
            var resultA = (a.Contains("se") ? "含まれる" : "含まれない");
            var resultB = (b.Contains("se") ? "含まれる" : "含まれない");
            Console.WriteLine($"{textA}には,{resultA}");
            Console.WriteLine($"{textB}には,{resultB}");
        }

        /// <summary>
        /// 引数x, y, zを受け取り「x時のyはz」という文字列を返す関数を実装せよ．さらに，x=12, y="気温", z=22.4として，実行結果を確認せよ．
        /// </summary>
        public void Answer07()
        {
            var x = 12;
            var y = "気温";
            var z = 22.4;
            Console.WriteLine(BuildText(x, y, z));
        }

        /// <summary>
        /// 文字列を返す関数。
        /// </summary>
        /// <returns>The text.</returns>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="z">The z coordinate.</param>
        private static string BuildText(int x, string y, double z)
        {
            return $"{x}時の{y}は{z}";
        }

        /// <summary>
        /// 与えられた文字列の各文字を，以下の仕様で変換する関数cipherを実装せよ．
        /// 英小文字ならば(219 - 文字コード)の文字に置換
        /// その他の文字はそのまま出力
        /// この関数を用い，英語のメッセージを暗号化・復号化せよ．
        /// </summary>
        public void Answer08()
        {
            string text = "Nlp100knock";
            string encrypt = cipher(text);
            Console.WriteLine(encrypt);
            string decrypt = DecryptCipher(encrypt);
            Console.WriteLine(decrypt);
            Debug.Assert(text == decrypt, "可逆性あり");
        }

        private static string DecryptCipher(string text)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < text.Length; i++)
            {
                var value = text[i];
                int num = 219 - value;
                if (num >= 'a'
                    && num <= 'z')
                {
                    result.Append((char) num);
                }
                else
                {
                    result.Append(value);
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 問題が関数名を指定していたので、命名規則違反だが小文字始まり
        /// </remarks>
        /// <returns>The cipher.</returns>
        /// <param name="text">Text.</param>
        // ReSharper disable once InconsistentNaming
        private static string cipher(string text)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < text.Length; i++)
            {
                var value = text[i];
                if (value >= 'a' && value <= 'z')
                {
                    int num = value;
                    result.Append((char) (219 - num));
                }
                else
                {
                    result.Append(value);
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// スペースで区切られた単語列に対して，各単語の先頭と末尾の文字は残し，それ以外の文字の順序をランダムに並び替えるプログラムを作成せよ．
        /// ただし，長さが４以下の単語は並び替えないこととする．適当な英語の文（例えば"I couldn't believe that I could actually understand what I was reading : the phenomenal power of the human mind ."）を与え，その実行結果を確認せよ
        /// </summary>
        public void Answer09()
        {
            string text =
                "I couldn't believe that I could actually understand what I was reading : the phenomenal power of the human mind .";
            Console.WriteLine(string.Join(" ", text.Split(' ').Select(arg => Randomize(arg))));
        }

        /// <summary>
        /// 単語の先頭と末尾の文字は残し，それ以外の文字の順序をランダムに並び替えます。
        /// ただし、長さが4以下の単語は並び替えません。
        /// </summary>
        /// <returns>The randomize.</returns>
        /// <param name="word">Word.</param>
        private static string Randomize(string word)
        {
            if (word.Length <= 4)
            {
                return word;
            }

            StringBuilder stringBuilder = new StringBuilder();
            Random random = new Random();
            var indices = Enumerable.Range(1, word.Length - 2).ToList();
            while (indices.Count > 0)
            {
                int value = random.Next(indices.Count);
                stringBuilder.Append(word[indices[value]]);
                indices.RemoveAt(value);
            }

            return word[0] + stringBuilder.ToString() + word[word.Length - 1];
        }
    }
}