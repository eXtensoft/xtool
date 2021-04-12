using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public class TagFilter : Filter
    {
        private string _Tag;
        public override string Name
        {
            get { return _Tag; }
        }

        public TagFilter(string tag)
        {
            _Tag = tag;
            IsSelected = true;
        }
    }
}
