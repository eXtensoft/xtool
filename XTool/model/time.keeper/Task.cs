// <copyright file="Task.cs" company="eXtensoft">
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
    public sealed class Task
    {

        #region local fields
        #endregion local fields

        #region properties

        public int TaskId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Group { get; set; }

        public int MasterId { get; set; }

        public string Url { get; set; }

        #endregion properties

        #region constructors
        public Task() { }
        #endregion constructors

    }
}
