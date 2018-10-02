using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Models;
using Microsoft.ML.Runtime.Api;
using Microsoft.ML.Trainers;
using Microsoft.ML.Transforms;
using Poseidon.Analysis;

namespace Chapter08.Core
{
    /// <summary>
    /// 回答を行うクラスです。
    /// </summary>
    public class AnswerService
    {
        private const string RootDir = @"..\..\..\..\Chapter08.Core";

        readonly string _dataSourceDirectoryPath =
            Path.GetFullPath(Path.Combine(RootDir, @"rt-polaritydata\rt-polaritydata"));

        readonly PorterStemmer _stemmer = new PorterStemmer();

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
            var list = EnumerableLine(negativePath).Select(s => new SentimentSentence {Point = -1, Sentence = s})
                .Union(EnumerableLine(positivePath)
                    .Select(s => new SentimentSentence {Point = 1, Sentence = s})).OrderBy(i => Guid.NewGuid())
                .ToList();
            string resultPath = GetSentimentTextFilePath();
            using (var writer = new StreamWriter(resultPath))
            {
                foreach (var sentimentSentence in list)
                {
                    writer.WriteLine($"{sentimentSentence.Point},{sentimentSentence.Sentence}");
                }

                writer.Flush();
            }
        }

        private static string GetSentimentTextFilePath()
        {
            return Path.Combine(RootDir, "Data", "sentiment.txt");
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
                "it",
                ".", ",", @""""
            };
            return stopWords.Contains(word.ToLower());
        }

        /*






77. 正解率の計測
76の出力を受け取り，予測の正解率，正例に関する適合率，再現率，F1スコアを求めるプログラムを作成せよ．

78. 5分割交差検定
76-77の実験では，学習に用いた事例を評価にも用いたため，正当な評価とは言えない．すなわち，分類器が訓練事例を丸暗記する際の性能を評価しており，モデルの汎化性能を測定していない．そこで，5分割交差検定により，極性分類の正解率，適合率，再現率，F1スコアを求めよ．

79. 適合率-再現率グラフの描画
ロジスティック回帰モデルの分類の閾値を変化させることで，適合率-再現率グラフを描画せよ．
*/
        /// <summary>
        /// 72. 素性抽出
        /// 極性分析に有用そうな素性を各自で設計し，学習データから素性を抽出せよ．
        /// 素性としては，レビューからストップワードを除去し，各単語をステミング処理したものが最低限のベースラインとなるであろう．
        /// 73. 学習
        /// 72で抽出した素性を用いて，ロジスティック回帰モデルを学習せよ．
        /// </summary>
        public async Task Answer72()
        {
            string fileName = Path.Combine(RootDir, "Data", "sentimentCleaning.txt");
            WriteSentimentCleaningText(fileName);
            // STEP 1: Create a model
            string modelPath = Path.Combine(RootDir, "Data", "Answer72.zip");
            var model = await TrainAnswer72Async(fileName, modelPath);

            // STEP2: Test accuracy
            EvaluateAnswer72(model, fileName);

            // STEP 3: Make a prediction
            var predictions = model.Predict(TestSentimentData.Sentiments);

            var sentimentsAndPredictions =
                TestSentimentData.Sentiments.Zip(predictions, (sentiment, prediction) => (sentiment, prediction));
            foreach (var item in sentimentsAndPredictions)
            {
                Console.WriteLine(
                    $"Sentiment: {item.sentiment.SentimentText} | Prediction: {(item.prediction.Sentiment ? "Positive" : "Negative")} sentiment");
            }
        }

        /// <summary>
        /// 74. 予測
        /// 73で学習したロジスティック回帰モデルを用い，与えられた文の極性ラベル（正例なら"+1"，負例なら"-1"）と，その予測確率を計算するプログラムを実装せよ．
        /// </summary>
        /// <returns></returns>
        public async Task Answer74()
        {
            string fileName = Path.Combine(RootDir, "Data", "sentimentCleaning.txt");
            WriteSentimentCleaningText(fileName);
            string modelPath = Path.Combine(RootDir, "Data", "Answer73.zip");
            var model = await TrainAnswer74Async(fileName, modelPath);
            EvaluateAnswer74(model, fileName);
            var predictions = model.Predict(TestSentimentData.Sentiments);
            var sentimentsAndPredictions =
                TestSentimentData.Sentiments.Zip(predictions, (sentiment, prediction) => (sentiment, prediction));
            foreach (var item in sentimentsAndPredictions)
            {
                Console.WriteLine(
                    $"Sentiment: {item.sentiment.SentimentText} | Prediction: {item.prediction.Sentiment}");
            }
        }

        /// <summary>
        /// 76. ラベル付け
        /// 学習データに対してロジスティック回帰モデルを適用し，正解のラベル，予測されたラベル，予測確率をタブ区切り形式で出力せよ．
        /// </summary>
        /// <returns></returns>
        public async Task Answer76()
        {
            string fileName = Path.Combine(RootDir, "Data", "sentimentCleaning.txt");
            WriteSentimentCleaningText(fileName);
            string modelPath = Path.Combine(RootDir, "Data", "Answer73.zip");
            var model = await TrainAnswer74Async(fileName, modelPath);
            EvaluateAnswer74(model, fileName);
            var sentiments = EnumerableSentimentData().ToList();
            var predictions = model.Predict(sentiments);
            var sentimentsAndPredictions =
                sentiments.Zip(predictions, (sentiment, prediction) => (sentiment, prediction)).ToList();
            using (var writer = new StreamWriter(Path.Combine(RootDir, "Data", "Answer76.txt")))
            {
                writer.WriteLine("正解のラベル\t予測されたラベル\t予測確率\t文章");
                foreach (var item in sentimentsAndPredictions.OrderBy(s => s.prediction.Sentiment))
                {
                    writer.WriteLine(
                        $"{item.sentiment.Sentiment}\t{(item.prediction.Sentiment > 0 ? 1 : -1)}\t{item.prediction.Sentiment}\t{item.sentiment.SentimentText}");
                }

                writer.Flush();
            }
        }

        /// <summary>
        /// 75. 素性の重み
        ///73で学習したロジスティック回帰モデルの中で，重みの高い素性トップ10と，重みの低い素性トップ10を確認せよ．
        /// </summary>
        /// <returns></returns>
        public async Task Answer75()
        {
            string fileName = Path.Combine(RootDir, "Data", "sentimentCleaning.txt");
            WriteSentimentCleaningText(fileName);
            string modelPath = Path.Combine(RootDir, "Data", "Answer73.zip");
            var model = await TrainAnswer74Async(fileName, modelPath);
            EvaluateAnswer74(model, fileName);
            var sentiments = EnumerableSentimentData().ToList();
            var predictions = model.Predict(sentiments);
            var sentimentsAndPredictions =
                sentiments.Zip(predictions, (sentiment, prediction) => (sentiment, prediction)).ToList();
            Console.WriteLine("=== Top10 ===");
            foreach (var item in sentimentsAndPredictions.OrderByDescending(s => s.prediction.Sentiment).Take(10))
            {
                Console.WriteLine($"{item.prediction.Sentiment}:{item.sentiment.SentimentText}");
            }

            Console.WriteLine("=== Worst10 ===");
            foreach (var item in sentimentsAndPredictions.OrderBy(s => s.prediction.Sentiment).Take(10))
            {
                Console.WriteLine($"{item.prediction.Sentiment}:{item.sentiment.SentimentText}");
            }
        }

        private static IEnumerable<SentimentData> EnumerableSentimentData()
        {
            using (var reader = new StreamReader(GetSentimentTextFilePath()))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    int commaIndex = line.IndexOf(",", StringComparison.Ordinal);
                    string value = line.Substring(0, commaIndex);
                    string text = line.Substring(commaIndex + 1);
                    yield return new SentimentData
                    {
                        SentimentText = text,
                        Sentiment = float.Parse(value)
                    };
                }
            }
        }


        public static async Task<PredictionModel<SentimentData, SentimentPrediction>> TrainAnswer72Async(
            string trainDataPath, string modelPath)
        {
            var pipeline = new LearningPipeline();
            pipeline.Add(new TextLoader(trainDataPath).CreateFrom<SentimentData>());
            pipeline.Add(new TextFeaturizer("Features", "SentimentText"));
            pipeline.Add(new LogisticRegressionBinaryClassifier());

            Console.WriteLine("=============== Training model ===============");
            var model = pipeline.Train<SentimentData, SentimentPrediction>();

            await model.WriteAsync(modelPath);

            Console.WriteLine("=============== End training ===============");
            Console.WriteLine("The model is saved to {0}", modelPath);

            return model;
        }

        public static async Task<PredictionModel<SentimentData, SentimentWeightPrediction>> TrainAnswer74Async(
            string trainDataPath, string modelPath)
        {
            var pipeline = new LearningPipeline();

            pipeline.Add(new TextLoader(trainDataPath).CreateFrom<SentimentData>());

            pipeline.Add(new TextFeaturizer("Features", "SentimentText"));

            pipeline.Add(new LogisticRegressionBinaryClassifier());

            Console.WriteLine("=============== Training model ===============");
            var model = pipeline.Train<SentimentData, SentimentWeightPrediction>();
            await model.WriteAsync(modelPath);
            Console.WriteLine("=============== End training ===============");
            Console.WriteLine("The model is saved to {0}", modelPath);

            return model;
        }

        private static void EvaluateAnswer72(PredictionModel<SentimentData, SentimentPrediction> model,
            string testDataPath)
        {
            var testData = new TextLoader(testDataPath).CreateFrom<SentimentData>();

            var evaluator = new BinaryClassificationEvaluator();

            Console.WriteLine("=============== Evaluating model ===============");

            var metrics = evaluator.Evaluate(model, testData);
            Console.WriteLine($"Accuracy: {metrics.Accuracy:P2}");
            Console.WriteLine($"Auc: {metrics.Auc:P2}");
            Console.WriteLine($"F1Score: {metrics.F1Score:P2}");
            Console.WriteLine("=============== End evaluating ===============");
            Console.WriteLine();
        }

        private static void EvaluateAnswer74(PredictionModel<SentimentData, SentimentWeightPrediction> model,
            string testDataPath)
        {
            var testData = new TextLoader(testDataPath).CreateFrom<SentimentData>();

            var evaluator = new BinaryClassificationEvaluator();

            Console.WriteLine("=============== Evaluating model ===============");

            var metrics = evaluator.Evaluate(model, testData);

            Console.WriteLine($"Accuracy: {metrics.Accuracy:P2}");
            Console.WriteLine($"Auc: {metrics.Auc:P2}");
            Console.WriteLine($"F1Score: {metrics.F1Score:P2}");
            Console.WriteLine("=============== End evaluating ===============");
            Console.WriteLine();
        }

        private void WriteSentimentCleaningText(string fileName)
        {
            using (var writer = new StreamWriter(fileName))
            {
                writer.WriteLine("Sentiment\tSentimentText");
                using (var reader = new StreamReader(GetSentimentTextFilePath()))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        writer.WriteLine(CleaningSentimentLine(line));
                    }
                }

                writer.Flush();
            }
        }

        /// <summary>
        /// 感情分析情報を前処理します。
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private string CleaningSentimentLine(string line)
        {
            int commaIndex = line.IndexOf(",", StringComparison.Ordinal);
            string value = line.Substring(0, commaIndex);
            string text = line.Substring(commaIndex + 1);
            return $"{value}\t{CleanText(text)}";
        }

        /// <summary>
        /// 文章を解析しやすい形に前処理します。
        /// </summary>
        /// <remarks>
        /// 単語に分割
        /// ストップワードを除外
        /// ステミング
        /// スペース区切りで単語をつないで戻す。
        /// </remarks>
        /// <param name="text"></param>
        /// <returns></returns>
        private string CleanText(string text)
        {
            string[] words = text.Split(" ");
            var cleanWords = EnumerableValidWords(words).Select(word => _stemmer.StemWord(word));
            return string.Join(" ", cleanWords.ToArray());
        }

        /// <summary>
        /// ストップワードを除外した有効な文字列を取得します。
        /// </summary>
        /// <param name="words"></param>
        /// <returns></returns>
        private IEnumerable<string> EnumerableValidWords(string[] words)
        {
            foreach (var word in words)
            {
                if (Answer71(word))
                {
                    continue;
                }

                yield return word;
            }
        }


        public class SentimentSentence
        {
            public int Point { get; set; }
            public string Sentence { get; set; }
        }
    }

    public class SentimentWeightPrediction
    {
        [ColumnName("Score")] public float Sentiment;
    }

    public class SentimentData
    {
        [Column(ordinal: "0", name: "Label")] public float Sentiment;

        [Column(ordinal: "1")] public string SentimentText;
    }

    public class SentimentPrediction
    {
        [ColumnName("PredictedLabel")] public bool Sentiment;
    }

    internal class TestSentimentData
    {
        internal static readonly IEnumerable<SentimentData> Sentiments = new[]
        {
            new SentimentData
            {
                SentimentText = "Contoso's 11 is a wonderful experience",
                Sentiment = 0
            },
            new SentimentData
            {
                SentimentText = "The acting in this movie is very bad",
                Sentiment = 0
            },
            new SentimentData
            {
                SentimentText = "Joe versus the Volcano Coffee Company is a great film.",
                Sentiment = 0
            }
        };
    }
}