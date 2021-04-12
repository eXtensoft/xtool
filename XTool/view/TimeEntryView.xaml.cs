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
    /// Interaction logic for TimeEntryView.xaml
    /// </summary>
    public partial class TimeEntryView : UserControl
    {
        public TimeEntryView()
        {
            InitializeComponent();
            this.DataContext = TimeKeeperProvider.Instance.ViewModel;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
           // this.DataContext = TimeKeeperProvider.Instance.ViewModel;
        }
    }
}
