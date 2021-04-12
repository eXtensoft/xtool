using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public class DataProfileItem
    {
        public string Key { get; set; }
        public int Count { get; set; }
        public double Percent { get; set; }
        public double CumulativePercent { get; set; }
    }
}
