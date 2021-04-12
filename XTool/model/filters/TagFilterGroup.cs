using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public class TagFilterGroup : FilterGroup
    {

        public override string Name
        {
            get { return "Tag"; }
        }


        public TagFilterGroup(IEnumerable<string> tags)
        {
            foreach (var tag in tags)
            {
                Filters.Add(new TagFilter(tag));
            }
        }
    }
}
