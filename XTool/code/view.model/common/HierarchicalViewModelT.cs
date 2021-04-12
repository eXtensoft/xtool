using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace XTool
{
    public class HierarchicalViewModel<T> : IViewModel, IHierarchicalViewModel, INotifyPropertyChanged, IDataErrorInfo
    {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion


        #region properties

        #region Validators (PropertyValidatorCollection)

        private PropertyValidatorCollection _Validators = new PropertyValidatorCollection();

        /// <summary>
        /// Gets or sets the PropertyValidatorCollection value for Validators
        /// </summary>
        /// <value> The PropertyValidatorCollection value.</value>

        public PropertyValidatorCollection Validators
        {
            get { return _Validators; }
            set
            {
                if (_Validators != value)
                {
                    _Validators = value;
                }
            }
        }

        #endregion

        #endregion

        #region validation methods

        public bool Validate()
        {
            bool b = true;
            if (_Validators != null && _Validators.Count > 0)
            {
                foreach (PropertyValidator validator in _Validators)
                {
                    if (!String.IsNullOrEmpty(validator.Executor.Invoke()))
                    {
                        b = false;
                    }
                }
            }

            return b;
        }
        public string GetValidationError(string propertyName)
        {
            string error = null;

            if (_Validators.Contains(propertyName))
            {
                error = _Validators[propertyName].Executor.Invoke();
            }
            return error;
        }

        protected string ValidateString(string propertyName, string propertyValue)
        {
            return (String.IsNullOrEmpty(propertyValue)) ? propertyName + " cannot be empty" : null;
        }

        #endregion

        #region IDataErrorInfo Members

        string IDataErrorInfo.Error
        {
            get { return null; }
        }

        string IDataErrorInfo.this[string propertyName]
        {
            get { return GetValidationError(propertyName); }
        }

        #endregion

        public T Model { get; set; }

        #region IHierarchicalViewModel Members

        private bool _IsExpanded;

        public bool IsExpanded
        {
            get { return _IsExpanded; }
            set
            {
                if (_IsExpanded != value)
                {
                    _IsExpanded = value;
                    OnPropertyChanged("IsExpanded");
                }
                if (_IsExpanded && _Master != null)
                {
                    _Master.IsExpanded = true;
                }
            }
        }

        private bool _IsSelected;

        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                if (_IsSelected != value)
                {
                    _IsSelected = value;
                    OnPropertyChanged("IsSelected");
                }
            }
        }

        private IHierarchicalViewModel _Master;

        public IHierarchicalViewModel Master
        {
            get
            {
                return _Master;
            }
            set
            {
                _Master = value;
            }
        }

        #endregion

        #region IViewModel Members
        private bool _IsDirty;
        public bool IsDirty
        {
            get { return _IsDirty; }
            set
            {
                if (_IsDirty != value)
                {
                    _IsDirty = value;
                    OnPropertyChanged("IsDirty");
                }
            }
        }

        private bool _MarkedForRemoval;
        public bool MarkedForRemoval
        {
            get { return _MarkedForRemoval; }
            set
            {
                if (_MarkedForRemoval != value)
                {
                    _MarkedForRemoval = value;
                    OnPropertyChanged("MarkedForRemoval");
                }
            }
        }

        #endregion

        protected virtual IEnumerable<string> GetProperties()
        {
            return new List<string>() { "IsDirty", "MarkedForRemoval" };
        }

    }
}
