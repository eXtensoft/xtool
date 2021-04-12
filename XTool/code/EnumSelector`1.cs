// <copyright file="EnumSelector_1.cs" company="eXtensible Solutions LLC">
// Copyright © 2015 All Right Reserved
// </copyright>

namespace XTool
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;

    public sealed class EnumSelector
    {
        public EnumSelector() 
        {
            DayOfWeek dow = new DayOfWeek();
            Enum.GetNames(typeof(DayOfWeek));
        }

    }
}
