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
    /// Interaction logic for OverlayView.xaml
    /// </summary>
    public partial class OverlayView : UserControl
    {
        private Visibility _CloseButtonVisibility = Visibility.Visible;
        public Visibility CloseButtonVisibility
        {
            get
            {
                return _CloseButtonVisibility;
            }
            set { _CloseButtonVisibility = value; }
        }

        public Action Close { get; set; }

        public OverlayView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Close != null)
            {
                Close.Invoke();
            }
        }

        public void SetTitle(string title)
        {
            txbTitle.Text = title;
            txbTitle.Visibility = System.Windows.Visibility.Visible;
        }

    }
}
