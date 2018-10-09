#region

using System;
using Chapter07.Core;
using NUnit.Framework;

#endregion

namespace Chapter07.Tests
{
    /// <summary>
    /// <seealso cref="KeyValueStoreAccessor"/>をテストするクラスです。
    /// </summary>
    [TestFixture]
    public class KeyValueStoreAccessorTest
    {
        /// <summary>
        /// <seealso cref="KeyValueStoreAccessor.GetAreaByArtistName"/>をテストします。
        /// </summary>
        /// <param name="name"></param>
        [TestCase("The Beatles", Description = "一つだけ取得される場合")]
        [TestCase("Nirvana", Description = "複数取得される場合")]
        [TestCase("Fonograff", Description = "何も取得されない場合")]
        public void GetAreaByArtistName(string name)
        {
            KeyValueStoreAccessor accessor = new KeyValueStoreAccessor();
            foreach (var area in accessor.GetAreaByArtistName(name))
            {
                Console.WriteLine(area);
            }
        }

        /// <summary>
        /// <seealso cref="KeyValueStoreAccessor.GetArtistNameByArea"/>をテストします。
        /// </summary>
        /// <param name="area"></param>
        [TestCase("Finland")]
        [TestCase("Dummy")]
        public void GetArtistNameByArea(string area)
        {
            KeyValueStoreAccessor accessor = new KeyValueStoreAccessor();
            foreach (var name in accessor.GetArtistNameByArea(area))
            {
                Console.WriteLine(name);
            }
        }

        /// <summary>
        /// <seealso cref="KeyValueStoreAccessor.GetTagsByArtistName"/>をテストします。
        /// </summary>
        /// <param name="name"></param>
        [TestCase("The Beatles", Description = "一つだけ取得される場合")]
        [TestCase("Fonograff", Description = "何も取得されない場合")]
        public void GetTagsByArtistName(string name)
        {
            KeyValueStoreAccessor accessor = new KeyValueStoreAccessor();
            foreach (var area in accessor.GetTagsByArtistName(name))
            {
                Console.WriteLine(area);
            }
        }
    }
}