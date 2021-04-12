using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Xml.Linq;

namespace XTool
{
    /// <summary>
    /// Interaction logic for ConnectionStringsImportView.xaml
    /// </summary>
    public partial class ConnectionStringsImportView : UserControl, INotifyPropertyChanged
    {
        public static IDictionary<ConnectionInfoTypeOption, Func<string,string,ConnectionInfoViewModel>> Factories = new Dictionary<ConnectionInfoTypeOption, Func<string,string, ConnectionInfoViewModel>>
        {
            {ConnectionInfoTypeOption.None,GenerateNone},
            {ConnectionInfoTypeOption.Api,GenerateApi },
            {ConnectionInfoTypeOption.SqlServer,GenerateSqlServer},
            {ConnectionInfoTypeOption.MongoDb,GenerateMongoDb},
            {ConnectionInfoTypeOption.Redis,GenerateRedis},
            {ConnectionInfoTypeOption.MySql,GenerateMySql},
            {ConnectionInfoTypeOption.Neo4j,GenerateNeo4j},
            {ConnectionInfoTypeOption.File,GenerateFile},
            {ConnectionInfoTypeOption.Excel,GenerateExcel},
            {ConnectionInfoTypeOption.Xml,GenerateXml},
            {ConnectionInfoTypeOption.Json,GenerateJson},


        };

        private ICommand _ParseCommand;
        public ICommand ParseCommand
        {
            get
            {
                if (_ParseCommand == null)
                {
                    _ParseCommand = new RelayCommand(
                        param => Parse(),
                        param => CanParse());
                }
                return _ParseCommand;
            }
        }

        private string _Candidates = String.Empty;
        public string Candidates
        {
            get { return _Candidates; }
            set
            {
                if (_Candidates != value)
                {
                    _Candidates = value;
                    OnPropertyChanged("Candidates");
                }
            }
        }
        public ConnectionStringsImportView()
        {
            InitializeComponent();
        }


        private bool CanParse()
        {
            return true;// !String.IsNullOrEmpty(Candidates);
        }
        private void Parse()
        {
            try
            {
                ObservableCollection<ConnectionInfoViewModel> connections = this.DataContext as ObservableCollection<ConnectionInfoViewModel>;
                if (connections != null)
                {
                    XDocument xdoc = XDocument.Parse("<root>" + Candidates + "</root>");
                    foreach (var item in xdoc.Descendants("add"))
                    {
                        string name = item.Attribute("name").Value;
                        string cn = item.Attribute("connectionString").Value;
                        string provider = item.Attribute("providerName").Value;
                        if (!String.IsNullOrWhiteSpace(name) && !String.IsNullOrWhiteSpace(cn) && AppConstants.ProviderNames.ContainsKey(provider) && Factories.ContainsKey(AppConstants.ProviderNames[provider]))
                        {
                            ConnectionInfoViewModel vm = Factories[AppConstants.ProviderNames[provider]].Invoke(name,cn);
                            if (vm != null)
                            {
                                connections.Add(vm);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }


        private static ConnectionInfoViewModel GenerateNone(string key, string connectionString)
        {
            ConnectionInfoViewModel vm = null;

            return vm;
        }

        private static ConnectionInfoViewModel GenerateApi(string key, string connectionString)
        {
            ApiConnectionInfo model = new ApiConnectionInfo()
            {
                Name = key,
                ConnectionType = ConnectionInfoTypeOption.Api,
                ProviderName = AppConstants.Providers[ConnectionInfoTypeOption.Api],
                Text = connectionString
            };

            ApiConnectionInfoViewModel vm = new ApiConnectionInfoViewModel(model);

            return vm;
        }

        private static ConnectionInfoViewModel GenerateSqlServer(string key, string connectionString)
        {
            Dictionary<string, string> kvps = ParseKvps(connectionString);
            

            Inference.SqlServerConnectionInfo model = new Inference.SqlServerConnectionInfo()
            {
                Name = key,
                ConnectionType = ConnectionInfoTypeOption.SqlServer,
                ProviderName = AppConstants.Providers[ConnectionInfoTypeOption.SqlServer],
            };
            if (kvps.ContainsKey("initial catalog"))
            {
                model.Catalog = kvps["initial catalog"].Trim();
            }
            if (kvps.ContainsKey("data source"))
            {
                model.Server = kvps["data source"].Trim();
            }
            if (kvps.ContainsKey("integrated security") && kvps["integrated security"].Equals("true",StringComparison.OrdinalIgnoreCase))
            {
                model.IntegratedSecurity = true;
            }
            if (kvps.ContainsKey("user id"))
            {
                model.User = kvps["user id"].Trim();
            }
            if (kvps.ContainsKey("password"))
            {
                model.Pwd = kvps["password"].Trim();
            }
            if (kvps.ContainsKey("timeout"))
            {

            }

            SqlServerConnectionInfoViewModel vm = new SqlServerConnectionInfoViewModel(model);
            vm.BuildConnectionString();
            return vm;
        }
        private static ConnectionInfoViewModel GenerateMySql(string key, string connectionString)
        {
            Dictionary<string, string> kvps = ParseKvps(connectionString);

            MySqlConnectionInfo model = new MySqlConnectionInfo()
            {
                Name = key,
                ConnectionType = ConnectionInfoTypeOption.MySql,
                ProviderName = AppConstants.Providers[ConnectionInfoTypeOption.MySql]
            };
            if (kvps.ContainsKey("server"))
            {
                model.Server = kvps["server"].Trim();
            }
            if (kvps.ContainsKey("database"))
            {
                model.Catalog = kvps["database"].Trim();
            }
            if (kvps.ContainsKey("uid"))
            {
                model.User = kvps["uid"].Trim();
            }
            if (kvps.ContainsKey("pwd"))
            {
                model.Pwd = kvps["pwd"].Trim();
            }
            ConnectionInfoViewModel vm = new MySqlConnectionInfoViewModel(model);
            vm.BuildConnectionString();
            return vm;
        }

        private static ConnectionInfoViewModel GenerateMongoDb(string key, string connectionString)
        {
            string[] t = connectionString.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            //string s = connectionString.Substring(connectionString.IndexOf("//") + 2);
            string server = t[1];
            MongoDbConnectionInfo model = new MongoDbConnectionInfo()
            {
                Name = key,
                ConnectionType = ConnectionInfoTypeOption.MongoDb,
                ProviderName = AppConstants.Providers[ConnectionInfoTypeOption.MongoDb],
                Server = server,
                Port = 27017,
            };
           
            ConnectionInfoViewModel vm = new MongoDbConnectionInfoViewModel(model);
            vm.BuildConnectionString();
            return vm;
        }
        private static ConnectionInfoViewModel GenerateRedis(string key, string connectionString)
        {
            RedisConnectionInfo model = new RedisConnectionInfo()
            {
                Name = key,
                ConnectionType = ConnectionInfoTypeOption.Redis,
                ProviderName = AppConstants.Providers[ConnectionInfoTypeOption.Redis],
                Server = connectionString
            };
            ConnectionInfoViewModel vm = new RedisConnectionInfoViewModel(model);
            vm.BuildConnectionString();
            return vm;
        }

        private static ConnectionInfoViewModel GenerateNeo4j(string key, string connectionString)
        {
            ConnectionInfoViewModel vm = null;

            return vm;
        }
        private static ConnectionInfoViewModel GenerateFile(string key, string connectionString)
        {
            ConnectionInfoViewModel vm = null;

            return vm;
        }
        private static ConnectionInfoViewModel GenerateExcel(string key, string connectionString)
        {
            ConnectionInfoViewModel vm = null;

            return vm;
        }
        private static ConnectionInfoViewModel GenerateXml(string key, string connectionString)
        {
            ConnectionInfoViewModel vm = null;

            return vm;
        }
        private static ConnectionInfoViewModel GenerateJson(string key, string connectionString)
        {
            ConnectionInfoViewModel vm = null;

            return vm;
        }

        private static Dictionary<string, string> ParseKvps(string connectionString)
        {
            HashSet<string> hs = new HashSet<string>();
            Dictionary<string, string> d = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            if (!String.IsNullOrWhiteSpace(connectionString))
            {
                string[] t = connectionString.Trim().Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string s in t)
                {
                    string[] kvp = s.Trim().Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                    if (kvp.Length == 2 && hs.Add(kvp[0].Trim()))
                    {
                        d.Add(kvp[0].Trim(), kvp[1].Trim());
                    }
                }
            }

            return d;
        }


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion


    }
}
