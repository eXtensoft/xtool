using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Text;
using System.Threading.Tasks;
using XF.Common.Db;

namespace XTool
{
    public class ScriptGenerator
    {
        #region local fields

        private static IList<ModelActionOption> modelactions = new List<ModelActionOption>
        {
            {ModelActionOption.Post},
            {ModelActionOption.Put},
            {ModelActionOption.Delete},
            {ModelActionOption.Get},
            {ModelActionOption.GetAll},
            {ModelActionOption.GetAllProjections},
        };

        private static List<Tuple<string, string, string>> csharpmaps = new List<Tuple<string, string, string>>
        {
            {new Tuple<string, string, string>("RN_VALUE", "int","GetInt32")},
            {new Tuple<string, string, string>("int", "int","GetInt32")},
            {new Tuple<string, string, string>("bigint", "long","")},
            {new Tuple<string, string, string>("bit", "bool","GetBoolean")},
            {new Tuple<string, string, string>("char", "string","GetString")},
            {new Tuple<string, string, string>("date", "DateTime","GetDateTime")},
            {new Tuple<string, string, string>("datetime", "DateTime","GetDateTime")},
            {new Tuple<string, string, string>("datetime2", "DateTime","GetDateTime")},
            {new Tuple<string, string, string>("datetimeoffset","DateTimeOffset","GetDateTimeOffset")},
            {new Tuple<string, string, string>("decimal", "decimal","GetDecimal")},
            {new Tuple<string, string, string>("float", "double","")},
            {new Tuple<string, string, string>("money", "decimal","GetDecimal")},
            {new Tuple<string, string, string>("nchar", "string","GetString")},
            {new Tuple<string, string, string>("ntext", "string","GetString")},
            {new Tuple<string, string, string>("numeric", "decimal","GetDecimal")},
            {new Tuple<string, string, string>("nvarchar", "string","GetString")},
            {new Tuple<string, string, string>("nvarchar(max)", "string","GetString")},
            {new Tuple<string, string, string>("smalldatetime", "DateTime","GetDateTime")},
            {new Tuple<string, string, string>("smallint", "short","GetInt16")},
            {new Tuple<string, string, string>("smallmoney", "decimal","GetDecimal")},
            {new Tuple<string, string, string>("text", "string","GetString")},
            {new Tuple<string, string, string>("tinyint", "short","GetByte")},
            {new Tuple<string, string, string>("uniqueidentifier", "Guid","GetGuid")},
            {new Tuple<string, string, string>("varchar", "string","GetString")},
            {new Tuple<string, string, string>("varchar(max)", "string","GetString")},
            {new Tuple<string, string, string>("xml", "XmlDocument","GetSqlXml")},
        };
        private SqlTableViewModel vm;

        #endregion

        #region properties

        public string Company { get; set; }

        public string CompanySchema { get; set; }

        public bool HasCompanySchema
        {
            get { return !String.IsNullOrWhiteSpace(CompanySchema); }
        }

        public string SprocPrefix { get; set; }

        #endregion

        #region constructors

        public ScriptGenerator(SqlTableViewModel viewModel)
        {
            vm = viewModel;
        }

        #endregion constructors

        #region class api

        public string GenerateModel()
        {
            int level = 0;
            StringBuilder sb = new StringBuilder();

            BuildHeader(sb, level,
                delegate
                {
                    BuildNamespace(sb, level,
                        delegate
                        {
                            string[] usings = new string[] { "", "" };
                            BuildUsings(sb, ++level,
                                delegate
                                {
                                    BuildModelClass(sb, level);
                                },
                                null
                                );
                        });
                },
                vm.ToModelName
            );

            return sb.ToString();
        }
       
        public string GenerateMDG()
        {
            return String.Empty;
        }

        public string GenerateConfig(bool isInline)
        {
            string modelName = String.Format("{0}.{1}", vm.Namespace, vm.ToModelName);
            DbConfig config = new DbConfig() { AppContextKey = vm.AppContext };
            Model m = new Model() { Key = modelName, 
                modelType = modelName, 
                Commands = new List<DbCommand>(), 
                DataMaps = new List<XF.Common.Db.DataMap>(), 
                ModelActions = new List<ModelAction>() };

            foreach (var item in modelactions)
            {
                if (vm.ModelActions.HasFlag(item))
                {
                    GenerateConfigModelActions(config, m, item);
                    GenerateConfigCommands(config, m, item,isInline);
                }
            }
            
            config.Models.Add(m);

            

            return GenericSerializer.XmlDocFromGenericItem<DbConfig>(config,true).OuterXml;
        }

        private void GenerateConfigModelActions(DbConfig config, Model m, XTool.ModelActionOption option)
        {
            XF.Common.ModelActionOption opt;
            if (Enum.TryParse<XF.Common.ModelActionOption>(option.ToString(), out opt))
            {
                m.ModelActions.Add(new ModelAction() { Verb = opt, DbCommandKey = opt.ToString().ToLower() });
            }
        }
        
        private void GenerateConfigCommands(DbConfig config, Model m, XTool.ModelActionOption option, bool isInline)
        {
            bool b = false;
            int i = 0;
            StringBuilder cmd = new StringBuilder();
            StringBuilder paramSql = new StringBuilder();
            StringBuilder selectSql = new StringBuilder();
            DbCommand command = new DbCommand() {};

            switch (option)
            {
                case ModelActionOption.None:
                    break;
                case ModelActionOption.Post:
                    command.Key = "post";
                    if (!isInline)
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure.ToString();
                        cmd.AppendFormat("{0}_{1}", vm.TableName, option.ToString());                      
                    }
                    else
                    {
                        command.CommandType = System.Data.CommandType.Text.ToString();
                        command.Parameters = new List<Parameter>();
                        cmd.AppendFormat("insert into [{0}].[{1}] ( ", vm.TableSchema, vm.TableName);
                        selectSql.AppendFormat("select ");  

                        foreach (var item in vm.Columns.Where(x=>x.IsSelected && !x.IsIdentity && !x.IsPrimaryKey && !x.IsComputed))
                        {
                            if (i++ > 0)
                            {
                                cmd.Append(",");
                                paramSql.Append(",");
                            }
                            cmd.AppendFormat("[{0}]",item.ColumnName);
                            string name = String.Format("@{0}", item.ColumnName.ToLower());
                            paramSql.Append(name);
                            command.Parameters.Add(new Parameter() { Mode = "in", DataType = item.DbType, Name = name, Target = item.ColumnName, AllowsNull = item.IsNullible });
                        }
                        cmd.Append(") values (");
                        cmd.AppendFormat("{0}) ", paramSql.ToString());
                        i = 0;
                        foreach (var item in vm.Columns.Where(x=>x.IsSelected))
                        {
                            if (i++ > 0)
                            {
                                selectSql.Append(",");
                            }
                            selectSql.AppendFormat("[{0}]", item.ColumnName);                        
                        }
                        var found = vm.Columns.FirstOrDefault(x=>x.IsPrimaryKey == true);
                        if (found != null)
                        {
                            selectSql.AppendFormat(" from [{0}].[{1}] where [{2}] = @scope_identity()", vm.TableSchema, vm.TableSchema, found.ColumnName);
                            b = true;
                        }
                        if (b)
                        {
                            cmd.Append(selectSql.ToString());
                        }

                    }

                    command.CommandText = cmd.ToString();
                    m.Commands.Add(command);                    
                    //
                    break;
                case ModelActionOption.Put:
                    command.Key = "put";
                    if (!isInline)
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure.ToString();
                        cmd.AppendFormat("{0}_{1}", vm.TableName, option.ToString());                      
                    }
                    else
                    {
                        command.CommandType = System.Data.CommandType.Text.ToString();
                        command.Parameters = new List<Parameter>();
                        cmd.AppendFormat("update [{0}].[{1}] set ", vm.TableSchema, vm.TableName);
                        selectSql.AppendFormat("select ");
                        foreach (var item in vm.Columns.Where(x => x.IsSelected && !x.IsIdentity && !x.IsPrimaryKey && !x.IsComputed))
                        {
                            if (i++ > 0)
                            {
                                cmd.Append(",");
                            }
                            string name = String.Format("@{0}", item.ColumnName.ToLower());
                            cmd.AppendFormat("[{0}] = [{1}]", item.ColumnName,name);
                            command.Parameters.Add(new Parameter() { Mode = "in", DataType = item.DbType, Name = name, Target = item.ColumnName, AllowsNull = item.IsNullible });
                        }
                        i = 0;
                        foreach (var item in vm.Columns.Where(x => x.IsSelected))
                        {
                            if (i++ > 0)
                            {
                                selectSql.Append(",");
                            }
                            selectSql.AppendFormat("[{0}]", item.ColumnName);
                        }

                        var found = vm.Columns.FirstOrDefault(x => x.IsPrimaryKey == true);
                        if (found != null)
                        {
                            cmd.AppendFormat(" where [{0}].[{1}] = @{2} ",vm.TableSchema,vm.TableName,found.ColumnName.ToLower());
                            selectSql.AppendFormat(" from [{0}].[{1}] where [{2}] = @{3}", vm.TableSchema, vm.TableSchema, found.ColumnName, found.ColumnName.ToLower());
                            b = true;
                        }
                        if (b)
                        {
                            cmd.Append(selectSql.ToString());
                        }
                        command.CommandText = cmd.ToString();
                        m.Commands.Add(command);

                    }
                    break;
                case ModelActionOption.Delete:
                    command.Key = "put";
                    if (!isInline)
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure.ToString();
                        cmd.AppendFormat("{0}_{1}", vm.TableName, option.ToString());
                    }
                    else
                    {
                        var found = vm.Columns.FirstOrDefault(x => x.IsPrimaryKey == true);
                        if (found != null)
                        {
                            command.CommandType = System.Data.CommandType.Text.ToString();
                            command.Parameters = new List<Parameter>();                                               
                            string name = String.Format("@{0}", found.ColumnName.ToLower());
                            cmd.AppendFormat("delete from [{0}].[{1}] where [{2}] = {3}",vm.TableSchema,vm.TableName,found.ColumnName, name);
                            command.Parameters.Add(new Parameter() { Mode = "in", DataType = found.DbType, Name = name, Target = found.ColumnName, AllowsNull = found.IsNullible });                            
                        }

                    }
                    command.CommandText = cmd.ToString();
                    m.Commands.Add(command);

                    break;
                case ModelActionOption.Get:
                    command.Key = "get";
                    if (!isInline)
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure.ToString();
                        cmd.AppendFormat("{0}_{1}", vm.TableName, option.ToString());
                    }
                    else
                    {
                        command.CommandType = System.Data.CommandType.Text.ToString();
                        command.Parameters = new List<Parameter>();
                        cmd.AppendFormat("select ");
                        i = 0;
                        foreach (var item in vm.Columns.Where(x => x.IsSelected))
                        {
                            if (i++ > 0)
                            {
                                cmd.Append(",");
                            }
                            cmd.AppendFormat("[{0}]", item.ColumnName);
                        }

                        var found = vm.Columns.FirstOrDefault(x => x.IsPrimaryKey == true);
                        if (found != null)
                        {
                            string name = String.Format("@{0}", found.ColumnName.ToLower());
                            command.CommandText = String.Format("{4} from [{0}].[{1}] where [{2}] = {3}", vm.TableSchema, vm.TableSchema, found.ColumnName, name,cmd.ToString());
                            command.Parameters.Add(new Parameter() { Mode = "in", DataType = found.DbType, Name = name, Target = found.ColumnName, AllowsNull = found.IsNullible });
                            m.Commands.Add(GenericSerializer.Clone<DbCommand>(command));

                        }
                        var fkFound = vm.Columns.FirstOrDefault(x => x.IsForeignKey == true);
                        if (fkFound != null)
                        {                            
                            string name = String.Format("@{0}", fkFound.ColumnName.ToLower());
                            command.Key = String.Format("get-by-{0}",name);
                            command.CommandText = String.Format("{4} from [{0}].[{1}] where [{2}] = {3}", vm.TableSchema, vm.TableSchema, fkFound.ColumnName, name,cmd.ToString());
                            command.Parameters.Clear();
                            command.Parameters.Add(new Parameter() { Mode = "in", DataType = fkFound.DbType, Name = name, Target = fkFound.ColumnName, AllowsNull = found.IsNullible });
                            m.Commands.Add(GenericSerializer.Clone<DbCommand>(command));                            
                        }
                    }


                    break;
                case ModelActionOption.GetAll:
                    command.Key = "get-all";
                    if (!isInline)
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure.ToString();
                        cmd.AppendFormat("{0}_{1}", vm.TableName, option.ToString());
                    }
                    else
                    {
                        command.CommandType = System.Data.CommandType.Text.ToString();
                        command.Parameters = new List<Parameter>();
                        cmd.AppendFormat("select ");
                        i = 0;
                        foreach (var item in vm.Columns.Where(x => x.IsSelected))
                        {
                            if (i++ > 0)
                            {
                                cmd.Append(",");
                            }
                            cmd.AppendFormat("[{0}]", item.ColumnName);
                        }

                        command.CommandText = cmd.ToString();
                        m.Commands.Add(GenericSerializer.Clone<DbCommand>(command));

                        var found = vm.Columns.FirstOrDefault(x => x.IsForeignKey == true);
                        if (found != null)
                        {
                            string name = String.Format("@{0}", found.ColumnName.ToLower());
                            command.Key = "get-all-by-" + name;
                            cmd.AppendFormat(" from [{0}].[{1}] where [{2}] = {3}", vm.TableSchema, vm.TableSchema, found.ColumnName, name);
                            command.Parameters.Add(new Parameter() { Mode = "in", DataType = found.DbType, Name = name, Target = found.ColumnName, AllowsNull = found.IsNullible });

                            command.CommandText = cmd.ToString();
                            m.Commands.Add(command);
                        }
                    }

                    break;
                case ModelActionOption.GetAllProjections:
                    break;
                case ModelActionOption.ExecuteAction:
                    break;
                default:
                    break;
            }
        }

        public string GenerateApiInterface()
        {
            int level = 0;
            StringBuilder sb = new StringBuilder();
            BuildHeader(sb, level,
                delegate
                {
                    BuildNamespace(sb, level,
                        delegate
                        {
                            string[] usingsStatements = new string[] { "System.Data" };
                            BuildUsings(sb, ++level,
                                delegate
                                {
                                    BuildDataServiceInterface(sb, level);
                                },
                                usingsStatements);
                        });
                }
                , vm.ToModelName
                );

            return sb.ToString();
        }

        public string GenerateApi()
        {
            int level = 0;
            StringBuilder sb = new StringBuilder();
            BuildHeader(sb, level,
                delegate
                {
                    BuildNamespace(sb, level,
                        delegate
                        {
                            string[] usingsStatements = new string[] { "System.Data", "Newtonsoft.Json",
                                "System.Net.Http" };
                            BuildUsings(sb, ++level,
                                delegate
                                {
                                    BuildDataService(sb, level);
                                },
                                usingsStatements);
                        });
                }
                ,vm.ToModelName
                );

            return sb.ToString();
        }

        public string GenerateDataProviderInterface()
        {
            int level = 0;
            StringBuilder sb = new StringBuilder();
            BuildHeader(sb, level,
                delegate
                {
                    BuildNamespace(sb, level,
                        delegate
                        {
                            string[] usingsStatements = new string[] { "System.Data" };
                            BuildUsings(sb, ++level,
                                delegate
                                {
                                    BuildDataProviderInterface(sb, level);
                                },
                                usingsStatements);
                        });
                }
                , vm.ToModelName
                );

            return sb.ToString();
        }

        public string GenerateSqlServer()
        {
            int level = 0;
            StringBuilder sb = new StringBuilder();
            BuildHeader(sb, level,
                delegate
                {
                    BuildNamespace(sb, level,
                        delegate
                        {
                            string[] usingsStatements = new string[] { "System.Data", "System.Data.SqlClient", "Microsoft.Extensions.Logging" };
                            BuildUsings(sb, ++level,
                                delegate
                                {
                                    BuildSqlServerDataProvider(sb, level);
                                },
                                usingsStatements);
                        });
                }, vm.ToModelName);

            return sb.ToString();
        }
        

        
        public string GenerateMDGInline()
        {
            int level = 0;
            StringBuilder sb = new StringBuilder();
            BuildHeader(sb, level,
                delegate
                {
                    BuildNamespace(sb, level,
                        delegate
                        {
                            string[] usingsStatements = new string[] { "System.Data", "System.Data.SqlClient", "XF.Common", "XF.DataServices" };
                            BuildUsings(sb, ++level,
                                delegate
                                {
                                    BuildMDGInlineClass(sb, level);
                                },
                                usingsStatements
                                );
                        });
                },vm.ToModelName
                );
            return sb.ToString();
        }
        
        public string GenerateMDGSproc()
        {
            int level = 0;
            StringBuilder sb = new StringBuilder();

            BuildHeader(sb, level,
                delegate
                {
                    BuildNamespace(sb, level,
                        delegate
                        {
                            string[] usingsStatements = new string[] { "System.Data","System.Data.SqlClient","XF.Common","XF.DataServices"};
                            BuildUsings(sb, ++level,
                                delegate
                                {
                                    BuildMDGSprocClass(sb, level);
                                },
                                usingsStatements
                                );
                        });
                },
                vm.ToModelName
            );

            return sb.ToString();
        }
        
        public string GenerateSprocs()
        {
            int level = 0;
            StringBuilder sb = new StringBuilder();
            BuildSprocs(sb,level);
            return sb.ToString();
        }
        
        #endregion class api

        #region local implementation Shared

        private void BuildHeader(StringBuilder sb, int level,  Action<StringBuilder,int> nextAction,string filename)
        {
            sb.AppendLine(String.Format("// <copyright file=\"{0}.cs\" company=\"{1}\">", filename, vm.Company));
            sb.AppendLine(String.Format("// Copyright © {0} All Right Reserved", DateTime.Now.Year));
            sb.AppendLine("// </copyright>");
            sb.AppendLine();
            if (nextAction != null)
            {
                nextAction.Invoke(sb, level);
            }           
        }

        private void BuildNamespace(StringBuilder sb, int level, Action<StringBuilder, int> nextAction)
        {
            sb.AppendLine(String.Format("namespace {0}", vm.Namespace));
            sb.AppendLine("{");
            if (nextAction != null)
            {
                nextAction.Invoke(sb, level);
            }
            sb.AppendLine("}");
        }
  
        private void BuildUsings(StringBuilder sb, int level, Action<StringBuilder, int> nextAction, IEnumerable<string> includeUsings = null)
        {
            sb.AppendLine(Indent.Format(level, "using System;"));
            sb.AppendLine(Indent.Format(level, "using System.Collections.Generic;"));
            sb.AppendLine(Indent.Format(level, "using System.Linq;"));
            sb.AppendLine(Indent.Format(level, "using System.Text;"));
            sb.AppendLine(Indent.Format(level, "using System.Xml.Serialization;"));
            if (includeUsings != null)
            {
                foreach (var item in includeUsings)
                {
                    sb.AppendLine(Indent.Format(level, "using {0};", item));
                }                
            }

            if (HasCompanySchema)
            {
                sb.AppendLine(Indent.Format(level, "using System.Runtime.Serialization;"));
            }

            sb.AppendLine();
            if (nextAction != null)
            {
                nextAction.Invoke(sb, level);
            }
        }

        private void BuildBorrowReader(StringBuilder sb, int level)
        {
            sb.AppendLine(Indent.Format(level, "#region Borrower overrides"));
            sb.AppendLine();
            sb.AppendLine(Indent.Format(level, "public override void BorrowReader(SqlDataReader reader, List<{0}> list)", vm.ToModelName));
            sb.AppendLine(Indent.Append(level, "{"));
            sb.AppendLine(Indent.Append(++level, "while (reader.Read())"));
            sb.AppendLine(Indent.Append(level++, "{"));
            sb.AppendLine(Indent.Format(level, "var model = new {0}();", vm.ToModelName));
            foreach (var item in vm.Columns.Where(x => x.IsSelected))
            {
                var found = csharpmaps.Find(x => x.Item1.Equals(item.Datatype, StringComparison.OrdinalIgnoreCase));
                if (found != null)
                {
                    string methodname = found != null ? found.Item3 : "GetObject";
                    if (found.Item1.Equals("xml"))
                    {
                        sb.AppendLine(Indent.Format(level, "var xmldoc = reader.{1}(reader.GetOrdinal(\"{0}\"));", item.ColumnName, methodname));
                        sb.AppendLine(Indent.Append(level, "if (!xmldoc.IsNull)"));
                        sb.AppendLine(Indent.Append(level++, "{"));
                        sb.AppendLine(Indent.Append(level, "// model.ComplexProperty = GenericSerializer.StringToGenericItem<[typename]>(xmldoc.Value);"));
                        sb.AppendLine(Indent.Append(--level, "}"));
                    }
                    else if (!item.IsNullible)
                    {
                        sb.AppendLine(Indent.Format(level, "model.{0} = reader.{1}(reader.GetOrdinal(\"{0}\"));", item.ColumnName, methodname));
                    }
                    else
                    {
                        sb.AppendLine(Indent.Format(level, "if (!reader.IsDBNull(reader.GetOrdinal(\"{0}\")))", item.ColumnName));
                        //sb.AppendLine(Indent.Format(level, "if (reader[\"{0}\"].Equals(DBNull.Value))", item.ColumnName));
                        sb.AppendLine(Indent.Append(level++, "{"));
                        sb.AppendLine(Indent.Format(level--, "model.{0} = reader.{1}(reader.GetOrdinal(\"{0}\"));", item.ColumnName, methodname));
                        sb.AppendLine(Indent.Append(level, "}"));
                    }
                }
            }

            sb.AppendLine(Indent.Append(level--, "list.Add(model);"));
            sb.AppendLine();
            sb.AppendLine(Indent.Append(level, "}"));
            sb.AppendLine(Indent.Append(--level, "}"));
            sb.AppendLine();
            sb.AppendLine(Indent.Format(level, "#endregion Borrower overrides"));


            if (!vm.TableSchema.Equals("dbo"))
            {
                sb.AppendLine(Indent.Append(level, "#region override schema"));
                sb.AppendLine(Indent.Append(level, "public override string GetDatabaseSchema()"));
                sb.AppendLine(Indent.Append(level, "{"));
                sb.AppendLine(Indent.Format(++level, "return DbSchema;", vm.TableSchema));
                sb.AppendLine(Indent.Append(--level, "}"));
                sb.AppendLine(Indent.Append(level, "#endregion override schema"));
            }

        }

        #region DataProvider Borrower

        private void BuildDataProviderBorrowReader(StringBuilder sb, int level)
        {
            sb.AppendLine(Indent.Format(level, "#region Borrower overrides"));
            sb.AppendLine();
            sb.AppendLine(Indent.Format(level, "public void Borrow(SqlDataReader reader, List<{0}> list)", vm.ToModelName));
            sb.AppendLine(Indent.Append(level, "{"));
            sb.AppendLine(Indent.Append(++level, "while (reader.Read())"));
            sb.AppendLine(Indent.Append(level++, "{"));
            sb.AppendLine(Indent.Format(level, "var model = new {0}();", vm.ToModelName));
            foreach (var item in vm.Columns.Where(x => x.IsSelected))
            {
                var found = csharpmaps.Find(x => x.Item1.Equals(item.Datatype, StringComparison.OrdinalIgnoreCase));
                if (found != null)
                {
                    string methodname = found != null ? found.Item3 : "GetObject";
                    if (found.Item1.Equals("xml"))
                    {
                        sb.AppendLine(Indent.Format(level, "var xmldoc = reader.{1}(reader.GetOrdinal(\"{0}\"));", item.ColumnName, methodname));
                        sb.AppendLine(Indent.Append(level, "if (!xmldoc.IsNull)"));
                        sb.AppendLine(Indent.Append(level++, "{"));
                        sb.AppendLine(Indent.Append(level, "// model.ComplexProperty = GenericSerializer.StringToGenericItem<[typename]>(xmldoc.Value);"));
                        sb.AppendLine(Indent.Append(--level, "}"));
                    }
                    else if (!item.IsNullible)
                    {
                        sb.AppendLine(Indent.Format(level, "model.{0} = reader.{1}(reader.GetOrdinal(\"{0}\"));", item.ColumnName, methodname));
                    }
                    else
                    {
                        sb.AppendLine(Indent.Format(level, "if (!reader.IsDBNull(reader.GetOrdinal(\"{0}\")))", item.ColumnName));
                        //sb.AppendLine(Indent.Format(level, "if (reader[\"{0}\"].Equals(DBNull.Value))", item.ColumnName));
                        sb.AppendLine(Indent.Append(level++, "{"));
                        sb.AppendLine(Indent.Format(level--, "model.{0} = reader.{1}(reader.GetOrdinal(\"{0}\"));", item.ColumnName, methodname));
                        sb.AppendLine(Indent.Append(level, "}"));
                    }
                }
            }

            sb.AppendLine(Indent.Append(level--, "list.Add(model);"));
            sb.AppendLine();
            sb.AppendLine(Indent.Append(level, "}"));
            sb.AppendLine(Indent.Append(--level, "}"));
            sb.AppendLine();
            sb.AppendLine(Indent.Format(level, "#endregion Borrower overrides"));


            //if (!vm.TableSchema.Equals("dbo"))
            //{
            //    sb.AppendLine(Indent.Append(level, "#region override schema"));
            //    sb.AppendLine(Indent.Append(level, "public override string GetDatabaseSchema()"));
            //    sb.AppendLine(Indent.Append(level, "{"));
            //    sb.AppendLine(Indent.Format(++level, "return DbSchema;", vm.TableSchema));
            //    sb.AppendLine(Indent.Append(--level, "}"));
            //    sb.AppendLine(Indent.Append(level, "#endregion override schema"));
            //}

        }

        #endregion

        #endregion local implementation Shared

        #region local implementation Model

        private void BuildModelClass(StringBuilder sb, int level)
        {
            if (HasCompanySchema)
            {
                sb.AppendLine(Indent.Format(level, "[DataContract(Namespace = \"{0}\")]", CompanySchema));
            }
            sb.AppendLine(Indent.Format(level, "[Serializable]"));
            sb.AppendLine(Indent.Format(level, "public sealed class {0}", vm.ToModelName));
            sb.AppendLine(Indent.Append(level++, "{"));
            sb.AppendLine();
            sb.AppendLine(Indent.Format(level, "#region local fields"));
            sb.AppendLine(Indent.Format(level, "#endregion local fields"));
            sb.AppendLine();
            BuildModelClassPropertySection(sb, level);
            sb.AppendLine();
            BuildModelClassConstructorSection(sb, level);
            sb.AppendLine();
            sb.AppendLine(Indent.Append(--level, "}"));
        }

        private void BuildModelClassPropertySection(StringBuilder sb, int level)
        {
            sb.AppendLine(Indent.Format(level, "#region properties"));
            foreach (var item in vm.Columns.Where(x=>x.IsSelected))
            {
                sb.AppendLine();

                var found = csharpmaps.Find(x => x.Item1.Equals(item.Datatype, StringComparison.OrdinalIgnoreCase));
                if (found != null)
                {
                    if (HasCompanySchema)
                    {
                        sb.AppendLine(Indent.Append(level, "[DataMember]"));
                    }
                    sb.AppendLine(Indent.Format(level, "public {0} {1} {{ get; set; }}", found.Item2, item.ColumnName));
                }
            }
            sb.AppendLine();
            sb.AppendLine(Indent.Format(level, "#endregion properties"));
        }

        private void BuildModelClassConstructorSection(StringBuilder sb, int level)
        { 
            sb.AppendLine(Indent.Format(level, "#region constructors"));
            sb.AppendLine(Indent.Format(level,"public {0}() {{ }}",vm.ToModelName));
            sb.AppendLine(Indent.Format(level, "#endregion constructors"));            
        }
   
        #endregion local implementation Model

        #region local implemenation MDGSproc

        private void BuildMDGSprocClass(StringBuilder sb, int level)
        {
            sb.AppendLine(Indent.Format(level,"public class {0}MDG : SqlServerModelDataGateway<{0}>",vm.ToModelName));
            sb.AppendLine(Indent.Append(level++, "{"));
            sb.AppendLine();
            sb.AppendLine(Indent.Format(level, "#region local fields"));
            sb.AppendLine();
            foreach (var item in vm.Columns.Where(x=>x.IsSelected && !x.IsComputed))
            {
                sb.AppendLine(Indent.Format(level, "private const string {0}ParamName = \"{1}\";", item.ColumnName,item.ColumnName.ToLower()));
            }
            sb.AppendLine();

            if (!vm.TableSchema.Equals("dbo"))
            {
                sb.AppendLine(Indent.Format(level,"private const string DbSchema = \"{0}\";",vm.TableSchema));
                sb.AppendLine();
            }

            StringBuilder sbConstants = new StringBuilder();
            StringBuilder sbCommands = new StringBuilder();
            
            if (vm.ModelActions.HasFlag(ModelActionOption.Post))
            {
                string sprocname = GenerateSprocName(ModelActionOption.Post);
                sbConstants.AppendLine(Indent.Format(level, "private const string {0}Command =\"{1}\";", ModelActionOption.Post, sprocname));

                sbCommands.AppendLine(Indent.Format(level, "public override SqlComand PostSqlCommand(SqlConnection cn, {0} model, IContext context)", vm.ToModelName));
                sbCommands.AppendLine(Indent.Append(level++, "{"));
                GenerateCreateCommand(level,sbCommands, ModelActionOption.Post);

                foreach (var item in vm.Columns.Where(x=>x.IsSelected && !x.IsIdentity && !x.IsPrimaryKey && !x.IsComputed))
                {
                    sbCommands.AppendLine(Indent.Format(level, "cmd.Parameters.AddWithValue({0}ParamName, model.{0});", item.ColumnName, vm.ToModelName));
                }

                sbCommands.AppendLine(Indent.Append(level, "return cmd;"));
                sbCommands.AppendLine(Indent.Append(--level, "}"));
                sbCommands.AppendLine();
            }
            if (vm.ModelActions.HasFlag(ModelActionOption.Put))
            {
                string sprocname = GenerateSprocName(ModelActionOption.Put);
                sbConstants.AppendLine(Indent.Format(level, "private const string {0}Command =\"{1}\";", ModelActionOption.Put, sprocname));

                sbCommands.AppendLine(Indent.Format(level, "public override SqlCommand PutSqlCommand(SqlConnection cn, {0} model, IContext context)", vm.ToModelName));
                sbCommands.AppendLine(Indent.Append(level++, "{"));
                GenerateCreateCommand(level, sbCommands, ModelActionOption.Put);

                foreach (var item in vm.Columns.Where(x => x.IsSelected && !x.IsComputed))
                {
                    sbCommands.AppendLine(Indent.Format(level, "cmd.Parameters.AddWithValue({0}ParamName, model.{0});", item.ColumnName, vm.ToModelName));
                }

                sbCommands.AppendLine(Indent.Append(level, "return cmd;"));
                sbCommands.AppendLine(Indent.Append(--level, "}"));
                sbCommands.AppendLine();
            }
            if (vm.ModelActions.HasFlag(ModelActionOption.Delete))
            {
                string sprocname = GenerateSprocName(ModelActionOption.Delete);
                sbConstants.AppendLine(Indent.Format(level, "private const string {0}Command =\"{1}\";", ModelActionOption.Delete, sprocname));

                sbCommands.AppendLine(Indent.Append(level, "public override SqlCommand DeleteSqlCommand(SqlConnection cn, ICriterion criterion, IContext context)"));
                sbCommands.AppendLine(Indent.Append(level++, "{"));
                GenerateCreateCommand(level, sbCommands, ModelActionOption.Delete);

                foreach (var item in vm.Columns.Where(x => x.IsPrimaryKey))
                {
                    sbCommands.AppendLine(Indent.Format(level, "cmd.Parameters.AddWithValue({0}ParamName, model.{0});", item.ColumnName, vm.ToModelName));
                }               

                sbCommands.AppendLine(Indent.Append(level, "return cmd;"));
                sbCommands.AppendLine(Indent.Append(--level, "}"));
                sbCommands.AppendLine();
            }
            if (vm.ModelActions.HasFlag(ModelActionOption.Get))
            {
                string sprocname = GenerateSprocName(ModelActionOption.Get);
                sbConstants.AppendLine(Indent.Format(level, "private const string {0}Command =\"{1}\";", ModelActionOption.Get, sprocname));

                sbCommands.AppendLine(Indent.Append(level, "public override SqlCommand GetSqlCommand(SqlConnection cn, ICriterion criterion, IContext context)"));
                sbCommands.AppendLine(Indent.Append(level++, "{"));
                GenerateCreateCommand(level, sbCommands, ModelActionOption.Get);

                foreach (var item in vm.Columns.Where(x => x.IsPrimaryKey))
                {
                    sbCommands.AppendLine(Indent.Format(level, "cmd.Parameters.AddWithValue({0}ParamName, model.{0});", item.ColumnName, vm.ToModelName));
                }

                sbCommands.AppendLine(Indent.Append(level, "return cmd;"));
                sbCommands.AppendLine(Indent.Append(--level, "}"));
                sbCommands.AppendLine();
            }
            if (vm.ModelActions.HasFlag(ModelActionOption.GetAll))
            {
                string sprocname = GenerateSprocName(ModelActionOption.GetAll);
                sbConstants.AppendLine(Indent.Format(level, "private const string {0}Command =\"{1}\";", ModelActionOption.GetAll, sprocname));

                sbCommands.AppendLine(Indent.Append(level, "public override SqlCommand GetAllSqlCommand(SqlConnection cn, ICriterion criterion, IContext context)"));
                sbCommands.AppendLine(Indent.Append(level++, "{"));
                GenerateCreateCommand(level, sbCommands, ModelActionOption.GetAll);

                sbCommands.AppendLine(Indent.Append(level, "return cmd;"));
                sbCommands.AppendLine(Indent.Append(--level, "}"));
                sbCommands.AppendLine();
            }
            if (vm.ModelActions.HasFlag(ModelActionOption.GetAllProjections))
            {
                string sprocname = GenerateSprocName(ModelActionOption.GetAllProjections);
                sbConstants.AppendLine(Indent.Format(level, "private const string {0}Command =\"{1}\";", ModelActionOption.GetAllProjections, sprocname));

                sbCommands.AppendLine(Indent.Append(level, "public override SqlCommand GetAllProjectionsSqlCommand(SqlConnection cn, ICriterion criterion, IContext context)"));
                sbCommands.AppendLine(Indent.Append(level++, "{"));
                GenerateCreateCommand(level, sbCommands, ModelActionOption.GetAllProjections);

                sbCommands.AppendLine(Indent.Append(level, "return cmd;"));
                sbCommands.AppendLine(Indent.Append(--level, "}"));
                sbCommands.AppendLine();
            }

            // append constants
            sb.Append(sbConstants.ToString());
            sb.AppendLine();
            sb.AppendLine(Indent.Format(level, "#endregion local fields"));
            
            sb.AppendLine();

            // append methods
            sb.AppendLine(Indent.Format(level, "#region SqlCommand overrides"));
            sb.AppendLine();
            sb.Append(sbCommands.ToString());
            sb.AppendLine();
            sb.AppendLine(Indent.Format(level, "#endregion SqlCommand overrides"));
            sb.AppendLine();

            BuildBorrowReader(sb, level);

            sb.AppendLine(Indent.Append(--level, "}"));
        }

        private void GenerateCreateCommand(int level, StringBuilder sb, ModelActionOption modelActionOption)
        {
            sb.AppendLine(Indent.Append(level, "SqlCommand cmd = cn.CreateCommand();"));
            sb.AppendLine(Indent.Append(level, "cmd.CommandType = CommandType.StoredProcedure;"));
            sb.AppendLine(Indent.Format(level, "cmd.CommandText = {0}Command;",modelActionOption.ToString()));
            //sb.AppendLine(Indent.Format(level, "cmd.CommandText = {0};", GenerateSprocName(modelActionOption)));
        }


        private string GenerateSprocName(ModelActionOption modelActionOption)
        {
            string s = (String.IsNullOrWhiteSpace(SprocPrefix)) ? vm.ToModelName : String.Format("{0}{1}", SprocPrefix.Trim(),vm.ToModelName);
            return String.Format("{0}_{1}", s, modelActionOption);
        }
        
        #endregion local implementation MDGSproc

        #region local implementation Sprocs

        private void BuildSprocs(StringBuilder sb, int level)
        {
            int i = 0;
            foreach (var item in modelactions)
            {
                if (item.HasFlag(item))
                {
                    if (i++ > 0)
                    {
                        DelimitSprocs(sb, level);
                    }
                    BuildSproc(sb, level, item);
                    sb.AppendLine(Indent.Append(level, "GO"));
                }
            }
        }

        private void DelimitSprocs(StringBuilder sb, int level)
        {
                        sb.AppendLine();
                        sb.AppendLine();
                        sb.AppendLine("/*****|*****/");
                        sb.AppendLine();
                        sb.AppendLine();
        }

        private void BuildSproc(StringBuilder sb, int level, ModelActionOption modelActionOption)
        {
            string sprocname = GenerateSprocName(modelActionOption);

            BuildSprocHeader(sb, level, modelActionOption, delegate
            {
                BuildParamsDeclaration(sb, level,modelActionOption, delegate
                {
                    BuildSprocBody(sb, level, modelActionOption);
                });
            }, sprocname);
        }

        private void BuildSprocHeader(StringBuilder sb, int level, ModelActionOption modelActionOption, Action<StringBuilder, int, ModelActionOption> nextAction, string sprocName)
        {
            
            sb.AppendLine("DECLARE @Name VarChar(100),@Type VarChar(20), @Schema VarChar(20);");
            sb.AppendLine(Indent.Format(level,"SELECT @Name = '{0}', @Type = 'PROCEDURE', @Schema = '{1}';",sprocName,vm.TableSchema));
            sb.AppendLine();
            sb.AppendLine("IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(@Schema + '.' +  @Name))");
            sb.AppendLine("BEGIN");
            sb.AppendLine(Indent.Append(++level, "DECLARE @SQL VARCHAR(1000);"));
            sb.AppendLine(Indent.Append(level, "SET @SQL = 'CREATE ' + @Type + ' ' + @Schema + '.' + @Name + ' AS SELECT * FROM sys.objects';"));
            sb.AppendLine(Indent.Append(level, "EXECUTE(@SQL)"));
            sb.AppendLine(Indent.Append(--level,"END"));
            sb.AppendLine(Indent.Append(level, "PRINT 'Updating ' + @Type + ' ' + @Schema + '.' + @Name+ ' on ' + CONVERT(VARCHAR,GETDATE(),121)  ;"));
            sb.AppendLine("GO");
            sb.AppendLine();
            sb.AppendLine("SET ANSI_NULLS ON");
            sb.AppendLine("GO");
            sb.AppendLine();
            sb.AppendLine("SET QUOTED_IDENTIFIER ON");
            sb.AppendLine("GO");
            sb.AppendLine();
            sb.AppendLine("/***********************************************************************************************");
            sb.AppendLine(Indent.Append(++level,"Created by            Date           Comments"));
            sb.AppendLine(Indent.Append(level, "------------------------------------------------------------------------------------------"));
            sb.AppendLine(Indent.Format(level,"{0}                  {1}            {2}","generated",DateTime.Now.ToShortDateString(),""));
            sb.AppendLine("");
            sb.AppendLine("");
            sb.AppendLine(Indent.Append(--level,"***********************************************************************************************/"));
            sb.AppendLine();
            sb.AppendLine(Indent.Format(level, "ALTER PROCEDURE [{0}].[{1}]", vm.TableSchema, sprocName));

            if (nextAction != null)
            {
                nextAction.Invoke(sb, level, modelActionOption);
            }

        }

        private void BuildParamsDeclaration(StringBuilder sb, int level, ModelActionOption modelActionOption, Action<StringBuilder, int, ModelActionOption> nextAction)
        {
            
            if (modelActionOption.Equals(ModelActionOption.Post))
            {

                List<SqlColumnViewModel> list = vm.Columns.Where(x => x.IsSelected && !x.IsIdentity && !x.IsPrimaryKey && !x.IsComputed && !(x.Datatype.Equals("datetime") && !String.IsNullOrWhiteSpace(x.DefaultValue))).ToList();
                if (list != null && list.Count >0)
                {
                    sb.AppendLine(Indent.Append(level++, "("));
                    var item = list[0];
                    sb.AppendLine(Indent.Format(level, "@{0} {1}", item.ColumnName.ToLower(), item.ToSprocDatatype));
                    for (int i = 1; i < list.Count; i++)
                    {
                        item = list[i];
                        sb.AppendLine(Indent.Format(level, ",@{0} {1}", item.ColumnName.ToLower(), item.ToSprocDatatype));
                    }
                    sb.AppendLine(Indent.Append(--level, ")"));
                }

                
            }
            else if (modelActionOption.Equals(ModelActionOption.Put))
            {
                sb.AppendLine(Indent.Append(level++, "("));
                List<SqlColumnViewModel> list = vm.Columns.Where(x => x.IsSelected && !x.IsComputed && !(x.Datatype.Equals("datetime") && !String.IsNullOrWhiteSpace(x.DefaultValue))).ToList();
                var item = list[0];
                sb.AppendLine(Indent.Format(level, "@{0} {1}", item.ColumnName.ToLower(), item.ToSprocDatatype));
                for (int i = 1; i < list.Count; i++)
                {
                    item = list[i];
                    sb.AppendLine(Indent.Format(level, ",@{0} {1}", item.ColumnName.ToLower(), item.ToSprocDatatype));
                }
                sb.AppendLine(Indent.Append(--level, ")"));
            }
            else if(modelActionOption.Equals(ModelActionOption.Delete) | modelActionOption.Equals(ModelActionOption.Get))
            {
                sb.AppendLine(Indent.Append(level++, "("));
                List<SqlColumnViewModel> list = vm.Columns.Where(x => x.IsPrimaryKey).ToList();
                if (list != null && list.Count >=1)
                {
                    var item = list[0];
                    sb.AppendLine(Indent.Format(level, "@{0} {1}", item.ColumnName.ToLower(), item.ToSprocDatatype));
                    for (int i = 1; i < list.Count; i++)
                    {
                        item = list[i];
                        sb.AppendLine(Indent.Format(level, ",@{0} {1}", item.ColumnName.ToLower(), item.ToSprocDatatype));
                    }                    
                }
                sb.AppendLine(Indent.Append(--level, ")"));
            }
            else if(modelActionOption.Equals(ModelActionOption.GetAll) | modelActionOption.Equals(ModelActionOption.GetAllProjections))
            {
                List<SqlColumnViewModel> list = vm.Columns.Where(x => x.IsForeignKey).ToList();
                if (list != null && list.Count > 0)
                {
                    sb.AppendLine(Indent.Append(level++, "("));
                    var item = list[0];
                    sb.AppendLine(Indent.Format(level, "@{0} {1}", item.ColumnName.ToLower(), item.ToSprocDatatype));
                    sb.AppendLine(Indent.Append(--level, ")"));
                }                
            }

            sb.AppendLine(Indent.Append(level, "AS"));
            sb.AppendLine(Indent.Append(level, "BEGIN"));
            sb.AppendLine(Indent.Append(level, "SET NOCOUNT ON"));
            sb.AppendLine();

            if (nextAction != null)
            {
                nextAction.Invoke(sb, level, modelActionOption);
            }

            sb.AppendLine(Indent.Append(level, "END"));
        }

        private void BuildSprocBody(StringBuilder sb, int level, ModelActionOption modelActionOption)
        {
            sb.AppendLine();
            if (modelActionOption.Equals(ModelActionOption.Post))
            {
                List<SqlColumnViewModel> list = vm.Columns.Where(x => x.IsSelected && !x.IsIdentity && !x.IsComputed && !(x.Datatype.Equals("datetime") && !String.IsNullOrWhiteSpace(x.DefaultValue))).ToList();
                if (list != null && list.Count > 0)
                {
                    var item = list[0];
                    StringBuilder sbValues = new StringBuilder();
                    sb.AppendLine(Indent.Format(level, "INSERT INTO [{0}].[{1}]",vm.TableSchema,vm.TableName));
                    level += 3;
                    sb.AppendLine(Indent.Format(level, "([{0}]",item.ColumnName.ToLower()));
                    sbValues.AppendLine(Indent.Format(level, "(@{0}", item.ColumnName.ToLower()));
                    int max = list.Count - 1;
                    for (int i = 1; i <= max; i++)
                    {
                        string format = i == max ? ",[{0}])" : ",[{0}]";
                        sb.AppendLine(Indent.Format(level, format,list[i].ColumnName));
                        string valueFormat = i == max ? ",@{0})" : ",@{0}";
                        sbValues.AppendLine(Indent.Format(level, valueFormat, list[i].ColumnName.ToLower()));
                    }
                    level -= 3;
                    sb.AppendLine(Indent.Append(level, "VALUES"));
                    sb.Append(sbValues.ToString());
                    var found = vm.Columns.ToList().Find(x => x.IsIdentity);
                    if (found != null)
                    {
                        sb.AppendLine(Indent.Format(level, "DECLARE @{0} int",found.ColumnName.ToLower()));
                        sb.AppendLine(Indent.Format(level, "SELECT @{0} = SCOPE_IDENTITY()",found.ColumnName.ToLower()));
                        sb.AppendLine();
                    
                        sb.AppendLine(Indent.Format(level, "SELECT [{0}]",vm.Columns.First(x=>x.IsSelected).ColumnName));
                        level += 3;
                        foreach (var selectItem in vm.Columns.Where(x=>x.IsSelected).Skip(1))
                        {
                            sb.AppendLine(Indent.Format(level, ",[{0}]", selectItem.ColumnName));
                        }
                        level -= 3;
                        sb.AppendLine(Indent.Format(level, "FROM [{0}].[{1}]", vm.TableSchema, vm.TableName));
                        sb.AppendLine(Indent.Format(level, "WHERE [{0}] = @{1}", found.ColumnName, found.ColumnName.ToLower()));                        
                    }                   
                }
            }
            else if (modelActionOption.Equals(ModelActionOption.Put))
            {
                List<SqlColumnViewModel> list = vm.Columns.Where(x => x.IsSelected && !x.IsIdentity && !x.IsComputed && !(x.Datatype.Equals("datetime") && !String.IsNullOrWhiteSpace(x.DefaultValue))).ToList();
                if (list != null && list.Count > 0)
                {
                    var found = vm.Columns.ToList().Find(x=>x.IsPrimaryKey);
                    if (found != null)
                    {
                        sb.AppendLine(Indent.Format(level, "UPDATE [{0}].[{1}]", vm.TableSchema, vm.TableName));
                        sb.AppendLine(Indent.Format(++level, "SET [{0}] = @{1}",list[0].ColumnName,list[0].ColumnName.ToLower()));
                        level++;
                        for (int i = 1; i < list.Count; i++)
                        {
                            sb.AppendLine(Indent.Format(level,",[{0}] = @{1}",list[i].ColumnName,list[i].ColumnName.ToLower()));
                        }
                        level -= 3;

                        sb.AppendLine(Indent.Format(level, "WHERE [{0}] = @{1}",found.ColumnName,found.ColumnName.ToLower()));

                        sb.AppendLine(Indent.Format(level, "SELECT [{0}]", vm.Columns.First(x => x.IsSelected).ColumnName));
                        level += 3;
                        foreach (var selectItem in vm.Columns.Where(x => x.IsSelected).Skip(1))
                        {
                            sb.AppendLine(Indent.Format(level, ",[{0}]", selectItem.ColumnName));
                        }
                        level -= 3;
                        sb.AppendLine(Indent.Format(level, "FROM [{0}].[{1}]", vm.TableSchema, vm.TableName));
                        sb.AppendLine(Indent.Format(level, "WHERE [{0}] = @{1}", found.ColumnName,found.ColumnName.ToLower()));                        
                    }

                }
                // update, read
            }
            else if (modelActionOption.Equals(ModelActionOption.Delete))
            {
                var found = vm.Columns.ToList().Find(x => x.IsPrimaryKey);
                if (found != null)
                {
                    sb.AppendLine(Indent.Format(level, "DELETE FROM [{0}].[{1}]",vm.TableSchema,vm.TableName));
                    sb.AppendLine(Indent.Format(level, "WHERE [{0}] = @{1}", found.ColumnName, found.ColumnName.ToLower()));
                }
            }
            else if (modelActionOption.Equals(ModelActionOption.Get))
            {
                var found = vm.Columns.ToList().Find(x => x.IsPrimaryKey);
                if (found != null)
                {
                    sb.AppendLine(Indent.Format(level, "SELECT [{0}]", vm.Columns.First(x => x.IsSelected).ColumnName));
                    level += 3;
                    foreach (var selectItem in vm.Columns.Where(x => x.IsSelected).Skip(1))
                    {
                        sb.AppendLine(Indent.Format(level, ",[{0}]", selectItem.ColumnName));
                    }
                    level -= 3;
                    sb.AppendLine(Indent.Format(level, "FROM [{0}].[{1}]", vm.TableSchema, vm.TableName));
                    sb.AppendLine(Indent.Format(level, "WHERE [{0}] = @{1}", found.ColumnName,found.ColumnName.ToLower()));                    
                }

            }
            else if (modelActionOption.Equals(ModelActionOption.GetAll))
            {
                var found = vm.Columns.ToList().Find(x => x.IsForeignKey);

                sb.AppendLine(Indent.Format(level, "SELECT [{0}]", vm.Columns.First(x => x.IsSelected).ColumnName));
                level += 3;
                foreach (var selectItem in vm.Columns.Where(x => x.IsSelected).Skip(1))
                {
                    sb.AppendLine(Indent.Format(level, ",[{0}]", selectItem.ColumnName));
                }
                level -= 3;
                sb.AppendLine(Indent.Format(level, "FROM [{0}].[{1}]", vm.TableSchema, vm.TableName));
                if (found != null)
                {
                    sb.AppendLine(Indent.Format(level, "WHERE [{0}] = @{1}", found.ColumnName,found.ColumnName.ToLower()));
                }                                    
            }
            else if (modelActionOption.Equals(ModelActionOption.GetAllProjections))
            {
                // read where
            }

            sb.AppendLine();
        }

        #endregion local implementation Sprocs

        #region local implementation API DataService

        private void BuildDataServiceInterface(StringBuilder sb, int level)
        {
            sb.AppendLine(Indent.Format(level, "public interface {0}DataService", vm.ToModelName));
            sb.AppendLine(Indent.Append(level++, "{"));

            sb.AppendLine();

            Discovery.SqlColumn column = null;
            if (vm.TryFindIdentityColumn(out column))
            {

                sb.AppendLine(Indent.Format(level, "bool Delete(int {0});", column.ColumnName));
                sb.AppendLine();
                sb.AppendLine(Indent.Format(level, "IEnumerable<{0}> Get(int {1});", vm.ToModelName,column.ColumnName));
            }
            else
            {
                sb.AppendLine(Indent.Append(level, "bool Delete(int id);"));
                sb.AppendLine();
                sb.AppendLine(Indent.Format(level, "IEnumerable<{0}> Get(int id);", vm.ToModelName));
            }
            
            sb.AppendLine();
            sb.AppendLine(Indent.Format(level, "{0} Post({0} model);", vm.ToModelName));
            sb.AppendLine();
            sb.AppendLine(Indent.Format(level, "{0} Put({0} model);", vm.ToModelName));
            sb.AppendLine();
            sb.AppendLine(Indent.Append(--level, "}"));
        }
        private void BuildDataService(StringBuilder sb, int level)
        {
            //  sb.AppendLine(Indent.Format(level, "",vm.ToModelName));
            //  sb.AppendLine(Indent.Format(level, "var request = GenerateApiRequest<{0}>(HttpVerb.GET);",vm.ToModelName));
            //  sb.AppendLine(Indent.Append(level, "var request = GenerateApiRequest(HttpVerb.GET);"));

            sb.AppendLine(Indent.Format(level, "public static class {0}DataService", vm.ToModelName));
            sb.AppendLine(Indent.Append(level++, "{"));
            sb.AppendLine();

            #region httpmethods

            sb.AppendLine(Indent.Format(level, "public static bool Delete(int id)", vm.ToModelName));
            sb.AppendLine(Indent.Append(level++, "{"));
            sb.AppendLine(Indent.Append(level, "return false;"));
            sb.AppendLine(Indent.Append(--level, "}"));


            sb.AppendLine(Indent.Format(level, "public static {0} Get(int id)", vm.ToModelName));
            sb.AppendLine(Indent.Append(level++, "{"));
            sb.AppendLine(Indent.Format(level, "{0} model = new {0}();", vm.ToModelName));

            sb.AppendLine(Indent.Append(level, "var request = GenerateApiRequest(HttpVerb.GET);"));
            sb.AppendLine(Indent.Format(level, "ApiResponse<{0}> response = Execute(request,ParseMany);", vm.ToModelName));
            sb.AppendLine();
            sb.AppendLine(Indent.Append(level, "if (response.IsOkay)"));
            sb.AppendLine(Indent.Append(level++, "{"));
            sb.AppendLine(Indent.Append(level, "model = response.Model;"));
            sb.AppendLine(Indent.Append(--level, "}"));
            sb.AppendLine(Indent.Append(level, "return model;"));
            sb.AppendLine(Indent.Append(--level, "}"));


            sb.AppendLine(Indent.Format(level, "public static {0} Post({0} model)", vm.ToModelName));
            sb.AppendLine(Indent.Append(level++, "{"));
            sb.AppendLine(Indent.Format(level, "var newModel = new {0}();", vm.ToModelName));
            sb.AppendLine(Indent.Append(level, "var request = GenerateApiRequest(HttpVerb.POST);"));
            sb.AppendLine(Indent.Append(level, "request.Model = model;"));

            sb.AppendLine(Indent.Format(level, "ApiResponse<{0}> response = Execute(request,ParseOne);", vm.ToModelName));

            sb.AppendLine(Indent.Append(level, "if (response.IsOkay)"));
            sb.AppendLine(Indent.Append(level++, "{"));
            sb.AppendLine(Indent.Append(level, "newModel = response.Model;"));
            sb.AppendLine(Indent.Append(--level, "}"));
            sb.AppendLine(Indent.Append(level, "return newModel;"));
            sb.AppendLine(Indent.Append(--level, "}"));


            sb.AppendLine(Indent.Format(level, "public static {0} Put({0} model)", vm.ToModelName));
            sb.AppendLine(Indent.Append(level++, "{"));
            sb.AppendLine(Indent.Format(level, "var updatedModel = new {0}();", vm.ToModelName));
            sb.AppendLine(Indent.Append(level, "var request = GenerateApiRequest(HttpVerb.PUT);"));
            sb.AppendLine(Indent.Append(level, "request.Model = model;"));
            sb.AppendLine(Indent.Format(level, "ApiResponse<{0}> response = Execute(request,ParseOne);", vm.ToModelName));

            sb.AppendLine(Indent.Append(level, "if (response.IsOkay)"));
            sb.AppendLine(Indent.Append(level++, "{"));
            sb.AppendLine(Indent.Append(level, "updatedModel = response.Model;"));
            sb.AppendLine(Indent.Append(--level, "}"));
            sb.AppendLine(Indent.Append(level, "return updatedModel;"));
            sb.AppendLine(Indent.Append(--level, "}"));



            #endregion

            #region boilerplate

            sb.AppendLine(Indent.Format(level, "private static ApiRequest<{0}> GenerateApiRequest(HttpVerb httpVerb = HttpVerb.GET)",vm.ToModelName));
            sb.AppendLine(Indent.Append(level++, "{"));

            sb.AppendLine(Indent.Format(level, "ApiRequest<{0}> request = new ApiRequest<{0}>()",vm.ToModelName));
            sb.AppendLine(Indent.Append(level++, "{"));
            sb.AppendLine(Indent.Append(level, "QueryString = new Dictionary<string,object>(),"));
            sb.AppendLine(Indent.Append(level, "HttpVerb = httpVerb"));
            sb.AppendLine(Indent.Append(--level, "};"));
            sb.AppendLine(Indent.Append(level,"InitializeRequest(request);"));
            sb.AppendLine(Indent.Append(level, "return request;"));

            sb.AppendLine(Indent.Append(--level, "}"));
            sb.AppendLine();
            sb.AppendLine(Indent.Format(level, "private static void InitializeRequest(ApiRequest<{0}> request)", vm.ToModelName));
            sb.AppendLine(Indent.Append(level++, "{"));
            sb.AppendLine(Indent.Append(level, "request.Protocol = ApiConstants.ApiProtocol;"));
            sb.AppendLine(Indent.Append(level, "request.RootUrl = ApiConstants.Url.Root;"));
            sb.AppendLine(Indent.Append(level, "ResolveUrl();"));
            sb.AppendLine(Indent.Append(level, "request.AddHeaders(AddDefaultHeaders);"));
            sb.AppendLine(Indent.Append(--level, "}"));
            sb.AppendLine();

            sb.AppendLine(Indent.Format(level, "private static string ResolveUrl()", vm.ToModelName));
            sb.AppendLine(Indent.Append(level++, "{"));
            sb.AppendLine(Indent.Append(level, "string url = String.Empty;"));
            sb.AppendLine(Indent.Format(level, "string key = \"{0}\";",vm.ToModelName));
            sb.AppendLine(Indent.Append(level, "if (ApiConstants.Url.EndpointMaps.ContainsKey(key))"));
            sb.AppendLine(Indent.Append(level++, "{"));
            sb.AppendLine(Indent.Append(level, "url = ApiConstants.Url.EndpointMaps[key];"));
            sb.AppendLine(Indent.Append(--level, "}"));
            sb.AppendLine(Indent.Append(level, "return url;"));            
            sb.AppendLine(Indent.Append(--level, "}"));

            sb.AppendLine();

            sb.AppendLine(Indent.Format(level, "private static List<Tuple<string,string>> AddDefaultHeaders()", vm.ToModelName));
            sb.AppendLine(Indent.Append(level++, "{"));
            sb.AppendLine(Indent.Append(level, "List<Tuple<string, string>> list = new List<Tuple<string, string>>();"));

            sb.AppendLine(Indent.Append(level, "return list;"));

            sb.AppendLine(Indent.Append(--level, "}"));
            

            sb.AppendLine();
            sb.AppendLine(Indent.Append(level, "#region execute api call"));
            sb.AppendLine(Indent.Format(level, "private static ApiResponse<{0}> Execute(ApiRequest<{0}> request,Func<string, IEnumerable<{0}>> parseJson)", vm.ToModelName));
            sb.AppendLine(Indent.Append(level++, "{"));
            sb.AppendLine(Indent.Append(level, "HttpResponseMessage message = null;"));
            sb.AppendLine(Indent.Format(level, "ApiResponse<{0}> response = new ApiResponse<{0}>(){{ Items = new List<{0}>()}};",vm.ToModelName));
            //using (var client = request.HttpClient())
            sb.AppendLine(Indent.Append(level, "using (var client = request.HttpClient())"));
            sb.AppendLine(Indent.Append(level++, "{"));

            sb.AppendLine(Indent.Append(level,"try"));
            sb.AppendLine(Indent.Append(level++, "{"));
            sb.AppendLine(Indent.Append(level, "switch (request.HttpVerb)"));
            sb.AppendLine(Indent.Append(level++, "{"));

            sb.AppendLine(Indent.Append(level++, "case HttpVerb.DELETE:"));
            sb.AppendLine(Indent.Append(level, "message = client.DeleteAsync(request.ComposeUrl()).Result;"));
            sb.AppendLine(Indent.Append(level--, "break;"));

            sb.AppendLine(Indent.Append(level++, "case HttpVerb.GET:"));
            sb.AppendLine(Indent.Append(level, "message = client.GetAsync(request.ComposeUrl(), HttpCompletionOption.ResponseContentRead).Result;"));
            sb.AppendLine(Indent.Append(level--, "break;"));

            sb.AppendLine(Indent.Append(level++, "case HttpVerb.POST:"));
            sb.AppendLine(Indent.Append(level, "message = client.PostAsync(request.ComposeUrl(), request.Content()).Result;"));
            sb.AppendLine(Indent.Append(level--, "break;"));

            sb.AppendLine(Indent.Append(level++, "case HttpVerb.PUT:"));
            sb.AppendLine(Indent.Append(level, "message = client.PutAsync(request.ComposeUrl(), request.Content()).Result;"));
            sb.AppendLine(Indent.Append(level--, "break;"));
            sb.AppendLine(Indent.Append(--level, "}"));
            sb.AppendLine(Indent.Append(--level, "}"));

            sb.AppendLine(Indent.Append(level, "catch (Exception ex)"));
            sb.AppendLine(Indent.Append(level++, "{"));
            sb.AppendLine(Indent.Append(level,"response.Message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;"));
            sb.AppendLine(Indent.Append(level, "response.StatusCode = System.Net.HttpStatusCode.InternalServerError;"));

            sb.AppendLine(Indent.Append(--level, "}"));
            sb.AppendLine(Indent.Append(--level, "}"));

            sb.AppendLine(Indent.Append(level, "if (message != null)"));
            sb.AppendLine(Indent.Append(level++, "{"));
            sb.AppendLine(Indent.Append(level, "response.StatusCode = response.StatusCode;"));
            sb.AppendLine(Indent.Append(level, "if (message.IsSuccessStatusCode)"));
            sb.AppendLine(Indent.Append(level++, "{"));
            sb.AppendLine(Indent.Append(level, "response.IsOkay = true;"));
            sb.AppendLine(Indent.Append(level, "var task = message.Content.ReadAsStringAsync();"));
            sb.AppendLine(Indent.Append(level, "if (task != null)"));
            sb.AppendLine(Indent.Append(level++, "{"));
            sb.AppendLine(Indent.Append(level, "string body = task.Result;"));
            sb.AppendLine(Indent.Append(level, "if (!String.IsNullOrWhiteSpace(body))"));
            sb.AppendLine(Indent.Append(level++, "{"));
            sb.AppendLine(Indent.Append(level, "response.Body = body;"));


            sb.AppendLine(Indent.Append(level, "try"));
            sb.AppendLine(Indent.Append(level++, "{"));
            sb.AppendLine(Indent.Append(level, "response.Items = parseJson(body).ToList();"));
            sb.AppendLine(Indent.Append(--level, "}"));

            sb.AppendLine(Indent.Append(level, "catch (Exception ex)"));
            sb.AppendLine(Indent.Append(level++, "{"));
            sb.AppendLine(Indent.Append(level, "response.Message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;"));
            sb.AppendLine(Indent.Append(level, "response.StatusCode = System.Net.HttpStatusCode.InternalServerError;"));

            sb.AppendLine(Indent.Append(--level, "}"));


            sb.AppendLine(Indent.Append(--level, "}"));

            sb.AppendLine(Indent.Append(--level, "}"));



            sb.AppendLine(Indent.Append(--level, "}"));





            sb.AppendLine(Indent.Append(--level, "}"));
            sb.AppendLine(Indent.Append(level, "return response;"));
            sb.AppendLine(Indent.Append(--level, "}"));
            //"ApiResponse<T> response = new ApiResponse<T>() { Request = request, Items = new List<T>() };"
            sb.AppendLine(Indent.Append(level, "#endregion"));


            sb.AppendLine(Indent.Append(level, "#region parsers"));

            sb.AppendLine(Indent.Format(level, "private static IEnumerable<{0}> ParseMany(string json)", vm.ToModelName));
            sb.AppendLine(Indent.Append(level++, "{"));
            sb.AppendLine(Indent.Format(level, "return JsonConvert.DeserializeObject<List<{0}>>(json);", vm.ToModelName));
            sb.AppendLine(Indent.Append(--level, "}"));

            sb.AppendLine(Indent.Format(level, "private static IEnumerable<{0}> ParseOne(string json)", vm.ToModelName));
            sb.AppendLine(Indent.Append(level++, "{"));
            sb.AppendLine(Indent.Format(level, "List<{0}> list = new List<{0}>();", vm.ToModelName));
            sb.AppendLine(Indent.Format(level, "{0} item = JsonConvert.DeserializeObject<{0}>(json);",vm.ToModelName));
            sb.AppendLine(Indent.Append(level, "list.Add(item);"));
            sb.AppendLine(Indent.Append(level, "return list;"));
            sb.AppendLine(Indent.Append(--level, "}"));

            sb.AppendLine(Indent.Append(level, "#endregion"));
            #endregion


            sb.AppendLine(Indent.Append(--level, "}"));
        }

        #endregion

        #region local implementation SqlServer DataProvider

        private void BuildDataProviderInterface(StringBuilder sb,int level)
        {
            sb.AppendLine(Indent.Format(level, "public interface I{0}DataProvider", vm.ToModelName));
            sb.AppendLine(Indent.Append(level++, "{"));

            sb.AppendLine();
            sb.AppendLine(Indent.Append(level, "bool Delete(Parameters parameters);"));
            sb.AppendLine();
            sb.AppendLine(Indent.Format(level, "IEnumerable<{0}> Get(Parameters parameters);", vm.ToModelName));
            sb.AppendLine();
            sb.AppendLine(Indent.Format(level, "{0} Post({0} model);", vm.ToModelName));
            sb.AppendLine();
            sb.AppendLine(Indent.Format(level, "{0} Put({0} model);", vm.ToModelName));
            sb.AppendLine();
            sb.AppendLine(Indent.Append(--level, "}"));
        }

        private void BuildSqlServerDataProvider(StringBuilder sb,int level)
        {
            sb.AppendLine(Indent.Format(level, "public class {0}DataProvider : I{0}DataProvider", vm.ToModelName));
            sb.AppendLine(Indent.Append(level++, "{"));
            sb.AppendLine();
            sb.AppendLine(Indent.Append(level, "#region local fields"));
            sb.AppendLine();
            foreach (var item in vm.Columns.Where(x => x.IsSelected && !x.IsComputed))
            {
                sb.AppendLine(Indent.Format(level, "private const string {0}ParamName = \"@{1}\";", item.ColumnName, item.ColumnName.ToLower()));
            }

            sb.AppendLine(Indent.Append(level, "public ILogger Logger { get; set; }"));
            sb.AppendLine(Indent.Append(level, "public IConnectionStringProvider ConnectionStringProvider { get; set; }"));

            sb.AppendLine();
            sb.AppendLine(Indent.Format(level, "#endregion local fields"));
            sb.AppendLine();

            sb.AppendLine(Indent.Append(level,"public SqlConnection GetConnection()"));
            sb.AppendLine(Indent.Append(level++, "{"));
            sb.AppendLine(Indent.Append(level, "string connectionString = ConnectionStringProvider.Get();"));
            sb.AppendLine(Indent.Append(level, "if (String.IsNullOrWhiteSpace(connectionString))"));
            sb.AppendLine(Indent.Append(level++, "{"));
            sb.AppendLine(Indent.Append(level, "throw new ArgumentNullException(nameof(connectionString));"));
            sb.AppendLine(Indent.Append(--level, "}"));
            sb.AppendLine(Indent.Append(level, "return new SqlConnection(connectionString);"));
            sb.AppendLine(Indent.Append(--level, "}"));
            sb.AppendLine();

            #region constructor

            sb.AppendLine(Indent.Format(level,"public {0}DataProvider(IConnectionStringProvider provider, ILoggerFactory loggerFactory = null)", vm.ToModelName));
            sb.AppendLine(Indent.Append(level++, "{"));
            sb.AppendLine(Indent.Append(level, "ConnectionStringProvider = provider;"));
            sb.AppendLine(Indent.Format(level, "Logger = loggerFactory.CreateLogger<{0}DataProvider>();",vm.ToModelName));
            sb.AppendLine(Indent.Append(--level, "}"));
            sb.AppendLine();
            #endregion


            sb.AppendLine(Indent.Append(level, "#region Standard Contract Methods"));

            foreach (var item in modelactions)
            {
                if (vm.ModelActions.HasFlag(item))
                {
                    sb.AppendLine(Indent.Format(level, "#region {0}",item.ToString()));
                    GenerateStandardContractInline(sb, level, item);
                    sb.AppendLine(Indent.Append(level, "#endregion"));
                }
            }
            sb.AppendLine(Indent.Append(level, "#endregion Standard Contract Methods"));
            sb.AppendLine();

            sb.AppendLine(Indent.Append(level, "#region SqlCommand Initializers"));
            foreach (var item in modelactions)
            {
                if (vm.ModelActions.HasFlag(item))
                {
                    GenerateStandardContractCommandInitializers(sb, level, item);
                }
            }
            sb.AppendLine(Indent.Append(level, "#endregion "));


            #region execute reader model
            sb.AppendLine(Indent.Format(level,"private Response<{0}> ExecuteReader(Action<SqlCommand,{0}> initializeCommand,Action<SqlDataReader,List<{0}>> borrowReader, {0} model)", vm.ToModelName));
            sb.AppendLine(Indent.Append(level++, "{"));

            sb.AppendLine(Indent.Format(level, "Response<{0}> response = new Response<{0}>();",vm.ToModelName));
            sb.AppendLine(Indent.Append(level, "try"));
            sb.AppendLine(Indent.Append(level++, "{"));
            sb.AppendLine(Indent.Append(level, "using (SqlConnection cn = GetConnection())"));
            sb.AppendLine(Indent.Append(level++, "{"));
            sb.AppendLine(Indent.Append(level, "cn.Open();"));
            sb.AppendLine(Indent.Append(level, "using (SqlCommand cmd = cn.CreateCommand())"));
            sb.AppendLine(Indent.Append(level++, "{"));
            sb.AppendLine(Indent.Append(level, "initializeCommand(cmd,model);"));
            sb.AppendLine(Indent.Append(level, "using (SqlDataReader reader = cmd.ExecuteReader())"));
            sb.AppendLine(Indent.Append(level++, "{"));
            sb.AppendLine(Indent.Append(level, "borrowReader(reader,response.Items);"));
            sb.AppendLine(Indent.Append(level, "response.SetStatus(true);"));
            sb.AppendLine(Indent.Append(--level, "}"));

            sb.AppendLine(Indent.Append(--level, "}"));
            sb.AppendLine(Indent.Append(--level, "}"));
            sb.AppendLine(Indent.Append(--level, "}"));
            sb.AppendLine(Indent.Append(level, "catch(Exception ex)"));
            sb.AppendLine(Indent.Append(level++, "{"));
            sb.AppendLine(Indent.Append(level, "string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;"));
            sb.AppendLine(Indent.Append(level, "Logger.LogError(ex,message,model);"));
            sb.AppendLine(Indent.Append(level, "response.SetStatus(ex);"));
            sb.AppendLine(Indent.Append(--level, "}"));
            sb.AppendLine(Indent.Append(level, "return response;"));

            sb.AppendLine(Indent.Append(--level, "}"));

            #endregion



            sb.AppendLine(Indent.Format(level, "private Response<{0}> ExecuteReader(Action<SqlCommand,Parameters> initializeCommand,Action<SqlDataReader,List<{0}>> borrowReader, Parameters parameters)", vm.ToModelName));
            sb.AppendLine(Indent.Append(level++, "{"));

            sb.AppendLine(Indent.Format(level, "Response<{0}> response = new Response<{0}>();", vm.ToModelName));
            sb.AppendLine(Indent.Append(level, "try"));
            sb.AppendLine(Indent.Append(level++, "{"));
            sb.AppendLine(Indent.Append(level, "using (SqlConnection cn = GetConnection())"));
            sb.AppendLine(Indent.Append(level++, "{"));
            sb.AppendLine(Indent.Append(level, "cn.Open();"));
            sb.AppendLine(Indent.Append(level, "using (SqlCommand cmd = cn.CreateCommand())"));
            sb.AppendLine(Indent.Append(level++, "{"));
            sb.AppendLine(Indent.Append(level, "initializeCommand(cmd,parameters);"));
            sb.AppendLine(Indent.Append(level, "using (SqlDataReader reader = cmd.ExecuteReader())"));
            sb.AppendLine(Indent.Append(level++, "{"));
            sb.AppendLine(Indent.Append(level, "borrowReader(reader,response.Items);"));
            sb.AppendLine(Indent.Append(level, "response.SetStatus(true);"));
            sb.AppendLine(Indent.Append(--level, "}"));

            sb.AppendLine(Indent.Append(--level, "}"));
            sb.AppendLine(Indent.Append(--level, "}"));
            sb.AppendLine(Indent.Append(--level, "}"));
            sb.AppendLine(Indent.Append(level, "catch(Exception ex)"));
            sb.AppendLine(Indent.Append(level++, "{"));
            sb.AppendLine(Indent.Append(level, "string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;"));
            sb.AppendLine(Indent.Append(level, "Logger.LogError(ex,message,parameters);"));
            sb.AppendLine(Indent.Append(level, "response.SetStatus(ex);"));
            sb.AppendLine(Indent.Append(--level, "}"));
            sb.AppendLine(Indent.Append(level, "return response;"));
            sb.AppendLine(Indent.Append(--level, "}"));



            sb.AppendLine();

            #region execute nonquery

            sb.AppendLine(Indent.Format(level, "private Response<{0}> ExecuteNonQuery(Action<SqlCommand,Parameters> initializeCommand, Parameters parameters)", vm.ToModelName));
            sb.AppendLine(Indent.Append(level++, "{"));

            sb.AppendLine(Indent.Format(level, "Response<{0}> response = new Response<{0}>();", vm.ToModelName));
            sb.AppendLine(Indent.Append(level, "try"));
            sb.AppendLine(Indent.Append(level++, "{"));
            sb.AppendLine(Indent.Append(level, "using (SqlConnection cn = GetConnection())"));
            sb.AppendLine(Indent.Append(level++, "{"));
            sb.AppendLine(Indent.Append(level, "cn.Open();"));
            sb.AppendLine(Indent.Append(level, "using (SqlCommand cmd = cn.CreateCommand())"));
            sb.AppendLine(Indent.Append(level++, "{"));
            sb.AppendLine(Indent.Append(level, "initializeCommand(cmd,parameters);"));
            sb.AppendLine(Indent.Append(level, "int i = cmd.ExecuteNonQuery();"));
            sb.AppendLine(Indent.Append(level, "bool b = i > 0 ? true : false;"));
            sb.AppendLine(Indent.Append(level, "response.SetStatus(b);"));

            //sb.AppendLine(Indent.Append(level, "using (SqlDataReader reader = cmd.ExecuteReader())"));
            //sb.AppendLine(Indent.Append(level++, "{"));
            //sb.AppendLine(Indent.Append(level, "borrowReader(reader,cmd.ExecuteReader();"));
            //sb.AppendLine(Indent.Append(level, "response.SetStatus(true)"));
            //sb.AppendLine(Indent.Append(--level, "}"));

            sb.AppendLine(Indent.Append(--level, "}"));
            sb.AppendLine(Indent.Append(--level, "}"));
            sb.AppendLine(Indent.Append(--level, "}"));
            sb.AppendLine(Indent.Append(level, "catch(Exception ex)"));
            sb.AppendLine(Indent.Append(level++, "{"));
            sb.AppendLine(Indent.Append(level, "string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;"));
            sb.AppendLine(Indent.Append(level, "Logger.LogError(ex,message,parameters);"));
            sb.AppendLine(Indent.Append(level, "response.SetStatus(ex);"));
            sb.AppendLine(Indent.Append(--level, "}"));
            sb.AppendLine(Indent.Append(level, "return response;"));
            sb.AppendLine(Indent.Append(--level, "}"));


            #endregion




            BuildDataProviderBorrowReader(sb, level);

            sb.AppendLine(Indent.Append(--level, "}"));
        }


        #endregion

        #region local implementation MDGInline

        private void BuildMDGInlineClass(StringBuilder sb, int level)
        {
            sb.AppendLine(Indent.Format(level, "public class {0}MDG : SqlServerModelDataGateway<{0}>", vm.ToModelName));
            sb.AppendLine(Indent.Append(level++, "{"));
            sb.AppendLine();
            sb.AppendLine(Indent.Append(level, "#region local fields"));
            sb.AppendLine();

            foreach (var item in vm.Columns.Where(x => x.IsSelected && !x.IsComputed))
            {
                sb.AppendLine(Indent.Format(level, "private const string {0}ParamName = \"@{1}\";", item.ColumnName, item.ColumnName.ToLower()));
            }
            if (!vm.TableSchema.Equals("dbo"))
            {
                sb.AppendLine(Indent.Format(level, "private const string DbSchema = \"{0}\";", vm.TableSchema));
            }
            sb.AppendLine();
            sb.AppendLine(Indent.Append(level, "#endregion local fields"));
            sb.AppendLine();


            sb.AppendLine(Indent.Append(level, "#region SqlCommand overrides"));
            sb.AppendLine();
            foreach (var item in modelactions)
            {
                if (vm.ModelActions.HasFlag(item))
                {
                    GenerateCreateCommandInline(sb, level, item);
                }
            }
            sb.AppendLine();
            sb.AppendLine(Indent.Append(level, "#endregion SqlCommand overrides"));
            sb.AppendLine();

            BuildBorrowReader(sb, level);

            sb.AppendLine(Indent.Append(--level, "}"));
        }

        private void GenerateStandardContractInline(StringBuilder sb, int level, ModelActionOption modelActionOption)
        {
            StringBuilder sbParams = new StringBuilder();

            if (modelActionOption.Equals(ModelActionOption.Delete))
            {
                sb.AppendLine(Indent.Format(level, "bool I{0}DataProvider.Delete(Parameters parameters)", vm.TableName));
                sb.AppendLine(Indent.Append(level++, "{"));
                sb.AppendLine(Indent.Append(level, "bool b = false;"));
                sb.AppendLine(Indent.Append(level, "var response = ExecuteNonQuery(DeleteSqlCommand,parameters);"));
                sb.AppendLine(Indent.Append(level, "if(response.IsOkay)"));
                sb.AppendLine(Indent.Append(level++, "{"));
                sb.AppendLine(Indent.Append(level, "b = true;"));
                sb.AppendLine(Indent.Append(--level, "}"));
                sb.AppendLine(Indent.Append(level, "return b;"));
                sb.AppendLine(Indent.Append(--level, "}"));
                sb.AppendLine();
            }

            if (modelActionOption.Equals(ModelActionOption.Get))
            {
                sb.AppendLine(Indent.Format(level, "IEnumerable<{0}> I{0}DataProvider.Get(Parameters parameters)", vm.TableName));
                sb.AppendLine(Indent.Append(level++, "{"));
                sb.AppendLine(Indent.Format(level, "List<{0}> list = new List<{0}>();", vm.TableName));
                sb.AppendLine(Indent.Append(level, "var response = ExecuteReader(GetSqlCommand,Borrow,parameters);"));
                sb.AppendLine(Indent.Append(level, "if(response.IsOkay)"));
                sb.AppendLine(Indent.Append(level++, "{"));
                sb.AppendLine(Indent.Append(level, "list = response.ToList();"));
                sb.AppendLine(Indent.Append(--level, "}"));
                sb.AppendLine(Indent.Append(level, "return list;"));

                sb.AppendLine(Indent.Append(--level, "}"));
                sb.AppendLine();
            }

            if (modelActionOption.Equals(ModelActionOption.Post))
            {
                sb.AppendLine(Indent.Format(level, "{0} I{0}DataProvider.Post({0} model)", vm.TableName));
                sb.AppendLine(Indent.Append(level++, "{"));
                sb.AppendLine(Indent.Format(level, "{0} item = null;",vm.TableName));
                sb.AppendLine(Indent.Append(level, "var response = ExecuteReader(PostSqlCommand,Borrow,model);"));
                sb.AppendLine(Indent.Append(level, "if(response.IsOkay)"));
                sb.AppendLine(Indent.Append(level++, "{"));
                sb.AppendLine(Indent.Append(level, "item = response.Model;"));
                sb.AppendLine(Indent.Append(--level, "}"));
                sb.AppendLine(Indent.Append(level, "return item;"));
                sb.AppendLine(Indent.Append(--level, "}"));
                sb.AppendLine();
            }
            if(modelActionOption.Equals(ModelActionOption.Put))
            {
                sb.AppendLine(Indent.Format(level, "{0} I{0}DataProvider.Put({0} model)", vm.TableName));
                sb.AppendLine(Indent.Append(level++, "{"));
                sb.AppendLine(Indent.Format(level, "{0} item = null;", vm.TableName));
                sb.AppendLine(Indent.Append(level, "var response = ExecuteReader(PutSqlCommand,Borrow,model);"));
                sb.AppendLine(Indent.Append(level, "if(response.IsOkay)"));
                sb.AppendLine(Indent.Append(level++, "{"));
                sb.AppendLine(Indent.Append(level, "item = response.Model;"));
                sb.AppendLine(Indent.Append(--level, "}"));
                sb.AppendLine(Indent.Append(level, "return item;"));
                sb.AppendLine(Indent.Append(--level, "}"));
                sb.AppendLine();
            }

        }

        private void GenerateStandardContractCommandInitializers(StringBuilder sb, int level, ModelActionOption modelActionOption)
        {

            StringBuilder sbParams = new StringBuilder();
            if (modelActionOption.Equals(ModelActionOption.Post))
            {
                sb.AppendLine(Indent.Format(level, "private static void PostSqlCommand(SqlCommand cmd, {0} model)", vm.TableName));
                sb.AppendLine(Indent.Append(level++, "{"));
                sb.AppendLine(Indent.Append(level, "cmd.CommandType = CommandType.Text;"));
                sb.AppendLine();


                int maxlength = 70;
                List<string> lines = new List<string>();
                List<string> sqllines = new List<string>();
                lines.Add(String.Format("string sql = \"insert into [{0}].[{1}] ( ", vm.TableSchema, vm.TableName));

                int i = 0;
                foreach (var item in vm.Columns.Where(x => x.IsSelected && !x.IsIdentity && !x.IsPrimaryKey && !x.IsComputed))
                {
                    if (i++ > 0)
                    {
                        lines.Add(",");
                        sqllines.Add(" + \",\" + ");
                    }
                    lines.Add(String.Format("[{0}]", item.ColumnName));
                    sqllines.Add(String.Format("{0}ParamName", item.ColumnName));

                    sbParams.AppendLine(Indent.Format(level, "cmd.Parameters.AddWithValue( {0}ParamName, model.{0} );", item.ColumnName));
                }
                lines.Add(String.Format(" ) values (\" + "));
                lines.AddRange(sqllines);
                lines.Add(" + \")\";");
                StringBuilder append = new StringBuilder();
                foreach (var item in lines)
                {
                    append.Append(item);
                }
                sb.AppendLine(Indent.Append(level, append.ToString()));
                sb.AppendLine();
                sb.AppendLine(Indent.Append(level, "cmd.CommandText = sql;"));
                sb.AppendLine();
                sb.Append(sbParams.ToString());
                sb.AppendLine();
                sb.AppendLine(Indent.Append(--level, "}"));
            }
            else if (modelActionOption.Equals(ModelActionOption.Put))
            {
                sb.AppendLine(Indent.Format(level, "private static void PutSqlCommand(SqlCommand cmd, {0} model)", vm.TableName));
                sb.AppendLine(Indent.Append(level++, "{"));
                sb.AppendLine(Indent.Append(level, "cmd.CommandType = CommandType.Text;"));
                sb.AppendLine();

                List<SqlColumnViewModel> list = vm.Columns.Where(x => x.IsSelected && !x.IsIdentity && !x.IsPrimaryKey && !x.IsComputed).ToList();
                var found = vm.Columns.ToList().Find(x => x.IsPrimaryKey);

                List<string> lines = new List<string>();
                List<string> sqllines = new List<string>();
                lines.Add(String.Format("string sql = \"update [{0}].[{1}]", vm.TableSchema, vm.TableName));

                if (list != null && list.Count > 0 && found != null)
                {
                    var first = list[0];
                    lines.Add(String.Format(" set [{0}] = \" + {0}ParamName ", first.ColumnName));
                    sbParams.AppendLine(Indent.Format(level, "cmd.Parameters.AddWithValue( {0}ParamName, model.{0} );", first.ColumnName));
                    foreach (var item in list.Skip(1))
                    {
                        lines.Add(String.Format("+ \" , [{0}] = \" + {0}ParamName ", item.ColumnName));
                        sbParams.AppendLine(Indent.Format(level, "cmd.Parameters.AddWithValue( {0}ParamName, model.{0} );", item.ColumnName));
                    }
                    lines.Add(String.Format(" + \" where [{0}] = \" + {0}ParamName ", found.ColumnName));

                }
                lines.Add(";");

                StringBuilder append = new StringBuilder();
                foreach (var item in lines)
                {
                    append.Append(item);
                }
                sb.AppendLine(Indent.Append(level, append.ToString()));
                sb.AppendLine();


                sb.AppendLine();
                sb.AppendLine(Indent.Append(level, "cmd.CommandText = sql;"));
                sb.AppendLine();
                sb.Append(sbParams.ToString());
                sb.AppendLine(Indent.Append(--level, "}"));
            }
            else if (modelActionOption.Equals(ModelActionOption.Delete))
            {
                sb.AppendLine(Indent.Format(level, "private static void DeleteSqlCommand(SqlCommand cmd, Parameters parameters)"));
                sb.AppendLine(Indent.Append(level++, "{"));
                sb.AppendLine(Indent.Append(level, "cmd.CommandType = CommandType.Text;"));
                sb.AppendLine();

                var found = vm.Columns.ToList().Find(x => x.IsPrimaryKey);
                if (found != null)
                {
                    sb.AppendLine(Indent.Format(level, "string sql = \"delete from [{0}].[{1}] where [{2}] = \" + {2}ParamName;", vm.TableSchema, vm.TableName, found.ColumnName));
                }

                sb.AppendLine();
                sb.AppendLine(Indent.Append(level, "cmd.CommandText = sql;"));
                sb.AppendLine();

                if (found != null)
                {
                    var csharptype = csharpmaps.Find(x => x.Item1.Equals(found.Datatype, StringComparison.OrdinalIgnoreCase));
                    sb.AppendLine(Indent.Format(level, "cmd.Parameters.AddWithValue( {0}ParamName, parameters.GetValue<{1}>(\"{0}\") );", found.ColumnName, csharptype.Item2));
                }

                sb.AppendLine();
                sb.AppendLine(Indent.Append(--level, "}"));
            }
            else if (modelActionOption.Equals(ModelActionOption.Get))
            {
                sb.AppendLine(Indent.Format(level, "public void GetSqlCommand(SqlCommand cmd, Parameters parameters)"));
                sb.AppendLine(Indent.Append(level++, "{"));
                sb.AppendLine(Indent.Append(level, "cmd.CommandType = CommandType.Text;"));
                sb.AppendLine();
                int i = 0;
                var found = vm.Columns.ToList().Find(x => x.IsPrimaryKey);
                if (found != null)
                {
                    StringBuilder sbSelect = new StringBuilder();
                    sbSelect.Append("select");
                    foreach (var item in vm.Columns.Where(x => x.IsSelected))
                    {
                        if (i++ > 0)
                        {
                            sbSelect.Append(",");
                        }
                        sbSelect.AppendFormat(" [{0}]", item.ColumnName);
                    }

                    //sbSelect.AppendFormat(" from [{0}].[{1}] where [{2}] = \" + {2}ParamName ;", vm.TableSchema, vm.TableName, found.ColumnName);
                    sbSelect.AppendFormat(" from [{0}].[{1}]", vm.TableSchema, vm.TableName);
                    sb.AppendLine(Indent.Append(level, "var sb = new StringBuilder();"));
                    sb.AppendLine(Indent.Format(level, "sb.Append(\"{0}\");",sbSelect.ToString()));
                    sb.AppendLine(Indent.Append(level, "int id;"));
                    sb.AppendLine(Indent.Format(level, "if(parameters != null && parameters.TryGet<int>(\"{0}\",out id))",found.ColumnName));
                    sb.AppendLine(Indent.Append(level++, "{"));
                    sb.AppendLine(Indent.Format(level,"sb.Append(\" where [{0}] = {0}ParamName\");",found.ColumnName));
                    sb.AppendLine(Indent.Format(level, "cmd.Parameters.AddWithValue({0}ParamName,id);",found.ColumnName));                    
                    sb.AppendLine(Indent.Append(--level, "}"));
                }


                sb.AppendLine();
                sb.AppendLine(Indent.Append(level, "cmd.CommandText = sql;"));
                sb.AppendLine();
                //if (found != null)
                //{
                //    var csharptype = csharpmaps.Find(x => x.Item1.Equals(found.Datatype, StringComparison.OrdinalIgnoreCase));
                //    sb.AppendLine(Indent.Format(level, "cmd.Parameters.AddWithValue( {0}ParamName, parameters.GetValue<{1}>(\"{0}\") );", found.ColumnName, csharptype.Item2));
                //}
                sb.AppendLine();
                sb.AppendLine(Indent.Append(--level, "}"));
            }
            else if (modelActionOption.Equals(ModelActionOption.GetAll))
            {
                sb.AppendLine(Indent.Format(level, "public void GetAllSqlCommand(SqlCommand cmd, Parameters parameters)"));
                sb.AppendLine(Indent.Append(level++, "{"));
                sb.AppendLine(Indent.Append(level, "cmd.CommandType = CommandType.Text;"));
                sb.AppendLine();

                int i = 0;
                var found = vm.Columns.ToList().Find(x => x.IsForeignKey);

                StringBuilder sbSelect = new StringBuilder();
                sbSelect.Append("string sql = \"select");
                foreach (var item in vm.Columns.Where(x => x.IsSelected))
                {
                    if (i++ > 0)
                    {
                        sbSelect.Append(",");
                    }
                    sbSelect.AppendFormat(" [{0}]", item.ColumnName);
                }
                if (found != null)
                {
                    sbSelect.AppendFormat(" from [{0}].[{1}] where [{2}] = \" + {2}ParamName ;", vm.TableSchema, vm.TableName, found.ColumnName);
                }
                else
                {
                    sbSelect.AppendFormat(" from [{0}].[{1}] \";", vm.TableSchema, vm.TableName);
                }
                sb.AppendLine(Indent.Append(level, sbSelect.ToString()));

                sb.AppendLine(Indent.Append(level, "cmd.CommandText = sql;"));
                sb.AppendLine();

                if (found != null)
                {
                    var csharptype = csharpmaps.Find(x => x.Item1.Equals(found.Datatype, StringComparison.OrdinalIgnoreCase));
                    sb.AppendLine(Indent.Format(level, "cmd.Parameters.AddWithValue( {0}ParamName, parameters.GetValue<{1}>(\"{0}\") );", found.ColumnName, csharptype.Item2));
                    sb.AppendLine();
                }
                sb.AppendLine(Indent.Append(--level, "}"));
            }
            else if (modelActionOption.Equals(ModelActionOption.GetAllProjections))
            {
                sb.AppendLine(Indent.Format(level, "public void GetAllProjectionsSqlCommand(SqlCommand cmd, Parameters parameters)"));
                sb.AppendLine(Indent.Append(level++, "{"));
                sb.AppendLine(Indent.Append(level, "cmd.CommandType = CommandType.Text;"));
                sb.AppendLine();

                sb.AppendLine(Indent.Append(level, "string sql = \"\";"));

                sb.AppendLine();
                sb.AppendLine(Indent.Append(level, "cmd.CommandText = sql;"));
                sb.AppendLine();
                sb.AppendLine(Indent.Append(--level, "}"));
            }

        }



        private void GenerateCreateCommandInline(StringBuilder sb, int level, ModelActionOption modelActionOption)
        {
            
            StringBuilder sbParams = new StringBuilder();
            if (modelActionOption.Equals(ModelActionOption.Post))
            {
                sb.AppendLine(Indent.Format(level, "public override SqlCommand PostSqlCommand(SqlConnection cn, {0} model, IContext context)",vm.TableName));
                sb.AppendLine(Indent.Append(level++, "{"));
                sb.AppendLine(Indent.Append(level, "SqlCommand cmd = cn.CreateCommand();"));
                sb.AppendLine(Indent.Append(level, "cmd.CommandType = CommandType.Text;"));
                sb.AppendLine();

                
                int maxlength = 70;
                List<string> lines = new List<string>();
                List<string> sqllines = new List<string>();
                lines.Add(String.Format("string sql = \"insert into [{0}].[{1}] ( ",vm.TableSchema,vm.TableName));

                int i = 0;
                foreach (var item in vm.Columns.Where(x => x.IsSelected && !x.IsIdentity && !x.IsPrimaryKey && !x.IsComputed))
                {
                    if (i++ > 0)
                    {
                        lines.Add(",");
                        sqllines.Add(" + \",\" + ");
                    }
                    lines.Add(String.Format("[{0}]", item.ColumnName));
                    sqllines.Add(String.Format("{0}ParamName",item.ColumnName));

                    sbParams.AppendLine(Indent.Format(level, "cmd.Parameters.AddWithValue( {0}ParamName, model.{0} );", item.ColumnName));
                } 
                lines.Add(String.Format(" ) values (\" + "));
                lines.AddRange(sqllines);
                lines.Add(" + \")\";");
                StringBuilder append = new StringBuilder();
                foreach (var item in lines)
                {
                    append.Append(item);
                }
                sb.AppendLine(Indent.Append(level, append.ToString()));
                sb.AppendLine();
                sb.AppendLine(Indent.Append(level, "cmd.CommandText = sql;"));
                sb.AppendLine();
                sb.Append(sbParams.ToString());
                sb.AppendLine();
                sb.AppendLine(Indent.Append(level, "return cmd;"));
                sb.AppendLine(Indent.Append(--level, "}"));
            }
            else if(modelActionOption.Equals(ModelActionOption.Put))
            {
                sb.AppendLine(Indent.Format(level, "public override SqlCommand PutSqlCommand(SqlConnection cn, {0} model, ICriterion criterion, IContext context)",vm.TableName));
                sb.AppendLine(Indent.Append(level++, "{"));
                sb.AppendLine(Indent.Append(level, "SqlCommand cmd = cn.CreateCommand();"));
                sb.AppendLine(Indent.Append(level, "cmd.CommandType = CommandType.Text;"));
                sb.AppendLine();

                List<SqlColumnViewModel> list = vm.Columns.Where(x => x.IsSelected && !x.IsIdentity && !x.IsPrimaryKey && !x.IsComputed).ToList();
                var found = vm.Columns.ToList().Find(x => x.IsPrimaryKey);

                List<string> lines = new List<string>();
                List<string> sqllines = new List<string>();
                lines.Add(String.Format("string sql = \"update [{0}].[{1}]",vm.TableSchema,vm.TableName));

                if (list != null && list.Count > 0 && found != null)
                {
                    var first = list[0];
                    lines.Add(String.Format(" set [{0}] = \" + {0}ParamName ", first.ColumnName));
                    sbParams.AppendLine(Indent.Format(level, "cmd.Parameters.AddWithValue( {0}ParamName, model.{0} );", first.ColumnName));
                    foreach (var item in list.Skip(1))
                    {
                        lines.Add(String.Format("+ \" , [{0}] = \" + {0}ParamName ",item.ColumnName));
                        sbParams.AppendLine(Indent.Format(level, "cmd.Parameters.AddWithValue( {0}ParamName, model.{0} );", item.ColumnName));
                    }
                    lines.Add(String.Format(" + \" where [{0}] = \" + {0}ParamName ",found.ColumnName));
                    
                }
                lines.Add(";");
 
                StringBuilder append = new StringBuilder();
                foreach (var item in lines)
                {
                    append.Append(item);
                }
                sb.AppendLine(Indent.Append(level, append.ToString()));
                sb.AppendLine();


                sb.AppendLine();
                sb.AppendLine(Indent.Append(level, "cmd.CommandText = sql;"));
                sb.AppendLine();
                sb.Append(sbParams.ToString());
                sb.AppendLine();
                sb.AppendLine(Indent.Append(level, "return cmd;"));
                sb.AppendLine(Indent.Append(--level, "}"));
            }
            else if (modelActionOption.Equals(ModelActionOption.Delete))
            {
                sb.AppendLine(Indent.Format(level, "public override SqlCommand DeleteSqlCommand(SqlConnection cn, ICriterion criterion, IContext context)"));
                sb.AppendLine(Indent.Append(level++, "{"));
                sb.AppendLine(Indent.Append(level, "SqlCommand cmd = cn.CreateCommand();"));
                sb.AppendLine(Indent.Append(level, "cmd.CommandType = CommandType.Text;"));
                sb.AppendLine();

                var found = vm.Columns.ToList().Find(x => x.IsPrimaryKey);
                if (found != null)
                {
                    sb.AppendLine(Indent.Format(level, "string sql = \"delete from [{0}].[{1}] where [{2}] = \" + {2}ParamName;",vm.TableSchema,vm.TableName,found.ColumnName));
                }

                sb.AppendLine();
                sb.AppendLine(Indent.Append(level, "cmd.CommandText = sql;"));
                sb.AppendLine();
                
                if (found != null)
                {
                    var csharptype = csharpmaps.Find(x => x.Item1.Equals(found.Datatype, StringComparison.OrdinalIgnoreCase));
                    sb.AppendLine(Indent.Format(level, "cmd.Parameters.AddWithValue( {0}ParamName, criterion.GetValue<{1}>(\"{0}\") );",found.ColumnName,csharptype.Item2));
                }
                
                sb.AppendLine();
                sb.AppendLine(Indent.Append(level, "return cmd;"));
                sb.AppendLine(Indent.Append(--level, "}"));
            }
            else if (modelActionOption.Equals(ModelActionOption.Get))
            {
                sb.AppendLine(Indent.Format(level, "public override SqlCommand GetSqlCommand(SqlConnection cn, ICriterion criterion, IContext context)"));
                sb.AppendLine(Indent.Append(level++, "{"));
                sb.AppendLine(Indent.Append(level, "SqlCommand cmd = cn.CreateCommand();"));
                sb.AppendLine(Indent.Append(level, "cmd.CommandType = CommandType.Text;"));
                sb.AppendLine();
                int i = 0;
                var found = vm.Columns.ToList().Find(x => x.IsPrimaryKey);
                if (found != null)
                {
                    StringBuilder sbSelect = new StringBuilder();
                    sbSelect.Append("string sql = \"select");
                    foreach (var item in vm.Columns.Where(x=>x.IsSelected))
                    {
                        if (i++ > 0)
                        {
                            sbSelect.Append(",");
                        }
                        sbSelect.AppendFormat(" [{0}]", item.ColumnName);
                    }
                    sbSelect.AppendFormat(" from [{0}].[{1}] where [{2}] = \" + {2}ParamName ;", vm.TableSchema, vm.TableName,found.ColumnName); 
                    sb.AppendLine(Indent.Append(level, sbSelect.ToString()));
                }

                
                sb.AppendLine();
                sb.AppendLine(Indent.Append(level, "cmd.CommandText = sql;"));
                sb.AppendLine();               
                if (found != null)
                {
                    var csharptype = csharpmaps.Find(x => x.Item1.Equals(found.Datatype, StringComparison.OrdinalIgnoreCase));
                    sb.AppendLine(Indent.Format(level, "cmd.Parameters.AddWithValue( {0}ParamName, criterion.GetValue<{1}>(\"{0}\") );", found.ColumnName, csharptype.Item2));
                }               
                sb.AppendLine();
                sb.AppendLine(Indent.Append(level, "return cmd;"));
                sb.AppendLine(Indent.Append(--level, "}"));
            }
            else if (modelActionOption.Equals(ModelActionOption.GetAll))
            {
                sb.AppendLine(Indent.Format(level, "public override SqlCommand GetAllSqlCommand(SqlConnection cn, ICriterion criterion, IContext context)"));
                sb.AppendLine(Indent.Append(level++, "{"));
                sb.AppendLine(Indent.Append(level, "SqlCommand cmd = cn.CreateCommand();"));
                sb.AppendLine(Indent.Append(level, "cmd.CommandType = CommandType.Text;"));
                sb.AppendLine();

                int i = 0;
                var found = vm.Columns.ToList().Find(x => x.IsForeignKey);

                StringBuilder sbSelect = new StringBuilder();
                sbSelect.Append("string sql = \"select");
                foreach (var item in vm.Columns.Where(x => x.IsSelected))
                {
                    if (i++ > 0)
                    {
                        sbSelect.Append(",");
                    }
                    sbSelect.AppendFormat(" [{0}]", item.ColumnName);
                }
                if (found != null)
                {
                    sbSelect.AppendFormat(" from [{0}].[{1}] where [{2}] = \" + {2}ParamName ;", vm.TableSchema, vm.TableName,found.ColumnName);
                }
                else
                {
                    sbSelect.AppendFormat(" from [{0}].[{1}] \";", vm.TableSchema, vm.TableName);
                }
                    sb.AppendLine(Indent.Append(level, sbSelect.ToString()));                   

                sb.AppendLine(Indent.Append(level, "cmd.CommandText = sql;"));
                sb.AppendLine();
                
                if (found != null)
                {
                    var csharptype = csharpmaps.Find(x => x.Item1.Equals(found.Datatype, StringComparison.OrdinalIgnoreCase));
                    sb.AppendLine(Indent.Format(level, "cmd.Parameters.AddWithValue( {0}ParamName, criterion.GetValue<{1}>(\"{0}\") );", found.ColumnName, csharptype.Item2));
                    sb.AppendLine();
                }
                            
                sb.AppendLine(Indent.Append(level, "return cmd;"));
                sb.AppendLine(Indent.Append(--level, "}"));
            }
            else if (modelActionOption.Equals(ModelActionOption.GetAllProjections))
            {
                sb.AppendLine(Indent.Format(level, "public override SqlCommand GetAllProjectionsSqlCommand(SqlConnection cn, ICriterion criterion, IContext context)"));
                sb.AppendLine(Indent.Append(level++, "{"));
                sb.AppendLine(Indent.Append(level, "SqlCommand cmd = cn.CreateCommand();"));
                sb.AppendLine(Indent.Append(level, "cmd.CommandType = CommandType.Text;"));
                sb.AppendLine();
                
                sb.AppendLine(Indent.Append(level, "string sql = \"\";"));

                sb.AppendLine();
                sb.AppendLine(Indent.Append(level, "cmd.CommandText = sql;"));
                sb.AppendLine();
                sb.AppendLine(Indent.Append(level, "return cmd;"));
                sb.AppendLine(Indent.Append(--level, "}"));
            }

        }

        #endregion local implementation MDGInline

    }
}
