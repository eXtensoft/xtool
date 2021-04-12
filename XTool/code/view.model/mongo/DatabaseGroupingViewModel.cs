// <copyright file="DatabaseGroupingViewModel.cs" company="eXtensible Solutions LLC">
// Copyright © 2015 All Right Reserved
// </copyright>

namespace XTool
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Runtime.Serialization;

    public sealed class DatabaseGroupingViewModel: GroupingViewModel
    {
        private ObservableCollection<CollectionViewModel> _Items;
        public ObservableCollection<CollectionViewModel> Items
        {
            get { return _Items; }
            set { _Items = value; }
        }
        public DatabaseGroupingViewModel()
        {
            Title = "Databases";
            _Items = new ObservableCollection<CollectionViewModel>();

        }
    }
}
