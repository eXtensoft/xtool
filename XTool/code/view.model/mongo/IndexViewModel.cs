// <copyright file="IndexViewModel.cs" company="eXtensible Solutions LLC">
// Copyright © 2015 All Right Reserved
// </copyright>

namespace XTool
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;


    public sealed class IndexViewModel : ViewModel<XTool.Mongo.Index>
    {

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

        #region ToDisplay (string)

        /// <summary>
        /// Gets or sets the string value for ToDisplay
        /// </summary>
        /// <value> The string value.</value>

        public string ToDisplay
        {
            get { return Model.ToString(); }

        }

        #endregion

        public IndexViewModel(XTool.Mongo.Index model)
        {
            Model = model;
        }

    }
}
