// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.Net.Mail;

namespace Z.ExtensionMethods
{
    public static partial class MailMessageExtension
    {
        /// <summary>
        ///     A MailMessage extension method that sends this message asynchronous.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="userToken">The user token.</param>
        /// <example>
        ///     <code>
        ///           using System.Net.Mail;
        ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///           using Z.ExtensionMethods;
        ///           
        ///           namespace ExtensionMethods.Examples
        ///           {
        ///               [TestClass]
        ///               public class System_Net_Mail_MailMessage_SendAsync
        ///               {
        ///                   [TestMethod]
        ///                   public void SendAsync()
        ///                   {
        ///                       var token = new object();
        ///           
        ///                       // Type
        ///                       var @this = new MailMessage(&quot;noreply@zzzportal.com&quot;, &quot;noreply@zzzportal.com&quot;, &quot;Fizz&quot;, &quot;Buzz&quot;);
        ///           
        ///                       // Examples
        ///                       @this.SendAsync(token); // Send a mail async
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        public static void SendAsync(this MailMessage @this, object userToken)
        {
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.SendAsync(@this, userToken);
            }
        }
    }
}