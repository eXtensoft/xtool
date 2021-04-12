using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public class ViewSchemaGroupingViewModel : GroupingViewModel
    {
        private ObservableCollection<SqlTableViewModel> _Items;
        public ObservableCollection<SqlTableViewModel> Items
        {
            get { return _Items; }
            set { _Items = value; }
        }
        public ViewSchemaGroupingViewModel(List<Discovery.SqlTable> models, string schemaName, SqlServerConnectionInfoViewModel connectionViewModel)
        {
            Title = schemaName;
            _Items = new ObservableCollection<SqlTableViewModel>((from m in models select new SqlTableViewModel(m,connectionViewModel)).ToList());

        }
    }
}
