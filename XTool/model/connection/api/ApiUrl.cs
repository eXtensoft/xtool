using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using XTool.Inference;

namespace XTool
{
    [Serializable]
    public class ApiUrl
    {
        [XmlElement]
        public ProtocolOption Protocol { get; set;}

        [XmlElement]
        public ZoneTypeOption Zone { get; set; }

        [XmlAttribute("url")]
        public string Url { get; set; }

        [XmlAttribute("cnt")]
        public int Count { get; set; }

        [XmlIgnore]
        public bool IsAvailable { get; set; }


        public ApiUrl(string model)
        {
            if (!String.IsNullOrWhiteSpace(model))
            {
                ZoneTypeOption zone;
                ProtocolOption protocol;
                string[] t = model.Trim().Split(new char[] { ':' });
                if (t.Length.Equals(3) && Enum.TryParse<ZoneTypeOption>(t[2], out zone) && Enum.TryParse<ProtocolOption>(t[0],out protocol))
                {
                    Url = t[1].ToLower();
                    Zone = zone;
                    Protocol = protocol;
                    IsAvailable = true;
                }
            }
        }
        public ApiUrl(ZoneTypeOption zone, string url, ProtocolOption protocol)
        {
            Zone = zone;
            Url = url;
            Protocol = protocol;
            IsAvailable = false;

        }

        public ApiUrl() { }


        public bool Validate()
        {

            bool b = Protocol != ProtocolOption.None;
            b = b ? (!String.IsNullOrWhiteSpace(Url) && !AppConstants.DefaultUrl.Contains(Url.ToLower())) : false;
            IsAvailable = b;


            return IsAvailable;
        }

    }
}
