using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public class DataProfileFieldCollection : KeyedCollection<int, DataProfileField>
    {
        protected override int GetKeyForItem(DataProfileField item)
        {
            return item.Index;
        }
    }
}
