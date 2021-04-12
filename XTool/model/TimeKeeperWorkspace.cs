using Chronometrix;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace XTool
{
    public class TimeKeeperWorkspace
    {
        public string LoadedAt { get; set; }
        public string ConnectionString { get; set; }

        public List<Task> Tasks { get; set; }

        public List<Resource> HumanResources { get; set; }

        public List<TimeEntry> Entries { get; set; }


        #region IsInitialized (bool)

        private bool _IsInitialized;

        /// <summary>
        /// Gets or sets the bool value for IsInitialized
        /// </summary>
        /// <value> The bool value.</value>

        public bool IsInitialized
        {
            get { return _IsInitialized; }
            set
            {
                if (_IsInitialized != value)
                {
                    _IsInitialized = value;
                }
            }
        }

        #endregion

        public TimeKeeperWorkspace() { }


        public bool Initialize(SqlServerConnectionInfoViewModel viewModel)
        {
            bool b = false;
            if (!viewModel.IsValidated)
            {
                viewModel.Initialize();
            }
            if (viewModel.IsValidated)
            {
                
                ConnectionString = viewModel.Text;
                b = LoadTimekeeper();
            }
            return b;

        }

        private bool LoadTimekeeper()
        {
            bool b = false;
            // TODO implement data access
            LoadedAt = DateTime.Now.ToShortDateString();
            // use ConnectionString to get Tasks, Resources, Assignments, TimeEntries from database
            // for the current time period (default to whatever today falls into)


            b = true;
            return b;
        }


    }
}
