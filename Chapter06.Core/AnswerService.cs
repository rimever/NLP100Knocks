using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Chapter06.Core.Models;
using Poseidon.Analysis;

namespace Chapter06.Core
{
    /// <summary>
    /// 第6章: 英語テキストの処理
    /// 英語のテキスト（nlp.txt）に対して，以下の処理を実行せよ．
    /// </summary>
    public class AnswerService
    {
        private readonly string _stanfordNLPFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
            @"..\..\..\Chapter06.Core\nlp.txt.xml");

        private readonly string _text;

        private readonly string _textFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
            @"..\..\..\Chapter06.Core\nlp.txt");

        public AnswerService()
        {
            Debug.Assert(File.Exists(_textFilePath), _textFilePath);
            Debug.Assert(File.Exists(_stanfordNLPFilePath), _stanfordNLPFilePath);
            _text = File.ReadAllText(_textFilePath);
            _text = _text.Replace("\n", " ");
        }

        /// <summary>
        /// 50. 文区切り
        /// (. or ; or : or ? or !) → 空白文字 → 英大文字というパターンを文の区切りと見なし，入力された文書を1行1文の形式で出力せよ．
        /// </summary>
        public void Answer50()
        {
            foreach (var sentence in SplitSentence())
            {
                Console.WriteLine(sentence);
            }
        }

        /// <summary>
        /// 文章単位で分割します。
        /// </summary>
        /// <returns></returns>
        private IEnumerable<string> SplitSentence()
        {
            int index = 0;
            const string pattern = @"[\.;!\?]\s[A-Z]";
            foreach (Match match in Regex.Matches(_text, pattern))
            {
                yield return _text.Substring(index, match.Index - index + 1);
                index = match.Index + 2;
            }

            yield return _text.Substring(index);
        }

        /// <summary>
        /// 51. 単語の切り出し
        /// 空白を単語の区切りとみなし，50の出力を入力として受け取り，1行1単語の形式で出力せよ．ただし，文の終端では空行を出力せよ．
        /// 
        /// </summary>
        public void Answer51()
        {
            foreach (var sentence in SplitSentence())
            {
                foreach (var word in SplitWords(sentence))
                {
                    Console.WriteLine(word);
                }

                Console.WriteLine();
            }
        }

        /// <summary>
        /// 単語に分割します。
        /// </summary>
        /// <param name="sentence"></param>
        /// <returns></returns>
        private IEnumerable<string> SplitWords(string sentence)
        {
            string[] words = sentence.Split(' ');

            foreach (var item in words.Select((value, index) => new {value, index}))
            {
                string word = item.value;
                string[] signs = {",", @"""", ".", "'", "?", "!"};
                foreach (var sign in signs)
                {
                    word = word.Replace(sign, string.Empty);
                }

                yield return word;
            }
        }

        /// <summary>
        /// 52. ステミング
        /// 51の出力を入力として受け取り，Porterのステミングアルゴリズムを適用し，単語と語幹をタブ区切り形式で出力せよ． Pythonでは，Porterのステミングアルゴリズムの実装としてstemmingモジュールを利用するとよい．
        /// </summary>
        public void Answer52()
        {
            PorterStemmer porterStemmer = new PorterStemmer();
            foreach (var sentence in SplitSentence())
            {
                foreach (var word in SplitWords(sentence))
                {
                    var stem = porterStemmer.StemWord(word);
                    Console.WriteLine($"{word}\t{stem}");
                }

                Console.WriteLine();
            }
        }

        /// <summary>
        /// 53. Tokenization
        /// Stanford Core NLPを用い，入力テキストの解析結果をXML形式で得よ．また，このXMLファイルを読み込み，入力テキストを1行1単語の形式で出力せよ．
        /// </summary>
        public void Answer53()
        {
            var xml = XDocument.Load(_stanfordNLPFilePath);
            foreach (var element in xml.Root.Elements("document").Elements("sentences").Elements("sentence")
                .Elements("tokens").Elements("token").Elements("word"))
            {
                Console.WriteLine(element.Value);
            }
        }

        /// <summary>
        /// 54. 品詞タグ付け
        /// Stanford Core NLPの解析結果XMLを読み込み，単語，レンマ，品詞をタブ区切り形式で出力せよ．
        /// </summary>
        public void Answer54()
        {
            var xml = XDocument.Load(_stanfordNLPFilePath);
            foreach (var element in xml.Root.Elements("document").Elements("sentences").Elements("sentence"))
            {
                var sentence = new Sentence(element);
                foreach (var word in sentence.Words)
                {
                    Console.WriteLine($"{word.Value}\t{word.Lemma}\t{word.POS}");
                }
            }
        }

        private static IEnumerable<Coreference> EnumerableCoreference(XDocument xml)
        {
            foreach (var element in xml.Root.Elements("document").Elements("coreference").Elements("coreference"))
            {
                yield return new Coreference(element);
            }
        }

        /// <summary>
        /// 55. 固有表現抽出
        /// 入力文中の人名をすべて抜き出せ．
        /// </summary>
        public void Answer55()
        {
            var xml = XDocument.Load(_stanfordNLPFilePath);
            foreach (var element in xml.Root.Elements("document").Elements("sentences").Elements("sentence")
                .Elements("tokens").Elements("token"))
            {
                var ner = element.Element("NER")?.Value;
                if (ner != "PERSON")
                {
                    continue;
                }

                var word = element.Element("word")?.Value;
                Console.WriteLine($"{word}");
            }
        }

        /// <summary>
        /// 56. 共参照解析
        /// Stanford Core NLPの共参照解析の結果に基づき，文中の参照表現（mention）を代表参照表現（representative mention）に置換せよ．ただし，置換するときは，「代表参照表現（参照表現）」のように，元の参照表現が分かるように配慮せよ．
        /// </summary>
        public void Answer56()
        {
            var xml = XDocument.Load(_stanfordNLPFilePath);
            var coreferences = EnumerableCoreference(xml).ToList();
            foreach (var sentenceElement in xml.Root.Elements("document").Elements("sentences").Elements("sentence"))
            {
                var sentenceId = int.Parse(sentenceElement.Attribute("id").Value);
                var tokens = sentenceElement.Elements("tokens").Elements("token").ToList();
                var reference = coreferences.Where(c => c.Mentions.Any(m => m.SentenceId == sentenceId)).ToList();
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 1; i <= tokens.Count; i++)
                {
                    if (i < tokens.Count
                        && !string.IsNullOrEmpty(stringBuilder.ToString()))
                    {
                        stringBuilder.Append(" ");
                    }

                    stringBuilder.Append(tokens[i - 1].Element("word").Value);
                    var coreference = reference.SingleOrDefault(r =>
                        r.Mentions.Any(m => m.SentenceId == sentenceId && m.EndId - 1 == i && !m.Representative));
                    if (coreference != null)
                    {
                        string representationText = coreference.Mentions.Single(s => s.Representative).Text;
                        stringBuilder.Append($"({representationText})");
                    }
                }

                Console.WriteLine(stringBuilder.ToString());
            }
        }

        /// <summary>
        /// 57. 係り受け解析
        /// Stanford Core NLPの係り受け解析の結果（collapsed-dependencies）を有向グラフとして可視化せよ．可視化には，係り受け木をDOT言語に変換し，Graphvizを用いるとよい．また，Pythonから有向グラフを直接的に可視化するには，pydotを使うとよい．
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Sentence> Answer57()
        {
            var xml = XDocument.Load(_stanfordNLPFilePath);
            foreach (var element in xml.Root.Elements("document").Elements("sentences").Elements("sentence"))
            {
                yield return new Sentence(element);
            }
        }

        /// <summary>
        /// 58. タプルの抽出
        /// Stanford Core NLPの係り受け解析の結果（collapsed-dependencies）に基づき，「主語 述語 目的語」の組をタブ区切り形式で出力せよ．ただし，主語，述語，目的語の定義は以下を参考にせよ．
        /// 述語: nsubj関係とdobj関係の子（dependant）を持つ単語
        /// 主語: 述語からnsubj関係にある子（dependent）
        /// 目的語: 述語からdobj関係にある子（dependent）
        /// </summary>
        public void Answer58()
        {
            var xml = XDocument.Load(_stanfordNLPFilePath);
            foreach (var element in xml.Root.Elements("document").Elements("sentences").Elements("sentence"))
            {
                var sentence = new Sentence(element);
                var dependencies = sentence.DependencyDictionary["collapsed-dependencies"];
                var words = sentence.Words.Where(w =>
                    dependencies.Any(d => d.DepType == "nsubj" && d.Governor.Index == w.Id) &&
                    dependencies.Any(d => d.DepType == "dobj" && d.Governor.Index == w.Id));
                foreach (var item in words)
                {
                    var nsubj = dependencies.First(d => d.DepType == "nsubj" && d.Governor.Index == item.Id);
                    var dobj = dependencies.First(d => d.DepType == "dobj" && d.Governor.Index == item.Id);
                    Console.WriteLine(
                        $"{sentence.Words.FirstOrDefault(w => w.Id == nsubj.Dependent.Index).Value}\t{item.Value}\t{sentence.Words.FirstOrDefault(w => w.Id == dobj.Dependent.Index).Value}");
                }
            }
        }

        /// <summary>
        /// 59. S式の解析
        /// Stanford Core NLPの句構造解析の結果（S式）を読み込み，文中のすべての名詞句（NP）を表示せよ．入れ子になっている名詞句もすべて表示すること．
        /// </summary>
        public void Answer59()
        {
            var xml = XDocument.Load(_stanfordNLPFilePath);
            foreach (var element in xml.Root.Elements("document").Elements("sentences").Elements("sentence"))
            {
                var sentence = new Sentence(element);
                foreach (var word in sentence.Words)
                {
                    if (word.POS != "NN" && word.POS != "NNP")
                    {
                        continue;
                    }

                    Console.WriteLine(word.Value);
                }
            }
        }
    }
}