// <copyright file="XToolWorkspaceViewModel.cs" company="eXtensible Solutions LLC">
// Copyright © 2015 All Right Reserved
// </copyright>

namespace XTool
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Dynamic;
    using System.Linq;
    using System.Data.Linq;
    using System.Runtime.Serialization;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Input;


    public sealed class XToolWorkspaceViewModel : ViewModel<XToolWorkspace>
    {
        public XToolWorkspaceViewModel() { }

        private List<XTool.Inference.ZoneTypeOption> _ZoneList = new List<Inference.ZoneTypeOption>();
        private List<ConnectionInfoTypeOption> _ConnectionTypeList = new List<ConnectionInfoTypeOption>();
        private List<string> _TagList = new List<string>();

        public ICollectionView ConnectionsView
        {
            get
            {
                return CollectionViewSource.GetDefaultView(Connections);
            }
        }

        public CyclopsWorkspaceViewModel Logging { get; set; }

        public ObservableCollection<FilterGroup> FilterGroups { get; set; }

        public ObservableCollection<ConnectionInfoViewModel> Connections { get; set; }

        public ObservableCollection<TemplateCommandViewModel> TemplateCommands { get; set; }

        public void GroupTemplateCommands()
        {
           // ConnectionsView.GroupDescriptions.Add(new PropertyGroupDescription("ConnectionType"));
        }

        private bool ExecuteFilter(object item)
        {
            bool b = true;

            ConnectionInfoViewModel info = item as ConnectionInfoViewModel;
            if (info != null)
            {                
                var zoneGroup = FilterGroups.ToList().Find(x => x.Name.Equals("Zone"));
                b = zoneGroup.ResolveFilter(info.Zone.ToString());
                if(b)
                {
                    var connectionTypeGroup = FilterGroups.ToList().Find(x => x.Name.Equals("Type"));
                    b = connectionTypeGroup.ResolveFilter(info.ConnectionType.ToString());
                }
            }

            return b;
        }

        public void GroupConnections(bool isSelected, string name)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(Connections);
            if (!isSelected)
            {
                bool b = false;
                for (int i = 0;!b && i < view.GroupDescriptions.Count; i++)
                {
                    var pgd = (PropertyGroupDescription)view.GroupDescriptions[i];
                    b = name.Equals(pgd.PropertyName);
                    if (b)
                    {
                        view.GroupDescriptions.RemoveAt(i);
                    }
                }
            }
            else if (!String.IsNullOrWhiteSpace(name))
            {
                view.GroupDescriptions.Add(new PropertyGroupDescription(name));
            }

        }

        #region SelectedConnection (ConnectionInfoViewModel)

        private ConnectionInfoViewModel _SelectedConnection;
        /// <summary>
        /// Gets or sets the ConnectionInfoViewModel value for SelectedConnection
        /// </summary>
        /// <value> The ConnectionInfoViewModel value.</value>

        public ConnectionInfoViewModel SelectedConnection
        {
            get { return _SelectedConnection; }
            set
            {
                if (_SelectedConnection != value)
                {
                    _SelectedConnection = value;
                    OnPropertyChanged("SelectedConnection");

                }
            }
        }

        #endregion

        private ICommand _SaveWorkspaceCommand;
        public ICommand SaveWorkspaceCommand
        {
            get
            {
                if (_SaveWorkspaceCommand == null)
                {
                    _SaveWorkspaceCommand = new RelayCommand(
                        param => WorkspaceProvider.Instance.Save(),
                        param => WorkspaceProvider.Instance.CanSave());
                }
                return _SaveWorkspaceCommand;
            }
        }

        private ICommand _AddConnectionInfoCommand;
        public ICommand AddConnectionInfoCommand
        {
            get
            {
                if (_AddConnectionInfoCommand == null)
                {
                    _AddConnectionInfoCommand = new RelayCommand(
                        param => AddConnectionInfo(),
                        param => CanAddConnectionInfo());
                }
                return _AddConnectionInfoCommand;
            }
        }

        private bool CanAddConnectionInfo()
        {
            return WorkspaceProvider.Instance.CanAddConnectionInfo();
        }

        private void AddConnectionInfo()
        {
            WorkspaceProvider.Instance.AddConnectionInfo();
            ConnectionsView.Refresh();
        }


        #region connection info viewmodel factories
        private static IDictionary<Type, Func<ConnectionInfo, ConnectionInfoViewModel>> factories = new Dictionary<Type, Func<ConnectionInfo, ConnectionInfoViewModel>>()
        {
            {typeof(ConnectionInfo),GenerateConnectionInfo},
            {typeof(XTool.Inference.SqlServerConnectionInfo),GenerateSqlServer},
            {typeof(MongoDbConnectionInfo),GenerateMongoDb},
            {typeof(RedisConnectionInfo),GenerateRedis},
            {typeof(MySqlConnectionInfo),GenerateMySql},
            {typeof(FileConnectionInfo),GenerateFile},
            {typeof(XmlConnectionInfo),GenerateXml},
            {typeof(JsonConnectionInfo),GenerateJson},
            {typeof(ExcelConnectionInfo),GenerateExcel},
            {typeof(ApiConnectionInfo),GenerateApi },
        };

        private static ConnectionInfoViewModel GenerateConnectionInfo(ConnectionInfo info)
        {
            return new ConnectionInfoViewModel(info);
        }
        private static ConnectionInfoViewModel GenerateSqlServer(ConnectionInfo info)
        {
            return new SqlServerConnectionInfoViewModel( info as XTool.Inference.SqlServerConnectionInfo);
        }
        private static ConnectionInfoViewModel GenerateMongoDb(ConnectionInfo info)
        {
            return new MongoDbConnectionInfoViewModel(info as MongoDbConnectionInfo);
        }
        private static ConnectionInfoViewModel GenerateRedis(ConnectionInfo info)
        {
            return new RedisConnectionInfoViewModel(info as RedisConnectionInfo);
        }
        private static ConnectionInfoViewModel GenerateMySql(ConnectionInfo info)
        {
            return new MySqlConnectionInfoViewModel(info as MySqlConnectionInfo);
        }
        private static ApiConnectionInfoViewModel GenerateApi(ConnectionInfo info)
        {
            return new ApiConnectionInfoViewModel(info as ApiConnectionInfo);
        }
        private static ConnectionInfoViewModel GenerateFile(ConnectionInfo info)
        {
            return new FileConnectionInfoViewModel(info as FileConnectionInfo);
        }

        private static ConnectionInfoViewModel GenerateXml(ConnectionInfo info)
        {
            return new XmlConnectionInfoViewModel(info as XmlConnectionInfo);
        }

        private static ConnectionInfoViewModel GenerateJson(ConnectionInfo info)
        {
            return new JsonConnectionInfoViewModel(info as JsonConnectionInfo);
        }

        private static ConnectionInfoViewModel GenerateExcel(ConnectionInfo info)
        {
            return new ExcelConnectionInfoViewModel(info as ExcelConnectionInfo);
        }




        #endregion


        private ICommand _AddCommandTemplateCommand;
        public ICommand AddCommandTemplateCommand
        {
            get
            {
                if (_AddCommandTemplateCommand == null)
                {
                    _AddCommandTemplateCommand = new RelayCommand(param => AddCommandTemplate());
                }
                return _AddCommandTemplateCommand;
            }
        }

        private void AddCommandTemplate()
        {
            CommandTemplate template = new CommandTemplate() { Id = Guid.NewGuid().ToString(), Name = "commandName", ConnectionType = ConnectionInfoTypeOption.None, Type = TemplateTypeOption.Instance };
            TemplateCommandViewModel vm = new TemplateCommandViewModel(template);
            TemplateCommands.Add(vm);
        }


        public XToolWorkspaceViewModel(XToolWorkspace model)
        {
            if (model.Logging == null)
            {
                model.Logging = new Cyclops.Workspace() { Connections = new List<Cyclops.CyclopsConnection>() };
                model.Logging.Connections.Add(new Cyclops.CyclopsConnection() { Title = "Local", ConnectionString = "Data Source=(local);Initial Catalog=dplog;Integrated Security=True" });
                model.Logging.Connections.Add(new Cyclops.CyclopsConnection() { Title = "UAT", ConnectionString = "Data Source=107.23.84.86;Initial Catalog=dplog;User ID=OCD;Password=OCD_03d" });
                model.Logging.Connections.Add(new Cyclops.CyclopsConnection() { Title = "QA", ConnectionString = "Data Source=54.208.69.124;Initial Catalog=dplog;User ID=OCD;Password=OCD_03d" });
                model.Logging.Connections.Add(new Cyclops.CyclopsConnection() { Title = "Preview", ConnectionString = "Data Source=184.106.32.151;Initial Catalog=dplog;User ID=dp_user;Password=abc123!@#" });
            }
            Model = model;

            Logging = new CyclopsWorkspaceViewModel(model.Logging);
            if (model.Connections != null)
            {
                List<ConnectionInfoViewModel> list = BuildList(model.Connections);
                Connections = new ObservableCollection<ConnectionInfoViewModel>(list);
            }
            //GroupConnections();
            Connections.CollectionChanged += Connections_CollectionChanged;


            TemplateCommands = new ObservableCollection<TemplateCommandViewModel>();
            if (model.TemplateCommands != null)
            {
                
                foreach (CommandTemplate item in model.TemplateCommands)
                {
                    switch (item.ConnectionType)
                    {
                        case ConnectionInfoTypeOption.None:
                            break;
                        case ConnectionInfoTypeOption.SqlServer:
                            TemplateCommands.Add(new SqlServerTemplateCommandViewModel(item as SqlServerCommandTemplate));
                            break;
                        case ConnectionInfoTypeOption.MongoDb:
                            TemplateCommands.Add(new MongoDbTemplateCommandViewModel(item as MongoDbCommandTemplate));
                            break;
                        case ConnectionInfoTypeOption.Redis:
                            break;
                        case ConnectionInfoTypeOption.MySql:
                            TemplateCommands.Add(new MySqlTemplateCommandViewModel(item as MySqlCommandTemplate));
                            break;
                        case ConnectionInfoTypeOption.Neo4j:
                            break;
                        case ConnectionInfoTypeOption.File:
                            break;
                        case ConnectionInfoTypeOption.Excel:
                            break;
                        case ConnectionInfoTypeOption.Xml:
                            break;
                        case ConnectionInfoTypeOption.Json:
                            break;
                        case ConnectionInfoTypeOption.Api:
                            break;
                        default:
                            break;
                    }
                }
            }
            //GroupTemplateCommands();

            TemplateCommands.CollectionChanged += TemplateCommands_CollectionChanged;

            foreach (var option in Enum.GetValues(typeof(XTool.Inference.ZoneTypeOption)).Cast<XTool.Inference.ZoneTypeOption>())
            {
                _ZoneList.Add(option);
            }

            foreach (var option in Enum.GetValues(typeof(ConnectionInfoTypeOption)).Cast<ConnectionInfoTypeOption>())
            {
                _ConnectionTypeList.Add(option);
            }



            List<FilterGroup> filterList = new List<FilterGroup>();
            //filterList.Add(new NoFilterGroup());
            filterList.Add(new ConnectionTypeFilterGroup(_ConnectionTypeList, FilterChanged) { HandleSelect = GroupingChanged });
            filterList.Add(new ZoneFilterGroup(_ZoneList, FilterChanged) { HandleSelect = GroupingChanged });
            filterList.Add(new TagFilterGroup(_TagList) { HandleSelect = GroupingChanged });

            FilterGroups = new ObservableCollection<FilterGroup>(filterList);
            //BuildFilterGroups();
            GroupTemplateCommands();
            ConnectionsView.Filter = ExecuteFilter;
        }

        private void BuildFilterGroups()
        {
            _ZoneList.Clear();
            _ConnectionTypeList.Clear();
            _TagList.Clear();

            HashSet<XTool.Inference.ZoneTypeOption> hsZones = new HashSet<XTool.Inference.ZoneTypeOption>();
            HashSet<ConnectionInfoTypeOption> hsTypes = new HashSet<ConnectionInfoTypeOption>();
            HashSet<string> hsTags = new HashSet<string>();

            foreach (var connectionInfo in Model.Connections)
            {
                var zone = connectionInfo.Zone;
                if (hsZones.Add(zone))
                {
                    _ZoneList.Add(zone);
                }
                var type = connectionInfo.ConnectionType;
                if (hsTypes.Add(type))
                {
                    _ConnectionTypeList.Add(type);
                }
                if (connectionInfo.Tags != null && connectionInfo.Tags.Count > 0)
                {
                    foreach (var tag in connectionInfo.Tags)
                    {
                        if (hsTags.Add(tag))
                        {
                            _TagList.Add(tag);
                        }
                    }
                }
            }

            List<FilterGroup> filterList = new List<FilterGroup>();
            //filterList.Add(new NoFilterGroup());
            filterList.Add(new ConnectionTypeFilterGroup(_ConnectionTypeList, FilterChanged) { HandleSelect = GroupingChanged });
            filterList.Add(new ZoneFilterGroup(_ZoneList, FilterChanged) { HandleSelect = GroupingChanged });
            filterList.Add(new TagFilterGroup(_TagList) { HandleSelect = GroupingChanged });

            FilterGroups = new ObservableCollection<FilterGroup>(filterList);
        }

        private void FilterChanged()
        {
            ConnectionsView.Refresh();
        }
        private void GroupingChanged(bool isSelected, string name)
        {
            if (true)
            {
                GroupConnections(isSelected,name);
            }
        }

        private void TemplateCommands_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    var vm = item as TemplateCommandViewModel;
                    if (item != null)
                    {
                        Model.TemplateCommands.Add(vm.Model);
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems)
                {
                    var vm = item as TemplateCommandViewModel;
                    if (item != null)
                    {
                        Model.TemplateCommands.Remove(vm.Model);
                    }
                }
            }
        }

        private List<ConnectionInfoViewModel> BuildList(List<ConnectionInfo> list)
        {
            List<ConnectionInfoViewModel> viewModels = new List<ConnectionInfoViewModel>();
            foreach (var item in list)
            {
                Type type = item.GetType();
                var vm = factories[type].Invoke(item);
                viewModels.Add(vm);
            }
            return viewModels;
        }

        void Connections_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    ConnectionInfoViewModel vm = item as ConnectionInfoViewModel;
                    if (vm != null)
                    {
                        Model.Connections.Add(vm.Model);
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems)
                {
                    ConnectionInfoViewModel vm = item as ConnectionInfoViewModel;
                    if (vm != null)
                    {
                        Model.Connections.Remove(vm.Model);
                    }
                }                
            }
            
        }


        #region icommand implementations



        #endregion

        private ICommand _ImportConnectionsCommand;
        public ICommand ImportConnectionsCommand
        {
            get
            {
                if (_ImportConnectionsCommand == null)
                {
                    _ImportConnectionsCommand = new RelayCommand(
                        param => ImportConnections(),
                        param => CanImportConnections());
                }
                return _ImportConnectionsCommand;
            }
        }
        private bool CanImportConnections()
        {
            return true;
        }
        private void ImportConnections()
        {
            dynamic param = new ExpandoObject();
            param.Title = "Add Connections";
            param.Control = new ConnectionStringsImportView() { DataContext = Connections };
            OverlayManager manager = Application.Current.Properties[AppConstants.OverlayManager] as OverlayManager;
            manager.SetOverlay(AppConstants.OverlayContent, param);
        }

    }


}
