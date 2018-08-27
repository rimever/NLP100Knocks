using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapter04.Core
{
    public class Word
    {
        /// <summary>
        /// 表層形
        /// </summary>
        public string Surface { get; set; }

        /// <summary>
        /// 基本形
        /// </summary>
        public string Base { get; set; }

        /// <summary>
        /// 品詞
        /// </summary>
        public string Pos { get; set; }

        /// <summary>
        /// 品詞細分類
        /// </summary>
        public string Pos1 { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="splits"></param>
        public Word(string[] splits)
        {
            Surface = splits[0];
            Base = splits[7];
            Pos = splits[1];
            Pos1 = splits[2];
        }
    }
}
