// <copyright file="Note.cs" company="eXtensoft">
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
    public sealed class Note
    {

        #region local fields
        #endregion local fields

        #region properties

        public int NoteId { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public string UserIdentity { get; set; }

        public DateTime Tds { get; set; }

        #endregion properties

        #region constructors
        public Note() { }
        #endregion constructors

    }
}
