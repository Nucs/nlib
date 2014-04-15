// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

public static partial class Int64Extension
{
    /// <summary>
    ///     Converts the specified Windows file time to an equivalent UTC time.
    /// </summary>
    /// <param name="fileTime">A Windows file time expressed in ticks.</param>
    /// <returns>
    ///     An object that represents the UTC time equivalent of the date and time represented by the  parameter.
    /// </returns>
    public static DateTime FromFileTimeUtc(this Int64 fileTime)
    {
        return DateTime.FromFileTimeUtc(fileTime);
    }
}