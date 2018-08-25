using System;
using System.Collections.Generic;
using System.Linq;

namespace Q06
{
    class MainClass
    {
        /// <summary>
        /// "paraparaparadise"と"paragraph"に含まれる文字bi-gramの集合を，それぞれ, XとYとして求め，XとYの和集合，積集合，差集合を求めよ．さらに，'se'というbi-gramがXおよびYに含まれるかどうかを調べよ．
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        public static void Main(string[] args)
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
    }
}