using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public class AsciiMapCollection : KeyedCollection<int, AsciiMap>
    {
        protected override int GetKeyForItem(AsciiMap item)
        {
            return item.Int;
        }

        public List<AsciiMap> ToList()
        {
            List<AsciiMap> list = new List<AsciiMap>();
            foreach (var item in this)
            {
                list.Add(item);
            }
            return list;
        }
    }

}
