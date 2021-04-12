// <copyright company="Recorded Books, Inc" file="ProjectFile.cs">
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
    using System.Xml;
    using System.Xml.Linq;

    [Serializable]
    public class ProjectFile
    {
        private FileInfo _info;

        public string ProjectId { get; set; }

        public ProjectTypeOption ProjectType { get; set; }

        public string ProjectTypeId { get; set; }

        public string Name { get; set; }

        public string RelativeFilepath { get; set; }

        public string AbsoluteFilepath { get; set; }

        public DateTime Tds { get; set; }

        public string CodeBranch { get; set; }

        
        public List<Reference> References { get; set; }

        public ProjectFile() { }

        public ProjectFile(FileInfo info)
        {
            _info = info;
        }

        public static ProjectFile Create(FileInfo info, string codeBranch, DateTime tds)
        {

            ProjectFile file = new ProjectFile(info) { Tds = tds, CodeBranch = codeBranch };
            
            file.Initialize();
            return file;
        
        }

        private void Initialize()
        {

            if (File.Exists(_info.FullName))
            {
                References = new List<Reference>();

                AbsoluteFilepath = _info.FullName;
                Name = _info.Name;
                var input = File.ReadAllText(_info.FullName);
                XDocument xdoc = XDocument.Load(_info.FullName);
                XElement root = xdoc.Root;
                XNamespace ns = "http://schemas.microsoft.com/developer/msbuild/2003";
                int i = 0;

                foreach (var el in root.Descendants(ns + "ItemGroup"))
                {
                    i++;
                    foreach (var libraryRef in el.Descendants(ns + "Reference"))
                    {
                        string name = libraryRef.Attribute("Include").Value;

                        References.Add(new ClassLibraryReference() { Name = name });
                        i++;
                    }
                    foreach (var projectRef in el.Descendants(ns + "ProjectReference"))
                    {
                        string path = projectRef.Attribute("Include").Value;
                        string id = projectRef.Element(ns + "Project").Value.TrimStart('{').TrimEnd('}');
                        string name = projectRef.Element(ns + "Name").Value;

                        References.Add(new ProjectReference() { Name = name, Filepath = path, ProjectId = id });
                    }
                }



            }
        }









    }

}
