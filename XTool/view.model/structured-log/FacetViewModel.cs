using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public class FacetViewModel : ViewModel<Cyclops.TypedItem>
    {

        #region Key (string)

        /// <summary>
        /// Gets or sets the string value for Key
        /// </summary>
        /// <value> The string value.</value>

        public string Key
        {
            get { return (String.IsNullOrEmpty(Model.Domain)) ? String.Empty : Model.Domain; }
        }

        #endregion

        #region Value (string)

        /// <summary>
        /// Gets or sets the string value for Value
        /// </summary>
        /// <value> The string value.</value>

        public string Value
        {
            get { return (String.IsNullOrEmpty(Model.Key)) ? String.Empty : Model.Key; }
        }

        #endregion

        #region Count (int)

        /// <summary>
        /// Gets or sets the int value for Count
        /// </summary>
        /// <value> The int value.</value>

        public int Count
        {
            get { return (int)Model.Value; }
        }

        #endregion

        #region IsSelected (bool)

        /// <summary>
        /// Gets or sets the bool value for IsSelected
        /// </summary>
        /// <value> The bool value.</value>

        public new bool IsSelected
        {
            get { return Model.IsSelected; }
            set
            {
                if (Model.IsSelected != value)
                {
                    Model.IsSelected = value;
                    OnPropertyChanged("IsSelected");
                    IsDirty = true;
                }
            }
        }

        #endregion

        public FacetViewModel(Cyclops.TypedItem model)
        {
            Model = model;
        }

        public FacetViewModel(Cyclops.TypedItem model, bool isSelected)
        {
            Model = model;
            IsSelected = true;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
