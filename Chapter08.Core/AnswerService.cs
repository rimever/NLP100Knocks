using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
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
            return Path.Combine(RootDir,"Data","sentiment.txt");
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

73. 学習
72で抽出した素性を用いて，ロジスティック回帰モデルを学習せよ．

74. 予測
73で学習したロジスティック回帰モデルを用い，与えられた文の極性ラベル（正例なら"+1"，負例なら"-1"）と，その予測確率を計算するプログラムを実装せよ．

75. 素性の重み
73で学習したロジスティック回帰モデルの中で，重みの高い素性トップ10と，重みの低い素性トップ10を確認せよ．

76. ラベル付け
学習データに対してロジスティック回帰モデルを適用し，正解のラベル，予測されたラベル，予測確率をタブ区切り形式で出力せよ．

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
        /// 
        /// </summary>
        public async Task Answer72()
        {
            string fileName = Path.Combine(RootDir, "Data", "sentimentCleaning.txt");
            WriteSentimentCleaningText(fileName);
            // STEP 1: Create a model
            string modelPath = Path.Combine(RootDir, "Data","SentimentModel.zip");
            var model = await TrainAsync(fileName,modelPath);

            // STEP2: Test accuracy
            Evaluate(model, fileName);

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
        public static async Task<PredictionModel<SentimentData, SentimentPrediction>> TrainAsync(string trainDataPath,string modelPath)
        {
            // LearningPipeline holds all steps of the learning process: data, transforms, learners.  
            var pipeline = new LearningPipeline();

            // The TextLoader loads a dataset. The schema of the dataset is specified by passing a class containing
            // all the column names and their types.
            pipeline.Add(new TextLoader(trainDataPath).CreateFrom<SentimentData>());

            // TextFeaturizer is a transform that will be used to featurize an input column to format and clean the data.
            pipeline.Add(new TextFeaturizer("Features", "SentimentText"));

            // FastTreeBinaryClassifier is an algorithm that will be used to train the model.
            // It has three hyperparameters for tuning decision tree performance. 
            pipeline.Add(new LogisticRegressionBinaryClassifier());

            Console.WriteLine("=============== Training model ===============");
            // The pipeline is trained on the dataset that has been loaded and transformed.
            var model = pipeline.Train<SentimentData, SentimentPrediction>();

            // Saving the model as a .zip file.
            await model.WriteAsync(modelPath);

            Console.WriteLine("=============== End training ===============");
            Console.WriteLine("The model is saved to {0}", modelPath);

            return model;
        }

        private static void Evaluate(PredictionModel<SentimentData, SentimentPrediction> model,string testDataPath)
        {
            // To evaluate how good the model predicts values, the model is ran against new set
            // of data (test data) that was not involved in training.
            var testData = new TextLoader(testDataPath).CreateFrom<SentimentData>();

            // BinaryClassificationEvaluator performs evaluation for Binary Classification type of ML problems.
            var evaluator = new BinaryClassificationEvaluator();

            Console.WriteLine("=============== Evaluating model ===============");

            var metrics = evaluator.Evaluate(model, testData);
            // BinaryClassificationMetrics contains the overall metrics computed by binary classification evaluators
            // The Accuracy metric gets the accuracy of a classifier which is the proportion 
            //of correct predictions in the test set.

            // The Auc metric gets the area under the ROC curve.
            // The area under the ROC curve is equal to the probability that the classifier ranks
            // a randomly chosen positive instance higher than a randomly chosen negative one
            // (assuming 'positive' ranks higher than 'negative').

            // The F1Score metric gets the classifier's F1 score.
            // The F1 score is the harmonic mean of precision and recall:
            //  2 * precision * recall / (precision + recall).

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

        readonly PorterStemmer _stemmer = new PorterStemmer();

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
    }
    public class SentimentData
    {
        [Column(ordinal: "0", name: "Label")]
        public float Sentiment;
        [Column(ordinal: "1")]
        public string SentimentText;
    }

    public class SentimentPrediction
    {
        [ColumnName("PredictedLabel")]
        public bool Sentiment;
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
