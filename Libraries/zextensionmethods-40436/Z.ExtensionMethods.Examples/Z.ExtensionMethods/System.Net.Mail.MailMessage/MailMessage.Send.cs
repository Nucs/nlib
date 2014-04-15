using System.Net.Mail;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_Net_Mail_MailMessage_Send
    {
        [TestMethod]
        public void Send()
        {
            // Type
            var @this = new MailMessage("noreply@zzzportal.com", "noreply@zzzportal.com", "Fizz", "Buzz");

            // Examples
            @this.Send(); // Send a mail
        }
    }
}