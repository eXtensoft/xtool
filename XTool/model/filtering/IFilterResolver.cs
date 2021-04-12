// <copyright file="IFilterResolver.cs" company="eXtensible Solutions LLC">
// Copyright © 2014 All Right Reserved
// </copyright>

namespace XTool.Filtering
{
    using System.Collections.Generic;

    public interface IFilterResolver
    {
        List<FilterItem> Filters { get; set; }

        bool Resolve(List<FilterItem> selectedFilters, object item);
    }
}