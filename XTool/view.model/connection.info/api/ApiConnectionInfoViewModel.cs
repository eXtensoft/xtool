

namespace XTool
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Bson;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Dynamic;
    using System.IO;
    using System.Data.Linq;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Input;
    using XTool.Inference;

    public class ApiConnectionInfoViewModel : ConnectionInfoViewModel
    {
        public new ApiConnectionInfo Model { get; set; }



        public ApiHeaderViewModel SelectedHeader { get; set; }

        public ObservableCollection<ApiHeaderViewModel> Headers { get; set; }

        #region SelectedUrl (ApiUrlViewModel)

        private ApiUrlViewModel _SelectedUrl;
        /// <summary>
        /// Gets or sets the ApiUrlViewModel value for SelectedUrl
        /// </summary>
        /// <value> The ApiUrlViewModel value.</value>

        public ApiUrlViewModel SelectedUrl
        {
            get { return _SelectedUrl; }
            set
            {
                if (_SelectedUrl != value)
                {
                    _SelectedUrl = value;
                    OnPropertyChanged("SelectedUrl");
                }
            }
        }

        #endregion


        #region Protocol (ProtocolOption)


        /// <summary>
        /// Gets or sets the ProtocolOption value for Protocol
        /// </summary>
        /// <value> The ProtocolOption value.</value>

        public ProtocolOption Protocol
        {
            get { return Model.Protocol; }
            set
            {
                if (Model.Protocol != value)
                {
                    Model.Protocol = value;
                    OnPropertyChanged("Protocol");
                    IsDirty = true;
                }
            }
        }

        #endregion

        public ObservableCollection<ApiUrlViewModel> Urls { get; set; }


        #region RawInput (string)
        private string _RawInput;
        /// <summary>
        /// Gets or sets the string value for RawInput
        /// </summary>
        /// <value> The string value.</value>

        public string RawInput
        {
            get { return (String.IsNullOrEmpty(_RawInput)) ? String.Empty : _RawInput; }
            set
            {
                if (_RawInput != value)
                {
                    _RawInput = value;
                    OnPropertyChanged("RawInput");
                    IsDirty = true;
                }
            }
        }


        public ObservableCollection<ApiEndpointViewModel> Endpoints { get; set; }

        public ObservableCollection<JTokenViewModel> Items { get; set; }

        public ObservableCollection<ApiEndpoint> TabularData { get; set; }


        public ObservableCollection<ApiCallViewModel> Calls { get; set; }


        #region SelectedApiCall (ApiCallViewModel)

        private ApiCallViewModel _SelectedApiCall;
        /// <summary>
        /// Gets or sets the ApiCallViewModel value for SelectedCall
        /// </summary>
        /// <value> The ApiCallViewModel value.</value>

        public ApiCallViewModel SelectedApiCall
        {
            get { return _SelectedApiCall; }
            set
            {
                if (_SelectedApiCall != value)
                {
                    _SelectedApiCall = value;
                    OnPropertyChanged("SelectedApiCall");
                    OnPropertyChanged("IsCallSelected");
                    IsDirty = true;
                }
            }
        }

        #endregion

        public bool IsCallSelected
        {
            get
            {
                return _SelectedApiCall != null;
            }
        }


        #endregion
        public ApiConnectionInfoViewModel(ApiConnectionInfo model)
        {
            Headers = new ObservableCollection<ApiHeaderViewModel>();
            Calls = new ObservableCollection<ApiCallViewModel>();
            Items = new ObservableCollection<JTokenViewModel>();
            TabularData = new ObservableCollection<ApiEndpoint>();
            EnsureUrls(model);
            Model = model;
            base.Model = model;

            if (model.Urls != null && model.Urls.Count > 0)
            {
                Urls = new ObservableCollection<ApiUrlViewModel>(from x in model.Urls select new ApiUrlViewModel(x));
            }

            if (model.Endpoints !=null && model.Endpoints.Count > 0)
            {
                Endpoints = new ObservableCollection<ApiEndpointViewModel>(from x in model.Endpoints select new ApiEndpointViewModel(x,this));
            }
            else
            {
                Endpoints = new ObservableCollection<ApiEndpointViewModel>();
            }
            if (model.Headers != null)
            {
                Headers = new ObservableCollection<ApiHeaderViewModel>(from x in model.Headers select new ApiHeaderViewModel(x));
            }
            Endpoints.CollectionChanged += Endpoints_CollectionChanged;

            Calls.CollectionChanged += Calls_CollectionChanged;

            Headers.CollectionChanged += Headers_CollectionChanged;
        }

        private void Headers_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    var vm = item as ApiHeaderViewModel;
                    if (vm != null)
                    {
                        Model.Headers.Add(vm.Model);
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems)
                {
                    var vm = item as ApiHeaderViewModel;
                    if (vm != null)
                    {
                        Model.Headers.Remove(vm.Model);
                    }
                }
            }
        }

        private void Calls_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {

            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    var vm = item as ApiCallViewModel;
                    if (vm != null)
                    {
                        // Model.Headers.Remove(vm.Model);
                    }
                }
            }
            else if(e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems)
                {
                    var vm = item as ApiCallViewModel;
                    if (vm != null)
                    {
                       // Model.Headers.Remove(vm.Model);
                    }
                }
            }
        }

        private void Endpoints_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    ApiEndpointViewModel vm = item as ApiEndpointViewModel;
                    if (vm != null)
                    {
                        Model.Endpoints.Add(vm.Model);
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems)
                {
                    ApiEndpointViewModel vm = item as ApiEndpointViewModel;
                    if (vm != null)
                    {
                        Model.Endpoints.Remove(vm.Model);
                    }
                }
            }
        }


        private void EnsureUrls(ApiConnectionInfo model)
        {
            int i = 0;
            //string rootUrl = "{api/url}";
            if (model.Urls == null)
            {
                model.Urls = new List<ApiUrl>();

            }
            List<ApiUrl> list = new List<ApiUrl>();
            foreach (ZoneTypeOption item in Enum.GetValues(typeof(ZoneTypeOption)))
            {
                var found = model.Urls.Find(x => x.Zone.Equals(item));
                if (found != null)
                {
                    list.Add(found);
                }
                else
                {
                    list.Add(new ApiUrl(item, AppConstants.DefaultUrl, ProtocolOption.None));
                }
            }
            model.Urls = list;

        }

        private ICommand _AddEndpointsCommand;
        public ICommand AddEndpointsCommand
        {
            get
            {
                if (_AddEndpointsCommand == null)
                {
                    _AddEndpointsCommand = new RelayCommand(
                        param => AddEndpoints(),
                        param => CanAddEndpoints());
                }
                return _AddEndpointsCommand;
            }
        }

        private bool CanAddEndpoints()
        {
            return TabularData != null && TabularData.Count() > 0;
        }

        private void AddEndpoints()
        {
            foreach (ApiEndpoint data in TabularData)
            {
                var found = Endpoints.ToList().Find(x => x.Model.Key.Equals(data.Key));
                if (found == null)
                {
                    Endpoints.Add(new ApiEndpointViewModel(data,this));
                }
            }
        }


        private ICommand _ImportApiEndpointsCommand;
        public ICommand ImportApiEndpointsCommand
        {
            get
            {
                if (_ImportApiEndpointsCommand == null)
                {
                    _ImportApiEndpointsCommand = new RelayCommand(
                        param=>ImportApiEndpoints(),
                        param=>CanImportApiEndpoints());
                }
                return _ImportApiEndpointsCommand;
            }
        }


        private bool CanImportApiEndpoints()
        {
            return true; // TabularData != null && TabularData.Count() > 0;
        }
        private void ImportApiEndpoints()
        {
            dynamic param = new ExpandoObject();
            param.Title = "Import API Endpoint(s)";
            param.Control = new ApiEndpointImportView() { DataContext = this };
            OverlayManager manager = Application.Current.Properties[AppConstants.OverlayManager] as OverlayManager;
            manager.SetOverlay(AppConstants.OverlayContent, param);
        }


        private ICommand _AddHeaderCommand;
        public ICommand AddHeaderCommand
        {
            get
            {
                if (_AddHeaderCommand == null)
                {
                    _AddHeaderCommand = new RelayCommand(param => AddHeader(), param => CanAddHeader());
                }
                return _AddHeaderCommand;
            }
        }
        private bool CanAddHeader()
        {
            return true;
        }
        private void AddHeader()
        {
            ApiHeader model = new ApiHeader() { Name = "headerName" };
            ApiHeaderViewModel vm = new ApiHeaderViewModel(model);
            Headers.Add(vm);
        }

        private  ICommand _ParseEndpointsCommand;
        public  ICommand ParseEndpointsCommand
        {
            get
            {
                if (_ParseEndpointsCommand == null)
                {
                    _ParseEndpointsCommand = new RelayCommand(
                        param=>ParseEndpoints(),
                        param=>CanParseEndpoints()
                       );
                }
                return _ParseEndpointsCommand;
            }
        }

        private bool CanParseEndpoints()
        {
            return !String.IsNullOrWhiteSpace(RawInput);
        }

        private void ParseEndpoints()
        {
            JTokenViewModel root = null;
            string jsonInput = RawInput.Trim();
            string json = String.Empty;
            TabularData.Clear();

            if (!String.IsNullOrWhiteSpace(jsonInput))
            {
                if (jsonInput.StartsWith("[") && jsonInput.EndsWith("]"))
                {
                    json = "{ 'data': " + jsonInput + "}";
                }
                else
                {
                    json = jsonInput;
                }
                try
                {
                    IEnumerable<ApiEndpoint> list;
                    string message;
                    if (EndpointParser.TryParse(jsonInput, out list, out message))
                    {
                        list.ToList().ForEach(TabularData.Add);
                    }

                    JObject obj = JObject.Parse(json);
                    Parse(obj, null, out root);
                    if (root != null)
                    {
                        Items.Add(root);

                    }
                }
                catch (Exception ex)
                {
                    string s = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    System.Windows.MessageBox.Show(s);
                }

            }
        }

        private static void Extract(JObject root)
        {
            var endpoints = (from x in root["registration"]
                            select (string)x["name"]).ToList();

            int j = endpoints.Count();
            using (StreamWriter file = File.CreateText(@"c:\temp\registration.bson"))
            using(JsonTextWriter writer = new JsonTextWriter(file))
            {
                root.WriteTo(writer);
            }
            string data = String.Empty;
            using (MemoryStream stream = new MemoryStream())
            using (BsonWriter writer = new BsonWriter(stream))
            {
                root.WriteTo(writer);
                data = Convert.ToBase64String(stream.ToArray());
            }
            string s = data;
        }

        private static void Parse(JToken token, JToken master, out JTokenViewModel viewModel)
        {

            viewModel = null;
            var type = token.GetType();
            if (type.Equals(typeof(JObject)))
            {
                var prop = token as JObject;
                viewModel = new JObjectViewModel() { Name = token.Path };

                foreach (JToken minion in prop.Children())
                {
                    JTokenViewModel minionVM;
                    Parse(minion, prop, out minionVM);
                    if (minionVM != null && viewModel != null)
                    {
                        if (viewModel.Items == null)
                        {
                            viewModel.Items = new ObservableCollection<JTokenViewModel>();
                        }
                        viewModel.Items.Add(minionVM);
                    }
                }

            }
            else if (type.Equals(typeof(JProperty)))
            {

                var prop = token as JProperty;

                var propType = prop.Value.GetType();
                if (propType.Equals(typeof(JValue)))
                {
                    var propValue = prop.Value as JValue;
                    viewModel = new JPropertyViewModel() { Name = prop.Name, Value = propValue };

                }
                else if (propType.Equals(typeof(JArray)))
                {
                    viewModel = new JArrayViewModel() { Name = prop.Name };
                    var arrayProp = prop.Value as JArray;
                    foreach (var minion in arrayProp.Children())
                    {
                        var minionType = minion.GetType();
                        JTokenViewModel minionVM;
                        Parse(minion, arrayProp, out minionVM);
                        if (minionVM != null)
                        {
                            if (viewModel.Items == null)
                            {
                                viewModel.Items = new ObservableCollection<JTokenViewModel>();
                            }
                            viewModel.Items.Add(minionVM);
                        }
                    }
                }
                else if (propType.Equals(typeof(JObject)))
                {

                    var objProp = prop.Value as JObject;
                    viewModel = new JObjectViewModel() { Name = objProp.Path };
                    foreach (JToken minion in objProp.Children())
                    {
                        JTokenViewModel minionVM;
                        Parse(minion, objProp, out minionVM);
                        if (minionVM != null && viewModel != null)
                        {
                            if (viewModel.Items == null)
                            {
                                viewModel.Items = new ObservableCollection<JTokenViewModel>();
                            }
                            viewModel.Items.Add(minionVM);
                        }
                    }

                }
            }
            else if (type.Equals(typeof(JArray)))
            {
                var prop = token as JArray;
                //if (viewModel == null)
                //{
                //    viewModel = new JArrayViewModel() { Name = prop.Path };
                //}
                foreach (var minion in prop.Children())
                {
                    JTokenViewModel minionVM;
                    Parse(minion, prop, out minionVM);
                    if (minionVM != null && viewModel != null)
                    {
                        if (viewModel.Items == null)
                        {
                            viewModel.Items = new ObservableCollection<JTokenViewModel>();
                        }
                        viewModel.Items.Add(minionVM);
                    }
                }
            }
            else
            {
                var prop = token as JValue;
                viewModel = new JPropertyViewModel() { Name = prop.Path, Value = prop.Value };
            }


        }


    }
}
