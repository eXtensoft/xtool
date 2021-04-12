using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace JunkHarness
{
    [Serializable]
    public class ClassLibraryReference : Reference
    {
        [XmlText]
        public string Filepath { get; set; }
    }
}
