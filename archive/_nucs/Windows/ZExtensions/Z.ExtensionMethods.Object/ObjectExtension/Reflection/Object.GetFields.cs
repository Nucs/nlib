// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.Reflection;

namespace Z.ExtensionMethods.Object.Reflection
{
    public static partial class ObjectExtension
    {
        /// <summary>
        ///     A T extension method that returns all the public fields of the current Type.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <returns>
        ///     An array of FieldInfo objects representing all the public fields defined for the current Type. or An empty
        ///     array of type FieldInfo, if no public fields are defined for the current Type.
        /// </returns>
        public static FieldInfo[] GetFields<T>(this T @this)
        {
            return @this.GetType().GetFields();
        }

        /// <summary>
        ///     A T extension method that searches for the fields defined for the current Type, using the specified binding
        ///     constraints.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="bindingAttr">A bitmask comprised of one or more BindingFlags that specify how the search is conducted.</param>
        /// <returns>
        ///     An array of FieldInfo objects representing all fields defined for the current Type that match the specified
        ///     binding constraints. or An empty array of type FieldInfo, if no fields are defined for the current Type, or
        ///     if none of the defined fields match the binding constraints.
        /// </returns>
        public static FieldInfo[] GetFields<T>(this T @this, BindingFlags bindingAttr)
        {
            return @this.GetType().GetFields(bindingAttr);
        }
    }
}