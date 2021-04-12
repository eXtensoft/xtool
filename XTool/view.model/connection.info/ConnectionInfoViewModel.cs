// <copyright file="ConnectionInfoViewModel.cs" company="eXtensible Solutions LLC">
// Copyright © 2015 All Right Reserved
// </copyright>

namespace XTool
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Windows;
    using System.Windows.Input;

    [KnownType(typeof(MySqlConnectionInfoViewModel))]
    [KnownType(typeof(SqlServerConnectionInfoViewModel))]
    public class ConnectionInfoViewModel : ViewModel<ConnectionInfo>
    {

        #region Explorer (SqlExplorer)

        private SqlExplorer _Explorer;

        /// <summary>
        /// Gets or sets the SqlExplorer value for Explorer
        /// </summary>
        /// <value> The SqlExplorer value.</value>

        public SqlExplorer Explorer
        {
            get { return _Explorer; }
            set
            {
                if (_Explorer != value)
                {
                    _Explorer = value;
                }
            }
        }

        #endregion

        #region ConnectionType (ConnectionInfoTypeOption)


        /// <summary>
        /// Gets or sets the ConnectionInfoTypeOption value for ConnectionType
        /// </summary>
        /// <value> The ConnectionInfoTypeOption value.</value>

        public ConnectionInfoTypeOption ConnectionType
        {
            get { return Model.ConnectionType; }
            set
            {
                if (Model.ConnectionType != value)
                {
                    Model.ConnectionType = value;
                    OnPropertyChanged("ConnectionType");
                    IsDirty = true;
                }
            }
        }

        #endregion


        #region Datastore booleans

        public bool IsNone
        {
            get { return Model.ConnectionType.Equals(ConnectionInfoTypeOption.None);}
            set
            {
                if (value)
                {
                    Model.ConnectionType = ConnectionInfoTypeOption.None;
                    ProviderName = AppConstants.Providers[ConnectionInfoTypeOption.None];
                    DatastoreChanged();
                    WorkspaceProvider.Instance.ConvertTo(this, ConnectionInfoTypeOption.None);
                }
            }
        }

        public bool IsApi
        {
            get { return Model.ConnectionType.Equals(ConnectionInfoTypeOption.Api); }
            set
            {
                if (value)
                {
                    Model.ConnectionType = ConnectionInfoTypeOption.Api;
                    ProviderName = AppConstants.Providers[ConnectionInfoTypeOption.Api];
                    DatastoreChanged();
                    WorkspaceProvider.Instance.ConvertTo(this, ConnectionInfoTypeOption.Api);
                }
            }
        }

        public bool IsSqlServer
        {
            get { return Model.ConnectionType.Equals(ConnectionInfoTypeOption.SqlServer); }
            set
            {
                if (value)
                {
                    Model.ConnectionType = ConnectionInfoTypeOption.SqlServer;
                    ProviderName = AppConstants.Providers[ConnectionInfoTypeOption.SqlServer];
                    DatastoreChanged();
                    WorkspaceProvider.Instance.ConvertTo(this, ConnectionInfoTypeOption.SqlServer);
                }
            }
        }

        public bool IsMongoDb
        {
            get { return Model.ConnectionType.Equals(ConnectionInfoTypeOption.MongoDb); }
            set
            {
                if (value)
                {
                    Model.ConnectionType = ConnectionInfoTypeOption.MongoDb;
                    ProviderName = AppConstants.Providers[ConnectionInfoTypeOption.MongoDb];
                    WorkspaceProvider.Instance.ConvertTo(this, ConnectionInfoTypeOption.MongoDb);
                    DatastoreChanged();
                }
            }
        }

        public bool IsRedis
        {
            get { return Model.ConnectionType.Equals(ConnectionInfoTypeOption.Redis); }
            set
            {
                if (value)
                {
                    Model.ConnectionType = ConnectionInfoTypeOption.Redis;
                    ProviderName = AppConstants.Providers[ConnectionInfoTypeOption.Redis];
                    WorkspaceProvider.Instance.ConvertTo(this, ConnectionInfoTypeOption.Redis);
                    DatastoreChanged();
                }
            }
        }

        public bool IsMySql
        {
            get { return Model.ConnectionType.Equals(ConnectionInfoTypeOption.MySql); }
            set
            {
                if (value)
                {
                    Model.ConnectionType = ConnectionInfoTypeOption.MySql;
                    ProviderName = AppConstants.Providers[ConnectionInfoTypeOption.MySql];
                    WorkspaceProvider.Instance.ConvertTo(this, ConnectionInfoTypeOption.MySql);
                    DatastoreChanged();
                }
            }
        }

        public bool IsNeo4j
        {
            get { return Model.ConnectionType.Equals(ConnectionInfoTypeOption.Neo4j); }
            set
            {
                if (value)
                {
                    Model.ConnectionType = ConnectionInfoTypeOption.Neo4j;
                    ProviderName = AppConstants.Providers[ConnectionInfoTypeOption.Neo4j];
                    WorkspaceProvider.Instance.ConvertTo(this, ConnectionInfoTypeOption.Neo4j);
                    DatastoreChanged();
                }
            }
        }

        public bool IsFile
        {
            get { return Model.ConnectionType.Equals(ConnectionInfoTypeOption.File); }
            set
            {
                if (value)
                {
                    Model.ConnectionType = ConnectionInfoTypeOption.File;
                    ProviderName = AppConstants.Providers[ConnectionInfoTypeOption.File];
                    WorkspaceProvider.Instance.ConvertTo(this, ConnectionInfoTypeOption.File);
                    DatastoreChanged();
                }
            }
        }
       
        private static IList<ConnectionInfoTypeOption> filesystem = new List<ConnectionInfoTypeOption>
        {
            ConnectionInfoTypeOption.File,
            ConnectionInfoTypeOption.Excel,
            ConnectionInfoTypeOption.Xml,
            ConnectionInfoTypeOption.Json,
        };
        public bool IsFileSystem
        {
            get
            {
                return filesystem.Contains(Model.ConnectionType);
            }
        }

        public bool IsExcel
        {
            get { return Model.ConnectionType.Equals(ConnectionInfoTypeOption.Excel); }
            set
            {
                if (value)
                {
                    Model.ConnectionType = ConnectionInfoTypeOption.Excel;
                    ProviderName = AppConstants.Providers[ConnectionInfoTypeOption.Excel];
                    DatastoreChanged();
                }
            }
        }

        public bool IsXml
        {
            get { return Model.ConnectionType.Equals(ConnectionInfoTypeOption.Xml); }
            set
            {
                if (value)
                {
                    Model.ConnectionType = ConnectionInfoTypeOption.Xml;
                    ProviderName = AppConstants.Providers[ConnectionInfoTypeOption.Xml];
                    DatastoreChanged();
                }
            }
        }

        public bool IsJson
        {
            get { return Model.ConnectionType.Equals(ConnectionInfoTypeOption.Json); }
            set
            {
                if (value)
                {
                    Model.ConnectionType = ConnectionInfoTypeOption.Json;
                    ProviderName = AppConstants.Providers[ConnectionInfoTypeOption.Json];
                    DatastoreChanged();
                }
            }
        }








        public void DatastoreChanged()
        {
            OnPropertyChanged("IsNone");
            OnPropertyChanged("IsApi");
            OnPropertyChanged("IsSqlServer");
            OnPropertyChanged("IsMongoDb");
            OnPropertyChanged("IsRedis");
            OnPropertyChanged("IsMySql");
            OnPropertyChanged("IsNeo4j");
            OnPropertyChanged("IsFile");
            OnPropertyChanged("ConnectionType");
            OnPropertyChanged("IsFileSystem");
            OnPropertyChanged("IsXml");
            OnPropertyChanged("IsExcel");
            OnPropertyChanged("IsJson");
        }

        #endregion

        #region Name (string)

        /// <summary>
        /// Gets or sets the string value for Name
        /// </summary>
        /// <value> The string value.</value>

        public string Name
        {
            get
            {

                if (!String.IsNullOrWhiteSpace(Model.Name))
                {
                    return Model.Name;
                }
                else
                {
                    return String.Format("{0} {1}",Zone,AppConstants.ProviderNames[ProviderName]);
                }
            }
            set
            {
                if (Model.Name != value)
                {
                    Model.Name = value;
                    OnPropertyChanged("Name");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region Server (string)

        /// <summary>
        /// Gets or sets the string value for Server
        /// </summary>
        /// <value> The string value.</value>

        public string Server
        {
            get { return (String.IsNullOrEmpty(Model.Server)) ? String.Empty : Model.Server; }
            set
            {
                if (Model.Server != value)
                {
                    Model.Server = value;
                    OnPropertyChanged("Server");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region Text (string)

        /// <summary>
        /// Gets or sets the string value for Text
        /// </summary>
        /// <value> The string value.</value>

        public string Text
        {
            get { return (String.IsNullOrEmpty(Model.Text)) ? String.Empty : Model.Text; }
            set
            {
                if (Model.Text != value)
                {
                    Model.Text = value;
                    OnPropertyChanged("Text");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region Count (int)

        /// <summary>
        /// Gets or sets the int value for Count
        /// </summary>
        /// <value> The int value.</value>

        public int Count
        {
            get { return Model.Count; }
            set
            {
                if (Model.Count != value)
                {
                    Model.Count = value;
                    OnPropertyChanged("Count");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region Zone (XTool.Inference.ZoneTypeOption)


        /// <summary>
        /// Gets or sets the ZoneTypeOption value for Zone
        /// </summary>
        /// <value> The ZoneTypeOption value.</value>

        public XTool.Inference.ZoneTypeOption Zone
        {
            get { return Model.Zone; }
            set
            {
                if (Model.Zone != value)
                {
                    Model.Zone = value;
                    OnPropertyChanged("Zone");
                    if (!String.IsNullOrWhiteSpace(Name))
                    {
                        OnPropertyChanged("Name");
                    }
                    
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region ProviderName (string)

        /// <summary>
        /// Gets or sets the string value for ProviderName
        /// </summary>
        /// <value> The string value.</value>

        public string ProviderName
        {
            get { return (String.IsNullOrEmpty(Model.ProviderName)) ? String.Empty : Model.ProviderName; }
            set
            {
                if (Model.ProviderName != value)
                {
                    Model.ProviderName = value;
                    OnPropertyChanged("ProviderName");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region IsValidated (bool)

        /// <summary>
        /// Gets or sets the bool value for IsValidated
        /// </summary>
        /// <value> The bool value.</value>
        private bool _IsValidated;
        public bool IsValidated
        {
            get { return _IsValidated; }
            set
            {
                if (_IsValidated != value)
                {
                    _IsValidated = value;
                    OnPropertyChanged("IsValidated");
                    IsDirty = true;
                }
            }
        }

        #endregion

        private ICommand _TestConnectionCommand;
        public ICommand TestConnectionCommand
        {
            get
            {
                if (_TestConnectionCommand == null)
                {
                    _TestConnectionCommand = new RelayCommand(
                        param => TestConnection(),
                        param => CanTestConnection()
                            );
                }
                return _TestConnectionCommand;
            }
        }

        private ICommand _ExecuteDiscoveryCommand;
        public ICommand ExecuteDiscoveryCommand
        {
            get
            {
                if (_ExecuteDiscoveryCommand == null)
                {
                    _ExecuteDiscoveryCommand = new RelayCommand(
                        param => ExecuteDiscovery(),
                        param => CanExecuteDiscovery());
                }
                return _ExecuteDiscoveryCommand;
            }
        }

        private ICommand _RemoveConnectionCommand;
        public ICommand RemoveConnectionCommand
        {
            get
            {
                if (_RemoveConnectionCommand == null)
                {

                    _RemoveConnectionCommand = new RelayCommand(
                        param => RemoveConnection()
                        );
                }
                return _RemoveConnectionCommand;
            }
        }

        private void RemoveConnection()
        {
            ConnectionInfoViewModel vm = (ConnectionInfoViewModel)this;
            WorkspaceProvider.Instance.RemoveConnectionInfo(vm);
        }

        protected virtual bool CanTestConnection()
        {
            return true;
        }

        protected virtual void TestConnection()
        {

        }

        public virtual void BuildConnectionString()
        {

        }

        protected virtual bool CanExecuteDiscovery()
        {
            return false;
        }

        protected virtual void ExecuteDiscovery()
        {

        }



        public ConnectionInfoViewModel(ConnectionInfo model)
        {
            Model = model;
        }

        public ConnectionInfoViewModel() { }
    }
}
