using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace XTool
{
    public class MappedTemplateSelector : DataTemplateSelector
    {
        private static readonly Dictionary<Type, Dictionary<string, string>> _Maps = new Dictionary<Type, Dictionary<string, string>>()
        {
            {typeof(ConnectionInfoViewModel),new Dictionary<string, string>()
            {
                {"image/jpeg","Snippets.ContentItemImageView"},
                {"image/x-portable-bitmap","Snippets.ContentItemImageView"},
                {"SqlServer","SqlServerShell"},
                {"MongoDb","MongoDbShell"},
                {"Redis","RedisShell"},
                {"MySql","MySqlShell"},
                {"MariaDb","MariaDbShell"},
                {"None","NoneShell"},
                {"File","FileShell"},
                {"Neo4j","Neo4jShell"},
                {"Xml","XmlShell"},
                {"Excel","ExcelShell"},
                {"Json","JsonShell"},
                {"Api","ApiShell" },
            }},
            {typeof(SelectionViewModel),new Dictionary<string, string>()
            {
                {"api.request","LogApiRequestDataTemplate" },
                {"api.error","LogErrorDataTemplate" },
                {"api.session","LogSessionDataTemplate" },
            }},

        };

        

        private static string GetTemplate(object item)
        {
            string s = String.Empty;
            string key = String.Empty;
            Type t = item.GetType();
            if (t.BaseType.Equals(typeof(ConnectionInfoViewModel)))
            {
                t = t.BaseType;
            }
            if (t == typeof(ConnectionInfoViewModel))
            {
                var vm = item as ConnectionInfoViewModel;
                if (_Maps.ContainsKey(t))
                {
                    key = vm.ConnectionType.ToString();                    

                }
            }
            else if(t == typeof(SelectionViewModel))
            {
                var vm = item as SelectionViewModel;
                if (_Maps.ContainsKey(t))
                {
                    key = vm.Tag;
                }                
            }
            if (!String.IsNullOrWhiteSpace(key) && _Maps[t].ContainsKey(key))
            {
                s = _Maps[t][key];
            }

            return s;
        }


        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            DataTemplate template = null;
            if (item != null)
            {
                string s = GetTemplate(item);
                if (!String.IsNullOrWhiteSpace(s))
                {
                    DataTemplate candidate = Application.Current.Resources[s] as DataTemplate;
                    if (candidate != null)
                    {
                        template = candidate;
                    }
                }

                else
                {
                    
                    string t = item.GetType().Name.Replace("ViewModel", "DataTemplate");
                    DataTemplate candidate = Application.Current.Resources[t] as DataTemplate;
                    if (candidate != null)
                    {
                        template = candidate;
                    }
                }
            }
            return template;
        }

    }
}
