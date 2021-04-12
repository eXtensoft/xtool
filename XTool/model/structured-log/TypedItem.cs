using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XTool.Cyclops
{
    public class TypedItem
    {

        [XmlIgnore]
        public string Datatype
        {
            get { return Value.GetType().Name; }
        }

        [DataMember]
        [XmlAttribute("domain")]
        public string Domain { get; set; }
        [XmlAttribute("selected")]
        public bool IsSelected { get; set; }
        [DataMember]
        [XmlAttribute("key")]
        public string Key { get; set; }
        [DataMember]
        [XmlElement("Value")]
        public object Value { get; set; }

        [XmlIgnore]
        public int Count
        {
            get { return (int)Value; }
        }
    }
}
