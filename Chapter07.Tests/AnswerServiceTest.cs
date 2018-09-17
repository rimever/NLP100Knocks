using Chapter07.Core;
using NUnit.Framework;

namespace Chapter07.Tests
{
    /// <summary>
    /// <seealso cref="AnswerService"/>をテストします。
    /// </summary>
    [TestFixture]
    public class AnswerServiceTest
    {
        /// <summary>
        /// <seealso cref="AnswerService.Answer61"/>をテストします。
        /// </summary>
        [Test]
        public void Answer61()
        {
            AnswerService service = new AnswerService(new ConnectionString());
            service.Answer61("Pink Floyd");
        }

        /// <summary>
        /// <seealso cref="AnswerService.Answer62"/>をテストします。
        /// </summary>
        [Test]
        public void Answer62()
        {
            AnswerService service = new AnswerService(new ConnectionString());
            service.Answer62();
        }

        /// <summary>
        /// <seealso cref="AnswerService.Answer63"/>をテストします。
        /// </summary>
        [Test]
        public void Answer63()
        {
            AnswerService service = new AnswerService(new ConnectionString());
            service.Answer63("Pink Floyd");
        }

        /// <summary>
        /// <seealso cref="AnswerService.Answer65"/>をテストします。
        /// </summary>
        [Test]
        public void Answer65()
        {
            AnswerService service = new AnswerService(new ConnectionString());
            service.Answer65("Queen");
        }
        /// <summary>
        /// <seealso cref="AnswerService.Answer66"/>をテストします。
        /// </summary>
        [Test]
        public void Answer66()
        {
            AnswerService service = new AnswerService(new ConnectionString());
            service.Answer66("Japan");
        }
        /// <summary>
        /// <seealso cref="AnswerService.Answer67"/>をテストします。
        /// </summary>
        [Test]
        public void Answer67()
        {
            AnswerService service = new AnswerService(new ConnectionString());
            service.Answer67("オアシス");
        }
        /// <summary>
        /// <seealso cref="AnswerService.Answer68"/>をテストします。
        /// </summary>
        [Test]
        public void Answer68()
        {
            AnswerService service = new AnswerService(new ConnectionString());
            service.Answer68();
        }
    }
}