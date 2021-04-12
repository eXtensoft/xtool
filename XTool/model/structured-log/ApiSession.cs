using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool.Cyclops
{
    public class ApiSession
    {
        public long Id { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public Guid BasicToken { get; set; }

        public string BearerToken { get; set; }

        public int TenantId { get; set; }

        public int PatronId { get; set; }

        public int SsoPatronId { get; set; }

        public int GatewayPatronId { get; set; }

        public string IPAddress { get; set; }

        public string UserAgent { get; set; }

        public string PassKey { get; set; }

        public string LinesOfBusiness { get; set; }

        public DateTimeOffset Tds { get; set; }

    }
}
