using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XTool
{
    public class PropertyValidator
    {
        #region propertys
        public string PropertyName { get; private set; }
        public Func<string> Executor { get; private set; }
        #endregion

        #region constructors

        public PropertyValidator() { }

        public PropertyValidator(string propertyName, Func<string> executor)
        {
            PropertyName = propertyName;
            Executor = executor;
        }
        #endregion
    }

}
