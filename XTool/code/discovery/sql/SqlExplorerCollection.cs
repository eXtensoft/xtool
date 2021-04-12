using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public class SqlExplorerCollection : KeyedCollection<string,SqlExplorer>
    {
        protected override string GetKeyForItem(SqlExplorer item)
        {
            return item.ConnectionString;
        }
    }
}
