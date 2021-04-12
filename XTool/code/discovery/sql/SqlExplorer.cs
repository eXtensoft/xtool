using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XTool
{
    [Serializable]
    public class SqlExplorer
    {
        public string Text { get; set; }

        [XmlAttribute("asOf")]
        public DateTime AsOf { get; set; }

        [XmlAttribute("compareBy")]
        public CompareOption CompareBy { get; set; }

        [XmlIgnore]
        public int Ordinality { get; set; }

        [XmlAttribute("moniker")]
        public string Moniker { get; set; }

        public List<string> Schemas { get; set; }

        [XmlElement("Table")]
        public List<XTool.Discovery.SqlTable> Tables { get; set; }

        public List<XTool.Discovery.SqlTable> Views { get; set; }

        [XmlElement("StoredProcedure")]
        public List<XTool.Discovery.SqlStoredProcedure> StoredProcedures { get; set; }

        [XmlElement("SqlScript")]
        public List<XTool.Discovery.SqlScript> SqlScripts { get; set; }


        public string ConnectionString { get; set; }


        public SqlExplorer() { }

        public SqlExplorer(string connectionString)
        {
            ConnectionString = connectionString;
        }


        public void DiscoverMySql(string catalog)
        {
            if (!String.IsNullOrWhiteSpace(ConnectionString))
            {
                int tableCount = 0;
                List<XTool.Discovery.SqlTable> list = new List<XTool.Discovery.SqlTable>();
                string sqlTables = Resources.Discovery_Tables_MySql.Replace("[catalog]", catalog);
                XTool.Discovery.SqlTable table = new Discovery.SqlTable() { TableName = "x:none" };
                try
                {
                    using (MySqlConnection cn = new MySqlConnection(ConnectionString))
                    {
                        cn.Open();
                        using (MySqlCommand cmd = new MySqlCommand(sqlTables,cn))
                        {
                            
                            using (MySqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    string tableName = reader.GetString(reader.GetOrdinal("TableName"));
                                    if (table.TableName != tableName)
                                    {
                                        if (tableCount > 0)
                                        {
                                            XTool.Discovery.SqlTable t = table;
                                            list.Add(t);                                            
                                        }
                                        tableCount++;
                                        table = new Discovery.SqlTable(reader);
                                    }
                                    table.Columns.Add(new Discovery.SqlColumn(reader));
                                }

                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    var tt = table;
                    string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    System.Windows.MessageBox.Show(message);
                }


                int tableschemasize = 3;
                if (list.Count == 1 && String.IsNullOrWhiteSpace(list[0].Catalog))
                {

                }
                else
                {
                    foreach (var item in list)
                    {
                        int j = item.TableSchema.Length;
                        if (j > tableschemasize)
                        {
                            tableschemasize = j;
                        }
                    }

                    Schemas = new List<string>();

                    Tables = list;
                    foreach (var t in Tables)
                    {
                        if (!Schemas.Contains(t.TableSchema))
                        {
                            Schemas.Add(t.TableSchema);
                        }
                    }
                }

                Schemas.Sort();

            }




        }


        public void Discover()
        {
            if (!String.IsNullOrWhiteSpace(ConnectionString))
            {                
                List<XTool.Discovery.SqlTable> list = new List<XTool.Discovery.SqlTable>();
                List<XTool.Discovery.SqlParameter> sqlparams = new List<Discovery.SqlParameter>();
                List<XTool.Discovery.SqlTable> views = new List<Discovery.SqlTable>();
                string sqlTables = Resources.Discovery_Tables;
                string sqlSprocs = Resources.Discovery_StoredProcedures;
                string sqlViews = Resources.Discovery_Views;
                string viewFormat = "SELECT * FROM [{0}].[{1}] WHERE 1 = 0";
                List<Tuple<string, string, string, string>> viewsql = new List<Tuple<string, string, string, string>>();
                try
                {

                    using (SqlConnection cn = new SqlConnection(ConnectionString))
                    {
                        cn.Open();
                        using (SqlCommand cmd = new SqlCommand(sqlTables, cn))
                        {
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                list.Add(new XTool.Discovery.SqlTable(reader));

                                while (reader.NextResult())
                                {
                                    list.Add(new XTool.Discovery.SqlTable(reader));
                                }
                            }
                        }

                        using (SqlCommand cmd = new SqlCommand(sqlSprocs, cn))
                        {
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    sqlparams.Add(new XTool.Discovery.SqlParameter(reader));
                                }
                            }
                        }
                        using (SqlCommand cmd = new SqlCommand(sqlViews, cn))
                        {
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    string catalog = reader.GetString(0);
                                    string viewschema = reader.GetString(1);
                                    string viewname = reader.GetString(2);
                                    if (!String.IsNullOrEmpty(viewname))
                                    {
                                        viewsql.Add(new Tuple<string, string, string, string>(catalog, viewschema, viewname, String.Format(viewFormat, viewschema, viewname)));
                                    }
                                }
                            }

                        }
                        foreach (Tuple<string, string, string, string> item in viewsql)
                        {
                            using (SqlCommand cmd = new SqlCommand(item.Item4, cn))
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                DataTable dt = reader.GetSchemaTable();
                                Views.Add(new XTool.Discovery.SqlTable(item.Item1, item.Item2, item.Item3, dt));
                            }
                        }
                


                    }
                }
                catch (Exception ex)
                {

                    //MessageBox.Show(ex.Message);
                }
                int tableschemasize = 3;
                if (list.Count == 1 && String.IsNullOrWhiteSpace(list[0].Catalog))
                {
                    
                }
                else
                {
                    foreach (var item in list)
                    {
                        int j = item.TableSchema.Length;
                        if (j > tableschemasize)
                        {
                            tableschemasize = j;
                        }
                    }

                    Schemas = new List<string>();

                    Tables = list;
                    foreach (var table in Tables)
                    {
                        if (!Schemas.Contains(table.TableSchema))
                        {
                            Schemas.Add(table.TableSchema);
                        }
                    }

                    Dictionary<string, List<XTool.Discovery.SqlParameter>> d = new Dictionary<string, List<XTool.Discovery.SqlParameter>>();
                    foreach (var item in sqlparams)
                    {
                        if (!Schemas.Contains(item.Schema))
                        {
                            Schemas.Add(item.Schema);
                        }
                        if (!d.ContainsKey(item.StoredProcedureName))
                        {
                            d.Add(item.StoredProcedureName, new List<XTool.Discovery.SqlParameter>());
                        }
                        d[item.StoredProcedureName].Add(item);
                    }
                    StoredProcedures = new List<XTool.Discovery.SqlStoredProcedure>();
                    int sprocschemasize = 3;
                    foreach (var item in d)
                    {

                        XTool.Discovery.SqlStoredProcedure sproc = new Discovery.SqlStoredProcedure(item.Value);
                        if (!Schemas.Contains(sproc.Schema))
                        {
                            Schemas.Add(sproc.Schema);
                        }
                        if (sproc.Schema.Length > sprocschemasize)
                        {
                            sprocschemasize = sproc.Schema.Length;
                        }
                        StoredProcedures.Add(sproc);
                    }


                    Schemas.Sort();
                }
            
            }
        }
    }
}
