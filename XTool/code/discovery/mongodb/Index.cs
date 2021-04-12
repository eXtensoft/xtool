// <copyright file="Index.cs" company="eXtensible Solutions LLC">
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
    public sealed class Index
    {
        public string Key { get; set; }

        public string Name { get; set; }

        public override string ToString()
        {
            return String.Format("{0}: {1}",Name,Key);
        }
    }
}
