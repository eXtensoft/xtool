using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace XTool
{
    [Serializable]
    public class State
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        #region Display (string)
        private string _Display;

        /// <summary>
        /// Gets or sets the string value for Display
        /// </summary>
        /// <value> The string value.</value>
        [XmlAttribute("display")]
        public string Display
        {
            get { return (String.IsNullOrEmpty(_Display)) ? Name : _Display; }
            set
            {
                if (_Display != value)
                {
                    _Display = value;
                }
            }
        }
        #endregion

        [XmlElement]
        public List<IEndpointAction> EndpointActions { get; set; }
    }
}
