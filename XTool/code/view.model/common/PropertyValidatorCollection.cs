using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace XTool
{
    public class PropertyValidatorCollection : KeyedCollection<string, PropertyValidator>
    {
        protected override string GetKeyForItem(PropertyValidator item)
        {
            return item.PropertyName;
        }

        public void AddValidator(string propertyName, Func<string> executor)
        {
            Add(new PropertyValidator(propertyName, executor));
        }
    }
}
