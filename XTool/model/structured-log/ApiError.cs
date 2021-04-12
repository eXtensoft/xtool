using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool.Cyclops
{
    public class ApiError
    {
        public long Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Month { get; set; }

        public string Day { get; set; }

        public string ApplicationKey { get; set; }

        public string Zone { get; set; }

        public string AppContextInstance { get; set; }

        public Guid MessageId { get; set; }

        public string Category { get; set; }

        public string Severity { get; set; }

        public string Message { get; set; }


        public string XmlData { get; set; }


        public long ApiRequestId { get; set; }

        public long SessionId { get; set; }




    }
}
