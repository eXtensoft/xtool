using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace XTool.Inference
{
    [Serializable]
    public class SqlCommand
    {

        [XmlAttribute("command")]
        public string Text { get; set; }

        [XmlAttribute("type")]
        public string CommandType { get; set; }

        [XmlAttribute("title")]
        public string Title { get; set; }

        [XmlAttribute("cnt")]
        public int Count { get; set; }

        [XmlElement("Parameter")]
        public List<Parameter> Parameters;

        public override string ToString()
        {
            return (!String.IsNullOrEmpty(Title)) ? Title : Text;
        }

    }
}
