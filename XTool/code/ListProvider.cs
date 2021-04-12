using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public static class ListProvider
    {

        public static IEnumerable<Display> GetCountLimits()
        {
            List<Display> list = new List<Display>()
            {
                {new Display("Twenty-Five",25)},
                {new Display("Fifty",50)},
                {new Display("One-Hundred",100)},
                {new Display("Two-Hundred-Fifty",250)},
                {new Display("Five-Hundred",500)},
                {new Display("One-Thousand",1000)},
                {new Display("100K",100000)},
                {new Display("All",Int32.MaxValue)},
            };
            return list;
        }
    }
}
