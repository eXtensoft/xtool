using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XTool
{
    public class MongoDbConnectionInfo : ConnectionInfo
    {

        [XmlElement("database")]
        public string[] Databases { get; set; }

        [XmlElement]
        public int Port { get; set; }

        [XmlElement]
        public string Username { get; set; }

        [XmlElement]
        public string Password { get; set; }

        [XmlAttribute("useSshTunnel")]
        public bool UseSSHTunnel { get; set; }

        [XmlAttribute("sshHost")]
        public string SshHost { get; set; }


        #region Commands (List<XTool.Mongo.Command>)

        private List<MongoDbCommand> _Commands = new List<MongoDbCommand>();

        /// <summary>
        /// Gets or sets the List<XTool.Mongo.Command> value for Commands
        /// </summary>
        /// <value> The List<XTool.Mongo.Command> value.</value>

        public List<MongoDbCommand> Commands
        {
            get { return _Commands; }
            set
            {
                if (_Commands != value)
                {
                    _Commands = value;
                }
            }
        }

        #endregion

    }
}
