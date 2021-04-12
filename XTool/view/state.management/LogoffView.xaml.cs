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
    /// Interaction logic for LogoffView.xaml
    /// </summary>
    public partial class LogoffView : UserControl
    {
        System.Timers.Timer _Timer = null;
       
        bool _IsCandidateLoaded = false;
        string _Candidate = String.Empty;


        public LogoffView()
        {
            InitializeComponent();

            _Candidate = Application.Current.Properties[AppConstants.Candidate] as string;
            _Timer = new System.Timers.Timer(1000.00);
            _Timer.Elapsed += new System.Timers.ElapsedEventHandler(Timer_Elapsed);
            _Timer.Enabled = true;
        }


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(_Candidate))
            {
                _IsCandidateLoaded = true;
            }
            else
            {
                txbSubheader.Text = "See you next time!";
            }
            _Timer.Start();
        }

        void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _Timer.Stop();

            Dispatcher.Invoke((Action)(() =>
            {
                if (_IsCandidateLoaded)
                {
                    //DevToolWorkspace workspace = WorkspaceProvider.Instance.ViewModel.Model;
                    //GenericSerializer.WriteGenericList<Inference.SqlConnectionInfo>(workspace.Connections, _Candidate);
                }
                Application.Current.Shutdown();
            }));


        }
    }
}
