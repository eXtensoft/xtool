using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public class TimeKeeperWorkspaceViewModel : ViewModel<TimeKeeperWorkspace>
    {
        

        public string LoadedAt
        {
            get { return Model.LoadedAt; }
        }

        public TimeKeeperWorkspaceViewModel(TimeKeeperWorkspace model)
        {
            Model = model;
        }





    }
}
