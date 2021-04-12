using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public class TableSchemaGroupingViewModel : GroupingViewModel
    {

        private ObservableCollection<SqlTableViewModel> _Items;
        public ObservableCollection<SqlTableViewModel> Items
        {
            get { return _Items; }
            set { _Items = value; }
        }

        #region IsChecked (bool)

        /// <summary>
        /// Gets or sets the bool value for IsChecked
        /// </summary>
        /// <value> The bool value.</value>
        private bool _IsChecked;
        public bool IsChecked
        {
            get { return _IsChecked; }
            set
            {
                if (_IsChecked != value)
                {
                    _IsChecked = value;
                    OnPropertyChanged("IsChecked");
                    foreach (var item in Items)
                    {
                        item.IsChecked = value;
                    }
                }
            }
        }

        #endregion

        public TableSchemaGroupingViewModel(List<Discovery.SqlTable> models, string schemaName, SqlServerConnectionInfoViewModel connectionViewModel)
        {
            Title = schemaName;
            _Items = new ObservableCollection<SqlTableViewModel>((from m in models select new SqlTableViewModel(m, connectionViewModel)).ToList());


        }

        public TableSchemaGroupingViewModel(List<Discovery.SqlTable> models, string schemaName, MySqlConnectionInfoViewModel connectionViewModel)
        {
            Title = schemaName;
            _Items = new ObservableCollection<SqlTableViewModel>((from m in models select new SqlTableViewModel(m, connectionViewModel)).ToList());


        }

    }
}
