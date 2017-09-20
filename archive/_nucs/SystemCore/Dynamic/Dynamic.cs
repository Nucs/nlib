#if NET4_5|| NET4_0

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using nucs.SystemCore.Reflection;
using nucs.Collections.Extensions;

namespace nucs.SystemCore.Dynamic {
    public static class Dynamic {
        public static IDictionary<string, object> ToDictionary(dynamic dynamicObject) {
            if (((object)dynamicObject).GetType().Name.Contains("ExpandoObject")) 
                return (IDictionary<string, object>) dynamicObject;
            if (Dynamic.IsAnonymousProperties(dynamicObject))
                return dynamicObject.AsDynamic;
            if (dynamicObject is ExpandoObject)
                return (ExpandoObject)dynamicObject;
            var obj = (object)dynamicObject;
            if (Dynamic.IsObjectAnonymous(dynamicObject))
#if NET4_5
                return obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).ToDictionary(p => p.Name, p => p.GetValue(obj));
#else
                return obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).ToDictionary(p => p.Name, p => p.GetValue(obj,null));
#endif

            if (obj.GetType().GetInterfaces().Any(i => i.Name.Contains("IDictionary`2"))) 
                try {
                    return (IDictionary<string, object>) dynamicObject;
                } catch {}
            throw new ArgumentException("Could not parse dynamic object to a dictionary", "dynamicObject");
        }

        public static void Map<T>(ExpandoObject source, T desitinion) where T : class {
            Mapper<T>.Map(source, desitinion);
        }

        public static IDictionary<string, object> FilterByType<T>(dynamic instance, bool includeFields = false) {
            return FilterByType<T>((IDictionary<string, object>)ToDictionary(instance), includeFields);
        }

        public static IDictionary<string, object> FilterByType<T>(IDictionary<string, object> instance, bool includeFields = false) {
            var propNames = typeof(T).GetProperties().Select(p => p.Name.ToLowerInvariant()).ToList();
            if (includeFields)
                propNames.AddRange(typeof(T).GetFields().Select(p=> p.Name.ToLowerInvariant()));

            return instance.Where(kv => propNames.Any(p => p == kv.Key.ToLowerInvariant())).ToDictionary();
        } 

        public static ExpandoObject ToExpandoObject(dynamic dic) {
            return ToExpandoObject(ToDictionary(dic));
        }
        public static ExpandoObject ToExpandoObject(IDictionary<string, object> dic) {
            if (dic is ExpandoObject)
                return dic as ExpandoObject;
            var eo = new ExpandoObject();
            var eoColl = (ICollection<KeyValuePair<string, object>>)eo;
            foreach (var kvp in dic)
                eoColl.Add(kvp);
            return eo;
        }

        public static dynamic ToAnonymous(this IDictionary<string, object> exp) {
            return ToAnonymous(ToExpandoObject(exp));
        }

        public static dynamic ToAnonymous(this ExpandoObject exp) {
            return exp;
        }

        public static bool IsAnonymousType(Type type) {
            if (type == null)
                return false;

            // HACK: The only way to detect anonymous types right now.
            return Attribute.IsDefined(type, typeof(CompilerGeneratedAttribute), false)
                && type.IsGenericType && type.Name.Contains("AnonymousType")
                && (type.Name.StartsWith("<>") || type.Name.StartsWith("VB$"))
                && (type.Attributes & TypeAttributes.NotPublic) == TypeAttributes.NotPublic;
        }

        public static bool IsObjectAnonymous(object obj) {
            return obj.GetType().Name.Contains("AnonymousType");
        }

        public static bool IsAnonymousProperties(object obj) {
            return obj.GetType().Name.Contains("AnonymousProperties");
        }

    }


    // By using a generic class we can take advantage
    // of the fact that .NET will create a new generic type
    // for each type T. This allows us to avoid creating
    // a dictionary of Dictionary<string, PropertyInfo>
    // for each type T. We also avoid the need for the 
    // lock statement with every call to Map.
    public static class Mapper<T>
        // We can only use reference types
        where T : class {
        private static readonly Dictionary<string, PropertyInfo> _propertyMap;

        static Mapper() {
            // At this point we can convert each
            // property name to lower case so we avoid 
            // creating a new string more than once.
            _propertyMap =
                typeof (T)
                    .GetProperties()
                    .ToDictionary(
                        p => p.Name.ToLower(),
                        p => p
                    );
        }

        public static void Map(ExpandoObject source, T destination) {
            // Might as well take care of null references early.
            if (source == null)
                throw new ArgumentNullException("source");
            if (destination == null)
                throw new ArgumentNullException("destination");

            // By iterating the KeyValuePair<string, object> of
            // source we can avoid manually searching the keys of
            // source as we see in your original code.
            foreach (var kv in source) {
                PropertyInfo p;
                if (_propertyMap.TryGetValue(kv.Key.ToLowerInvariant(), out p)) {
                    var propType = p.PropertyType;
                    /*if (kv.Value == null) {
                        if (!propType.IsByRef && propType.Name != "Nullable`1") {
                            // Throw if type is a value type 
                            // but not Nullable<>
                            throw new ArgumentException("not nullable");
                        }
                    }*/
                    if (kv.Value.GetType() != propType) {
                        // You could make this a bit less strict 
                        // but I don't recommend it.
                        //throw new ArgumentException("type mismatch");
                        continue;
                    }
                    p.SetValue(destination, kv.Value, null);
                }
            }
        }
    }
}
#endif