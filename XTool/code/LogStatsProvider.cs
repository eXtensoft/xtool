using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool.Cyclops
{
    public static class LogStatsProvider
    {

        public static bool TryGetStatisitics(string connectionString, out LogStats statistics)
        {
            List<LogStat> list = new List<LogStat>();
            string message = String.Empty;
            statistics = new LogStats() { CreatedAt = DateTime.Now, Statistics = new List<LogStat>() };
            bool b = false;
            if (!String.IsNullOrWhiteSpace(connectionString) && TryConnect(connectionString, out message))
            {
                
                try
                {
                    using (SqlConnection cn = new SqlConnection(connectionString))
                    {
                        cn.Open();
                        using (SqlCommand cmd = cn.CreateCommand())
                        {
                            cmd.CommandType = System.Data.CommandType.Text;
                            cmd.CommandText = BuildSchemaDiscovery().ToString();

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    LogStat item = new LogStat() {  };
                                    item.Schema = reader.GetString(reader.GetOrdinal("LogSchema"));
                                    item.Tablename = reader.GetString(reader.GetOrdinal("Tablename"));
                                    item.MaxId = reader.GetInt64(reader.GetOrdinal("RecordCount"));
                                    item.LastEntryAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"));
                                    list.Add(item);
                                }
                            }
                        }
                    }
                    b = true;
                }
                catch (Exception ex)
                {
                    statistics.Message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                }

            }

            if (b)
            {
                statistics.Statistics = list;
                //statistics.Statistics = list.OrderBy(x => x.Tablename).OrderBy(y => y.Schema).ToList();
                SetDefaults(statistics);
                
            }
            return b;
        }

        private static void SetDefaults(LogStats stats)
        {
            DateTime now = DateTime.Now;

            stats.MonthOfYear = now.ToString("MMM").ToLower();
            stats.WeekOfYear = now.Date.WeekOfYear().ToString("000");
            stats.DayOfWeek = now.DayOfWeek.ToString().Substring(0, 3).ToLower();

           
        }

        private static bool TryConnect(string connectionString, out string message)
        {
            message = String.Empty;
            bool b = false;

            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    cn.Open();
                    if (cn.State == System.Data.ConnectionState.Open)
                    {
                        b = true;
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }


            return b;


        }

        private static StringBuilder BuildSchemaDiscovery()
        {
            StringBuilder sb = new StringBuilder();
            string temptable = " create table #r(LogSchema nvarchar(15),Tablename nvarchar(15),RecordCount bigint,CreatedAt Datetime)";
            string selectTempTable = " select * from #r";
            string dropTempTable = " drop table #r";
            string schemaExists = " if exists(select * from sys.schemas where name = 'log')";
            string begin = " begin";
            string end = " end";


            sb.AppendLine(temptable);

            // Log
            sb.AppendLine(schemaExists);
            sb.AppendLine(begin);
            sb.Append(BuildSchema("log"));
            sb.AppendLine(end);

            // MonthOfYear
            List<string> months = GenerateMonths();

            sb.AppendLine(schemaExists);
            sb.AppendLine(begin);
            foreach (var item in months)
            {
                sb.Append(BuildSchema(item));
            }

            sb.AppendLine(end);

            // DayOfWeek
            List<string> days = GenerateDays();

            sb.AppendLine(schemaExists);
            sb.AppendLine(begin);
            foreach (var item in days)
            {
                sb.Append(BuildSchema(item));
            }
            sb.AppendLine(end);


            // WeekOfYear
            List<string> weeks = GenerateWeeks();

            sb.AppendLine(schemaExists);
            sb.AppendLine(begin);
            foreach (var item in weeks)
            {
                sb.Append(BuildSchema(item));
            }
            sb.AppendLine(end);

            sb.AppendLine(selectTempTable);
            sb.AppendLine(dropTempTable);
            return sb;
        }

        private static string BuildSchema(string schema)
        {

            string schemaTemplate = "<schema>";
            string begin = " begin";
            string end = " end";
            string orelse = " else";
            string insertR = " insert into #r([LogSchema],[Tablename],[RecordCount],[CreatedAt])";


            StringBuilder sb = new StringBuilder();

            // if Api table exists for this schema
            string insertApi = " SELECT TOP 1 '<schema>','ApiRequest', l.[ApiRequestId] as [Id],l.[Start] as [CreatedAt] FROM [<schema>].[ApiRequest] l ORDER BY l.[ApiRequestId] DESC";
            string existsApi = " if exists (select * from sys.objects where object_id = OBJECT_ID(N'[<schema>].[ApiRequest]') and type in (N'u'))";
            string ifApiExist = " if exists (select top 1 l.[ApiRequestId] from [<schema>].[ApiRequest] l order by l.[ApiRequestId] desc)";
            string insertApiDefault = " values ('<schema>','ApiRequest',0,'2000-1-1')";

            sb.AppendLine(existsApi.Replace(schemaTemplate, schema));
            sb.AppendLine(begin);
            sb.AppendLine(ifApiExist.Replace(schemaTemplate, schema));
            sb.AppendLine(begin);
            sb.AppendLine(insertR);
            sb.AppendLine(insertApi.Replace(schemaTemplate, schema));
            sb.AppendLine(end);
            sb.AppendLine(orelse);
            sb.AppendLine(begin);
            sb.AppendLine(insertR);
            sb.AppendLine(insertApiDefault.Replace(schemaTemplate, schema));
            sb.AppendLine(end);
            sb.AppendLine(end);

            // if Error table exists for this schema
            string insertError = " SELECT TOP 1 '<schema>','Error',e.[Id],e.[CreatedAt] FROM [<schema>].[Error] e ORDER BY e.[Id] DESC";
            string existsError = " if exists (select * from sys.objects where object_id = OBJECT_ID(N'[<schema>].[Error]') and type in (N'u'))";
            string ifErrorExist = "if exists (select top 1 e.[Id] from [<schema>].[Error] e order by e.[Id] desc)";
            string insertErrorDefault = " values ('<schema>','Error',0,'2000-1-1')";

            sb.AppendLine(existsError.Replace(schemaTemplate, schema));
            sb.AppendLine(begin);
            sb.AppendLine(ifErrorExist.Replace(schemaTemplate, schema));
            sb.AppendLine(begin);
            sb.AppendLine(insertR);
            sb.AppendLine(insertError.Replace(schemaTemplate, schema));
            sb.AppendLine(end);
            sb.AppendLine(orelse);
            sb.AppendLine(begin);
            sb.AppendLine(insertR);
            sb.AppendLine(insertErrorDefault.Replace(schemaTemplate, schema));
            sb.AppendLine(end);
            sb.AppendLine(end);

            // if Session table exists for this schema
            string insertSession = " SELECT TOP 1 '<schema>','Session',s.[Id],s.[CreatedAt] FROM [<schema>].[Session] s ORDER BY s.[Id] DESC";
            string existsSession = " if exists (select * from sys.objects where object_id = OBJECT_ID(N'[<schema>].[Session]') and type in (N'u'))";
            string ifSessionExist = "if exists (select top 1 s.[Id] from [<schema>].[Session] s order by s.[Id] desc)";
            string insertSessionDefault = " values ('<schema>','Session',0,'2000-1-1')";

            sb.AppendLine(existsSession.Replace(schemaTemplate, schema));
            sb.AppendLine(begin);
            sb.AppendLine(ifSessionExist.Replace(schemaTemplate, schema));
            sb.AppendLine(begin);
            sb.AppendLine(insertR);
            sb.AppendLine(insertSession.Replace(schemaTemplate, schema));
            sb.AppendLine(end);
            sb.AppendLine(orelse);
            sb.AppendLine(begin);
            sb.AppendLine(insertR);
            sb.AppendLine(insertSessionDefault.Replace(schemaTemplate, schema));
            sb.AppendLine(end);
            sb.AppendLine(end);

            return sb.ToString();
        }
        private static List<string> GenerateWeeks()
        {
            List<string> list = new List<string>();

            for (int i = 1; i <= 53; i++)
            {
                string schema = i.ToString("000");
                list.Add(schema);
            }
            return list;
        }

        private static List<string> GenerateDays()
        {
            DateTime begin = new DateTime(2017, 6, 4);
            List<string> list = new List<string>();
            for (int i = 0; i < 7; i++)
            {
                DateTime target = begin.AddDays(i);
                string schema = target.DayOfWeek.ToString().Substring(0, 3).ToLower();
                list.Add(schema);
            }
            return list;
        }
        private static List<string> GenerateMonths()
        {
            DateTime target = new DateTime(2017, 1, 15);
            List<string> list = new List<string>();
            for (int i = 0; i < 12; i++)
            {
                string schema = target.AddMonths(i).ToString("MMM").ToLower();
                list.Add(schema);
            }

            return list;
        }






    }
}
