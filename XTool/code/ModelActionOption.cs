using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    [Flags]
    public enum ModelActionOption
    {
        None = 0,
        Post = 1,
        Put = 2,
        Delete = 4,
        Get = 8,
        GetAll = 16,
        GetAllProjections = 32,
        ExecuteAction = 64
    }
}
