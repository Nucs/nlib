// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.Net;

namespace Z.ExtensionMethods
{
    public static partial class Int64Extension
    {
        /// <summary>
        ///     Converts a long value from host byte order to network byte order.
        /// </summary>
        /// <param name="host">The number to convert, expressed in host byte order.</param>
        /// <returns>A long value, expressed in network byte order.</returns>
        public static Int64 HostToNetworkOrder(this Int64 host)
        {
            return IPAddress.HostToNetworkOrder(host);
        }
    }
}