using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapter04.Core
{

    /// <summary>
    /// 形態素解析器
    /// </summary>
    public class MorphologicalAnalyzer
    {
    private static readonly string FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
    @"..\..\..\Chapter04.Core\neko.txt");

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MorphologicalAnalyzer()
        {
            Debug.Assert(File.Exists(FileName), Path.GetFullPath(FileName));

        }
    }
}
