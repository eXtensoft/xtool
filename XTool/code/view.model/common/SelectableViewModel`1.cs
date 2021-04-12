//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.ComponentModel;

//namespace XTool
//{
//    /// <summary>
//    /// This class provides the basis on which a Model can be bound to a
//    /// XAML based UX in an operational sense, yet remain logically separate.
//    /// This class implements various notification commonalities found in most
//    /// scenarios.
//    /// </summary>
//    /// <typeparam name="T"></typeparam>
//    public class SelectableViewModel<T> : IViewModel, IDisposable, INotifyPropertyChanged, IDataErrorInfo
//    {
//        #region properties

//        #region Validators (PropertyValidatorCollection)

//        private PropertyValidatorCollection _Validators = new PropertyValidatorCollection();

//        /// <summary>
//        /// Gets or sets the PropertyValidatorCollection value for Validators
//        /// </summary>
//        /// <value> The PropertyValidatorCollection value.</value>

//        public PropertyValidatorCollection Validators
//        {
//            get { return _Validators; }
//            set
//            {
//                if (_Validators != value)
//                {
//                    _Validators = value;
//                }
//            }
//        }

//        #endregion

//        #endregion

//        #region validation methods

//        public bool Validate()
//        {
//            bool b = true;
//            if (_Validators != null && _Validators.Count > 0)
//            {
//                foreach (PropertyValidator validator in _Validators)
//                {
//                    if (!String.IsNullOrEmpty(validator.Executor.Invoke()))
//                    {
//                        b = false;
//                    }
//                }
//            }

//            return b;
//        }
//        public string GetValidationError(string propertyName)
//        {
//            string error = null;

//            if (_Validators.Contains(propertyName))
//            {
//                error = _Validators[propertyName].Executor.Invoke();
//            }
//            return error;
//        }

//        protected string ValidateString(string propertyName, string propertyValue)
//        {
//            return (String.IsNullOrEmpty(propertyValue)) ? propertyName + " cannot be empty" : null;
//        }

//        #endregion

//        #region IDataErrorInfo Members

//        string IDataErrorInfo.Error
//        {
//            get { return null; }
//        }

//        string IDataErrorInfo.this[string propertyName]
//        {
//            get { return GetValidationError(propertyName); }
//        }

//        #endregion


//        public T Model { get; set; }

//        #region IViewModel Members
//        private bool _IsDirty;
//        /// <summary>
//        /// This property indicates if the Model has changed.  The ViewModel
//        /// implementation must set this property whenever a change is made for
//        /// which a state change is desirable.
//        /// </summary>
//        public bool IsDirty
//        {
//            get { return _IsDirty; }
//            set
//            {
//                if (_IsDirty != value)
//                {
//                    _IsDirty = value;
//                    OnPropertyChanged("IsDirty");
//                }
//            }
//        }


//        private bool _MarkedForRemoval;
//        /// <summary>
//        /// This property marks the Model for removal.
//        /// </summary>
//        public bool MarkedForRemoval
//        {
//            get { return _MarkedForRemoval; }
//            set
//            {
//                if (_MarkedForRemoval != value)
//                {
//                    _MarkedForRemoval = value;
//                    OnPropertyChanged("MarkedForRemoval");
//                }
//            }
//        }

//        #endregion

//        /// <summary>
//        /// This property exposes a string list of the ViewModel's properties
//        /// in order to facilitate UX form validation operations.
//        /// </summary>
//        /// <returns></returns>
//        protected virtual IEnumerable<string> GetProperties()
//        {
//            return new List<string>() { "IsDirty", "MarkedForRemoval" };
//        }

//        public virtual void ResetInternalViewModels()
//        {
//        }

//        /// <summary>
//        /// This method refreshes form validation logic by
//        /// firing the INotifyPropertyChanged implementor
//        /// </summary>
//        public void Refresh()
//        {
//            var properties = GetProperties();
//            if (properties != null)
//            {
//                foreach (var property in properties)
//                {
//                    OnPropertyChanged(property);
//                }
//            }
//        }

//        #region IDisposable Members

//        void IDisposable.Dispose()
//        {
//            this.OnDispose();
//        }



//        protected virtual void OnDispose()
//        {
//        }
//#if DEBUG

//        ~SelectableViewModel()
//        {
//            string s = (this.Model != null) ? this.Model.ToString() : "Empty Model";
//            string msg = string.Format("{0} ({1}) ({2}) Finalized", this.GetType().Name, s, this.GetHashCode());
//            System.Diagnostics.Debug.WriteLine(msg);
//        }

//#endif


//        #endregion

//        #region INotifyPropertyChanged Members

//        public event PropertyChangedEventHandler PropertyChanged;

//        public virtual void OnPropertyChanged(string propertyName)
//        {
//            if (PropertyChanged != null)
//            {
//                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
//            }
//        }

//        #endregion

//    }
//}
