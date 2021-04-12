// <copyright file="Resource.cs" company="eXtensoft">
// Copyright © 2017 All Right Reserved
// </copyright>

namespace Chronometrix
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    [Serializable]
    public sealed class Resource
    {

        #region local fields
        #endregion local fields

        #region properties

        public int ResourceId { get; set; }

        public Guid Id { get; set; }

        public string Moniker { get; set; }

        #endregion properties

        #region constructors
        public Resource() { }
        #endregion constructors

    }
}
