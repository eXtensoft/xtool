using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public static class Indent
    {
        public static string Format(int level,string format, params object[] data)
        {
            string unindentedText = String.Format(format, data);
            return ApplyIndentation(level, unindentedText);
        } 
        public static string Append(int level, string data)
        {
            return ApplyIndentation(level,data);
        }

        private static string ApplyIndentation(int level, string unindentedText)
        {
            string s = unindentedText.Trim();
            int spacesPerIndentLevel = 4;
            int spacesCount = (level > 0) ? spacesPerIndentLevel * level : 0;
            int totalLength = s.Length + spacesCount;
            string final = s.PadLeft(totalLength);
            return final;
        }
    }
}
