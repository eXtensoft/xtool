using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    //[KnownType(SqlServerTemplateCommandViewModel)]
    //[KnownType(MySqlTemplateCommandViewModel)]
    //[KnownType(MongoDbTemplateCommandViewModel)]
    public class TemplateCommandViewModel : ViewModel<CommandTemplate>
    {


        #region Id (string)

        /// <summary>
        /// Gets or sets the string value for Id
        /// </summary>
        /// <value> The string value.</value>

        public string Id
        {
            get { return (String.IsNullOrEmpty(Model.Id)) ? String.Empty : Model.Id; }
            set
            {
                if (Model.Id != value)
                {
                    Model.Id = value;
                    OnPropertyChanged("Id");
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
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region GroupName (string)

        /// <summary>
        /// Gets or sets the string value for GroupName
        /// </summary>
        /// <value> The string value.</value>

        public string GroupName
        {
            get { return (String.IsNullOrEmpty(Model.GroupName)) ? String.Empty : Model.GroupName; }
            set
            {
                if (Model.GroupName != value)
                {
                    Model.GroupName = value;
                    OnPropertyChanged("GroupName");
                    IsDirty = true;
                }
            }
        }

        #endregion


        #region ConnectionType (ConnectionInfoTypeOption)


        /// <summary>
        /// Gets or sets the ConnectionInfoTypeOption value for ConnectionType
        /// </summary>
        /// <value> The ConnectionInfoTypeOption value.</value>

        public ConnectionInfoTypeOption ConnectionType
        {
            get { return Model.ConnectionType; }
            set
            {
                if (Model.ConnectionType != value)
                {
                    Id = Guid.NewGuid().ToString();
                    Model.ConnectionType = value;
                    OnPropertyChanged("ConnectionType");
                    IsDirty = true;
                    WorkspaceProvider.Instance.ConvertTo(this, value);
                }
            }
        }

        #endregion



        #region Type (TemplateTypeOption)


        /// <summary>
        /// Gets or sets the TemplateTypeOption value for Type
        /// </summary>
        /// <value> The TemplateTypeOption value.</value>

        public TemplateTypeOption Type
        {
            get { return Model.Type; }
            set
            {
                if (Model.Type != value)
                {
                    Model.Type = value;
                    Id = Guid.NewGuid().ToString();
                    OnPropertyChanged("Type");
                    IsDirty = true;
                   
                }
            }
        }

        #endregion

        #region Description (string)

        /// <summary>
        /// Gets or sets the string value for Description
        /// </summary>
        /// <value> The string value.</value>

        public string Description
        {
            get { return (String.IsNullOrEmpty(Model.Description)) ? String.Empty : Model.Description; }
            set
            {
                if (Model.Description != value)
                {
                    Model.Description = value;
                    OnPropertyChanged("Description");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region Command (string)

        /// <summary>
        /// Gets or sets the string value for Command
        /// </summary>
        /// <value> The string value.</value>

        public string Command
        {
            get { return (String.IsNullOrEmpty(Model.Command)) ? String.Empty : Model.Command; }
            set
            {
                if (Model.Command != value)
                {
                    Model.Command = value;
                    OnPropertyChanged("Command");
                    IsDirty = true;
                }
            }
        }

        #endregion

        public TemplateCommandViewModel() { }

        public TemplateCommandViewModel(CommandTemplate model)
        {
            Model = model;
        }
    }
}
