using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace XTool
{
    public class ApiHeaderViewModel : ViewModel<ApiHeader>
    {
        private ApiParameterSetViewModel _Master;

        #region Display (string)

        /// <summary>
        /// Gets or sets the string value for Display
        /// </summary>
        /// <value> The string value.</value>

        public string Display
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                if (!String.IsNullOrEmpty(Scope))
                {
                    sb.Append(Scope);
                    if (!String.IsNullOrEmpty(Type))
                    {
                        sb.Append(String.Format(" ({0})", Type));
                    }
                }
                else
                {
                    sb.Append(Value);
                }

                return sb.ToString();
                    
            }

        }

        #endregion

        #region Type (string)

        /// <summary>
        /// Gets or sets the string value for Type
        /// </summary>
        /// <value> The string value.</value>

        public string Type
        {
            get { return (String.IsNullOrEmpty(Model.Type)) ? String.Empty : Model.Type; }
            set
            {
                if (Model.Type != value)
                {
                    Model.Type = value;
                    OnPropertyChanged("Type");
                    OnPropertyChanged("Display");
                    OnPropertyChanged("ToDisplay");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region Scope (string)

        /// <summary>
        /// Gets or sets the string value for Scope
        /// </summary>
        /// <value> The string value.</value>

        public string Scope
        {
            get { return (String.IsNullOrEmpty(Model.Scope)) ? String.Empty : Model.Scope; }
            set
            {
                if (Model.Scope != value)
                {
                    Model.Scope = value;
                    OnPropertyChanged("Scope");
                    OnPropertyChanged("Display");
                    OnPropertyChanged("ToDisplay");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region Name (string)

        /// <summary>
        /// Gets or sets the string value for Name
        /// </summary>
        /// <value> The string value.</value>

        public string Name
        {
            get { return (String.IsNullOrEmpty(Model.Name)) ? String.Empty : Model.Name; }
            set
            {
                if (Model.Name != value)
                {
                    Model.Name = value;
                    OnPropertyChanged("Name");
                    OnPropertyChanged("Display");
                    OnPropertyChanged("ToDisplay");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region Value (string)

        /// <summary>
        /// Gets or sets the string value for Value
        /// </summary>
        /// <value> The string value.</value>

        public string Value
        {
            get { return (String.IsNullOrEmpty(Model.Value)) ? String.Empty : Model.Value; }
            set
            {
                if (Model.Value != value)
                {
                    Model.Value = value;
                    OnPropertyChanged("Value");
                    OnPropertyChanged("Display");
                    OnPropertyChanged("ToDisplay");
                    IsDirty = true;
                }
            }
        }

        #endregion



        #region IsTemplate (bool)

        /// <summary>
        /// Gets or sets the bool value for IsTemplate
        /// </summary>
        /// <value> The bool value.</value>

        public bool IsTemplate
        {
            get { return Model.IsTemplate; }
        }


        #region Tag (string)

        /// <summary>
        /// Gets or sets the string value for Tag
        /// </summary>
        /// <value> The string value.</value>

        public string Tag
        {
            get { return (String.IsNullOrEmpty(Model.Tag)) ? String.Empty : Model.Tag; }
            set
            {
                if (Model.Tag != value)
                {
                    Model.Tag = value;
                    OnPropertyChanged("Tag");
                    IsDirty = true;
                }
            }
        }

        #endregion


        #region ToDisplay (string)

        /// <summary>
        /// Gets or sets the string value for Display
        /// </summary>
        /// <value> The string value.</value>

        public string ToDisplay
        {
            get { return String.Format("{0}: {1}", Name, Value); }

        }

        #endregion



        private ICommand _RemoveCommand;
        public ICommand RemoveCommand
        {
            get
            {
                if (_RemoveCommand == null)
                {
                    _RemoveCommand = new RelayCommand(param => Remove());
                }
                return _RemoveCommand;
            }
        }
        private void Remove()
        {
            if (_Master != null)
            {
                _Master.Headers.Remove(this);
            }
        }

        #endregion
        public ApiHeaderViewModel(ApiHeader model)
        {
            Model = model;
        }

        public ApiHeaderViewModel(ApiHeader model, ApiParameterSetViewModel master)
        {
            _Master = master;
            Model = model;
        }

    }
}
