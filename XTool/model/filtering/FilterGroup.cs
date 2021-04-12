// <copyright file="FilterGroup.cs" company="eXtensible Solutions LLC">
// Copyright © 2014 All Right Reserved
// </copyright>

namespace XTool.Filtering
{
    using System.Collections.Generic;
    using System.Reflection;

    public abstract class FilterGroup
    {
        public string Key { get; set; }

        public PropertyInfo Info { get; set; }

        #region Items (List<FilterItem>)

        private List<FilterItem> _Filters = new List<FilterItem>();

        /// <summary>
        /// Gets or sets the List<FilterItem> value for Items
        /// </summary>
        /// <value> The List<FilterItem> value.</value>

        public List<FilterItem> Filters
        {
            get { return _Filters; }
            set
            {
                if (_Filters != value)
                {
                    _Filters = value;
                }
            }
        }

        #endregion Items (List<FilterItem>)

        public bool FiltersExist { get; set; }

        public abstract bool Resolve(object item);
    }
}