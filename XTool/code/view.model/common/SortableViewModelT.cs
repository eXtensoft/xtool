using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace XTool
{
    public class SortableViewModel<T> : ViewModel<T>
    {
        public Action<object> MoveUp;
        public Action<object> MoveDown;
        public Action<object> Remove;

        public virtual void Initialize(T model)
        {
        }

        #region ItemIndex (int)

        private int _ItemIndex;

        /// <summary>
        /// Gets or sets the int value for ItemIndex
        /// </summary>
        /// <value> The int value.</value>

        public int ItemIndex
        {
            get { return _ItemIndex; }
            set
            {
                if (_ItemIndex != value)
                {
                    _ItemIndex = value;
                }
            }
        }

        #endregion

        #region ItemsCount (int)

        private int _ItemsCount;

        /// <summary>
        /// Gets or sets the int value for ItemsCount
        /// </summary>
        /// <value> The int value.</value>

        public int ItemsCount
        {
            get { return _ItemsCount; }
            set
            {
                if (_ItemsCount != value)
                {
                    _ItemsCount = value;
                }
            }
        }

        #endregion

        #region MoveUpCommand (ICommand)
        private ICommand _MoveUpCommand;

        public ICommand MoveUpCommand
        {
            get
            {
                if (_MoveUpCommand == null)
                {
                    _MoveUpCommand = new RelayCommand(
                        param => TryMoveUp(),
                        param => TryCanMoveUp());
                }
                return _MoveUpCommand;
            }
        }
        #endregion

        #region MoveDownCommand (ICommand)
        private ICommand _MoveDownCommand;

        public ICommand MoveDownCommand
        {
            get
            {
                if (_MoveDownCommand == null)
                {
                    _MoveDownCommand = new RelayCommand(
                        param => TryMoveDown(),
                        param => TryCanMoveDown());
                }
                return _MoveDownCommand;
            }
        }
        #endregion

        #region RemoveCommand

        private ICommand _RemoveCommand;

        public ICommand RemoveCommand
        {
            get
            {
                if (_RemoveCommand == null)
                {
                    _RemoveCommand = new RelayCommand(
                        param => TryRemove());
                }
                return _RemoveCommand;
            }
        }

        #endregion


        public SortableViewModel() { }

        #region helper methods

        private void TryMoveUp()
        {
            if (MoveUp != null)
            {
                MoveUp(this);
            }
        }

        private void TryMoveDown()
        {
            if (MoveDown != null)
            {
                MoveDown(this);
            }
        }

        private void TryRemove()
        {
            if (Remove != null)
            {
                Remove(this);
            }
        }

        private bool TryCanMoveUp()
        {
            return (ItemIndex > 0) ? true : false;
        }

        private bool TryCanMoveDown()
        {
            return (ItemIndex + 1 < ItemsCount) ? true : false;
        }

        #endregion
    }
}
