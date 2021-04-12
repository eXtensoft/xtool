using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for LogSessionView.xaml
    /// </summary>
    public partial class LogSessionView : UserControl, INotifyPropertyChanged
    {

        List<Cyclops.TypedItem> _Facets = null;

        public CyclopsWorkspaceViewModel Logging { get; set; }

        public ObservableCollection<FacetViewModel> BasicTokens { get; set; }

        public ObservableCollection<FacetViewModel> Tenants { get; set; }

        public ObservableCollection<FacetViewModel> DaysOfWeek { get; set; }

        public ObservableCollection<FacetViewModel> HoursOfDay { get; set; }

        public List<ApiSessionViewModel> Items { get; set; }

        public XTool.Filtering.FilterProvider<ApiSessionViewModel> Provider { get; set; }

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

        private void ClearFacets()
        {
            _Facets.Clear();
            if (BasicTokens != null)
            {
                BasicTokens.Clear();
            }
            if (Tenants != null)
            {
                Tenants.Clear();
            }
            if (DaysOfWeek != null)
            {
                DaysOfWeek.Clear();
            }
            if (HoursOfDay != null)
            {
                HoursOfDay.Clear();
            }
        }

        private void LoadFacets()
        {
            BasicTokens = new ObservableCollection<FacetViewModel>(
               (from x in _Facets
                where x.Domain.Equals("BasicToken")
                orderby x.Count descending
                select new FacetViewModel(x)).ToList()
               );
            OnPropertyChanged("BasicTokens");

            Tenants = new ObservableCollection<FacetViewModel>(
               (from x in _Facets
                where x.Domain.Equals("Tenant")
                orderby x.Count descending
                select new FacetViewModel(x)).ToList()
               );
            OnPropertyChanged("Tenants");

            DaysOfWeek = new ObservableCollection<FacetViewModel>(
               (from x in _Facets
                where x.Domain.Equals("DayOfWeek")
                orderby x.Count descending
                select new FacetViewModel(x)).ToList()
               );
            OnPropertyChanged("DaysOfWeek");

            HoursOfDay = new ObservableCollection<FacetViewModel>(
                (from x in _Facets
                 where x.Domain.Equals("HourOfDay")
                 orderby x.Count descending
                 select new FacetViewModel(x)).ToList()
                );
            OnPropertyChanged("HoursOfDay");


            ExpandFacets();
        }

        private void InitializeResults()
        {
            CollectionViewSource cvsGroup = (CollectionViewSource)(this.Resources["cvsGroup"]);
            cvsGroup.Source = Items;
            Provider = new Filtering.FilterProvider<ApiSessionViewModel>(true, (CollectionViewSource)this.Resources["cvs"]);
            Provider.CollectionView.Source = Items;
            Provider.AddDistinctValueFilterGroup("BasicToken", Items);
            Provider.AddDistinctValueFilterGroup("Tenant", Items);
            Provider.AddDistinctValueFilterGroup("DayOfWeek", Items);
            Provider.AddDistinctValueFilterGroup("HourOfDay", Items);
            lsbBasicToken.DataContext = Provider["BasicToken"];
            lsbTenant.DataContext = Provider["Tenant"];
            lsbDayOfWeek.DataContext = Provider["DayOfWeek"];
            lsbHourOfDay.DataContext = Provider["HourOfDay"];

            Provider.CollectionView.Filter += new FilterEventHandler(Provider.ExecuteFilter);
        }

        /// <summary>
        /// Filter selection changed event handler. This will be fired for each selection on Intervension type and agent role/name.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FilterItem_Selected(object sender, RoutedEventArgs e)
        {
            Provider.CollectionView.View.Refresh();
        }

        /// <summary>
        /// Filter selection changed event handler. This will be fired for each unselect on Intervension type and agent role/name.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FilterItem_Unselected(object sender, RoutedEventArgs e)
        {
            Provider.CollectionView.View.Refresh();
        }


        private void ExpandFacets()
        {
            expBasicToken.IsExpanded = true;
            expTenant.IsExpanded = true;
            expDayOfWeek.IsExpanded = true;
            expHourOfDay.IsExpanded = true;
        }




        private ICommand _RefreshFacetsCommand;
        public ICommand RefreshFacetsCommand
        {
            get
            {
                if (_RefreshFacetsCommand == null)
                {
                    _RefreshFacetsCommand = new RelayCommand(
                        param => RefreshFacets(),
                        param => CanRefreshFacets());
                }
                return _RefreshFacetsCommand;
            }
        }


        private bool CanRefreshFacets()
        {
            return true;
        }

        private void RefreshFacets()
        {

            if (Logging.Selected != null && !String.IsNullOrWhiteSpace(Logging.Selected.ConnectionString))
            {
                ClearFacets();
                DateTime from = _From;
                DateTime to = _To;
                List<Cyclops.TypedItem> list = new List<Cyclops.TypedItem>();
                try
                {
                    using (SqlConnection cn = new SqlConnection(Logging.Selected.ConnectionString))
                    {
                        cn.Open();
                        using (SqlCommand cmd = cn.CreateCommand())
                        {
                            cmd.CommandType = System.Data.CommandType.Text;
                            cmd.CommandText = ComposeFacetSql();

                            cmd.Parameters.AddWithValue("@From", from);
                            cmd.Parameters.AddWithValue("@To", to);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    Cyclops.TypedItem item = new Cyclops.TypedItem();
                                    item.Domain = reader.GetString(reader.GetOrdinal("FacetKey"));
                                    item.Key = reader.GetString(reader.GetOrdinal("FacetValue"));
                                    item.Value = reader.GetInt32(reader.GetOrdinal("InstanceCount"));
                                    list.Add(item);
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
                    _Facets = list;
                }
                //_Facets = _Repository.Dynamics.LogFacetsReadList(From, To);
                LoadFacets();

            }

        }

        private string ComposeFacetSql()
        {
            string schema = Logging.Selected.SelectedSessionSchema.Key;
            string sql = XTool.Resources.cyclops_facets_session.Replace("<schema>", schema);
            return sql;
        }


        private ICommand _SearchCommand;
        public ICommand SearchCommand
        {
            get
            {
                if (_SearchCommand == null)
                {
                    _SearchCommand = new RelayCommand(
                        param => Search(),
                        param => CanSearch());
                }
                return _SearchCommand;
            }
        }

        private bool CanSearch()
        {
            return true;
        }

        private void Search()
        {
            int connectionTimeout = 0;
            int commandTimeout = 0;
            if (Logging.Selected != null && !String.IsNullOrWhiteSpace(Logging.Selected.ConnectionString))
            {
                List<Cyclops.ApiSession> list = new List<Cyclops.ApiSession>();
                try
                {
                    using (SqlConnection cn = new SqlConnection(Logging.Selected.ConnectionString))
                    {
                        connectionTimeout = cn.ConnectionTimeout;
                        cn.Open();
                        using (SqlCommand cmd = cn.CreateCommand())
                        {
                            commandTimeout = cmd.CommandTimeout;
                            cmd.CommandType = System.Data.CommandType.Text;
                            cmd.CommandText = ComposeSessionSql();
                            cmd.Parameters.AddWithValue("@From", _From);
                            cmd.Parameters.AddWithValue("@To", _To);
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    Cyclops.ApiSession item = new Cyclops.ApiSession();


                                    list.Add(item);

                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(String.Format("connectionTimeout: {0}\tcommandTimeout: {1}", connectionTimeout, commandTimeout));
                    string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    sb.AppendLine(message);
                    System.Windows.MessageBox.Show(sb.ToString());
                }

                if (list.Count > 0)
                {
                    Items = new List<ApiSessionViewModel>(
                            (from x in list select new ApiSessionViewModel(x)).ToList()
                            );
                    InitializeResults();
                }
            }

        }





        public LogSessionView()
        {
            InitializeComponent();
            Logging = WorkspaceProvider.Instance.ViewModel.Logging;
            this.DataContext = Logging;

            _Facets = new List<Cyclops.TypedItem>();
            InitializeDates();
            InitializeMaxDistinct();
        }

        private void InitializeMaxDistinct()
        {
            List<Display> list = new List<Display>()
            {
                 new Display("Twenty-Five",25)
                ,new Display("Fifty",50)
                ,new Display("One-Hundred",100)
                ,new Display("Two-Hundred-Fifty",250)
                ,new Display("Max",100000)
            };
            cboMaxDistinct.ItemsSource = list;
            cboMaxDistinct.SelectedIndex = 4;
        }

        private void InitializeDates()
        {
            _From = DateTime.Now.AddDays(-7);
            _To = DateTime.Now.AddDays(1).Date;
            OnPropertyChanged("From");
            OnPropertyChanged("To");
        }



        private string ComposeSessionSql()
        {
            return Logging.IsFacetsChecked ? ComposeSessionFacets() : ComposeSessionSqlTopN();
        }

        private string ComposeSessionSqlTopN()
        {
            int max = (Logging.TopNCount.MaxDistinct > 0) ? Logging.TopNCount.MaxDistinct : 100;

            string schema = Logging.Selected.SelectedApiSchema.Key;
            string sessionSchema = Logging.Selected.SelectedSessionSchema.Key;
            string errorSchema = Logging.Selected.SelectedErrorSchema.Key;
            string sql = String.Format("select top {0} ", max) + XTool.Resources.cyclops_search_session.Replace("<schema>", schema);
            string sqlWhere = sql + " ORDER BY [Id] desc";
            return sqlWhere;
        }

        private string ComposeSessionFacets()
        {
            string schema = Logging.Selected.SelectedApiSchema.Key;
            string sessionSchema = Logging.Selected.SelectedSessionSchema.Key;
            string errorSchema = Logging.Selected.SelectedErrorSchema.Key;
            string sql = XTool.Resources.cyclops_search_session.Replace("<schema>", schema);
            int i = _Facets.Count(x => x.IsSelected);
            //MessageBox.Show(i.ToString());
            if (i > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(" AND (");
                var groups = _Facets.GroupBy(x => x.Domain).Select(g => g.Where(m => m.IsSelected == true).ToList()).Where(g => g.Count > 0).ToList();
                int j = groups.Count;

                int groupCount = 0;
                foreach (var g in groups)
                {

                    if (groupCount++ > 0)
                    {
                        sb.Append(" AND");
                    }
                    sb.Append(" (");

                    int whereCount = 0;

                    foreach (Cyclops.TypedItem item in g)
                    {
                        if (whereCount++ > 0)
                        {
                            sb.Append(" OR");

                        }
                        sb.Append(String.Format(" e.[{0}] = '{1}'", item.Domain, item.Key));
                    }



                    sb.Append(")");
                    int k = g.Count;
                }
                sb.Append(")");
                sql += sb.ToString();
                // 
            }
            string sqlWhere = sql + " ORDER BY e.[Id] desc".Replace("<schema>", schema);
            return sqlWhere;
        }


        private void grdItems_Click(object sender, RoutedEventArgs e)
        {
            Control ctl = e.OriginalSource as Control;
            if (ctl != null)
            {
                ApiErrorViewModel target = ctl.Tag as ApiErrorViewModel;
                //if (target != null)
                //{
                //    Guid messageId = target.MessageId;
                //    var id = target.Id;
                //    if (String.IsNullOrWhiteSpace(target.Model.XmlData))
                //    {
                //        string xml = GetErrorXml(id);
                //        if (!String.IsNullOrEmpty(xml))
                //        {
                //            target.XmlData = xml;

                //        }
                //    }
                //    if (target.HasApiRequest)
                //    {
                //        target.Request = Logging.GetApiRequest(Logging.Selected.ConnectionString, id, Logging.Selected.SelectedApiSchema.Key);
                //    }
                //    if (target.HasSession)
                //    {
                //        target.Session = Logging.GetSession(Logging.Selected.ConnectionString, id, Logging.Selected.SelectedSessionSchema.Key);
                //    }
                //    //Cyclops.ApiRequest apirequest = Logging.GetApiRequest(Logging.Selected.ConnectionString,id);
                //    //Cyclops.ApiRequest apirequest = Logging.GetApiRequest(Logging.Selected.ConnectionString, 5);
                //    //if (apirequest != null)
                //    //{
                //    //    target.Request = apirequest;
                //    //}

                //    UserControl overlaycontrol = null;
                //    //if (target.Title.Equals("Metrix"))
                //    //{
                //    //    overlaycontrol = new LogDetailView();
                //    //}
                //    //else
                //    //{
                //    overlaycontrol = new LogErrorDetailView() { };

                //    //}
                //    overlaycontrol.DataContext = target;
                //    grdContent.Children.Add(overlaycontrol);
                //    grdOverlay.Visibility = System.Windows.Visibility.Visible;

                //}
                //else
                //{
                //    //Metrix metrix = ctl.Tag as Metrix;
                //    //if (metrix != null)
                //    //{
                //    //    UserControl overlaycontrol = new LogDetailView();
                //    //    overlaycontrol.DataContext = metrix;
                //    //    grdContent.Children.Add(overlaycontrol);
                //    //    grdOverlay.Visibility = System.Windows.Visibility.Visible;
                //    //}
                //}
            }
        }



        private void btnCloseOverlay_Click(object sender, RoutedEventArgs e)
        {
            grdContent.Children.Clear();
            grdOverlay.Visibility = System.Windows.Visibility.Collapsed;
        }

        //private void grdItems_Click(object sender, RoutedEventArgs e)
        //{
        //    Control ctl = e.OriginalSource as Control;
        //    if (ctl != null)
        //    {
        //        ApiErrorViewModel target = ctl.Tag as ApiErrorViewModel;
        //        if (target != null)
        //        {
        //            Guid messageId = target.MessageId;
        //            var id = target.Id;
        //            if (String.IsNullOrWhiteSpace(target.Model.XmlData))
        //            {
        //                string xml = GetErrorXml(id);
        //                if (!String.IsNullOrEmpty(xml))
        //                {
        //                    target.XmlData = xml;

        //                }
        //            }
        //            if (target.HasApiRequest)
        //            {
        //                target.Request = Logging.GetApiRequest(Logging.Selected.ConnectionString, id, Logging.Selected.SelectedApiSchema.Key);
        //            }
        //            if (target.HasSession)
        //            {
        //                target.Session = Logging.GetSession(Logging.Selected.ConnectionString, id, Logging.Selected.SelectedSessionSchema.Key);
        //            }
        //            //Cyclops.ApiRequest apirequest = Logging.GetApiRequest(Logging.Selected.ConnectionString,id);
        //            //Cyclops.ApiRequest apirequest = Logging.GetApiRequest(Logging.Selected.ConnectionString, 5);
        //            //if (apirequest != null)
        //            //{
        //            //    target.Request = apirequest;
        //            //}

        //            UserControl overlaycontrol = null;
        //            //if (target.Title.Equals("Metrix"))
        //            //{
        //            //    overlaycontrol = new LogDetailView();
        //            //}
        //            //else
        //            //{
        //            overlaycontrol = new LogErrorDetailView() { };

        //            //}
        //            overlaycontrol.DataContext = target;
        //            grdContent.Children.Add(overlaycontrol);
        //            grdOverlay.Visibility = System.Windows.Visibility.Visible;

        //        }
        //        else
        //        {
        //            //Metrix metrix = ctl.Tag as Metrix;
        //            //if (metrix != null)
        //            //{
        //            //    UserControl overlaycontrol = new LogDetailView();
        //            //    overlaycontrol.DataContext = metrix;
        //            //    grdContent.Children.Add(overlaycontrol);
        //            //    grdOverlay.Visibility = System.Windows.Visibility.Visible;
        //            //}
        //        }
        //    }
        //}










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
