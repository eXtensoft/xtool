using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for DatastoreView.xaml
    /// </summary>
    public partial class DatastoreView : UserControl
    {
        public DatastoreView()
        {
            InitializeComponent();
            //Cvs.Source = XTool.WorkspaceProvider.Instance.ViewModel.Connections;
            //GroupBy();
            DataContext = XTool.WorkspaceProvider.Instance.ViewModel;
            //GroupBy();
        }

        public CollectionViewSource Cvs
        {
            get
            {
                return Resources["cvsConnectionInfo"] as CollectionViewSource;
            }
        }

        private void GroupBy()
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(XTool.WorkspaceProvider.Instance.ViewModel.Connections);
            view.GroupDescriptions.Add(new PropertyGroupDescription("Zone"));
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var a = e.OriginalSource;
        }
    }
}
