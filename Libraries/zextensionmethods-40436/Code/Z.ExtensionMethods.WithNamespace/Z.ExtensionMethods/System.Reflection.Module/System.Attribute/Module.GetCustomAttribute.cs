// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.Reflection;

namespace Z.ExtensionMethods
{
    public static partial class ModuleExtension
    {
        /// <summary>
        ///     Retrieves a custom attribute applied to a module. Parameters specify the module, and the type of the custom
        ///     attribute to search for.
        /// </summary>
        /// <param name="element">An object derived from the  class that describes a portable executable file.</param>
        /// <param name="attributeType">The type, or a base type, of the custom attribute to search for.</param>
        /// <returns>
        ///     A reference to the single custom attribute of type  that is applied to , or null if there is no such
        ///     attribute.
        /// </returns>
        public static Attribute GetCustomAttribute(this Module element, Type attributeType)
        {
            return Attribute.GetCustomAttribute(element, attributeType);
        }

        /// <summary>
        ///     Retrieves a custom attribute applied to a module. Parameters specify the module, the type of the custom
        ///     attribute to search for, and an ignored search option.
        /// </summary>
        /// <param name="element">An object derived from the  class that describes a portable executable file.</param>
        /// <param name="attributeType">The type, or a base type, of the custom attribute to search for.</param>
        /// <param name="inherit">This parameter is ignored, and does not affect the operation of this method.</param>
        /// <returns>
        ///     A reference to the single custom attribute of type  that is applied to , or null if there is no such
        ///     attribute.
        /// </returns>
        public static Attribute GetCustomAttribute(this Module element, Type attributeType, Boolean inherit)
        {
            return Attribute.GetCustomAttribute(element, attributeType, inherit);
        }
    }
}