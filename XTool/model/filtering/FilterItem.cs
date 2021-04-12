// <copyright file="FilterItem.cs" company="eXtensible Solutions LLC">
// Copyright © 2014 All Right Reserved
// </copyright>

namespace XTool.Filtering
{
    using System.ComponentModel;

    public class FilterItem : INotifyPropertyChanged
    {
        public string Text { get; set; }

        public string Tag { get; set; }

        #region IsFilter (bool)

        /// <summary>
        /// Gets or sets the bool value for IsFilter
        /// </summary>
        /// <value> The bool value.</value>
        private bool _IsFilter;

        public bool IsFilter
        {
            get { return _IsFilter; }
            set
            {
                if (_IsFilter != value)
                {
                    _IsFilter = value;
                    OnPropertyChanged("IsFilter");
                }
            }
        }

        #endregion IsFilter (bool)

        public int Count { get; set; }

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