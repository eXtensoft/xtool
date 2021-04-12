using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public static class ModelConverter
    {

        public static U ConvertTo<U>(object o)
            where U : class, new()
        {
            U u = new U();
            foreach (var map in ResolveProperties<U>(o.GetType()))
            {
                PropertyInfo tinfo = map.Item1;
                PropertyInfo uinfo = map.Item2;
                var sourceValue = tinfo.GetValue(o, null);
                uinfo.SetValue(u, sourceValue, null);
            }
            return u;
        }

        private static IList<Tuple<PropertyInfo,PropertyInfo>> ResolveProperties<U>(Type t)
            where U : class, new()
        {
            return (from x in t.GetProperties()
                    from u in typeof(U).GetProperties()
                    where x.Name == u.Name &&
                    x.CanRead && u.CanRead &&
                    x.PropertyType.IsPublic && u.PropertyType.IsPublic &&
                    x.PropertyType == u.PropertyType &&
                    (
                        (x.PropertyType.IsValueType && u.PropertyType.IsValueType) ||
                        (x.PropertyType == typeof(string) && u.PropertyType == typeof(string))
                    )
                    select new Tuple<PropertyInfo, PropertyInfo>(x, u)
                            ).ToList();

        }


        public static U ConvertTo<T, U>(T t)
            where U : class, new()
            where T : class, new()
        {
            U u = new U();
            foreach (var map in ResolveProperties<T, U>())
            {
                PropertyInfo tinfo = map.Item1;
                PropertyInfo uinfo = map.Item2;
                var sourceValue = tinfo.GetValue(t, null);
                uinfo.SetValue(u, sourceValue, null);
            }
            return u;
        }

        private static IList<Tuple<PropertyInfo, PropertyInfo>> ResolveProperties<T, U>()
            where U : class, new()
            where T : class, new()
        {
            return (from t in typeof(T).GetProperties()
                    from u in typeof(U).GetProperties()
                    where t.Name == u.Name &&
                    t.CanRead && u.CanRead &&
                    t.PropertyType.IsPublic && u.PropertyType.IsPublic &&
                    t.PropertyType == t.PropertyType &&
                   (
                       (t.PropertyType.IsValueType && u.PropertyType.IsValueType) ||
                       (t.PropertyType == typeof(string) && u.PropertyType == typeof(string))
                   )
                    select new Tuple<PropertyInfo, PropertyInfo>(t, u)
                     ).ToList();
        }

    }
}
