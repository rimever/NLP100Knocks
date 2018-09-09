using Chapter06.Core;
using NUnit.Framework;

namespace Chapter06.Tests
{
    [TestFixture]
    public class AnswerServiceTest
    {
        readonly AnswerService service = new AnswerService();

        [TestCase]
        public void Constructor()
        {
            Assert.NotNull(service);
        }

        /// <summary>
        /// <seealso cref="AnswerService.Answer50"/>をテストします。
        /// </summary>
        [Test]
        public void Answer50()
        {
            service.Answer50();
        }

        /// <summary>
        /// <seealso cref="AnswerService.Answer51"/>をテストします。
        /// </summary>
        [Test]
        public void Answer51()
        {
            service.Answer51();
        }
        /// <summary>
        /// <seealso cref="AnswerService.Answer52"/>をテストします。
        /// </summary>
        [Test]
        public void Answer52()
        {
            service.Answer52();
        }
    }
}