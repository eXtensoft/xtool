// <copyright company="eXtensoft, LLC" file="SqlScript.cs">
// Copyright © 2016 All Right Reserved
// </copyright>

namespace XTool.Discovery
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    [Serializable]
    public sealed class SqlScript
    {
        [XmlAttribute("key")]
        public string Key { get; set; }

        [XmlAttribute("type")]
        public SqlObjectTypeOption ObjectType { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("schema")]
        public string Schema { get; set; }

        [XmlElement]
        public DateTime CreatedAt { get; set; }

        [XmlElement]
        public string Text { get; set; }


    }

}
