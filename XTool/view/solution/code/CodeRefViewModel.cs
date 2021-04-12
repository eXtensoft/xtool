using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JunkHarness
{
    public class CodeRefViewModel
    {
        public string Name { get; set; }
        public string GroupName { get; set; }

        public List<ProjectRefViewModel> Projects { get; set; }

        public override string ToString()
        {
            return String.Format("{0} - {1}", Name, Projects.Count);
        }
    }
}
