using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public class DataProfileItemCollection : KeyedCollection<string, DataProfileItem>
    {
        protected override string GetKeyForItem(DataProfileItem item)
        {
            return item.Key;
        }
    }
}
