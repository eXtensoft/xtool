// <copyright file="Neo4jConnectionInfoViewModel.cs" company="eXtensible Solutions LLC">
// Copyright © 2015 All Right Reserved
// </copyright>

namespace XTool
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;



    public sealed class Neo4jConnectionInfoViewModel : ConnectionInfoViewModel
    {

        public new Neo4jConnectionInfo Model { get; set; }

        public Neo4jConnectionInfoViewModel(Neo4jConnectionInfo model)
        {
            Model = model;
            base.Model = model;
        }

    }
}
