using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public class FieldTypeProfileItemCollection : KeyedCollection<FieldTypeOption,FieldTypeProfileItem>
    {
        public int MaxPrecision { get; set; }

        protected override FieldTypeOption GetKeyForItem(FieldTypeProfileItem item)
        {
            return item.FieldType;
        }

        public void Profile(string input)
        {



        }


        internal void Profile(string input, string characters)
        {
            FieldTypeOption option = FieldTypeOption.String;

            if (characters.Equals("Numeric", StringComparison.OrdinalIgnoreCase))
            {
                decimal d;
                int i;

                if (Decimal.TryParse(input, out d))
                {

                    option = FieldTypeOption.Decimal;
                }  
                else if(Int32.TryParse(input, out i))
                {
                    option = FieldTypeOption.Integer;
                }
            }
            else
            {
                DateTime d;
                bool b;
                if (DateTime.TryParse(input, out d))
                {
                    if (d.Hour.Equals(0) && d.Minute.Equals(0) && d.Second.Equals(0))
                    {
                        option = FieldTypeOption.Date;
                    }
                    else
                    {
                        option = FieldTypeOption.Datetime;
                    }
                }
                else if (Boolean.TryParse(input,out b))
                {
                    option = FieldTypeOption.Boolean;
                }
            }
            if (!this.Contains(option))
            {
                Add(new FieldTypeProfileItem() { FieldType = option, Count = 0 });
            }
            this[option].Count++;
        }

        
    }
}
