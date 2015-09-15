// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.Linq;

namespace Z.ExtensionMethods.Object.Reflection
{
    public static partial class ObjectExtension
    {
        /// <summary>
        ///     An object extension method that query if '@this' is attribute defined.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="attributeType">Type of the attribute.</param>
        /// <param name="inherit">true to inherit.</param>
        /// <returns>true if attribute defined, false if not.</returns>
        public static bool IsAttributeDefined(this object @this, Type attributeType, bool inherit) {
            return @this.GetType().IsDefined(attributeType, inherit);
        }

        /// <summary>
        ///     An object extension method that query if '@this' is attribute defined.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="attributeTypeName">Name of the Type of the attribute.</param>
        /// <param name="inherit">true to inherit.</param>
        /// <returns>true if attribute defined, false if not.</returns>
        public static bool IsAttributeDefined(this object @this, string attributeTypeName, bool inherit) {
#if NET_4_5
            return @this.GetType().CustomAttributes.Select(attr => attr.AttributeType.Name).Any(t => t.Equals(attributeTypeName));
#else
            var a = AppDomain.CurrentDomain.GetAssemblies().SelectMany(asm => asm.GetTypes()).FirstOrDefault(t => t.Name.Contains(attributeTypeName));
            return @this.GetType().GetCustomAttribute(a, inherit) != null; //.CustomAttributes.Select(attr => attr.AttributeType.Name).Any(t => t.Equals(attributeTypeName));
#endif
        }

        /// <summary>
        ///     An object extension method that query if '@this' is attribute defined.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="inherit">true to inherit.</param>
        /// <returns>true if attribute defined, false if not.</returns>
        public static bool IsAttributeDefined<T>(this object @this, bool inherit) where T : Attribute
        {
            return @this.GetType().IsDefined(typeof (T), inherit);
        }
    }
}