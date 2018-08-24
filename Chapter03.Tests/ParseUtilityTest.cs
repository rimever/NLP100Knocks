using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Chapter03.Tests
{
    /// <summary>
    /// <see cref="ParseUtility"/>をテストするクラスです。
    /// </summary>
    [TestFixture]
    public class ParseUtilityTest
    {
        [Test]
        public void ParseBetweenBrace()
        {
            IList<string> result = ParseUtility.ParseBetweenBrace("b{{test{{a}}test}}{{testing}}");
            Assert.AreEqual(result.Count, 2);
            Assert.AreEqual(result[0], "test{{a}}test");
            Assert.AreEqual(result[1], "testing");
        }
        [Test]
        public void ParseKeyValue()
        {
            IDictionary<string,string> result = ParseUtility.ParseKeyValue("基礎情報|a = b{{test}}|c = d[[e{{f}}g]]|h = <i = ttt>");
            Assert.AreEqual(result["a"], "b{{test}}");
            Assert.AreEqual(result["c"], "d[[e{{f}}g]]");
            Assert.AreEqual(result["h"], "<i = ttt>");
        }
        [Test]
        public void RemoveStrongMarkup()
        {
            string text = @"''他との区別''|'''強調'''|'''''斜体と強調'''''";
            var actual = ParseUtility.RemoveStrongMarkup(text);
            Assert.AreEqual(actual, "他との区別|強調|斜体と強調");
        }
    }
}
