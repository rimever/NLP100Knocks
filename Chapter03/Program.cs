using System;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Chapter03
{
    /// <summary>
    /// 3章の回答を行うクラス
    /// </summary>
    class MainClass
    {
        public static void Main(string[] args)
        {
            string path = "../../jawiki-country.json";
            System.Diagnostics.Debug.Assert(File.Exists(path));
            var json = GetCountry(path, "イギリス");
            Console.WriteLine("Quesution20");
            Question20(json);
            Console.WriteLine("Quesution21");
            Question21(json);
            Console.WriteLine("Quesution22");
            Question22(json);
        }
        /// <summary>
        /// 記事のカテゴリ名を（行単位ではなく名前で）抽出せよ．
        /// </summary>
        /// <param name="json">Json.</param>
        private static void Question22(JObject json)
        {
            var text = json.GetValue("text").ToString();
            const string prefix = "Category:";
            foreach (var line in text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None))
            {
                if (line.Contains(prefix))
                {
                    string result = line.Substring(line.IndexOf(prefix, StringComparison.InvariantCulture) + prefix.Length);
                    result = result.Substring(0, result.Length - "]]".Length);
                    // イギリス|* という表記のケースがある
                    const string separator = "|";
                    if (result.Contains(separator)) {
                        result = result.Substring(0, result.IndexOf(separator,StringComparison.InvariantCulture));
                    }
                    Console.WriteLine(result);
                }
            }

        }

        /// <summary>
        /// Wikipedia記事のJSONファイルを読み込み，「イギリス」に関する記事本文を表示せよ．問題21-29では，ここで抽出した記事本文に対して実行せよ．
        /// </summary>
        /// <param name="json">Json.</param>
        private static void Question20(JObject json) {
            var text = json.GetValue("text").ToString();
            Console.WriteLine(text);
        }
        /// <summary>
        /// カテゴリ名を含む行を抽出
        /// </summary>
        /// <param name="json">Json.</param>
        private static void Question21(JObject json)
        {
            var text = json.GetValue("text").ToString();
            foreach (var line in text.Split(new string[] {Environment.NewLine}, StringSplitOptions.None))
            {
                if (line.Contains("Category:")) {
                    Console.WriteLine(line);
                }
            }
        }

        private static JObject GetCountry(string path , string countryName) {
            foreach (var line in File.ReadLines(path))
            {
                JObject json = JObject.Parse(line);
                var title = json.GetValue("title").ToString();
                if (title == countryName)
                {
                    return json;
                }
            }
            throw new InvalidOperationException($"{countryName}の記事は見つかりませんでした。");
        }

    }
}
