using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for SqlTableView.xaml
    /// </summary>
    public partial class SqlTableView : UserControl
    {
        private ICommand _CopyToClipboardCommand;
        public ICommand CopyToClipboardCommand
        {
            get
            {
                if (_CopyToClipboardCommand == null)
                {
                    _CopyToClipboardCommand = new RelayCommand(
                        param => CopyToClipboard());
                }
                return _CopyToClipboardCommand;
            }
        }

        DataTable datasource = null;

        public SqlTableView()
        {
            InitializeComponent();
        }

        private void CopyToClipboard()
        {
            grdItems.SelectAllCells();
            grdItems.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
            ApplicationCommands.Copy.Execute(null, grdItems);
            grdItems.UnselectAllCells();
        }

        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            SqlTableViewModel vm = DataContext as SqlTableViewModel;
            if (vm != null)
            {
                datasource = vm.DataDictionary;
            }
            grdItems.DataContext = datasource;
        }
    }
}
