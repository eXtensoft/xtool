// <copyright file="Collection.cs" company="eXtensible Solutions LLC">
// Copyright © 2015 All Right Reserved
// </copyright>

namespace XTool.Mongo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;


    [Serializable]
    [DataContract(Namespace = "http://eXtensibleSolutions/schemas/2016/04")]
    public sealed class Collection
    {
        public string Name { get; set; }

        public string FullName { get; set; }

        public double AvgDocumentSize { get; set; }

        public long DataSize { get; set; }

        public int IndexCount { get; set; }

        public long DocumentCount { get; set; }

        public List<Index> Indexes { get; set; }

        public string Json { get; set; }

        [IgnoreDataMember]
        [XmlIgnore]
        public List<MongoDbCommand> Commands { get; set; }

        public override string ToString()
        {
            return Name;
        }

    }
}
