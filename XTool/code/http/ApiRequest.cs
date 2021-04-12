using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public class ApiRequest
    {
        public HttpMethodOption HttpMethod { get; set; }
        public string BaseUrl { get; set; }
        public string Url { get; set; }
        public string RequestBody { get; set; }

        public ProtocolOption Protocol { get; set; }

        public List<RequestHeader> Headers { get; set; }
    }
}
