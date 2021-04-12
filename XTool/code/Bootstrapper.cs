using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace XTool
{
    public static class Bootstrapper
    {
        public static void Start()
        {
            LocalInitialization();
        }
        private static string GetMySqlSchema()
        {
            return " SELECT TABLE_SCHEMA AS Catalog, '{catalog}' as TableSchema, TABLE_NAME as TableName, COLUMN_NAME as ColumnName," +
                    " ORDINAL_POSITION AS OrdinalPosition, COLUMN_DEFAULT as DefaultValue," +
                    " CASE WHEN IS_NULLABLE = 'YES' THEN 1 ELSE 0 END AS IsNullible, DATA_TYPE AS Datatype," +
                    " CHARACTER_MAXIMUM_LENGTH AS MaxLength, NULL AS IsComputed, " +
                    " NULL AS ColumnLength, NULL AS IsIdentity, NULL AS IsRowGuidColumn," +
                    " CASE WHEN COLUMN_KEY = 'PRI' THEN 1 ELSE 0 END AS IsPrimaryKey," +
                    " CASE WHEN COLUMN_KEY = 'MUL' THEN 1 ELSE 0 END AS IsForeignKey," +
                    " CASE WHEN COLUMN_KEY = 'UNI' THEN 1 ELSE 0 END AS HasUniqueConstraint" +
                    " FROM INFORMATION_SCHEMA.COLUMNS" +
                    " WHERE TABLE_SCHEMA = '{catalog}' and TABLE_NAME like '%{tableName}%'" +
                    " ORDER BY TABLE_NAME, ORDINAL_POSITION";
        }

        internal static bool TryLoadCyclops(out SqlServerConnectionInfoViewModel vm)
        {
            vm = null;
            var found = WorkspaceProvider.Instance.ViewModel.Connections.ToList().Find(x => x.Zone == Inference.ZoneTypeOption.Cyclops && x.ConnectionType == ConnectionInfoTypeOption.SqlServer);
            if (found != null)
            {
                vm = found as SqlServerConnectionInfoViewModel;
            }
            return vm != null;
        }

        public static TimeKeeperWorkspace LoadTimekeeper()
        {
            return new TimeKeeperWorkspace();
        }

        private static void foo()
        {
            List<CommandTemplate> list = new List<CommandTemplate>();
            list.Add( new MySqlCommandTemplate() {Id = Guid.NewGuid().ToString(), Type = TemplateTypeOption.System, Name = "name", ConnectionType = ConnectionInfoTypeOption.MySql, GroupName = "groupName", Description = "desc", Command =  GetMySqlSchema() } );
            list.Add(new SqlServerCommandTemplate() { Id = Guid.NewGuid().ToString(), Type = TemplateTypeOption.System, Name = "name", ConnectionType = ConnectionInfoTypeOption.SqlServer, GroupName = "groupName", Description = "desc", Command = "select * from sys.objects where type = 'u'" });
            list.Add(new SqlServerCommandTemplate() { Id = Guid.NewGuid().ToString(), Type = TemplateTypeOption.System, Name = "name", ConnectionType = ConnectionInfoTypeOption.SqlServer, GroupName = "groupName", Description = "desc", Command =  "select * from sys.objects where type = 'u'" } );
            list.Add(new MongoDbCommandTemplate() { Id = Guid.NewGuid().ToString(), Type = TemplateTypeOption.System, Name = "name", ConnectionType = ConnectionInfoTypeOption.MongoDb, GroupName = "groupName", Description = "desc", Command = "select * from sys.objects where type = 'u'" } );
            list.Add(new MongoDbCommandTemplate() { Id = Guid.NewGuid().ToString(), Type = TemplateTypeOption.System, Name = "name", ConnectionType = ConnectionInfoTypeOption.MongoDb, GroupName = "groupName", Description = "desc", Command = "select * from sys.objects where type = 'u'" } );
            GenericSerializer.WriteGenericList<CommandTemplate>(list, @"c:\temp\templates.xml");
        }

        private static void LocalInitialization()
        {
            InitializeStateMachine();
            InitializeOverlayManager();
            InitializeFormatStrings();
            InitializeImageMaps();
            InitializeAsciiMaps();
            //LoadWorkspace();
        }
        private static void InitializeOverlayManager()
        {
            OverlayManager overlay = new OverlayManager();
            Application.Current.Properties[AppConstants.OverlayManager] = overlay;
            
        }

        private static void InitializeFormatStrings()
        {
            Dictionary<string, string> formats = new Dictionary<string, string>();
            //formats.Add("FormatString.Label", "{0}:");
            //formats.Add("FormatString.Date", "{0:d}");
            //formats.Add("FormatString.Days", "InterventionDayRange.{0}");
            //formats.Add("FormatString.Month", "Month.{0}");
            Application.Current.Properties[AppConstants.FormatStrings] = formats;
 
        }
        private static void InitializeImageMaps()
        {
            List<Tuple<string, string>> list = new List<Tuple<string, string>>();

            list.Add(new Tuple<string, string>("resource/url", "images/content.url.png"));
            list.Add(new Tuple<string, string>("application/vnd.ms-excel", "images/content.msexcel.png"));
            list.Add(new Tuple<string, string>("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "images/content.msexcel.png"));
            list.Add(new Tuple<string, string>("text/xml", "images/content.xml.png"));
            list.Add(new Tuple<string, string>("application/msword", "images/content.msword.png"));
            list.Add(new Tuple<string, string>("application/vnd.openxmlformats-officedocument.wordprocessingml.document", "images/content.msword.png"));
            list.Add(new Tuple<string, string>("application/vnd.visio", "images/content.visio.png"));
            list.Add(new Tuple<string, string>("application/pdf", "images/content.help.png"));
            list.Add(new Tuple<string, string>("application/vnd.openxmlformats-officedocument.presentationml.presentation", "images/content.powerpoint.png"));
            list.Add(new Tuple<string, string>("text/snippet", "images/content.text.png"));
            list.Add(new Tuple<string, string>("text/plain", "images/content.snippet.png"));
            list.Add(new Tuple<string, string>("text/credential", "images/content.snippet.png"));
            list.Add(new Tuple<string, string>("image/bmp", "images/content.image.png"));
            list.Add(new Tuple<string, string>("image/gif", "images/content.image.png"));
            list.Add(new Tuple<string, string>("image/png", "images/content.image.png"));
            list.Add(new Tuple<string, string>("image/tiff", "images/content.image.png"));
            list.Add(new Tuple<string, string>("image/jpeg", "images/content.image.png"));
            list.Add(new Tuple<string, string>("Recent", "images/circle.orange.png"));
            list.Add(new Tuple<string, string>("None", "images/circle.gray.png"));
            list.Add(new Tuple<string, string>("Favorite", "images/circle.green.png"));
            list.Add(new Tuple<string, string>("System", "images/circle.blue.png"));
            list.Add(new Tuple<string, string>("application/pdf", "images/content.pdf.png"));
            list.Add(new Tuple<string, string>("MongoDb.Driver", "images/info.mongodb.png"));
            list.Add(new Tuple<string, string>("StackExchange.Redis", "images/info.redis.png"));
            list.Add(new Tuple<string, string>("MySql.Data.MySqlClient", "images/info.mysql.png"));
            list.Add(new Tuple<string, string>("System.Data.SqlClient", "images/info.sqlserver.png"));


            //list.Add(new Tuple<string, string>(AppConstants.STATUSACTIVE, "images/status.active.png"));
            //list.Add(new Tuple<string, string>(AppConstants.STATUSINACTIVE, "images/status.inactive.png"));

            //list.Add(new Tuple<string, string>(AppConstants.TRUE, "images/check.green.png"));
            //list.Add(new Tuple<string, string>(AppConstants.FALSE, "images/x.red.png"));


            Application.Current.Properties[AppConstants.ImageMaps] = list;
        }
        private static void InitializeStateMachine()
        {
            StateMachine machine = new StateMachine()
            {
                BeginState = ActivityStateOption.LoggedOff.ToString(),
                EndState = ActivityStateOption.LoggedOff.ToString()
            };

            machine.States.Add(new State() { Name = ActivityStateOption.LoggedOff.ToString() });
            machine.States.Add(new State() { Name = ActivityStateOption.Authenticated.ToString() });
            machine.States.Add(new State() { Name = ActivityStateOption.Authorized.ToString() });
            machine.States.Add(new State() { Name = ActivityStateOption.Unauthorized.ToString() });
            machine.States.Add(new State() { Name = ActivityStateOption.Error.ToString() });
            machine.States.Add(new State() { Name = ActivityStateOption.TemplateCommands.ToString() });
            machine.States.Add(new State() { Name = ActivityStateOption.TimeEntry.ToString() });
            machine.States.Add(new State() { Name = ActivityStateOption.Logging.ToString() });


            machine.Transitions.Add(new Transition() { Name = TransitionTypeOption.Login.ToString(), OriginState = ActivityStateOption.LoggedOff.ToString(), DestinationState = ActivityStateOption.Authenticated.ToString() });
            machine.Transitions.Add(new Transition() { Name = TransitionTypeOption.UnAuthorize.ToString(), OriginState = ActivityStateOption.Authenticated.ToString(), DestinationState = ActivityStateOption.Unauthorized.ToString() });
            machine.Transitions.Add(new Transition() { Name = TransitionTypeOption.Authorize.ToString(), OriginState = ActivityStateOption.Authenticated.ToString(), DestinationState = ActivityStateOption.Authorized.ToString() });
            machine.Transitions.Add(new Transition() { Name = TransitionTypeOption.Logoff.ToString(), OriginState = ActivityStateOption.Authorized.ToString(), DestinationState = ActivityStateOption.LoggedOff.ToString() });
            machine.Transitions.Add(new Transition() { Name = TransitionTypeOption.Logoff.ToString(), OriginState = ActivityStateOption.Authenticated.ToString(), DestinationState = ActivityStateOption.LoggedOff.ToString() });
            machine.Transitions.Add(new Transition() { Name = TransitionTypeOption.Logoff.ToString(), OriginState = ActivityStateOption.Unauthorized.ToString(), DestinationState = ActivityStateOption.LoggedOff.ToString() });
            machine.Transitions.Add(new Transition() { Name = TransitionTypeOption.OnError.ToString(), OriginState = ActivityStateOption.Authenticated.ToString(), DestinationState = ActivityStateOption.Error.ToString() });
            machine.Transitions.Add(new Transition() { Name = TransitionTypeOption.OnError.ToString(), OriginState = ActivityStateOption.Authorized.ToString(), DestinationState = ActivityStateOption.Error.ToString() });
            machine.Transitions.Add(new Transition() { Name = TransitionTypeOption.Logoff.ToString(), OriginState = ActivityStateOption.Error.ToString(), DestinationState = ActivityStateOption.LoggedOff.ToString() });
            machine.Transitions.Add(new Transition() { Name = TransitionTypeOption.ToggleTemplateCommands.ToString(), OriginState = ActivityStateOption.Authorized.ToString(), DestinationState = ActivityStateOption.TemplateCommands.ToString() });
            machine.Transitions.Add(new Transition() { Name = TransitionTypeOption.ToggleTemplateCommands.ToString(), OriginState = ActivityStateOption.TemplateCommands.ToString(), DestinationState = ActivityStateOption.Authorized.ToString() });
            machine.Transitions.Add(new Transition() { Name = TransitionTypeOption.ToggleTimeEntry.ToString(), OriginState = ActivityStateOption.Authorized.ToString(), DestinationState = ActivityStateOption.TimeEntry.ToString() });
            machine.Transitions.Add(new Transition() { Name = TransitionTypeOption.ToggleTimeEntry.ToString(), OriginState = ActivityStateOption.TimeEntry.ToString(), DestinationState = ActivityStateOption.Authorized.ToString() });
            machine.Transitions.Add(new Transition() { Name = TransitionTypeOption.ToggleLogging.ToString(), OriginState = ActivityStateOption.Authorized.ToString(), DestinationState = ActivityStateOption.Logging.ToString() });
            machine.Transitions.Add(new Transition() { Name = TransitionTypeOption.ToggleLogging.ToString(), OriginState = ActivityStateOption.Logging.ToString(), DestinationState = ActivityStateOption.Authorized.ToString() });

            var mgr = new StateManager() { Machine = machine };
            Application.Current.Properties[AppConstants.StateManager] = mgr;
        }

        public static XToolWorkspace LoadWorkspace()
        {
            XToolWorkspace workspace = null;

            if (System.IO.File.Exists(AppConstants.WorkspaceFilename))
            {
                try
                {
                    workspace = GenericSerializer.ReadGenericItem<XToolWorkspace>(AppConstants.WorkspaceFilename);
                }
                catch
                {
                }
            }
            if (workspace == null)
            {
                workspace = GenerateDefaultWorkspace();
            }

            if (workspace.TemplateCommands == null)
            {
                workspace.TemplateCommands = new List<CommandTemplate>();
            }
            List<CommandTemplate> templates = GenericSerializer.StringToGenericList<CommandTemplate>(Resources.CommandTemplates);
            templates.ForEach(workspace.TemplateCommands.Add);
            return workspace;
        }

        private static XToolWorkspace GenerateDefaultWorkspace()
        {
            XToolWorkspace workspace = new XToolWorkspace() { Connections = new List<ConnectionInfo>() };
            workspace.Connections.Add( new Inference.SqlServerConnectionInfo() 
            { 
                Server = "(local)", 
                Catalog = "catalog", 
                ConnectionType = ConnectionInfoTypeOption.SqlServer, 
                Zone = Inference.ZoneTypeOption.Local, 
                Name = "Local Catalog", 
                ProviderName = "System.Data.SqlClient", 
                IntegratedSecurity = true, 
                Commands = new List<Inference.SqlCommand>() });

            return workspace;
        }

        public static ConnectionInfoViewModel GenerateConnectionInfoViewModel(ConnectionInfoTypeOption option)
        {
            ConnectionInfoViewModel vm = null;
            var model = GenerateConnection(option);

            switch (option)
            {
                case ConnectionInfoTypeOption.None:
                    vm = new ConnectionInfoViewModel(model);
                    break;
                case ConnectionInfoTypeOption.SqlServer:
                    vm = new XTool.SqlServerConnectionInfoViewModel((Inference.SqlServerConnectionInfo)model);        
                    break;
                case ConnectionInfoTypeOption.MongoDb:
                    break;
                case ConnectionInfoTypeOption.Redis:
                    break;
                case ConnectionInfoTypeOption.MySql:
                    break;
                default:
                    break;
            }

            return vm;
        }

        public static ConnectionInfo GenerateConnection(ConnectionInfoTypeOption option)
        {
            ConnectionInfo info = null;
            switch (option)
            {
                case ConnectionInfoTypeOption.None:
                    info = new ConnectionInfo();
                    info.ProviderName = "None";
                    break;
                case ConnectionInfoTypeOption.SqlServer:
                    info = new XTool.Inference.SqlServerConnectionInfo()
                    {
                         IntegratedSecurity = true, 
                         Commands = new List<Inference.SqlCommand>(),
                         ProviderName = "System.Data.SqlClient"
                    };
                    break;
                case ConnectionInfoTypeOption.MongoDb:
                    break;
                case ConnectionInfoTypeOption.Redis:
                    break;
                case ConnectionInfoTypeOption.MySql:
                    break;
                default:
                    break;
            }
            info.Name = String.Empty;
            info.ConnectionType = option;
            info.Server = "(local)";
            return info;
        }

        private static void InitializeAsciiMaps()
        {
            AsciiMapCollection c = new AsciiMapCollection();
            List<AsciiMap> list = GenericSerializer.StringToGenericList<AsciiMap>(Resources.AsciiMap);
            foreach (var item in list)
            {
                c.Add(item);
            }
            Application.Current.Properties[AppConstants.AsciiMaps] = c;
        }

        public static List<AsciiMap> GetAsciiMaps()
        {
            return GenericSerializer.StringToGenericList<AsciiMap>(Resources.AsciiMap);
        }
        
        private static List<Inference.SqlServerConnectionInfo> GenerateDefaultConnection()
        {
            List<Inference.SqlServerConnectionInfo> list = new List<Inference.SqlServerConnectionInfo>();
            Inference.SqlServerConnectionInfo info = new Inference.SqlServerConnectionInfo();
            info.Catalog = "AdventureWorks";
            //info.ServerName = "(local)";
            info.Text = "";
            //info.Title = "(local) AdventureWorks";
            info.Commands = new List<Inference.SqlCommand>();
            info.Commands.Add(new Inference.SqlCommand() { CommandType = "Text", Text = "select * from sysobjects where type = 'u'" });
            list.Add(info);
            return list;
        }
        
        

    }
}
