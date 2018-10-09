#region

using System.Linq;
using Chapter05.Core;
using NUnit.Framework;

#endregion

namespace Chapter05.Tests
{
    [TestFixture]
    public class CabochaAnalyzerTest
    {
        /// <summary>
        /// コンストラクタをテストします。
        /// </summary>
        [Test]
        public void Constructor()
        {
            var analyzer = new CabochaAnalyzer();
            Assert.IsNotNull(analyzer);
        }

        /// <summary>
        /// <seealso cref="CabochaAnalyzer.Execute"/>をテストします。
        /// </summary>
        [Test]
        public void Execute()
        {
            var analyzer = new CabochaAnalyzer();
            analyzer.Execute();
            Assert.IsTrue(analyzer.Sentences.Any());
        }
    }
}