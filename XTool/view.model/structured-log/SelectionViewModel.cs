using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public class SelectionViewModel : ViewModel<Cyclops.Selection>
    {
        public string Title
        {
            get { return Model.Title; }
        }

        public string Tag
        {
            get { return Model.Tag; }
        }

        public int Order
        {
            get { return Model.Order; }
        }

        public SelectionViewModel(Cyclops.Selection model)
        {
            Model = model;
        }

        public SelectionViewModel()
        {

        }
    }
}
