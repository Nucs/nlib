// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.Net;

namespace Z.ExtensionMethods
{
    public static partial class Int32Extension
    {
        /// <summary>
        ///     Converts an integer value from network byte order to host byte order.
        /// </summary>
        /// <param name="network">The number to convert, expressed in network byte order.</param>
        /// <returns>An integer value, expressed in host byte order.</returns>
        public static Int32 NetworkToHostOrder(this Int32 network)
        {
            return IPAddress.NetworkToHostOrder(network);
        }
    }
}