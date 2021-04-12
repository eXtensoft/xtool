using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.IO;
using System.Diagnostics;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace XTool
{
    public class RedisConnectionInfoViewModel : ConnectionInfoViewModel
    {

        #region fields


        private bool _FolderExists = false;

        private bool _FolderHasContents = false;


        #endregion

        public new RedisConnectionInfo Model { get; set; }

        public ObservableCollection<FileInfoViewModel> KnownTypes { get; set; }

        private IDatabase _database;

        private ConnectionMultiplexer _connection;

        public ObservableCollection<RedisKey> Keys {get;set;}


        private RedisKey _SelectedKey;
        public RedisKey SelectedKey
        {
            get { return _SelectedKey; }
            set
            {
                _SelectedKey = value;
                OnPropertyChanged("SelectedKey");
                ExecuteFetch();
            }
        }

        private string _SelectedObject;
        public string SelectedObject
        {
            get { return _SelectedObject; }
            set
            {
                _SelectedObject = value;
                OnPropertyChanged("SelectedObject");
            }
        }

        public IServer RedisServer { get; set; }



        public int KeyCount
        {
            get { return Keys != null ? Keys.Count() : 0; }
        }

        #region output type

        private OutputTypeOption _OutputType = OutputTypeOption.Xml;


        #region IsXmlChecked (bool)

        /// <summary>
        /// Gets or sets the bool value for IsXmlChecked
        /// </summary>
        /// <value> The bool value.</value>

        public bool IsXmlChecked
        {
            get { return GetOutputType(OutputTypeOption.Xml); }
            set
            {
                if (value)
                {
                    SetOutputType(OutputTypeOption.Xml);
                }
            }
        }

        #endregion



        #region IsJsonChecked (bool)

        /// <summary>
        /// Gets or sets the bool value for IsJsonChecked
        /// </summary>
        /// <value> The bool value.</value>

        public bool IsJsonChecked
        {
            get { return GetOutputType(OutputTypeOption.Json); }
            set
            {
                if (value)
                {
                    SetOutputType(OutputTypeOption.Json);
                }

            }
        }

        #endregion

        private void SetOutputType(OutputTypeOption option)
        {
            _OutputType = option;
            foreach (OutputTypeOption item in Enum.GetValues(typeof(OutputTypeOption)))
            {
                string s = String.Format("Is{0}Checked", item.ToString());
                OnPropertyChanged(s);
                //OnPropertyChanged("OutputToFileLabel");
            }

            ExecuteFetch();
        }

        private bool GetOutputType(OutputTypeOption caller)
        {
            return (caller == _OutputType);
        }

        #endregion

        private ICommand _AddLibraryCommand;
        public ICommand AddLibraryCommand
        {
            get
            {
                if (_AddLibraryCommand == null)
                {
                    _AddLibraryCommand = new RelayCommand(
                        param => AddLibrary());
                }
                return _AddLibraryCommand;
            }
        }

        private ICommand _ExecuteValidateCacheCommand;
        public ICommand ExecuteValidateCacheCommand
        {
            get
            {

                if (_ExecuteValidateCacheCommand == null)
                {
                    _ExecuteValidateCacheCommand = new RelayCommand(param => ExecuteValidateCache(), param => CanExecuteValidateCache());
                }
                return _ExecuteValidateCacheCommand;
            }
        }

        private bool CanExecuteValidateCache()
        {
            return IsValidated;
        }

        private void ExecuteValidateCache()
        {
            int max = 3;
            //List<string> list = 
            
            List<string> list = Enumerable.Range(1, max).Select(x => x.ToString("000")).ToList();
            //_database.StringSet()
            foreach  (string item in list)
            {
                _database.Set( Guid.NewGuid().ToString(),item);
            }
            //_database.Set()


        }
        private void AddLibrary()
        {
            FileInfo info = null;
            string candidate = String.Empty;
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = String.Format("Class Library|*.dll;");

            Nullable<bool> result = dialog.ShowDialog();
            if (result == true)
            {
                candidate = dialog.FileName;
                if (TryValidateCandidate(candidate, out info))
                {
                    EnsureKnownTypes(info);
                }
            } 
            // if successful, show message indicating that the new dll will only be used upon next startup. OR, better yet...
            // find way to add to appdomain.
        }

        private void EnsureKnownTypes(FileInfo info)
        {
            EnsureDirectory();

            CopyFile(info);

            EnsureKnownTypes();

            var vms = GetFiles();

        }
        private void CopyFile(FileInfo info)
        {
            string filepath = AppConstants.RedisKnownTypesFoldername + "\\" + info.Name;
            File.Copy(info.FullName, filepath,true);
        }
        
        private void EnsureKnownTypes()
        {
            if (KnownTypes == null)
            {
                KnownTypes = new ObservableCollection<FileInfoViewModel>();
            }
            else
            {
                KnownTypes.Clear();
            }
            var vms = GetFiles();
            foreach (var vm in vms)
            {
                KnownTypes.Add(vm);
            }
            OnPropertyChanged("KnownTypes");
        }
        private void EnsureDirectory()
        {
            if (!Directory.Exists(AppConstants.RedisKnownTypesFoldername))
            {
                Directory.CreateDirectory(AppConstants.RedisKnownTypesFoldername);
            }

        }

        private void OnClassLibrarySelected(FileInfo info)
        {
            System.Windows.MessageBox.Show(info.FullName);
        }
        private void EnsureFiles()
        {

        }

        private List<FileInfoViewModel> GetFiles()
        {
            if (!Directory.Exists(AppConstants.RedisKnownTypesFoldername))
            {
                Directory.CreateDirectory(AppConstants.RedisKnownTypesFoldername);
            }
            var files = Directory.GetFiles(AppConstants.RedisKnownTypesFoldername);
            List<FileInfoViewModel> vms = new List<FileInfoViewModel>();
            if (files != null && files.Count() > 0)
            {
                foreach (var file in files)
                {
                    FileInfo fi = new FileInfo(file);
                    vms.Add(new FileInfoViewModel(fi, OnClassLibrarySelected));
                }
            }
            return vms;
        }


        private bool TryValidateCandidate(string candidate, out FileInfo info)
        {
            info = null;
            bool b = false;
            if (!String.IsNullOrEmpty(candidate) && File.Exists(candidate))
            {
                info = new FileInfo(candidate);
                b = true;
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
            if (KnownTypes != null)
            {
                KnownTypes.Clear();
            }

            DirectoryInfo di = new DirectoryInfo(AppConstants.RedisKnownTypesFoldername);
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
            DirectoryInfo di = new DirectoryInfo(AppConstants.RedisKnownTypesFoldername);
            if (di.Exists)
            {
                Process.Start(di.FullName);
            }
        }

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

        private ICommand _RefreshMetadataCommand;
        public ICommand RefreshMetadataCommand
        {
            get
            {
                if (_RefreshMetadataCommand == null)
                {
                    _RefreshMetadataCommand = new RelayCommand(
                        param => EnsureKnownTypes());
                }
                return _RefreshMetadataCommand;
            }
        }


        private ICommand _ClearCacheAllCommand;
        public ICommand ClearCacheAllCommand
        {
            get
            {
                if (_ClearCacheAllCommand == null)
                {
                    _ClearCacheAllCommand = new RelayCommand(
                        param => ClearAll(),
                        param =>CanClearAll());
                }
                return _ClearCacheAllCommand;
            }
        }

        private ICommand _ClearCacheOneCommand;
        public ICommand ClearCacheOneCommand
        {
            get
            {
                if (_ClearCacheOneCommand == null)
                {
                    _ClearCacheOneCommand = new RelayCommand(
                        param => ClearOne(),
                        param=>CanClearOne());
                }
                return _ClearCacheOneCommand;
            }
        }


        private ICommand _ExecuteFetchCommand;
        public ICommand ExecuteFetchCommand
        {
            get
            {
                if (_ExecuteFetchCommand == null)
                {
                    _ExecuteFetchCommand = new RelayCommand(
                        param => ExecuteFetch(),
                        param => CanExecuteFetch());
                }
                return _ExecuteFetchCommand;
            }
        }

        private bool CanClearAll()
        {
            return Keys != null && Keys.Count() > 0;
        }

        private void ExecuteFetch()
        {
            if (!String.IsNullOrWhiteSpace(SelectedKey))
            {
                bool b = false;
                object o = null;
                string s = SelectedKey.ToString();
                var found = _database.StringGet(s);
            
                try
                {
                    using (System.IO.MemoryStream stream = new System.IO.MemoryStream(found))
                    {
                        System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                        o = (object)formatter.Deserialize(stream);
                    }
                    b = true;
                }
                catch (Exception ex)
                {
                    var message = new Message() { Start = DateTime.Now, Timespan = 6000, Extract = ex.Message, Text = new List<string>() };
                    message.Text.Add(ex.Message);
                    WorkspaceProvider.Instance.Messages.Push(message);
                    System.Windows.MessageBox.Show(ex.Message);

                }

                if (b)
                {
                    if (_OutputType == OutputTypeOption.Xml)
                    {
                        System.Xml.XmlDocument xml = GenericSerializer.XmlDocFromItem(o);
                        SelectedObject = FormatXml(xml);                    
                    }
                    else
                    {
                        JsonSerializerSettings settings = new JsonSerializerSettings() 
                        { 
                            TypeNameHandling = TypeNameHandling.Auto, 
                            Formatting = Formatting.Indented 
                        };
                       
                        SelectedObject = JsonConvert.SerializeObject(o, settings);
                    }               
                }
                else
                {
                    SelectedObject = found.ToString();
                }                
            }



        }

     
        private static string FormatXml(System.Xml.XmlDocument xml)
        {
            StringBuilder sb = new StringBuilder();
            System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings()
            {
                Indent = true,
                IndentChars = "  ",
                NewLineChars = "\r\n",
                NewLineHandling = System.Xml.NewLineHandling.Replace
            };
            using (System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(sb,settings))
            {
                xml.Save(writer);
            }

            return sb.ToString();
        }

        private bool CanExecuteFetch()
        {
            string s = SelectedKey.ToString();
            return !String.IsNullOrWhiteSpace(s);
        }

        private bool CanClearOne()
        {
            string key = SelectedKey.ToString();
            return (!key.Equals("(null)") && Keys.Contains(key));
        }

        #region Version (string)

        /// <summary>
        /// Gets or sets the string value for Version
        /// </summary>
        /// <value> The string value.</value>

        public string Version
        {
            get { return (String.IsNullOrEmpty(Model.Version)) ? String.Empty : Model.Version; }
            set
            {
                if (Model.Version != value)
                {
                    Model.Version = value;
                    OnPropertyChanged("Version");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region Endpoint (string)

        /// <summary>
        /// Gets or sets the string value for Endpoint
        /// </summary>
        /// <value> The string value.</value>

        public string Endpoint
        {
            get { return (String.IsNullOrEmpty(Model.Endpoint)) ? String.Empty : Model.Endpoint; }
            set
            {
                if (Model.Endpoint != value)
                {
                    Model.Endpoint = value;
                    OnPropertyChanged("Endpoint");
                    IsDirty = true;
                }
            }
        }

        #endregion

        public RedisConnectionInfoViewModel(RedisConnectionInfo model)
        {            
            base.Model = model;
            Model = model;
            EnsureKnownTypes();
        }

        public override void BuildConnectionString()
        {
            Build();
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
            sb.AppendFormat("{0}", Server);
            Text = sb.ToString();
        }

        private bool IsValidConnectionString()
        {
            bool b = true;

            b = b ? !String.IsNullOrWhiteSpace(base.Model.Server) : false;

            return b;
        }

        protected override void TestConnection()
        {
            IsValidated = false;
            if (TryBuild())
            {
                try
                {
                    string connectionstring = String.Format("{0},{1}", Text, "allowAdmin=true");
                    _connection = ConnectionMultiplexer.Connect(connectionstring);
                    IsValidated = _connection.IsConnected;
                    if (IsValidated)
                    {
                        var endpoint = _connection.GetEndPoints();
                        RedisServer = _connection.GetServer(endpoint[0]);
                        Version = RedisServer.Version.ToString();
                        Endpoint = RedisServer.EndPoint.ToString();
                        //Keys = new ObservableCollection<RedisKey>( RedisServer.Keys().OrderByDescending(x=>x.ToString()).ToList());

                        IDatabase db = _connection.GetDatabase();
                        _database = db;
                        
                    }
                }
                catch (Exception ex)
                {
                    var message = new Message() { Start = DateTime.Now, Timespan = 6000, Extract = "Connection attempt failed", Text = new List<string>() };
                    message.Text.Add(ex.Message);
                    WorkspaceProvider.Instance.Messages.Push(message);
                    System.Windows.MessageBox.Show(ex.Message);
                }
            }
        }


        private void ClearOne()
        {

            if (CanClearOne())
            {
                string key = SelectedKey.ToString();

                var found = Keys.First(x => x.ToString().Equals(key, StringComparison.OrdinalIgnoreCase));
                if (!String.IsNullOrWhiteSpace(found.ToString()))
                {                    
                    try
                    {
                        Keys.Remove(found);
                        _database.KeyDelete(key);
                        OnPropertyChanged("Keys");
                        OnPropertyChanged("KeyCount");
                    }
                    catch (Exception ex)
                    {
                        string s = String.Format("Clear key failed for {0}", key);
                            var message = new Message() { Start = DateTime.Now, Timespan = 6000, Extract = s, Text = new List<string>() };
                            message.Text.Add(ex.Message);
                            WorkspaceProvider.Instance.Messages.Push(message);
                            System.Windows.MessageBox.Show(ex.Message);
                    }                
                }
            }


        }

        private void ClearAll()
        {
            string key = String.Empty;
            try
            {
                Keys.Clear();
                var endpoints = _connection.GetEndPoints(true);
                foreach (var endpoint in endpoints)
                {
                    
                    var server = _connection.GetServer(endpoint);
                    key = server.EndPoint.ToString();
                    server.FlushAllDatabases();


                }

                OnPropertyChanged("Keys");
                OnPropertyChanged("KeyCount");                
            }
            catch (Exception ex)
            {
                string s = String.Format("Clear all failed for {0}", key);
                    var message = new Message() { Start = DateTime.Now, Timespan = 6000, Extract = s, Text = new List<string>() };
                    message.Text.Add(ex.Message);
                    WorkspaceProvider.Instance.Messages.Push(message);
                    System.Windows.MessageBox.Show(ex.Message);
            }
           
        }

        //private void Refresh()
        //{
        //    try
        //    {
        //        if (_database != null)
        //        {
        //            Keys.Clear();
        //            Keys = new ObservableCollection<RedisKey>( RedisServer.Keys().ToList());
        //            OnPropertyChanged("Keys");
        //            OnPropertyChanged("KeyCount");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var message = new Message() { Start = DateTime.Now, Timespan = 6000, Extract = "Connection attempt failed", Text = new List<string>() };
        //        message.Text.Add(ex.Message);
        //        WorkspaceProvider.Instance.Messages.Push(message);
        //        System.Windows.MessageBox.Show(ex.Message);                
        //    }
        //}

        protected override bool CanExecuteDiscovery()
        {
            return RedisServer != null;
        }

        protected override void ExecuteDiscovery()
        {
            ExecuteValidateCache();
            Keys = new ObservableCollection<RedisKey>(RedisServer.Keys().OrderByDescending(x => x.ToString()).ToList());

            OnPropertyChanged("Keys");
            OnPropertyChanged("KeyCount");
        }


       

    }
}
