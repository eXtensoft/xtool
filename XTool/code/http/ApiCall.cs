using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public class ApiCall
    {
        public Guid Id { get; set; }
        public string Key { get; set; }

        public DateTime RanAt { get; set; }
        public ApiRequest Request { get; set; }
        public ApiResponse Response { get; set; }

    }
}
