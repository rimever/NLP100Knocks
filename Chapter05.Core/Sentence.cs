using System.Collections.Generic;

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