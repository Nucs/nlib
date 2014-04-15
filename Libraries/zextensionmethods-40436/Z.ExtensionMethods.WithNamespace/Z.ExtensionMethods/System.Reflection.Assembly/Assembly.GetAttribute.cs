// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.Reflection;

namespace Z.ExtensionMethods
{
    public static partial class AssemblyExtension
    {
        /// <summary>
        ///     An Assembly extension method that gets an attribute.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The attribute.</returns>
        public static T GetAttribute<T>(this Assembly @this) where T : Attribute
        {
            object[] configAttributes = Attribute.GetCustomAttributes(@this, typeof (T), false);

            if (configAttributes != null && configAttributes.Length > 0)
            {
                return (T) configAttributes[0];
            }

            return null;
        }
    }
}