// <copyright file="PropertyFilterGroup.cs" company="eXtensible Solutions LLC">
// Copyright © 2014 All Right Reserved
// </copyright>

namespace XTool.Filtering
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class PropertyFilterGroup : FilterGroup
    {
        public override bool Resolve(object item)
        {
            bool b = false;
            if (Info != null && Info.CanRead)
            {
                string s = (string)Info.GetValue(item, null);
                string p = Truncate(s, 35);
                FilterItem filter = Filters.FirstOrDefault(x => x.Text.Equals(p, StringComparison.OrdinalIgnoreCase));
                if (filter != null)
                {
                    b = filter.IsFilter;
                }
            }
            return b;
        }

        public PropertyFilterGroup(string propertyName)
        {
            Key = propertyName;
        }

        public static PropertyFilterGroup GenerateFilterGroup<T>(string propertyName, IEnumerable<T> items, int maxLength = 35)
        {
            Dictionary<string, int> d = new Dictionary<string, int>();
            Type t = typeof(T);
            PropertyFilterGroup group = null;
            PropertyInfo info = t.GetProperty(propertyName);
            if (info != null)
            {
                group = new PropertyFilterGroup(propertyName) { Info = info };
                foreach (var item in items)
                {
                    string s = info.GetValue(item, null).ToString();
                    string key = Truncate(s, maxLength);
                    if (!d.ContainsKey(key))
                    {
                        d.Add(key, 0);
                    }
                    d[key]++;
                }
                group.Filters = new List<FilterItem>(
                    (from x in d
                     orderby x.Key
                     select new FilterItem() { Text = x.Key, Count = x.Value, IsFilter = true }).ToList()
                    );
            }
            return group;
        }

        private static string Truncate(string s, int length)
        {
            return (s.Length > length) ? String.Format("{0}...", s.Substring(0, length)) : s;
        }
    }
}