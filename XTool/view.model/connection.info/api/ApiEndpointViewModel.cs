using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Input;

namespace XTool
{
    public class ApiEndpointViewModel : ViewModel<ApiEndpoint>
    {
        private ApiConnectionInfoViewModel _Master;



        public ObservableCollection<ApiHeaderViewModel> Headers
        {
            get
            {
                return _Master.Headers;
            }
        }






        #region RequiresAuthorization (bool)

        /// <summary>
        /// Gets or sets the bool value for RequiresAuthorization
        /// </summary>
        /// <value> The bool value.</value>

        public bool RequiresAuthorization
        {
            get { return Model.RequiresAuthorization; }
            set
            {
                if (Model.RequiresAuthorization != value)
                {
                    Model.RequiresAuthorization = value;
                    OnPropertyChanged("RequiresAuthorization");
                    IsDirty = true;
                }
            }
        }

        #endregion


        #region Authorization (HeaderTypeOption)


        /// <summary>
        /// Gets or sets the HeaderTypeOption value for Authorization
        /// </summary>
        /// <value> The HeaderTypeOption value.</value>

        public HeaderTypeOption Authorization
        {
            get { return Model.Authorization; }
            set
            {
                if (Model.Authorization != value)
                {
                    Model.Authorization = value;
                    OnPropertyChanged("Authorization");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region Header (string)

        /// <summary>
        /// Gets or sets the string value for Header
        /// </summary>
        /// <value> The string value.</value>

        public string Header
        {
            get { return (String.IsNullOrEmpty(Model.Header)) ? String.Empty : Model.Header; }
            set
            {
                if (Model.Header != value)
                {
                    Model.Header = value;
                    OnPropertyChanged("Header");
                    IsDirty = true;
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
            get { return _Master.Protocol; }
        }

        #endregion


        #region Order (int)

        /// <summary>
        /// Gets or sets the int value for Order
        /// </summary>
        /// <value> The int value.</value>

        public int Order
        {
            get { return Model.Order; }
            set
            {
                if (Model.Order != value)
                {
                    Model.Order = value;
                    OnPropertyChanged("Order");
                    IsDirty = true;
                }
            }
        }

        #endregion


        #region Moniker (string)

        /// <summary>
        /// Gets or sets the string value for Moniker
        /// </summary>
        /// <value> The string value.</value>

        public string Moniker
        {
            get { return (String.IsNullOrEmpty(Model.Moniker)) ? String.Empty : Model.Moniker; }
            set
            {
                if (Model.Moniker != value)
                {
                    Model.Moniker = value;
                    OnPropertyChanged("Moniker");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region Pattern (string)

        /// <summary>
        /// Gets or sets the string value for Pattern
        /// </summary>
        /// <value> The string value.</value>

        public string Pattern
        {
            get { return (String.IsNullOrEmpty(Model.Pattern)) ? String.Empty : Model.Pattern; }
            set
            {
                if (Model.Pattern != value)
                {
                    Model.Pattern = value;
                    OnPropertyChanged("Pattern");
                    IsDirty = true;
                }
            }
        }

        #endregion


        #region UrlSuffix (string)

        /// <summary>
        /// Gets or sets the string value for UrlSuffix
        /// </summary>
        /// <value> The string value.</value>

        public string UrlSuffix
        {
            get { return (String.IsNullOrEmpty(Model.UrlSuffix)) ? String.Empty : Model.UrlSuffix; }
            set
            {
                if (Model.UrlSuffix != value)
                {
                    Model.UrlSuffix = value;
                    OnPropertyChanged("UrlSuffix");
                    IsDirty = true;
                }
            }
        }

        #endregion

        public ObservableCollection<ApiParameterViewModel> Parameters { get; set; }


        #region HttpMethod (HttpMethodOption)


        /// <summary>
        /// Gets or sets the HttpMethodOption value for HttpMethod
        /// </summary>
        /// <value> The HttpMethodOption value.</value>

        public HttpMethodOption HttpMethod
        {
            get { return Model.HttpMethod; }
            set
            {
                if (Model.HttpMethod != value)
                {
                    Model.HttpMethod = value;
                    OnPropertyChanged("HttpMethod");
                    IsDirty = true;
                }
            }
        }

        #endregion




        public ObservableCollection<ApiParameterSetViewModel> ParameterSets { get; set; }


        private ApiParameterSetViewModel _SelectedSet;
        public ApiParameterSetViewModel SelectedSet
        {
            get { return _SelectedSet; }
            set
            {
                if (value != null)
                {
                    _SelectedSet = value;
                    AssembleUrls();                   
                    OnPropertyChanged("SelectedSet");
                }
            }
        }


        #region HasBody (bool)

        /// <summary>
        /// Gets or sets the bool value for HasBody
        /// </summary>
        /// <value> The bool value.</value>

        public bool HasBody
        {
            get { return Model.HasBody; }
            set
            {
                if (Model.HasBody != value)
                {
                    Model.HasBody = value;
                    OnPropertyChanged("HasBody");
                    IsDirty = true;
                }
            }
        }

        #endregion


        #region Body (string)

        /// <summary>
        /// Gets or sets the string value for Body
        /// </summary>
        /// <value> The string value.</value>

        public string Body
        {
            get { return (String.IsNullOrEmpty(Model.Body)) ? String.Empty : Model.Body; }
            set
            {
                if (Model.Body != value)
                {
                    Model.Body = value;
                    OnPropertyChanged("Body");
                    IsDirty = true;
                }
            }
        }

        #endregion


        #region BaseUrl (string)

        /// <summary>
        /// Gets or sets the string value for BaseUrl
        /// </summary>
        /// <value> The string value.</value>
        private string _BaseUrl;
        public string BaseUrl
        {
            get { return (String.IsNullOrEmpty(_BaseUrl)) ? String.Empty : _BaseUrl; }
            set
            {
                if (_BaseUrl != value)
                {
                    _BaseUrl = value;
                    OnPropertyChanged("BaseUrl");
                    IsDirty = true;
                }
            }
        }

        #endregion


        #region Url (string)

        /// <summary>
        /// Gets or sets the string value for Url
        /// </summary>
        /// <value> The string value.</value>
        private string _Url;
        public string Url
        {
            get { return (String.IsNullOrEmpty(_Url)) ? String.Empty : _Url; }
            set
            {
                if (_Url != value)
                {
                    _Url = value;
                    OnPropertyChanged("Url");
                }
            }
        }

        #endregion



        private void AssembleUrls()
        {
            StringBuilder baseUrl = new StringBuilder();
            if (_Master.SelectedUrl == null || String.IsNullOrWhiteSpace(_Master.SelectedUrl.Url))
            {
                MessageBox.Show("null values");
            }
            if (_Master.SelectedUrl != null)
            {
                baseUrl.Append(_Master.SelectedUrl.Protocol.ToString().ToLower());
                baseUrl.Append("://");
                baseUrl.Append(_Master.SelectedUrl.Url);
            }
            BaseUrl = baseUrl.ToString();

            StringBuilder url = new StringBuilder();
           
            string pattern = Pattern.ToLower();
            if (SelectedSet != null)
            {
                foreach (var item in SelectedSet.Parameters)
                {
                    string token = String.Format("{{{0}}}", item.Name.ToLower());
                    if (pattern.Contains(token) && item.Value != null)
                    {
                        pattern = pattern.Replace(token, item.Value.ToString());
                    }
                }
            }

            url.Append("/");
            url.Append(pattern);
            if (!String.IsNullOrWhiteSpace(UrlSuffix))
            {
                url.Append(UrlSuffix);
            }
            Url = url.ToString();

        }

        public ApiEndpointViewModel(ApiEndpoint model, ApiConnectionInfoViewModel master)
        {
            _Master = master;

            ParameterSets = new ObservableCollection<ApiParameterSetViewModel>();
            if (model.ParameterSets == null)
            {
                model.ParameterSets = new List<ApiParameterSet>();
            }

            Model = model;

            if (model.Parameters != null)
            {
                Parameters = new ObservableCollection<ApiParameterViewModel>(
                    (from x in model.Parameters select new ApiParameterViewModel(x)).ToList());
            }

            ParameterSets.CollectionChanged += ParameterSets_CollectionChanged;
        }

        private void ParameterSets_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    var vm = item as ApiParameterSetViewModel;
                    if (vm != null)
                    {
                        Model.ParameterSets.Add(vm.Model);
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems)
                {
                    var vm = item as ApiParameterSetViewModel;
                    if (vm != null)
                    {
                        Model.ParameterSets.Remove(vm.Model);
                    }
                }
            }
        }


        private ICommand _ExecuteCommand;
        public ICommand ExecuteCommand
        {
            get
            {
                if (_ExecuteCommand == null)
                {
                    _ExecuteCommand = new RelayCommand(
                        param => Execute(), 
                        param => CanExecute());
                }
                return _ExecuteCommand;
            }
        }

        private bool CanExecute()
        {
            return true;
        }
        private void Execute()
        {
            ApiRequest request = AssembleRequest();

            var model = HttpCaller.Execute(request);
            _Master.Calls.Add(new ApiCallViewModel(model));
        }

        private ApiRequest AssembleRequest()
        {
            ApiRequest request = new ApiRequest()
            {
                BaseUrl = BaseUrl,
                Url = Url,
                HttpMethod = HttpMethod,
                Protocol = _Master.SelectedUrl.Protocol
            };
            if (HasBody && SelectedSet.HasBody)
            {
                request.RequestBody = SelectedSet.Body;
            }

            return request;
            
        }

        private ICommand _RemoveSetCommand;
        public ICommand RemoveSetCommand
        {
            get
            {
                if (_RemoveSetCommand == null)
                {
                    _RemoveSetCommand = new RelayCommand(
                        param => RemoveSet(),
                        param => CanRemoveSet());
                }
                return _RemoveSetCommand;
            }
        }

        private bool CanRemoveSet()
        {
            return SelectedSet != null;
        }
        private void RemoveSet()
        {
            ParameterSets.Remove(SelectedSet);
        }

        private ICommand _AddSetCommand;
        public ICommand AddSetCommand
        {
            get
            {
                if (_AddSetCommand == null)
                {
                    _AddSetCommand = new RelayCommand(
                        param=>AddSet(),
                        param=>CanAddSet()
                        );
                }
                return _AddSetCommand;
            }
        }

        private bool CanAddSet()
        {
            return ParameterSets != null;
        }

        private void AddSet()
        {
            ApiParameterSet model = new ApiParameterSet() { Key = Model.Key };
            model.Parameters = GenericSerializer.Clone<List<ApiParameter>>(Model.Parameters);
            model.Body = Body;
            model.HasBody = HasBody;
            model.UrlSuffix = UrlSuffix;
            model.Headers = _Master.Model.Headers;
            
            ApiParameterSetViewModel vm = new ApiParameterSetViewModel(model,this);
            ParameterSets.Add(vm);
        }








    }
}
