using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using XTool.Cyclops;

namespace XTool
{
    public class CyclopsWorkspaceViewModel : ViewModel<Cyclops.Workspace>
    {
        //DateTime now = DateTime.Now;
        private string defaultMonthOfYear;// = now.ToString("MMM").ToLower();
        private string defaultWeekOfYear;// = now.Date.WeekOfYear().ToString("000");
        private string defaultDayOfWeek;// = now.DayOfWeek.ToString().Substring(0, 3).ToLower();

        public string Moniker { get { return "cyclops"; } }
        public ObservableCollection<SelectionViewModel> Items { get; set; }

        public Dictionary<Cyclops.LogSchema,List<TypedItem>> SchemaSelections { get; set; }

        #region Selected (CyclopsConnectionViewModel)


        /// <summary>
        /// Gets or sets the CyclopsConnectionViewModel value for Selected
        /// </summary>
        /// <value> The CyclopsConnectionViewModel value.</value>
        private CyclopsConnectionViewModel _Selected;
        public CyclopsConnectionViewModel Selected
        {
            get { return _Selected; }
            set
            {
                if (_Selected != value)
                {
                    _Selected = value;
                    if (!_Selected.IsValid)
                    {
                        _Selected.IsValid = Validate(_Selected.ConnectionString);
                    }
                    OnPropertyChanged("Selected");
                    OnPropertyChanged("IsConnectionSelected");
                    IsDirty = true;
                }
            }
        }


        #endregion


        #region IsConnectionSelected (bool)

        /// <summary>
        /// Gets or sets the bool value for IsConnectionSelected
        /// </summary>
        /// <value> The bool value.</value>

        public bool IsConnectionSelected
        {
            get { return _Selected != null; }

        }

        #endregion


        private ICommand _DiscoverSchemasCommand;
        public ICommand DiscoverSchemasCommand
        {
            get
            {
                if (_DiscoverSchemasCommand == null)
                {
                    _DiscoverSchemasCommand = new RelayCommand(
                        param => DiscoverSchemas(),
                        param => CanDiscoverSchemas()
                        );
                }
                return _DiscoverSchemasCommand;
            }
        }


        private bool CanDiscoverSchemas()
        {
            return Selected != null;
        }

        private void DiscoverSchemas()
        {

            // check if schemas exist, then check current( max-record-count/most-recent)
            try
            {

            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                MessageBox.Show(message);
            }
        }



        private ICommand _AddConnectionCommand;
        public ICommand AddConnectionCommand
        {
            get
            {
                if (_AddConnectionCommand == null)
                {
                    _AddConnectionCommand = new RelayCommand(param => AddConnection());
                }
                return _AddConnectionCommand;
            }
        }

        private void AddConnection()
        {
            Cyclops.CyclopsConnection model = new CyclopsConnection()
            {
                Title = "logSource",
                ConnectionString = "Data Source=[serverName];Initial Catalog=dplog;User ID=[userName];Password=[password",
                ErrorSchema = LogSchema.Log,
                ApiSchema = LogSchema.Log,
                SessionSchema = LogSchema.Log,
            };

            Connections.Add(new CyclopsConnectionViewModel(model,SchemaSelections) );

        }



        #region IsFacetsChecked (bool)

        /// <summary>
        /// Gets or sets the bool value for IsFacetsChecked
        /// </summary>
        /// <value> The bool value.</value>
        private bool _IsFacetsChecked;
        public bool IsFacetsChecked
        {
            get { return _IsFacetsChecked; }
            set
            {
                if (_IsFacetsChecked != value)
                {
                    _IsFacetsChecked = value;
                    OnPropertyChanged("IsFacetsChecked");
                }
            }
        }

        #endregion

        #region TopNCount (Display)


        /// <summary>
        /// Gets or sets the Display value for TopNCount
        /// </summary>
        /// <value> The Display value.</value>
        private Display _TopNCount;
        public Display TopNCount
        {
            get { return _TopNCount; }
            set
            {
                if (_TopNCount != value)
                {
                    _TopNCount = value;
                    OnPropertyChanged("TopNCount");
                    IsDirty = true;
                }
            }
        }

        #endregion

        private bool Validate(string connectionString)
        {
            bool b = false;
            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    cn.Open();
                    if (cn.State == ConnectionState.Open)
                    {
                        b = true;
                    }
                }
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                MessageBox.Show(message);
            }
            return b;
        }

        public Cyclops.ApiSession GetSession(string cnString, long id, string schema)
        {
            Cyclops.ApiSession model = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(cnString))
                {
                    cn.Open();
                    using (SqlCommand cmd = cn.CreateCommand())
                    {
                        const string IdParamName = "@apirequestid";
                        cmd.CommandType = CommandType.Text;

                        string sql = "select [Id], [CreatedAt], [BasicToken], [BearerToken], [TenantId], [PatronId]," +
                            "[SsoPatronId], [GatewayPatronId], [IPAddress], [UserAgent], [PassKey], [LinesOfBusiness]," +
                            "[Tds] from [<schema>].[Session] where [Id] = ".Replace("<schema>",schema) + IdParamName;

                        cmd.CommandText = sql;

                        cmd.Parameters.AddWithValue(IdParamName, id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if(reader.Read())
                            {
                                model = new ApiSession();
                                model.Id = reader.GetInt64(reader.GetOrdinal("Id"));
                                model.CreatedAt = reader.GetDateTimeOffset(reader.GetOrdinal("CreatedAt"));
                                model.BasicToken = reader.GetGuid(reader.GetOrdinal("BasicToken"));
                                model.BearerToken = reader.GetString(reader.GetOrdinal("BearerToken"));
                                model.TenantId = reader.GetInt32(reader.GetOrdinal("TenantId"));
                                model.PatronId = reader.GetInt32(reader.GetOrdinal("PatronId"));
                                model.SsoPatronId = reader.GetInt32(reader.GetOrdinal("SsoPatronId"));
                                model.GatewayPatronId = reader.GetInt32(reader.GetOrdinal("GatewayPatronId"));
                                if (!reader.IsDBNull(reader.GetOrdinal("IPAddress")))
                                {
                                    model.IPAddress = reader.GetString(reader.GetOrdinal("IPAddress"));
                                }
                                if (!reader.IsDBNull(reader.GetOrdinal("UserAgent")))
                                {
                                    model.UserAgent = reader.GetString(reader.GetOrdinal("UserAgent"));
                                }
                                if (!reader.IsDBNull(reader.GetOrdinal("PassKey")))
                                {
                                    model.PassKey = reader.GetString(reader.GetOrdinal("PassKey"));
                                }
                                if (!reader.IsDBNull(reader.GetOrdinal("LinesOfBusiness")))
                                {
                                    model.LinesOfBusiness = reader.GetString(reader.GetOrdinal("LinesOfBusiness"));
                                }
                                model.Tds = reader.GetDateTimeOffset(reader.GetOrdinal("Tds"));
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                var message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                MessageBox.Show(message);
            }


            return model;
        }
        public Cyclops.ApiRequest GetApiRequest(string cnString,long id,string schema)
        {
            Cyclops.ApiRequest model = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(cnString))
                {
                    cn.Open();
                    using (SqlCommand cmd = cn.CreateCommand())
                    {
                        const string ApiRequestIdParamName = "@apirequestid";
                        cmd.CommandType = CommandType.Text;

                        string sql = "select [ApiRequestId], [AppKey], [AppZone], [AppInstance], [Elapsed], [Start], [Protocol], [Host], " +
                            "[Path], [ClientIP], [UserAgent], [HttpMethod], [ControllerName], [ControllerMethod], [MethodReturnType]," +
                            "[ResponseCode], [ResponseText], [XmlData], [MessageId], [BasicToken], [BearerToken], [AuthSchema]," +
                            "[AuthValue], [MessageBody], [HasLog], [Tds] from [<schema>].[ApiRequest] where [ApiRequestId] = ".Replace("<schema>",schema) + ApiRequestIdParamName;

                        cmd.CommandText = sql;

                        cmd.Parameters.AddWithValue(ApiRequestIdParamName, id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if(reader.Read())
                            {
                                model = new Cyclops.ApiRequest();
                                model.ApiRequestId = reader.GetInt64(reader.GetOrdinal("ApiRequestId"));
                                model.AppKey = reader.GetString(reader.GetOrdinal("AppKey"));
                                model.AppZone = reader.GetString(reader.GetOrdinal("AppZone"));
                                model.AppInstance = reader.GetString(reader.GetOrdinal("AppInstance"));
                                model.Elapsed = reader.GetDecimal(reader.GetOrdinal("Elapsed"));
                                model.Start = reader.GetDateTime(reader.GetOrdinal("Start"));
                                model.Protocol = reader.GetString(reader.GetOrdinal("Protocol"));
                                model.Host = reader.GetString(reader.GetOrdinal("Host"));
                                model.Path = reader.GetString(reader.GetOrdinal("Path"));
                                model.ClientIP = reader.GetString(reader.GetOrdinal("ClientIP"));
                                model.UserAgent = reader.GetString(reader.GetOrdinal("UserAgent"));
                                model.HttpMethod = reader.GetString(reader.GetOrdinal("HttpMethod"));
                                model.ControllerName = reader.GetString(reader.GetOrdinal("ControllerName"));
                                model.ControllerMethod = reader.GetString(reader.GetOrdinal("ControllerMethod"));
                                model.MethodReturnType = reader.GetString(reader.GetOrdinal("MethodReturnType"));
                                model.ResponseCode = reader.GetString(reader.GetOrdinal("ResponseCode"));
                                model.ResponseText = reader.GetString(reader.GetOrdinal("ResponseText"));
                                var xmldoc = reader.GetSqlXml(reader.GetOrdinal("XmlData"));
                                
                                if (!xmldoc.IsNull)
                                {
                                    // model.ComplexProperty = GenericSerializer.StringToGenericItem<[typename]>(xmldoc.Value);
                                }
                                model.MessageId = reader.GetGuid(reader.GetOrdinal("MessageId"));
                                model.BasicToken = reader.GetString(reader.GetOrdinal("BasicToken"));
                                model.BearerToken = reader.GetString(reader.GetOrdinal("BearerToken"));
                                model.AuthSchema = reader.GetString(reader.GetOrdinal("AuthSchema"));
                                model.AuthValue = reader.GetString(reader.GetOrdinal("AuthValue"));
                                if (!reader.IsDBNull(reader.GetOrdinal("MessageBody")))
                                {
                                    model.MessageBody = reader.GetString(reader.GetOrdinal("MessageBody"));
                                }
                                model.HasLog = reader.GetBoolean(reader.GetOrdinal("HasLog"));
                                model.Tds = reader.GetDateTimeOffset(reader.GetOrdinal("Tds"));

                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                var message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                MessageBox.Show(message);
            }


            return model;
        }

        public ObservableCollection<CyclopsConnectionViewModel> Connections { get; set; }
        public CyclopsWorkspaceViewModel(Cyclops.Workspace model)
        {
            Model = model;

            DateTime now = DateTime.Now;
            defaultMonthOfYear = now.ToString("MMM").ToLower();
            defaultWeekOfYear = now.Date.WeekOfYear().ToString("000");
            defaultDayOfWeek = now.DayOfWeek.ToString().Substring(0, 3).ToLower();

            GenerateSchemaSelections();

            List<Cyclops.Selection> list = new List<Cyclops.Selection>();
            list.Add(new Cyclops.Selection() { Title = "Error", Tag = "api.error", Order = 1 });
            list.Add(new Cyclops.Selection() { Title = "Api", Tag = "api.request", Order = 2 });
            list.Add(new Cyclops.Selection() { Title = "Session", Tag = "api.session", Order = 3 });
            Items = new ObservableCollection<SelectionViewModel>(from x in list select new SelectionViewModel(x));

            if (model.Connections != null)
            {
                Connections = new ObservableCollection<CyclopsConnectionViewModel>(from x in model.Connections select new CyclopsConnectionViewModel(x, SchemaSelections ));
            }

            Connections.CollectionChanged += Connections_CollectionChanged;
        }

        private void Connections_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    var vm = item as CyclopsConnectionViewModel;
                    if (vm != null)
                    {
                        Model.Connections.Add(vm.Model);
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems)
                {
                    var vm = item as CyclopsConnectionViewModel;
                    if (vm != null)
                    {
                        Model.Connections.Remove(vm.Model);
                    }
                }
            }
        }

        private void GenerateSchemaSelections()
        {

            SchemaSelections = new Dictionary<LogSchema, List<TypedItem>>();

            SchemaSelections.Add(LogSchema.Log, new List<TypedItem>() { new TypedItem() { Key = "log", Domain = "[log]", IsSelected=true } });
            
            DateTime begin = new DateTime(2017, 6, 25);
            List<TypedItem> daysOfWeek = new List<TypedItem>();
            for (int i = 0; i < 7; i++)
            {
                DateTime target = begin.AddDays(i);
                TypedItem item = new TypedItem() {
                    Key = target.DayOfWeek.ToString().Substring(0,3).ToLower(),
                    Domain = target.DayOfWeek.ToString()
                };
                if (item.Key.Equals(defaultDayOfWeek))
                {
                    item.IsSelected = true;
                }
                daysOfWeek.Add(item);
            }
            SchemaSelections.Add(LogSchema.DayOfWeek, daysOfWeek);

            List<TypedItem> weeksOfYear = new List<TypedItem>();
            
            for (int i = 1; i <= 54; i++)
            {
                string schema = i.ToString("000");
                TypedItem item = new TypedItem()
                {
                    Key = schema,
                    Domain =String.Format("[{0}] {1}", schema,DateTime.Now.GetWeekOfYear(i).ToString()),
                };
                if (item.Key.Equals(defaultWeekOfYear))
                {
                    item.IsSelected = true;
                }
                weeksOfYear.Add(item);
            }
            SchemaSelections.Add(LogSchema.WeekOfYear, weeksOfYear);

            List<TypedItem> monthsOfYear = new List<TypedItem>();        
            DateTime months = new DateTime(2017, 1, 15);
            for (int i = 0; i < 12; i++)
            {
                string schema = months.AddMonths(i).ToString("MMM");
                TypedItem item =new TypedItem()
                {
                    Key = schema.ToLower(),
                    Domain = String.Format("[{0}] {1}",schema,months.AddMonths(i).ToString("M")),
                };
                if (item.Key.Equals(defaultMonthOfYear))
                {
                    item.IsSelected = true;
                }
                monthsOfYear.Add(item);
            }
            SchemaSelections.Add(LogSchema.MonthOfYear, monthsOfYear);



        }

    }
}
