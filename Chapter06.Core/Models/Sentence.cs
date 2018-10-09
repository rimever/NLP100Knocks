#region

using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

#endregion

namespace Chapter06.Core.Models
{
    /// <summary>
    /// 文章
    /// </summary>
    public class Sentence
    {
        public List<Word> Words;

        /// <summary>
        /// 
        /// </summary>
        public Sentence(XElement element)
        {
            Words = EnumerableWords(element).ToList();

            foreach (var dependency in element.Elements("dependencies"))
            {
                string type = dependency.Attribute("type").Value;
                var list = new List<Dependency>();
                foreach (var dep in dependency.Elements("dep"))
                {
                    list.Add(new Dependency(dep));
                }

                DependencyDictionary.Add(type, list);
            }
        }


        public string Text
        {
            get
            {
                string text = string.Empty;
                foreach (var item in Words.Select((value, index) => new {value, index}))
                {
                    text += item.value.Value;
                    if (item.index < Words.Count - 2)
                    {
                        text += " ";
                    }
                }

                return text;
            }
        }

        public Dictionary<string, List<Dependency>> DependencyDictionary { get; set; } =
            new Dictionary<string, List<Dependency>>();

        private IEnumerable<Word> EnumerableWords(XElement element)
        {
            foreach (var wordElement in element.Elements("tokens").Elements("token"))
            {
                yield return new Word(wordElement);
            }
        }
    }
}