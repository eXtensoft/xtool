using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XTool
{
    [Serializable]
    public class ApiParameter
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("datatype")]
        public string Datatype { get; set; }


        [XmlAttribute("source")]
        public string Source { get; set; }

        public object Value { get; set; }

        public override string ToString()
        {
            return String.Format("{0}({1})", Name, Datatype);
        }
    }
}
