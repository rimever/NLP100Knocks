using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapter07.Core
{
    public class AnswerService
    {
        public void Answer61(string name)
        {
            KeyValueStoreAccessor accessor = new KeyValueStoreAccessor();
            foreach (var area in accessor.GetAreaByArtistName(name).ToList())
            {
                Console.WriteLine(area);
            }

        }

        public void Answer62()
        {
            KeyValueStoreAccessor accessor = new KeyValueStoreAccessor();
            foreach (var name in accessor.GetArtistNameByArea("Japan").ToList())
            {
                Console.WriteLine(name);
            }
        }
    }
}
