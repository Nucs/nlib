using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Reflection;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace nucs.SystemCore.Reflection {
    public static class ReflectionTools {
/*        public static List<PropertyInfo> GetProperties<T>(BindingFlags flags) {
            return typeof(T).GetProperties(flags).ToList();
        } 

        public static List<PropertyInfo> GetProperties(object instance, BindingFlags flags) {
            if (instance == null)
                return null;
            return instance.GetType().GetProperties(flags).ToList();
        }*/

        public static Dictionary<string, object> GetNotDefaultValues(object obj) { //todo test it, might not work, cant recall..
#if NET_4_5
            return (obj.GetType().GetProperties().Where(prop => GetDefaultValue(prop.PropertyType).Equals(prop.GetValue(obj)) == false)).ToDictionary(p => p.Name, p => p.GetValue(obj));
#else
            return (obj.GetType().GetProperties().Where(prop => GetDefaultValue(prop.PropertyType).Equals(prop.GetValue(obj, null)) == false)).ToDictionary(p => p.Name, p => p.GetValue(obj, null));
#endif
        } 

        public static object GetDefaultValue(Type type) {  //todo test it, might not work, cant recall..
            if (type.IsValueType) {
                return Activator.CreateInstance(type);
            }
            return null;
        }
    }


    /// <summary>
    /// Reference Article http://www.codeproject.com/KB/tips/SerializedObjectCloner.aspx
    /// Provides a method for performing a deep copy of an object.
    /// Binary Serialization is used to perform the copy.
    /// </summary>
    public static class ObjectCopier {
        /// <summary>
        /// Perform a deep Copy of the object.
        /// </summary>
        /// <typeparam name="T">The type of object being copied.</typeparam>
        /// <param name="source">The object instance to copy.</param>
        /// <returns>The copied object.</returns>
        public static T Clone<T>(this T source) where T : ISerializable {
            if (!typeof(T).IsSerializable) {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            // Don't serialize a null object, simply return the default for that object
            if (Object.ReferenceEquals(source, null)) {
                return default(T);
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream) {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }
    }

}
