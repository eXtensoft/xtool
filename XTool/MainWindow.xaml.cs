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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private OverlayManager _Overlay = null;
        private StateManager _StateManager = null;

        public MainWindow()
        {
            InitializeComponent();
            _StateManager = Application.Current.Properties[AppConstants.StateManager] as StateManager;
            _StateManager.RegisterEndpointAction(ActivityStateOption.Authenticated.ToString(), EndpointOption.Arrival, OnAuthenticated);
            _StateManager.RegisterEndpointAction(ActivityStateOption.Authorized.ToString(), EndpointOption.Arrival, OnAuthorized);
            _StateManager.RegisterEndpointAction(ActivityStateOption.LoggedOff.ToString(), EndpointOption.Arrival, OnLoggedOff);
            _StateManager.RegisterEndpointAction(ActivityStateOption.Unauthorized.ToString(), EndpointOption.Arrival, OnUnauthorized);
            _StateManager.RegisterEndpointAction(ActivityStateOption.Error.ToString(), EndpointOption.Arrival, OnError);
            _StateManager.RegisterEndpointAction(ActivityStateOption.TemplateCommands.ToString(), EndpointOption.Arrival, OnToggleTemplateCommands);
            _StateManager.RegisterEndpointAction(ActivityStateOption.TimeEntry.ToString(), EndpointOption.Arrival, OnToggleTimeEntry);
            _StateManager.RegisterEndpointAction(ActivityStateOption.Logging.ToString(), EndpointOption.Arrival, OnToggleLogging);

            _Overlay = Application.Current.Properties[AppConstants.OverlayManager] as OverlayManager;
            _Overlay.RegisterOverlay(AppConstants.OverlayContent, ShowContentOverlay);
            AddToggleCommands();
        }

        private void AddToggleCommands()
        {
            KeyGesture toggleCommands = new KeyGesture(Key.Q, ModifierKeys.Control);
            KeyBinding toggleCommandsBinding = new KeyBinding(ToggleCommandTemplatesCommand, toggleCommands);
            KeyGesture toggleTimeEntry = new KeyGesture(Key.T, ModifierKeys.Control);
            KeyBinding toggleTimeEntryBinding = new KeyBinding(ToggleTimeEntryCommand, toggleTimeEntry);
            KeyGesture toggleLogging = new KeyGesture(Key.J, ModifierKeys.Control);
            KeyBinding toggleLoggingBinding = new KeyBinding(ToggleLoggingCommand, toggleLogging);
            KeyGesture toggleGen = new KeyGesture(Key.G, ModifierKeys.Control);
            KeyBinding toggleGenBinding = new KeyBinding(ToggleGenCommand, toggleGen);
            this.InputBindings.Add(toggleCommandsBinding);
            this.InputBindings.Add(toggleTimeEntryBinding);
            this.InputBindings.Add(toggleLoggingBinding);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _StateManager.Machine.ExecuteTransition(TransitionTypeOption.Login.ToString());
        }


        private void OnAuthenticated()
        {
            LayoutRoot.Children.Clear();
            LayoutRoot.Children.Add(new AuthorizationView());
        }

        private void OnAuthorized()
        {
            LayoutRoot.Children.Clear();
            LayoutRoot.Children.Add(ShellView);

        }


        private void OnUnauthorized()
        {
            LayoutRoot.Children.Clear();
            LayoutRoot.Children.Add(new UnauthorizedView());
        }

        private void OnError()
        {

            LayoutRoot.Children.Clear();
            LayoutRoot.Children.Add(new ErrorView());
        }

        private void OnLoggedOff()
        {
            //Application.Current.Shutdown();
            //HtmlPage.Window.Navigate(new Uri("http://www.google.com",UriKind.RelativeOrAbsolute));
            // navigate to a configurable web page
            //HtmlPage.Window.Invoke("close");
            LayoutRoot.Children.Clear();
            LayoutRoot.Children.Add(new LogoffView());
        }

        private void LoadGlobalData()
        {
            //throw new NotImplementedException();
        }

        private void RemoveOverlay()
        {
            LayoutRoot.Children.RemoveAt(LayoutRoot.Children.Count - 1);
        }


        private void ShowContentOverlay(dynamic args)
        {
            OverlayView overlay = new OverlayView() { Close = RemoveOverlay };
            overlay.grdOverlay.Children.Add(args.Control);
            overlay.SetTitle((string)args.Title);
            LayoutRoot.Children.Add(overlay);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            StateManager manager = Application.Current.Properties[AppConstants.StateManager] as StateManager;
            if (!manager.CurrentState.Equals(ActivityStateOption.LoggedOff.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                e.Cancel = true;
                manager.Machine.ExecuteTransition(TransitionTypeOption.Logoff.ToString());
            }
        }


        private ICommand _ToggleCommandTemplatesCommand;
        public ICommand ToggleCommandTemplatesCommand
        {
            get
            {
                if (_ToggleCommandTemplatesCommand == null)
                {
                    _ToggleCommandTemplatesCommand = new RelayCommand(
                        param => ToggleTemplateCommands(),
                        param => CanToggleTemplateCommands()
                            );
                }
                return _ToggleCommandTemplatesCommand;
            }
        }

        private ICommand _ToggleGenCommand;
        public ICommand ToggleGenCommand
        {
            get
            {
                if (_ToggleGenCommand == null)
                {
                    _ToggleGenCommand = new RelayCommand(
                        param => ToggleGenCommands(), param => CanToggleGenCommands());
                }
                return _ToggleGenCommand;
            }
        }

        private ICommand _ToggleTimeEntryCommand;
        public ICommand ToggleTimeEntryCommand
        {
            get
            {
                if (_ToggleTimeEntryCommand == null)
                {
                    _ToggleTimeEntryCommand = new RelayCommand(
                        param=>ToggleTimeEntry(),
                        param=>CanToggleTimeEntry()
                        );
                }
                return _ToggleTimeEntryCommand;
            }
        }

        private ICommand _ToggleLoggingCommand;
        public ICommand ToggleLoggingCommand
        {
            get
            {
                if (_ToggleLoggingCommand == null)
                {
                    _ToggleLoggingCommand = new RelayCommand(
                        param => ToggleLogging(), 
                        param => CanToggleLogging());
                }
                return _ToggleLoggingCommand;
            }
        }



        private Shell _ShellView;
        public Shell ShellView
        {
            get
            {
                if (_ShellView == null)
                {
                    LoadGlobalData();
                    _ShellView = new Shell();
                }
                return _ShellView;
            }
        }

        private TemplateCommandsView _TemplateCommands;
        public TemplateCommandsView TemplateCommands
        {
            get
            {
                if (_TemplateCommands == null)
                {
                    _TemplateCommands = new TemplateCommandsView();
                }
                return _TemplateCommands;
            }
        }

        private TimeEntryView _TimeEntrys;
        public TimeEntryView TimeEntrys
        {
            get
            {
                if (_TimeEntrys == null)
                {
                    _TimeEntrys = new TimeEntryView();
                }
                return _TimeEntrys;
            }
        }

        private SqlServerLogView _Logging;
        public SqlServerLogView Logging
        {
            get
            {
                if (_Logging == null)
                {
                    _Logging = new SqlServerLogView();
                }
                return _Logging;
            }
        }

        private bool CanToggleGenCommands()
        {
            return true;
        }

        private void ToggleGenCommands()
        {
            StateManager manager = Application.Current.Properties[AppConstants.StateManager] as StateManager;
            manager.Machine.ExecuteTransition(TransitionTypeOption.ToggleGen.ToString());
        }

        private bool CanToggleTemplateCommands()
        {
            return true;
        }

        private void ToggleTemplateCommands()
        {

            StateManager manager = Application.Current.Properties [AppConstants.StateManager] as StateManager;
            manager.Machine.ExecuteTransition(TransitionTypeOption.ToggleTemplateCommands.ToString());
        }

        private bool CanToggleTimeEntry()
        {
            return true;
        }

        private void ToggleTimeEntry()
        {
            StateManager manager = Application.Current.Properties[AppConstants.StateManager] as StateManager;
            manager.Machine.ExecuteTransition(TransitionTypeOption.ToggleTimeEntry.ToString());
        }

        private bool CanToggleLogging()
        {
            return true;
        }

        private void ToggleLogging()
        {
            StateManager manager = Application.Current.Properties[AppConstants.StateManager] as StateManager;
            manager.Machine.ExecuteTransition(TransitionTypeOption.ToggleLogging.ToString());
        }


        private void OnToggleTemplateCommands()
        {
            LayoutRoot.Children.Clear();
            LayoutRoot.Children.Add(TemplateCommands);        
        }


        private void OnToggleTimeEntry()
        {
            LayoutRoot.Children.Clear();
            LayoutRoot.Children.Add(TimeEntrys);
        }


        private void OnToggleLogging()
        {
            LayoutRoot.Children.Clear();
            LayoutRoot.Children.Add(Logging);
        }




    }
}
