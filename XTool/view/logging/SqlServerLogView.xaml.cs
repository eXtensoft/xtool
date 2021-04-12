using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for SqlServerLogView.xaml
    /// </summary>
    public partial class SqlServerLogView : UserControl
    {
        // https://joshsmithonwpf.wordpress.com/2007/02/24/stretching-content-in-an-expander-header/

        public CyclopsWorkspaceViewModel Logging { get; set; }

        public SqlServerLogView()
        {
            InitializeComponent();

            Logging = WorkspaceProvider.Instance.ViewModel.Logging;

        }

        public void btnCloseOverlay_Click(object sender, RoutedEventArgs e)
        {
            grdContent.Children.Clear();
            grdOverlay.Visibility = System.Windows.Visibility.Collapsed;
        }

        private ICommand _ViewSchemaStatsCommand;
        public ICommand ViewSchemaStatsCommand
        {
            get
            {
                if (_ViewSchemaStatsCommand == null)
                {
                    _ViewSchemaStatsCommand = new RelayCommand(param => ViewSchemaStats(),
                        param=>CanViewSchemaStats());
                }
                return _ViewSchemaStatsCommand;
            }
        }


        private bool CanViewSchemaStats()
        {
            return Logging.Selected != null && !String.IsNullOrWhiteSpace(Logging.Selected.ConnectionString);
        }

        private void ViewSchemaStats()
        {
            XTool.Cyclops.LogStats stats;
            if (XTool.Cyclops.LogStatsProvider.TryGetStatisitics(Logging.Selected.ConnectionString, out stats))
            {
                UserControl overlaycontrol = new StatsView();
                overlaycontrol.DataContext = stats;
                grdContent.Children.Add(overlaycontrol);
                grdOverlay.Visibility = Visibility.Visible;

                // pass data to overlay;

            }
            else
            {
                MessageBox.Show(stats.Message);
            }
        }
    }
}
