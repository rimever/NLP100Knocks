using Chapter06.Core;
using NUnit.Framework;

namespace Chapter06.Tests
{
    [TestFixture]
    public class AnswerServiceTest
    {
        readonly AnswerService service = new AnswerService();

        [Test]
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
        /// <summary>
        /// <seealso cref="AnswerService.Answer53"/>をテストします。
        /// </summary>
        [Test]
        public void Answer53()
        {
            service.Answer53();
        }
        /// <summary>
        /// <seealso cref="AnswerService.Answer54"/>をテストします。
        /// </summary>
        [Test]
        public void Answer54()
        {
            service.Answer54();
        }
        /// <summary>
        /// <seealso cref="AnswerService.Answer55"/>をテストします。
        /// </summary>
        [Test]
        public void Answer55()
        {
            service.Answer55();
        }
        /// <summary>
        /// <seealso cref="AnswerService.Answer56"/>をテストします。
        /// </summary>
        [Test]
        public void Answer56()
        {
            service.Answer56();
        }
        /// <summary>
        /// <seealso cref="AnswerService.Answer58"/>をテストします。
        /// </summary>
        [Test]
        public void Answer58()
        {
            service.Answer58();
        }
    }
}