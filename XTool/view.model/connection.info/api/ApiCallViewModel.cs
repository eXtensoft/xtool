using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public class ApiCallViewModel : ViewModel<ApiCall>
    {

        #region Key (string)

        /// <summary>
        /// Gets or sets the string value for Key
        /// </summary>
        /// <value> The string value.</value>

        public string Key
        {
            get { return (String.IsNullOrEmpty(Model.Key)) ? String.Empty : String.Format("{0}: {1}",Model.Response.ResponseCode,Model.Key); }
            set
            {
                if (Model.Key != value)
                {
                    Model.Key = value;
                    OnPropertyChanged("Key");
                    IsDirty = true;
                }
            }
        }

        #endregion


        #region Message (string)
        private string _Message;
        /// <summary>
        /// Gets or sets the string value for Message
        /// </summary>
        /// <value> The string value.</value>

        public string Message
        {
            get { return (String.IsNullOrEmpty(_Message)) ? String.Empty : _Message; }
            set
            {
                if (_Message != value)
                {
                    _Message = value;
                    OnPropertyChanged("Message");
                    IsDirty = true;
                }
            }
        }

        #endregion


        #region IsOkay (bool)
        private bool _IsOkay;
        /// <summary>
        /// Gets or sets the bool value for IsOkay
        /// </summary>
        /// <value> The bool value.</value>

        public bool IsOkay
        {
            get { return _IsOkay; }
            set
            {
                if (_IsOkay != value)
                {
                    _IsOkay = value;
                    OnPropertyChanged("IsOkay");
                    IsDirty = true;
                }
            }
        }

        #endregion

        public ObservableCollection<JTokenViewModel> Items { get; set; }

        #region response




        #region ResponseBody (string)

        /// <summary>
        /// Gets or sets the string value for ResponseBody
        /// </summary>
        /// <value> The string value.</value>

        public string ResponseBody
        {
            get { return (String.IsNullOrEmpty(Model.Response.ResponseBody)) ? String.Empty : Model.Response.ResponseBody; }
            set
            {
                if (Model.Response.ResponseBody != value)
                {
                    Model.Response.ResponseBody = value;
                    OnPropertyChanged("ResponseBody");
                    IsDirty = true;
                }
            }
        }

        #endregion
        #endregion



        public ApiCallViewModel(ApiCall model)
        {
            Items = new ObservableCollection<JTokenViewModel>();
            Model = model;
            string message;
            JTokenViewModel vm;
            if (!String.IsNullOrWhiteSpace(Model.Response.ResponseBody))
            {
                if(JsonViewParser.Parse(Model.Response.ResponseBody,out vm,out message))
                {
                    Items.Add(vm);
                    _IsOkay = true;
                }
                else
                {
                    _IsOkay = false;
                    _Message = message;
                }
            }
            else
            {

            }
        }
    }
}
