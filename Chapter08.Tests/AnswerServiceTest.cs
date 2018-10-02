using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chapter08.Core;
using NUnit.Framework;

namespace Chapter08.Tests
{
    /// <summary>
    /// <seealso cref="AnswerService"/>���e�X�g����N���X�ł��B
    /// </summary>
    [TestFixture]
    public class AnswerServiceTest
    {
        /// <summary>
        /// <seealso cref="AnswerService.Answer70"/>���e�X�g���܂��B
        /// </summary>
        [Test]
        public void Answer70()
        {
            AnswerService answerService = new AnswerService();
            answerService.Answer70();
        }
        /// <summary>
        /// <seealso cref="AnswerService.Answer71"/>���e�X�g���܂��B
        /// </summary>
       [TestCaseSource(nameof(Answer71TestCaseSource))]
        public bool Answer71(string word)
        {
            AnswerService answerService = new AnswerService();
            return answerService.Answer71(word);
        }
        /// <summary>
        /// <seealso cref="Answer71"/>�̃e�X�g�P�[�X�ł��B
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<TestCaseData> Answer71TestCaseSource()
        {
            yield return new TestCaseData("as").Returns(true);
            yield return new TestCaseData("The").Returns(true);
            yield return new TestCaseData("machine").Returns(false);
        }
        /// <summary>
        /// <seealso cref="AnswerService.Answer72"/>���e�X�g���܂��B
        /// </summary>
        [Test]
        public async Task Answer72()
        {
            AnswerService answerService = new AnswerService();
            await answerService.Answer72();
        }
        /// <summary>
        /// <seealso cref="AnswerService.Answer74"/>���e�X�g���܂��B
        /// </summary>
        [Test]
        public async Task Answer74()
        {
            AnswerService answerService = new AnswerService();
            await answerService.Answer74();
        }
        /// <summary>
        /// <seealso cref="AnswerService.Answer75"/>���e�X�g���܂��B
        /// </summary>
        [Test]
        public async Task Answer75()
        {
            AnswerService answerService = new AnswerService();
            await answerService.Answer75();
        }
    }
}
