﻿using System.Threading;
using Chapter05.Core;
using NUnit.Framework;

namespace Chapter05.Tests
{
    [TestFixture]
    public class AnswerServiceTest
    {
        private readonly AnswerService _answerService = new AnswerService();

        /// <summary>
        /// <seealso cref="AnswerService.Answer40"/>を検証します。
        /// </summary>
        [Test]
        public void Answer40()
        {
            _answerService.Answer40();
        }

        /// <summary>
        /// <seealso cref="AnswerService.Answer41"/>を検証します。
        /// </summary>
        [Test]
        public void Answer41()
        {
            _answerService.Answer41();
        }
        /// <summary>
        /// <seealso cref="AnswerService.Answer42"/>を検証します。
        /// </summary>
        [Test]
        public void Answer42()
        {
            _answerService.Answer42();
        }
        /// <summary>
        /// <seealso cref="AnswerService.Answer43"/>を検証します。
        /// </summary>
        [Test]
        public void Answer43()
        {
            _answerService.Answer43();
        }
        /// <summary>
        /// <seealso cref="AnswerService.Answer45"/>を検証します。
        /// </summary>
        [Test]
        public void Answer45()
        {
            _answerService.Answer45();
        }

    }
}