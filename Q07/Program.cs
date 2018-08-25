using System;

namespace Q07
{
    class MainClass
    {
        /// <summary>
        /// 引数x, y, zを受け取り「x時のyはz」という文字列を返す関数を実装せよ．さらに，x=12, y="気温", z=22.4として，実行結果を確認せよ．
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        public static void Main(string[] args)
        {
            var x = 12;
            var y = "気温";
            var z = 22.4;
            Console.WriteLine(BuildText(x, y, z));
        }

        /// <summary>
        /// 文字列やを返す関数。
        /// </summary>
        /// <returns>The text.</returns>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="z">The z coordinate.</param>
        private static string BuildText(int x, string y, double z)
        {
            return $"{x}時の{y}は{z}";
        }
    }
}