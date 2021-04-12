//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.ComponentModel;

//namespace XTool
//{
//    public class SelectableGroupingViewModel : INotifyPropertyChanged
//    {
//        public string Title { get; set; }

//        #region IsSelected (bool)

//        /// <summary>
//        /// Gets or sets the bool value for IsSelected
//        /// </summary>
//        /// <value> The bool value.</value>
//        private bool _IsSelected;
//        public bool IsSelected
//        {
//            get { return _IsSelected; }
//            set
//            {
//                if (_IsSelected != value)
//                {
//                    _IsSelected = value;
//                    OnPropertyChanged("IsSelected");
//                }
//            }
//        }

//        #endregion


//        #region INotifyPropertyChanged
//        public event PropertyChangedEventHandler PropertyChanged;

//        public virtual void OnPropertyChanged(string propertyName)
//        {
//            if (PropertyChanged != null)
//            {
//                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
//            }
//        }

//        #endregion
//    }
//}
