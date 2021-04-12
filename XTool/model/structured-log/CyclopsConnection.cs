using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XTool.Cyclops
{
    [Serializable]
    public class CyclopsConnection
    {
        [XmlAttribute("title")]
        public string Title { get; set; }

        [XmlAttribute("error")]
        
        public LogSchema ErrorSchema { get; set; }
        [XmlAttribute("api")]
        public LogSchema ApiSchema { get; set; }
        [XmlAttribute("session")]
        public LogSchema SessionSchema { get; set; }
        [XmlAttribute("cn")]
        public string ConnectionString { get; set; }

    }
}
