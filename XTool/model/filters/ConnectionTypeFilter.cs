using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public class ConnectionTypeFilter : Filter
    {
        private ConnectionInfoTypeOption _ConnectionType;
        

        public override string Name
        {
            get
            {
                return _ConnectionType.ToString();
            }
        }

        public ConnectionTypeFilter(ConnectionInfoTypeOption connectionType, Action refresh)
        {
            _ConnectionType = connectionType;
            IsSelected = true;
            base.Refresh = refresh;
        }

        
    }
}
