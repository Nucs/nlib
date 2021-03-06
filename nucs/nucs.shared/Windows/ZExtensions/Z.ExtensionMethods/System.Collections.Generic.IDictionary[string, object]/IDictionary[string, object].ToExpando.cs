// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).
#if NET45|| NET40

using System.Collections.Generic;
using System.Dynamic;

public static partial class IDictionaryExtension {


    /// <summary>
    ///     An IDictionary&lt;string,object&gt; extension method that converts the @this to an expando.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>@this as an ExpandoObject.</returns>
    public static ExpandoObject ToExpando(this IDictionary<string, object> @this)
    {
        var expando = new ExpandoObject();
        var expandoDict = (IDictionary<string, object>) expando;

        foreach (var item in @this)
        {
            if (item.Value is IDictionary<string, object>)
            {
                var d = (IDictionary<string, object>) item.Value;
                expandoDict.Add(item.Key, d.ToExpando());
            }
            else
            {
                expandoDict.Add(item);
            }
        }

        return expando;
    }
}

#endif
