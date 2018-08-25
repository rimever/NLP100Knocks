using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chapter03.Core;
using NUnit.Framework;

namespace Chapter03.Tests
{
    [TestFixture]
    public class WikiDocumentTest
    {
        [Test]
        public void Constructor()
        {
            WikiDocument wikiDocument = new WikiDocument();
            Assert.NotNull(wikiDocument);
        }
        [Test]
        public void GetCountry()
        {
            WikiDocument wikiDocument = new WikiDocument();
            Assert.NotNull(wikiDocument.GetCountryText("イギリス"));
            Assert.Throws<InvalidOperationException>(() => wikiDocument.GetCountryText("存在しない国は例外をスロー"));
        }
    }
}
