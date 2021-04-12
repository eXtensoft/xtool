using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public class SqlServerTemplateCommandViewModel : TemplateCommandViewModel
    {

        public new  SqlServerCommandTemplate Model { get; set; }
        public SqlServerTemplateCommandViewModel(SqlServerCommandTemplate model)
        {
            Model = model;
            base.Model = model;
        }
    }
}
