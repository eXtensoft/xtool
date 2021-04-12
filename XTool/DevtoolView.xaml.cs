using MongoDB.Driver;
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
    /// Interaction logic for DevtoolView.xaml
    /// </summary>
    public partial class DevtoolView : UserControl
    {
        public DevtoolView()
        {
            InitializeComponent();

        }

        private ICommand _AddTextCommand;
        public ICommand AddTextCommand
        {
            get
            {
                if (_AddTextCommand == null)
                {
                    _AddTextCommand = new RelayCommand(param => AddText());
                }
                return _AddTextCommand;
            }
        }
        private void AddText()
        {
            int j = 0;
        }


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = WorkspaceProvider.Instance.ViewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //MongoClient client = new MongoClient("");
            //client.GetServer();
            //MongoServer server = MongoServer.Create();
            //server.Connect();
            //var list = server.GetDatabaseNames();
            //foreach (var item in list)
            //{
            //    MessageBox.Show(item);
            //}
        }
    }
}
