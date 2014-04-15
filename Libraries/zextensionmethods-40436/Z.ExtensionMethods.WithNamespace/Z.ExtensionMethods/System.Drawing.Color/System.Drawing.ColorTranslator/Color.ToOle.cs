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
        ///     Translates the specified  structure to an OLE color.
        /// </summary>
        /// <param name="c">The  structure to translate.</param>
        /// <returns>The OLE color value.</returns>
        public static Int32 ToOle(this Color c)
        {
            return ColorTranslator.ToOle(c);
        }
    }
}