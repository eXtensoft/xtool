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
    /// Interaction logic for ApiEndpointImportView.xaml
    /// </summary>
    public partial class ApiEndpointImportView : UserControl
    {
        List<ApiEndpointViewModel> _endpoints = new List<ApiEndpointViewModel>();
        public ApiEndpointImportView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            txbRawInput.Text = XTool.Resources.registration;
        }
    }
}
