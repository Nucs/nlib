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

    /// <summary>
    /// Finds a method within the assemblies with the given attribute name (full name, 'MyAttribute') and returns the matching methods
    /// </summary>
    public static MethodInfo[] FindAttributedMethod(this IEnumerable<Assembly> assemblies, string attributeName) {
        if (attributeName.EndsWith("Attribute") == false)
            attributeName = attributeName + "Attribute";

        return assemblies.SelectMany(m => m.GetTypes())
                .SelectMany(t => t.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static))
                .Where(m => m.CustomAttributes.Any(attr => attr.AttributeType.Name.Equals(attributeName)))
                .ToArray();
    }

}