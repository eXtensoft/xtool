using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public class FilterValueViewModel : ViewModel<FilterValue>
    {
        private Action _NotifyChanged = null;
        #region IsSelected (bool)

        /// <summary>
        /// Gets or sets the bool value for IsSelected
        /// </summary>
        /// <value> The bool value.</value>

        public bool IsSelected
        {
            get { return Model.IsSelected; }
            set
            {
                if (Model.IsSelected != value)
                {
                    Model.IsSelected = value;
                    OnPropertyChanged("IsSelected");
                    if (_NotifyChanged != null)
                    {
                        _NotifyChanged.Invoke();
                    }
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region Key (string)

        /// <summary>
        /// Gets or sets the string value for Key
        /// </summary>
        /// <value> The string value.</value>

        public string Key
        {
            get { return (String.IsNullOrEmpty(Model.Key)) ? String.Empty : Model.Key; }
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

        public FilterValueViewModel(FilterValue model,Action notifyChanged)
        {
            Model = model;
            _NotifyChanged = notifyChanged;
        }
    }
}
