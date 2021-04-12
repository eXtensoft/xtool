using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace JunkHarness
{
    [XmlInclude(typeof(ClassLibraryReference))]
    [XmlInclude(typeof(ProjectReference))]
    [Serializable]
    public abstract class Reference
    {
        [XmlAttribute("name")]
        public string Name { get; set; }


    }
}
