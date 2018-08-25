using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace Chapter03.Core
{
    public class WikiDocument
    {
        private const string CategoryPrefix = "Category:";

        private static readonly string FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
            @"..\..\..\Chapter03.Core\jawiki-country.json");

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public WikiDocument()
        {
            Debug.Assert(File.Exists(FileName), Path.GetFullPath(FileName));
        }

        /// <summary>
        /// 指定名の国の記事を取得する。
        /// </summary>
        /// <param name="countryName"></param>
        /// <returns></returns>
        public string GetCountryText(string countryName)
        {
            foreach (var line in File.ReadLines(FileName))
            {
                JObject json = JObject.Parse(line);
                var title = json.GetValue("title").ToString();
                if (title == countryName)
                {
                    return json.GetValue("text").ToString();
                }
            }

            throw new InvalidOperationException($"{countryName}の記事は見つかりませんでした。");
        }

        /// <summary>
        /// 国情報を行単位で取得します。
        /// </summary>
        /// <param name="countryText"></param>
        /// <returns></returns>
        public IEnumerable<string> EnumerableCountryLines(string countryText)
        {
            foreach (var line in countryText.Split(new[] {'\n'}, StringSplitOptions.None))
            {
                yield return line;
            }
        }

        /// <summary>
        /// 国のカテゴリ情報を取得します。
        /// </summary>
        /// <param name="countryText"></param>
        /// <returns></returns>
        public IEnumerable<string> EnumerableCountryCategory(string countryText)
        {
            foreach (var line in EnumerableCountryLines(countryText))
            {
                if (line.Contains(CategoryPrefix))
                {
                    yield return line;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public string RemoveCategoryMarkup(string line)
        {
            string result =
                line.Substring(line.IndexOf(CategoryPrefix, StringComparison.InvariantCulture) + CategoryPrefix.Length);
            result = result.Substring(0, result.Length - "]]".Length);
            // イギリス|* という表記のケースがある
            const string separator = "|";
            if (result.Contains(separator))
            {
                result = result.Substring(0, result.IndexOf(separator, StringComparison.InvariantCulture));
            }

            return result;
        }

        /// <summary>
        /// セクションをパースします。
        /// </summary>
        /// <param name="line"></param>
        /// <returns>セクションでなければnullを返します</returns>
        public Section ParseSection(string line)
        {
            var equalPattern = "==+";
            var notEqualPattern = @"[^=]+";
            var pattern = $@"{equalPattern}{notEqualPattern}{equalPattern}";
            var match = Regex.Match(line, pattern);
            if (match.Success)
            {
                var sectionName = Regex.Match(match.Value, notEqualPattern).Value.Trim();
                var level = Regex.Match(match.Value, equalPattern).Value.Length;
                return new Section
                {
                    Level = level,
                    Name = sectionName
                };
            }

            return null;
        }

        public IEnumerable EnumerableCountryFile(string text)
        {
            const string mediaFilePrefix = "ファイル:";
            foreach (var line in EnumerableCountryLines(text))
            {
                if (!line.Contains(mediaFilePrefix))
                {
                    continue;
                }

                int startIndex = line.IndexOf(mediaFilePrefix, 0, StringComparison.InvariantCulture) +
                                 mediaFilePrefix.Length;
                var fileName = line.Substring(startIndex,
                    line.IndexOf("|", startIndex, StringComparison.InvariantCulture) - startIndex);
                yield return fileName;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string GetBasicInfomation(string text)
        {
            var basicInfomation = String.Empty;
            const string basicPrefix = "基礎情報";
            var blocks = ParseBetweenBrace(text);
            foreach (var line in blocks)
            {
                if (line.StartsWith(basicPrefix))
                {
                    basicInfomation = line;
                    break;
                }
            }

            return basicInfomation;
        }

        /// <summary>
        /// 指定されたファイルを取得します。
        /// </summary>
        /// <param name="fileName"></param>
        public static void JumpImageFilePage(string fileName)
        {
            string url =
                $"https://commons.wikimedia.org/w/api.php?action=query&format=json&titles=File:{fileName}&prop=imageinfo&&iiprop=url";
            Encoding encoding = Encoding.UTF8;

            WebRequest webRequest = WebRequest.Create(url);
            WebResponse webResponse = webRequest.GetResponse();

            using (Stream stream = webResponse.GetResponseStream())
            {
                Debug.Assert(stream != null, nameof(stream) + " != null");
                using (StreamReader streamReader = new StreamReader(stream, encoding))
                {
                    string html = streamReader.ReadToEnd();
                    JObject json = JObject.Parse(html);
                    var imageUrl = json["query"]["pages"].Children().First().Children().First()["imageinfo"].Children()
                        .First()["url"];
                    Process.Start(imageUrl.ToString());
                }
            }
        }

        private const string StartDoubleBrace = "{{";
        private const string EndDoubleBrace = "}}";
        private const string StartDoubleBracket = "[[";
        private const string EndDoubleBracket = "]]";
        private const string StartSingleBracket = "[";
        private const string EndSingleBracket = "]";
        private const string Separator = "|";

        public static IList<string> ParseBetweenBrace(string text)
        {
            var results = new List<string>();
            int bracketCount = 0;
            int firstBracketIndex = -1;
            for (int nowIndex = 0; nowIndex < text.Length; nowIndex++)
            {
                if (text.Substring(nowIndex).StartsWith(StartDoubleBrace))
                {
                    if (bracketCount == 0)
                    {
                        firstBracketIndex = nowIndex + StartDoubleBrace.Length;
                    }

                    bracketCount++;
                    continue;
                }

                if (text.Substring(nowIndex).StartsWith(EndDoubleBrace))
                {
                    bracketCount--;
                    if (bracketCount == 0)
                    {
                        string between = text.Substring(firstBracketIndex, nowIndex - firstBracketIndex);
                        results.Add(between);
                    }
                }
            }

            return results;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static IDictionary<string, string> ParseKeyValue(string text)
        {
            var results = new Dictionary<string, string>();
            int braceCount = 0;
            int firstSeparatorIndex = -1;
            int bracketCount = 0;


            void StoreDictionary(string keyAndValue)
            {
                // a = <ref name = test>というケースがあるのでsplitしない
                var index = keyAndValue.IndexOf("=", StringComparison.Ordinal);
                results.Add(keyAndValue.Substring(0, index).Trim(),
                    keyAndValue.Substring(index + 1, keyAndValue.Length - index - 1).Trim());
            }

            for (int nowIndex = 0; nowIndex < text.Length; nowIndex++)
            {
                string rest = text.Substring(nowIndex);
                if (rest.StartsWith(Separator))
                {
                    if (braceCount == 0
                        && bracketCount == 0)
                    {
                        if (firstSeparatorIndex != -1)
                        {
                            string between = text.Substring(firstSeparatorIndex, nowIndex - firstSeparatorIndex);
                            StoreDictionary(between);
                        }

                        firstSeparatorIndex = nowIndex + Separator.Length;
                    }
                }
                else if (rest.StartsWith(StartDoubleBrace))
                {
                    braceCount++;
                }
                else if (rest.StartsWith(EndDoubleBrace))
                {
                    braceCount--;
                }
                else if (rest.StartsWith(StartDoubleBracket))
                {
                    bracketCount++;
                }
                else if (rest.StartsWith(EndDoubleBracket))
                {
                    bracketCount--;
                }
            }

            StoreDictionary(text.Substring(firstSeparatorIndex));
            return results;
        }

        /// <summary>
        /// 強調表現を表すマークアップを取り除いて、テキストに変換
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RemoveStrongMarkup(string text)
        {
            return text.Replace("'", String.Empty);
        }

        /// <summary>
        /// 内部リンクのマークアップを取り除く
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RemoveInnerLinkMarkup(string text)
        {
            StringBuilder stringBuilder = new StringBuilder();
            int bracketStartIndex = -1;
            for (int i = 0; i < text.Length; i++)
            {
                string rest = text.Substring(i);
                if (rest.StartsWith(StartDoubleBracket))
                {
                    bracketStartIndex = i;
                    i += StartDoubleBracket.Length - 1;
                }
                else if (rest.StartsWith(EndDoubleBracket))
                {
                    string value = text.Substring(bracketStartIndex + StartDoubleBracket.Length,
                        i - bracketStartIndex - StartDoubleBracket.Length);
                    int sepratorIndex = value.IndexOf(Separator, StringComparison.Ordinal);
                    if (sepratorIndex != -1)
                    {
                        value = value.Substring(sepratorIndex + 1);
                    }

                    stringBuilder.Append(value);
                    bracketStartIndex = -1;
                    i += EndDoubleBracket.Length - 1;
                }
                else if (bracketStartIndex == -1)
                {
                    stringBuilder.Append(text[i]);
                }
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// メディアリンクを取り除く
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RemoveMediaLinkMarkup(string text)
        {
            StringBuilder stringBuilder = new StringBuilder();
            int bracketDoubleStartIndex = -1;
            int bracketSingleStartIndex = -1;
            for (int i = 0; i < text.Length; i++)
            {
                string rest = text.Substring(i);
                if (rest.StartsWith(StartDoubleBracket))
                {
                    bracketDoubleStartIndex = i;
                    i += StartDoubleBracket.Length - 1;
                }
                else if (rest.StartsWith(EndDoubleBracket))
                {
                    string value = text.Substring(bracketDoubleStartIndex + StartDoubleBracket.Length,
                        i - bracketDoubleStartIndex - StartDoubleBracket.Length);
                    int sepratorIndex = value.LastIndexOf(Separator, StringComparison.Ordinal);
                    if (sepratorIndex != -1)
                    {
                        value = value.Substring(sepratorIndex + 1);
                    }

                    stringBuilder.Append(value);
                    bracketDoubleStartIndex = -1;
                    i += EndDoubleBracket.Length - 1;
                }
                else if (rest.StartsWith(StartSingleBracket))
                {
                    bracketSingleStartIndex = i;
                    i += StartSingleBracket.Length - 1;
                }
                else if (rest.StartsWith(EndSingleBracket))
                {
                    string value = text.Substring(bracketSingleStartIndex + StartSingleBracket.Length,
                        i - bracketSingleStartIndex - StartSingleBracket.Length);
                    int spaceIndex = value.IndexOf(" ", StringComparison.Ordinal);
                    if (spaceIndex != -1)
                    {
                        value = value.Substring(spaceIndex + 1);
                    }

                    stringBuilder.Append(value);
                    bracketSingleStartIndex = -1;
                    i += EndSingleBracket.Length - 1;
                }
                else if (bracketDoubleStartIndex == -1
                         && bracketSingleStartIndex == -1)
                {
                    stringBuilder.Append(text[i]);
                }
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// セクション情報
        /// </summary>
        public class Section
        {
            /// <summary>
            /// セクション名
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// セクションレベル
            /// </summary>
            public int Level { get; set; }
        }
    }
}