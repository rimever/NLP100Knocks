using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chapter06.Core.Models;

namespace Chapter06.Q57.WebApplication.Models
{
    public class TextViewModel
    {
        /// <summary>
        /// 文章一覧
        /// </summary>
        public IList<Sentence> Sentences { get; set; }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="sentences"></param>
        public TextViewModel(IList<Sentence> sentences)
        {
            Sentences = sentences;
        }
    }
}
