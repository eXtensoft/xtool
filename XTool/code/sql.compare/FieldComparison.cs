using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public class FieldComparison :  IEnumerable<Field>
    {
        public List<Field> Items { get; set; }

        public string TableSchema { get; set; }

        public string TableName { get; set; }

        private int _ComparisonCount;
        private CompareOption _CompareBy;
        private string _Key;

        public string FieldName
        {
            get
            {
                return (Items != null && Items.Count > 0) ? Items[0].Name : String.Empty;
            }
        }

        public FieldComparison(int comparisonCount,CompareOption compareBy, string key, string tableSchema, string tableName)
        {
            _ComparisonCount = comparisonCount;
            _CompareBy = compareBy;
            _Key = key;
            TableName = tableName;
            TableSchema = tableSchema;
            Items = new List<Field>();
        }
        public FieldComparison() { }
        public void Add(Field field)
        {
            Items.Add(field);
        }

        public string ComposeKey()
        {
            StringBuilder sb = new StringBuilder();
            if (!String.IsNullOrWhiteSpace(_Key))
            {
                sb.Append(_Key);
            }
            else
            {
                switch (_CompareBy)
                {
                    case CompareOption.None:
                    case CompareOption.Table:
                        sb.AppendFormat("{0}.{1}", Items[0].TableName, Items[0].Name);
                        break;
                    case CompareOption.Schema:
                    case CompareOption.Database:
                        sb.AppendFormat("{0}.{1}.{2}", Items[0].TableSchema, Items[0].TableName, Items[0].Name);
                        break;
                    default:
                        break;
                }
            }
            return sb.ToString();
        }

        IEnumerator<Field> IEnumerable<Field>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
