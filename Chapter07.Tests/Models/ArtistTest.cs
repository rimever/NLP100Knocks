using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chapter07.Core;
using Chapter07.Core.Models;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Chapter07.Tests.Models
{
    [TestFixture]
    public class ArtistTest
    {
        /// <summary>
        /// <seealso cref="Artist"/>のコンストラクタをテストします。
        /// </summary>
        /// <returns></returns>
        [TestCaseSource(nameof(ConstructorTestCaseDatas))]
        public void Constructor(JObject jObject)
        {
            var artist = new Artist(jObject);
            Assert.NotNull(artist);
        }

        /// <summary>
        /// <seealso cref="Constructor"/>のテストケースです。
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<TestCaseData> ConstructorTestCaseDatas()
        {
            var reader = new DataSourceJsonReader();
            foreach (var json in reader.EnumerableJson().Take(100))
            {
                yield return new TestCaseData(json);
            }
        }
    }
}
