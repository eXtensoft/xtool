// <copyright file="CharProfileItem.cs" company="eXtensible Solutions LLC">
// Copyright © 2015 All Right Reserved
// </copyright>

namespace XTool
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;


    [Serializable]
    [DataContract(Namespace = "http://eXtensibleSolutions/schemas/2016/04")]
    public sealed class CharProfileItem
    {
        public int Key { get; set; }

        public string Display
        {
            get { return Key > 0 ? ((char)Key).ToString() : String.Empty; }
        }

        public int Count  { get; set; }

        public override string ToString()
        {
            return Key > 0 ? String.Format("{0}\t{1}\t{2}",Display,Key,Count) : String.Empty;
        }
    }
}
