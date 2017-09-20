/*// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.Web;

/// ###
/// <summary>HTTP response extension.</summary>
public static partial class HttpResponseExtension
{
    /// <summary>
    ///     A HttpResponse extension method that sets the response to status code 500 (Internal server error. ).
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    public static void SetStatusInternalServerError(this HttpResponse @this)
    {
        @this.StatusCode = 500;
        @this.StatusDescription = "Internal server error. ";
    }
}*/