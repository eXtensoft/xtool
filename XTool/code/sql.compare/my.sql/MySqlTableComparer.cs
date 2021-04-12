using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public static class MySqlTableComparer
    {
        public static DataTable Execute(string connectionString, string sqlText, Func<IDataReader, Tuple<string, string>> borrower)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add(new DataColumn("Field", typeof(string)));
            dt.Columns.Add(new DataColumn("Count", typeof(Int32)));
            int cIndex = 2;
            int rIndex = 0;

            SortedDictionary<string, int> rows = new SortedDictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            SortedDictionary<string, int> columns = new SortedDictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            Dictionary<string, int> rowIndex = new Dictionary<string, int>();
            Dictionary<string, int> columnIndex = new Dictionary<string, int>();

            int j = 1;

            try
            {
                using (MySqlConnection cn = new MySqlConnection(connectionString))
                {
                    cn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(sqlText, cn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Tuple<string, string> schema = borrower.Invoke(reader);

                                if (!rows.ContainsKey(schema.Item2))
                                {
                                    rows.Add(schema.Item2, 0);
                                }
                                rows[schema.Item2]++;

                                if (!columns.ContainsKey(schema.Item1))
                                {
                                    columns.Add(schema.Item1, 0);
                                }
                                columns[schema.Item1]++;
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


            foreach (var item in columns)
            {
                Console.WriteLine(String.Format("Creating column: {0}", item.Key));

                DataColumn dc = new DataColumn(item.Key, typeof(Int32));
                dt.Columns.Add(dc);
                columnIndex.Add(item.Key, cIndex++);
            }
            foreach (var item in rows)
            {
                DataRow row = dt.NewRow();
                row[0] = item.Key;
                row["Count"] = item.Value;
                dt.Rows.Add(row);
                rowIndex.Add(item.Key, rIndex++);
            }

            DataRow finalRow = dt.NewRow();
            foreach (var item in columns)
            {
                finalRow[item.Key] = item.Value;
            }
            dt.Rows.Add(finalRow);

            try
            {
                using (MySqlConnection cn = new MySqlConnection(connectionString))
                {
                    cn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(sqlText, cn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Tuple<string, string> schema = borrower.Invoke(reader);

                                int rowI = rowIndex[schema.Item2];
                                int colI = columnIndex[schema.Item1];
                                DataRow row = dt.Rows[rowIndex[schema.Item2]];
                                row[colI] = 1;
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

            return dt;
        }


    }
}
