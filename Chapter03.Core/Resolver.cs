using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Chapter03.Core
{
    public class Resolver
    {
        private readonly WikiDocument _wikiDocument = new WikiDocument();
        private readonly string _countryText;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Resolver()
        {
            _countryText = _wikiDocument.GetCountryText("イギリス");
        }

        /// <summary>
        /// Wikipedia記事のJSONファイルを読み込み，「イギリス」に関する記事本文を表示せよ．問題21-29では，ここで抽出した記事本文に対して実行せよ．
        /// </summary>
        public void Answer20()
        {
            Console.WriteLine(_countryText);
        }
        /// <summary>
        /// <summary>
        /// カテゴリ名を含む行を抽出
        /// </summary>
        /// </summary>
        public void Answer21()
        {
            var text = _countryText;            
            foreach (var line in _wikiDocument.EnumerableCountryCategory(text))
            {
                Console.WriteLine(line);
            }
        }
        /// <summary>
        /// 記事のカテゴリ名を（行単位ではなく名前で）抽出せよ．
        /// </summary>
        public void Answer22()
        {
            var text = _countryText;
            foreach (var line in _wikiDocument.EnumerableCountryCategory(text))
            {
                var result = _wikiDocument.RemoveCategoryMarkup(line);
                Console.WriteLine(result);
            }
        }
    }
}
