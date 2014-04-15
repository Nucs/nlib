// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

namespace Z.ExtensionMethods
{
    public static partial class StringExtension
    {
        /// <summary>
        ///     Retrieves the system&#39;s reference to the specified .
        /// </summary>
        /// <param name="str">A string to search for in the intern pool.</param>
        /// <returns>
        ///     The system&#39;s reference to , if it is interned; otherwise, a new reference to a string with the value of .
        /// </returns>
        public static String Intern(this String str)
        {
            return String.Intern(str);
        }
    }
}