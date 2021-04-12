using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using XTool.Inference;


namespace XTool
{
    [KnownType(typeof(MongoDbConnectionInfo))]
    [KnownType(typeof(SqlServerConnectionInfo))]
    [KnownType(typeof(RedisConnectionInfo))]
    [KnownType(typeof(Neo4jConnectionInfo))]
    [KnownType(typeof(MySqlConnectionInfo))]
    [KnownType(typeof(FileConnectionInfo))]
    [KnownType(typeof(ExcelConnectionInfo))]
    [KnownType(typeof(JsonConnectionInfo))]
    [KnownType(typeof(ApiConnectionInfo))]
    [XmlInclude(typeof(MongoDbConnectionInfo))]
    [XmlInclude(typeof(SqlServerConnectionInfo))]
    [XmlInclude(typeof(RedisConnectionInfo))]
    [XmlInclude(typeof(Neo4jConnectionInfo))]
    [XmlInclude(typeof(MySqlConnectionInfo))]
    [XmlInclude(typeof(FileConnectionInfo))]
    [XmlInclude(typeof(ExcelConnectionInfo))]
    [XmlInclude(typeof(JsonConnectionInfo))]
    [XmlInclude(typeof(ApiConnectionInfo))]
    [Serializable]
    public class ConnectionInfo
    {
        [XmlAttribute("type")]
        public ConnectionInfoTypeOption ConnectionType { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("server")]
        public string Server { get; set; }

        [XmlElement]
        public string Text { get; set; }

        [XmlAttribute("cnt")]
        public int Count { get; set; }

        [XmlAttribute("zone")]
        public ZoneTypeOption Zone { get; set; }

        [XmlElement]
        public string ProviderName { get; set; }

        [XmlElement(ElementName = "Tag")]
        public List<string> Tags { get; set; }

        public virtual void PrepareToSave()
        {

        }

        public virtual void AddCommandTemplate()
        {

        }
    }
}
