using System;
using System.Linq;
using Chapter07.Core;
using NUnit.Framework;

namespace Chapter07.Tests
{
    /// <summary>
    /// <seealso cref="DataSourceJsonReader"/>をテストします。
    /// </summary>
    [TestFixture]
    public class DataSourceJsonReaderTest
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        [Test]
        public void Constructor()
        {
            var reader = new DataSourceJsonReader();
            Assert.NotNull(reader);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void EnumerableJson()
        {
            var reader = new DataSourceJsonReader();
            var list = reader.EnumerableJson().ToList();
            Assert.IsTrue(list.Any());
            foreach (var json in list)
            {
                Console.WriteLine(json);
            }
        }
    }
}
