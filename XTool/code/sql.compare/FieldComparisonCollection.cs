using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public class FieldComparisonCollection : KeyedCollection<string,FieldComparison>
    {
        private List<SqlExplorer> _Explorers;

        private DataTable dtTables;
        private DataTable dtScripts;

        private Dictionary<string, FieldComparison> _scripts;

        private List<string> _Keys = new List<string>();
        private List<string> _ScriptKeys = new List<string>();

        protected override string GetKeyForItem(FieldComparison item)
        {
            return item.ComposeKey();
        }

        public FieldComparisonCollection(IEnumerable<SqlExplorer> explorers)
        {
            _Explorers = explorers.ToList();
        }

        public DataTable CompareTables(CompareOption compareBy = CompareOption.None)
        {
            dtTables = null;
            ProcessTables(compareBy);
            return dtTables;
        }

        public DataTable CompareStoredProcedures(CompareOption compareBy = CompareOption.None)
        {
            dtScripts = null;
            ProcessScripts(compareBy);
            return dtScripts;
        }

        public void ProcessScripts(CompareOption compareBy = CompareOption.None)
        {
            int max = _Explorers.Count();
            _scripts = new Dictionary<string, FieldComparison>();
            CompareOption comparison = ResolveComparison(_Explorers, compareBy);
            dtScripts = new DataTable() { TableName = "ScriptComparison" };

            DataColumn dc1 = new DataColumn("Schema", typeof(String));
            DataColumn dc2 = new DataColumn("Name", typeof(String));
            ////DataColumn dc3 = new DataColumn("Field", typeof(String));
            DataColumn dc4 = new DataColumn("Datatype", typeof(bool));

            dtScripts.Columns.Add(dc1);
            dtScripts.Columns.Add(dc2);
            //dtTables.Columns.Add(dc3);
            //dtTables.Columns.Add(dc4);

            int i = 1;
            Dictionary<int, string> d = new Dictionary<int, string>();
            HashSet<string> hs = new HashSet<string>();

            Dictionary<string, int> monikers = new Dictionary<string, int>();
            foreach (SqlExplorer explorer in _Explorers)
            {
                if (!monikers.ContainsKey(explorer.Moniker))
                {
                    monikers.Add(explorer.Moniker, 0);
                }
                monikers[explorer.Moniker]++;
            }


            foreach (SqlExplorer explorer in _Explorers)
            {

                string moniker = String.Empty;
                if (monikers[explorer.Moniker] == 1)
                {
                    moniker = explorer.Moniker;
                }
                else
                {
                    moniker = String.Format("{0} ({1})", explorer.Moniker, explorer.AsOf.ToShortDateString());
                }

                if (!hs.Add(moniker))
                {
                    moniker += " " + Guid.NewGuid().ToString().Substring(0, 4);
                }

                DataColumn dc = new DataColumn(moniker.Replace('.', ' '), typeof(String));
                dtScripts.Columns.Add(dc);

                explorer.Ordinality = i++;
                foreach (Field field in BuildStoredProcedures(explorer))
                {
                    string key = field.ComposeKey(comparison).ToLower();

                    if (!_scripts.ContainsKey(key))
                    {
                        _ScriptKeys.Add(key);
                        var c = new FieldComparison(max, CompareOption.Database, key, field.TableSchema, field.TableName);
                        _scripts.Add(c.ComposeKey(),c);
                    }
                    _scripts[key].Add(field);
                }
            }
            _ScriptKeys.Sort((x, y) => x.CompareTo(y));
            foreach (string key in _ScriptKeys)
            {
                if (_scripts.ContainsKey(key))
                {
                    var found = _scripts[key];
                    DataRow r = dtScripts.NewRow();
                    string display = String.Empty;
                    bool b = true;

                    for (int j = 1; j <= max; j++)
                    {
                        var f = found.Items.Find(x => x.Ordinality.Equals(j));
                        if (f != null)
                        {
                            if (String.IsNullOrEmpty(display))
                            {
                                display = f.Display;
                            }
                            if (b && !display.Equals(f.Display))
                            {
                                b = false;
                            }

                            r[j + 1] = f.Display;
                        }

                    }
                    string[] t = key.Split('.');

                    r[0] = found.TableSchema;
                    

                    dtScripts.Rows.Add(r);
                }
            }


        }

        public void ProcessTables(CompareOption compareBy = CompareOption.None)
        {
            int max = _Explorers.Count();

            CompareOption comparison = ResolveComparison(_Explorers, compareBy);
            dtTables = new DataTable() { TableName = "TableComparison" };

            DataColumn dc1 = new DataColumn("Schema", typeof(String));
            DataColumn dc2 = new DataColumn("Table", typeof(String));
            DataColumn dc3 = new DataColumn("Field", typeof(String));
            DataColumn dc4 = new DataColumn("Datatype", typeof(bool));

            dtTables.Columns.Add(dc1);
            dtTables.Columns.Add(dc2);
            dtTables.Columns.Add(dc3);
            dtTables.Columns.Add(dc4);



            int i = 1;

            Dictionary<string, int> monikers = new Dictionary<string, int>();
            foreach (SqlExplorer explorer in _Explorers)
            {
                if (!monikers.ContainsKey(explorer.Moniker))
                {
                    monikers.Add(explorer.Moniker, 0);
                }
                monikers[explorer.Moniker]++;
            }


            Dictionary<int, string> d = new Dictionary<int, string>();
            HashSet<string> hs = new HashSet<string>();


            foreach (SqlExplorer explorer in _Explorers)
            {
                
                string moniker = String.Empty;
                if (monikers[explorer.Moniker] == 1)
                {
                    moniker = explorer.Moniker;
                }
                else
                {
                    moniker = String.Format("{0} ({1})", explorer.Moniker, explorer.AsOf.ToShortDateString());
                }

                if (!hs.Add(moniker))
                {
                    moniker += " " + Guid.NewGuid().ToString().Substring(0, 4);
                }
                
                DataColumn dc = new DataColumn(moniker.Replace('.',' '), typeof(String));
                dtTables.Columns.Add(dc);

                explorer.Ordinality = i++;
                foreach (Field field in BuildTables(explorer))
                {
                    string key = field.ComposeKey(comparison).ToLower();

                    if (!this.Contains(key))
                    {
                        _Keys.Add(key);
                        this.Add(new FieldComparison(max, CompareOption.Database, key,field.TableSchema,field.TableName));
                    }
                    this[key].Add(field);
                }             
            }
            _Keys.Sort((x, y) => x.CompareTo(y));
            foreach (string key in _Keys)
            {
                if (this.Contains(key))
                {
                    var found = this[key];
                    DataRow r = dtTables.NewRow();
                    string display = String.Empty;
                    bool b = true;

                    for (int j = 1; j <= max ; j++)
                    {
                        var f = found.Items.Find(x => x.Ordinality.Equals(j));
                        if (f != null)
                        {
                            if (String.IsNullOrEmpty(display))
                            {
                                display = f.Display;
                            }
                            if (b && !display.Equals(f.Display))
                            {
                                b = false;
                            }

                            r[j+3] = f.Display;
                        }

                    }
                    string[] t = key.Split('.');

                    r[0] = found.TableSchema;
                    r[1] = found.TableName;
                    r[2] = found.FieldName;
                    r[3] = b;


                    dtTables.Rows.Add(r);
                }
            }
            
        }

        private CompareOption ResolveComparison(List<SqlExplorer> _Explorers, CompareOption compareBy)
        {
            CompareOption option = CompareOption.None;
            if (compareBy != CompareOption.None)
            {
                option = compareBy;
            }
            else
            {
                for (int i = 0; i < _Explorers.Count; i++)
                {
                    CompareOption compare = _Explorers[i].CompareBy;
                    if (option == CompareOption.None)
                    {
                        option = compare;
                    }
                    else if (compare != CompareOption.None && option != CompareOption.None)
                    {
                        if (compare < option)
                        {
                            option = compare;
                        }
                    }
                }
            }
            return option;
        }

        private static IEnumerable<Field> BuildStoredProcedures(SqlExplorer explorer)
        {
            List<Field> list = new List<Field>();

            foreach (XTool.Discovery.SqlScript item in explorer.SqlScripts)
            {
                Field f = new Field() { Ordinality = explorer.Ordinality, TableSchema = item.Schema, ScriptText = item.Text, Name = item.Name };
                list.Add(f);
            }

            return list;
        }


        private static IEnumerable<Field> BuildTables(SqlExplorer explorer)
        {
            List<Field> list = new List<Field>();
            foreach (var table in explorer.Tables)
            {
                foreach (var column in table.Columns)
                {
                    Field f = new Field(column, table.TableName, table.TableSchema)
                    {
                        Ordinality = explorer.Ordinality
                    };
                    list.Add(f);
                }
            }

            return list;
        }


    }
}
