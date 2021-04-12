using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using XTool.Cyclops;

namespace XTool
{
    public class CyclopsConnectionViewModel : ViewModel<Cyclops.CyclopsConnection>
    {


        #region IsValid (bool)

        /// <summary>
        /// Gets or sets the bool value for IsValid
        /// </summary>
        /// <value> The bool value.</value>
        private bool _IsValid;
        public bool IsValid
        {
            get { return _IsValid; }
            set
            {
                if (_IsValid != value)
                {
                    _IsValid = value;
                    OnPropertyChanged("IsValid");
                    IsDirty = true;
                }
            }
        }

        #endregion


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


        #region ConnectionString (string)

        /// <summary>
        /// Gets or sets the string value for ConnectionString
        /// </summary>
        /// <value> The string value.</value>

        public string ConnectionString
        {
            get { return (String.IsNullOrEmpty(Model.ConnectionString)) ? String.Empty : Model.ConnectionString; }
            set
            {
                if (Model.ConnectionString != value)
                {
                    Model.ConnectionString = value;
                    OnPropertyChanged("ConnectionString");
                    IsDirty = true;
                }
            }
        }

        #endregion



        #region ErrorSchema (LogSchema)


        /// <summary>
        /// Gets or sets the LogSchema value for ErrorSchema
        /// </summary>
        /// <value> The LogSchema value.</value>

        public Cyclops.LogSchema ErrorSchema
        {
            get { return Model.ErrorSchema; }
            set
            {
                if (Model.ErrorSchema != value)
                {
                    
                    Model.ErrorSchema = value;                                                        
                    OnPropertyChanged("ErrorSchema");
                    OnPropertyChanged("ErrorSchemas");
                    SelectedErrorSchema = Schemas[value].Find(x => x.IsSelected);
                }
            }
        }


        #endregion

        #region ApiSchema (LogSchema)


        /// <summary>
        /// Gets or sets the LogSchema value for ApiSchema
        /// </summary>
        /// <value> The LogSchema value.</value>

        public Cyclops.LogSchema ApiSchema
        {
            get { return Model.ApiSchema; }
            set
            {
                if (Model.ApiSchema != value)
                {
                    Model.ApiSchema = value;
                    OnPropertyChanged("ApiSchema");
                    OnPropertyChanged("ApiSchemas");
                    SelectedApiSchema = Schemas[value].Find(x => x.IsSelected);

                }
            }
        }

        #endregion


        #region SessionSchema (LogSchema)


        /// <summary>
        /// Gets or sets the LogSchema value for SessionSchema
        /// </summary>
        /// <value> The LogSchema value.</value>

        public Cyclops.LogSchema SessionSchema
        {
            get { return Model.SessionSchema; }
            set
            {
                if (Model.SessionSchema != value)
                {
                    Model.SessionSchema = value;
                  
                    OnPropertyChanged("SessionSchema");
                    OnPropertyChanged("SessionSchemas");
                    SelectedSessionSchema = Schemas[value].Find(x => x.IsSelected);
                }
            }
        }

        #endregion




        public List<TypedItem> ApiSchemas {  get { return Schemas[ApiSchema];  } }

        public List<TypedItem> ErrorSchemas { get { return Schemas[ErrorSchema]; } }

        public List<TypedItem> SessionSchemas { get { return Schemas[SessionSchema]; } }


        public Dictionary<Cyclops.LogSchema, List<TypedItem>> Schemas { get; set; }



        #region SelectedErrorSchema (TypedItem)


        /// <summary>
        /// Gets or sets the TypedItem value for SelectedErrorSchema
        /// </summary>
        /// <value> The TypedItem value.</value>
        private TypedItem _SelectedErrorSchema;
        public TypedItem SelectedErrorSchema
        {
            get { return _SelectedErrorSchema; }
            set
            {
                if (_SelectedErrorSchema != value)
                {
                    _SelectedErrorSchema = value;
                    OnPropertyChanged("SelectedErrorSchema");
                }
            }
        }

        #endregion



        #region SelectedApiSchema (TypedItem)


        /// <summary>
        /// Gets or sets the TypedItem value for SelectedApiSchema
        /// </summary>
        /// <value> The TypedItem value.</value>

        private TypedItem _SelectedApiSchema;
        public TypedItem SelectedApiSchema
        {
            get { return _SelectedApiSchema; }
            set
            {
                if (_SelectedApiSchema != value)
                {
                    _SelectedApiSchema = value;
                    OnPropertyChanged("SelectedApiSchema");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region SelectedSessionSchema (TypedItem)


        /// <summary>
        /// Gets or sets the TypedItem value for SelectedSessionSchema
        /// </summary>
        /// <value> The TypedItem value.</value>
        private TypedItem _SelectedSessionSchema;
        public TypedItem SelectedSessionSchema
        {
            get { return _SelectedSessionSchema; }
            set
            {
                if (_SelectedSessionSchema != value)
                {
                    _SelectedSessionSchema = value;
                    OnPropertyChanged("SelectedSessionSchema");
                }
            }
        }

        #endregion



        #region MonthOfYear (string)

        /// <summary>
        /// Gets or sets the string value for MonthOfYear
        /// </summary>
        /// <value> The string value.</value>
        private string _MonthOfYear;
        public string MonthOfYear
        {
            get { return _MonthOfYear; }
            set
            {
                if (_MonthOfYear != value)
                {
                    _MonthOfYear = value;
                    OnPropertyChanged("MonthOfYear");
                    IsDirty = true;
                }
            }
        }

        #endregion


        #region WeekOfYear (string)

        /// <summary>
        /// Gets or sets the string value for WeekOfYear
        /// </summary>
        /// <value> The string value.</value>
        private string _WeekOfYear;
        public string WeekOfYear
        {
            get { return _WeekOfYear; }
            set
            {
                if (_WeekOfYear != value)
                {
                    _WeekOfYear = value;
                    OnPropertyChanged("WeekOfYear");
                    IsDirty = true;
                }
            }
        }

        #endregion


        #region DayOfWeek (string)

        /// <summary>
        /// Gets or sets the string value for DayOfWeek
        /// </summary>
        /// <value> The string value.</value>
        private string _DayOfWeek;
        public string DayOfWeek
        {
            get { return _DayOfWeek; }
            set
            {
                if (_DayOfWeek != value)
                {
                    _DayOfWeek = value;
                    OnPropertyChanged("DayOfWeek");
                    IsDirty = true;
                }
            }
        }

        #endregion



        private ICommand _ViewSchemaStatsCommand;
        public ICommand ViewSchemaStatsCommand
        {
            get
            {
                if (_ViewSchemaStatsCommand == null)
                {
                    _ViewSchemaStatsCommand = new RelayCommand(param => ViewSchemaStats());
                }
                return _ViewSchemaStatsCommand;
            }
        }


        public CyclopsConnectionViewModel(Cyclops.CyclopsConnection model, Dictionary<Cyclops.LogSchema, List<TypedItem>> schemas)
        {

            Model = model;
            Schemas = schemas;
            ResetSchemas(DateTime.Now);
            SelectedErrorSchema = Schemas[ErrorSchema].Find(x => x.IsSelected);
            SelectedApiSchema = Schemas[ApiSchema].Find(x => x.IsSelected);
            SelectedSessionSchema = Schemas[SessionSchema].Find(x => x.IsSelected);

        }


        private void ViewSchemaStats()
        {
            XTool.Cyclops.LogStats stats;
            if(XTool.Cyclops.LogStatsProvider.TryGetStatisitics(this.ConnectionString, out stats))
            {

                // pass data to overlay;

            }
        }

        private void ResetSchemas(DateTime now)
        {
            _MonthOfYear = now.Month.ToString("MMM");
            _WeekOfYear = now.Date.WeekOfYear().ToString("000");
            _DayOfWeek = now.DayOfWeek.ToString().Substring(0, 3);
        }

        private void SyncSelections(string v, LogSchema value)
        {
            throw new NotImplementedException();
        }



    }
}
