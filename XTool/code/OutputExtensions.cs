using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Data;
using System.IO;

namespace XTool
{
    public static class OutputExtensions
    {
        public static void ToXmlFilepath(this DataTable dt, string filePath, string rootName, string itemName)
        {
            XDocument xdoc = dt.ToXDocument(rootName, itemName);
            xdoc.Save(filePath);
        }

        public static void ToXmlFilepath(this DataTable dt, string filePath)
        {
            XDocument xdoc = dt.ToXDocument(String.Empty, String.Empty);
            xdoc.Save(filePath);
        }

        public static XDocument ToXDocument(this DataTable dt, string rootName, string itemName)
        {
            string t = (!String.IsNullOrEmpty(itemName.Trim())) ? itemName.Trim().ToAlphaNumericOnly() : String.Empty;
            string r = (!String.IsNullOrEmpty(rootName.Trim())) ? rootName.Trim().ToAlphaNumericOnly() : String.Empty;
            string item = (!String.IsNullOrEmpty(t)) ? t : (!String.IsNullOrEmpty(dt.TableName)) ? dt.TableName.ToAlphaNumericOnly() : "item";
            string root = (!String.IsNullOrEmpty(r)) ? r : String.Format("{0}s", item);

            List<Tuple<string, string>> list = new List<Tuple<string, string>>();
            foreach (DataColumn column in dt.Columns)
            {
                list.Add(new Tuple<string, string>(column.ColumnName.Replace(" ", String.Empty), column.ColumnName));
            }

            XDocument xdoc = new XDocument(new XDeclaration("1.0", "utf-8", "true"),
                new XElement(root, from row in dt.AsEnumerable()
                                   select new XElement(item, from x in list
                                                             select new XElement(x.Item1, row[x.Item2])
                                                               )));
            return xdoc;
        }

        public static void ToDelimitedFilepath(this DataTable table, string filePath, char delimiter)
        {
            StringBuilder sb = new StringBuilder();
            char quote = (char)34;
            bool isComma = ((int)delimiter == 44);
            bool isTab = ((int)delimiter == 9);
            StringBuilder header = new StringBuilder();
            List<Tuple<int, string>> list = new List<Tuple<int, string>>();
            int j = 0;

            foreach (DataColumn column in table.Columns)
            {
                if (j > 0)
                {
                    header.Append(delimiter);
                }
                string s = column.ColumnName.Replace(" ", String.Empty);
                list.Add(new Tuple<int, string>(j, s));
                header.Append(s);
                j++;

            }
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(filePath, false))
            {
                sw.Write(header.ToString());
                sw.Write(sw.NewLine);
                foreach (DataRow row in table.Rows)
                {
                    StringBuilder line = new StringBuilder();
                    int i = 0;
                    foreach (var item in list)
                    {
                        int index = item.Item1;
                        if (!Convert.IsDBNull(row[index]))
                        {

                            if (i > 0)
                            {
                                line.Append(delimiter);
                            }
                            string p = row[index].ToString();
                            if (isComma & (p.Contains(',') | p.Contains('"')))
                            {
                                line.Append(quote + p + quote);
                            }
                            else
                            {
                                line.Append(p);
                            }
                        }
                        else if (i > 0)
                        {
                            line.Append(delimiter);
                        }
                        i++;
                    }
                    sw.Write(line.ToString());
                    sw.Write(sw.NewLine);
                }

            }
        }

        public static string ToDynamic(this DataTable table, string recordDelimiter, string fieldDelimiter, string prePend, string postPend)
        {

            string fieldFormat = String.Empty;


            StringBuilder sb = new StringBuilder();


            StringBuilder header = new StringBuilder();
            List<Tuple<int, string>> list = new List<Tuple<int, string>>();
            int j = 0;

            foreach (DataColumn column in table.Columns)
            {
                if (j > 0)
                {
                    header.Append(fieldDelimiter);
                }
                string s = column.ColumnName.Replace(" ", String.Empty);
                list.Add(new Tuple<int, string>(j, s));
                header.Append(s);
                j++;
            }


            int k = 0;
            foreach (DataRow row in table.Rows)
            {
                if (k > 0 && !String.IsNullOrWhiteSpace(recordDelimiter))
                {
                    sb.Append(recordDelimiter);
                }
                k++;
                StringBuilder line = new StringBuilder();
                int i = 0;
                foreach (var item in list)
                {
                    int index = item.Item1;
                    if (!Convert.IsDBNull(row[index]))
                    {
                        if (i > 0 && !String.IsNullOrWhiteSpace(fieldDelimiter))
                        {
                            line.Append(fieldDelimiter);
                        }
                        string p = row[index].ToString();
                        line.Append(prePend + p + postPend);
                    }
                    i++;
                }

                sb.Append(line.ToString());

            }
            return sb.ToString();
        }

        public static string ToDelimited(this DataTable table, char delimiter)
        {
            StringBuilder sb = new StringBuilder();
            char quote = (char)34;
            bool isComma = ((int)delimiter == 44);
            bool isTab = ((int)delimiter == 9);
            StringBuilder header = new StringBuilder();
            List<Tuple<int, string>> list = new List<Tuple<int, string>>();
            int j = 0;

            foreach (DataColumn column in table.Columns)
            {
                if (j > 0)
                {
                    header.Append(delimiter);
                }
                string s = column.ColumnName.Replace(" ", String.Empty);
                list.Add(new Tuple<int, string>(j, s));
                header.Append(s);
                j++;
            }
            StringBuilder sw = new StringBuilder();
            sw.AppendLine(header.ToString());

            foreach (DataRow row in table.Rows)
            {
                StringBuilder line = new StringBuilder();
                int i = 0;
                foreach (var item in list)
                {
                    int index = item.Item1;
                    if (!Convert.IsDBNull(row[index]))
                    {

                        if (i > 0)
                        {
                            line.Append(delimiter);
                        }
                        string p = row[index].ToString();
                        if (isComma & (p.Contains(',') | p.Contains('"')))
                        {
                            line.Append(quote + p + quote);
                        }
                        else
                        {
                            line.Append(p);
                        }
                    }
                    else if (i > 0)
                    {
                        line.Append(delimiter);
                    }
                    i++;
                }
                sw.AppendLine(line.ToString());
            }
            return sw.ToString();

        }



        public static void ToFixedFilepath(this DataTable table, string filePath, int padding, PadOption option)
        {
            int max = table.Columns.Count;
            StringBuilder sb = new StringBuilder();
            Dictionary<int, int> d = new Dictionary<int, int>();
            for (int i = 0; i < max; i++)
            {
                d.Add(i, table.Columns[i].ColumnName.Length);
            }
            StringBuilder header = new StringBuilder();
            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < max; i++)
                {
                    if (row[i] != DBNull.Value)
                    {
                        int j = row[i].ToString().Length;
                        if (j > d[i])
                        {
                            d[i] = j;
                        }
                    }
                }
            }
            for (int i = 0; i < max; i++)
            {
                d[i] += padding;
            }

            for (int i = 0; i < max; i++)
            {
                string s = table.Columns[i].ColumnName;
                int length = d[i];
                if (option == PadOption.Right)
                {
                    header.Append(s.PadRight(length));
                }
                else
                {
                    header.Append(s.PadLeft(length));
                }
            }

            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(filePath, false))
            {
                sw.WriteLine(header.ToString());
                foreach (DataRow row in table.Rows)
                {
                    StringBuilder line = new StringBuilder();
                    foreach (var item in d)
                    {
                        if (!Convert.IsDBNull(row[item.Key]))
                        {
                            string s = row[item.Key].ToString();
                            string field = (option == PadOption.Right) ? s.PadRight(item.Value) : s.PadLeft(item.Value);
                            line.Append(field);
                        }
                        else
                        {
                            line.Append(new string(' ', item.Value));
                        }
                    }
                    sw.WriteLine(line.ToString());
                }
            }
        }

        public static string ToFixed(this DataTable table, int padding, PadOption option)
        {
            int max = table.Columns.Count;
            StringBuilder sb = new StringBuilder();
            Dictionary<int, int> d = new Dictionary<int, int>();
            for (int i = 0; i < max; i++)
            {
                d.Add(i, table.Columns[i].ColumnName.Length);
            }
            StringBuilder header = new StringBuilder();
            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < max; i++)
                {
                    if (row[i] != DBNull.Value)
                    {
                        int j = row[i].ToString().Length;
                        if (j > d[i])
                        {
                            d[i] = j;
                        }
                    }
                }
            }
            for (int i = 0; i < max; i++)
            {
                d[i] += padding;
            }

            for (int i = 0; i < max; i++)
            {
                string s = table.Columns[i].ColumnName;
                int length = d[i];
                if (option == PadOption.Right)
                {
                    header.Append(s.PadRight(length));
                }
                else
                {
                    header.Append(s.PadLeft(length));
                }
            }

            StringBuilder sw = new StringBuilder();
            sw.AppendLine(header.ToString());
            foreach (DataRow row in table.Rows)
            {
                StringBuilder line = new StringBuilder();
                foreach (var item in d)
                {
                    if (!Convert.IsDBNull(row[item.Key]))
                    {
                        string s = row[item.Key].ToString();
                        string field = (option == PadOption.Right) ? s.PadRight(item.Value) : s.PadLeft(item.Value);
                        line.Append(field);
                    }
                    else
                    {
                        line.Append(new string(' ', item.Value));
                    }
                }
                sw.AppendLine(line.ToString());
            }
            return sw.ToString();
        }

        //public static string ToAlphaNumericOnly(this string s)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    foreach (char c in s.ToCharArray())
        //    {
        //        if (Char.IsLetterOrDigit(c))
        //        {
        //            sb.Append(c);
        //        }
        //    }
        //    return sb.ToString();
        //}

        public static Dictionary<string, Dictionary<string, object>> DataTableToDictionary(this DataTable dt, string id)
        {
            var cols = dt.Columns.Cast<DataColumn>().Where(c => c.ColumnName != id);
            return dt.Rows.Cast<DataRow>()
                     .ToDictionary(r => r[id].ToString(),
                                   r => cols.ToDictionary(c => c.ColumnName, c => r[c.ColumnName]));
        }

        public static string ToJson(this DataTable dt, string entityName = "")
        {
            bool isEntity = !String.IsNullOrEmpty(entityName);
            StringBuilder sb = new StringBuilder();
            string indent = @"  ";
            sb.AppendLine("[");
            List<Tuple<int, string, bool>> schema = new List<Tuple<int, string, bool>>();
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                DataColumn column = dt.Columns[i];
                if (_TypeIsNumeric.ContainsKey(column.DataType.Name))
                {
                    var found = _TypeIsNumeric[column.DataType.Name];
                    schema.Add(new Tuple<int, string, bool>(i, dt.Columns[i].ColumnName, _TypeIsNumeric[column.DataType.Name]));
                }
            }
            int max = dt.Rows.Count - 1;
            for (int i = 0; i <= max; i++)
            {
                DataRow row = dt.Rows[i];
                sb.AppendLine("  {");

                string s = String.Empty;
                bool b = false;
                int schemaMax = schema.Count - 1;
                for (int j = 0; j <= schemaMax; j++)
                {
                    var item = schema[j];
                    object o = row[item.Item1];
                    if (o != null && !String.IsNullOrEmpty(o.ToString()))
                    {
                        if (item.Item3)
                        {
                            if (j < schemaMax)
                            {
                                s = String.Format("  \"{0}\" : {1},", item.Item2, row[item.Item1].ToString(), indent);
                            }
                            else
                            {
                                s = String.Format("  \"{0}\" : {1}", item.Item2, row[item.Item1].ToString(), indent);
                            }
                        }
                        else
                        {
                            if (j < schemaMax)
                            {
                                s = String.Format("  \"{0}\" : \"{1}\",", item.Item2, row[item.Item1].ToString(), indent);
                            }
                            else
                            {
                                s = String.Format("  \"{0}\" : \"{1}\"", item.Item2, row[item.Item1].ToString(), indent);
                            }                            
                        }
                        sb.AppendLine(String.Format("  {0}", s));
                    }
                    
                }
                if (i < max)
                {
                    sb.AppendLine("  },");
                }
                else
                {
                    sb.AppendLine("  }");
                }

            }
            sb.AppendLine("]");

            return sb.ToString();
        }

        public static string ToRazorTemplate(this DataTable dt)
        {

            string name = (!String.IsNullOrEmpty(dt.TableName)) ? dt.TableName : "ResultSet";

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("@{ bool b = false; }");
            sb.AppendLine(String.Format("<h2>{0}</h2>", name));
            sb.AppendLine("<div id=\"dt\">");
            sb.AppendLine("<table>");
            sb.AppendLine("\t<thead>");
            sb.AppendLine("\t\t<tr>");
            int max = dt.Columns.Count;
            for (int i = 0; i < max; i++)
            {
                sb.AppendLine(String.Format("\t\t\t<th>{0}</th>", dt.Columns[i].ColumnName));
            }
            sb.AppendLine("\t\t</tr>");
            sb.AppendLine("\t</thead>");
            sb.AppendLine("\t<tbody>");

            sb.AppendLine("@foreach (var @m in @Model.Items)");
            sb.AppendLine("{");
            sb.AppendLine("\tb = !b;");
            sb.AppendLine("\tif(!b)");
            sb.AppendLine("\t{");

            sb.AppendLine("\t\t@:<tr class='alt'>");
            sb.AppendLine("\t}");
            sb.AppendLine("\telse");
            sb.AppendLine("\t{");
            sb.AppendLine("\t\t@:<tr>");
            sb.AppendLine("}");
            for (int i = 0; i < max; i++)
            {
                bool isNumeric = _TypeIsNumeric[dt.Columns[i].DataType.Name];
                if (!isNumeric)
                {
                    sb.AppendLine(String.Format("\t\t\t<td>@m.{0}</td>", dt.Columns[i].ColumnName));
                }
                else
                {
                    sb.AppendLine(String.Format("\t\t\t<td class=\"data-numeric\">@m.{0}</td>", dt.Columns[i].ColumnName));
                }

            }
            sb.AppendLine("\t\t</tr>");
            sb.AppendLine("}");
            sb.AppendLine("\t</tbody>");
            sb.AppendLine("</table>");
            sb.AppendLine("</div>");

            return sb.ToString();
        }

        public static void ToJsonFilepath(this DataTable dt, string filePath, string entityName = "")
        {
            File.WriteAllText(filePath, dt.ToJson(entityName));
        }

        private static Dictionary<string, bool> _TypeIsNumeric = new Dictionary<string, bool>()
        {
            {"Boolean",false},
            {"Byte",true},
            {"Char",false},
            {"DateTime",false},
            {"Decimal",true},
            {"Double",true},
            {"Int16",true},
            {"Int32",true},
            {"Int64",true},
            {"SByte",true},
            {"String",false},
            {"TimeSpan",false},
            {"UInt16",true},
            {"UInt32",true},
            {"UInt64",true},
            {"Byte[]",true},
        };
    }
}
