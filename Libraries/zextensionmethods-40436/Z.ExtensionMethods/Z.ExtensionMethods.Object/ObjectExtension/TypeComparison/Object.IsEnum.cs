// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).


public static partial class ObjectExtension
{
    /// <summary>
    ///     A T extension method that query if '@this' is enum.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <returns>true if enum, false if not.</returns>
    public static bool IsEnum<T>(this T @this)
    {
        return @this.GetType().IsEnum;
    }
}