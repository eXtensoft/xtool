using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace XTool
{
    /// <summary>
    /// Interaction logic for LogApiRequestView.xaml
    /// </summary>
    public partial class LogApiRequestView : UserControl, INotifyPropertyChanged
    {
        public CyclopsWorkspaceViewModel Logging { get; set; }

        public List<ApiRequestViewModel> Items { get; set; }

        public XTool.Filtering.FilterProvider<ApiRequestViewModel> Provider { get; set; }



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
                }
            }
        }

        #endregion



        private DateTime _From;
        public DateTime From
        {
            get { return _From; }
            set
            {
                if (_From != value)
                {
                    _From = value;
                    OnPropertyChanged("From");
                }
            }
        }

        private DateTime _To;
        public DateTime To
        {
            get { return _To; }
            set
            {
                if (_To != value)
                {
                    _To = value;
                    OnPropertyChanged("To");
                }
            }
        }


        public LogApiRequestView()
        {
            InitializeComponent();
            InitializeDates();
            Logging = WorkspaceProvider.Instance.ViewModel.Logging;
            this.DataContext = Logging;
        }


        private ICommand _SearchCommand;
        public ICommand SearchCommand
        {
            get
            {
                if (_SearchCommand == null)
                {
                    _SearchCommand = new RelayCommand(param => Search());
                }
                return _SearchCommand;
            }
        }

        private void grdItems_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnCloseOverlay_Click(object sender, RoutedEventArgs e)
        {
            grdContent.Children.Clear();
            grdOverlay.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void InitializeDates()
        {
            _From = DateTime.Now.AddDays(-7);
            _To = DateTime.Now.AddDays(1).Date;
            OnPropertyChanged("From");
            OnPropertyChanged("To");
        }

        private void ExpandFacets()
        {
            
            //expZone.IsExpanded = true;
            //expAppContextInstance.IsExpanded = true;
            //expApplicationKey.IsExpanded = true;
            //expCategory.IsExpanded = true;
            //expSeverity.IsExpanded = true;
        }



        private void Search()
        {
            if (Logging.Selected != null && !String.IsNullOrWhiteSpace(Logging.Selected.ConnectionString))
            {
                List<Cyclops.ApiRequest> list = new List<Cyclops.ApiRequest>();
                try
                {
                    using (SqlConnection cn = new SqlConnection(Logging.Selected.ConnectionString))
                    {
                        cn.Open();
                        using (SqlCommand cmd = cn.CreateCommand())
                        {
                            cmd.CommandType = System.Data.CommandType.Text;
                            cmd.CommandText = ComposeApiSql();
                            cmd.Parameters.AddWithValue("@From", _From);
                            cmd.Parameters.AddWithValue("@To", _To);
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    var model = new Cyclops.ApiRequest();
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
                                    ////var xmldoc = reader.GetSqlXml(reader.GetOrdinal("XmlData"));
                                    //if (!xmldoc.IsNull)
                                    //{
                                    //    // model.ComplexProperty = GenericSerializer.StringToGenericItem<[typename]>(xmldoc.Value);
                                    //}
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
                                    list.Add(model);

                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    System.Windows.MessageBox.Show(message);
                }

                if (list.Count > 0)
                {
                    Items = new List<ApiRequestViewModel>(
                            (from x in list select new ApiRequestViewModel(x)).ToList()
                            );
                    InitializeResults();
                }
            }
        }

        private void InitializeResults()
        {
            CollectionViewSource cvsGroup = (CollectionViewSource)(this.Resources["cvsGroup"]);
            cvsGroup.Source = Items;
            Provider = new Filtering.FilterProvider<ApiRequestViewModel>(true, (CollectionViewSource)this.Resources["cvs"]);
            Provider.CollectionView.Source = Items;
        }

        private string ComposeApiSql()
        {
            return ComposeApiSqlTopN();
        }

        private string ComposeApiSqlTopN()
        {
            int max = (Logging.TopNCount.MaxDistinct > 0) ? Logging.TopNCount.MaxDistinct : 100;

            string schema = Logging.Selected.SelectedApiSchema.Key;
            string sql = String.Format("select top {0} [ApiRequestId], [AppKey], [AppZone], [AppInstance], [Elapsed], [Start], [Protocol], [Host]," +
            "[Path], [ClientIP], [UserAgent], [HttpMethod], [ControllerName], [ControllerMethod], [MethodReturnType], [ResponseCode], [ResponseText]," +
            "[XmlData], [MessageId], [BasicToken], [BearerToken], [AuthSchema], [AuthValue], [MessageBody], [HasLog], [Tds] from" +
            " [<schema>].[ApiRequest] WHERE ([Start] >= @From and [Start] <= @To)",max).Replace("<schema>",schema);

            string sqlWhere = sql + " ORDER BY [ApiRequestId] desc";
            return sqlWhere;

        }

        private string ComposeApiSqlFacets()
        {
            return String.Empty;
        }


        #region INotifyPropertyChanged Members


        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion


    }
}
