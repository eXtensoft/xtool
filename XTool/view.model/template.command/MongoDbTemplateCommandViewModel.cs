using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public class MongoDbTemplateCommandViewModel : TemplateCommandViewModel
    {

        public new MongoDbCommandTemplate Model { get; set; }
        public MongoDbTemplateCommandViewModel(MongoDbCommandTemplate model)
        {
            Model = model;
            base.Model = model;
        }
    }
}
