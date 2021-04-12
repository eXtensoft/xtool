using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace XTool
{
    public class ApiErrorViewModel : ViewModel<Cyclops.ApiError>
    {

        #region Id (int)

        /// <summary>
        /// Gets or sets the int value for Id
        /// </summary>
        /// <value> The int value.</value>

        public long Id
        {
            get { return Model.Id; }
            set
            {
                if (Model.Id != value)
                {
                    Model.Id = value;
                    OnPropertyChanged("Id");
                    IsDirty = true;
                }
            }
        }

        #endregion


        #region CreatedAt (DateTime)

        /// <summary>
        /// Gets or sets the DateTime value for CreatedAt
        /// </summary>
        /// <value> The DateTime value.</value>

        public DateTime CreatedAt
        {
            get { return Model.CreatedAt; }
            set
            {
                if (Model.CreatedAt != value)
                {
                    Model.CreatedAt = value;
                    OnPropertyChanged("CreatedAt");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region Month (string)

        /// <summary>
        /// Gets or sets the string value for Month
        /// </summary>
        /// <value> The string value.</value>

        public string Month
        {
            get { return (String.IsNullOrEmpty(Model.Month)) ? String.Empty : Model.Month; }
            set
            {
                if (Model.Month != value)
                {
                    Model.Month = value;
                    OnPropertyChanged("Month");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region Day (string)

        /// <summary>
        /// Gets or sets the string value for Day
        /// </summary>
        /// <value> The string value.</value>

        public string Day
        {
            get { return (String.IsNullOrEmpty(Model.Day)) ? String.Empty : Model.Day; }
            set
            {
                if (Model.Day != value)
                {
                    Model.Day = value;
                    OnPropertyChanged("Day");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region ApplicationKey (string)

        /// <summary>
        /// Gets or sets the string value for ApplicationKey
        /// </summary>
        /// <value> The string value.</value>

        public string ApplicationKey
        {
            get { return (String.IsNullOrEmpty(Model.ApplicationKey)) ? String.Empty : Model.ApplicationKey; }
            set
            {
                if (Model.ApplicationKey != value)
                {
                    Model.ApplicationKey = value;
                    OnPropertyChanged("ApplicationKey");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region Zone (string)

        /// <summary>
        /// Gets or sets the string value for Zone
        /// </summary>
        /// <value> The string value.</value>

        public string Zone
        {
            get { return (String.IsNullOrEmpty(Model.Zone)) ? String.Empty : Model.Zone; }
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

        #region AppContextInstance (string)

        /// <summary>
        /// Gets or sets the string value for AppContextInstance
        /// </summary>
        /// <value> The string value.</value>

        public string AppContextInstance
        {
            get { return (String.IsNullOrEmpty(Model.AppContextInstance)) ? String.Empty : Model.AppContextInstance; }
            set
            {
                if (Model.AppContextInstance != value)
                {
                    Model.AppContextInstance = value;
                    OnPropertyChanged("AppContextInstance");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region MessageId (Guid)

        /// <summary>
        /// Gets or sets the Guid value for MessageId
        /// </summary>
        /// <value> The Guid value.</value>

        public Guid MessageId
        {
            get { return (Model.MessageId != null) ? Model.MessageId : Guid.Empty; }
            set
            {
                if (Model.MessageId != value)
                {
                    Model.MessageId = value;
                    OnPropertyChanged("MessageId");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region Category (string)

        /// <summary>
        /// Gets or sets the string value for Category
        /// </summary>
        /// <value> The string value.</value>

        public string Category
        {
            get { return (String.IsNullOrEmpty(Model.Category)) ? String.Empty : Model.Category; }
            set
            {
                if (Model.Category != value)
                {
                    Model.Category = value;
                    OnPropertyChanged("Category");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region Severity (string)

        /// <summary>
        /// Gets or sets the string value for Severity
        /// </summary>
        /// <value> The string value.</value>

        public string Severity
        {
            get { return (String.IsNullOrEmpty(Model.Severity)) ? String.Empty : Model.Severity; }
            set
            {
                if (Model.Severity != value)
                {
                    Model.Severity = value;
                    OnPropertyChanged("Severity");
                    IsDirty = true;
                }
            }
        }

        #endregion
        #region Message (string)

        /// <summary>
        /// Gets or sets the string value for Message
        /// </summary>
        /// <value> The string value.</value>

        public string Message
        {
            get { return (String.IsNullOrEmpty(Model.Message)) ? String.Empty : Model.Message; }
            set
            {
                if (Model.Message != value)
                {
                    Model.Message = value;
                    OnPropertyChanged("Message");
                    IsDirty = true;
                }
            }
        }

        #endregion


        #region XmlData (string)

        /// <summary>
        /// Gets or sets the string value for XmlData
        /// </summary>
        /// <value> The string value.</value>

        public string XmlData
        {
            get { return (String.IsNullOrEmpty(Model.XmlData)) ? String.Empty : Model.XmlData; }
            set
            {
                if (Model.XmlData != value)
                {
                    Model.XmlData = value;
                    OnPropertyChanged("XmlData");
                    IsDirty = true;
                }
            }
        }

        #endregion


        #region ApiRequestId (long)

        /// <summary>
        /// Gets or sets the int value for ApiRequestId
        /// </summary>
        /// <value> The int value.</value>

        public long ApiRequestId
        {
            get { return Model.ApiRequestId; }
            set
            {
                if (Model.ApiRequestId != value)
                {
                    Model.ApiRequestId = value;
                    OnPropertyChanged("ApiRequestId");
                    IsDirty = true;
                }
            }
        }

        #endregion


        #region HasApiRequest (bool)

        /// <summary>
        /// Gets or sets the bool value for HasApiRequest
        /// </summary>
        /// <value> The bool value.</value>

        public bool HasApiRequest
        {
            get { return Model.ApiRequestId > 0; }

        }

        #endregion


        #region HasSession (bool)

        /// <summary>
        /// Gets or sets the bool value for HasSession
        /// </summary>
        /// <value> The bool value.</value>

        public bool HasSession
        {
            get { return SessionId > 0; }
        }

        #endregion



        #region SessionId (long)

        /// <summary>
        /// Gets or sets the int value for SessionId
        /// </summary>
        /// <value> The int value.</value>

        public long SessionId
        {
            get { return Model.SessionId; }
            set
            {
                if (Model.SessionId != value)
                {
                    Model.SessionId = value;
                    OnPropertyChanged("SessionId");
                    IsDirty = true;
                }
            }
        }

        #endregion

        public Cyclops.ApiRequest Request { get; set; }

        public Cyclops.ApiSession Session { get; set; }

        //private List<Cyclops.TypedItem> _Items = null;
        //public List<Cyclops.TypedItem> Items
        //{
        //    get
        //    {
        //        if (_Items == null && !String.IsNullOrWhiteSpace(Model.XmlData))
        //        {
        //            _Items = GenericSerializer.StringToGenericList<Cyclops.TypedItem>(Model.XmlData);
        //        }
        //        if (_Items == null)
        //        {
        //            _Items = new List<Cyclops.TypedItem>();
        //        }
        //        return _Items;
        //    }
        //}
        public ApiErrorViewModel(Cyclops.ApiError model)
        {
            Model = model;
        }
    }
}
