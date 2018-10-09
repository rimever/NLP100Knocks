#region

using System;

#endregion

namespace Chapter03.Core
{
    public class AnswerService
    {
        private readonly string _countryText;
        private readonly WikiDocument _wikiDocument = new WikiDocument();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AnswerService()
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

        /// <summary>
        /// 記事中に含まれるセクション名とそのレベル（例えば"== セクション名 =="なら1）を表示せよ．
        /// </summary>
        public void Answer23()
        {
            var text = _countryText;
            foreach (var line in _wikiDocument.EnumerableCountryLines(text))
            {
                var section = _wikiDocument.ParseSection(line);
                if (section != null)
                {
                    Console.WriteLine($"{section.Level} {section.Name}");
                }
            }
        }

        /// <summary>
        /// 記事から参照されているメディアファイルをすべて抜き出せ．
        /// </summary>
        public void Answer24()
        {
            var text = _countryText;
            foreach (var line in _wikiDocument.EnumerableCountryFile(text))
            {
                Console.WriteLine(line);
            }
        }

        /// <summary>
        /// 記事中に含まれる「基礎情報」テンプレートのフィールド名と値を抽出し，辞書オブジェクトとして格納せよ．
        /// </summary>
        public void Answer25()
        {
            var text = _countryText;
            string basicInfomation = WikiDocument.GetBasicInfomation(text);
            var hash = WikiDocument.ParseKeyValue(basicInfomation);
            foreach (var item in hash)
            {
                Console.WriteLine($"{item.Key} = {item.Value}");
            }
        }

        /// <summary>
        /// 25の処理時に，テンプレートの値からMediaWikiの強調マークアップ（弱い強調，強調，強い強調のすべて）を除去してテキストに変換せよ
        /// </summary>
        public void Answer26()
        {
            var text = _countryText;
            string basicInfomation = WikiDocument.GetBasicInfomation(text);
            var hash = WikiDocument.ParseKeyValue(basicInfomation);
            foreach (var item in hash)
            {
                string value = WikiDocument.RemoveStrongMarkup(item.Value);
                Console.WriteLine($"{item.Key} = {value}");
            }
        }

        /// <summary>
        /// 26の処理に加えて，テンプレートの値からMediaWikiの内部リンクマークアップを除去し，テキストに変換せよ
        /// </summary>
        public void Answer27()
        {
            var text = _countryText;
            string basicInfomation = WikiDocument.GetBasicInfomation(text);
            var hash = WikiDocument.ParseKeyValue(basicInfomation);
            foreach (var item in hash)
            {
                string value = WikiDocument.RemoveStrongMarkup(item.Value);
                value = WikiDocument.RemoveInnerLinkMarkup(value);
                Console.WriteLine($"{item.Key} = {value}");
            }
        }

        /// <summary>
        /// 27の処理に加えて，テンプレートの値からMediaWikiマークアップを可能な限り除去し，国の基本情報を整形せよ．
        /// </summary>
        public void Answer28()
        {
            var text = _countryText;
            string basicInfomation = WikiDocument.GetBasicInfomation(text);
            var hash = WikiDocument.ParseKeyValue(basicInfomation);
            foreach (var item in hash)
            {
                string value = WikiDocument.RemoveStrongMarkup(item.Value);
                value = WikiDocument.RemoveMediaLinkMarkup(value);
                Console.WriteLine($"{item.Key} = {value}");
            }
        }

        /// <summary>
        /// テンプレートの内容を利用し，国旗画像のURLを取得せよ．（ヒント: MediaWiki APIのimageinfoを呼び出して，ファイル参照をURLに変換すればよい）
        /// </summary>
        public void Answer29()
        {
            var text = _countryText;
            string basicInfomation = WikiDocument.GetBasicInfomation(text);
            var hash = WikiDocument.ParseKeyValue(basicInfomation);
            foreach (var item in hash)
            {
                if (item.Key == "国旗画像")
                {
                    WikiDocument.JumpImageFilePage(item.Value);
                }
            }
        }
    }
}