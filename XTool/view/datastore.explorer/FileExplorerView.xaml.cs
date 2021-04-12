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
    /// Interaction logic for FileExplorerView.xaml
    /// </summary>
    public partial class FileExplorerView : UserControl
    {
        public FileExplorerView()
        {
            InitializeComponent();
        }

        private void trvItems_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            DataSetViewModel vm = DataContext as DataSetViewModel;
            grdItem.Children.Clear();
            object o = e.NewValue;
            if (o != null)
            {
                DataTableViewModel tbl = o as DataTableViewModel;
                DataColumnViewModel col = o as DataColumnViewModel;
                if (col != null)
                {
                    DataColumnDetailView ctl = new DataColumnDetailView() { DataContext = col };
                    grdItem.Children.Add(ctl);
                }
                else if (tbl != null)
                {
                    DataTableDetailView ctl = new DataTableDetailView() { DataContext = tbl };
                    grdItem.Children.Add(ctl);
                }
                //transformView.DataContext = o;
            }
        }
    }
}
