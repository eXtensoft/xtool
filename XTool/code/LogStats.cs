using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XTool.Cyclops
{
    [Serializable]
    public class LogStats
    {
        public DateTime CreatedAt { get; set; }



        [XmlAttribute("monthOfYear")]
        public string MonthOfYear { get; set; }

        [XmlAttribute("dayOfWeek")]
        public string DayOfWeek { get; set; }

        [XmlAttribute("weekOfYear")]
        public string WeekOfYear { get; set; }
        [XmlIgnore]
        public string Message { get; set; }


        [XmlElement("Statistic")]
        public List<LogStat> Statistics { get; set; }


    }


    [Serializable]
    public class LogStat
    {
        private static DateTime target = new DateTime(2001, 1, 1);
        [XmlAttribute("schema")]
        public string Schema { get; set; }

        [XmlAttribute("table")]
        public string Tablename { get; set; }
        [XmlAttribute("id")]
        public long MaxId { get; set; }

        public DateTime LastEntryAt { get; set; }

        [XmlAttribute("current")]
        public bool IsCurrent { get; set; }
      
        public bool IsEmpty
        {

            get { return LastEntryAt < target; }
        }

    }
}
