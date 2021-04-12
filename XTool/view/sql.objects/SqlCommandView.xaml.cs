using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace XTool
{
    /// <summary>
    /// Interaction logic for SqlCommandView.xaml
    /// </summary>
    public partial class SqlCommandView : UserControl
    {
        private ICommand _AddParameterCommand;
        public ICommand AddParameterCommand
        {
            get
            {
                if (_AddParameterCommand == null)
                {
                    _AddParameterCommand = new RelayCommand(
                        param => AddParameter(),
                        param => CanAddParameter());
                }
                return _AddParameterCommand;
            }
        }

        private ICommand _RemoveParameterCommand;
        public ICommand RemoveParameterCommand
        {
            get
            {
                if (_RemoveParameterCommand == null)
                {
                    _RemoveParameterCommand = new RelayCommand<ParameterViewModel>(
                        new Action<ParameterViewModel>(RemoveParameter));
                }
                return _RemoveParameterCommand;
            }
        }



        public List<System.Data.DbType> DbTypes { get; set; }

        public SqlCommandView()
        {
            InitializeComponent();

            DbTypes = new List<System.Data.DbType>();
            foreach (string item in Enum.GetNames(typeof(System.Data.DbType)))
            {
                DbTypes.Add((System.Data.DbType)Enum.Parse(typeof(System.Data.DbType), item, true));
            }
        }


        private bool CanAddParameter()
        {
            return true;
        }
        private void AddParameter()
        {
            SqlCommandViewModel vm = DataContext as SqlCommandViewModel;
            if (vm != null)
            {
                if (vm.Parameters != null)
                {
                    ParameterViewModel pvm = new ParameterViewModel(new Inference.Parameter() { DataType = System.Data.DbType.String });
                    vm.Parameters.Add(pvm);
                }
            }
        }

        private void RemoveParameter(ParameterViewModel viewModel)
        {
            SqlCommandViewModel vm = DataContext as SqlCommandViewModel;
            vm.Parameters.Remove(viewModel);
        }
    }
}
