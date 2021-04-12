using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using XTool.Inference;

namespace XTool
{
    public class ApiParameterSetViewModel : ViewModel<ApiParameterSet>
    {
        private ApiEndpointViewModel _Master;

        public ObservableCollection<ApiHeaderViewModel> ApiHeaders
        {
            get { return _Master.Headers; }
        }

        #region Zone (ZoneTypeOption)


        /// <summary>
        /// Gets or sets the ZoneTypeOption value for Zone
        /// </summary>
        /// <value> The ZoneTypeOption value.</value>

        public ZoneTypeOption Zone
        {
            get { return Model.Zone; }
            set
            {
                if (Model.Zone != value)
                {
                    Model.Zone = value;
                    OnPropertyChanged("Zone");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region Name (string)

        /// <summary>
        /// Gets or sets the string value for Name
        /// </summary>
        /// <value> The string value.</value>

        public string Name
        {
            get { return (String.IsNullOrEmpty(Model.Name)) ? String.Empty : Model.Name; }
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

        #region RunAt (DateTime)

        /// <summary>
        /// Gets or sets the DateTime value for RunAt
        /// </summary>
        /// <value> The DateTime value.</value>

        public DateTime RunAt
        {
            get { return Model.RunAt; }
            set
            {
                if (Model.RunAt != value)
                {
                    Model.RunAt = value;
                    OnPropertyChanged("RunAt");
                    IsDirty = true;
                }
            }
        }

        #endregion

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

        public ObservableCollection<ApiParameterViewModel> Parameters { get; set; }

        public ObservableCollection<ApiHeaderViewModel> Headers { get; set; }


        #region SelectedHeader (ApiHeaderViewModel)
        private ApiHeaderViewModel _SelectedApiHeader;

        /// <summary>
        /// Gets or sets the ApiHeaderViewModel value for SelectedHeader
        /// </summary>
        /// <value> The ApiHeaderViewModel value.</value>

        public ApiHeaderViewModel SelectedApiHeader
        {
            get { return _SelectedApiHeader; }
            set
            {
                if (_SelectedApiHeader != value)
                {
                    _SelectedApiHeader = value;
                    OnPropertyChanged("SelectedHeader");
                    IsDirty = true;
                }
            }
        }

        #endregion

        private ICommand _AddHeaderCommand;
        public ICommand AddHeaderCommand
        {
            get
            {
                if (_AddHeaderCommand == null)
                {
                    _AddHeaderCommand = new RelayCommand(
                        param => AddHeader(), 
                        param => CanAddHeader());
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
            if (_SelectedApiHeader != null)
            {
                ApiHeader model = new ApiHeader()
                {
                    Name = _SelectedApiHeader.Model.Name,
                    Scope = _SelectedApiHeader.Model.Scope,
                    Tag = _SelectedApiHeader.Model.Tag,
                    Type = _SelectedApiHeader.Model.Type,
                    Value = _SelectedApiHeader.Model.Value
                };
                Headers.Add(new ApiHeaderViewModel(model,this));
            }
            else
            {
                ApiHeader model = new ApiHeader()
                {
                    Name =  "name",
                    Scope = "scope",
                    Tag = String.Empty,
                    Type = "type",
                    Value = null,
                };
                Headers.Add(new ApiHeaderViewModel(model,this));
            }
        }
        public ApiParameterSetViewModel(ApiParameterSet model, ApiEndpointViewModel master)
        {
            _Master = master;
            Model = model;
            Headers = new ObservableCollection<ApiHeaderViewModel>();
            Headers.CollectionChanged += Headers_CollectionChanged;
            if (model.Parameters != null)
            {
                Parameters = new ObservableCollection<ApiParameterViewModel>(from x in model.Parameters select new ApiParameterViewModel(x));
                Parameters.CollectionChanged += Parameters_CollectionChanged;
            }
        }


        private ICommand _RemoveCommand;
        public ICommand RemoveCommand
        {
            get
            {
                if (_RemoveCommand == null)
                {
                    _RemoveCommand = new RelayCommand(param => Remove());
                }
                return _RemoveCommand;
            }
        }

        private void Remove()
        {
            int index = _Master.ParameterSets.IndexOf(this);
            if (index > -1)
            {
                _Master.ParameterSets.RemoveAt(index);
            }
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

        private void Parameters_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    var vm = item as ApiParameterViewModel;
                    if (vm != null)
                    {
                        Model.Parameters.Add(vm.Model);
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems)
                {
                    var vm = item as ApiParameterViewModel;
                    if (vm != null)
                    {
                        Model.Parameters.Remove(vm.Model);
                    }
                }
            }
        }
    }
}
