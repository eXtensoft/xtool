using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public class AsciiMap
    {
        public int Int { get; set; }
        public int Oct { get; set; }
        public string Hex { get; set; }
        public string Bin { get; set; }
        public string Symbol { get; set; }
        public string Html { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Printable { get; set; }
        public bool IsAlpha { get; set; }
        public bool IsNumeric { get; set; }
        public bool IsExtendedChar { get; set; }
        public bool ResolvesToAlpha { get; set; }
        public bool ResolvesToNumeric { get; set; }
        public int ResolvesTo { get; set; }
        public bool ResolvesToBoolean { get; set; }
        public bool ResolvesToDate { get; set; }
        public bool ResolvesToTime { get; set; }

        public AsciiMap()
        {

        }

        public AsciiMap(string data)
        {
            string[] t = data.Split('\t');
            if (t.Length == 11)
            {
                Int = Convert.ToInt32(t[0]);
                Oct = Convert.ToInt32(t[1]);
                Hex = t[2];
                Bin = t[3];
                Symbol = t[4];
                Html = t[5];
                Name = t[6];
                Description = t[7];
                Printable = Boolean.Parse(t[8]);
                IsAlpha = Boolean.Parse(t[9]);
                IsNumeric = Boolean.Parse(t[10]);
            }
        }
    }
}
