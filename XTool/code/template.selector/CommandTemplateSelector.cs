using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace XTool
{
    public class CommandTemplateSelector : DataTemplateSelector
    {
        public DataTemplate TextTemplate { get; set; }
        public DataTemplate StoredProcedureTemplate { get; set; }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            DataTemplate template = null;
            if (item != null && item is SqlCommandViewModel)
            {
                SqlCommandViewModel vm = (SqlCommandViewModel)item;
                if (vm.CommandType.Equals("Text", StringComparison.OrdinalIgnoreCase))
                {
                    template = TextTemplate;
                }
                else
                {
                    template = StoredProcedureTemplate;
                }
            }

            return template;
        }
    }
}
