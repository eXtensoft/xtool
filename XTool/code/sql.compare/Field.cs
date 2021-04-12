using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public class Field
    {
        private Discovery.SqlColumn _Column;

        public string TableName { get; set; }
        public string TableSchema { get; set; }

        public string ScriptText { get; set; }


        private string _Name;
        public string Name
        {
            get
            {
                return (String.IsNullOrWhiteSpace(_Name) )?  _Column.ColumnName: _Name;
            }
            set { _Name = value; }
        }

        public string Datatype { get { return _Column.Datatype; } }

        public string Display
        {
            get
            {
                return (_Column != null) ?  _Column.ToString() : String.Format("{0}.{1}",TableSchema,Name);
            }
        }

        public int Ordinality { get; set; }

        public Field() { }

        public Field(Discovery.SqlColumn column,string tableName, string tableSchema)
        {
            _Column = column;
            TableName = tableName;
            TableSchema = tableSchema;
        }

        public string ComposeKey(CompareOption compareBy)
        {
            StringBuilder sb = new StringBuilder();
            if (String.IsNullOrWhiteSpace(ScriptText))
            {


                switch (compareBy)
                {
                    case CompareOption.None:
                    case CompareOption.Table:
                        sb.AppendFormat("{0}.{1}", TableName, Name);
                        break;
                    case CompareOption.Schema:
                    case CompareOption.Database:
                        sb.AppendFormat("{0}.{1}.{2}", TableSchema, TableName, Name);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                sb.AppendFormat("{0}.{1}", TableSchema, Name);
            }
            return sb.ToString();
        }
    }
}
