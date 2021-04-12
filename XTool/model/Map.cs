using System;
using System.Collections.Generic;

namespace XTool
{
    public class Mapping : List<Map>
    {
    }

    public class Map
    {
        public string X { get; set; }

        public string Y { get; set; }

        public Map()
        {

        }
        public Map(string x, string y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return String.Format("{0}: {1}", X, Y);
        }
    }
}