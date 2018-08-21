using System;
using System.Collections.Generic;
using System.Linq;

namespace Q03
{
    /// <summary>
    /// "Now I need a drink, alcoholic of course, after the heavy lectures involving quantum mechanics."という文を単語に分解し，各単語の（アルファベットの）文字数を先頭から出現順に並べたリストを作成せよ．
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            var result =
                EnumerableWordLength(
                        "Now I need a drink, alcoholic of course, after the heavy lectures involving quantum mechanics.")
                    .ToList();
            Console.WriteLine(string.Join("", result.Select(i => i.ToString())));
            Console.ReadKey();
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
    }
}