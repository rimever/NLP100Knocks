using System;
using System.Collections;
using System.Collections.Generic;
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
        /// <summary>
        /// <seealso cref="AnswerService.Answer71"/>をテストします。
        /// </summary>
        [Theory]
        [ClassData(typeof(Answer71TestData))]
        public void Answer71(string word, bool expected)
        {
            AnswerService answerService = new AnswerService();
            Assert.Equal(answerService.Answer71(word), expected);
        }
    }
    /// <summary>
    /// <seealso cref="AnswerServiceTest.Answer71"/>のテストケースです。
    /// </summary>
    public class Answer71TestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "as", true };
            yield return new object[] { "The", true };
            yield return new object[] { "machine", false };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
