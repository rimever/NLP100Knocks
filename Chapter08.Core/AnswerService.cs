using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;

namespace Chapter08.Core
{
    /// <summary>
    /// 回答を行うクラスです。
    /// </summary>
    public class AnswerService
    {
        private const string RootDir = @"..\..\..\..\Chapter08.Core";

        readonly string _dataSourceDirectoryPath = Path.GetFullPath(Path.Combine(RootDir, @"rt-polaritydata\rt-polaritydata"));

        /// <summary>
        /// 70. データの入手・整形
        /// 文に関する極性分析の正解データを用い，以下の要領で正解データ（sentiment.txt）を作成せよ．
        /// 
        /// rt-polarity.posの各行の先頭に"+1 "という文字列を追加する（極性ラベル"+1"とスペースに続けて肯定的な文の内容が続く）
        /// rt-polarity.negの各行の先頭に"-1 "という文字列を追加する（極性ラベル"-1"とスペースに続けて否定的な文の内容が続く）
        /// 上述1と2の内容を結合（concatenate）し，行をランダムに並び替える
        /// sentiment.txtを作成したら，正例（肯定的な文）の数と負例（否定的な文）の数を確認せよ．
        /// </summary>
        public void Answer70()
        {

            if (!Directory.Exists(_dataSourceDirectoryPath))
            {
                throw new DirectoryNotFoundException();
            }

            string negativePath = Path.Combine(_dataSourceDirectoryPath, "rt-polarity.neg");
            string positivePath = Path.Combine(_dataSourceDirectoryPath, "rt-polarity.pos");
            var list = EnumerableLine(negativePath).Select(s => new SentimentSentence() {Point = -1, Sentence = s})
                .Union(EnumerableLine(positivePath)
                    .Select(s => new SentimentSentence() {Point = 1, Sentence = s})).OrderBy(i => Guid.NewGuid()).ToList();
            string resultPath = Path.Combine(RootDir,"Data","sentiment.txt");
            using (var writer = new StreamWriter(resultPath))
            {
                foreach (var sentimentSentence in list)
                {
                    writer.WriteLine($"{sentimentSentence.Point},{sentimentSentence.Sentence}");
                }
                writer.Flush();
            }
        }
        

        public class SentimentSentence
        {
            public int Point { get; set; }
            public string Sentence { get; set; }
        }

        private static IEnumerable<string> EnumerableLine(string fileName)
        {
            using (var reader = new StreamReader(fileName))
            {
                while (reader.Peek() >= 0)
                {
                    yield return reader.ReadLine();

                }
            }
        }
        /// <summary>
        /// 71. ストップワード
        /// 英語のストップワードのリスト（ストップリスト）を適当に作成せよ．さらに，引数に与えられた単語（文字列）がストップリストに含まれている場合は真，それ以外は偽を返す関数を実装せよ．さらに，その関数に対するテストを記述せよ
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public bool Answer71(string word)
        {
            var stopWords = new[]
            {
                "the",
                "of",
                "to",
                "a",
                "and",
                "is",
                "in",
                "that",
                "on",
                "which",
                "with",
                "for",
                "or",
                "The",
                "are",
                "from",
                "as",
                "were",
                "by",
                "can",
                "be",
                "into",
                "was",
                "when",
                "been",
                "than",
                "there",
                "at",
                "an",
                "this",
            };
            return stopWords.Contains(word.ToLower());
        }
    }
}
