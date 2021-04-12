using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public class JArrayViewModel : JTokenViewModel
    {
        //#region Name (string)

        ///// <summary>
        ///// Gets or sets the string value for Name
        ///// </summary>
        ///// <value> The string value.</value>
        //private string _Name;
        //public string Name
        //{
        //    get
        //    {

        //        string s = (String.IsNullOrEmpty(_Name)) ? String.Empty : _Name;
        //        if (!String.IsNullOrWhiteSpace(_Name))
        //        {
        //            if (_Name.EndsWith("]"))
        //            {
        //                s = _Name.Substring( _Name.LastIndexOf('['));
        //            }
        //        }
        //        return s;
        //    }
        //    set
        //    {
        //        if (_Name != value)
        //        {
        //            _Name = value;
        //            OnPropertyChanged("Name");
        //        }
        //    }
        //}

        //#endregion

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
