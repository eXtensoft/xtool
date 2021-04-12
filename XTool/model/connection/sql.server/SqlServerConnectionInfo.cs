using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace XTool.Inference
{
    [Serializable]
    public class SqlServerConnectionInfo : ConnectionInfo
    {
        [XmlAttribute("catalog")]
        public string Catalog { get; set; }

        [XmlElement]
        public string User { get; set; }

        [XmlElement]
        public string Pwd { get; set; }

        [XmlElement]
        public bool IntegratedSecurity { get; set; }

        #region Commands (List<SqlCommand>)

        private List<XTool.Inference.SqlCommand> _Commands = new List<XTool.Inference.SqlCommand>();

        /// <summary>
        /// Gets or sets the List<SqlCommand> value for Commands
        /// </summary>
        /// <value> The List<SqlCommand> value.</value>
        [XmlElement("Command")]
        public List<XTool.Inference.SqlCommand> Commands
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
