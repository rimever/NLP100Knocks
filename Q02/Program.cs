using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Q02
{
    class Program
    {
        /// <summary>
        /// 「パトカー」＋「タクシー」の文字を先頭から交互に連結して文字列「パタトクカシーー」を得よ．
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            string result = MergeText("パトカー", "タクシー");
            Debug.Assert(result == "パタトクカシーー", $"{result} == パタトクカシーー");
            Console.WriteLine(result);
            Console.ReadKey();
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
    }
}