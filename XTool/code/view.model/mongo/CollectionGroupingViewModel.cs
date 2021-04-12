// <copyright file="CollectionGroupingViewModel.cs" company="eXtensible Solutions LLC">
// Copyright © 2015 All Right Reserved
// </copyright>

namespace XTool
{
    using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;

    public sealed class CollectionGroupingViewModel : GroupingViewModel
    {
        private ObservableCollection<GroupingViewModel> _Items;
        public ObservableCollection<GroupingViewModel> Items
        {
            get { return _Items; }
            set { _Items = value; }
        }
        public CollectionGroupingViewModel()
        {
            Title = "Collections";
            _Items = new ObservableCollection<GroupingViewModel>();

        }
    }
}

