using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows.Input;


namespace XTool
{
    public class SqlCommandViewModel : ViewModel<Inference.SqlCommand>
    {

        #region properties

        #region Title (string)

        /// <summary>
        /// Gets or sets the string value for Title
        /// </summary>
        /// <value> The string value.</value>

        public string Title
        {
            get { return (String.IsNullOrEmpty(Model.Title)) ? String.Empty : Model.Title; }
            set
            {
                if (Model.Title != value)
                {
                    Model.Title = value;
                    OnPropertyChanged("Title");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region Elapsed (string)

        private string _Elapsed;

        /// <summary>
        /// Gets or sets the string value for Elapsed
        /// </summary>
        /// <value> The string value.</value>

        public string Elapsed
        {
            get { return (String.IsNullOrEmpty(_Elapsed)) ? String.Empty : _Elapsed; }
            set
            {
                if (_Elapsed != value)
                {
                    _Elapsed = value;
                    OnPropertyChanged("Elapsed");
                }
            }
        }

        #endregion

        #region CommandType (string)

        /// <summary>
        /// Gets or sets the string value for CommandType
        /// </summary>
        /// <value> The string value.</value>

        public string CommandType
        {
            get { return (String.IsNullOrEmpty(Model.CommandType)) ? String.Empty : Model.CommandType; }
            set
            {
                if (Model.CommandType != value)
                {
                    Model.CommandType = value;
                    OnPropertyChanged("CommandType");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region Text (string)

        /// <summary>
        /// Gets or sets the string value for Text
        /// </summary>
        /// <value> The string value.</value>

        public string Text
        {
            get { return (String.IsNullOrEmpty(Model.Text)) ? String.Empty : Model.Text; }
            set
            {
                if (Model.Text != value)
                {
                    Model.Text = value;
                    OnPropertyChanged("Text");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region Parameters (ObservableCollection<ParameterViewModel>)

        private ObservableCollection<ParameterViewModel> _Parameters;

        /// <summary>
        /// Gets or sets the ObservableCollection<ParameterViewModel> value for Parameters
        /// </summary>
        /// <value> The ObservableCollection<ParameterViewModel> value.</value>

        public ObservableCollection<ParameterViewModel> Parameters
        {
            get { return _Parameters; }
            set
            {
                if (_Parameters != value)
                {
                    _Parameters = value;
                }
            }
        }

        #endregion


        public string Display
        {
            get
            {
                return Model.ToString();
            }
        }

        #endregion

        #region constructors

        public SqlCommandViewModel(Inference.SqlCommand model)
        {
            Model = model;
            Parameters = new ObservableCollection<ParameterViewModel>();
            if (model.Parameters != null)
            {

                foreach (var item in model.Parameters)
                {
                    Parameters.Add(new ParameterViewModel(item));
                }
            }
            Parameters.CollectionChanged += new NotifyCollectionChangedEventHandler(Parameters_CollectionChanged);
        }

        void Parameters_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (Model.Parameters == null)
                {
                    Model.Parameters = new List<Inference.Parameter>();
                }
                foreach (ParameterViewModel item in e.NewItems)
                {
                    Model.Parameters.Add(item.Model);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (ParameterViewModel item in e.OldItems)
                {
                    Model.Parameters.Remove(item.Model);
                }
            }
        }

        #endregion


        
        internal MySql.Data.MySqlClient.MySqlCommand CreateMySqlCommand(MySql.Data.MySqlClient.MySqlConnection cn)
        {
            MySql.Data.MySqlClient.MySqlCommand cmd = null;
            if (CommandType.Equals("StoredProcedure", StringComparison.OrdinalIgnoreCase))
            {

            }
            else
            {
                string commandText = Text.Replace('\t', ' ');
                cmd = new MySql.Data.MySqlClient.MySqlCommand(commandText, cn);
            }

            return cmd;
        }

        internal System.Data.SqlClient.SqlCommand CreateCommand(System.Data.SqlClient.SqlConnection cn)
        {
            System.Data.SqlClient.SqlCommand cmd = null;
            if (CommandType.Equals("StoredProcedure", StringComparison.OrdinalIgnoreCase))
            {
                cmd = new System.Data.SqlClient.SqlCommand(Text, cn);
                if (Parameters != null && Parameters.Count > 0)
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    System.Data.SqlClient.SqlCommandBuilder.DeriveParameters(cmd);
                    cmd.Parameters.RemoveAt(0);

                    foreach (System.Data.SqlClient.SqlParameter item in cmd.Parameters)
                    {
                        var found = Model.Parameters.Find(x => x.Name.Equals(item.ParameterName));
                        if (found != null)
                        {
                            if (found.DataType.Equals(DbType.Guid))
                            {
                                item.Value = new Guid(found.Value.ToString());
                            }
                            else
                            {
                                item.Value = found.Value;
                            }
                            
                        }
                    }
                }
                return cmd;
            }
            else if (CommandType.Equals("Text", StringComparison.OrdinalIgnoreCase))
            {
                cmd = new System.Data.SqlClient.SqlCommand(Text, cn);
            }
            return cmd;
        }
    }
}
