using System;
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
            System.Diagnostics.Debug.Assert(text == decrypt, "可逆性あり");                           
            Console.WriteLine("Hello World!");
        }

        private static string DecryptCipher(string text)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < text.Length; i++)
            {
                var value = text[i];
                int num = 219 - (int)value;
                if (num >= (int)'a' 
                    && num <= (int)'z')
                {
                    result.Append((char)num);
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
        private static string cipher(string text) {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < text.Length; i++) {
                var value = text[i];
                if (value >= 'a' && value <= 'z')
                {
                    int num = (int)value;
                    result.Append((char)(219 - num));
                }else {
                    result.Append(value);
                } 
            }
            return result.ToString();
        }
    }
}
