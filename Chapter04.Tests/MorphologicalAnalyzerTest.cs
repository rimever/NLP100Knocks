#region

using System.Linq;
using Chapter04.Core;
using NUnit.Framework;

#endregion

namespace Chapter04.Tests
{
    /// <summary>
    /// <seealso cref="MorphologicalAnalyzer"/>をテストするクラスです。
    /// </summary>
    [TestFixture]
    public class MorphologicalAnalyzerTest
    {
        /// <summary>
        /// コンストラクタを検証します。
        /// </summary>
        [Test]
        public void Constructor()
        {
            var morphologicalAnalyzer = new MorphologicalAnalyzer();
            Assert.NotNull(morphologicalAnalyzer);
        }

        /// <summary>
        /// <seealso cref="MorphologicalAnalyzer.EnumerableWords"/>をテストします。        
        /// </summary>
        [Test]
        public void EnumerableWords()
        {
            var morphologicalAnalyzer = new MorphologicalAnalyzer();
            morphologicalAnalyzer.Execute();
            Assert.IsTrue(morphologicalAnalyzer.EnumerableWords().Any());
        }

        /// <summary>
        /// <seealso cref="MorphologicalAnalyzer.Execute"/>をテストします。
        /// </summary>
        [Test]
        public void Execute()
        {
            var morphologicalAnalyzer = new MorphologicalAnalyzer();
            morphologicalAnalyzer.Execute();
        }
    }
}