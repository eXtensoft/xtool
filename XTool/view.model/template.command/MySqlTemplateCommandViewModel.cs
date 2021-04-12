using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public class MySqlTemplateCommandViewModel : TemplateCommandViewModel
    {

        public new MySqlCommandTemplate Model { get; set; }

        public MySqlTemplateCommandViewModel(MySqlCommandTemplate model)
        {
            Model = model;
            base.Model = model;
        }
    }
}
