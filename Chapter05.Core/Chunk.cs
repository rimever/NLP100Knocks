using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapter05.Core
{
    /// <summary>
    /// 文節を表すクラス
    /// </summary>
    public class Chunk
    {
        /// <summary>
        /// 形態素リスト
        /// </summary>
        public IList<Morph> Morphs { get; set; } = new List<Morph>();
        /// <summary>
        /// 係先文節インデックス番号
        /// </summary>
        public int Dst { get; set; }

        /// <summary>
        /// 係元文節インデックス番号のリスト
        /// </summary>
        public IList<int> Srcs { get; set; } = new List<int>();
    }
}
