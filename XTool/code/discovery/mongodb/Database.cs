// <copyright file="Database.cs" company="eXtensible Solutions LLC">
// Copyright © 2015 All Right Reserved
// </copyright>

namespace XTool.Mongo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;


    [Serializable]
    [DataContract(Namespace = "http://eXtensibleSolutions/schemas/2016/04")]
    public sealed class Database
    {
        public string Name { get; set; }

        public List<Collection> Collections { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
