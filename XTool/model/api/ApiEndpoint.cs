using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;


namespace XTool
{
    [Serializable]
    public class ApiEndpoint
    {
        [XmlAttribute("order")]
        public int Order { get; set; }
        [XmlAttribute("registrationOrder")]
        public int RegistrationOrder { get; set; }
        [XmlAttribute("registredAs")]
        public string RegisteredAs { get; set; }

        [XmlAttribute("moniker")]
        public string Moniker { get; set; }

        [XmlIgnore]
        public string Key { get
            {
                return String.Format("{0}:{1}:{2}", HttpMethod,Moniker, Pattern);
            }
        }

        [XmlElement]
        public HttpMethodOption HttpMethod { get; set; }

        [XmlElement]
        public string Pattern { get; set; }

        [XmlAttribute("requiresAuthorization")]
        public bool RequiresAuthorization { get; set; }

        [XmlElement]
        public HeaderTypeOption Authorization { get; set; }

        [XmlElement]
        public string UrlSuffix { get; set; }

        public List<ApiParameter> Parameters { get; set; }

        [XmlAttribute("hasBody")]
        public bool HasBody { get; set; }
        [XmlAttribute("header")]
        public string Header { get; set; }
        [XmlElement]
        public string Body { get; set; }
        [XmlIgnore]
        public string Params
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                if (Parameters != null)
                {
                    sb.Append(String.Format("{0}: ", Parameters.Count));
                    int i = 0;
                    foreach (var item in Parameters)
                    {
                        if (i > 0)
                        {
                            sb.Append(", ");
                        }
                        sb.Append(item.Name);
                        i++;
                    }
                }

                return sb.ToString();

            }
        }

        [XmlIgnore]
        public List<ApiParameterSet> ParameterSets { get; set; }

        [XmlElement]
        public List<ApiHeader> Headers { get; set; }

    }
}
