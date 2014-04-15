// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

public static partial class ObjectExtension
{
    /// <summary>
    ///     A T extension method that query if '@this' is subclass of.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="type">The Type to process.</param>
    /// <returns>true if subclass of, false if not.</returns>
    public static bool IsSubclassOf<T>(this T @this, Type type)
    {
        return @this.GetType().IsSubclassOf(type);
    }
}