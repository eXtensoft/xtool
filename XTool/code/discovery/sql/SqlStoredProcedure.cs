using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace XTool.Discovery
{
    [Serializable]
    public class SqlStoredProcedure
    {
        private string _tooltip = "None";

        private static string[] verbs = {
                                            ModelActionOption.Delete.ToString().ToLower(),
                                            ModelActionOption.Get.ToString().ToLower(),
                                            ModelActionOption.Post.ToString().ToLower(),
                                            ModelActionOption.Put.ToString().ToLower()
                                        };

        public string Schema { get; set; }

        public string Name { get; set; }

        [XmlIgnore]
        public bool IsChecked { get; set; }

        public string Model { get; set; }

        public string ModelAction { get; set; }

        public string Modifier { get; set; }

        public bool IsModelAction { get; set; }

        [XmlElement("Parameter")]
        public List<SqlParameter> Parameters { get; set; }

        public SqlStoredProcedure() { }

        public SqlStoredProcedure(List<SqlParameter> list)
        {
            
            Parameters = list;            
            if (list.Count > 0)
            {
                Schema = list[0].Schema;
                Name = list[0].StoredProcedureName;
                list.RemoveAt(0);
                //if (list.Count==1 && list[0].OrdinalPosition.Equals(0))
                //{
                //    Parameters.Clear();
                //}
            }
            ParseModelAction();
        }

        private void ParseModelAction()
        {
            string[] t = Name.Split('_');
            if (t.Length >= 2)
            {
                string verb = t[1].ToLower();
                if (verbs.Contains(verb))
                {
                    bool b = false;
                    char[] array = t[0].ToCharArray();
                    StringBuilder sb = new StringBuilder();
                    foreach (char c in array)
                    {
                        if (b)
                        {
                            sb.Append(c.ToString());
                        }
                        if (!b && Char.IsUpper(c))
                        {
                            sb.Append(c.ToString());
                            b = true;
                        }
                    }
                    string s = sb.ToString();
                    Model = sb.ToString();
                    ModelAction = ((ModelActionOption)Enum.Parse(typeof(ModelActionOption), verb, true)).ToString();
                    
                    if (t.Length == 3)
                    {
                        Modifier = t[2];
                        _tooltip = String.Format("{0}.{1} <{2}>", Model, ModelAction, Modifier);
                    }
                    else
                    {
                        _tooltip = String.Format("{0}.{1}", Model, ModelAction);
                    }
                }
            }
        }

        public override string ToString()
        {
            return ToDisplay(1);
        }

        public string ToDisplay(int schemaSize)
        {
            return String.Format("[{0}].[{1}]", Schema, Name);
        }

        public string ToTooltip()
        {
            return _tooltip;
        }

    }
}

