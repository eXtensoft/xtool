using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XTool
{
    public interface IHierarchicalViewModel
    {
        bool IsExpanded { get; set; }
        bool IsSelected { get; set; }
        IHierarchicalViewModel Master { get; set; }
    }
}
