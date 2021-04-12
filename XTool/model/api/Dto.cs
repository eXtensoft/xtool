using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XTool.Dto
{
    [DataContract]
    [Serializable]
    public class Endpoint
    {
        [DataMember]
        [XmlAttribute("order")]
        public int Order { get; set; }

        [DataMember]
        [XmlAttribute("httpMethod")]
        public string HttpMethod { get; set; }



        [DataMember]
        public string Pattern { get; set; }

        [DataMember]
        [XmlAttribute("controller")]
        public string Controller { get; set; }

        [DataMember]
        [XmlAttribute("controllerMethod")]
        public string ControllerMethod { get; set; }

        [DataMember]
        [XmlAttribute("registeredAs")]
        public string RegisterName { get; set; }

        [DataMember]
        [XmlElement]
        public List<EndpointParameter> Parameters { get; set; }

    }

    [DataContract]
    [Serializable]
    public class EndpointParameter
    {
        [DataMember]
        [XmlAttribute("name")]
        public string Name { get; set; }

        [DataMember]
        [XmlAttribute("type")]
        public string Type { get; set; }

        [DataMember]
        [XmlAttribute("source")]
        public string Source { get; set; }

        [DataMember]
        [XmlAttribute("defaultValue")]
        public string DefaultValue { get; set; }
    }


    [DataContract]
    [Serializable]
    public class ControllerRegistration
    {
        [DataMember]
        [XmlAttribute("order")]
        public int Order { get; set; }

        [DataMember]
        [XmlAttribute("name")]
        public string Name { get; set; }
        [DataMember]

        public List<string> Methods { get; set; }

        [DataMember]
        public List<Endpoint> Endpoints { get; set; }
    }

    [DataContract]
    [Serializable]
    public class RegistrationCall
    {
        [DataMember]
        public string Machine { get; set; }
        [DataMember]
        public string Config { get; set; }
        [DataMember]
        public string Mode { get; set; }
        [DataMember]
        public List<ControllerRegistration> Registration { get; set; }
    }


}
