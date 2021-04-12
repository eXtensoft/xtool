using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public class JPropertyViewModel : JTokenViewModel
    {

        #region Value (object)


        /// <summary>
        /// Gets or sets the object value for Value
        /// </summary>
        /// <value> The object value.</value>
        private object _Value;
        public object Value
        {
            get { return _Value; }
            set
            {
                if (_Value != value)
                {
                    _Value = value;
                    OnPropertyChanged("Value");
                }
            }
        }

        #endregion

        public override string ToString()
        {
            return _Value.ToString();
        }
    }
}
