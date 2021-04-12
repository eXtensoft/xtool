using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.IO;
using System.Windows;

namespace XTool
{
    public class SqlTableViewModel : ViewModel<Discovery.SqlTable>
    {
        #region fields

        private static char[] vowels = { 'a', 'A', 'e', 'E', 'i', 'I', 'o', 'O', 'u', 'U' };

        private ConnectionInfoViewModel _ConnectionViewModel;

        private bool _IsMySql = false;

        #endregion
        #region Catalog (string)

        /// <summary>
        /// Gets or sets the string value for Catalog
        /// </summary>
        /// <value> The string value.</value>

        public string Catalog
        {
            get { return (String.IsNullOrEmpty(Model.Catalog)) ? String.Empty : Model.Catalog; }
            set
            {
                if (Model.Catalog != value)
                {
                    Model.Catalog = value;
                    OnPropertyChanged("Catalog");
                    IsDirty = true;
                }
            }
        }

        #endregion
        #region TableSchema (string)

        /// <summary>
        /// Gets or sets the string value for TableSchema
        /// </summary>
        /// <value> The string value.</value>

        public string TableSchema
        {
            get { return (String.IsNullOrEmpty(Model.TableSchema)) ? String.Empty : Model.TableSchema; }
            set
            {
                if (Model.TableSchema != value)
                {
                    Model.TableSchema = value;
                    OnPropertyChanged("TableSchema");
                    IsDirty = true;
                }
            }
        }

        #endregion
        #region TableName (string)

        /// <summary>
        /// Gets or sets the string value for TableName
        /// </summary>
        /// <value> The string value.</value>

        public string TableName
        {
            get { return (String.IsNullOrEmpty(Model.TableName)) ? String.Empty : Model.TableName; }
            set
            {
                if (Model.TableName != value)
                {
                    Model.TableName = value;
                    OnPropertyChanged("TableName");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region IsChecked (bool)

        /// <summary>
        /// Gets or sets the bool value for IsChecked
        /// </summary>
        /// <value> The bool value.</value>

        public bool IsChecked
        {
            get { return Model.IsChecked; }
            set
            {
                if (Model.IsChecked != value)
                {
                    Model.IsChecked = value;
                    OnPropertyChanged("IsChecked");
                    IsDirty = true;
                }
            }
        }

        #endregion



        #region operational properties

        private string _Company;
        public string Company
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_Company))
                {
                    object o = Application.Current.Properties["copyright.company"];
                    if (o != null)
                    {
                        _Company = o.ToString();
                    }
                }
                return !String.IsNullOrWhiteSpace(_Company) ? _Company : String.Empty;
            }
            set
            {
                if (_Company != value)
                {
                    Application.Current.Properties["copyright.company"] = value;
                    _Company = value;
                    OnPropertyChanged("Company");
                    IsDirty = true;
                }
            }
        }

        private ModelActionOption _ModelActions = ModelActionOption.Post | ModelActionOption.Put | ModelActionOption.Delete | ModelActionOption.Get | ModelActionOption.GetAll | ModelActionOption.GetAllProjections;
        public ModelActionOption ModelActions
        {
            get
            {
                return _ModelActions;
            }
            set
            {
                _ModelActions = value;
                OnPropertyChanged("ModelActions");
            }
        }

        public bool IsPost
        {
            get { return HasFlag(ModelActionOption.Post); }
            set { SetFlag(value, ModelActionOption.Post); }
        }

        public bool IsPut
        {
            get { return HasFlag(ModelActionOption.Put); }
            set { SetFlag(value, ModelActionOption.Put); }
        }
        public bool IsDelete
        {
            get { return HasFlag(ModelActionOption.Delete); }
            set { SetFlag(value, ModelActionOption.Delete); }
        }
        public bool IsGet
        {
            get { return HasFlag(ModelActionOption.Get); }
            set { SetFlag(value, ModelActionOption.Get); }
        }
        public bool IsGetAll
        {
            get { return HasFlag(ModelActionOption.GetAll); }
            set { SetFlag(value, ModelActionOption.GetAll); }
        }
        public bool IsGetAllProjections
        {
            get { return HasFlag(ModelActionOption.GetAllProjections); }
            set { SetFlag(value, ModelActionOption.GetAllProjections); }
        }
        public bool IsExecuteAction
        {
            get { return HasFlag(ModelActionOption.ExecuteAction); }
            set { SetFlag(value, ModelActionOption.ExecuteAction); }
        }



        private bool HasFlag(ModelActionOption option)
        {
            return _ModelActions.Has(option);
        }

        private void SetFlag(bool isAdd, ModelActionOption option)
        {
            if (isAdd)
            {
                _ModelActions = _ModelActions | option;
            }
            else
            {
                _ModelActions = _ModelActions ^ option;
            }
            string propertyName = String.Format("is{0}", option);
            OnPropertyChanged(propertyName);
            OnPropertyChanged("ModelActions");
        }


        #endregion

        private string _CreateConnection;
        public string CreateConnection
        {
            get
            {
                if (String.IsNullOrEmpty(_CreateConnection))
                {
                    _CreateConnection = "ConnectionProvider.CreateConnection()";
                }
                return _CreateConnection;
            }
            set
            {
                if (_CreateConnection != value)
                {
                    _CreateConnection = value;
                    OnPropertyChanged("CreateConnection");
                }
            }
        }

        private ICommand _AddTextCommand;
        public ICommand AddTextCommand
        {
            get
            {
                if (_AddTextCommand == null)
                {
                    _AddTextCommand = new RelayCommand(param => AddText());
                }
                return _AddTextCommand;
            }
        }

        private ICommand _PreviewCommand;
        public ICommand PreviewCommand
        {
            get
            {
                if (_PreviewCommand == null)
                {
                    _PreviewCommand = new RelayCommand(
                        param => Preview());
                }
                return _PreviewCommand;
            }
        }

        private ICommand _OutputToFolderCommand;
        public ICommand OutputToFolderCommand
        {
            get
            {
                if (_OutputToFolderCommand == null)
                {
                    _OutputToFolderCommand = new RelayCommand(
                        param => OutputToFolder());
                }
                return _OutputToFolderCommand;
            }
        }

        private string _OutputFolder;
        public string OutputFolder
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_OutputFolder))
                {
                    object o = Application.Current.Properties["output.folder"];
                    if (o != null)
                    {
                        _OutputFolder = o.ToString();
                    }
                }
                return !String.IsNullOrWhiteSpace(_OutputFolder) ? _OutputFolder : String.Empty;
            }
            set
            {
                if (_OutputFolder != value)
                {
                    Application.Current.Properties["output.folder"] = value;
                    _OutputFolder = value;
                    OnPropertyChanged("OutputFolder");
                    IsDirty = true;
                }
            }
        }


        private ICommand _SaveMetadataCommand;
        public ICommand SaveMetadataCommand
        {
            get
            {
                if (_SaveMetadataCommand == null)
                {
                    _SaveMetadataCommand = new RelayCommand(
                        param => SaveMetadata(),
                        param => CanSaveMetadata());
                }
                return _SaveMetadataCommand;
            }
        }

        public bool CanSaveMetadata()
        {
            return Model != null;
        }

        public void SaveMetadata()
        {
            SqlExplorer explorer = _ConnectionViewModel.Explorer;
            SqlExplorer clone = GenericSerializer.Clone<SqlExplorer>(explorer);

            StringBuilder sb = new StringBuilder();
            sb.Append(_ConnectionViewModel.Server.Trim(new char[] { '(', ')', '\\', '/' }));
            sb.Append("-");
            sb.Append(Model.Catalog.Trim(new char[] { '(', ')', '\\', '/' }));
            clone.Moniker = sb.ToString();
            sb.Append("-");
            sb.Append(Model.TableSchema);
            sb.Append("-");
            sb.Append(Model.TableName);
            sb.Append("-");
            sb.Append(explorer.AsOf.Date.Year + "." + explorer.AsOf.Date.Month + "." + explorer.AsOf.Date.Day);
            sb.Append(".xml");


            clone.Tables.Clear();
            clone.StoredProcedures.Clear();
            clone.CompareBy = CompareOption.Table;
            Discovery.SqlTable table = GenericSerializer.Clone<Discovery.SqlTable>(Model);
            clone.Tables.Add(table);

            string filepath = Path.Combine(AppConstants.ExplorerFoldername, sb.ToString());

            if (!Directory.Exists(AppConstants.ExplorerFoldername))
            {
                Directory.CreateDirectory(AppConstants.ExplorerFoldername);
            }

            try
            {
                GenericSerializer.WriteGenericItem<SqlExplorer>(clone, filepath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }



        private string _Namespace;
        public string Namespace
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_Namespace))
                {
                    object o = Application.Current.Properties["file.namespace"];
                    if (o != null)
                    {
                        _Namespace = o.ToString();
                    }
                }
                return !String.IsNullOrWhiteSpace(_Namespace) ? _Namespace : String.Empty;
            }
            set
            {
                if (_OutputFolder != value)
                {
                    Application.Current.Properties["file.namespace"] = value;
                    _Namespace = value;
                    OnPropertyChanged("Namespace");
                    IsDirty = true;
                }
            }
        }

        private string _AppContext;
        public string AppContext
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_AppContext))
                {
                    object o = Application.Current.Properties["file.appcontext"];
                    if (o != null)
                    {
                        _AppContext = o.ToString();
                    }
                }
                return !String.IsNullOrWhiteSpace(_AppContext) ? _AppContext : String.Empty;
            }
            set
            {
                if (_OutputFolder != value)
                {
                    Application.Current.Properties["file.appcontext"] = value;
                    _AppContext = value;
                    OnPropertyChanged("AppContext");
                    IsDirty = true;
                }
            }
        }



        #region Entity (string)

        private string _Entity;

        /// <summary>
        /// Gets or sets the string value for Entity
        /// </summary>
        /// <value> The string value.</value>

        public string Entity
        {
            get { return (String.IsNullOrEmpty(_Entity)) ? Model.TableName : _Entity; }
            set
            {
                if (!String.IsNullOrEmpty(value) && Model.TableName != value)
                {
                    _Entity = value;
                }
            }
        }

        #endregion

        private System.Data.DataTable _DataDictionary;
        internal System.Data.DataTable DataDictionary
        {
            get
            {
                if (_DataDictionary == null)
                {
                    _DataDictionary = GenerateDataDictionary();
                }
                return _DataDictionary;
            }

        }

        #region Columns (ObservableCollection<SqlColumnViewModel>)

        private ObservableCollection<SqlColumnViewModel> _Columns = new ObservableCollection<SqlColumnViewModel>();

        /// <summary>
        /// Gets or sets the ObservableCollection<SqlColumnViewModel> value for Columns
        /// </summary>
        /// <value> The ObservableCollection<SqlColumnViewModel> value.</value>

        public ObservableCollection<SqlColumnViewModel> Columns
        {
            get { return _Columns; }
            set
            {
                if (_Columns != value)
                {
                    _Columns = value;
                }
            }
        }

        #endregion

        #region operational properties
        private bool _IsMDGInline;
        public bool IsMDGInline
        {
            get { return _IsMDGInline; }
            set
            {
                _IsMDGInline = value;
                OnPropertyChanged("IsMDGInline");
            }
        }

        private bool _IsMDGSproc;
        public bool IsMDGSproc
        {
            get { return _IsMDGSproc; }
            set
            { _IsMDGSproc = value;
                OnPropertyChanged("IsMDGSproc");
            }
        }

        private bool _IsSproc;
        public bool IsSproc
        {
            get { return _IsSproc; }
            set
            {
                _IsSproc = value;
                OnPropertyChanged("IsSproc");
            }
        }

        private bool _IsSqlServer;
        public bool IsSqlServer
        {
            get { return _IsSqlServer; }
            set
            {
                _IsSqlServer = value;
                OnPropertyChanged("SqlServer");
            }
        }


        private bool _IsApi;
        public bool IsApi
        {
            get { return _IsApi; }
            set
            {
                _IsApi = value;
                OnPropertyChanged("IsApi");
            }
        }


        private bool _IsConfigText;
        public bool IsConfigText
        {
            get { return _IsConfigText; }
            set
            {
                _IsConfigText = value;
                if (value && _IsConfigSproc)
                {
                    IsConfigSproc = false;
                }
                OnPropertyChanged("IsConfigText");
            }
        }

        private bool _IsConfigSproc;
        public bool IsConfigSproc
        {
            get { return _IsConfigSproc; }
            set
            {
                _IsConfigSproc = value;
                if (value && _IsConfigText)
                {
                    IsConfigText = false;
                }
                OnPropertyChanged("IsConfigSproc");
            }
        }

        private bool _IsModel;
        public bool IsModel
        {
            get { return _IsModel; }
            set
            {
                _IsModel = value;
                OnPropertyChanged("IsModel");
            }
        }

        public bool TryFindIdentityColumn(out Discovery.SqlColumn identityColumn)
        {
            identityColumn = null;
            bool b = false;
            for (int i = 0; !b && i < Model.Columns.Count; i++)
            {
                if (Model.Columns[i].IsIdentity)
                {
                    identityColumn = Model.Columns[i];
                    b = true;
                }
            }
            return b;
        }

        public bool TryFindPKColumn(out Discovery.SqlColumn pkColumn)
        {
            pkColumn = null;
            bool b = false;
            for (int i = 0; !b && i < Model.Columns.Count; i++)
            {
                if (Model.Columns[i].IsPrimaryKey)
                {
                    pkColumn = Model.Columns[i];
                    b = true;
                }
            }
            return b;
        }

        public bool TryFindFKColumn(out Discovery.SqlColumn fkColumn)
        {
            fkColumn = null;
            bool b = false;
            for (int i = 0; i < Model.Columns.Count; i++)
            {
                if (Model.Columns[i].IsForeignKey)
                {
                    fkColumn = Model.Columns[i];
                    b = true;
                }
            }
            return b;
        }


        public string ToDisplay
        {
            get { return String.Format("[{0}].[{1}]", TableSchema, TableName); }
        }
        private string _ToModelName = String.Empty;
        public string ToModelName
        {
            get
            {
                if (String.IsNullOrEmpty(_ToModelName))
                {
                    int j = 0;
                    StringBuilder sb = new StringBuilder();
                    char[] arr = TableName.ToCharArray();
                    for (int i = 0; i < arr.Length; i++)
                    {
                        if (j == 0)
                        {
                            if (Char.IsUpper(arr[i]))
                            {
                                sb.Append(arr[i]);
                                j = 1;
                            }
                        }
                        else if (j == 1)
                        {
                            if (Char.IsLetter(arr[i]))
                            {
                                sb.Append(arr[i]);
                            }
                            else
                            {
                                j++;
                            }
                        }
                    }
                    string s = sb.ToString();
                    if (s.Length >= 5 && vowels.Contains(s[s.Length - 2]))
                    {
                        _ToModelName = s.TrimEnd('s');
                    }
                    else
                    {
                        _ToModelName = s;
                    }
                }

                return _ToModelName;
            }
            set
            {
                if (_ToModelName != value)
                {
                    _ToModelName = value;
                    OnPropertyChanged("ToModelName");
                }
            }
        }
        #endregion

        #region generated code
        private string _ModelText;
        public string ModelText
        {
            get { return _ModelText; }
            set { _ModelText = value; OnPropertyChanged("ModelText"); }
        }

        private string _MDGText;
        public string MDGText
        {
            get { return _MDGText; }
            set { _MDGText = value; OnPropertyChanged("MDGText"); }
        }

        private string _MDGSproc;
        public string MDGSproc
        {
            get { return _MDGSproc; }
            set { _MDGSproc = value; OnPropertyChanged("MDGSproc"); }
        }

        private string _MDGInline;
        public string MDGInline
        {
            get { return _MDGInline; }
            set { _MDGInline = value; OnPropertyChanged("MDGInline"); }
        }
        #endregion

        private string _SprocsText;
        public string SprocsText
        {
            get { return _SprocsText; }
            set { _SprocsText = value; OnPropertyChanged("SprocsText"); }
        }

        private string _DataProviderInterfaceText;
        public string DataProviderInterfaceText
        {
            get { return _DataProviderInterfaceText; }
            set { _DataProviderInterfaceText = value;OnPropertyChanged("DataProviderInterfaceText"); }
        }
        private string _SqlServerText; public string SqlServerText
        {
            get { return _SqlServerText; }
            set { _SqlServerText = value; OnPropertyChanged("SqlServerText"); }
        }


        private string _ApiText;
        public string ApiText
        {
            get { return _ApiText; }
            set { _ApiText = value; OnPropertyChanged("ApiText"); }
        }

        private string _ApiInterfaceText;
        public string ApiInterfaceText
        {
            get { return _ApiInterfaceText; }
            set { _ApiInterfaceText = value; OnPropertyChanged("ApiInterfaceText"); }
        }

        private string _ConfigText;
        public string ConfigText
        {
            get { return _ConfigText; }
            set { _ConfigText = value; OnPropertyChanged("ConfigText"); }
        }

        public string GetConnectionString()
        {
            return _ConnectionViewModel.Text;
        }

        #region constructors

        public SqlTableViewModel(Discovery.SqlTable model, SqlServerConnectionInfoViewModel connectionViewModel)
        {
            Model = model;
            if (model.Columns != null)
            {
                foreach (var item in model.Columns)
                {
                    _Columns.Add(new SqlColumnViewModel(item));
                }
            }
            _ConnectionViewModel = connectionViewModel;
        }

        public SqlTableViewModel(Discovery.SqlTable model, MySqlConnectionInfoViewModel connectionViewModel)
        {
            Model = model;
            _IsMySql = true;
            if (model.Columns != null)
            {
                foreach (var item in model.Columns)
                {
                    _Columns.Add(new SqlColumnViewModel(item));
                }
            }
            _ConnectionViewModel = connectionViewModel;
        }

        #endregion

        private System.Data.DataTable GenerateDataDictionary()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add(new System.Data.DataColumn("OrdinalPosition", typeof(System.Int32)));
            dt.Columns.Add(new System.Data.DataColumn("ColumnName", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("DataType", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("AllowNulls", typeof(System.Boolean)));
            dt.Columns.Add(new System.Data.DataColumn("DefaultValue", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("IsComputed", typeof(System.Boolean)));
            dt.Columns.Add(new System.Data.DataColumn("IsIdentity", typeof(System.Boolean)));
            dt.Columns.Add(new System.Data.DataColumn("IsPrimaryKey", typeof(System.Boolean)));
            dt.Columns.Add(new System.Data.DataColumn("IsForeignKey", typeof(System.Boolean)));
            if (Columns != null)
            {
                foreach (var item in Columns)
                {
                    item.GenerateDataDictionaryEntry(dt);
                }
            }

            return dt;
        }

        private void Preview()
        {
            ScriptGenerator generator = new ScriptGenerator(this);
            if (IsModel)
            {
                ModelText = generator.GenerateModel();
            }

            MDGText = generator.GenerateMDG();

            if (IsMDGInline)
            {
                MDGInline = generator.GenerateMDGInline();
            }
            if (IsMDGSproc)
            {
                MDGSproc = generator.GenerateMDGSproc();
            }
            if (_IsSproc)
            {
                SprocsText = generator.GenerateSprocs();
            }
            if (_IsConfigText)
            {
                ConfigText = generator.GenerateConfig(true);
            }
            if (_IsConfigSproc)
            {
                ConfigText = generator.GenerateConfig(false);
            }
            if (_IsSqlServer)
            {
                SqlServerText = generator.GenerateSqlServer();
                DataProviderInterfaceText = generator.GenerateDataProviderInterface();
            }
            if (_IsApi)
            {
                ApiText = generator.GenerateApi();
                ApiInterfaceText = generator.GenerateApiInterface();
            }
            
        }

        private void OutputToFolder()
        {
            string folder;
            if (String.IsNullOrWhiteSpace(OutputFolder) && FileSystem.SolicitFolderpath(out folder,true))
            {
                OutputFolder = folder;
            }

            Preview();

            if (!String.IsNullOrWhiteSpace(OutputFolder))
            {
                if (IsModel)
                {
                    string filename = String.Format("{0}.cs",Entity);
                    File.WriteAllText(Path.Combine(OutputFolder, filename), ModelText);
                } 
                if (IsMDGInline)
                {
                    string filename = String.Format("{0}MDG.cs", Entity);
                    File.WriteAllText(Path.Combine(OutputFolder, filename), MDGInline);
                }
                if (IsMDGSproc)
                {
                    string filename = String.Format("{0}MDG.cs", Entity);
                    File.WriteAllText(Path.Combine(OutputFolder, filename), MDGSproc);
                }
                if (IsSproc)
                {
                    string filename = String.Format("{0}.sql", Entity);
                    File.WriteAllText(Path.Combine(OutputFolder, filename), SprocsText);
                }
                if (IsConfigText | IsConfigSproc)
                {
                    string filename = String.Format("{0}.dbconfig.xml", Entity);
                    File.WriteAllText(Path.Combine(OutputFolder, filename), ConfigText);
                }
                if (IsSqlServer)
                {
                    string filename = String.Format("{0}{1}.cs", Entity, "Provider");
                    File.WriteAllText(Path.Combine(OutputFolder, filename), SqlServerText);
                }
                if (IsApi)
                {
                    string filename = String.Format("{0}{1}.cs", Entity, "DataService");
                    File.WriteAllText(Path.Combine(OutputFolder, filename), ApiText);
                }

            }



        }

        private void AddText()
        {
            string s = Model.CreateTextCommand();
            string t = TableName;
            var txt = new Inference.SqlCommand()
            {
                CommandType = "Text",
                Text = s,
                Title = t
            };
            if (_IsMySql)
            {
                var vm = _ConnectionViewModel as MySqlConnectionInfoViewModel;
                vm.Commands.Add(new SqlCommandViewModel(txt));
            }
            else
            {
                var vm = _ConnectionViewModel as SqlServerConnectionInfoViewModel;
                vm.Commands.Add(new SqlCommandViewModel(txt));
            }
        }
    }
}
