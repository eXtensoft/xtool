using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.ObjectModel;

namespace XTool
{
    public class DataTableViewModel : ViewModel<DataTable>
    {
        private DataSetViewModel _Master = null;

        private DataRow _Header = null;

        private Mapping _Maps = new Mapping();

        private PadOption _Pad = PadOption.Right;

        #region IsPadLeft (bool)

        private bool _IsPadLeft;

        /// <summary>
        /// Gets or sets the bool value for IsPadLeft
        /// </summary>
        /// <value> The bool value.</value>

        public bool IsPadLeft
        {
            get { return (_Pad == PadOption.Left) ? true : false; }
            set
            {
                _Pad = (value) ? PadOption.Left : PadOption.Right;
                OnPropertyChanged("IsPadRight");
                OnPropertyChanged("IsPadLeft");
            }
        }

        #endregion

        #region IsPadRight (bool)

        /// <summary>
        /// Gets or sets the bool value for IsPadRight
        /// </summary>
        /// <value> The bool value.</value>

        public bool IsPadRight
        {
            get { return (_Pad == PadOption.Right) ? true : false; }
            set
            {
                _Pad = (value) ? PadOption.Right : PadOption.Left;
                OnPropertyChanged("IsPadRight");
                OnPropertyChanged("IsPadLeft");
            }
        }

        #endregion

        #region Padding (int)

        private int _Padding;

        /// <summary>
        /// Gets or sets the int value for Padding
        /// </summary>
        /// <value> The int value.</value>

        public int Padding
        {
            get { return _Padding; }
            set
            {
                if (_Padding != value)
                {
                    _Padding = value;
                    OnPropertyChanged("Padding");
                }
            }
        }

        #endregion

        #region MappedFields (ObservableCollection<MapViewModel>)

        private SortableObservableCollection<ColumnMap, ColumnMapViewModel> _MappedFields;

        /// <summary>
        /// Gets or sets the ObservableCollection<MapViewModel> value for MappedFields
        /// </summary>
        /// <value> The ObservableCollection<MapViewModel> value.</value>

        public SortableObservableCollection<ColumnMap, ColumnMapViewModel> MappedFields
        {
            get
            {
                //if (_MappedFields == null || _MappedFields.Count == 0)
                //{
                //    GenerateMappedFields();
                //}
                return _MappedFields;
            }
            set
            {
                if (_MappedFields != value)
                {
                    _MappedFields = value;
                    OnPropertyChanged("MappedFields");
                }
            }
        }

        #endregion

        #region Columns (ObservableCollection<DataColumnViewModel>)

        private ObservableCollection<DataColumnViewModel> _Columns = new ObservableCollection<DataColumnViewModel>();

        /// <summary>
        /// Gets or sets the ObservableCollection<DataColumnViewModel> value for Columns
        /// </summary>
        /// <value> The ObservableCollection<DataColumnViewModel> value.</value>

        public ObservableCollection<DataColumnViewModel> Columns
        {
            get { return _Columns; }
            set
            {
                if (_Columns != value)
                {
                    _Columns = value;
                }
            }
        }

        #endregion

        #region Label (string)

        private string _Label;

        /// <summary>
        /// Gets or sets the string value for Label
        /// </summary>
        /// <value> The string value.</value>

        public string Label
        {
            get { return (String.IsNullOrEmpty(_Label)) ? String.Empty : _Label; }
        }

        #endregion

        #region Name (string)

        /// <summary>
        /// Gets or sets the string value for TableName
        /// </summary>
        /// <value> The string value.</value>

        public string Name
        {
            get { return (String.IsNullOrEmpty(Model.TableName)) ? String.Empty : Model.TableName; }
            set
            {
                if (Model.TableName != value)
                {
                    Model.TableName = value;
                    OnPropertyChanged("Name");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region Alias (string)

        private string _Alias;

        /// <summary>
        /// Gets or sets the string value for Alias
        /// </summary>
        /// <value> The string value.</value>

        public string Alias
        {
            get { return (String.IsNullOrEmpty(_Alias)) ? String.Empty : _Alias; }
            set
            {
                if (_Alias != value)
                {
                    _Alias = value;
                    OnPropertyChanged("Alias");
                }
            }
        }

        #endregion

        #region IsChecked (bool)

        private bool _IsChecked = true;

        /// <summary>
        /// Gets or sets the bool value for IsChecked
        /// </summary>
        /// <value> The bool value.</value>

        public bool IsChecked
        {
            get { return _IsChecked; }
            set
            {
                if (_IsChecked != value)
                {
                    _IsChecked = value;
                    OnPropertyChanged("IsChecked");
                }
            }
        }

        #endregion

        #region IsSelected (bool)

        private bool _IsSelected;

        /// <summary>
        /// Gets or sets the bool value for IsSelected
        /// </summary>
        /// <value> The bool value.</value>

        public bool IsSelected
        {

            set
            {
                _Master.SelectedDataTable = this;
            }
        }

        #endregion

        #region HasHeader (bool)

        private bool _HasHeader;

        /// <summary>
        /// Gets or sets the bool value for HasHeader
        /// </summary>
        /// <value> The bool value.</value>

        public bool HasHeader
        {
            get { return _HasHeader; }
            set
            {
                if (_HasHeader != value)
                {
                    ImplementHasHeader(value);
                    _HasHeader = value;
                    OnPropertyChanged("HasHeader");
                }
            }
        }

        #endregion

        #region XmlRoot (string)

        private string _XmlRoot;

        /// <summary>
        /// Gets or sets the string value for XmlRoot
        /// </summary>
        /// <value> The string value.</value>

        public string XmlRoot
        {
            get { return String.IsNullOrEmpty(_XmlRoot) ? null : _XmlRoot; }
            set
            {
                if (_XmlRoot != value)
                {
                    _XmlRoot = value;
                    OnPropertyChanged("XmlRoot");
                }
            }
        }

        #endregion

        #region Delimiter (DelimiterOption)

        private DelimiterOption _Delimiter;

        /// <summary>
        /// Gets or sets the DelimiterOption value for Delimiter
        /// </summary>
        /// <value> The DelimiterOption value.</value>

        public DelimiterOption Delimiter
        {
            get { return _Delimiter; }
            set
            {
                if (_Delimiter != value)
                {
                    _Delimiter = value;
                }
            }
        }

        #endregion

        #region Data (DataTable)



        /// <summary>
        /// Gets or sets the DataTable value for Data
        /// </summary>
        /// <value> The DataTable value.</value>

        public DataTable Data
        {
            get { return Model; }
        }

        #endregion

        public DataTableViewModel(DataTable model, DataSetViewModel master)
        {
            _Label = "Worksheet";
            Model = model;
            _Master = master;
            foreach (DataColumn item in model.Columns)
            {
                Columns.Add(new DataColumnViewModel(item, this));
            }
            GenerateMappedFields();
            GenerateLengths();

        }

        private void GenerateLengths()
        {
            int max = Model.Columns.Count;
            Dictionary<int, int> d = new Dictionary<int, int>();
            for (int i = 0; i < max; i++)
            {
                d.Add(i, 0);
            }
            foreach (DataRow row in Model.Rows)
            {
                for (int i = 0; i < max; i++)
                {
                    if (!Convert.IsDBNull(row[i]))
                    {
                        string s = row[i].ToString().Trim();
                        int j = s.Length;
                        if (j > d[i])
                        {
                            d[i] = j;
                        }
                    }
                }
            }
            for (int i = 0; i < Columns.Count; i++)
            {
                Columns[i].MaxLength = d[i];
            }
        }

        private void GenerateMappedFields()
        {
            MappedFields = new SortableObservableCollection<ColumnMap, ColumnMapViewModel>(
                    (from x in Columns
                     select new ColumnMap() { ViewModel = x }
                     ).ToList());
        }

        private void ImplementHasHeader(bool hasHeader)
        {
            if (hasHeader)
            {
                _Header = Model.NewRow();
                _Header.ItemArray = (object[])Model.Rows[0].ItemArray.Clone();
                for (int i = 0; i < Columns.Count; i++)
                {
                    Columns[i].Alias = _Header[i].ToString();
                }
                Model.Rows.RemoveAt(0);
            }
            else
            {
                Model.Rows.InsertAt(_Header, 0);
                int i = 1;
                foreach (DataColumnViewModel column in Columns)
                {
                    column.Alias = null;
                    i++;
                }
            }
        }
        public void MapColumn(string target, bool isChecked)
        {
            if (!isChecked)
            {
                var found = MappedFields.FirstOrDefault(x => x.Model.ViewModel.Name.Equals(target, StringComparison.OrdinalIgnoreCase));
                if (found != null)
                {
                    MappedFields.Remove(found);
                }
            }
            else
            {
                var found = Columns.FirstOrDefault(x => x.Name.Equals(target, StringComparison.OrdinalIgnoreCase));
                if (found != null)
                {
                    ColumnMapViewModel vm = new ColumnMapViewModel();
                    vm.Initialize(new ColumnMap() { ViewModel = found });
                    MappedFields.Add(vm);
                }
            }

        }


    }
}

