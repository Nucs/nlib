using System.Net;
using System.Net.Mail;

namespace nucs.Network.Mail {
    /// <summary>
    ///     Simple Gmail client to send emails.
    /// </summary>
    public class Gmail {
        public static void SendMail(string userName, string password, string toAddress, string subject, string messageBody) {
            if (userName.IndexOf('@') == -1)
                userName += "@gmail.com";

            var mail = new MailMessage(userName, toAddress, subject, messageBody);

            var networkCredential = new NetworkCredential(userName, password);
            mail.IsBodyHtml = true;

            var smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = networkCredential;

            smtpClient.Send(mail);
        }

        public static void SendMail(string userName, string password, string toAddress, string subject, string messageBody, string[] attachment_filenames) {
            if (userName.IndexOf('@') == -1)
                userName += "@gmail.com";

            using (var mail = new MailMessage(userName, toAddress, subject, messageBody)) {
                foreach (string attachment_filename in attachment_filenames) {
                    /*string mime_type = "application/unknown";
                
                    string ext = System.IO.Path.GetExtension(attachment_filename).ToLower();
                
                    Microsoft.Win32.RegistryKey reg_key = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
                
                    if (reg_key != null && reg_key.GetValue("Content Type") != null)
                        mime_type = reg_key.GetValue("Content Type").ToString();*/

                    mail.Attachments.Add(new Attachment(attachment_filename));
                }

                var networkCredential = new NetworkCredential(userName, password);
                mail.IsBodyHtml = true;

                var smtpClient = new SmtpClient("smtp.gmail.com", 587);
                smtpClient.UseDefaultCredentials = false;
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = networkCredential;

                smtpClient.Send(mail);
            }
        }
    }
}