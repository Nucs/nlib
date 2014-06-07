// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace nucs.SystemCore {
    public static class EnumExtension {
        /// <summary>
        ///     An object extension method that gets description attribute.
        /// </summary>
        /// <param name="value">The value to act on.</param>
        /// <returns>The description attribute.</returns>
        public static string GetCustomAttributeDescription(this global::System.Enum value) {
            var attr = value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof (DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;
            return attr.Description;
        }
    }
}