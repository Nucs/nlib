using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

public static class AttributesTools {
        /// <summary>
        /// Gives you all the types that has <param name="attribute"></param> attached to it in the entire <see cref="AppDomain"/>.
        /// </summary>
        /// <param name="attribute">the type of the attribute.</param>
        /// <returns></returns>
        public static IEnumerable<Type> GetAllAttributeHolders(this Type attribute) {
            foreach (var assmb in AppDomain.CurrentDomain.GetAssemblies()) {
                foreach (var type in assmb.GetTypes()) {
                    if (type.GetCustomAttributes(attribute, true).Length > 0) {
                        yield return type;
                    }
                }
            }
        }
    }