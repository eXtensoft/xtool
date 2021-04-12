using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public class ApiRequestViewModel : ViewModel<Cyclops.ApiRequest>
    {

        #region ApiRequestId (int)

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

        #region AppKey (string)

        /// <summary>
        /// Gets or sets the string value for AppKey
        /// </summary>
        /// <value> The string value.</value>

        public string AppKey
        {
            get { return (String.IsNullOrEmpty(Model.AppKey)) ? String.Empty : Model.AppKey; }
            set
            {
                if (Model.AppKey != value)
                {
                    Model.AppKey = value;
                    OnPropertyChanged("AppKey");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region AppZone (string)

        /// <summary>
        /// Gets or sets the string value for AppZone
        /// </summary>
        /// <value> The string value.</value>

        public string AppZone
        {
            get { return (String.IsNullOrEmpty(Model.AppZone)) ? String.Empty : Model.AppZone; }
            set
            {
                if (Model.AppZone != value)
                {
                    Model.AppZone = value;
                    OnPropertyChanged("AppZone");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region AppInstance (string)

        /// <summary>
        /// Gets or sets the string value for AppInstance
        /// </summary>
        /// <value> The string value.</value>

        public string AppInstance
        {
            get { return (String.IsNullOrEmpty(Model.AppInstance)) ? String.Empty : Model.AppInstance; }
            set
            {
                if (Model.AppInstance != value)
                {
                    Model.AppInstance = value;
                    OnPropertyChanged("AppInstance");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region Elapsed (decimal)

        /// <summary>
        /// Gets or sets the int value for Elaped
        /// </summary>
        /// <value> The int value.</value>

        public decimal Elapsed
        {
            get { return Model.Elapsed; }
            set
            {
                if (Model.Elapsed != value)
                {
                    Model.Elapsed = value;
                    OnPropertyChanged("Elaped");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region Start (DateTime)

        /// <summary>
        /// Gets or sets the DateTime value for Start
        /// </summary>
        /// <value> The DateTime value.</value>

        public DateTime Start
        {
            get { return Model.Start; }
            set
            {
                if (Model.Start != value)
                {
                    Model.Start = value;
                    OnPropertyChanged("Start");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region Protocol (string)

        /// <summary>
        /// Gets or sets the string value for Protocol
        /// </summary>
        /// <value> The string value.</value>

        public string Protocol
        {
            get { return (String.IsNullOrEmpty(Model.Protocol)) ? String.Empty : Model.Protocol; }
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

        #region Path (string)

        /// <summary>
        /// Gets or sets the string value for Path
        /// </summary>
        /// <value> The string value.</value>

        public string Path
        {
            get { return (String.IsNullOrEmpty(Model.Path)) ? String.Empty : Model.Path; }
            set
            {
                if (Model.Path != value)
                {
                    Model.Path = value;
                    OnPropertyChanged("Path");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region ClientIP (string)

        /// <summary>
        /// Gets or sets the string value for ClientIP
        /// </summary>
        /// <value> The string value.</value>

        public string ClientIP
        {
            get { return (String.IsNullOrEmpty(Model.ClientIP)) ? String.Empty : Model.ClientIP; }
            set
            {
                if (Model.ClientIP != value)
                {
                    Model.ClientIP = value;
                    OnPropertyChanged("ClientIP");
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

        #region HttpMethod (string)

        /// <summary>
        /// Gets or sets the string value for HttpMethod
        /// </summary>
        /// <value> The string value.</value>

        public string HttpMethod
        {
            get { return (String.IsNullOrEmpty(Model.HttpMethod)) ? String.Empty : Model.HttpMethod; }
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

        #region ControllerName (string)

        /// <summary>
        /// Gets or sets the string value for ControllerName
        /// </summary>
        /// <value> The string value.</value>

        public string ControllerName
        {
            get { return (String.IsNullOrEmpty(Model.ControllerName)) ? String.Empty : Model.ControllerName; }
            set
            {
                if (Model.ControllerName != value)
                {
                    Model.ControllerName = value;
                    OnPropertyChanged("ControllerName");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region ControllerMethod (string)

        /// <summary>
        /// Gets or sets the string value for ControllerMethod
        /// </summary>
        /// <value> The string value.</value>

        public string ControllerMethod
        {
            get { return (String.IsNullOrEmpty(Model.ControllerMethod)) ? String.Empty : Model.ControllerMethod; }
            set
            {
                if (Model.ControllerMethod != value)
                {
                    Model.ControllerMethod = value;
                    OnPropertyChanged("ControllerMethod");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region MethodReturnType (string)

        /// <summary>
        /// Gets or sets the string value for MethodReturnType
        /// </summary>
        /// <value> The string value.</value>

        public string MethodReturnType
        {
            get { return (String.IsNullOrEmpty(Model.MethodReturnType)) ? String.Empty : Model.MethodReturnType; }
            set
            {
                if (Model.MethodReturnType != value)
                {
                    Model.MethodReturnType = value;
                    OnPropertyChanged("MethodReturnType");
                    IsDirty = true;
                }
            }
        }

        #endregion
        #region ResponseCode (string)

        /// <summary>
        /// Gets or sets the string value for ResponseCode
        /// </summary>
        /// <value> The string value.</value>

        public string ResponseCode
        {
            get { return (String.IsNullOrEmpty(Model.ResponseCode)) ? String.Empty : Model.ResponseCode; }
            set
            {
                if (Model.ResponseCode != value)
                {
                    Model.ResponseCode = value;
                    OnPropertyChanged("ResponseCode");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region ResponseText (string)

        /// <summary>
        /// Gets or sets the string value for ResponseText
        /// </summary>
        /// <value> The string value.</value>

        public string ResponseText
        {
            get { return (String.IsNullOrEmpty(Model.ResponseText)) ? String.Empty : Model.ResponseText; }
            set
            {
                if (Model.ResponseText != value)
                {
                    Model.ResponseText = value;
                    OnPropertyChanged("ResponseText");
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

        #region BasicToken (string)

        /// <summary>
        /// Gets or sets the string value for BasicToken
        /// </summary>
        /// <value> The string value.</value>

        public string BasicToken
        {
            get { return (String.IsNullOrEmpty(Model.BasicToken)) ? String.Empty : Model.BasicToken; }
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

        #region AuthSchema (string)

        /// <summary>
        /// Gets or sets the string value for AuthSchema
        /// </summary>
        /// <value> The string value.</value>

        public string AuthSchema
        {
            get { return (String.IsNullOrEmpty(Model.AuthSchema)) ? String.Empty : Model.AuthSchema; }
            set
            {
                if (Model.AuthSchema != value)
                {
                    Model.AuthSchema = value;
                    OnPropertyChanged("AuthSchema");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region AuthValue (string)

        /// <summary>
        /// Gets or sets the string value for AuthValue
        /// </summary>
        /// <value> The string value.</value>

        public string AuthValue
        {
            get { return (String.IsNullOrEmpty(Model.AuthValue)) ? String.Empty : Model.AuthValue; }
            set
            {
                if (Model.AuthValue != value)
                {
                    Model.AuthValue = value;
                    OnPropertyChanged("AuthValue");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region MessageBody (string)

        /// <summary>
        /// Gets or sets the string value for MessageBody
        /// </summary>
        /// <value> The string value.</value>

        public string MessageBody
        {
            get { return (String.IsNullOrEmpty(Model.MessageBody)) ? String.Empty : Model.MessageBody; }
            set
            {
                if (Model.MessageBody != value)
                {
                    Model.MessageBody = value;
                    OnPropertyChanged("MessageBody");
                    IsDirty = true;
                }
            }
        }

        #endregion


        public ApiRequestViewModel(Cyclops.ApiRequest model)
        {
            Model = model;
        }
    }
}
