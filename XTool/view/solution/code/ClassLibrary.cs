using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JunkHarness
{
    public class ClassLibrary
    {
        public CodeDomainOption CodeDomain { get; set; }

        public string CodeGroup { get; set; }

        public string Namespace { get; set; }

        public SortedDictionary<string,int> Items { get; set; }

        public ClassLibrary()
        {

        }

        public ClassLibrary(string nameSpace)
        {
            Namespace = nameSpace;
            Items = new SortedDictionary<string, int>();
        }

        public void Add(string item)
        {
            if (Items == null)
            {
                Items = new SortedDictionary<string, int>();
            }
            if (!Items.ContainsKey(item))
            {
                Items.Add(item,0);
            }
            Items[item]++;
        }


    }
}
