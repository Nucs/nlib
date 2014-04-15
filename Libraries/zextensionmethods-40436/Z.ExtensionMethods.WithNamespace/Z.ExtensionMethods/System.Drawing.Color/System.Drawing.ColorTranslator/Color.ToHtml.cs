// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.Drawing;

namespace Z.ExtensionMethods
{
    public static partial class ColorExtension
    {
        /// <summary>
        ///     Translates the specified  structure to an HTML string color representation.
        /// </summary>
        /// <param name="c">The  structure to translate.</param>
        /// <returns>The string that represents the HTML color.</returns>
        public static String ToHtml(this Color c)
        {
            return ColorTranslator.ToHtml(c);
        }
    }
}