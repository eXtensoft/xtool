using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public class TableGroupingViewModel : GroupingViewModel
    {
        private ObservableCollection<GroupingViewModel> _Items;
        public ObservableCollection<GroupingViewModel> Items
        {
            get { return _Items; }
            set { _Items = value; }
        }

        #region IsSelected (bool)

        /// <summary>
        /// Gets or sets the bool value for IsSelected
        /// </summary>
        /// <value> The bool value.</value>
        private bool _IsSelected;
        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                if (_IsSelected != value)
                {
                    _IsSelected = value;
                    OnPropertyChanged("IsSelected");
                }
            }
        }

        #endregion

        public TableGroupingViewModel()
        {
            Title = "Tables";
            _Items = new ObservableCollection<GroupingViewModel>();

        }
    }
}
