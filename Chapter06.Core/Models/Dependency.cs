#region

using System.Xml.Linq;

#endregion

namespace Chapter06.Core.Models
{
    public class Dependency
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="element"></param>
        public Dependency(XElement element)
        {
            DepType = element.Attribute("type").Value;
            Governor = new DependencyChild(element.Element("governor"));
            Dependent = new DependencyChild(element.Element("dependent"));
        }

        public string DepType { get; set; }

        public DependencyChild Dependent { get; set; }

        public DependencyChild Governor { get; set; }
    }

    public class DependencyChild
    {
        public DependencyChild(XElement element)
        {
            Index = int.Parse(element.Attribute("idx").Value);
            Text = element.Value;
        }

        public int Index { get; set; }

        public string Text { get; set; }
    }
}