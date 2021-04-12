using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace XTool
{
    public class DataColumnViewModel : ViewModel<DataColumn>
    {


        private DataTableViewModel _Master = null;

        public int MaxLength { get; set; }

        #region Label (string)

        private string _Label;

        /// <summary>
        /// Gets or sets the string value for Label
        /// </summary>
        /// <value> The string value.</value>

        public string Label
        {
            get { return (String.IsNullOrEmpty(_Label)) ? String.Empty : _Label; }
        }

        #endregion


        #region Name (string)

        /// <summary>
        /// Gets or sets the string value for ColumnName
        /// </summary>
        /// <value> The string value.</value>

        public string Name
        {
            get { return (String.IsNullOrEmpty(Model.ColumnName)) ? String.Empty : Model.ColumnName; }
            set
            {
                if (Model.ColumnName != value)
                {
                    Model.ColumnName = value;
                    OnPropertyChanged("Name");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region Alias (string)

        private string _Alias;

        /// <summary>
        /// Gets or sets the string value for Alias
        /// </summary>
        /// <value> The string value.</value>

        public string Alias
        {
            get { return (String.IsNullOrEmpty(_Alias)) ? String.Empty : _Alias; }
            set
            {
                if (_Alias != value)
                {
                    _Alias = value;
                    OnPropertyChanged("Alias");
                    OnPropertyChanged("Display");
                }
            }
        }

        #endregion

        #region AsAttribute (bool)

        private bool _AsAttribute;

        /// <summary>
        /// Gets or sets the bool value for AsAttribute
        /// </summary>
        /// <value> The bool value.</value>

        public bool AsAttribute
        {
            get { return _AsAttribute; }
            set
            {
                if (_AsAttribute != value)
                {
                    _AsAttribute = value;
                    OnPropertyChanged("AsAttribute");
                }
            }
        }

        #endregion

        #region IsChecked (bool)

        private bool _IsChecked = true;

        /// <summary>
        /// Gets or sets the bool value for IsChecked
        /// </summary>
        /// <value> The bool value.</value>

        public bool IsChecked
        {
            get { return _IsChecked; }
            set
            {
                if (_IsChecked != value)
                {
                    _IsChecked = value;
                    _Master.MapColumn(Name, value);
                    OnPropertyChanged("IsChecked");
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
            set
            {
                _Master.IsSelected = value;
            }
        }

        #endregion

        public string Display
        {
            get
            {
                string s = (!String.IsNullOrEmpty(Alias)) ? Alias : Name;
                return s.ToAlphaNumericOnly();
            }
        }



        public DataColumnViewModel(DataColumn model, DataTableViewModel master)
        {
            _Label = "Column";
            Model = model;
            _Master = master;
        }
    }
}
