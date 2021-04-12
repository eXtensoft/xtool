// <copyright company="Recorded Books, Inc" file="SolutionFile.cs">
// Copyright © 2015 All Rights Reserved
// </copyright>

namespace JunkHarness
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Serialization;

    [Serializable]
    public class SolutionFile
    {
        private FileInfo _info;

        [XmlIgnore]
        public int Ordinality { get; set; }

        [XmlAttribute("moniker")]
        public string Moniker { get; set; }

        public DateTime Tds { get; set; }

        public string Name { get; set; }

        public string Filepath {get;set;}

        public string CodeBranch { get; set; }

        public string SolutionVersion { get; set; }

        public string VisualStudioName { get; set; }

        public string VisualStudioVersion { get; set; }

        public string MinimumVersion { get; set; }

        public List<ProjectFile> Projects { get; set; }

        public SolutionFile() { }

        public SolutionFile(FileInfo info, string codeBranch, DateTime tds)
        {
            _info = info;
            Name = info.Name;
            Filepath = info.FullName;
            CodeBranch = codeBranch;
            Tds = tds;
        }

        public static SolutionFile Create(FileInfo info,string codeBranch, DateTime now)
        {

            SolutionFile file = new SolutionFile(info,codeBranch,now);
            file.Initialize();
            return file;
        }

        private void Initialize()
        {
            if (File.Exists(_info.FullName))
            {
                Projects = new List<ProjectFile>();

                var directory = _info.Directory;
                bool b = false;
                var input = File.ReadAllLines(_info.FullName);
                List<string> lines = new List<string>(input.Where(x=> !String.IsNullOrEmpty(x)));
                int i = 0;
                while (!b && i < lines.Count)
                {
                    string line = lines[i];
                    if (!String.IsNullOrWhiteSpace(line))
                    {
                        if (i==0)
                        {
                            SolutionVersion = line;
                        }
                        else if (i == 1)
                        {
                            VisualStudioName = line;
                        }
                        else if (i == 2) 
                        {
                            VisualStudioVersion = line;
                        }
                        else if(i==3)
                        {
                            MinimumVersion = line;
                        }
                        else
                        {
                            if (line.Length >= 10 && line.Substring(0,10).Equals("EndProject"))
                            {
                                
                            }
                            else if (line.Length >= 7 && line.Substring(0,7).Equals("Project"))
                            {

                                string[] directoryRootpath = _info.Directory.FullName.Split(new char[]{'\\'});

                                string[] project = line.Split(new char[] {'=' }, StringSplitOptions.RemoveEmptyEntries);
                                string s = project[0];
                                int start = s.IndexOf('{');

                                string id = s.Substring(start+1, s.IndexOf('}')-start-1);
                                if (!id.Equals("2150E333-8FDC-42A3-9474-1A3956D46DE8",StringComparison.OrdinalIgnoreCase))
                                {
                                    string[] t = project[1].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                    if (t.Length.Equals(3))
                                    {
                                        string t0 = t[0].Trim().Trim('"');
                                        string t1 = t[1].Trim().Trim('"');
                                        string t2 = Between('{', '}', t[2]);
                                        string projectName = t0;
                                        string projectFilepath = t1;
                                        string projectId = t2;

                                        int j = 0;
                                        bool isUpFolder = projectFilepath.Length >= 6 && projectFilepath.Substring(0, 3).Equals(@"..\");
                                        while (isUpFolder && projectFilepath.Substring(0, 3).Equals(@"..\"))
                                        {
                                            j++;
                                            projectFilepath = projectFilepath.Substring(3);
                                        }
                                        string directoryCandidate = String.Empty;
                                        if (j > 0)
                                        {
                                            StringBuilder rootbuilder = new StringBuilder();
                                            for (int x = 0; x < directoryRootpath.Length - j; x++)
                                            {
                                                if (x > 0)
                                                {
                                                    rootbuilder.Append('\\');
                                                }
                                                rootbuilder.Append(directoryRootpath[x]);
                                            }
                                            directoryCandidate = rootbuilder.ToString();
                                        }
                                        else
                                        {
                                            List<string> fullfilepath = new List<string>(directoryRootpath);
                                            //fullfilepath.AddRange(projectFilepath.Split(new char[]{'\\'}, StringSplitOptions.RemoveEmptyEntries));
                                            StringBuilder sbFilepath = new StringBuilder();
                                            for (int m = 0; m < directoryRootpath.Length; m++)
                                            {
                                                if (m > 0)
                                                {
                                                    sbFilepath.Append("\\");
                                                }
                                                sbFilepath.Append(fullfilepath[m]);
                                            }
                                            string ffp = sbFilepath.ToString();

                                            directoryCandidate = ffp;
                                        }
                                        DirectoryInfo di = new DirectoryInfo(directoryCandidate);
                                        if (di.Exists)
                                        {
                                            string projectfilepath = Path.Combine(di.FullName, projectFilepath);
                                            FileInfo projectInfo = new FileInfo(projectfilepath);
                                            if (projectInfo.Exists)
                                            {
                                                ProjectFile file = ProjectFile.Create(projectInfo, CodeBranch, Tds);
                                                Projects.Add(file);
                                            }
                                        }
                                        else
                                        {

                                        }
                                    }
                                }
   
                            }
                            else
                            {
                                b = true;
                            }
                            //Project EndProject
                            //Global
                            //  GlobalSection EndGlobalSection
                            //      = preSolution
                            //      = postSolution

                        }                        
                    }
                    i++;
                }

            }
        }

        private static string Between(char first, char last, string input)
        {
            int i = 0;
            StringBuilder sb = new StringBuilder();
            foreach (var c in input.ToCharArray())
            {
                if (i > 0)
                {
                    i++; 
                }
                if (i > 1 && c.Equals(last))
                {
                    i = 0;
                }
                else if (i==0 && c.Equals(first))
                {
                    i++;
                }

                if (i>1)
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();

        }

    }

}
