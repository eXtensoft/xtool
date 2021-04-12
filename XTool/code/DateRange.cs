using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public class DateRange
    {
        public BetweenOption Between { get; set; }
        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public override string ToString()
        {
            //string from = From.ToString
            return String.Format("{0} - {1}", From.ToString("m"), To.ToString("m"));
            //return String.Format("{0} - {1}", From.ToString("MM-dd"), To.ToString("MM-dd"));
        }
    }
}
