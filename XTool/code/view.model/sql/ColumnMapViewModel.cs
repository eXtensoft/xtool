using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XTool
{
    public class ColumnMapViewModel : SortableViewModel<ColumnMap>
    {

        public ColumnMapViewModel()
        {

        }
        public override void Initialize(ColumnMap model)
        {
            Model = model;
        }
    }
}
