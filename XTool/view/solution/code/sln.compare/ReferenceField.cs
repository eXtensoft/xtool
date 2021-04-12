// <copyright company="eXtensoft, LLC" file="ReferenceField.cs">
// Copyright © 2016 All Right Reserved
// </copyright>

namespace JunkHarness
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public sealed class ReferenceField
    {
        #region local members
        
        #endregion

        #region properties
        public string Solution { get; set; }
        public string Project { get; set; }
        public Reference CodeReference { get; set; }
        public string[] AQNS { get; set; }

        public string Name { get; set; }

        public int Ordinality { get; set; }


        #endregion

        #region constructors
        public ReferenceField(Reference reference, string projectName, string solutionName)
        {
            CodeReference = reference;
            AQNS = reference.Name.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            Name = AQNS.Length > 0 ? AQNS[0]: reference.Name;
            Project = projectName;
            Solution = solutionName;

        }

        public ReferenceField(string projectName, string solutionName)
        {
            Project = projectName;
            Solution = solutionName;
        }
        #endregion

        public string ComposeKey(CompareOption option)
        {
            string key = String.Empty;
            switch (option)
            {
                case CompareOption.None:
                    break;
                case CompareOption.Project:
                    key = Project;
                    break;
                case CompareOption.Solution:
                    key = Project;
                    break;
                default:
                    break;
            }
            return key;
        }

        public override string ToString()
        {
            return String.Format("{0} {1} {2}",Solution, Project, Name);
        }

    }

}
