using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XTool
{
    [Serializable]
    public class RedisConnectionInfo : ConnectionInfo
    {
        [XmlElement]
        public string Version { get; set; }

        [XmlElement]
        public string Endpoint { get; set; }
    }

}
