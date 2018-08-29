using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using CsvHelper;
using NMeCab;

namespace Chapter04.Core
{
    /// <summary>
    /// 形態素解析器
    /// </summary>
    public class MorphologicalAnalyzer
    {
        private static readonly string FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
            @"..\..\..\Chapter04.Core\neko.txt");


        private static readonly string MecabFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
            @"..\..\..\Chapter04.Core\neko.txt.mecab");

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MorphologicalAnalyzer()
        {
            Debug.Assert(File.Exists(FileName), Path.GetFullPath(FileName));
        }

        /// <summary>
        /// 形態素解析を行い、結果を保存します。
        /// </summary>
        public void Execute()
        {
            var allText = File.ReadAllText(FileName);
            var mecabParam = new MeCabParam
            {
                DicDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    @"..\..\..\Chapter04.Core\dic\ipadic")
            };
            MeCabTagger meCabTagger = MeCabTagger.Create(mecabParam);
            using (var writer = new StreamWriter(MecabFileName, false))
            {
                MeCabNode node = meCabTagger.ParseToNode(allText);
                while (node != null)
                {
                    if (node.CharType > 0)
                    {
                        writer.WriteLine(node.Surface + "," + node.Feature);
                    }

                    node = node.Next;
                }

                writer.Flush();
            }
        }

        /// <summary>
        /// 形態素解析から単語情報を列挙します。
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Word> EnumerableWords()
        {
            using (var reader = new CsvReader(new StreamReader(MecabFileName, Encoding.UTF8)))
            {
                while (reader.Read())
                {
                    yield return new Word(reader.Context.Record);
                }
            }
        }

        /// <summary>
        /// 単語ごとに分類します。
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, List<Word>> GetGroupByWord()
        {
            IDictionary<string, List<Word>> result = new Dictionary<string, List<Word>>();
            foreach (var word in EnumerableWords())
            {
                if (!result.ContainsKey(word.Base))
                {
                    result.Add(word.Base, new List<Word>());
                }

                result[word.Base].Add(word);
            }

            return result;
        }
    }
}