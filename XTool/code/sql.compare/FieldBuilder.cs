using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public static class FieldBuilder
    {
        public static IEnumerable<Field> Build(SqlExplorer explorer)
        {
            List<Field> list = new List<Field>();
            foreach (var table in explorer.Tables)
            {
                foreach (var column in table.Columns)
                {
                    Field f = new Field(column,table.TableName,table.TableSchema) 
                    { 
                        Ordinality = explorer.Ordinality 
                    };
                    list.Add(f);
                }
            }
            
            return list;
        }

        public static IEnumerable<FieldComparison> Build(IEnumerable<SqlExplorer> explorers)
        {
            SortedDictionary<string, FieldComparison> dd = new SortedDictionary<string, FieldComparison>(StringComparer.OrdinalIgnoreCase);
            FieldComparisonCollection c = new FieldComparisonCollection(explorers);

            c.ProcessTables();


            List<FieldComparison> list = new List<FieldComparison>();
            int i = 1;
            foreach (SqlExplorer explorer in explorers)
            {
                explorer.Ordinality = i++;
                foreach (Field field in Build(explorer))
                {
                    string key = field.ComposeKey(explorer.CompareBy);

                    if (!dd.ContainsKey(key))
                    {
                        dd.Add(key, new FieldComparison(explorers.Count(),explorer.CompareBy,key,field.TableSchema,field.TableName));
                    }
                    dd[key].Add(field);

                    if (!c.Contains(key))
                    {
                        c.Add(new FieldComparison(explorers.Count(), CompareOption.Database, key, field.TableSchema, field.TableName));
                    }
                    c[key].Add(field);
                }
            }





            return list;
        }
    }
}
