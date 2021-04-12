using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public class ZoneFilterGroup : FilterGroup
    {

        public override string Name
        {
            get
            {
                return "Zone";
            }

        }

        public ZoneFilterGroup(IEnumerable<XTool.Inference.ZoneTypeOption> zones, Action refresh)
        {
            foreach (var zone in zones)
            {
                this.Filters.Add(new ZoneFilter(zone, refresh));
            }
        }




    }
}
