// <copyright company="eXtensoft, LLC" file="ReferenceFieldComparison.cs">
// Copyright © 2016 All Right Reserved
// </copyright>

namespace JunkHarness
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public sealed class ReferenceFieldComparison : IEnumerable<ReferenceField>
    {
        #region local members
        private int _ComparisonCount;
        private CompareOption _CompareBy;
        private string _Key;
        #endregion

        public List<ReferenceField> Items { get; set; }
        public string FieldName
        {
            get
            {
                return (Items != null && Items.Count > 0) ? Items[0].Name : String.Empty;
            }
        }

        public string GroupName
        {
            get
            {
                return (Items != null && Items.Count > 0) ? Items[0].Project : String.Empty;
            }
        }

        #region constructors
        public ReferenceFieldComparison(int comparisonCount, CompareOption compareBy, string key)
        {
            _ComparisonCount = comparisonCount;
            _CompareBy = compareBy;
            _Key = key;
            Items = new List<ReferenceField>();
        }
        #endregion


        public void Add(ReferenceField field)
        {
            Items.Add(field);
        }

        public string ComposeKey()
        {
            return _Key;
        }

        public IEnumerator<ReferenceField> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }

}
