using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public class ApiSessionViewModel : ViewModel<Cyclops.ApiSession>
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

        public DateTimeOffset CreatedAt
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



        #region BasicToken (Guid)

        /// <summary>
        /// Gets or sets the Guid value for BasicToken
        /// </summary>
        /// <value> The Guid value.</value>

        public Guid BasicToken
        {
            get { return (Model.BasicToken != null) ? Model.BasicToken : Guid.Empty; }
            set
            {
                if (Model.BasicToken != value)
                {
                    Model.BasicToken = value;
                    OnPropertyChanged("BasicToken");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region BearerToken (string)

        /// <summary>
        /// Gets or sets the string value for BearerToken
        /// </summary>
        /// <value> The string value.</value>

        public string BearerToken
        {
            get { return (String.IsNullOrEmpty(Model.BearerToken)) ? String.Empty : Model.BearerToken; }
            set
            {
                if (Model.BearerToken != value)
                {
                    Model.BearerToken = value;
                    OnPropertyChanged("BearerToken");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region TenantId (int)

        /// <summary>
        /// Gets or sets the int value for TenantId
        /// </summary>
        /// <value> The int value.</value>

        public int TenantId
        {
            get { return Model.TenantId; }
            set
            {
                if (Model.TenantId != value)
                {
                    Model.TenantId = value;
                    OnPropertyChanged("TenantId");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region PatronId (int)

        /// <summary>
        /// Gets or sets the int value for PatronId
        /// </summary>
        /// <value> The int value.</value>

        public int PatronId
        {
            get { return Model.PatronId; }
            set
            {
                if (Model.PatronId != value)
                {
                    Model.PatronId = value;
                    OnPropertyChanged("PatronId");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region SsoPatronId (int)

        /// <summary>
        /// Gets or sets the int value for SsoPatronId
        /// </summary>
        /// <value> The int value.</value>

        public int SsoPatronId
        {
            get { return Model.SsoPatronId; }
            set
            {
                if (Model.SsoPatronId != value)
                {
                    Model.SsoPatronId = value;
                    OnPropertyChanged("SsoPatronId");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region GatewayPatronId (int)

        /// <summary>
        /// Gets or sets the int value for GatewayPatronId
        /// </summary>
        /// <value> The int value.</value>

        public int GatewayPatronId
        {
            get { return Model.GatewayPatronId; }
            set
            {
                if (Model.GatewayPatronId != value)
                {
                    Model.GatewayPatronId = value;
                    OnPropertyChanged("GatewayPatronId");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region IPAddress (string)

        /// <summary>
        /// Gets or sets the string value for IPAddress
        /// </summary>
        /// <value> The string value.</value>

        public string IPAddress
        {
            get { return (String.IsNullOrEmpty(Model.IPAddress)) ? String.Empty : Model.IPAddress; }
            set
            {
                if (Model.IPAddress != value)
                {
                    Model.IPAddress = value;
                    OnPropertyChanged("IPAddress");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region UserAgent (string)

        /// <summary>
        /// Gets or sets the string value for UserAgent
        /// </summary>
        /// <value> The string value.</value>

        public string UserAgent
        {
            get { return (String.IsNullOrEmpty(Model.UserAgent)) ? String.Empty : Model.UserAgent; }
            set
            {
                if (Model.UserAgent != value)
                {
                    Model.UserAgent = value;
                    OnPropertyChanged("UserAgent");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region PassKey (string)

        /// <summary>
        /// Gets or sets the string value for PassKey
        /// </summary>
        /// <value> The string value.</value>

        public string PassKey
        {
            get { return (String.IsNullOrEmpty(Model.PassKey)) ? String.Empty : Model.PassKey; }
            set
            {
                if (Model.PassKey != value)
                {
                    Model.PassKey = value;
                    OnPropertyChanged("PassKey");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region LinesOfBusiness (string)

        /// <summary>
        /// Gets or sets the string value for LinesOfBusiness
        /// </summary>
        /// <value> The string value.</value>

        public string LinesOfBusiness
        {
            get { return (String.IsNullOrEmpty(Model.LinesOfBusiness)) ? String.Empty : Model.LinesOfBusiness; }
            set
            {
                if (Model.LinesOfBusiness != value)
                {
                    Model.LinesOfBusiness = value;
                    OnPropertyChanged("LinesOfBusiness");
                    IsDirty = true;
                }
            }
        }

        #endregion


        #region Tds (DateTime)

        /// <summary>
        /// Gets or sets the DateTime value for Tds
        /// </summary>
        /// <value> The DateTime value.</value>

        public DateTimeOffset Tds
        {
            get { return Model.Tds; }
            set
            {
                if (Model.Tds != value)
                {
                    Model.Tds = value;
                    OnPropertyChanged("Tds");
                    IsDirty = true;
                }
            }
        }

        #endregion


        public ApiSessionViewModel(Cyclops.ApiSession model)
        {
            Model = model;
        }
    }
}
