using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml.Linq;
using System.IO;
using System.Xml.Serialization;

namespace XTool
{
    public static class DataToolExtensionMethods
    {
        public static string ToAlphaNumericOnly(this string s)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in s.ToCharArray())
            {
                if (Char.IsLetterOrDigit(c))
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        public static void ToCsvFile(this DataTable dt, string filePath)
        {
            char apostrophe = (char)39;
            char quote = (char)34;
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(filePath, false))
            {
                int max = dt.Columns.Count;
                for (int i = 0; i < max; i++)
                {
                    if (i > 0)
                    {
                        sw.Write(",");
                    }
                    sw.Write(dt.Columns[i].ColumnName);

                }
                sw.Write(sw.NewLine);

                foreach (DataRow row in dt.Rows)
                {
                    for (int i = 0; i < max; i++)
                    {
                        if (!Convert.IsDBNull(row[i]))
                        {
                            if (i > 0)
                            {
                                sw.Write(",");
                            }
                            string s = row[i].ToString().Replace(quote, apostrophe);
                            if (s.Contains(',') | s.Contains('"'))
                            {
                                sw.Write(quote + s + quote);
                            }
                            else
                            {
                                sw.Write(s);
                            }

                        }
                        else if (i > 0)
                        {
                            sw.Write(",");
                        }
                    }
                    sw.Write(sw.NewLine);
                }

            }
        }



        public static void ToCsvFilepath(this DataTableViewModel table, char delimiter, string filePath)
        {
            StringBuilder sb = new StringBuilder();
            char quote = (char)34;
            bool isComma = ((int)delimiter == 44);
            bool isTab = ((int)delimiter == 9);
            StringBuilder header = new StringBuilder();
            List<Tuple<int, string>> list = new List<Tuple<int, string>>();
            int j = 0;

            foreach (ColumnMapViewModel vm in table.MappedFields)
            {
                var found = table.Columns.FirstOrDefault(x => x.Name.Equals(vm.Model.ViewModel.Name, StringComparison.OrdinalIgnoreCase));
                int index = table.Columns.IndexOf(found);
                string s = found.Display;
                list.Add(new Tuple<int, string>(index, s));
                if (j > 0)
                {
                    header.Append(delimiter);
                }
                header.Append(s);
                j++;
            }
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(filePath, false))
            {
                sw.Write(header.ToString());
                sw.Write(sw.NewLine);
                foreach (DataRow row in table.Model.Rows)
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



        public static string ToCsv(this DataTableViewModel table, char delimiter)
        {
            StringBuilder sb = new StringBuilder();
            char quote = (char)34;
            char apostrophe = (char)39;

            bool isComma = ((int)delimiter == 44);
            bool isTab = ((int)delimiter == 9);

            StringBuilder header = new StringBuilder();
            List<Tuple<int, string>> list = new List<Tuple<int, string>>();
            int j = 0;
            foreach (ColumnMapViewModel vm in table.MappedFields)
            {
                var found = table.Columns.FirstOrDefault(x => x.Name.Equals(vm.Model.ViewModel.Name, StringComparison.OrdinalIgnoreCase));
                int index = table.Columns.IndexOf(found);
                string s = found.Display;
                list.Add(new Tuple<int, string>(index, s));
                if (j > 0)
                {
                    header.Append(delimiter);
                }
                header.Append(s);
                j++;
            }

            sb.AppendLine(header.ToString());

            foreach (DataRow row in table.Model.Rows)
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
                        //string s = isComma ? p.Replace(quote, apostrophe) : p;
                        //string s = (p.IndexOf(",") > -1) ? 

                        if (isComma & (p.Contains(',') | p.Contains('"')))
                        {
                            line.Append(quote + p + quote);
                        }
                        else
                        {
                            line.Append(p);
                        }

                        //string s = (isComma) ? row[i].ToString().Replace(quote, apostrophe), row[i].ToString();
                    }
                    else if (i > 0)
                    {
                        line.Append(delimiter);
                    }
                    i++;
                }
                sb.AppendLine(line.ToString());
            }
            return sb.ToString();
        }



        public static string ToCsv(this DataTableViewModel table)
        {
            char apostrophe = (char)39;
            char quote = (char)34;

            StringBuilder sb = new StringBuilder();
            StringBuilder header = new StringBuilder();
            List<Tuple<int, string>> list = new List<Tuple<int, string>>();
            int j = 0;

            foreach (ColumnMapViewModel vm in table.MappedFields)
            {
                var found = table.Columns.FirstOrDefault(x => x.Name.Equals(vm.Model.ViewModel.Name, StringComparison.OrdinalIgnoreCase));
                int index = table.Columns.IndexOf(found);
                string s = found.Display;
                list.Add(new Tuple<int, string>(index, s));
                if (j > 0)
                {
                    header.Append(",");
                }
                header.Append(s);
                j++;
            }


            sb.AppendLine(header.ToString());
            foreach (DataRow row in table.Model.Rows)
            {
                StringBuilder sbline = new StringBuilder();
                int i = 0;
                foreach (var item in list)
                {
                    if (!Convert.IsDBNull(row[i]))
                    {
                        if (i > 0)
                        {
                            sbline.Append(",");
                        }
                        string s = row[i].ToString().Replace(quote, apostrophe);
                        if (s.Contains(',') | s.Contains('"'))
                        {
                            sbline.Append(quote + s + quote);
                        }
                        else
                        {
                            sbline.Append(s);
                        }

                    }
                    else if (i > 0)
                    {
                        sbline.Append(",");
                    }
                    i++;

                }
                sb.AppendLine(sbline.ToString());
            }

            return sb.ToString();

        }

        public static XDocument ToXDoc(this DataTable dt)
        {
            List<Tuple<string, string>> list = new List<Tuple<string, string>>();
            foreach (DataColumn column in dt.Columns)
            {
                list.Add(new Tuple<string, string>(column.ColumnName.Replace(" ", String.Empty), column.ColumnName));
            }
            string item = dt.TableName.Replace(" ", String.Empty);
            string root = String.Format("{0}s", item);
            XDocument xdoc = new XDocument(new XDeclaration("1.0", "utf-8", "true"),
                new XElement(root, from row in dt.AsEnumerable()
                                   select new XElement(item, from x in list
                                                             select new XElement(x.Item1, row[x.Item2])
                                                               )));
            return xdoc;

        }


        public static string ToXml(this DataSet ds)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (TextWriter streamWriter = new StreamWriter(memoryStream))
                {
                    var xmlSerializer = new XmlSerializer(typeof(DataSet));
                    xmlSerializer.Serialize(streamWriter, ds);
                    return Encoding.UTF8.GetString(memoryStream.ToArray());
                }
            }
        }

        public static XDocument ToXDocument(this DataTableViewModel table)
        {
            Mapping maps = new Mapping();
            foreach (ColumnMapViewModel vm in table.MappedFields)
            {
                var found = table.Columns.FirstOrDefault(x => x.Name.Equals(vm.Model.ViewModel.Name, StringComparison.OrdinalIgnoreCase));
                maps.Add(new Map(found.Display, found.Name));
            }

            string t = !String.IsNullOrEmpty(table.Alias) ? table.Alias.Replace(" ", String.Empty) : table.Model.TableName.Replace(" ", String.Empty);
            string root = t + "s";
            string xmlroot = table.XmlRoot ?? root;
            XDocument xdoc = new XDocument(new XDeclaration("1.0", "utf-8", "true"),
                new XElement(xmlroot, from row in table.Model.AsEnumerable()
                                      select new XElement(t, from map in maps
                                                             select new XElement(map.X.Replace("#", String.Empty), row[map.Y])
                                                             )));

            return xdoc;
        }

        public static void ToXmlFilepath(this DataTableViewModel table, string fullFilepath)
        {
            XDocument xdoc = table.ToXDocument();
            xdoc.Save(fullFilepath);
        }

        public static string ToFixed(this DataTableViewModel table)
        {
            StringBuilder sb = new StringBuilder();
            Dictionary<int, int> d = new Dictionary<int, int>();
            StringBuilder header = new StringBuilder();
            foreach (ColumnMapViewModel vm in table.MappedFields)
            {

                var found = table.Columns.FirstOrDefault(x => x.Name.Equals(vm.Model.ViewModel.Name, StringComparison.OrdinalIgnoreCase));
                int index = table.Columns.IndexOf(found);
                string display = found.Display;
                int length = (display.Length > found.MaxLength) ? display.Length : found.MaxLength;
                d.Add(index, length + table.Padding);
                if (table.IsPadRight)
                {
                    header.Append(display.PadRight(length + table.Padding));
                }
                else
                {
                    header.Append(display.PadLeft(length + table.Padding));
                }

            }
            sb.AppendLine(header.ToString());
            foreach (DataRow row in table.Model.Rows)
            {
                StringBuilder line = new StringBuilder();
                foreach (var item in d)
                {
                    if (!Convert.IsDBNull(row[item.Key]))
                    {
                        string s = row[item.Key].ToString();
                        string field = (table.IsPadRight) ? s.PadRight(item.Value) : s.PadLeft(item.Value);
                        line.Append(field);
                    }
                    else
                    {
                        line.Append(new string(' ', item.Value));
                    }
                }
                sb.AppendLine(line.ToString());
            }
            return sb.ToString();
        }

        public static void ToFixedFilepath(this DataTableViewModel table, string filePath)
        {
            StringBuilder sb = new StringBuilder();
            Dictionary<int, int> d = new Dictionary<int, int>();
            StringBuilder header = new StringBuilder();
            foreach (ColumnMapViewModel vm in table.MappedFields)
            {
                var found = table.Columns.FirstOrDefault(x => x.Name.Equals(vm.Model.ViewModel.Name, StringComparison.OrdinalIgnoreCase));
                int index = table.Columns.IndexOf(found);
                string display = found.Display;
                int length = (display.Length > found.MaxLength) ? display.Length : found.MaxLength;
                d.Add(index, length + table.Padding);
                if (table.IsPadRight)
                {
                    header.Append(display.PadRight(length + table.Padding));
                }
                else
                {
                    header.Append(display.PadLeft(length + table.Padding));
                }
            }
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(filePath, false))
            {
                sw.WriteLine(header.ToString());
                foreach (DataRow row in table.Model.Rows)
                {
                    StringBuilder line = new StringBuilder();
                    foreach (var item in d)
                    {
                        if (!Convert.IsDBNull(row[item.Key]))
                        {
                            string s = row[item.Key].ToString();
                            string field = (table.IsPadRight) ? s.PadRight(item.Value) : s.PadLeft(item.Value);
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

    }
}
