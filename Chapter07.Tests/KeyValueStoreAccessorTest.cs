using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chapter07.Core;
using NUnit.Framework;

namespace Chapter07.Tests
{
    [TestFixture]
    public class KeyValueStoreAccessorTest
    {
        [TestCase("The Beatles", Description = "一つだけ取得される場合")]
        [TestCase("Nirvana", Description = "複数取得される場合")]
        [TestCase("Fonograff", Description = "何も取得されない場合")]
        public void GetAreaByArtistName(string name)
        {
            KeyValueStoreAccessor accessor = new KeyValueStoreAccessor();
            foreach (var area in accessor.GetAreaByArtistName(name).ToList())
            {
                Console.WriteLine(area);
            }
        }

        [TestCase("Finland", Description = "一つだけ取得される場合")]
        [TestCase("Dummy", Description = "何も取得されない場合")]
        public void GetArtistNameByArea(string area)
        {
            KeyValueStoreAccessor accessor = new KeyValueStoreAccessor();
            foreach (var name in accessor.GetArtistNameByArea(area).ToList())
            {
                Console.WriteLine(name);
            }
        }
    }
}
