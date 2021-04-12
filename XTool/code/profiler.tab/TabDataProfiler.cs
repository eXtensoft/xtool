using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public class TabDataProfiler
    {
        public DataProfileFieldCollection Fields { get; set; }
        public int RecordCount { get; set; }
        public int FieldCount { get; set; }
        private ProfilerSettings _Settings;
        private List<FieldFilter> _Filters;
        private bool _IsAnd = false;

        public TabDataProfiler(ProfilerSettings settings, List<FieldFilter> filters, bool isAnd)
        {
            _Settings = settings;
            Fields = new DataProfileFieldCollection();
            _Filters = filters;
            _IsAnd = isAnd;

        }

        public TabDataProfiler(ProfilerSettings settings)
        :this(settings, new List<FieldFilter>(),false){}

        public void Execute(IDataReader reader)
        {

            SetupFields(reader);
            ExecuteProfile(reader);
            Calculate();
        }

        private bool PassFilters(IDataReader reader)
        {
            bool b = true;
            if (_Filters.Count > 0)
            {
                if (!_IsAnd)
                {
                    b = false;
                    for (int i = 0;!b && i < _Filters.Count; i++)
                    {
                    
                        var filter = _Filters[i];
                        string fieldValue = reader[filter.ColumnIndex].ToString();
                        if (filter.FilterValues.Contains(fieldValue))
                        {
                            b = true;
                        }
                    } 
                }
                else
                {
                    for (int i = 0;b && i < _Filters.Count; i++)
                    {
                        var filter = _Filters[i];
                        string fieldValue = reader[filter.ColumnIndex].ToString();
                        if (!filter.FilterValues.Contains(fieldValue))
                        {
                            b = false;
                        }
                    }
                }
               
            }

            return b;
        }

        private void ExecuteProfile(IDataReader reader)
        {
            while (reader.Read())
            {
                for (int i = 0; i < FieldCount; i++)
                {
                    if (PassFilters(reader))
                    {
                        Fields[i].Execute(reader[i].ToString());
                    }
                    
                }
                RecordCount++;
            }
        }

        private void SetupFields(IDataReader reader)
        {
            DataTable dt = reader.GetSchemaTable();
            foreach (DataRow dr in dt.Rows)
            {
                DataProfileField fld = new DataProfileField(dr) { Index = FieldCount, MaxDistinct = _Settings.MaxDistinct };
                Fields.Add(fld);
                FieldCount++;
                var found = _Filters.FirstOrDefault(x => x.ColumnName.Equals(fld.Name));
                if (found != null)
                {
                    found.ColumnIndex = fld.Index;
                    found.FilterValues = new List<string>();
                    foreach (var item in found.Values)
                    {
                        found.FilterValues.Add(item.Key);
                    }
                }
            }
        }

        public void Execute<T>(IEnumerable<T> list, string[] fields) where T : class , new()
        {
            SetupFields(list, fields);
            ExecuteProfile(list);
            Calculate();
        }

        private void ExecuteProfile<T>(IEnumerable<T> list) where T : class, new()
        {
            foreach (var item in list)
            {
                foreach (var fld in Fields)
                {
                    fld.Execute(item);
                }
            }
        }

        private void SetupFields<T>(IEnumerable<T> list, string[] fields) where T : class, new()
        {
            T t = Activator.CreateInstance<T>();
            Type type = t.GetType();
            PropertyInfo[] infos = type.GetProperties();
            foreach (PropertyInfo info in infos)
            {
                if (fields.Contains(info.Name))
                {
                    DataProfileField fld = new DataProfileField(info) { MaxDistinct = _Settings.MaxDistinct };
                    Fields.Add(fld);
                    FieldCount++;
                }
            }
        }

        internal void Calculate()
        {
            //   ming rocks!
            foreach (var field in Fields)
            {
                field.CharProfile.Summarize();
                field.Calculate(field.HasData);
            }
        }
        
        public bool CanGetFilters()
        {
            return Fields != null && Fields.Count > 0;
        }
        public List<FieldFilter> GetFilters()
        {
            return (from x in Fields where x.IsSelected == true
                    select x.GetFilter()).ToList();
        }
    
    }

}

