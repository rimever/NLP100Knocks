using System.Collections.Generic;
using System.Linq;

namespace Chapter05.Core
{
    /// <summary>
    /// 文章
    /// </summary>
    public class Sentence
    {
        public IList<Chunk> Chunks { get; set; } = new List<Chunk>();

        public static IList<int> GetWordChainList(Sentence sentence, int start)
        {
            var list = new List<int> {start};
            while (sentence.Chunks[list.Last()].Dst > 0)
            {
                list.Add(sentence.Chunks[list.Last()].Dst);
            }

            return list;
        }
    }
}