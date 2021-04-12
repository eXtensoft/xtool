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
    public class ApiConnectionInfo : ConnectionInfo
    {

        [XmlElement]
        public ProtocolOption Protocol { get; set; }

        public List<ApiUrl> Urls { get; set; }

        #region Endpoints (List<ApiEndpoint>)

        private List<ApiEndpoint> _Endpoints = new List<ApiEndpoint>();

        /// <summary>
        /// Gets or sets the List<ApiEndpoint> value for Endpoints
        /// </summary>
        /// <value> The List<ApiEndpoint> value.</value>

        public List<ApiEndpoint> Endpoints
        {
            get { return _Endpoints; }
            set
            {
                if (_Endpoints != value)
                {
                    _Endpoints = value;
                }
            }
        }

        #endregion

        public List<ApiParameterSet> ApiCalls {get;set;}


        #region Headers (List<ApiHeader>)

        private List<ApiHeader> _Headers = new List<ApiHeader>();

        /// <summary>
        /// Gets or sets the List<ApiHeader> value for Headers
        /// </summary>
        /// <value> The List<ApiHeader> value.</value>

        public List<ApiHeader> Headers
        {
            get { return _Headers; }
            set
            {
                if (_Headers != value)
                {
                    _Headers = value;
                }
            }
        }

        #endregion

        public override void PrepareToSave()
        {
            StringBuilder sb = new StringBuilder();
            List<ApiUrl> list = new List<ApiUrl>();
            int i = 0;
            foreach (ZoneTypeOption item in Enum.GetValues(typeof(ZoneTypeOption)))
            {
                var found = Urls.Find(x => x.Zone.Equals(item));
                if (found != null)
                {
                    if (!String.IsNullOrEmpty(found.Url) &&  !AppConstants.DefaultUrls.Contains(found.Url))
                    {
                        list.Add(found);
                        if (i > 0)
                        {
                            sb.Append(";");
                        }
                        sb.Append(String.Format("{0}:{1}:{2}:{3}", found.Zone, found.Protocol, found.Url, found.Count));
                        i++;
                    }
                }

            }
            Urls = list;
            // Text = sb.ToString();
        }



    }
}
