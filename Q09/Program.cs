using System;
using System.Linq;
using System.Text;

namespace Q09
{
    /// <summary>
    /// スペースで区切られた単語列に対して，各単語の先頭と末尾の文字は残し，それ以外の文字の順序をランダムに並び替えるプログラムを作成せよ．
    /// ただし，長さが４以下の単語は並び替えないこととする．適当な英語の文（例えば"I couldn't believe that I could actually understand what I was reading : the phenomenal power of the human mind ."）を与え，その実行結果を確認せよ
    /// </summary>
    class MainClass
    {
        public static void Main(string[] args)
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
            var indicies = Enumerable.Range(1, word.Length - 2).ToList();
            while (indicies.Count > 0)
            {
                int value = random.Next(indicies.Count);
                stringBuilder.Append(word[indicies[value]]);
                indicies.RemoveAt(value);
            }

            return word[0] + stringBuilder.ToString() + word[word.Length - 1];
        }
    }
}