using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XTool.Inference;

namespace XTool
{
    public class MySqlCommandViewModel : ViewModel<Inference.SqlCommand>
    {
        #region Title (string)

        /// <summary>
        /// Gets or sets the string value for Title
        /// </summary>
        /// <value> The string value.</value>

        public string Title
        {
            get { return (String.IsNullOrEmpty(Model.Title)) ? String.Empty : Model.Title; }
            set
            {
                if (Model.Title != value)
                {
                    Model.Title = value;
                    OnPropertyChanged("Title");
                    IsDirty = true;
                }
            }
        }

        #endregion

        public MySqlCommandViewModel(Inference.SqlCommand model)
        {
            Model = model;
        }
    }
}
