using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace Chapter03
{
    /// <summary>
    /// 3章の回答を行うクラス
    /// </summary>
    class MainClass
    {
        private const string EndBracket = "}}";

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
            Console.WriteLine("Quesution23");
            Question23(json);
            Console.WriteLine("Quesution24");
            Question24(json);
            Console.WriteLine("Quesution25");
            Question25(json);
            Console.WriteLine("Quesution26");
            Question26(json);
            Console.ReadKey();
        }
        /// <summary>
        /// 25の処理時に，テンプレートの値からMediaWikiの強調マークアップ（弱い強調，強調，強い強調のすべて）を除去してテキストに変換せよ
        /// </summary>
        /// <param name="json"></param>
        private static void Question26(JObject json)
        {
            var text = json.GetValue("text").ToString();
            var basicInfomation = string.Empty;
            const string basicPrefix = "基礎情報";
            var blocks = ParseUtility.ParseBetweenBrace(text);
            foreach (var line in blocks)
            {
                if (line.StartsWith(basicPrefix))
                {
                    basicInfomation = line;
                    break;
                }
            }

            var hash = ParseUtility.ParseKeyValue(basicInfomation);
            foreach (var item in hash)
            {
                string value = ParseUtility.RemoveStrongMarkup(item.Value);
                Console.WriteLine($"{item.Key} = {value}");
            }

        }

        /// <summary>
        /// 記事中に含まれる「基礎情報」テンプレートのフィールド名と値を抽出し，辞書オブジェクトとして格納せよ．
        /// </summary>
        /// <param name="json">Json.</param>
        private static void Question25(JObject json)
        {
            var text = json.GetValue("text").ToString();
            var basicInfomation = string.Empty;
            const string basicPrefix = "基礎情報";
            var blocks = ParseUtility.ParseBetweenBrace(text);
            foreach (var line in blocks)
            {
                if (line.StartsWith(basicPrefix))
                {
                    basicInfomation = line;
                    break;
                }
            }

            var hash = ParseUtility.ParseKeyValue(basicInfomation);
            foreach (var item in hash)
            {
                Console.WriteLine($"{item.Key} = {item.Value}");
            }
        }

        /// <summary>
        /// 記事から参照されているメディアファイルをすべて抜き出せ．
        /// </summary>
        /// <param name="json">Json.</param>
        private static void Question24(JObject json)
        {
            var text = json.GetValue("text").ToString();
            foreach (var line in text.Split(new[] { '\n' }, StringSplitOptions.None))
            {
                const string mediaFilePrefix = "ファイル:";
                if (line.Contains(mediaFilePrefix))
                {
                    int startIndex = line.IndexOf(mediaFilePrefix, 0, StringComparison.InvariantCulture) +
                                     mediaFilePrefix.Length;
                    Console.WriteLine(line.Substring(startIndex,
                        line.IndexOf("|", startIndex, StringComparison.InvariantCulture) - startIndex));
                }
            }
        }

        /// <summary>
        /// 記事中に含まれるセクション名とそのレベル（例えば"== セクション名 =="なら1）を表示せよ．
        /// </summary>
        /// <param name="json">Json.</param>
        private static void Question23(JObject json)
        {
            var text = json.GetValue("text").ToString();
            foreach (var line in text.Split(new[] { '\n' }, StringSplitOptions.None))
            {
                var equalPattern = "==+";
                var notEqualPattern = @"[^=]+";
                var pattern = $@"{equalPattern}{notEqualPattern}{equalPattern}";
                var match = Regex.Match(line, pattern);
                if (match.Success)
                {
                    var sectionName = Regex.Match(match.Value, notEqualPattern).Value.Trim();
                    var level = Regex.Match(match.Value, equalPattern).Value.Length;
                    Console.WriteLine($"{level} {sectionName}");
                }
            }
        }

        /// <summary>
        /// 記事のカテゴリ名を（行単位ではなく名前で）抽出せよ．
        /// </summary>
        /// <param name="json">Json.</param>
        private static void Question22(JObject json)
        {
            var text = json.GetValue("text").ToString();
            const string prefix = "Category:";
            foreach (var line in text.Split(new[] { '\n' }, StringSplitOptions.None))
            {
                if (line.Contains(prefix))
                {
                    string result =
                        line.Substring(line.IndexOf(prefix, StringComparison.InvariantCulture) + prefix.Length);
                    result = result.Substring(0, result.Length - "]]".Length);
                    // イギリス|* という表記のケースがある
                    const string separator = "|";
                    if (result.Contains(separator))
                    {
                        result = result.Substring(0, result.IndexOf(separator, StringComparison.InvariantCulture));
                    }

                    Console.WriteLine(result);
                }
            }

        }

        /// <summary>
        /// Wikipedia記事のJSONファイルを読み込み，「イギリス」に関する記事本文を表示せよ．問題21-29では，ここで抽出した記事本文に対して実行せよ．
        /// </summary>
        /// <param name="json">Json.</param>
        private static void Question20(JObject json)
        {
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
            foreach (var line in text.Split(new[] { '\n' }, StringSplitOptions.None))
            {
                if (line.Contains("Category:"))
                {
                    Console.WriteLine(line);
                }
            }
        }

        private static JObject GetCountry(string path, string countryName)
        {
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

