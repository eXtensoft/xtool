// <copyright company="Recorded Books, Inc" file="ComparisonFileInfoViewModel.cs">
// Copyright © 2015 All Rights Reserved
// </copyright>

namespace XTool
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ComparisonFileInfoViewModel : ViewModel<FileInfo>
    {
        public string AsOf
        {
            get
            {
                return _EffectiveOn.ToShortDateString();
            }
        }

        #region EffectiveOn (DateTime)

        private DateTime _EffectiveOn;

        /// <summary>
        /// Gets or sets the DateTime value for EffectiveOn
        /// </summary>
        /// <value> The DateTime value.</value>

        public DateTime EffectiveOn
        {
            get { return (_EffectiveOn != null) ? _EffectiveOn : DateTime.MinValue; }
            set
            {
                if (_EffectiveOn != value)
                {
                    _EffectiveOn = value;
                    OnPropertyChanged("EffectiveOn;");
                    OnPropertyChanged("AsOf");
                }
            }
        }

        #endregion

        #region Server (string)

        private string _Server;

        /// <summary>
        /// Gets or sets the string value for Server
        /// </summary>
        /// <value> The string value.</value>

        public string Server
        {
            get { return (String.IsNullOrEmpty(_Server)) ? String.Empty : _Server; }
            set
            {
                if (_Server != value)
                {
                    OnPropertyChanged("Server");
                    _Server = value;
                }
            }
        }

        #endregion

        #region Catalog (string)

        private string _Catalog;

        /// <summary>
        /// Gets or sets the string value for Catalog
        /// </summary>
        /// <value> The string value.</value>

        public string Catalog
        {
            get { return (String.IsNullOrEmpty(_Catalog)) ? String.Empty : _Catalog; }
            set
            {
               
                if (_Catalog != value)
                {    OnPropertyChanged("Catalog");
                    _Catalog = value;
                }
            }
        }

        #endregion

        #region Schema (string)

        private string _Schema;

        /// <summary>
        /// Gets or sets the string value for Schema
        /// </summary>
        /// <value> The string value.</value>

        public string Schema
        {
            get { return (String.IsNullOrEmpty(_Schema)) ? String.Empty : _Schema; }
            set
            {
                if (_Schema != value)
                {
                    OnPropertyChanged("Schema");
                    _Schema = value;
                }
            }
        }

        #endregion

        #region Table (string)

        private string _Table;

        /// <summary>
        /// Gets or sets the string value for Table
        /// </summary>
        /// <value> The string value.</value>

        public string Table
        {
            get { return (String.IsNullOrEmpty(_Table)) ? String.Empty : _Table; }
            set
            {
                if (_Table != value)
                {
                    OnPropertyChanged("Table");
                    _Table = value;
                }
            }
        }

        #endregion


        #region Name (string)

        /// <summary>
        /// Gets or sets the string value for Name
        /// </summary>
        /// <value> The string value.</value>

        public string Name
        {
            get { return (String.IsNullOrEmpty(Model.Name)) ? String.Empty : Model.Name; }
        }

        #endregion

        #region FullName (string)

        /// <summary>
        /// Gets or sets the string value for FullName
        /// </summary>
        /// <value> The string value.</value>

        public string FullName
        {
            get { return (String.IsNullOrEmpty(Model.FullName)) ? String.Empty : Model.FullName; }
        }

        #endregion

        #region IsSelected (bool)

        /// <summary>
        /// Gets or sets the bool value for IsSelected
        /// </summary>
        /// <value> The bool value.</value>
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
                    IsDirty = true;
                }
            }
        }

        #endregion

        public ComparisonFileInfoViewModel() { }

        public ComparisonFileInfoViewModel(FileInfo model)
        {
            Model = model;
            string[] t = model.Name.Split('-');
            if (t.Length == 5)
            {
                Server = t[0];
                Catalog = t[1];
                Schema = t[2];
                Table = t[3];
                string[] s = t[4].Split('.');
                int y, m, d;
                if (Int32.TryParse(s[0], out y) && Int32.TryParse(s[1], out m) && Int32.TryParse(s[2],out d))
                {
                    EffectiveOn = new DateTime(y, m, d);
                }
            }
            
        }
    }

}
