// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.Net;

public static partial class Int16Extension
{
    /// <summary>
    ///     Converts a short value from network byte order to host byte order.
    /// </summary>
    /// <param name="network">The number to convert, expressed in network byte order.</param>
    /// <returns>A short value, expressed in host byte order.</returns>
    public static Int16 NetworkToHostOrder(this Int16 network)
    {
        return IPAddress.NetworkToHostOrder(network);
    }
}