using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using XTool.Inference;

namespace XTool
{
    public class ApiParameterSet
    {
        [XmlElement]
        public string Key { get; set; }
        [XmlElement]
        public ZoneTypeOption Zone { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("cnt")]
        public int Count { get; set; }

        [XmlAttribute("runAt")]
        public DateTime RunAt { get; set; }

        [XmlElement]
        public string UrlSuffix { get; set; }


        public List<ApiParameter> Parameters { get; set; }

        public List<ApiHeader> Headers { get; set; }

        [XmlAttribute("hasBody")]
        public bool HasBody { get; set; }

        [XmlElement]
        public string Body { get; set; }

    }
}
