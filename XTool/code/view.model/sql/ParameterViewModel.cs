using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public class ParameterViewModel : ViewModel<Inference.Parameter>
    {
        #region properties

        #region Name (string)

        /// <summary>
        /// Gets or sets the string value for Name
        /// </summary>
        /// <value> The string value.</value>

        public string Name
        {
            get { return (String.IsNullOrEmpty(Model.Name)) ? String.Empty : Model.Name; }
            set
            {
                if (Model.Name != value)
                {
                    Model.Name = value;
                    OnPropertyChanged("Name");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region Mode (string)

        /// <summary>
        /// Gets or sets the string value for Mode
        /// </summary>
        /// <value> The string value.</value>

        public string Mode
        {
            get { return (String.IsNullOrEmpty(Model.Mode)) ? String.Empty : Model.Mode; }
            set
            {
                if (Model.Mode != value)
                {
                    Model.Mode = value;
                    OnPropertyChanged("Mode");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region DataType (string)

        /// <summary>
        /// Gets or sets the string value for DataType
        /// </summary>
        /// <value> The DbType value.</value>

        public System.Data.DbType DataType
        {
            get { return Model.DataType; }
            set
            {
                if (Model.DataType != value)
                {
                    Model.DataType = value;
                    OnPropertyChanged("DataType");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region Value (object)


        /// <summary>
        /// Gets or sets the object value for Value
        /// </summary>
        /// <value> The object value.</value>

        public object Value
        {
            get { return Model.Value; }
            set
            {
                if (Model.Value != value)
                {
                    Model.Value = value;
                    OnPropertyChanged("Value");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #endregion


        #region constructors

        public ParameterViewModel(Inference.Parameter model)
        {
            Model = model;
        }

        #endregion

    }
}
