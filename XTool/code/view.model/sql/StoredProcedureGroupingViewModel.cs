using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public class StoredProcedureGroupingViewModel : GroupingViewModel
    {
        private ObservableCollection<GroupingViewModel> _Items;
        public ObservableCollection<GroupingViewModel> Items
        {
            get { return _Items; }
            set { _Items = value; }
        }




        public StoredProcedureGroupingViewModel()
        {
            Title = "Stored Procedures";
            _Items = new ObservableCollection<GroupingViewModel>();
        }
    }
}
