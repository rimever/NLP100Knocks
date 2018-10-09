#region

using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

#endregion

namespace Chapter06.Core.Models
{
    public class Coreference
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="element"></param>
        public Coreference(XElement element)
        {
            Mentions = EnumerableMention(element).ToList();
        }

        public List<Mention> Mentions { get; set; }

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