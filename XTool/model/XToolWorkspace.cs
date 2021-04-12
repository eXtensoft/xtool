// <copyright file="XToolWorkspace.cs" company="eXtensible Solutions LLC">
// Copyright © 2015 All Right Reserved
// </copyright>

namespace XTool
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;


    [Serializable]
    [DataContract(Namespace = "http://eXtensibleSolutions/schemas/2016/04")]
    public class XToolWorkspace
    {
        [DataMember]
        [XmlAttribute("title")]
        public string Title { get; set; }

        [DataMember]
        [XmlAttribute("createdAt")]
        public DateTime CreatedAt { get; set; }

        [DataMember]
        [XmlAttribute("createdBy")]
        public string CreatedBy { get; set; }

        [DataMember]
        public List<ConnectionInfo> Connections { get; set; }

        [DataMember]
        public List<CommandTemplate> TemplateCommands { get; set; }

        [DataMember]
        public Cyclops.Workspace Logging { get; set; }

        internal void PrepareToSave()
        {
            Connections.ForEach(x => x.PrepareToSave());
            List<CommandTemplate> instanceTemplates = TemplateCommands.Where(x => x.Type == TemplateTypeOption.Instance).ToList();
            if (instanceTemplates != null && instanceTemplates.Count > 0)
            {
                TemplateCommands = instanceTemplates;
            }
            else
            {
                TemplateCommands.Clear();
            }
        }
    }
}
