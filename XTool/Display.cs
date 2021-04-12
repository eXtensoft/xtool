using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public class Display
    {
        public string Text { get; set; }
        public int MaxDistinct { get; set; }

        public Display(string text, int maxDistinct)
        {
            Text = text;
            MaxDistinct = maxDistinct;
        }
    }
}
