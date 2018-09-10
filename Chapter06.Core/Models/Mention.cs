using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Chapter06.Core.Models
{

    public class Mention
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="mentionElement"></param>
        public Mention(XElement mentionElement)
        {
            Representative = mentionElement.Attributes().Any(a => a.Name == "representative");
            SentenceId = int.Parse(mentionElement.Element("sentence").Value);
            StartId = int.Parse(mentionElement.Element("start").Value);
            EndId = int.Parse(mentionElement.Element("end").Value);
            HeadId = int.Parse(mentionElement.Element("head").Value);
            Text = mentionElement.Element("text").Value;
        }

        public int SentenceId { get; set; }

        public int StartId { get; set; }

        public int EndId { get; set; }

        public int HeadId { get; set; }

        public string Text { get; set; }

        public bool Representative { get; set; }
    }
}
