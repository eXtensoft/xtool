using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public class SelectItemViewModel : ViewModel<SelectItem>
    {
        #region Display (string)

        /// <summary>
        /// Gets or sets the string value for Display
        /// </summary>
        /// <value> The string value.</value>

        public string Display
        {
            get { return (String.IsNullOrEmpty(Model.Display)) ? String.Empty : Model.Display; }
            set
            {
                if (Model.Display != value)
                {
                    Model.Display = value;
                    OnPropertyChanged("Display");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region GroupName (string)

        /// <summary>
        /// Gets or sets the string value for GroupName
        /// </summary>
        /// <value> The string value.</value>

        public string GroupName
        {
            get { return (String.IsNullOrEmpty(Model.GroupName)) ? String.Empty : Model.GroupName; }
            set
            {
                if (Model.GroupName != value)
                {
                    Model.GroupName = value;
                    OnPropertyChanged("GroupName");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region Value (string)

        private string _Value;

        /// <summary>
        /// Gets or sets the string value for Value
        /// </summary>
        /// <value> The string value.</value>

        public string Value
        {
            get { return (String.IsNullOrEmpty(_Value)) ? String.Empty : _Value; }
            set
            {
                if (_Value != value)
                {
                    _Value = value;
                }
            }
        }

        #endregion

        public SelectItemViewModel() { }

        public SelectItemViewModel(SelectItem model)
        {
            Model = model;
        }
    }
}
