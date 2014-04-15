// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.Web;

namespace Z.ExtensionMethods
{
    /// ###
    /// <summary>HTTP response extension.</summary>
    public static partial class HttpResponseExtension
    {
        /// <summary>
        ///     A HttpResponse extension method that sets the response to status code 412 (Precondition failed.).
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        public static void SetStatusPreconditionFailed(this HttpResponse @this)
        {
            @this.StatusCode = 412;
            @this.StatusDescription = "Precondition failed.";
        }
    }
}