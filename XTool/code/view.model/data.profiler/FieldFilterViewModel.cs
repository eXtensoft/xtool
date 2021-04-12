using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace XTool
{
    public class FieldFilterViewModel : ViewModel<FieldFilter>
    {
        #region Name (string)

        /// <summary>
        /// Gets or sets the string value for Name
        /// </summary>
        /// <value> The string value.</value>

        public string Name
        {
            get { return Model.ColumnName; }
            set
            {
                if (Model.ColumnName != value)
                {
                    Model.ColumnName = value;
                    OnPropertyChanged("Name");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region Datatype (string)

        /// <summary>
        /// Gets or sets the string value for Datatype
        /// </summary>
        /// <value> The string value.</value>

        public string Datatype
        {
            get { return (String.IsNullOrEmpty(Model.ColumnDatatype)) ? String.Empty : Model.ColumnDatatype; }
            set
            {
                if (Model.ColumnDatatype != value)
                {
                    Model.ColumnDatatype = value;
                    OnPropertyChanged("ColumnDatatype");
                    IsDirty = true;
                }
            }
        }

        #endregion


        CollectionViewSource SelectedValues { get; set; }

        public ObservableCollection<FilterValueViewModel> Values { get; set; }

        public FieldFilterViewModel(FieldFilter model, Action notifyChanged)
        {
            Model = model;
            if (model.Values != null && model.Values.Count > 0)
            {
                Values = new ObservableCollection<FilterValueViewModel>(from x in model.Values select new FilterValueViewModel(x,notifyChanged));

            }
        }

        internal void ProcureFilter(List<FieldFilter> list)
        {
            List<FilterValue> values = (from x in Model.Values
                                        where x.IsSelected == true
                                        select new FilterValue() { Key = x.Key, Count = x.Count, IsSelected = true }).ToList();
            if (values != null && values.Count > 0)
            {
                list.Add( new FieldFilter() { ColumnDatatype = Datatype, ColumnName = Name, Values = values });
            }
        }

        public string ComposeFilterLogic()
        {
            StringBuilder sb = new StringBuilder();
            int i = 0;
            foreach (var filterValue in Values)
            {
                if (filterValue.IsSelected)
                {
                    if (i++ > 0)
                    {
                        sb.Append(" or ");
                    }
                    sb.AppendFormat("'{0}'", filterValue.Key);
                }
            }
            return String.Format("{0} = {1}", Name, sb.ToString());
        }

    }
}
