using System;
using System.Text;

namespace Q01
{
    class Program
    {
        /// <summary>
        /// 01. 「パタトクカシーー」
        /// 「パタトクカシーー」という文字列の1,3,5,7文字目を取り出して連結した文字列を得よ
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            string text = "パタトクカシーー";
            int[] index = {1, 3, 5, 7};
            StringBuilder result = new StringBuilder();
            foreach (var item in index)
            {
                result.Append(text[item - 1]);
            }

            Console.WriteLine(result.ToString());
            Console.ReadKey();
        }
    }
}