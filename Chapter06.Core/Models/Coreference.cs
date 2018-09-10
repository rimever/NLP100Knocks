using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Chapter06.Core.Models
{
    public class Coreference
    {
        public List<Mention> Mentions { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="element"></param>
        public Coreference(XElement element)
        {
            Mentions = EnumerableMention(element).ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private IEnumerable<Mention> EnumerableMention(XElement element)
        {
            foreach (var mentionElement in element.Elements("mention"))
            {
                yield return new Mention(mentionElement);
            }
        }
    }
}
