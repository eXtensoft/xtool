using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace XTool
{
    public class WorkspaceProvider
    {
        public DateTimeOffset Start { get; set;}

        public static WorkspaceProvider Instance { get; set; }

        public Stack<Message> Messages { get; set; }

        public OverlayManager Overlay
        {
            get
            {
                return Application.Current.Properties[AppConstants.OverlayManager] as OverlayManager;
            }
        }

        static WorkspaceProvider()
        {
            Instance = new WorkspaceProvider() { };
        }

        private WorkspaceProvider()
        {
            Start = DateTime.Now;
            Messages = new Stack<Message>();
        }

        private XToolWorkspaceViewModel _ViewModel;
        public XToolWorkspaceViewModel ViewModel
        {
            get
            {
                if (_ViewModel == null)
                {
                    XToolWorkspace workspace = Bootstrapper.LoadWorkspace();
                    _ViewModel = new XToolWorkspaceViewModel(workspace);                   
                }
                return _ViewModel;
            }
        }

        #region ICommand implementations

        public bool Save()
        {
            bool b = false;
            try
            {
                XToolWorkspace workspace = GenericSerializer.CloneItem<XToolWorkspace>(ViewModel.Model);
                workspace.PrepareToSave();
                GenericSerializer.WriteGenericItem<XToolWorkspace>(workspace, AppConstants.WorkspaceFilename);
                b = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return b;
        }

        public bool CanSave()
        {
            return ViewModel != null;
        }

        public void AddConnectionInfo()
        {
            //ConnectionInfo connection = Bootstrapper.GenerateConnection(ConnectionInfoTypeOption.None);
            var vm = Bootstrapper.GenerateConnectionInfoViewModel( ConnectionInfoTypeOption.None);
            ViewModel.Connections.Add(vm);
            
            //ViewModel.Model.Connections.Add(connection);
        }

        public bool CanAddConnectionInfo()
        {
            return true;
        }

        public void RemoveConnectionInfo(ConnectionInfoViewModel vm)
        {
            if (ViewModel.Connections.Contains(vm))
            {
                ViewModel.Connections.Remove(vm);
            }
        }
        #endregion



        public void ConvertTo(ConnectionInfoViewModel connectionViewModel, ConnectionInfoTypeOption option)
        {

            var index = ViewModel.Connections.IndexOf(connectionViewModel);
            
            if (index > -1)
            {
                var converted = ConvertTo(connectionViewModel.Model, option);
                ViewModel.Connections.RemoveAt(index);
                ViewModel.Connections.Insert(index, converted);
            }

        }

        private ConnectionInfoViewModel ConvertTo(ConnectionInfo connectionInfo, ConnectionInfoTypeOption option)
        {
            ConnectionInfoViewModel vm = null;

            // var from = connectionInfo.GetType();

            switch (option)
            {
                case ConnectionInfoTypeOption.None:
                    ConnectionInfo info = ModelConverter.ConvertTo<ConnectionInfo>(connectionInfo);
                    vm = new ConnectionInfoViewModel(info);
                    break;
                case ConnectionInfoTypeOption.SqlServer:
                    XTool.Inference.SqlServerConnectionInfo sqlInfo = ModelConverter.ConvertTo<XTool.Inference.SqlServerConnectionInfo>(connectionInfo);
                    vm = new SqlServerConnectionInfoViewModel(sqlInfo);
                    break;
                case ConnectionInfoTypeOption.MongoDb:
                    MongoDbConnectionInfo mongoInfo = ModelConverter.ConvertTo<MongoDbConnectionInfo>(connectionInfo);
                    vm = new MongoDbConnectionInfoViewModel(mongoInfo);
                    break;
                case ConnectionInfoTypeOption.Redis:
                    RedisConnectionInfo redisInfo = ModelConverter.ConvertTo<RedisConnectionInfo>(connectionInfo);
                    vm = new RedisConnectionInfoViewModel(redisInfo);
                    break;
                case ConnectionInfoTypeOption.MySql:
                    MySqlConnectionInfo mySqlInfo = ModelConverter.ConvertTo<MySqlConnectionInfo>(connectionInfo);
                    vm = new MySqlConnectionInfoViewModel(mySqlInfo);
                    break;
                case ConnectionInfoTypeOption.Neo4j:
                    Neo4jConnectionInfo neoInfo = ModelConverter.ConvertTo<Neo4jConnectionInfo>(connectionInfo);
                    vm = new Neo4jConnectionInfoViewModel(neoInfo);
                    break;
                case ConnectionInfoTypeOption.File:
                    FileConnectionInfo fileInfo = ModelConverter.ConvertTo<FileConnectionInfo>(connectionInfo);
                    vm = new FileConnectionInfoViewModel(fileInfo);
                    break;
                case ConnectionInfoTypeOption.Excel:
                    ExcelConnectionInfo xlInfo = ModelConverter.ConvertTo<ExcelConnectionInfo>(connectionInfo);
                    vm = new ExcelConnectionInfoViewModel(xlInfo);
                    break;
                case ConnectionInfoTypeOption.Xml:
                    XmlConnectionInfo xmlInfo = ModelConverter.ConvertTo<XmlConnectionInfo>(connectionInfo);
                    vm = new XmlConnectionInfoViewModel(xmlInfo);
                    break;
                case ConnectionInfoTypeOption.Json:
                    JsonConnectionInfo jsonInfo = ModelConverter.ConvertTo<JsonConnectionInfo>(connectionInfo);
                    vm = new JsonConnectionInfoViewModel(jsonInfo);
                    break;
                case ConnectionInfoTypeOption.Api:
                    ApiConnectionInfo apiInfo = ModelConverter.ConvertTo<ApiConnectionInfo>(connectionInfo);
                    vm = new ApiConnectionInfoViewModel(apiInfo);
                    break;
                default:
                    break;
            }
            return vm;
        }


        public void ConvertTo(TemplateCommandViewModel viewModel, ConnectionInfoTypeOption option)
        {
            var index = ViewModel.TemplateCommands.IndexOf(viewModel);
            if (index > -1)
            {
                var converted = ConvertTemplateTo(viewModel.Model, option);
                ViewModel.TemplateCommands.RemoveAt(index);
                ViewModel.TemplateCommands.Insert(index, converted);
            }
        }


        public TemplateCommandViewModel ConvertTemplateTo(CommandTemplate model, ConnectionInfoTypeOption option)
        {
            TemplateCommandViewModel vm = null;
            switch (option)
            {
                case ConnectionInfoTypeOption.None:
                    break;
                case ConnectionInfoTypeOption.SqlServer:
                    SqlServerCommandTemplate sqlserverTemplate = ModelConverter.ConvertTo<SqlServerCommandTemplate>(model);
                    vm = new SqlServerTemplateCommandViewModel(sqlserverTemplate);
                    break;
                case ConnectionInfoTypeOption.MongoDb:
                    MongoDbCommandTemplate mongoTemplate = ModelConverter.ConvertTo<MongoDbCommandTemplate>(model);
                    vm = new MongoDbTemplateCommandViewModel(mongoTemplate);
                    break;
                case ConnectionInfoTypeOption.Redis:
                    break;
                case ConnectionInfoTypeOption.MySql:
                    MySqlCommandTemplate mysqlTemplate = ModelConverter.ConvertTo<MySqlCommandTemplate>(model);
                    vm = new MySqlTemplateCommandViewModel(mysqlTemplate);
                    break;
                case ConnectionInfoTypeOption.Neo4j:
                    break;
                case ConnectionInfoTypeOption.File:
                    break;
                case ConnectionInfoTypeOption.Excel:
                    break;
                case ConnectionInfoTypeOption.Xml:
                    break;
                case ConnectionInfoTypeOption.Json:
                    break;
                case ConnectionInfoTypeOption.Api:
                    break;
                default:
                    break;
            }
            return vm;
        }

    }
}
