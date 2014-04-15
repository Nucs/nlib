// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.Drawing;

namespace Z.ExtensionMethods
{
    public static partial class Int32Extension
    {
        /// <summary>
        ///     Translates an OLE color value to a GDI+  structure.
        /// </summary>
        /// <param name="oleColor">The OLE color to translate.</param>
        /// <returns>The  structure that represents the translated OLE color.</returns>
        public static Color FromOle(this Int32 oleColor)
        {
            return ColorTranslator.FromOle(oleColor);
        }
    }
}