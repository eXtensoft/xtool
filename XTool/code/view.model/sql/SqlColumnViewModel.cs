using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public class SqlColumnViewModel : ViewModel<Discovery.SqlColumn>
    {
        #region properties

        #region ColumnName (string)

        /// <summary>
        /// Gets or sets the string value for ColumnName
        /// </summary>
        /// <value> The string value.</value>

        public string ColumnName
        {
            get { return (String.IsNullOrEmpty(Model.ColumnName)) ? String.Empty : Model.ColumnName; }
            set
            {
                if (Model.ColumnName != value)
                {
                    Model.ColumnName = value;
                    OnPropertyChanged("ColumnName");
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

        #region DefaultValue (string)

        /// <summary>
        /// Gets or sets the string value for DefaultValue
        /// </summary>
        /// <value> The string value.</value>

        public string DefaultValue
        {
            get { return (String.IsNullOrEmpty(Model.DefaultValue)) ? String.Empty : Model.DefaultValue; }
            set
            {
                if (Model.DefaultValue != value)
                {
                    Model.DefaultValue = value;
                    OnPropertyChanged("DefaultValue");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region IsNullible (bool)

        /// <summary>
        /// Gets or sets the bool value for IsNullible
        /// </summary>
        /// <value> The bool value.</value>

        public bool IsNullible
        {
            get { return Model.IsNullible; }
            set
            {
                if (Model.IsNullible != value)
                {
                    Model.IsNullible = value;
                    OnPropertyChanged("IsNullible");
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

        #region ToDisplay (string)

        /// <summary>
        /// Gets or sets the string value for ToDisplay
        /// </summary>
        /// <value> The string value.</value>

        public string ToDisplay
        {
            get { return (String.IsNullOrEmpty(Model.ToDisplay)) ? String.Empty : Model.ToDisplay; }
        }

        public string ToSprocDatatype
        {
            get { return (String.IsNullOrWhiteSpace(Model.ToSprocDatatype())) ? String.Empty : Model.ToSprocDatatype(); }
        }

        #endregion

        #region IsComputed (bool)

        /// <summary>
        /// Gets or sets the bool value for IsComputed
        /// </summary>
        /// <value> The bool value.</value>

        public bool IsComputed
        {
            get { return Model.IsComputed; }
            set
            {
                if (Model.IsComputed != value)
                {
                    Model.IsComputed = value;
                    OnPropertyChanged("IsComputed");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region IsIdentity (bool)

        /// <summary>
        /// Gets or sets the bool value for IsIdentity
        /// </summary>
        /// <value> The bool value.</value>

        public bool IsIdentity
        {
            get { return Model.IsIdentity; }
            set
            {
                if (Model.IsIdentity != value)
                {
                    Model.IsIdentity = value;
                    OnPropertyChanged("IsIdentity");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region IsPrimaryKey (bool)

        /// <summary>
        /// Gets or sets the bool value for IsPrimaryKey
        /// </summary>
        /// <value> The bool value.</value>

        public bool IsPrimaryKey
        {
            get { return Model.IsPrimaryKey; }
            set
            {
                if (Model.IsPrimaryKey != value)
                {
                    Model.IsPrimaryKey = value;
                    OnPropertyChanged("IsPrimaryKey");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region IsForeignKey (bool)

        /// <summary>
        /// Gets or sets the bool value for IsForeignKey
        /// </summary>
        /// <value> The bool value.</value>

        public bool IsForeignKey
        {
            get { return Model.IsForeignKey; }
            set
            {
                if (Model.IsForeignKey != value)
                {
                    Model.IsForeignKey = value;
                    OnPropertyChanged("IsForeignKey");
                    IsDirty = true;
                }
            }
        }

        #endregion

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
                    IsDirty = true;
                }
            }
        }

        #endregion

        public System.Data.DbType DbType
        {
            get
            {
                return DataMapProvider.TranslateToDbType(Datatype);
            }
        }


        #endregion


        #region IsEncrypt (bool)

        /// <summary>
        /// Gets or sets the bool value for IsEncrypt
        /// </summary>
        /// <value> The bool value.</value>
        private bool _IsEncrypt;
        public bool IsEncrypt
        {
            get { return _IsEncrypt; }
            set
            {
                if (_IsEncrypt != value)
                {
                    _IsEncrypt = value;
                    OnPropertyChanged("IsEncrypt");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region constructors
        public SqlColumnViewModel(Discovery.SqlColumn model)
        {
            Model = model;
        }
        #endregion


        public void GenerateDataDictionaryEntry(System.Data.DataTable dt)
        {
            System.Data.DataRow r = dt.NewRow();
            r["ColumnName"] = ColumnName;
            r["OrdinalPosition"] = OrdinalPosition;
            //r["MaxLength"] = (MaxLength == -1) ? "MAX" : ((MaxLength == 0) ? String.Empty : MaxLength.ToString());
            r["AllowNulls"] = IsNullible;
            if (MaxLength == 0)
            {
                r["DataType"] = Datatype;
            }
            else
            {
                string s = (MaxLength == -1) ? "MAX" : MaxLength.ToString();
                r["DataType"] = String.Format("{0}({1})", Datatype, s);
            }

            r["DefaultValue"] = DefaultValue;
            r["IsComputed"] = IsComputed;
            r["IsIdentity"] = IsIdentity;
            r["IsPrimaryKey"] = IsPrimaryKey;
            r["IsForeignKey"] = IsForeignKey;
            dt.Rows.Add(r);
        }

    }
}

