// <copyright file="JsonConnectionInfo.cs" company="eXtensible Solutions LLC">
// Copyright © 2015 All Right Reserved
// </copyright>

namespace XTool
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;


    public sealed class JsonConnectionInfo : ConnectionInfo
    {
        public string Key { get; set; }

        public string Namespace { get; set; }

        public object Value { get; set; }

    }
}
