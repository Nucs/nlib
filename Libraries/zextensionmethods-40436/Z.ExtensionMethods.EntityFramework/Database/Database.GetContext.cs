// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.Data.Entity;
using System.Reflection;

public static partial class DatabaseExtension
{
    /// <summary>
    ///     A Database extension method that gets a context.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The context.</returns>
    public static T GetContext<T>(this Database @this)
    {
        object internalContext = @this.GetType().GetField("_internalContext", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(@this);
        return (T) internalContext.GetType().BaseType.GetField("_owner", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(internalContext);
    }
}