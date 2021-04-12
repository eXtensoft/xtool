using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace XTool
{
    public class SqlStoredProcedureViewModel : ViewModel<Discovery.SqlStoredProcedure>
    {

        private SqlServerConnectionInfoViewModel _ConnectionViewModel;

        //private int _SchemaSize = 3;

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

        #region IsChecked (bool)

        /// <summary>
        /// Gets or sets the bool value for IsChecked
        /// </summary>
        /// <value> The bool value.</value>

        public bool IsChecked
        {
            get { return Model.IsChecked; }
            set
            {
                if (Model.IsChecked != value)
                {
                    Model.IsChecked = value;
                    OnPropertyChanged("IsChecked");
                    IsDirty = true;
                }
            }
        }

        #endregion



        #region operational properties

        #region SqlText (string)
        private string _SqlText;
        /// <summary>
        /// Gets or sets the string value for SqlText
        /// </summary>
        /// <value> The string value.</value>

        public string SqlText
        {
            get { return (String.IsNullOrEmpty(_SqlText)) ? String.Empty : _SqlText; }
            set
            {
                if (_SqlText != value)
                {
                    _SqlText = value;
                    OnPropertyChanged("SqlText");
                    IsDirty = true;
                }
            }
        }

        #endregion

        public string ToDisplay
        {
            get { return Model.ToString(); }
        }

        public string ToTooltip
        {
            get
            {
                return Model.ToTooltip();
            }
        }

        #endregion

        private ICommand _AddSprocCommand;
        public ICommand AddSprocCommand
        {
            get{
                if (_AddSprocCommand == null)
                {
                    _AddSprocCommand = new RelayCommand(
                        param => AddSproc());
                }

                return _AddSprocCommand;
            }
            
        }



        #region Parameters (ObservableCollection<SqlParameterViewModel>)

        private ObservableCollection<SqlParameterViewModel> _Parameters;

        /// <summary>
        /// Gets or sets the ObservableCollection<SqlParameterViewModel> value for Parameters
        /// </summary>
        /// <value> The ObservableCollection<SqlParameterViewModel> value.</value>

        public ObservableCollection<SqlParameterViewModel> Parameters
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

        public SqlStoredProcedureViewModel(Discovery.SqlStoredProcedure model, SqlServerConnectionInfoViewModel connectionViewModel)
        {
            Model = model;
            if (model.Parameters != null)
            {
                _Parameters = new ObservableCollection<SqlParameterViewModel>((from m in Model.Parameters select new SqlParameterViewModel(m)).ToList());
            }
            _ConnectionViewModel = connectionViewModel;
        }

        private void AddSproc()
        {
            var sp = new Inference.SqlCommand()
            {
                CommandType = "StoredProcedure",
                Text = Model.ToString(),
                Title = Model.ToString()
            };
            if (Parameters != null && Parameters.Count > 0)
            {
                sp.Parameters = (from x in Model.Parameters
                                 select new Inference.Parameter()
                                 {
                                     Name = x.ParamName,
                                     Mode = x.Mode,
                                     DataType = DataMapper.Map(x.Datatype, x.MaxLength)
                                 }).ToList();

            }

            _ConnectionViewModel.Commands.Add(new SqlCommandViewModel(sp));
        }


        private ICommand _GetProcedureViewCommand;
        public ICommand GetProcedureViewCommand
        {
            get
            {
                if (_GetProcedureViewCommand == null)
                {
                    _GetProcedureViewCommand = new RelayCommand(param => GetProcedureView(), param => CanGetProcedureView());
                }
                return _GetProcedureViewCommand;
            }
        }

        private void GetProcedureView()
        {
            SqlText = _ConnectionViewModel.ExecuteStoredProcedureText(String.Format("{0}.{1}",Schema,Name));
        }


        private void ExecuteProcedureView(System.Data.SqlClient.SqlConnection cn)
        {

        }

        private bool CanGetProcedureView()
        {
            return true;// return CommandType.Equals("StoredProcedure", StringComparison.OrdinalIgnoreCase);
        }

    }
}
