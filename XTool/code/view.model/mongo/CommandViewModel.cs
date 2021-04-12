using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public class CommandViewModel : ViewModel<MongoDbCommand>
    {
        private MongoDbCommandTypeOption _CommandType = MongoDbCommandTypeOption.Find;
        public MongoDbCommandTypeOption CommandType
        {
            get
            {
                return _CommandType;
            }
            set
            {
                _CommandType = value;
                OnPropertyChanged("CommandType");
                OnPropertyChanged("IsFind");
                OnPropertyChanged("IsGroup");
                OnPropertyChanged("IsAggregate");
                OnPropertyChanged("MongoCommand");
            }
        }

        #region Collection (string)

        /// <summary>
        /// Gets or sets the string value for Collection
        /// </summary>
        /// <value> The string value.</value>

        public string Collection
        {
            get { return (String.IsNullOrEmpty(Model.Collection)) ? String.Empty : Model.Collection; }
            set
            {
                if (Model.Collection != value)
                {
                    Model.Collection = value;
                    OnPropertyChanged("Collection");
                    IsDirty = true;
                }
            }
        }

        #endregion

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

        #region Count (int)

        /// <summary>
        /// Gets or sets the int value for Count
        /// </summary>
        /// <value> The int value.</value>

        public int Count
        {
            get { return Model.Count; }
            set
            {
                if (Model.Count != value)
                {
                    Model.Count = value;
                    OnPropertyChanged("Count");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region Text (string)

        /// <summary>
        /// Gets or sets the string value for Text
        /// </summary>
        /// <value> The string value.</value>

        public string Text
        {
            get { return (String.IsNullOrEmpty(Model.Text)) ? String.Empty : Model.Text; }
            set
            {
                if (Model.Text != value)
                {
                    Model.Text = value;
                    OnPropertyChanged("Text");
                    IsDirty = true;
                }
            }
        }

        #endregion

        public string MongoCommand
        {
            get
            {
                return String.Format("db.{0}(", CommandType);
            }
        }

        public bool IsFind
        {
            get
            {
                return _CommandType.Equals(MongoDbCommandTypeOption.Find);
            }
            set
            {
                CommandType = MongoDbCommandTypeOption.Find;

            }
        }

        public bool IsGroup
        {
            get { return _CommandType.Equals(MongoDbCommandTypeOption.Group); }
            set { CommandType = MongoDbCommandTypeOption.Group; }
        }

        public bool IsAggregate
        {
            get { return _CommandType.Equals(MongoDbCommandTypeOption.Aggregate); }
            set { CommandType = MongoDbCommandTypeOption.Aggregate; }
        }

        public CommandViewModel(MongoDbCommand model)
        {
            Model = model;
        }
    }
}
