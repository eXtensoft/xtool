// <copyright file="DatabaseViewModel.cs" company="eXtensible Solutions LLC">
// Copyright © 2015 All Right Reserved
// </copyright>

namespace XTool
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Runtime.Serialization;



    public sealed class DatabaseViewModel : ViewModel<XTool.Mongo.Database>
    {
        private string _ConnectionString;

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
        #region Collections (ObservableCollection<CollectionViewModel>)

        private ObservableCollection<CollectionViewModel> _Collections = new ObservableCollection<CollectionViewModel>();

        /// <summary>
        /// Gets or sets the ObservableCollection<CollectionViewModel> value for Collections
        /// </summary>
        /// <value> The ObservableCollection<CollectionViewModel> value.</value>

        public ObservableCollection<CollectionViewModel> Collections
        {
            get { return _Collections; }
            set
            {
                if (_Collections != value)
                {
                    _Collections = value;
                }
            }
        }

        #endregion

        public DatabaseViewModel(XTool.Mongo.Database model, string connectionString, List<MongoDbCommand> commands)
        {
            Model = model;
            if (model.Collections != null)
            {
                foreach (var item in model.Collections)
                {
                    item.Commands = commands.Where(x => x.Collection.Equals(item.FullName, StringComparison.OrdinalIgnoreCase)).ToList();
                    Collections.Add(new CollectionViewModel(item,connectionString,commands));
                }
            }
        }
    }
}
