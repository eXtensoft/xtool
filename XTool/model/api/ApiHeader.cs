using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XTool
{
    [DataContract]
    [Serializable]
    public class ApiHeader
    {
        [DataMember]
        [XmlAttribute("type")]
        public string Type { get; set; }
        [DataMember]
        [XmlAttribute("scope")]
        public string Scope { get; set; }
        [DataMember]
        [XmlElement]
        public string Name { get; set; }
        [DataMember]
        [XmlElement]
        public string Value { get; set; }

        [IgnoreDataMember]
        [XmlIgnore]
        public bool IsTemplate
        {
            get
            {
                return AuthType.Equals(AuthorizationRuntimeType.Template);
            }
        }

        [DataMember]
        [XmlAttribute("tag")]
        public string Tag { get; set; }

        [DataMember]
        [XmlElement]
        public AuthorizationRuntimeType AuthType { get; set; }


    }
}
