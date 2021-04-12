using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.ObjectModel;
using System.Windows;

namespace XTool
{
    public class DataSetViewModel : ViewModel<DataSet>
    {
        #region DataSetName (string)

        /// <summary>
        /// Gets or sets the string value for DataSetName
        /// </summary>
        /// <value> The string value.</value>

        public string DataSetName
        {
            get { return (String.IsNullOrEmpty(Model.DataSetName)) ? String.Empty : Model.DataSetName; }
            set
            {
                if (Model.DataSetName != value)
                {
                    Model.DataSetName = value;
                    OnPropertyChanged("DataSetName");
                    IsDirty = true;
                }
            }
        }

        #endregion


        #region Tables (ObservableCollection<DataTableViewModel>)

        private ObservableCollection<DataTableViewModel> _Tables = new ObservableCollection<DataTableViewModel>();

        /// <summary>
        /// Gets or sets the ObservableCollection<DataTableViewModel> value for Tables
        /// </summary>
        /// <value> The ObservableCollection<DataTableViewModel> value.</value>

        public ObservableCollection<DataTableViewModel> Tables
        {
            get { return _Tables; }
            set
            {
                if (_Tables != value)
                {
                    _Tables = value;
                }
            }
        }

        #endregion

        #region SelectedDataTable (DataTableViewModel)

        private DataTableViewModel _SelectedDataTable;

        /// <summary>
        /// Gets or sets the DataTableViewModel value for SelectedDataTable
        /// </summary>
        /// <value> The DataTableViewModel value.</value>

        public DataTableViewModel SelectedDataTable
        {
            get { return _SelectedDataTable; }
            set
            {
                if (_SelectedDataTable != value)
                {
                    _SelectedDataTable = value;
                    OnPropertyChanged("SelectedDataTable");
                    OnPropertyChanged("DataVisibility");
                }
            }
        }

        #endregion

        public ExcelHelper Helper { get; set; }

        public Visibility DataVisibility
        {
            get
            {
                return (SelectedDataTable != null) ? Visibility.Visible : Visibility.Hidden;
            }
        }

        public DataSetViewModel(DataSet model)
        {
            Model = model;
            foreach (DataTable item in model.Tables)
            {
                Tables.Add(new DataTableViewModel(item, this));
            }

        }

        public void ImplementHasHeader(bool hasHeader, string sheetName)
        {
            var found = this.Tables.Where(x => x.Name.Equals(sheetName));
            if (found != null)
            {

            }
        }
    }
}
