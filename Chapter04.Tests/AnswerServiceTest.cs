using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chapter04.Core;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Chapter04.Tests
{
    /// <summary>
    /// <seealso cref="AnswerService"/>をテストするクラスです。
    /// </summary>
    [TestFixture()]
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
    }
}
