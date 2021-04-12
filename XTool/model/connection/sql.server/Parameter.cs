using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Data;

namespace XTool.Inference
{
    [Serializable]
    public class Parameter
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("dataType")]
        public DbType DataType { get; set; }

        [XmlAttribute("mode")]
        public string Mode { get; set; }

        [XmlElement]
        public object Value { get; set; }

        [XmlElement]
        public string Target { get; set; }

    }
}
