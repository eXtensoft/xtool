using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public class ConnectionTypeFilterGroup : FilterGroup
    {
        public override string Name
        {
            get { return "Type"; }
        }

        public ConnectionTypeFilterGroup(IEnumerable<ConnectionInfoTypeOption> connectionTypes, Action refresh)
        {
            foreach (var connectionType in connectionTypes)
            {
                Filters.Add(new ConnectionTypeFilter(connectionType,refresh));
            }
        }


    }
}
