using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XTool
{
    [KnownType(typeof(MongoDbCommandTemplate))]
    [KnownType(typeof(MySqlCommandTemplate))]
    [KnownType(typeof(SqlServerCommandTemplate))]
    [XmlInclude(typeof(MongoDbCommandTemplate))]
    [XmlInclude(typeof(MySqlCommandTemplate))]
    [XmlInclude(typeof(SqlServerCommandTemplate))]
    [DataContract]
    [Serializable]
    public class CommandTemplate
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("groupName")]
        public string GroupName { get; set; }

        [XmlAttribute("provider")]
        public ConnectionInfoTypeOption ConnectionType { get; set; }

        [XmlAttribute("templateType")]
        public TemplateTypeOption Type { get; set; }

        [XmlElement]
        public string Description { get; set; }

        [XmlElement]
        public string Command { get; set; }


    }
}
