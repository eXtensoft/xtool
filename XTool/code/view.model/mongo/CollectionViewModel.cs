// <copyright file="CollectionViewModel.cs" company="eXtensible Solutions LLC">
// Copyright © 2015 All Right Reserved
// </copyright>

namespace XTool
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization;
    using MongoDB.Driver;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Windows.Input;


    public sealed class CollectionViewModel : ViewModel<XTool.Mongo.Collection>
    {
        #region local fields

        private string _Database = String.Empty;
        private string _ConnectionString = String.Empty;
        private List<MongoDbCommand> commands = null;

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

        #region FullName (string)

        /// <summary>
        /// Gets or sets the string value for FullName
        /// </summary>
        /// <value> The string value.</value>

        public string FullName
        {
            get { return (String.IsNullOrEmpty(Model.FullName)) ? String.Empty : Model.FullName; }
            set
            {
                if (Model.FullName != value)
                {
                    Model.FullName = value;
                    OnPropertyChanged("FullName");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region AvgDocumentSize (int)

        /// <summary>
        /// Gets or sets the int value for AvgDocumentSize
        /// </summary>
        /// <value> The int value.</value>

        public double AvgDocumentSize
        {
            get { return Model.AvgDocumentSize; }
            set
            {
                if (Model.AvgDocumentSize != value)
                {
                    Model.AvgDocumentSize = value;
                    OnPropertyChanged("AvgDocumentSize");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region DataSize (int)

        /// <summary>
        /// Gets or sets the int value for DataSize
        /// </summary>
        /// <value> The int value.</value>

        public long DataSize
        {
            get { return Model.DataSize; }
            set
            {
                if (Model.DataSize != value)
                {
                    Model.DataSize = value;
                    OnPropertyChanged("DataSize");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region IndexCount (int)

        /// <summary>
        /// Gets or sets the int value for IndexCount
        /// </summary>
        /// <value> The int value.</value>

        public int IndexCount
        {
            get { return Model.IndexCount; }
            set
            {
                if (Model.IndexCount != value)
                {
                    Model.IndexCount = value;
                    OnPropertyChanged("IndexCount");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region DocumentCount (int)

        /// <summary>
        /// Gets or sets the int value for DocumentCount
        /// </summary>
        /// <value> The int value.</value>

        public long DocumentCount
        {
            get { return Model.DocumentCount; }
            set
            {
                if (Model.DocumentCount != value)
                {
                    Model.DocumentCount = value;
                    OnPropertyChanged("DocumentCount");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region Indexes (ObservableCollection<IndexViewModel>)

        private ObservableCollection<IndexViewModel> _Indexes = new ObservableCollection<IndexViewModel>();

        /// <summary>
        /// Gets or sets the ObservableCollection<IndexViewModel> value for Indexes
        /// </summary>
        /// <value> The ObservableCollection<IndexViewModel> value.</value>

        public ObservableCollection<IndexViewModel> Indexes
        {
            get { return _Indexes; }
            set
            {
                if (_Indexes != value)
                {
                    _Indexes = value;
                }
            }
        }

        #endregion

        #region Json (string)

        /// <summary>
        /// Gets or sets the string value for Json
        /// </summary>
        /// <value> The string value.</value>

        public string Json
        {
            get { return (String.IsNullOrEmpty(Model.Json)) ? String.Empty : Model.Json; }
            set
            {
                if (Model.Json != value)
                {
                    Model.Json = value;
                    OnPropertyChanged("Json");
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

        #region Commands (ObservableCollection<CommandViewModel>)

        private ObservableCollection<CommandViewModel> _Commands = new ObservableCollection<CommandViewModel>();
        /// <summary>
        /// Gets or sets the ObservableCollection<CommandViewModel> value for Commands
        /// </summary>
        /// <value> The ObservableCollection<CommandViewModel> value.</value>

        public ObservableCollection<CommandViewModel> Commands
        {
            get { return _Commands; }
            set
            {
                if (_Commands != value)
                {
                    _Commands = value;
                    OnPropertyChanged("Commands");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region SelectedCommand (CommandViewModel)

        private CommandViewModel _SelectedCommand;
        /// <summary>
        /// Gets or sets the CommandViewModel value for SelectedCommand
        /// </summary>
        /// <value> The CommandViewModel value.</value>

        public CommandViewModel SelectedCommand
        {
            get { return _SelectedCommand; }
            set
            {
                if (_SelectedCommand != value)
                {
                    _SelectedCommand = value;
                    OnPropertyChanged("SelectedCommand");
                    IsDirty = true;
                }
            }
        }

        #endregion



        private ICommand _AddQueryCommand;

        public ICommand AddQueryCommand
        {
            get
            {
                if (_AddQueryCommand == null)
                {
                    _AddQueryCommand = new RelayCommand(
                        param => AddQuery(),
                        param => CanAddQuery());
                }
                return _AddQueryCommand;
            }
        }


        private ICommand _ExecuteQueryCommand;
        public ICommand ExecuteQueryCommand
        {
            get
            {
                if (_ExecuteQueryCommand == null)
                {
                    _ExecuteQueryCommand = new RelayCommand(
                        param => ExecuteQuery(),
                        param => CanExecuteQuery());
                }
                return _ExecuteQueryCommand;
            }
        }

        private dynamic _result = null;
        public dynamic Result
        {
            get { return _result; }
            set { _result = value; OnPropertyChanged("Result"); }
        }


        private void ExecuteQuery()
        {

            string s = SelectedCommand.Text;

            if (!String.IsNullOrWhiteSpace(SelectedCommand.Text))
            {
                Result = XTool.Mongo.Inquisitor.Execute(_ConnectionString, _Database, Name, "find", SelectedCommand.Text);
            }


            int i = 0;
            int j = i;

        }

        private bool CanExecuteQuery()
        {
            return SelectedCommand != null;
        }

        public CollectionViewModel(XTool.Mongo.Collection model, string connectionString, List<MongoDbCommand> mongodbCommands)
        {

            Model = model;
            commands = mongodbCommands;
            string[] t = model.FullName.Split(new char[] { '.' });
            _Database = t[0];

            _ConnectionString = connectionString;

            if (model.Indexes != null)
            {
                foreach (var item in model.Indexes)
                {
                    Indexes.Add(new IndexViewModel(item));
                }
            }


            foreach (var item in Model.Commands)
            {
                Commands.Add(new CommandViewModel(item));
            }
            OnPropertyChanged("Commands");
            Commands.CollectionChanged += Commands_CollectionChanged;
        }

        void Commands_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    var vm = item as CommandViewModel;
                    if (vm != null)
                    {
                        Model.Commands.Add(vm.Model);
                        commands.Add(vm.Model);
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems)
                {
                    var vm = item as CommandViewModel;
                    if (vm != null)
                    {
                        Model.Commands.Remove(vm.Model);
                        commands.Remove(vm.Model);
                    }
                }
            }
        }
   
        private void AddQuery()
        {

            Commands.Add(new CommandViewModel( new MongoDbCommand() 
            { 
                Collection = FullName, 
                Title = "mongo-query",
                Text = String.Format("db.{0}.find().take(50)", Name)
            }));

        }

        private bool CanAddQuery()
        {
            return true;
        }    
    
    
    }
}
