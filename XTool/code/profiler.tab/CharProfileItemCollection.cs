// <copyright file="CharProfileItemCollection.cs" company="eXtensible Solutions LLC">
// Copyright © 2015 All Right Reserved
// </copyright>

namespace XTool
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;


    [Serializable]
    [DataContract(Namespace = "http://eXtensibleSolutions/schemas/2016/04")]
    public sealed class CharProfileItemCollection : KeyedCollection<int,CharProfileItem>
    {
        public string Summary { get; set; }

        protected override int GetKeyForItem(CharProfileItem item)
        {
            return item.Key;
        }

        public void Profile(string input)
        {
            if (!String.IsNullOrWhiteSpace(input))
            {
                foreach (var item in input.ToCharArray())
                {
                    int i = (int)item;
                    if (!Contains(i))
                    {
                        Add(new CharProfileItem() { Key = i, Count = 0 });
                    }
                    this[i].Count++;
                }
            }
        }

        //public CharTypes Profile(string input)
        //{

        //    CharTypes ctypes = CharTypes.None;
        //    int i = 0;
        //    if (!String.IsNullOrWhiteSpace(input))
        //    {
        //        foreach (var item in input.ToCharArray())
        //        {
        //            i = (int)item;
        //            if (!Contains(i))
        //            {
        //                Add(new CharProfileItem() { Key = i, Count = 0 });
        //            }
        //            this[i].Count++;
        //        }
        //    }
        //    return ctypes;
        //}

        public void Summarize()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in this.OrderByDescending(x=>x.Count))
            {
                sb.AppendLine(item.ToString());
            }
            Summary = sb.ToString();
        }
    }
}
