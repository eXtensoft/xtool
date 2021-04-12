using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    [FlagsAttribute]
    public enum CharTypes
    {
        None = 0,
        Alpha = 1,
        Numeric = 2,
        Point = 4,
        Space = 6,
        Comma = 8,
        Sign = 16,
        Dash = 32,
        Pound = 64,
        Ampersand = 128,
        Asterisk = 256,
        Tab = 512,
        DollarSign = 1024,
        Parenthesis = 2048,
        Bracket = 4096,
        GreaterLesserThan = 8192,
        SingleQuote = 16384,
        DoubleQuote = 32768,
        CrLf = 65536,
        Equals = 131072,
        QuestionMark = 262144,
        At = 524288,
        Special = 1048576
    }
}
