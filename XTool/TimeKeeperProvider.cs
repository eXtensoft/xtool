using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public class TimeKeeperProvider
    {
        public static TimeKeeperProvider Instance { get; set; }

        public string ConnectionString { get; set; }
        static TimeKeeperProvider()
        {
            Instance = new TimeKeeperProvider();
        }

        private TimeKeeperWorkspaceViewModel _ViewModel;
        public TimeKeeperWorkspaceViewModel ViewModel
        {
            get
            {
                if (_ViewModel == null)
                {
                    TimeKeeperWorkspace workspace = Bootstrapper.LoadTimekeeper();
                    SqlServerConnectionInfoViewModel vm = null;
                    if (Bootstrapper.TryLoadCyclops(out vm)  && workspace.Initialize(vm))
                    {
                        ConnectionString = vm.Text;
                        _ViewModel = new TimeKeeperWorkspaceViewModel(workspace);
                    }
                }
                return _ViewModel;
            }
        }

        

    }
}
