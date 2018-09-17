using System;
using Chapter07.Core;
using NUnit.Framework;

namespace Chapter07.Tests
{
    /// <summary>
    /// <seealso cref="JsonAccessor"/>をテストするクラスです。
    /// </summary>
    [TestFixture]
    public class JsonAccessorTest
    {
        /// <summary>
        /// <seealso cref="JsonAccessor.GetRecordsByArtistName"/>をテストします。
        /// </summary>
        /// <param name="name"></param>
        [TestCase("The Beatles", Description = "一つだけ取得される場合")]
        [TestCase("Fonograff", Description = "何も取得されない場合")]
        public void GetRecordsByArtistName(string name)
        {
            JsonAccessor accessor = new JsonAccessor();
            foreach (var record in accessor.GetRecordsByArtistName(name))
            {
                Console.WriteLine(record);
            }
        }
        /// <summary>
        /// <seealso cref="JsonAccessor.GetRecordsByArea"/>をテストします。
        /// </summary>
        /// <param name="name"></param>
        [TestCase("Finland")]
        [TestCase("Dummy")]
        public void GetRecordsByArea(string name)
        {
            JsonAccessor accessor = new JsonAccessor();
            foreach (var record in accessor.GetRecordsByArea(name))
            {
                Console.WriteLine(record);
            }
        }
    }
}