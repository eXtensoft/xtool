using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public class ApiResponse
    {
        public HttpStatusCode ResponseCode { get; set; }
        public string ResponseText { get; set; }
        public string ResponseBody { get; set; }

    }
}
