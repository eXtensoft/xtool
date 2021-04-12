// <copyright file="RedisExtensionMethods.cs" company="eXtensible Solutions LLC">
// Copyright © 2015 All Right Reserved
// </copyright>

namespace XTool
{
    using StackExchange.Redis;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;



    public static class RedisExtensionMethods
    {
        public static T Get<T>(this IDatabase cache, string key)
        {
            var found = cache.StringGet(key);
            return Deserialize<T>(found);
        }

        public static object Get(this IDatabase cache, string key)
        {
            var found = cache.StringGet(key);
            return Deserialize<object>(found);
        }

        public static bool Set(this IDatabase cache, string key, object value)
        {
            byte[] bytes = Serialize(value);
            return cache.StringSet(key, bytes);
        }

        public static bool Set(this IDatabase cache, string key, object value, TimeSpan ts)
        {
            byte[] bytes = Serialize(value);
            return cache.StringSet(key, bytes, ts, When.Always, CommandFlags.None);
        }

        static byte[] Serialize(object o)
        {
            if (o == null)
            {
                return null;
            }

            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, o);
                byte[] bytes = stream.ToArray();
                return bytes;
            }
        }

        static T Deserialize<T>(byte[] bytes)
        {
            T t = default(T);
            if (bytes == null)
            {
                return t;
            }

            using (MemoryStream stream = new MemoryStream(bytes))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                t = (T)formatter.Deserialize(stream);
            }
            return t;
        }        

    }
}
