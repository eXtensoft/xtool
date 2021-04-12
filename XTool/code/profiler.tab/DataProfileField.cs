using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace XTool
{
    public class DataProfileField 
    {
        //private static Dictionary<int, int> maps = DataLists.CharacterResolutions();
        #region Maps (AsciiMapCollection)

        private AsciiMapCollection _Maps;

        /// <summary>
        /// Gets or sets the AsciiMapCollection value for Maps
        /// </summary>
        /// <value> The AsciiMapCollection value.</value>

        public AsciiMapCollection Maps
        {
            get { return _Maps; }
            set
            {
                if (_Maps != value)
                {
                    _Maps = value;
                }
            }
        }

        #endregion
        
        private AsciiMapCollection maps = null;

        private static int[] foo = new int[] { 1, 2, 3 };
        public Brush TitleBrush
        {
            get
            {
                Brush brush = Brushes.Black;
                if (DistinctCount > 0 && DistinctCount <= 6)
                {
                    brush = Brushes.DarkOrange;
                }
                else if(DistinctCount < 15)
                {
                    brush = Brushes.DarkMagenta;
                }
                else if(DistinctCount < 30)
                {
                    brush = Brushes.DarkGoldenrod;
                }
                else if(DistinctCount < 100)
                {
                    brush = Brushes.DarkRed;
                }
                else if ( DistinctCount <= MaxDistinct)
                {
                    brush = Brushes.DarkGreen;
                }
                return brush;
            }
        }
        public int Index { get; set; }
        public string Name { get; set; }
        public string Datatype { get; set; }
        public int HasData { get; set; }
        public int HasNoData { get; set; }
        public int DistinctCount { get; set; }
        public double DistinctPctHasData { get; set; }
        public double DistinctPct { get; set; }
        public int MaxLength { get; set; }
        public int MaxDistinct { get; set; }
        public string Characters { get; set; }
        public DataProfileItemCollection Items { get; set; }
        public CharProfileItemCollection CharProfile { get; set; }
        public bool IsSelected { get; set; }

        public FieldTypeProfileItemCollection FieldTypes { get; set; }

        private DataRow _SchemaRow;

        private PropertyInfo _Info = null;

        public DataProfileField(PropertyInfo info)
        {
            _Info = info;
            Name = info.Name;
            Datatype = info.PropertyType.Name;
            Items = new DataProfileItemCollection();
            CharProfile = new CharProfileItemCollection();
            FieldTypes = new FieldTypeProfileItemCollection();
        }

        public DataProfileField(DataRow schemaRow)
        {
            Name = schemaRow["ColumnName"].ToString();
            Datatype = schemaRow["DataType"].ToString();
            _SchemaRow = schemaRow;
            Items = new DataProfileItemCollection();
            _Maps = maps = Application.Current.Properties[AppConstants.AsciiMaps] as AsciiMapCollection;
            CharProfile = new CharProfileItemCollection();
            FieldTypes = new FieldTypeProfileItemCollection();
        }

        public DataProfileField()
        {
            _Maps = maps = Application.Current.Properties[AppConstants.AsciiMaps] as AsciiMapCollection;
        }
        public override string ToString()
        {
            return String.Format("{0} ({1}) {2}", Name, Datatype, Characters);
        }

        public FieldFilter GetFilter()
        {
            return new FieldFilter()
            {
                ColumnName = Name,
                ColumnDatatype = Datatype,
                Values = (from x in Items
                         select new FilterValue() 
                         {
                             Key = x.Key, 
                             Count = x.Count
                         }).ToList()
            };
        }
        internal void Execute(object item)
        {
            if (_Info != null && _Info.CanRead)
            {
                object o = _Info.GetValue(item, null);
                if (o != null)
                {
                    string s = o.ToString();
                    if (s.Length > this.MaxLength)
                    {
                        MaxLength = s.Length;
                    }
                    if (String.IsNullOrEmpty(s))
                    {
                        HasNoData++;
                    }
                    else if (MaxDistinct == -1)
                    {
                        HasData++;
                    }
                    else
                    {
                        HasData++;
                        if (Items.Count <= MaxDistinct)
                        {
                            if (!Items.Contains(s))
                            {
                                Items.Add(new DataProfileItem() { Key = s });
                            }
                            Items[s].Count++;
                        }
                        else
                        {
                            Items.Clear();
                            MaxDistinct = -1;
                        }
                    }
                    
                }
            }
        }


        internal void Execute(string p)
        {
            string s = p.Trim();
            if (s.Length > this.MaxLength)
            {
                MaxLength = s.Length;
            }
            if (String.IsNullOrEmpty(s))
            {
                HasNoData++;
            }
            else
            {
                if (MaxDistinct == -1)
                {
                }
                else
                {
                    HasData++;

                    Characters = ResolveCharacters(s);
                    if (Items.Count <= MaxDistinct)
                    {
                        if (!Items.Contains(s))
                        {
                            Items.Add(new DataProfileItem() { Key = s });
                        }
                        Items[s].Count++;
                    }
                    else
                    {
                        Items.Clear();
                        MaxDistinct = -1;
                    }
                    CharProfile.Profile(s);                    
                }
                string sum = CharProfile.Summary;
                FieldTypes.Profile(s,Characters);
            }

            


        }



        public string ResolveCharacters(string s)
        {

            if (String.IsNullOrEmpty(s))
            {
                return CharTypes.None.ToString();
            }
            else
            {
                List<int> list = new List<int>();
                char[] arr = s.ToCharArray();
                foreach (var c in arr)
                {
                    int j = Convert.ToInt32(c);
                    if (maps.Contains(j) && !list.Contains(maps[j].ResolvesTo)) // and some sort of mapping contains j
                    {
                        list.Add(maps[j].ResolvesTo); // add the mapping resolutions
                    }
                }
                int i = 0;
                foreach (int k in list)
                {
                    i += k;
                }
                CharClassOption ctp = (CharClassOption)Enum.Parse(typeof(CharClassOption), i.ToString(), true);
                return ctp.ToString();
            }
        }



        internal void Calculate(int RecordCount)
        {
            List<DataProfileItem> temp = Items.OrderByDescending(x => x.Count).ToList();
            Items.Clear();
            Items = new DataProfileItemCollection();
            double cumulative = 0.00;
            double max = (double)RecordCount;
            foreach (var item in temp)
            {
                item.Percent = (double)item.Count / max;
                cumulative += item.Percent;
                item.CumulativePercent = cumulative;
                Items.Add(item);
            }
            if (Items != null && Items.Count > 0)
            {
                DistinctCount = Items.Count;
                double distinctcount = (double)Items.Count;
                DistinctPct = distinctcount / max;
                double hasdata = (double)HasData;
                DistinctPctHasData = distinctcount / hasdata;
            }
        }

    }
}
