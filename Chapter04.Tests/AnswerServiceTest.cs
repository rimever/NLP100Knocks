using Chapter04.Core;
using NUnit.Framework;

namespace Chapter04.Tests
{
    /// <summary>
    /// <seealso cref="AnswerService"/>をテストするクラスです。
    /// </summary>
    [TestFixture]
    public class AnswerServiceTest
    {
        /// <summary>
        /// <seealso cref="AnswerService.Answer30"/>を検証します。
        /// </summary>
        [Test]
        public void Answer30()
        {
            AnswerService answerService = new AnswerService();
            answerService.Answer30();
        }

        /// <summary>
        /// <seealso cref="AnswerService.Answer31"/>を検証します。
        /// </summary>
        [Test]
        public void Answer31()
        {
            AnswerService answerService = new AnswerService();
            answerService.Answer31();
        }
    }
}