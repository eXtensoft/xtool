// <copyright file="CustomFilterGroup.cs" company="eXtensible Solutions LLC">
// Copyright © 2014 All Right Reserved
// </copyright>

namespace XTool.Filtering
{
    using System.Collections.Generic;
    using System.Linq;

    public class CustomFilterGroup<T> : FilterGroup
    {
        #region local fields

        private IFilterResolver _Resolver;

        #endregion local fields

        public CustomFilterGroup(string propertyName, IFilterResolver resolver)
        {
            Key = propertyName;
            _Resolver = resolver;
            Filters = resolver.Filters;
            Info = typeof(T).GetProperty(propertyName);
        }

        public override bool Resolve(object item)
        {
            bool b = false;
            if (Info != null && Info.CanRead)
            {
                object o = Info.GetValue(item, null);
                List<FilterItem> selectedfilters = Filters.Where(x => x.IsFilter).ToList();
                b = _Resolver.Resolve(selectedfilters, o);
            }
            return b;
        }
    }
}