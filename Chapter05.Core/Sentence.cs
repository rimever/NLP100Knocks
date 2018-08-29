using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapter05.Core
{
    /// <summary>
    /// 文章
    /// </summary>
    public class Sentence
    {
        public IList<Chunk> Chunks { get; set; } = new List<Chunk>();
    }
}
