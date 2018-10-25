using System.IO;
using Chapter09.Core;
using NUnit.Framework;

namespace Chapter09.Tests
{
    /// <summary>
    ///     <see cref="AnswerService" />
    /// </summary>
    [TestFixture]
    public class AnswerServiceTest
    {
        [Test]
        public void DataSourceFilePath()
        {
            var answerService = new AnswerService();
            Assert.IsTrue(File.Exists(answerService._dataSourceFilePath));
        }
    }
}