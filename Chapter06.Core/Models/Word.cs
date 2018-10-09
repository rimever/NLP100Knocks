#region

using System.Xml.Linq;

#endregion

namespace Chapter06.Core.Models
{
    public class Word
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="element"></param>
        public Word(XElement element)
        {
            Id = int.Parse(element.Attribute("id").Value);
            Value = element.Element("word")?.Value;
            Lemma = element.Element("lemma")?.Value;
            POS = element.Element("POS")?.Value;
        }

        public int Id { get; set; }

        public string Value { get; set; }

        public string Lemma { get; set; }

        public string POS { get; set; }
    }
}