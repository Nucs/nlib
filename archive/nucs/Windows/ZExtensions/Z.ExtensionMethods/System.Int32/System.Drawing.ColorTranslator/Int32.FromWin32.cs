/*// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.Drawing;

public static partial class Int32Extension
{
    /// <summary>
    ///     Translates a Windows color value to a GDI+  structure.
    /// </summary>
    /// <param name="win32Color">The Windows color to translate.</param>
    /// <returns>The  structure that represents the translated Windows color.</returns>
    public static Color FromWin32(this Int32 win32Color)
    {
        return ColorTranslator.FromWin32(win32Color);
    }
}*/