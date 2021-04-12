using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public class FileInfoViewModel : ViewModel<FileInfo>
    {
        #region Name (string)

        /// <summary>
        /// Gets or sets the string value for Name
        /// </summary>
        /// <value> The string value.</value>

        public string Name
        {
            get { return (String.IsNullOrEmpty(Model.Name)) ? String.Empty : Model.Name; }
        }

        #endregion

        #region FullName (string)

        /// <summary>
        /// Gets or sets the string value for FullName
        /// </summary>
        /// <value> The string value.</value>

        public string FullName
        {
            get { return (String.IsNullOrEmpty(Model.FullName)) ? String.Empty : Model.FullName; }
        }

        #endregion

        #region IsSelected (bool)

        /// <summary>
        /// Gets or sets the bool value for IsSelected
        /// </summary>
        /// <value> The bool value.</value>
        private bool _IsSelected;
        public new bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                if (_IsSelected != value)
                {
                    _IsSelected = value;
                    OnPropertyChanged("IsSelected");
                    IsDirty = true;
                    if (_OnSelected != null)
                    {
                        _OnSelected.Invoke(Model);
                    }
                }
            }
        }

        #endregion

        #region Folder (string)

        /// <summary>
        /// Gets or sets the string value for Folder
        /// </summary>
        /// <value> The string value.</value>

        public string Folder
        {
            get { return Model.Directory.ToString(); }

        }

        #endregion

        private Action<FileInfo> _OnSelected;

        public FileInfoViewModel() { }

        public FileInfoViewModel(FileInfo model)
        {
            Model = model;
            
        }

        public FileInfoViewModel(FileInfo model, Action<FileInfo> onSelected)
        {
            Model = model;
            _OnSelected = onSelected;
        }
    }
}
