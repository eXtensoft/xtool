// <copyright file="FilterProvider.cs" company="eXtensible Solutions LLC">
// Copyright © 2014 All Right Reserved
// </copyright>

namespace XTool.Filtering
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Data;

    public class FilterProvider<T> : INotifyPropertyChanged where T : class
    {
        #region local fields

        private Type _Filterable;

        #endregion local fields

        #region properties

        private CollectionViewSource _CollectionView;

        public CollectionViewSource CollectionView
        {
            get
            {
                return _CollectionView;
            }
            set
            {
                _CollectionView = value;
            }
        }

        private bool _DefaultAccepted = false;

        public bool DefaultAccepted
        {
            get
            {
                return _DefaultAccepted;
            }
            set
            {
                _DefaultAccepted = value;
                OnPropertyChanged("DefaultAccepted");
            }
        }

        #region Groups (List<FilterGroup>)

        private List<FilterGroup> _Groups = new List<FilterGroup>();

        /// <summary>
        /// Gets or sets the List<FilterGroup> value for Groups
        /// </summary>
        /// <value> The List<FilterGroup> value.</value>

        public List<FilterGroup> Groups
        {
            get { return _Groups; }
            set
            {
                if (_Groups != value)
                {
                    _Groups = value;
                }
            }
        }

        #endregion Groups (List<FilterGroup>)

        public int TotalItemsCount { get; set; }

        public int FilteredItemsCount { get; set; }

        #endregion properties

        #region constructors

        public FilterProvider(bool defaultAccepted, CollectionViewSource collectionView)
        {
            _DefaultAccepted = defaultAccepted;
            _CollectionView = collectionView;
            _Filterable = typeof(T);
        }

        #endregion constructors

        #region instance methods

        public IEnumerable<FilterItem> this[string key]
        {
            get
            {
                return GetItems(key);
            }
        }

        private IEnumerable<FilterItem> GetItems(string key)
        {
            FilterGroup found = Groups.FirstOrDefault(x => x.Key.Equals(key, StringComparison.OrdinalIgnoreCase));
            if (found != null)
            {
                return found.Filters;
            }
            else
            {
                return new List<FilterItem>();
            }
        }

        public void AddDistinctValueFilterGroup(string propertyName, IEnumerable<T> items)
        {
            FilterGroup group = PropertyFilterGroup.GenerateFilterGroup<T>(propertyName, items);
            _Groups.Add(group);
        }

        public void AddCustomFilterGroup(CustomFilterGroup<T> group)
        {
            _Groups.Add(group);
        }

        public bool FiltersExist()
        {
            bool b = false;
            if (Groups != null)
            {
                foreach (var group in Groups)
                {
                    foreach (var filter in group.Filters)
                    {
                        if (filter.IsFilter)
                        {
                            group.FiltersExist = true;
                            b = true;
                            break;
                        }
                    }
                }
            }
            return b;
        }

        public bool FiltersExtant(string groupName)
        {
            bool b = false;
            return b;
        }

        public void ExecuteFilter(object o, FilterEventArgs args)
        {
            args.Accepted = DefaultAccepted;

            T t = args.Item as T;
            if (t != null && FiltersExist())
            {
                List<bool> resolutions = new List<bool>();
                foreach (var group in Groups)
                {
                    if (group.FiltersExist)
                    {
                        bool b = group.Resolve(t);
                        resolutions.Add(b);
                    }
                    else
                    {
                        resolutions.Add(false);
                    }
                }

                if (resolutions.Contains(false))
                {
                    args.Accepted = false;
                }
            }
        }

        #endregion instance methods

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion INotifyPropertyChanged Members
    }
}