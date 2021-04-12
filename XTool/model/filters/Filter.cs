using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public abstract class Filter
    {

        protected Action Refresh;

        private bool _IsSelected;
        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                _IsSelected = value;
                if (Refresh != null)
                {
                    Refresh.Invoke();
                }
            }
        }

        public abstract string Name { get; }

    }
}
