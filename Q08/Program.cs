using System;
using System.Diagnostics;
using System.Text;

namespace Q08
{
    class MainClass
    {
        /// <summary>
        /// 与えられた文字列の各文字を，以下の仕様で変換する関数cipherを実装せよ．
        /// 英小文字ならば(219 - 文字コード)の文字に置換
        /// その他の文字はそのまま出力
        /// この関数を用い，英語のメッセージを暗号化・復号化せよ．
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        public static void Main(string[] args)
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
        /// <returns>The cipher.</returns>
        /// <param name="text">Text.</param>
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
    }
}