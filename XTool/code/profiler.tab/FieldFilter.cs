using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public class FieldFilter
    {
        public int ColumnIndex { get; set; }
        public string ColumnName { get; set; }
        public string ColumnDatatype { get; set; }
        public List<FilterValue> Values { get; set; }

        public List<string> FilterValues { get; set; }

        public bool IsAnd { get; set; }

    }
}
