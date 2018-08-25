using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            System.Diagnostics.Debug.Assert(File.Exists(FileName), Path.GetFullPath(FileName));
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
            foreach (var line in countryText.Split(new[] { '\n' }, StringSplitOptions.None))
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
    }
}
