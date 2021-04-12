using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
     public abstract class JTokenViewModel : INotifyPropertyChanged
    {

        private static Dictionary<string, string> maps = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            {"key","Key"},
            {"title","Title"},
            {"name","Name"},
            {"display","Display"},
            {"valuedisplay","ValueDisplay" }
        };
        private static List<string> mapKeys = new List<string>() {"key","title","name","value" };
        
        #region Name (string)

        /// <summary>
        /// Gets or sets the string value for Name
        /// </summary>
        /// <value> The string value.</value>
        private string _Name;
        public string Name
        {
            get
            {

                string s = String.Empty;
                if (!String.IsNullOrWhiteSpace(_Name))
                {
                    //if (_Name.EndsWith("]"))
                    if(_Name.Contains("]"))
                    {
                        if (Items != null)
                        {
                            bool b = false;
                            StringBuilder sb = new StringBuilder();
                            string kvpValue = String.Empty;
                            foreach (var item in Items)
                            {
                                //
                                //if(MapsContain(item.Name))
                                if (maps.ContainsKey(item.Name))
                                {
                                    sb.Append(item.ToString());
                                    b = true;
                                }
                                if (item.Name.Equals("value",StringComparison.OrdinalIgnoreCase))
                                {
                                    kvpValue = item.ToString();
                                }
                            }
                            if (b && !String.IsNullOrWhiteSpace(kvpValue))
                            {
                                sb.Append(String.Format(": {0}", kvpValue));
                            }
                            s = sb.ToString();
                        }
                        if (String.IsNullOrWhiteSpace(s))
                        {
                            if (_Name.EndsWith("]"))
                            {
                                s = _Name.Substring(_Name.LastIndexOf('['));
                            }
                            else
                            {
                                s = _Name.Substring(_Name.LastIndexOf("].")+2);
                            }
                            
                        }
                        
                    }
                }
                s = !String.IsNullOrWhiteSpace(s) ? s : _Name;
                return s;
            }
            set
            {
                if (_Name != value)
                {
                    _Name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        public ObservableCollection<JTokenViewModel> Items { get; set; }



        private static bool MapsContain(string input)
        {
            bool b = false;
            if (!String.IsNullOrWhiteSpace(input))
            {
                for (int i = 0;!b && i < mapKeys.Count; i++)
                {
                    string key = mapKeys[i];
                    if (key.Length <= input.Length && input.ToLower().Contains(key))
                    {
                        b = true;
                    }
                }
            }
            return b;
        }

    }
}
