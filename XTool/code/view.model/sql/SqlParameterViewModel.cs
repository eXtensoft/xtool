using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public class SqlParameterViewModel : ViewModel<Discovery.SqlParameter>
    {
        #region Schema (string)

        /// <summary>
        /// Gets or sets the string value for Schema
        /// </summary>
        /// <value> The string value.</value>

        public string Schema
        {
            get { return (String.IsNullOrEmpty(Model.Schema)) ? String.Empty : Model.Schema; }
            set
            {
                if (Model.Schema != value)
                {
                    Model.Schema = value;
                    OnPropertyChanged("Schema");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region StoredProcedureName (string)

        /// <summary>
        /// Gets or sets the string value for StoredProcedureName
        /// </summary>
        /// <value> The string value.</value>

        public string StoredProcedureName
        {
            get { return (String.IsNullOrEmpty(Model.StoredProcedureName)) ? String.Empty : Model.StoredProcedureName; }
            set
            {
                if (Model.StoredProcedureName != value)
                {
                    Model.StoredProcedureName = value;
                    OnPropertyChanged("StoredProcedureName");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region OrdinalPosition (int)

        /// <summary>
        /// Gets or sets the int value for OrdinalPosition
        /// </summary>
        /// <value> The int value.</value>

        public int OrdinalPosition
        {
            get { return Model.OrdinalPosition; }
            set
            {
                if (Model.OrdinalPosition != value)
                {
                    Model.OrdinalPosition = value;
                    OnPropertyChanged("OrdinalPosition");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region ParamName (string)

        /// <summary>
        /// Gets or sets the string value for ParamName
        /// </summary>
        /// <value> The string value.</value>

        public string ParamName
        {
            get { return (String.IsNullOrEmpty(Model.ParamName)) ? String.Empty : Model.ParamName; }
            set
            {
                if (Model.ParamName != value)
                {
                    Model.ParamName = value;
                    OnPropertyChanged("ParamName");
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

        #region MaxLength (int)

        /// <summary>
        /// Gets or sets the int value for MaxLength
        /// </summary>
        /// <value> The int value.</value>

        public int MaxLength
        {
            get { return Model.MaxLength; }
            set
            {
                if (Model.MaxLength != value)
                {
                    Model.MaxLength = value;
                    OnPropertyChanged("MaxLength");
                    IsDirty = true;
                }
            }
        }

        #endregion


        #region constructors

        public SqlParameterViewModel(Discovery.SqlParameter model)
        {
            Model = model;
        }
        #endregion


    }
}
