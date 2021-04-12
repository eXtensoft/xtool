using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JunkHarness
{
    public sealed class CodeRefResolver
    {
        private string _Namespace;



        public CodeDomainOption Option { get; set; }
        public string GroupName { get; set; }

        public Dictionary<string, Tuple<CodeDomainOption, string>> d = new Dictionary<string, Tuple<CodeDomainOption, string>>(StringComparer.OrdinalIgnoreCase){

            {"microsoft",new Tuple<CodeDomainOption,string>(CodeDomainOption.Microsoft,".net")},
            {"presentationcore",new Tuple<CodeDomainOption,string>(CodeDomainOption.Microsoft,".net")},
            {"presentationframework",new Tuple<CodeDomainOption,string>(CodeDomainOption.Microsoft,".net")},
            {"webactivatorex",new Tuple<CodeDomainOption,string>(CodeDomainOption.Microsoft,".net")},
            {"webapicontrib",new Tuple<CodeDomainOption,string>(CodeDomainOption.Microsoft,".net")},
            {"webgrease",new Tuple<CodeDomainOption,string>(CodeDomainOption.Microsoft,".net")},
            {"windowsbase",new Tuple<CodeDomainOption,string>(CodeDomainOption.Microsoft,".net")},
            {"xf",new Tuple<CodeDomainOption,string>(CodeDomainOption.Framework,"eXtensoft")},
            {"rb",new Tuple<CodeDomainOption,string>(CodeDomainOption.Company,"RecordedBooks")},
            {"mongodb", new Tuple<CodeDomainOption,string>(CodeDomainOption.Framework,"MongoDb")},
            {"newtonsoft", new Tuple<CodeDomainOption,string>(CodeDomainOption.Microsoft,".net")},
            {"nest", new Tuple<CodeDomainOption,string>(CodeDomainOption.Company,"Nest")},
            {"memcachedproviders", new Tuple<CodeDomainOption,string>(CodeDomainOption.Company,"memcached")},
            {"log4net", new Tuple<CodeDomainOption,string>(CodeDomainOption.Company,"log4net")},
            {"elasticsearch", new Tuple<CodeDomainOption,string>(CodeDomainOption.Company,"ElasticSearch")},
            {"awssdk", new Tuple<CodeDomainOption,string>(CodeDomainOption.Company,"Amazon")},
            {"system", new Tuple<CodeDomainOption,string>(CodeDomainOption.Microsoft,".net")},
            //{"", new Tuple<CodeDomainOption,string>(CodeDomainOption.None,"")},
            //{"", new Tuple<CodeDomainOption,string>(CodeDomainOption.None,"")},
            //{"", new Tuple<CodeDomainOption,string>(CodeDomainOption.None,"")},
            //{"", new Tuple<CodeDomainOption,string>(CodeDomainOption.None,"")},
            //{"", new Tuple<CodeDomainOption,string>(CodeDomainOption.None,"")},
            //{"", new Tuple<CodeDomainOption,string>(CodeDomainOption.None,"")},
            //{"", new Tuple<CodeDomainOption,string>(CodeDomainOption.None,"")},
            //{"", new Tuple<CodeDomainOption,string>(CodeDomainOption.None,"")},
            //{"", new Tuple<CodeDomainOption,string>(CodeDomainOption.None,"")},
            //{"", new Tuple<CodeDomainOption,string>(CodeDomainOption.None,"")},
            //{"", new Tuple<CodeDomainOption,string>(CodeDomainOption.None,"")},
            //{"", new Tuple<CodeDomainOption,string>(CodeDomainOption.None,"")},
        };

        

        public CodeRefResolver( string nameSpace)
        {
            bool b = false;
            _Namespace = nameSpace;
            if (!String.IsNullOrWhiteSpace(nameSpace))
            {
                string[] t = nameSpace.Split(new char[]{'.'}, StringSplitOptions.RemoveEmptyEntries);
                if (t.Length > 0 &&d.ContainsKey(t[0]))
                {
                    Option = d[t[0]].Item1;
                    GroupName = d[t[0]].Item2;
                    b = true;
                }

            }
            if (!b)
            {
                Option = CodeDomainOption.Custom;
                GroupName = "unknown";
            }
        }
    }
}
