using Chapter04.Core;
using NUnit.Framework;

namespace Chapter04.Tests
{
    [TestFixture]
    public class WordTest
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        [Test]
        public void Constructor()
        {
            string testData = "なかっ,形容詞,自立,*,*,形容詞・アウオ段,連用タ接続,ない,ナカッ,ナカ";
            Word word = new Word(testData.Split(','));

            Assert.AreEqual(word.Surface, "なかっ");
            Assert.AreEqual(word.Base, "ない");
            Assert.AreEqual(word.Pos, "形容詞");
            Assert.AreEqual(word.Pos1, "自立");
        }
    }
}