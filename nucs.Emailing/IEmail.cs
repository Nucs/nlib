using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Mailzory;
using nucs.Emailing.Helpers;
using nucs.Emailing.Templating;

namespace nucs.Emailing {
    public interface IEmail {
        /// <summary>
        ///     Gets a smtp client, with valid credetials - make sure to dispose at the end!
        /// </summary>
        SmtpClientAdv GetClient { get; }

        #region Templated Send

        #region Overloads

        Task SendTemplate(string sender, string sender_displayname, string receiver, string subject, EmailSource source, string identifier);
        Task SendTemplate(string sender, string sender_displayname, string[] receiver, string subject, EmailSource source, string identifier);
        Task SendTemplate(string sender, string sender_displayname, MailAddress receiver, string subject, EmailSource source, string identifier);
        Task SendTemplate(string sender, string sender_displayname, MailAddress[] receiver, string subject, EmailSource source, string identifier);

        Task SendTemplate(Sender sender, string receiver, string subject, EmailSource source, string identifier);
        Task SendTemplate(Sender sender, string[] receiver, string subject, EmailSource source, string identifier);

        Task SendTemplate(Sender sender, MailAddress receiver, string subject, EmailSource source, string identifier);
        Task SendTemplate(Sender sender, MailAddress[] receivers, string subject, EmailSource source, string identifier);

        #endregion

        /// <summary>
        ///     Sends a templated email.
        /// </summary>
        /// <param name="sender">Which email address sends this email</param>
        /// <param name="receivers">The receiver of the email</param>
        /// <param name="subject">Subject of the email</param>
        /// <param name="body">Content of the email</param>
        /// <param name="source">Which source refer to</param>
        /// <param name="identifier">Depends on the EmailSource, if File then a path, if Resource then the resource name.</param>
        Task SendTemplate(MailAddress sender, MailAddress[] receivers, string subject, EmailSource source, string identifier);

        #region Overloads

        Task SendTemplate<TModel>(string sender, string sender_displayname, string receiver, string subject, EmailSource source, string identifier, TModel model) where TModel : class;
        Task SendTemplate<TModel>(string sender, string sender_displayname, string[] receiver, string subject, EmailSource source, string identifier, TModel model) where TModel : class;
        Task SendTemplate<TModel>(string sender, string sender_displayname, MailAddress receiver, string subject, EmailSource source, string identifier, TModel model) where TModel : class;
        Task SendTemplate<TModel>(string sender, string sender_displayname, MailAddress[] receiver, string subject, EmailSource source, string identifier, TModel model) where TModel : class;
        Task SendTemplate<TModel>(Sender sender, string receiver, string subject, EmailSource source, string identifier, TModel model) where TModel : class;
        Task SendTemplate<TModel>(Sender sender, string[] receiver, string subject, EmailSource source, string identifier, TModel model) where TModel : class;
        Task SendTemplate<TModel>(Sender sender, MailAddress receiver, string subject, EmailSource source, string identifier, TModel model) where TModel : class;
        Task SendTemplate<TModel>(Sender sender, MailAddress[] receivers, string subject, EmailSource source, string identifier, TModel model) where TModel : class;

        #endregion

        /// <summary>
        ///     Sends a templated email.
        /// </summary>
        /// <param name="sender">Which email address sends this email</param>
        /// <param name="receivers">The receiver of the email</param>
        /// <param name="subject">Subject of the email</param>
        /// <param name="body">Content of the email</param>
        /// <param name="source">Which source refer to</param>
        /// <param name="identifier">Depends on the EmailSource, if File then a path, if Resource then the resource name.</param>
        /// <param name="model">The model to pass to the view, default(TModel) to pass nothing.</param>
        Task SendTemplate<TModel>(MailAddress sender, MailAddress[] receivers, string subject, EmailSource source, string identifier, TModel model) where TModel : class;

        #endregion

        #region Regular Send

        #region Overloads

        Task Send(string sender, string sender_displayname, string receiver, string subject, string body);

        Task Send(string sender, string sender_displayname, string[] receiver, string subject, string body);

        Task Send(string sender, string sender_displayname, MailAddress receiver, string subject, string body);

        Task Send(string sender, string sender_displayname, MailAddress[] receiver, string subject, string body);
        Task Send(Sender sender, string receiver, string subject, string body);

        Task Send(Sender sender, string[] receiver, string subject, string body);
        Task Send(Sender sender, MailAddress receiver, string subject, string body);
        Task Send(Sender sender, MailAddress[] receivers, string subject, string body);

        #endregion

        /// <summary>
        ///     Sends an email.
        /// </summary>
        /// <param name="sender">Which email address sends this email</param>
        /// <param name="receiver">The receiver of the email</param>
        /// <param name="subject">Subject of the email</param>
        /// <param name="body">Content of the email</param>
        Task Send(MailAddress sender, MailAddress[] receiver, string subject, string body);

        #endregion

        /// <summary>
        ///     Tests the connection and authentication
        /// </summary>
        /// <returns></returns>
        Task<bool> TestConnection();
    }
}