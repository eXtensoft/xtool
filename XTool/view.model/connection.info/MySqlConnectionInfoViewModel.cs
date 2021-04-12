using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;
using System.Diagnostics;
using System.Dynamic;
using System.Windows;
using System.Windows.Controls;
using System.IO;

namespace XTool
{
    public class MySqlConnectionInfoViewModel : ConnectionInfoViewModel
    {
        #region local fields


        private bool _FolderExists = false;

        private bool _FolderHasContents = false;

        #endregion

        #region properties

        public List<System.Data.DbType> DbTypes { get; set; }

        public new MySqlConnectionInfo Model { get; set; }

        //public ObservableCollection<FileInfoViewModel> SqlMetadata { get; set; }

        public ObservableCollection<ComparisonFileInfoViewModel> SqlMetadata { get; set; }

        #region MetadataComparison (DataTable)

        private DataTable _MetadataComparison;

        /// <summary>
        /// Gets or sets the DataTable value for MetadataComparison
        /// </summary>
        /// <value> The DataTable value.</value>

        public DataTable MetadataComparison
        {
            get { return _MetadataComparison; }
            set
            {
                if (_MetadataComparison != value)
                {
                    _MetadataComparison = value;
                    OnPropertyChanged("MetadataComparison");
                }
            }
        }

        #endregion

        #region IsMetadataExpanded (bool)

        private bool _IsMetadataExpanded;

        /// <summary>
        /// Gets or sets the bool value for IsMetadataExpanded
        /// </summary>
        /// <value> The bool value.</value>

        public bool IsMetadataExpanded
        {
            get { return _IsMetadataExpanded; }
            set
            {
                if (_IsMetadataExpanded != value)
                {

                    _IsMetadataExpanded = value;
                    OnPropertyChanged("IsMetadataExpanded");
                }
            }
        }

        #endregion


        #region DataProfile (TabDataProfiler)

        private TabDataProfiler _DataProfiler;

        /// <summary>
        /// Gets or sets the TabDataProfiler value for DataProfile
        /// </summary>
        /// <value> The TabDataProfiler value.</value>

        public TabDataProfiler DataProfiler
        {
            get { return _DataProfiler; }
            set
            {
                if (_DataProfiler != value)
                {
                    _DataProfiler = value;
                    OnPropertyChanged("DataProfiler");
                }
            }
        }

        #endregion

        public ObservableCollection<FieldFilterViewModel> Filters { get; set; }

        private Display _SelectedCount = null;
        public Display SelectedCount
        {
            get { return _SelectedCount; }
            set
            {
                _SelectedCount = value;
                OnPropertyChanged("SelectedDisplay");
            }
        }


        private DataTable _Data = null;
        public DataTable Data
        {
            get { return _Data; }
            set
            {
                _Data = value;
                OnPropertyChanged("Data");
            }
        }

        int _ResultSetCount = -1;
        public int ResultSetCount
        {
            get { return _ResultSetCount; }
            set
            {
                if (_ResultSetCount != value)
                {
                    _ResultSetCount = value;

                    OnPropertyChanged("ResultSetCount");
                    OnPropertyChanged("ResultSet");
                }
            }
        }
        string _ResultSet = "ResultSet";

        public string ResultSet
        {
            get
            {
                if (_ResultSetCount < 1)
                {
                    return _ResultSet;
                }
                else
                {
                    return String.Format("{0} {1}", _ResultSet, _ResultSetCount.ToString());
                }

            }

        }

        public string FilterText
        {
            get { return ComposeFilterLogic(_IsAnd); }
        }
        #endregion

        #region Discovery Observable Properties

        #region ShowSchemaGroupings (bool)

        private bool _ShowSchemaGroupings = true;

        /// <summary>
        /// Gets or sets the bool value for ShowSchemaGroupings
        /// </summary>
        /// <value> The bool value.</value>

        public bool ShowSchemaGroupings
        {
            get { return _ShowSchemaGroupings; }
            set
            {
                if (_ShowSchemaGroupings != value)
                {
                    OnPropertyChanged("ShowSchemaGroupings");
                    _ShowSchemaGroupings = value;
                }
            }
        }

        #endregion

        private ObservableCollection<GroupingViewModel> _Groupings = new ObservableCollection<GroupingViewModel>();
        public ObservableCollection<GroupingViewModel> Groupings
        {
            get { return _Groupings; }
            set
            {
                _Groupings = value;
                OnPropertyChanged("Groupings");
                OnPropertyChanged("IsDiscovered");
            }

        }

        public bool IsDiscovered
        {
            get { return Groupings != null && Groupings.Count > 0; }
        }
        #endregion


        #region Connection Observable Properties

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
                    TryBuild();
                }
            }
        }

        #endregion

        #region User (string)

        /// <summary>
        /// Gets or sets the string value for User
        /// </summary>
        /// <value> The string value.</value>

        public string User
        {
            get { return (String.IsNullOrEmpty(Model.User)) ? String.Empty : Model.User; }
            set
            {
                if (Model.User != value)
                {
                    Model.User = value;
                    OnPropertyChanged("User");
                    IsDirty = true;
                    TryBuild();
                }
            }
        }

        #endregion

        #region Pwd (string)

        /// <summary>
        /// Gets or sets the string value for Pwd
        /// </summary>
        /// <value> The string value.</value>

        public string Pwd
        {
            get { return (String.IsNullOrEmpty(Model.Pwd)) ? String.Empty : Model.Pwd; }
            set
            {
                if (Model.Pwd != value)
                {
                    Model.Pwd = value;
                    OnPropertyChanged("Pwd");
                    IsDirty = true;
                    TryBuild();
                }
            }
        }

        #endregion

        #region IntegratedSecurity (bool)

        /// <summary>
        /// Gets or sets the bool value for IntegratedSecurity
        /// </summary>
        /// <value> The bool value.</value>

        public bool IntegratedSecurity
        {
            get { return Model.IntegratedSecurity; }
            set
            {
                if (Model.IntegratedSecurity != value)
                {
                    Model.IntegratedSecurity = value;
                    OnPropertyChanged("IntegratedSecurity");
                    IsDirty = true;
                    TryBuild();
                }
            }
        }

        #endregion

        #endregion


        #region Command Properties

        public bool IsCommandSelected
        {
            get
            {
                return SelectedCommand != null;
            }
        }

        private SqlCommandViewModel _SelectedCommand = null;
        public SqlCommandViewModel SelectedCommand
        {
            get { return _SelectedCommand; }
            set
            {
                _SelectedCommand = value;
                OnPropertyChanged("SelectedCommand");
                if (value != null)
                {
                    OnPropertyChanged("IsCommandSelected");
                }
            }
        }


        #region Commands (ObservableCollection<SqlCommandViewModel>)


        private ObservableCollection<SqlCommandViewModel> _Commands = new ObservableCollection<SqlCommandViewModel>();

        /// <summary>
        /// Gets or sets the ObservableCollection<SqlCommandViewModel> value for Commands
        /// </summary>
        /// <value> The ObservableCollection<SqlCommandViewModel> value.</value>

        public ObservableCollection<SqlCommandViewModel> Commands
        {
            get { return _Commands; }
            set
            {
                if (_Commands != value)
                {
                    _Commands = value;
                }
            }
        }

        #endregion

        #endregion

        private ICommand _AddTextSqlCommand;
        public ICommand AddTextSqlCommand
        {
            get
            {
                if (_AddTextSqlCommand == null)
                {
                    _AddTextSqlCommand = new RelayCommand(
                        param => AddTextSql(),
                        param => CanAddTextSql()
                            );
                }
                return _AddTextSqlCommand;
            }
        }



        private void AddTextSql()
        {
            Commands.Insert(0,new SqlCommandViewModel(new Inference.SqlCommand() { CommandType = "Text", Title = "Select...", Text = "SELECT * FROM sysobjects where type='u'" }));
        }
        private bool CanAddTextSql()
        {
            return true;
        }


        private ICommand _RemoveSqlCommand;
        public ICommand RemoveSqlCommand
        {
            get
            {
                if (_RemoveSqlCommand == null)
                {
                    _RemoveSqlCommand = new RelayCommand<SqlCommandViewModel>(
                        new Action<SqlCommandViewModel>(RemoveSql));
                }
                return _RemoveSqlCommand;
            }
        }


        private void RemoveSql(SqlCommandViewModel viewModel)
        {
            Commands.Remove(viewModel);
        }


        private ICommand _AddStoredProcedureSqlCommand;
        public ICommand AddStoredProcedureSqlCommand
        {
            get
            {
                if (_AddStoredProcedureSqlCommand == null)
                {
                    _AddStoredProcedureSqlCommand = new RelayCommand(
                        param => AddStoredProcedureSql(),
                        param => CanAddStoredProcedureSql()
                            );
                }
                return _AddStoredProcedureSqlCommand;
            }
        }





        private ICommand _ExecuteProfileCommand;
        public ICommand ExecuteProfileCommand
        {
            get
            {
                if (_ExecuteProfileCommand == null)
                {
                    _ExecuteProfileCommand = new RelayCommand(
                        param => ExecuteProfile(),
                        param => CanExecuteProfile());
                }
                return _ExecuteProfileCommand;
            }
        }

        private void ExecuteProfile()
        {
            if (SelectedCount != null)
            {
                ExecuteProfile(SelectedCount.MaxDistinct);
            }
        }
        private bool CanExecuteProfile()
        {
            return SelectedCommand != null && SelectedCount != null;
        }


        private ICommand _EditSqlTextCommand;
        public ICommand EditSqlTextCommand
        {
            get
            {
                if (_EditSqlTextCommand == null)
                {
                    _EditSqlTextCommand = new RelayCommand(
                        param => EditSqlText(),
                        param => CanEditSqlText());
                }
                return _EditSqlTextCommand;
            }
        }

        private void EditSqlText()
        {
            dynamic param = new ExpandoObject();
            param.Title = "Sql text command editor";

            param.Control = new SqlTextEditorView() { DataContext = SelectedCommand };
            OverlayManager manager = Application.Current.Properties[AppConstants.OverlayManager] as OverlayManager;
            manager.SetOverlay(AppConstants.OverlayContent, param);
            // manager.SetOverlay("key", param);
        }

        private bool CanEditSqlText()
        {
            return SelectedCommand != null;
        }

        private ICommand _ProcureFiltersCommand;
        public ICommand ProcureFiltersCommand
        {
            get
            {
                if (_ProcureFiltersCommand == null)
                {
                    _ProcureFiltersCommand = new RelayCommand(
                        param => ProcureFilters(),
                        param => CanProcureFilters());
                }
                return _ProcureFiltersCommand;
            }
        }

        private ICommand _RefreshMetadataCommand;
        public ICommand RefreshMetadataCommand
        {
            get
            {
                if (_RefreshMetadataCommand == null)
                {
                    _RefreshMetadataCommand = new RelayCommand(
                        param => RefreshMetadata());
                }
                return _RefreshMetadataCommand;
            }
        }

        private void RefreshMetadata()
        {
            if (SqlMetadata != null)
            {
                SqlMetadata.Clear();
            }


            MetadataComparison = null;

            List<FileInfo> list = new List<FileInfo>();
            DirectoryInfo di = new DirectoryInfo(AppConstants.ExplorerFoldername);
            if (di.Exists)
            {
                _FolderExists = true;

                var fileInfo = di.GetFileSystemInfos();
                if (fileInfo != null && fileInfo.Count() > 0)
                {
                    foreach (var info in fileInfo)
                    {
                        if (info is FileInfo && info.Extension.Equals(".xml", StringComparison.OrdinalIgnoreCase))
                        {
                            _FolderHasContents = true;
                            list.Add((FileInfo)info);
                        }
                    }
                }
            }
            if (list.Count > 0)
            {
                SqlMetadata = new ObservableCollection<ComparisonFileInfoViewModel>();
                foreach (var item in list)
                {
                    SqlMetadata.Add(new ComparisonFileInfoViewModel(item));
                }
                OnPropertyChanged("SqlMetadata");
            }
            IsMetadataExpanded = true;
        }

        private ICommand _CompareMetadataCommand;
        public ICommand CompareMetadataCommand
        {
            get
            {
                if (_CompareMetadataCommand == null)
                {
                    _CompareMetadataCommand = new RelayCommand(
                        param => CompareMetadata());
                }
                return _CompareMetadataCommand;
            }
        }

        private void CompareMetadata()
        {
            IsMetadataExpanded = false;
            string message;
            ComparisonFileInfoViewModel[] selected;

            if (CanCompareMetadata(out selected, out message))
            {
                List<SqlExplorer> list = new List<SqlExplorer>();
                foreach (ComparisonFileInfoViewModel vm in selected)
                {
                    SqlExplorer explorer = GenericSerializer.ReadGenericItem<SqlExplorer>(vm.FullName);

                    list.Add(explorer);
                }
                var c = new FieldComparisonCollection(list);
                MetadataComparison = c.ToDataTable();
            }
        }

        private bool CanCompareMetadata(out ComparisonFileInfoViewModel[] fileInfos, out string message)
        {
            fileInfos = null;
            message = String.Empty;
            bool b = false;
            List<ComparisonFileInfoViewModel> selected = new List<ComparisonFileInfoViewModel>();
            if (SqlMetadata != null)
            {
                foreach (ComparisonFileInfoViewModel vm in SqlMetadata)
                {
                    if (vm.IsSelected)
                    {
                        selected.Add(vm);
                    }
                }
                if (selected.Count > 1)
                {
                    b = true;
                    fileInfos = selected.ToArray();
                }
            }

            return b;
        }


        private ICommand _ClearFolderCommand;
        public ICommand ClearFolderCommand
        {
            get
            {
                if (_ClearFolderCommand == null)
                {
                    _ClearFolderCommand = new RelayCommand(
                        param => ClearFolder(),
                        param => CanClearFolder());
                }
                return _ClearFolderCommand;
            }
        }

        private bool CanClearFolder()
        {
            return true;
        }
        private void ClearFolder()
        {
            if (SqlMetadata != null)
            {
                SqlMetadata.Clear();
            }

            DirectoryInfo di = new DirectoryInfo(AppConstants.ExplorerFoldername);
            if (di.Exists)
            {
                di.Empty();
                _FolderHasContents = false;
            }

        }

        private ICommand _OpenFolderCommand;
        public ICommand OpenFolderCommand
        {
            get
            {
                if (_OpenFolderCommand == null)
                {
                    _OpenFolderCommand = new RelayCommand(
                        param => OpenFolder(),
                        param => CanOpenFolder());
                }
                return _OpenFolderCommand;
            }
        }

        private bool CanOpenFolder()
        {
            return _FolderExists;
        }
        private void OpenFolder()
        {
            DirectoryInfo di = new DirectoryInfo(AppConstants.ExplorerFoldername);
            if (di.Exists)
            {
                Process.Start(di.FullName);
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
            return Explorer != null;
        }

        public void SaveMetadata()
        {
            SqlExplorer clone = GenericSerializer.Clone<SqlExplorer>(Explorer);

            StringBuilder sb = new StringBuilder();
            sb.Append(Model.Server.Trim(new char[] { '(', ')', '\\', '/' }).Replace(@"\", " "));
            sb.Append("-");
            sb.Append(Model.Catalog.Trim(new char[] { '(', ')', '\\', '/' }).Replace(@"\", " "));
            clone.Moniker = sb.ToString();

            List<string> allschemas = new List<string>();
            Dictionary<string, bool> schemas = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
            List<XTool.Discovery.SqlTable> tables = new List<Discovery.SqlTable>();

            foreach (XTool.Discovery.SqlTable table in Explorer.Tables)
            {
                if (!allschemas.Contains(table.TableSchema))
                {
                    allschemas.Add(table.TableSchema);
                }
                if (table.IsChecked)
                {
                    if (!schemas.ContainsKey(table.TableSchema))
                    {
                        schemas.Add(table.TableSchema, true);
                    }
                    tables.Add(table);
                }
                else if (schemas.ContainsKey(table.TableSchema))
                {
                    schemas[table.TableSchema] = false;
                }
            }
            StringBuilder sbt = new StringBuilder();
            sbt.Append("-");


            if (allschemas.Count().Equals(schemas.Count))
            {
                // allschemas
                bool isAll = true;
                foreach (var key in schemas.Keys)
                {
                    isAll = isAll ? schemas[key] : false;
                }
                string t = isAll ? "alltables" : "sometables";
                sbt.AppendFormat("allschemas-{0}", t);
            }
            else
            {
                int i = 0;
                bool isAll = true;
                foreach (var key in schemas.Keys)
                {
                    if (i++ > 0)
                    {
                        sbt.Append("|");
                    }
                    sbt.Append(key);
                    isAll = isAll ? schemas[key] : false;
                }
                string t = isAll ? "alltables" : "sometables";
                //sbt.AppendFormat("-{0}", t);
                sbt.Clear();
                sbt.AppendFormat("-someschemas-{0}", t);

            }

            sbt.Append("-");

            sb.Append(sbt.ToString());

            clone.Tables = tables;

            sb.Append(Explorer.AsOf.Date.Year + "." + Explorer.AsOf.Date.Month + "." + Explorer.AsOf.Date.Day);
            sb.Append(".xml");

            string s = sb.ToString();

            clone.StoredProcedures.Clear();
            clone.CompareBy = CompareOption.Database;

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


        public Visibility LogicalVisibility
        {
            get
            {
                return (Filters != null && Filters.Count > 1) ? Visibility.Visible : Visibility.Hidden;
            }
        }

        private bool _IsAnd = false;
        public bool IsAnd
        {
            get { return _IsAnd; }
            set
            {
                _IsAnd = value;
                OnPropertyChanged("IsAnd");
                OnPropertyChanged("FilterText");
            }
        }


        public string ProcureFiltersLabel
        {
            get
            {
                return (Filters != null && Filters.Count > 0) ? "Clear Filters" : "Get Filters";
            }
        }

        private bool CanProcureFilters()
        {
            return DataProfiler != null ? DataProfiler.CanGetFilters() : false;
        }

        private void FilterChanged()
        {
            OnPropertyChanged("FilterText");
        }

        private string ComposeFilterLogic(bool isAnd)
        {
            int i = 0;
            string operand = isAnd ? "and" : "or";
            StringBuilder sb = new StringBuilder();
            if (Filters != null)
            {
                foreach (var filter in Filters)
                {
                    if (i++ > 0)
                    {
                        sb.AppendLine(operand);
                    }
                    sb.AppendLine(filter.ComposeFilterLogic());
                }
            }
            return sb.ToString();
        }

        private void ProcureFilters()
        {
            if (Filters != null)
            {
                Filters.Clear();
            }
            var list = DataProfiler.GetFilters();
            if (list != null && list.Count > 0)
            {
                Filters = new ObservableCollection<FieldFilterViewModel>(from x in list select new FieldFilterViewModel(x, FilterChanged));
                OnPropertyChanged("Filters");
            }
            OnPropertyChanged("ProcureFiltersLabel");
            OnPropertyChanged("LogicalVisibility");

        }

        private List<FieldFilter> GetSelectedFilters()
        {
            List<FieldFilter> list = new List<FieldFilter>();
            if (Filters != null && Filters.Count > 0)
            {
                foreach (var item in Filters)
                {
                    item.ProcureFilter(list);
                }
            }

            return list;
        }


        private void ExecuteProfile(int max)
        {
            Stopwatch sw = new Stopwatch();
            if (Validate())
            {
                SelectedCommand.Model.Count += 1;
                ProfilerSettings settings = new ProfilerSettings() { MaxDistinct = max };
                List<FieldFilter> filters = GetSelectedFilters();
                var profiler = new TabDataProfiler(settings, filters, _IsAnd);
                try
                {
                    string connectionString = String.Format("{0};Convert Zero Datetime=True", Text);
                    using (MySqlConnection cn = new MySqlConnection(connectionString))
                    {
                        cn.Open();

                        using (MySqlCommand cmd = SelectedCommand.CreateMySqlCommand(cn))
                        {
                            sw.Start();
                            using (MySqlDataReader reader = cmd.ExecuteReader())
                            {
                                profiler.Execute(reader);
                            }
                        }
                    }
                    sw.Stop();
                    profiler.Calculate();
                    DataProfiler = profiler;
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                    Message message = new Message() { Start = DateTime.Now, Timespan = 4000 };
                    message.Text.Add(ex.Message);
                    //WorkspaceProvider.Instance.Messages.Aggregate(message);
                }
            }
            //grdResults.DataContext = _Profiler;

            TimeSpan ts = sw.Elapsed;
            SelectedCommand.Elapsed = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
        }






    private void AddStoredProcedureSql()
        {
            Commands.Insert(0,new SqlCommandViewModel(new Inference.SqlCommand()
            {
                CommandType = "StoredProcedure",
                Title = "Title",
                Text = "[dbo].[SprocName]",
                Parameters = new List<Inference.Parameter>()
            }));
        }

        private bool CanAddStoredProcedureSql()
        {
            return true;
        }


        private ICommand _ExecuteComparisonCommand;
        public ICommand ExecuteComparisonCommand
        {
            get
            {
                if (_ExecuteComparisonCommand == null)
                {
                    _ExecuteComparisonCommand = new RelayCommand(param => ExecuteComparison(), param => CanExecuteComparison());
                }
                return _ExecuteComparisonCommand;
            }
        }


        private bool CanExecuteComparison()
        {
            return SelectedCommand != null;
        }

        private void ExecuteComparison()
        {
            DataTable dt = new DataTable();
            dt = MySqlTableComparer.Execute(Text, SelectedCommand.Text, BorrowCompare);
            Data = dt;
        }

        private static string GetLibraryServicesSql()
        {
            return " SELECT TABLE_SCHEMA AS Catalog, 'RBDG' as TableSchema, TABLE_NAME as TableName, COLUMN_NAME as ColumnName," +
                    " ORDINAL_POSITION AS OrdinalPosition, COLUMN_DEFAULT as DefaultValue," +
                    " CASE WHEN IS_NULLABLE = 'YES' THEN 1 ELSE 0 END AS IsNullible, DATA_TYPE AS Datatype," +
                    " CHARACTER_MAXIMUM_LENGTH AS MaxLength, NULL AS IsComputed, " +
                    " NULL AS ColumnLength, NULL AS IsIdentity, NULL AS IsRowGuidColumn," +
                    " CASE WHEN COLUMN_KEY = 'PRI' THEN 1 ELSE 0 END AS IsPrimaryKey," +
                    " CASE WHEN COLUMN_KEY = 'MUL' THEN 1 ELSE 0 END AS IsForeignKey," +
                    " CASE WHEN COLUMN_KEY = 'UNI' THEN 1 ELSE 0 END AS HasUniqueConstraint" +
                    " FROM INFORMATION_SCHEMA.COLUMNS" +
                    " WHERE TABLE_SCHEMA = 'RBDG' and TABLE_NAME like '%_Service_Library'" +
                    " ORDER BY TABLE_NAME, ORDINAL_POSITION";
        }

        private Tuple<string, string> BorrowCompare(IDataReader reader)
        {
            string tableName = reader.GetString(reader.GetOrdinal("TableName"));
            string columnName = reader.GetString(reader.GetOrdinal("ColumnName"));
            string dataType = reader.GetString(reader.GetOrdinal("Datatype"));
            //string service = tableName.Substring(0, tableName.IndexOf('_'));
            return new Tuple<string, string>(tableName, columnName);
        }



        private ICommand _ExecuteResultsetCommand;
        public ICommand ExecuteResultsetCommand
        {
            get
            {
                if (_ExecuteResultsetCommand == null)
                {
                    _ExecuteResultsetCommand = new RelayCommand(
                        param => ExecuteResultset(),
                        param => CanExecuteResultset());
                }
                return _ExecuteResultsetCommand;
            }
        }

        private bool CanExecuteResultset()
        {
            return (SelectedCommand != null);
        }


        private void ExecuteResultset()
        {
            ResetResultSet();
            DataTable dt = new DataTable();
            if (Validate())
            {
                Model.Count += 1;
                try
                {
                    string connectionString = String.Format("{0};Convert Zero Datetime=True", Text);
                    using (MySqlConnection cn = new MySqlConnection(connectionString))
                    {
                        cn.Open();
                        using (MySqlCommand cmd = SelectedCommand.CreateMySqlCommand(cn))
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                        ResultSetCount = dt.Rows.Count;
                    }
                }
                catch (Exception ex)
                {
                    Message message = new Message() { Start = DateTime.Now, Timespan = 4000, Text = new List<string>() };
                    message.Text.Add(ex.Message);
                    WorkspaceProvider.Instance.Messages.Push(message);
                    MessageBox.Show(ex.ToString());
                }
            }

            Data = dt;
        }

        private void ResetResultSet()
        {
            ResultSetCount = -1;
            //dgrResultSet.DataContext = null;
        }

        private ICommand _DataReaderCommand;
        public ICommand ExportDataReaderCommand
        {
            get
            {
                if (_DataReaderCommand == null)
                {
                    _DataReaderCommand = new RelayCommand(
                        param => ExportDataReader(),
                        param => CanExportDataReader());
                }
                return _DataReaderCommand;
            }
        }

        private bool CanExportDataReader()
        {
            return (Data != null && Data.Rows.Count > 0);
        }
        public void ExportDataReader()
        {
            dynamic param = new ExpandoObject();
            param.Title = "Export DataReader";
            param.Control = new ExportDataReaderView() { DataContext = Data };
            WorkspaceProvider.Instance.Overlay.SetOverlay(AppConstants.OverlayContent, param);
        }


        private ICommand _ExportResultsetCommand;
        public ICommand ExportResultsetCommand
        {
            get
            {
                if (_ExportResultsetCommand == null)
                {
                    _ExportResultsetCommand = new RelayCommand(
                        param => ExportResultset(),
                        param => CanExportResultset());
                }
                return _ExportResultsetCommand;
            }
        }

        private void ExportResultset()
        {
            dynamic param = new ExpandoObject();
            param.Title = "Export Resultset";
            param.Control = new ExportTabResultsetView() { DataContext = Data };
            WorkspaceProvider.Instance.Overlay.SetOverlay(AppConstants.OverlayContent, param);
        }

        private bool CanExportResultset()
        {
            return (Data != null && Data.Rows.Count > 0);
        }

        private ICommand _CopyToClipboardCommand;
        public ICommand CopyToClipboardCommand
        {
            get
            {
                if (_CopyToClipboardCommand == null)
                {
                    _CopyToClipboardCommand = new RelayCommand<DataGrid>(
                       new Action<DataGrid>(CopyToClipboard));
                }
                return _CopyToClipboardCommand;
            }
        }



        private void CopyToClipboard(DataGrid dataGrid)
        {
            dataGrid.SelectAllCells();
            dataGrid.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
            ApplicationCommands.Copy.Execute(null, dataGrid);
            dataGrid.UnselectAllCells();
        }

        private ICommand _CopyComparisonToClipboardCommand;
        public ICommand CopyComparisonToClipboardCommand
        {
            get
            {
                if (_CopyComparisonToClipboardCommand == null)
                {
                    _CopyComparisonToClipboardCommand = new RelayCommand<DataGrid>(
                       new Action<DataGrid>(CopyComparisonToClipboard));
                }
                return _CopyComparisonToClipboardCommand;
            }
        }



        private void CopyComparisonToClipboard(DataGrid dataGrid)
        {
            dataGrid.SelectAllCells();
            dataGrid.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
            ApplicationCommands.Copy.Execute(null, dataGrid);
            dataGrid.UnselectAllCells();
        }


        void Commands_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    SqlCommandViewModel vm = item as SqlCommandViewModel;
                    if (vm != null)
                    {
                        Model.Commands.Add(vm.Model);
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems)
                {
                    SqlCommandViewModel vm = item as SqlCommandViewModel;
                    if (vm != null)
                    {
                        Model.Commands.Remove(vm.Model);
                    }
                }
            }
        }




        public MySqlConnectionInfoViewModel(MySqlConnectionInfo model)
        {
            base.Model = model;
            Model = model;
            if (model.Commands != null)
            {
                foreach (var item in model.Commands)
                {
                    Commands.Add(new SqlCommandViewModel(item));
                }
            }
            Commands.CollectionChanged += new NotifyCollectionChangedEventHandler(Commands_CollectionChanged);

            DbTypes = new List<System.Data.DbType>();
            foreach (string item in Enum.GetNames(typeof(System.Data.DbType)))
            {
                DbTypes.Add((System.Data.DbType)Enum.Parse(typeof(System.Data.DbType), item, true));
            }
        }


        protected override bool CanExecuteDiscovery()
        {
            return IsValidated;
        }

        protected override void TestConnection()
        {
            IsValidated = false;
            if (TryBuild())
            {
                try
                {
                    using (MySqlConnection cn = new MySqlConnection(Text))
                    {
                        cn.Open();
                        if (cn.State == ConnectionState.Open)
                        {
                            IsValidated = true;
                        }
                    }

                }
                catch (Exception ex)
                {
                    var message = new Message() { Start = DateTime.Now, Timespan = 6000, Extract = "Connection attempt failed", Text = new List<string>() };
                    message.Text.Add(ex.Message);
                    WorkspaceProvider.Instance.Messages.Push(message);
                }
            }
        }

        private bool IsValidConnectionString()
        {
            bool b = true;
            b = b ? !String.IsNullOrWhiteSpace(base.Model.Server) : false;
            b = b ? !String.IsNullOrWhiteSpace(Model.Catalog) : false;
            if (!Model.IntegratedSecurity)
            {
                b = b ? (!String.IsNullOrWhiteSpace(Model.User) && !String.IsNullOrWhiteSpace(Model.Pwd)) : false;
            }
            return b;
        }

        protected override void ExecuteDiscovery()
        {
            Explorer = new SqlExplorer(Text) { AsOf = DateTime.Now, Text = Model.Text };
            Explorer.DiscoverMySql(Model.Catalog);

            if (ShowSchemaGroupings && Explorer.Schemas != null)
            {
                List<GroupingViewModel> groups = new List<GroupingViewModel>();
                TableGroupingViewModel tables = new TableGroupingViewModel();

                foreach (string item in Explorer.Schemas)
                {
                    if (Explorer.Tables != null)
                    {
                        var tgvm = new TableSchemaGroupingViewModel((Explorer.Tables.Where(x => x.TableSchema.Equals(item))).ToList(), item, this);
                        if (tgvm.Items.Count > 0)
                        {
                            tables.Items.Add(tgvm);
                        }
                    }


                }
                groups.Add(tables);
                Groupings = new ObservableCollection<GroupingViewModel>(groups);

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
            MySqlConnectionStringBuilder sb = new MySqlConnectionStringBuilder();
            sb.Server = base.Server;
            sb.Database = Model.Catalog;

            if (Model.IntegratedSecurity)
            {
                sb.IntegratedSecurity = Model.IntegratedSecurity;
            }
            else
            {
                sb.UserID = Model.User;
                sb.Password = Model.Pwd;
            }

            base.Text = sb.ConnectionString;
        }

    }
}
