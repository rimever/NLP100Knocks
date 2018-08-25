using System.Collections.Generic;
using Chapter03.Core;
using NUnit.Framework;

namespace Chapter03.Tests
{
    /// <summary>
    /// <see cref="ParseUtility"/>をテストするクラスです。
    /// </summary>
    [TestFixture]
    public class ParseUtilityTest
    {
        /// <summary>
        /// <seealso cref="ParseUtility.RemoveMediaLinkMarkup"/>をテストします。
        /// </summary>
        /// <returns></returns>
        [TestCaseSource(nameof(RemoveMediaLinkMarkupTestCaseDatas))]
        public string RemoveMediaLinkMarkup(string text)
        {
            return ParseUtility.RemoveMediaLinkMarkup(text);
        }

        /// <summary>
        /// <seealso cref="RemoveMediaLinkMarkup"/>のテストケースです。
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<TestCaseData> RemoveMediaLinkMarkupTestCaseDatas()
        {
            yield return new TestCaseData("[[記事名]]")
                .Returns("記事名")
                .SetName($"{nameof(RemoveMediaLinkMarkup)} 内部リンク1");
            yield return new TestCaseData("[[記事名|表示文字]]")
                .Returns("表示文字")
                .SetName($"{nameof(RemoveMediaLinkMarkup)} 内部リンク2");
            yield return new TestCaseData("[[記事名#節名|表示文字]]")
                .Returns("表示文字")
                .SetName($"{nameof(RemoveMediaLinkMarkup)} 内部リンク3");
            yield return new TestCaseData("[[ファイル:Wikipedia-logo-v2-ja.png|thumb|説明文]]")
                .Returns("説明文")
                .SetName($"{nameof(RemoveMediaLinkMarkup)} ファイル");
            yield return new TestCaseData("[http://www.example.org]")
                .Returns("http://www.example.org")
                .SetName($"{nameof(RemoveMediaLinkMarkup)} 外部リンク1");
            yield return new TestCaseData("[http://www.example.org 表示文字]")
                .Returns("表示文字")
                .SetName($"{nameof(RemoveMediaLinkMarkup)} 外部リンク2");
        }

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
            IDictionary<string, string> result =
                ParseUtility.ParseKeyValue("基礎情報|a = b{{test}}|c = d[[e{{f}}g]]|h = <i = ttt>");
            Assert.AreEqual(result["a"], "b{{test}}");
            Assert.AreEqual(result["c"], "d[[e{{f}}g]]");
            Assert.AreEqual(result["h"], "<i = ttt>");
        }

        [Test]
        public void RemoveInnerLink()
        {
            string text = "test[[記事名]] [[記事名|表示文字]] [[記事名#節名|表示文字]]end";
            var actual = ParseUtility.RemoveInnerLinkMarkup(text);
            Assert.AreEqual(actual, "test記事名 表示文字 表示文字end");
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