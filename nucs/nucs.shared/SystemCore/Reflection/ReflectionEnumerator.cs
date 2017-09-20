using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace nucs.SystemCore.Reflection
{
    public static class ReflectiveEnumerator
    {
        static ReflectiveEnumerator() { }
        /// <summary>
        ///     Will enumerate all assemblies for classes that inheriet T and return an instance of them.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="constructorArgs"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetEnumerableOfType<T>(params object[] constructorArgs) where T : class
        {
            List<T> objects = new List<T>();
            foreach (Type type in
                Assembly.GetAssembly(typeof(T)).GetTypes()
                    .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T))))
            {
                objects.Add((T)Activator.CreateInstance(type, constructorArgs));
            }
            objects.Sort();
            return objects;
        }

        /// <summary>
        ///     Will enumerate all assemblies for classes that inheriet T and return an instance of them.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="constructorArgs"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetEnumerableOfInterface<T>(params object[] constructorArgs) where T : class
        {
            var type = typeof(T);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract).Select(Activator.CreateInstance).Cast<T>();
            return types;
        }
    }
}
