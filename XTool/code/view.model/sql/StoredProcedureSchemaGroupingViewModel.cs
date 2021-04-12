using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public class StoredProcedureSchemaGroupingViewModel : GroupingViewModel
    {
        private ObservableCollection<SqlStoredProcedureViewModel> _Items;
        public ObservableCollection<SqlStoredProcedureViewModel> Items
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

        public StoredProcedureSchemaGroupingViewModel(List<Discovery.SqlStoredProcedure> models, string schemaName, SqlServerConnectionInfoViewModel viewModel)
        {
            Title = schemaName;
            _Items = new ObservableCollection<SqlStoredProcedureViewModel>((from model in models select new SqlStoredProcedureViewModel(model,viewModel)).ToList());
        }
    }
}
