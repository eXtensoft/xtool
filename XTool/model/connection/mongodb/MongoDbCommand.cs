using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XTool
{
    [Serializable]
    public class MongoDbCommand
    {
        [XmlAttribute("collection")]
        public string Collection { get; set; }

        [XmlAttribute("title")]
        public string Title { get; set; }

        [XmlAttribute("cmd")]
        public string Command { get; set; }

        [XmlAttribute("cnt")]
        public int Count { get; set; }

        [XmlText]
        public string Text { get; set; }

        public override string ToString()
        {
            return (!String.IsNullOrEmpty(Title)) ? Title : Text;
        }

    }
}
