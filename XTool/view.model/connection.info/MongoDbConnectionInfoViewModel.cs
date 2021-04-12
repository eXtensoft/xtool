using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace XTool
{

    public class MongoDbConnectionInfoViewModel : ConnectionInfoViewModel
    {


        #region local fields

        #endregion


        public new MongoDbConnectionInfo Model { get; set; }

        private string _Version;

        public string Version
        {
            get { return _Version; }
            set
            {
                _Version = value;
                OnPropertyChanged("Version");
            }
        }

        #region Port (int)

        /// <summary>
        /// Gets or sets the int value for Port
        /// </summary>
        /// <value> The int value.</value>

        public int Port
        {
            get { return Model.Port; }
            set
            {
                if (Model.Port != value)
                {
                    Model.Port = value;
                    OnPropertyChanged("Port");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region Username (string)

        /// <summary>
        /// Gets or sets the string value for Username
        /// </summary>
        /// <value> The string value.</value>

        public string Username
        {
            get { return (String.IsNullOrEmpty(Model.Username)) ? String.Empty : Model.Username; }
            set
            {
                if (Model.Username != value)
                {
                    Model.Username = value;
                    OnPropertyChanged("Username");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region Password (string)

        /// <summary>
        /// Gets or sets the string value for Password
        /// </summary>
        /// <value> The string value.</value>

        public string Password
        {
            get { return (String.IsNullOrEmpty(Model.Password)) ? String.Empty : Model.Password; }
            set
            {
                if (Model.Password != value)
                {
                    Model.Password = value;
                    OnPropertyChanged("Password");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region Databases (string)

        /// <summary>
        /// Gets or sets the string value for Databases
        /// </summary>
        /// <value> The string value.</value>

        public string DatabaseNames
        {
            get 
            {
                if (Model.Databases == null || Model.Databases.Length == 0)
                {
                    return String.Empty;
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < Model.Databases.Length; i++)
                    {
                        if (i > 0)
                        {
                            sb.Append(",");
                        }
                        sb.Append(Model.Databases[i]);
                    }
                    return sb.ToString();
                }
            }
            set
            {

                if (!String.IsNullOrWhiteSpace(value))
                {
                    Model.Databases = value.Split(new char[] { ',', ' ', ';' }, StringSplitOptions.RemoveEmptyEntries);
                    OnPropertyChanged("Databases");
                    IsDirty = true;
                }

            }
        }

        #endregion

        #region Databases (ObservableCollection<DatabaseViewModel>)

        private ObservableCollection<DatabaseViewModel> _Databases = new ObservableCollection<DatabaseViewModel>();

        /// <summary>
        /// Gets or sets the ObservableCollection<DatabaseViewModel> value for Databases
        /// </summary>
        /// <value> The ObservableCollection<DatabaseViewModel> value.</value>

        public ObservableCollection<DatabaseViewModel> Databases
        {
            get { return _Databases; }
            set
            {
                if (_Databases != value)
                {
                    _Databases = value;
                }
            }
        }

        #endregion



        public MongoDbConnectionInfoViewModel(MongoDbConnectionInfo model)
        {
            base.Model = model;
            Model = model;

        }

        public override void BuildConnectionString()
        {
            Build();
        }

        protected override void TestConnection()
        {
            IsValidated = false;
            Version = String.Empty;
            if (TryBuild())
            {
                try
                {
                    var client = new MongoClient(Text);
                    //client.
                    //var server = client.GetServer();
                    //server.Connect();
                    //var info = server.BuildInfo;
                    //if (info != null)
                    //{
                    //    Version = String.Format("v{0} ({1}bit)", info.VersionString, info.Bits);
                    //    IsValidated = true;
                    //}
                    IsValidated = true;
                }
                catch (Exception ex)
                {
                    var message = new Message() { Start = DateTime.Now, Timespan = 6000, Extract = "Connection attempt failed", Text = new List<string>() };
                    message.Text.Add(ex.Message);
                    WorkspaceProvider.Instance.Messages.Push(message);
                }
            }

        }

        private bool TryBuild()
        {
            bool b = false;
            if (IsValidConnectionString())
            {
                Build();
                b = true;
            }
            return b;
        }

        private void Build()
        {
            IsValidated = false;
            StringBuilder sb = new StringBuilder();

            if (!String.IsNullOrWhiteSpace(Username) && !String.IsNullOrWhiteSpace(Password))
            {
                sb.AppendFormat(@"mongodb://{0}:{1}@{2}:{3}", Username, Password, Server, Port);
            }
            else
            {
                sb.AppendFormat(@"mongodb://{0}:{1}", Server, Port);
            }
            
            Text = sb.ToString();
        }

        private bool IsValidConnectionString()
        {
            bool b = true;
            b = b ? !String.IsNullOrWhiteSpace(base.Model.Server) : false;
            b = b ? Port > 0 : false;
            return b;
        }

        protected override bool CanExecuteDiscovery()
        {
            return IsValidated;
        }

        protected override void ExecuteDiscovery()
        {
            XTool.Mongo.MongoExplorer explorer = new Mongo.MongoExplorer(Text);
            explorer.Discover();


            List<GroupingViewModel> groups = new List<GroupingViewModel>();
            DatabaseGroupingViewModel databases = new DatabaseGroupingViewModel();

            foreach (var item in explorer.Databases)
            {
                DatabaseViewModel vm = new DatabaseViewModel(item,Text, Model.Commands);

                Databases.Add(vm);
            }
        }

    }





}
