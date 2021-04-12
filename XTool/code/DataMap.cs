using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XTool
{
    [Serializable]
    public class DataMap
    {
        [XmlAttribute("databaseType")]
        public string DatabaseType { get; set; }

        [XmlAttribute("dbType")]
        public System.Data.DbType DataAccessType { get; set; }

        [XmlAttribute("readerAccessor")]
        public string DataReaderAccessor { get; set; }

        [XmlAttribute("clrType")]
        public string ClrType
        {
            get
            {
                return SystemType.FullName;
            }
            set
            {
                SystemType = Type.GetType(value);
            }

        }
        [XmlIgnore]
        public Type SystemType { get; set; }

    }

}

