using Chapter07.Core;
using NUnit.Framework;

namespace Chapter07.Tests
{
    [TestFixture]
    public class AnswerServiceTest
    {
        [Test]
        public void Answer61()
        {
            AnswerService service = new AnswerService();
            service.Answer61("Pink Floyd");
        }
        [Test]
        public void Answer62()
        {
            AnswerService service = new AnswerService();
            service.Answer62();
        }
    }
}
