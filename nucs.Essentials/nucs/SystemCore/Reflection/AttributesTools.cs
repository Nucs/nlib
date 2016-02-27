using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

public static class AttributesTools {
    /// <summary>
    /// Gives you all the types that has <param name="attribute"></param> attached to it in the entire <see cref="AppDomain"/>.
    /// </summary>
    /// <param name="attribute">the type of the attribute.</param>
    /// <returns></returns>
    public static IEnumerable<Type> GetAllAttributeHolders(this Type attribute) {
        return from assmb in AppDomain.CurrentDomain.GetAssemblies() from type in gettypes(assmb) where type.GetCustomAttributes(attribute, true).Length > 0 select type;
    }

/*
    /// <summary>
    /// Gives you all the MethodInfo that has <param name="attribute"></param> attached to it in the entire <see cref="AppDomain"/>.
    /// </summary>
    /// <param name="attribute">the type of the attribute.</param>
    /// <returns></returns>
    public static IEnumerable<Type> GetMethodsWithAttribute(this Type attribute) {
        //return from assmb in AppDomain.CurrentDomain.GetAssemblies() from type in gettypes(assmb) where type.GetCustomAttributes(attribute, true).Length > 0 select type;

        var methods = AppDomain.CurrentDomain.GetAssemblies().SelectMany(gettypes).SelectMany(t=>t.GetMethods());

        List<KeyValuePair<String, MethodInfo>> items =
            new List<KeyValuePair<string, MethodInfo>>();

        foreach (MethodInfo method in methods)
        {
            var token = Attribute.GetCustomAttribute(method,
                attribute, false);
            if (token == null)
                continue;

            items.Add(new KeyValuePair<String, MethodInfo>(
                token., method));

        }


    }
*/

    private static Type[] gettypes(Assembly assmb) {
            return !File.Exists(AssemblyDirectory(assmb)) ? new Type[0] : assmb.GetTypes();
    }

    private static string AssemblyDirectory(Assembly asm) { 
        string codeBase = asm.CodeBase;
        UriBuilder uri = new UriBuilder(codeBase);
        string path = Uri.UnescapeDataString(uri.Path);
        return Path.GetDirectoryName(path);
    }

#if NET_4_5
    /// <summary>
    /// Finds a method within the assemblies with the given attribute name (full name, 'MyAttribute') and returns the matching methods
    /// </summary>
    public static MethodInfo[] FindAttributedMethod(this IEnumerable<Assembly> assemblies, string attributeName) {
        if (attributeName.EndsWith("Attribute") == false)
            attributeName = attributeName + "Attribute";

        return assemblies.SelectMany(m => m.GetTypes()).AsParallel()
                .SelectMany(t => t.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static))
                .Where(m => m.CustomAttributes.Any(attr => attr.AttributeType.Name.Equals(attributeName)))
                .ToArray();
    }
#endif
}