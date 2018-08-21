using System;
using System.Collections.Generic;
using System.Linq;

namespace Q04
{
    /// <summary>
    /// "Hi He Lied Because Boron Could Not Oxidize Fluorine. New Nations Might Also Sign Peace Security Clause. Arthur King Can."という文を単語に分解し，1, 5, 6, 7, 8, 9, 15, 16, 19番目の単語は先頭の1文字，それ以外の単語は先頭に2文字を取り出し，取り出した文字列から単語の位置（先頭から何番目の単語か）への連想配列（辞書型もしくはマップ型）を作成せよ．
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            string text =
                "Hi He Lied Because Boron Could Not Oxidize Fluorine. New Nations Might Also Sign Peace Security Clause. Arthur King Can.";
            var oneCharIndex = new[] {1, 5, 6, 7, 8, 9, 15, 16, 19};
            var result = CreateWordHash(text, oneCharIndex);
            Console.WriteLine(string.Join(",", result.Select(pair => $"{pair.Key}=>{pair.Value}")));
            Console.ReadKey();
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
    }
}