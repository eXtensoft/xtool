// <copyright file="TimeEntry.cs" company="eXtensoft">
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
    public sealed class TimeEntry
    {

        #region local fields
        #endregion local fields

        #region properties

        public int TimeEntryId { get; set; }

        public Guid Id { get; set; }

        public string TicketId { get; set; }

        public DateTimeOffset BeginAt { get; set; }

        public long Timespan { get; set; }

        public string Resource { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        #endregion properties

        #region constructors
        public TimeEntry() { }
        #endregion constructors

    }
}
