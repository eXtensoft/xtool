using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public class ZoneFilter : Filter
    {
        private XTool.Inference.ZoneTypeOption _Zone;
        public override string Name
        {
            get { return _Zone.ToString(); }
        }

        public ZoneFilter(XTool.Inference.ZoneTypeOption zone, Action refresh)
        {
            _Zone = zone;
            IsSelected = true;
            base.Refresh = refresh;
        }
    }
}
