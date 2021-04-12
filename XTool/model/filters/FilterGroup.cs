using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public abstract class FilterGroup : INotifyPropertyChanged
    {
        public abstract string Name { get; }

        private bool _IsFilter;
        public bool IsFilter 
        {
            get { return _IsFilter; }
            set
            {
                _IsFilter = value;
                OnPropertyChanged("IsFilter");
            }
        }

        private bool _IsGroupBy;
        public bool IsGroupBy
        {
            get { return _IsGroupBy; }
            set
            {
                _IsGroupBy = value;
                OnPropertyChanged("IsGroupBy");
                OnSelect(value);

            }
        }


        private Filter _SelectedFilter;
        public Filter SelectedFilter 
        {
            get { return _SelectedFilter; }
            set
            {
                _SelectedFilter = value;
                OnPropertyChanged("SelectedFilter");
            }        
        }

        #region Filters (List<Filter>)

        private List<Filter> _Filters = new List<Filter>();

        /// <summary>
        /// Gets or sets the List<Filter> value for Filters
        /// </summary>
        /// <value> The List<Filter> value.</value>

        public List<Filter> Filters
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

        #endregion

        public bool ResolveFilter(string filterName)
        {
            bool b = false;
            var found = Filters.Find(x => x.Name.Equals(filterName, StringComparison.OrdinalIgnoreCase));
            if (found != null)
            {
                b = found.IsSelected;
            }
            return b;
        }
        public Action<bool,string> HandleSelect { get; set; }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
        

        private void OnSelect(bool isSelected)
        {
            if (HandleSelect != null)
            {
                string token = maps.ContainsKey(Name) ? maps[Name] : String.Empty;
                HandleSelect.Invoke(isSelected, token);
            }
        }

        private static IDictionary<string, string> maps = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            {"Type","ProviderName" },
            {"Zone","Zone" },
            {"Tag","Tag" },
        };
    }
}
