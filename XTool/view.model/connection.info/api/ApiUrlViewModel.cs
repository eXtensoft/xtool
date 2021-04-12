using System;
using System.Windows.Input;
using XTool.Inference;

namespace XTool
{
    public class ApiUrlViewModel : ViewModel<ApiUrl>
    {


        #region IsAvailable (bool)

        /// <summary>
        /// Gets or sets the bool value for IsAvailable
        /// </summary>
        /// <value> The bool value.</value>

        public bool IsAvailable
        {
            get
            {
                return Model.Validate();
            }
            set
            {
                if (Model.IsAvailable != value)
                {
                    Model.IsAvailable = value;
                    OnPropertyChanged("IsAvailable");
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
            get { return Model.Protocol; }
            set
            {
                if (Model.Protocol != value)
                {
                    Model.Protocol = value;
                    OnPropertyChanged("Protocol");
                    OnPropertyChanged("IsAvailable");
                    IsDirty = true;
                }
            }
        }

        #endregion


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


        #region Url (string)

        /// <summary>
        /// Gets or sets the string value for Url
        /// </summary>
        /// <value> The string value.</value>

        public string Url
        {
            get { return (String.IsNullOrEmpty(Model.Url)) ? String.Empty : Model.Url; }
            set
            {
                if (Model.Url != value)
                {
                    Model.Url = value;
                    OnPropertyChanged("Url");
                    OnPropertyChanged("IsAvailable");
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

        public ApiUrlViewModel(ApiUrl model)
        {
            Model = model;
        }


        private ICommand _ImportUrlsCommand;
        public ICommand ImportUrlsCommand
        {
            get
            {
                if (_ImportUrlsCommand == null)
                {
                    _ImportUrlsCommand = new RelayCommand(
                        param => ParseEndpoints(),
                        param => CanParseEndpoints());
                }
                return _ImportUrlsCommand;
            }
        }

        #region ImportUrls (string)
        private string _ImportUrls = String.Empty;
        /// <summary>
        /// Gets or sets the string value for ImportUrls
        /// </summary>
        /// <value> The string value.</value>

        public string ImportUrls
        {
            get { return (String.IsNullOrEmpty(_ImportUrls)) ? String.Empty : _ImportUrls; }
            set
            {
                if (_ImportUrls != value)
                {
                    _ImportUrls = value;
                    OnPropertyChanged("ImportUrls");
                }
            }
        }

        #endregion

        private bool CanParseEndpoints()
        {
            return !String.IsNullOrWhiteSpace(_ImportUrls);
        }

        private void ParseEndpoints()
        {
            int j = 0;
            int i = j;
        }

    }
}
