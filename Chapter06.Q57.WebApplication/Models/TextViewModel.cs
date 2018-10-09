#region

using System.Collections.Generic;
using Chapter06.Core.Models;

#endregion

namespace Chapter06.Q57.WebApplication.Models
{
    public class TextViewModel
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="sentences"></param>
        public TextViewModel(IList<Sentence> sentences)
        {
            Sentences = sentences;
        }

        /// <summary>
        /// 文章一覧
        /// </summary>
        public IList<Sentence> Sentences { get; set; }
    }
}