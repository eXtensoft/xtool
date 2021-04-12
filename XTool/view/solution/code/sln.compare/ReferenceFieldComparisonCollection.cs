// <copyright company="eXtensoft, LLC" file="ReferenceFieldComparisonCollection.cs">
// Copyright © 2016 All Right Reserved
// </copyright>

namespace JunkHarness
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Linq;
    using System.Text;

    public sealed class ReferenceFieldComparisonCollection : KeyedCollection<string,ReferenceFieldComparison>
    {
        private List<SolutionFile> _Solutions;

        private List<string> _Keys = new List<string>();

        private DataTable dt;

        public ReferenceFieldComparisonCollection(IEnumerable<SolutionFile> solutions)
        {
            _Solutions = solutions.ToList();
        }
        protected override string GetKeyForItem(ReferenceFieldComparison item)
        {
            return item.ComposeKey();
        }

        public DataTable ToDataTable(CompareOption compareBy)
        {
            Process(compareBy);
            return dt;
        }

        public void ProcessSolutions()
        {
            int max = _Solutions.Count;
            dt = new DataTable() { TableName = "SolutionComparison" };
            DataColumn dc1 = new DataColumn("Project", typeof(String));
            dt.Columns.Add(dc1);

            Dictionary<string, int> monikers = new Dictionary<string, int>();
            foreach (SolutionFile file in _Solutions)
            {
                if (!monikers.ContainsKey(file.Moniker))
                {
                    monikers.Add(file.Moniker, 0);
                }
                monikers[file.Moniker]++;
            }

            Dictionary<int, string> d = new Dictionary<int, string>();
            HashSet<string> hs = new HashSet<string>();
            int i = 0;
            foreach (SolutionFile solution in _Solutions)
            {
                string moniker = String.Empty;
                if (monikers[solution.Moniker] == 1)
                {
                    moniker = String.Format("{0} {1} ({2})", solution.Name, solution.CodeBranch, solution.Tds.ToShortDateString()).Replace('.','-');// solution.Moniker;
                }
                else
                {
                    moniker = String.Format("{0} ({1})", solution.Moniker, solution.Tds.ToShortDateString());
                }

                if (!hs.Add(moniker))
                {
                    moniker += " " + Guid.NewGuid().ToString().Substring(0, 4);
                }
                DataColumn dc = new DataColumn(moniker.Replace('.', ' '), typeof(String));
                dt.Columns.Add(dc);

                solution.Ordinality = i++;
                foreach (ReferenceField field in Build(solution, CompareOption.Solution))
                {
                    string key = field.ComposeKey(CompareOption.Solution).ToLower();
                    if (!this.Contains(key))
                    {
                        _Keys.Add(key);
                        this.Add(new ReferenceFieldComparison(max, CompareOption.None, key));

                    }
                    this[key].Add(field);
                }
            }
            _Keys.Sort((x, y) => x.CompareTo(y));


            foreach (string key in _Keys)
            {
                if (this.Contains(key))
                {
                    var found = this[key];
                    DataRow r = dt.NewRow();
                    string display = String.Empty;
                    bool b = true;

                    for (int j = 0; j < max; j++)
                    {
                        var f = found.Items.Find(x => x.Ordinality.Equals(j));
                        if (f != null)
                        {
                            if (String.IsNullOrEmpty(display))
                            {
                                display = f.Name;
                            }
                            if (b && !display.Equals(f.Name))
                            {
                                b = false;
                            }

                            r[j + 1] = f.Name;
                        }
                    }


            //        string[] t = key.Split('.');
                    r[0] = found.GroupName;
            //        r[1] = found.FieldName;




                    dt.Rows.Add(r);

                }
            }





            //int i = 1;


        }

        public void ProcessProjects()
        {
            int max = _Solutions.Count;
            dt = new DataTable() { TableName = "ProjectComparison" };

            Dictionary<string, int> monikers = new Dictionary<string, int>();
            foreach (SolutionFile file in _Solutions)
            {
                if (!monikers.ContainsKey(file.Moniker))
                {
                    monikers.Add(file.Moniker, 0);
                }
                monikers[file.Moniker]++;
            }
            HashSet<string> hs = new HashSet<string>();
            int i = 0;
            foreach (SolutionFile solution in _Solutions)
            {
                string moniker = String.Empty;
                if (monikers[solution.Moniker] == 1)
                {
                    moniker = solution.Moniker;
                }
                else
                {
                    moniker = String.Format("{0} ({1})", solution.Moniker, solution.Tds.ToShortDateString());
                }

                if (!hs.Add(moniker))
                {
                    moniker += " " + Guid.NewGuid().ToString().Substring(0, 4);
                }
                DataColumn dc = new DataColumn(moniker.Replace('.', ' '), typeof(String));
                dt.Columns.Add(dc);

                solution.Ordinality = i++;
                foreach (ReferenceField field in Build(solution, CompareOption.Project))
                {
                    string key = field.ComposeKey(CompareOption.Project).ToLower();
                    if (!this.Contains(key))
                    {
                        _Keys.Add(key);
                        this.Add(new ReferenceFieldComparison(max, CompareOption.Project, key));

                    }
                    this[key].Add(field);
                }

            }
            _Keys.Sort((x, y) => x.CompareTo(y));


            ClassLibraryCollection coderefs = new ClassLibraryCollection();

            foreach (var key in _Keys)
            {
                ReferenceFieldComparison found = this[key];
                foreach (var item in found.Items)
                {
                    var codeReference = item.Name;
                    if (!coderefs.Contains(codeReference))
                    {
                        CodeRefResolver resolver = new CodeRefResolver(codeReference);
                        var cl = new ClassLibrary(codeReference);
                        cl.CodeDomain = resolver.Option;
                        cl.CodeGroup = resolver.GroupName;
                        coderefs.Add(cl);
                    }
                    coderefs[codeReference].Add(item.Project);
                }
            }
            var sortedcoderefs = coderefs.ToList().OrderBy(x => x.Namespace);
            StringBuilder sb = new StringBuilder();
            foreach (var item in sortedcoderefs)
            {
                sb.AppendLine(String.Format("{0} : {1}", item.Namespace,item.Items.Count()));
            }

            string s = sb.ToString();
            List<CodeRefViewModel> pr = new List<CodeRefViewModel>();
            var groups = coderefs.ToLookup(x => x.CodeGroup, x => x);
            foreach (var item in groups)
            {
                CodeRefViewModel vm = new CodeRefViewModel()
                {
                    GroupName = item.Key,
                    Name = String.Empty,
                    Projects = new List<ProjectRefViewModel>()
                };
                int k = 0;
                foreach (ClassLibrary proj in item)
                {
                    if (k==0)
                    {
                        vm.Name = proj.CodeDomain.ToString();
                    }
                    vm.Projects.Add(new ProjectRefViewModel() { Name = proj.Namespace, Count = proj.Items.Count });
                }
                pr.Add(vm);
            }
            int j = s.Length;

        }

        private CodeDomainOption ResolveDomain(string codeReference)
        {
            throw new NotImplementedException();
        }

        private string ResolveGroup(string codeReference)
        {
            throw new NotImplementedException();
        }

        public void Process(CompareOption compareBy)
        {
            if (compareBy.Equals(CompareOption.Solution))
            {
                ProcessSolutions();
            }
            else if (compareBy.Equals(CompareOption.Project))
            {
                ProcessProjects();
            }
            //int max = _Solutions.Count();

            //CompareOption comparison = compareBy;
            //dt = new DataTable() { TableName = "Comparison" };

            //DataColumn dc1 = new DataColumn("Project", typeof(String));
            //DataColumn dc2 = new DataColumn("ProjectRefernce", typeof(string));
            //DataColumn dc3 = new DataColumn("RB", typeof(bool));
            //DataColumn dc4 = new DataColumn("XF", typeof(bool));

            //dt.Columns.Add(dc1);
            //dt.Columns.Add(dc2);
            //dt.Columns.Add(dc3);
            //dt.Columns.Add(dc4);

            //int i = 1;

            //Dictionary<string, int> monikers = new Dictionary<string, int>();
            //foreach (SolutionFile file in _Solutions)
            //{
            //    if (!monikers.ContainsKey(file.Moniker))
            //    {
            //        monikers.Add(file.Moniker, 0);
            //    }
            //    monikers[file.Moniker]++;
            //}

            //Dictionary<int, string> d = new Dictionary<int, string>();
            //HashSet<string> hs = new HashSet<string>();

            //foreach (SolutionFile solution in _Solutions)
            //{
            //    string moniker = String.Empty;
            //    if (monikers[solution.Moniker]==1)
            //    {
            //        moniker = solution.Moniker;
            //    }
            //    else
            //    {
            //        moniker = String.Format("{0} ({1})", solution.Moniker, solution.Tds.ToShortDateString());
            //    }

            //    if (!hs.Add(moniker))
            //    {
            //        moniker += " " + Guid.NewGuid().ToString().Substring(0, 4);
            //    }
            //    DataColumn dc = new DataColumn(moniker.Replace('.', ' '), typeof(String));
            //    dt.Columns.Add(dc);

            //    solution.Ordinality = i++;
            //    foreach (ReferenceField field in Build(solution,comparison))
            //    {
            //        string key = field.ComposeKey(comparison).ToLower();
            //        if (!this.Contains(key))
            //        {
            //            _Keys.Add(key);
            //            this.Add(new ReferenceFieldComparison(max, CompareOption.None, key));

            //        }
            //        this[key].Add(field);
            //    }

            //}
            //_Keys.Sort((x, y) => x.CompareTo(y));


            //foreach (string key in _Keys)
            //{
            //    if (this.Contains(key))
            //    {
            //        var found = this[key];
            //        DataRow r = dt.NewRow();
            //        string display = String.Empty;
            //        bool b = true;

            //        for (int j = 1; j <= max; j++)
            //        {
            //            var f = found.Items.Find(x => x.Ordinality.Equals(j));
            //            if (f != null)
            //            {
            //                if (String.IsNullOrEmpty(display))
            //                {
            //                    display = f.Name;
            //                }
            //                if (b && !display.Equals(f.Name))
            //                {
            //                    b = false;
            //                }

            //                r[j + 3] = f.Name;
            //            }

            //        }
            //        string[] t = key.Split('.');
            //        r[0] = found.GroupName;
            //        r[1] = found.FieldName;

                    


            //        dt.Rows.Add(r);
            //    }



            //}

            }



        private static IEnumerable<ReferenceField> Build(SolutionFile solution, CompareOption comparison)
        {
            List<ReferenceField> list = new List<ReferenceField>();
            foreach (ProjectFile file in solution.Projects)
            {
                if (comparison.Equals(CompareOption.Solution))
                {
                    list.Add( new ReferenceField(file.Name, solution.Name) { Ordinality = solution.Ordinality, Name = file.Name });
                }
                else if (comparison.Equals(CompareOption.Project))
                {
                    foreach (Reference reference in file.References)
                    {
                        string projectName = file.Name; // blah.csproj
                        string[] aqns = reference.Name.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        ReferenceField f = new ReferenceField(reference,file.Name,solution.Name);
                        f.Ordinality = solution.Ordinality;
                        list.Add(f);
                    }                      
                }

            }


            return list;

        }


    }

}
