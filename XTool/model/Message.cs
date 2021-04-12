// <copyright file="Message.cs" company="eXtensible Solutions LLC">
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
    public sealed class Message
    {
        public DateTime Start { get; set; }

        public int Timespan { get; set; }

        public string Extract { get; set; }

        public List<string> Text { get; set; }

    }
}
