using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Data.Linq;
using System.Globalization;

namespace XTool
{
    public static class ExtensionMethods
    {
        public static int GetIso8601WeekOfYear(this DateTime target)
        {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(target);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                target.AddDays(3);
            }
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(target, CalendarWeekRule.FirstFourDayWeek,DayOfWeek.Monday);
        }

        public static DateTime FirstDateOfWeek(int year, int weekOfYear, System.Globalization.CultureInfo cultureInfo)
        {
            DateTime beginningOfYear = new DateTime(year, 1, 1);
            int dayOffset = (int)cultureInfo.DateTimeFormat.FirstDayOfWeek - (int)beginningOfYear.DayOfWeek;
            DateTime firstWeekday = beginningOfYear.AddDays(dayOffset);
            int firstWeek = cultureInfo.Calendar.GetWeekOfYear(beginningOfYear, cultureInfo.DateTimeFormat.CalendarWeekRule, cultureInfo.DateTimeFormat.FirstDayOfWeek);
            if ((firstWeek <= 1 || firstWeek >= 52) && (dayOffset >= -3))
            {
                weekOfYear -= 1;
            }
            return firstWeekday.AddDays(weekOfYear * 7);
        }

        public static DateRange GetWeekOfYear(this DateTime target, int weekOfYear)
        {
            int year = target.Year;
            DateTime first = FirstDateOfWeek(target.Year, weekOfYear, CultureInfo.InvariantCulture);
            DateRange dateRange = new DateRange() { Between = BetweenOption.None, From = first, To = first.AddDays(7) };

            return dateRange;
        }



        public static int WeekOfYear(this DateTime now)
        {
            int day = (int)now.DayOfWeek;
            if (--day < 0)
            {
                day = 6;
            }
            int weekNumber = (now.AddDays(3 - day).DayOfYear - 1) / 7 + 1;
            return weekNumber;
        }

        public static Guid ToSpecial(this Guid guid)
        {
            DateTime now = DateTime.Now;
            string token = guid.ToString();
            string s = String.Format("{0}{1}{2}{3}{4}{5}{6}",
                                    token.Substring(0, 11),
                                    now.ToString("MM"),
                                    token.Substring(13, 3),
                                    now.ToString("dd"),
                                    token.Substring(18, 4),
                                    (int)now.DayOfWeek,
                                    token.Substring(23, 13));
            return new Guid(s);
        }

        public static bool IsSpecial(this Guid guid)
        {
            bool b = false;
            string token = guid.ToString();
            DateTime now = DateTime.Now;

            try
            {
                Guid g = new Guid(token);
                string dayOfWeek = ((int)now.DayOfWeek).ToString();
                string dayOfMonth = now.ToString("dd");
                string month = now.ToString("MM");
                string monthCandidate = token.Substring(11, 2);
                string dateCandidate = token.Substring(16, 2);
                string dayCandidate = token.Substring(22, 1);
                b = (b == false) ? b : month.Equals(monthCandidate);
                b = (b == false) ? b : dayOfMonth.Equals(dateCandidate);
                b = (b == false) ? b : dayOfWeek.Equals(dayCandidate);
            }
            catch
            {
                b = false;
            }

            return b;
        }


        public static T[] GetEnumValues<T>() where T : struct
        {
            if (typeof(T).IsEnum)
            {
                return (T[])Enum.GetValues(typeof(T));
            }
            else
            {
                throw new ArgumentException("GetEnumValues<T> accepts only Enums");
            }
        }
        /// <summary>
        /// Extension method to convert dynamic data to a DataTable. Useful for databinding.
        /// </summary>
        /// <param name="items"></param>
        /// <returns>A DataTable with the copied dynamic data.</returns>
        public static DataTable ToDataTable(this IEnumerable<dynamic> items)
        {
            var data = items.ToArray();
            if (data.Count() == 0) return null;

            var dt = new DataTable();
            foreach (var key in ((IDictionary<string, object>)data[0]).Keys)
            {
                dt.Columns.Add(key);
            }
            foreach (var d in data)
            {
                var item = (IDictionary<string, object>)d;

                dt.Rows.Add(item.ToArray());
                //dt.Rows.Add(((IDictionary<string, object>)d).Values.ToArray());
            }
            return dt;
        }

        //public static DataTable ToDataTable2(this IEnumerable<dynamic> items)
        //{
        //    var list = items.Cast<IDictionary<string, object>>().ToList();
        //    if (!list.Any()) return null;

        //    var table = new DataTable();
        //    list.First().Keys.Each(x => table.Columns.Add(x));
        //    list.Each(x => x.Values.Each(y => table.Rows.Add(y)));

        //    return table;
        //}

        public static void Empty(this System.IO.DirectoryInfo directoryInfo)
        {
            foreach (System.IO.FileInfo info in directoryInfo.GetFiles())
            {
                info.Delete();
            }
            foreach (System.IO.DirectoryInfo info in directoryInfo.GetDirectories())
            {
                info.Delete();
            }
        }

        public static bool IsSerializable(this Type type)
        {
            var result = type.GetCustomAttributes(typeof(SerializableAttribute), false);
            return (result != null && result.Length > 0);
        }

        public static string Coalesce(this string target, string defaultString)
        {
            return (!String.IsNullOrEmpty(target)) ? target : defaultString;
        }

        public static List<string> ToListOfString(this string target)
        {
            string[] t = target.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> list = new List<string>(t);
            return list;
        }

        public static DataTable Filter(this DataTable dt, string columnName, string filterValue, bool matchCase)
        {
            string filter = matchCase ? filterValue : filterValue.ToLower();
            DataTable dtr = dt.Clone();
            if (dt.Columns.Contains(columnName))
            {
                foreach (DataRow r in dt.Rows)
                {
                    string s = matchCase ? r[columnName].ToString() : r[columnName].ToString().ToLower();
                    if (s == filter)
                    {
                        DataRow row = dtr.NewRow();
                        row.ItemArray = r.ItemArray;
                        dtr.Rows.Add(row);
                    }

                }
            }
            return dtr;
        }

        public static DataTable DirectoryFilesToDataTable(this string item)
        {
            DataTable dt = new DataTable();
            if (Directory.Exists(item))
            {
                var query = from s in Directory.GetFiles(item)
                            let fi = new FileInfo(s)
                            select new
                            {
                                Filepath = fi.FullName,
                                Filename = fi.Name,
                                Extension = fi.Extension,
                                Exists = fi.Exists,
                                Directory = fi.DirectoryName
                            };

                dt = query.ToDataTable();
            }
            dt.TableName = "Files";
            return dt;
        }



        public static DataTable ToDataTable<T>(this IEnumerable<T> source)
        {
            return new ObjectShredder<T>().Shred(source, null, null);
        }

        public static DataTable ToDataTable<T>(this IEnumerable<T> source,
                                                    DataTable table, LoadOption? options)
        {
            return new ObjectShredder<T>().Shred(source, table, options);
        }


        #region helper classes

        internal class ObjectShredder<T>
        {
            private FieldInfo[] _fi;
            private PropertyInfo[] _pi;
            private Dictionary<string, int> _ordinalMap;
            private Type _type;

            public ObjectShredder()
            {
                _type = typeof(T);
                _fi = _type.GetFields();
                _pi = _type.GetProperties();
                _ordinalMap = new Dictionary<string, int>();
            }

            public DataTable Shred(IEnumerable<T> source, DataTable table, LoadOption? options)
            {
                if (typeof(T).IsPrimitive)
                {
                    return ShredPrimitive(source, table, options);
                }


                if (table == null)
                {
                    table = new DataTable(typeof(T).Name);
                }

                // now see if need to extend datatable base on the type T + build ordinal map
                table = ExtendTable(table, typeof(T));

                table.BeginLoadData();
                using (IEnumerator<T> e = source.GetEnumerator())
                {
                    while (e.MoveNext())
                    {
                        if (options != null)
                        {
                            table.LoadDataRow(ShredObject(table, e.Current), (LoadOption)options);
                        }
                        else
                        {
                            table.LoadDataRow(ShredObject(table, e.Current), true);
                        }
                    }
                }
                table.EndLoadData();
                return table;
            }

            public DataTable ShredPrimitive(IEnumerable<T> source, DataTable table, LoadOption? options)
            {
                if (table == null)
                {
                    table = new DataTable(typeof(T).Name);
                }

                if (!table.Columns.Contains("Value"))
                {
                    table.Columns.Add("Value", typeof(T));
                }

                table.BeginLoadData();
                using (IEnumerator<T> e = source.GetEnumerator())
                {
                    Object[] values = new object[table.Columns.Count];
                    while (e.MoveNext())
                    {
                        values[table.Columns["Value"].Ordinal] = e.Current;

                        if (options != null)
                        {
                            table.LoadDataRow(values, (LoadOption)options);
                        }
                        else
                        {
                            table.LoadDataRow(values, true);
                        }
                    }
                }
                table.EndLoadData();
                return table;
            }

            public DataTable ExtendTable(DataTable table, Type type)
            {
                // value is type derived from T, may need to extend table.
                foreach (FieldInfo f in type.GetFields())
                {
                    if (!_ordinalMap.ContainsKey(f.Name))
                    {
                        DataColumn dc = table.Columns.Contains(f.Name) ? table.Columns[f.Name]
                            : table.Columns.Add(f.Name, f.FieldType);
                        _ordinalMap.Add(f.Name, dc.Ordinal);
                    }
                }
                foreach (PropertyInfo p in type.GetProperties())
                {
                    if (!_ordinalMap.ContainsKey(p.Name))
                    {
                        DataColumn dc = table.Columns.Contains(p.Name) ? table.Columns[p.Name]
                            : table.Columns.Add(p.Name, p.PropertyType);
                        _ordinalMap.Add(p.Name, dc.Ordinal);
                    }
                }
                return table;
            }

            public object[] ShredObject(DataTable table, T instance)
            {

                FieldInfo[] fi = _fi;
                PropertyInfo[] pi = _pi;

                if (instance.GetType() != typeof(T))
                {
                    ExtendTable(table, instance.GetType());
                    fi = instance.GetType().GetFields();
                    pi = instance.GetType().GetProperties();
                }

                Object[] values = new object[table.Columns.Count];
                foreach (FieldInfo f in fi)
                {
                    values[_ordinalMap[f.Name]] = f.GetValue(instance);
                }

                foreach (PropertyInfo p in pi)
                {
                    values[_ordinalMap[p.Name]] = p.GetValue(instance, null);
                }
                return values;
            }

        }

        #endregion
    }
}
