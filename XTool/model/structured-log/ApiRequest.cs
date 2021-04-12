using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XTool.Cyclops
{
    public class ApiRequest
    {

        #region properties

        public long ApiRequestId { get; set; }

        public string AppKey { get; set; }

        public string AppZone { get; set; }

        public string AppInstance { get; set; }

        public decimal Elapsed { get; set; }

        public DateTime Start { get; set; }

        public string Protocol { get; set; }

        public string Host { get; set; }

        public string Path { get; set; }

        public string ClientIP { get; set; }

        public string UserAgent { get; set; }

        public string HttpMethod { get; set; }

        public string ControllerName { get; set; }

        public string ControllerMethod { get; set; }

        public string MethodReturnType { get; set; }

        public string ResponseCode { get; set; }

        public string ResponseText { get; set; }

        public XmlDocument XmlData { get; set; }

        public Guid MessageId { get; set; }

        public string BasicToken { get; set; }

        public string BearerToken { get; set; }

        public string AuthSchema { get; set; }

        public string AuthValue { get; set; }

        public string MessageBody { get; set; }

        public bool HasLog { get; set; }

        public DateTimeOffset Tds { get; set; }

        #endregion properties

        #region constructors
        public ApiRequest() { }
        #endregion constructors

    }

}
