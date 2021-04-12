using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public class FilterValue
    {
        public bool IsSelected { get; set; }
        public string Key { get; set; }
        public int Count { get; set; }

        public override string ToString()
        {
            return Key;
        }
    }
}
