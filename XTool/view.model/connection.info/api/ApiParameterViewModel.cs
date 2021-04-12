using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public class ApiParameterViewModel : ViewModel<ApiParameter>
    {



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



        #region Datatype (string)

        /// <summary>
        /// Gets or sets the string value for Datatype
        /// </summary>
        /// <value> The string value.</value>

        public string Datatype
        {
            get { return (String.IsNullOrEmpty(Model.Datatype)) ? String.Empty : Model.Datatype; }
            set
            {
                if (Model.Datatype != value)
                {
                    Model.Datatype = value;
                    OnPropertyChanged("Datatype");
                    IsDirty = true;
                }
            }
        }

        #endregion


        #region Source (string)

        /// <summary>
        /// Gets or sets the string value for Source
        /// </summary>
        /// <value> The string value.</value>

        public string Source
        {
            get { return (String.IsNullOrEmpty(Model.Source)) ? String.Empty : Model.Source; }
            set
            {
                if (Model.Source != value)
                {
                    Model.Source = value;
                    OnPropertyChanged("Source");
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

        public ApiParameterViewModel(ApiParameter model)
        {
            Model = model;
        }


    }
}
