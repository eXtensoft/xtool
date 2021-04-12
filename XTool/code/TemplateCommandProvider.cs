using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public class TemplateCommandProvider
    {
        public ConnectionInfoTypeOption ConnectionType { get; set; }

        public ObservableCollection<TemplateCommandViewModel> Commands
        {
            get
            {
                return WorkspaceProvider.Instance.ViewModel.TemplateCommands;
            }
        }
    }
}
