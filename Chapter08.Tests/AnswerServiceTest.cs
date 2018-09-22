using System;
using Chapter08.Core;
using Xunit;

namespace Chapter08.Tests
{
    /// <summary>
    /// <seealso cref="AnswerService"/>をテストするクラスです。
    /// </summary>
    public class AnswerServiceTest
    {
        /// <summary>
        /// <seealso cref="AnswerService.Answer70"/>をテストします。
        /// </summary>
        [Fact]
        public void Answer70()
        {
            AnswerService answerService = new AnswerService();
            answerService.Answer70();
        }
    }
}
