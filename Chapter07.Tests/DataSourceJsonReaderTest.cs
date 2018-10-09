#region

using System;
using System.Collections.Generic;
using System.Linq;
using Chapter07.Core;
using NUnit.Framework;

#endregion

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
        public void EnumerableDictionary()
        {
            var reader = new DataSourceJsonReader();
            var list = reader.EnumerableDictionary().Take(1).ToList();
            Assert.IsTrue(list.Any());
            foreach (var json in list)
            {
                List<string> array = new List<string>();
                foreach (var keyPair in json)
                {
                    var value = keyPair.Value.ToString().Replace(@"""", @"`""").Replace("'", "`'");
                    array.Add($@"""{keyPair.Key}"" => ""{value}""");
                }

                Console.WriteLine($"'{string.Join(",", array.ToArray())}'");
            }
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