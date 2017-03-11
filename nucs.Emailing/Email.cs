using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using B2BFamily.SmtpValidation.Lib;
using Mailzory;
using nucs.Emailing.Helpers;
using nucs.Emailing.Templating;

namespace nucs.Emailing {
    public class Email : IEmail {
        public SimpleLog Logger { get; set; }
        public SmtpSettings Settings { get; set; }

        public Email(SimpleLog logger, SmtpSettings settings) {
            Logger = logger;
            Settings = settings;
        }

        public Email(Configuration config) {
            Logger = SimpleLog.LoadConfiguration(config);
            Settings = SmtpSettings.LoadConfiguration(config);
        }

        /// <summary>
        ///     Gets a smtp client, with valid credetials - make sure to dispose at the end!
        /// </summary>
        public SmtpClientAdv GetClient {
            get {
                var client = Settings.UseDefaultCredentials ? new SmtpClientAdv(Settings.HostIp, Settings.Port) : new SmtpClientAdv(Settings.HostIp, Settings.Port, Settings.Username, Settings.Password);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl = Settings.EnableSSL;
                return client;
            }
        }

        #region Templated Send

        #region Overloads

        public Task SendTemplate(string sender, string sender_displayname, string receiver, string subject, EmailSource source, string identifier) {
            return SendTemplate(new MailAddress(sender, sender_displayname), new[] {new MailAddress(receiver)}, subject, source, identifier);
        }

        public Task SendTemplate(string sender, string sender_displayname, string[] receiver, string subject, EmailSource source, string identifier) {
            return SendTemplate(new MailAddress(sender, sender_displayname), receiver.Select(s => new MailAddress(s)).ToArray(), subject, source, identifier);
        }

        public Task SendTemplate(string sender, string sender_displayname, MailAddress receiver, string subject, EmailSource source, string identifier) {
            return SendTemplate(new MailAddress(sender, sender_displayname), new[] {receiver}, subject, source, identifier);
        }

        public Task SendTemplate(string sender, string sender_displayname, MailAddress[] receiver, string subject, EmailSource source, string identifier) {
            return SendTemplate(new MailAddress(sender, sender_displayname), receiver, subject, source, identifier);
        }

        public Task SendTemplate(Sender sender, string receiver, string subject, EmailSource source, string identifier) {
            return SendTemplate(new MailAddress(Settings.DefaultSender, Settings.DefaultSenderDisplayName), new[] {new MailAddress(receiver)}, subject, source, identifier);
        }

        public Task SendTemplate(Sender sender, string[] receiver, string subject, EmailSource source, string identifier) {
            return SendTemplate(new MailAddress(Settings.DefaultSender, Settings.DefaultSenderDisplayName), receiver.Select(s => new MailAddress(s)).ToArray(), subject, source, identifier);
        }

        public Task SendTemplate(Sender sender, MailAddress receiver, string subject, EmailSource source, string identifier) {
            return SendTemplate(new MailAddress(Settings.DefaultSender, Settings.DefaultSenderDisplayName), new[] {receiver}, subject, source, identifier);
        }

        public Task SendTemplate(Sender sender, MailAddress[] receivers, string subject, EmailSource source, string identifier) {
            return SendTemplate(new MailAddress(Settings.DefaultSender, Settings.DefaultSenderDisplayName), receivers, subject, source, identifier);
        }

        #endregion

        /// <summary>
        ///     Sends a templated email.
        /// </summary>
        /// <param name="sender">Which email address sends this email</param>
        /// <param name="receivers">The receiver of the email</param>
        /// <param name="subject">Subject of the email</param>
        /// <param name="source">Which source refer to</param>
        /// <param name="identifier">Depends on the EmailSource, if File then a path, if Resource then the resource name.</param>
        public async Task SendTemplate(MailAddress sender, MailAddress[] receivers, string subject, EmailSource source, string identifier) {
            await SendTemplate<TVoid>(sender, receivers, subject, source, identifier,null);
        }

        #region Overloads

        public Task SendTemplate<TModel>(string sender, string sender_displayname, string receiver, string subject, EmailSource source, string identifier, TModel model) where TModel : class {
            return SendTemplate(new MailAddress(sender, sender_displayname), new[] {new MailAddress(receiver)}, subject, source, identifier, model);
        }

        public Task SendTemplate<TModel>(string sender, string sender_displayname, string[] receiver, string subject, EmailSource source, string identifier, TModel model) where TModel : class {
            return SendTemplate(new MailAddress(sender, sender_displayname), receiver.Select(s => new MailAddress(s)).ToArray(), subject, source, identifier, model);
        }

        public Task SendTemplate<TModel>(string sender, string sender_displayname, MailAddress receiver, string subject, EmailSource source, string identifier, TModel model) where TModel : class {
            return SendTemplate(new MailAddress(sender, sender_displayname), new[] {receiver}, subject, source, identifier, model);
        }

        public Task SendTemplate<TModel>(string sender, string sender_displayname, MailAddress[] receiver, string subject, EmailSource source, string identifier, TModel model) where TModel : class {
            return SendTemplate(new MailAddress(sender, sender_displayname), receiver, subject, source, identifier, model);
        }

        public Task SendTemplate<TModel>(Sender sender, string receiver, string subject, EmailSource source, string identifier, TModel model) where TModel : class {
            return SendTemplate(new MailAddress(Settings.DefaultSender, Settings.DefaultSenderDisplayName), new[] {new MailAddress(receiver)}, subject, source, identifier, model);
        }

        public Task SendTemplate<TModel>(Sender sender, string[] receiver, string subject, EmailSource source, string identifier, TModel model) where TModel : class {
            return SendTemplate(new MailAddress(Settings.DefaultSender, Settings.DefaultSenderDisplayName), receiver.Select(s => new MailAddress(s)).ToArray(), subject, source, identifier, model);
        }

        public Task SendTemplate<TModel>(Sender sender, MailAddress receiver, string subject, EmailSource source, string identifier, TModel model) where TModel : class {
            return SendTemplate(new MailAddress(Settings.DefaultSender, Settings.DefaultSenderDisplayName), new[] {receiver}, subject, source, identifier, model);
        }

        public Task SendTemplate<TModel>(Sender sender, MailAddress[] receivers, string subject, EmailSource source, string identifier, TModel model) where TModel : class {
            return SendTemplate(new MailAddress(Settings.DefaultSender, Settings.DefaultSenderDisplayName), receivers, subject, source, identifier, model);
        }

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
        public async Task SendTemplate<TModel>(MailAddress sender, MailAddress[] receivers, string subject, EmailSource source, string identifier, TModel model) where TModel : class {
            if (sender == null)
                throw new ArgumentNullException(nameof(sender));
            if (receivers == null)
                throw new ArgumentNullException(nameof(receivers));

            var template = EmailSources.Fetch(source, identifier);
            if (template == null)
                throw new FileNotFoundException($"Template could not have been found!\n Source: {source}, Identifier: {identifier}");
            //support for void model:
            Mailzory.Email e = typeof(TModel) == typeof(void) ? (Mailzory.Email) (new OutEmail(template, GetClient)) : (Mailzory.Email) new OutEmail<TModel>(template, model, GetClient);

            if (Logger.LogLocally)
                Logger.LogMessage(subject, receivers.Select(r=>r.Address).ToArray(), sender.Address,sender.DisplayName, ((IBodyGenerator) e).GenerateMailBody()); //todo add a body to this!
            e.SetFrom(sender.Address, sender.DisplayName);
            await e.SendAsync(receivers, subject ?? "No Subject");
        }

        #endregion

        #region Regular Send

        #region Overloads
        public Task Send(string sender, string sender_displayname, string receiver, string subject, string body) {
            return Send(new MailAddress(sender, sender_displayname), new[] {new MailAddress(receiver)}, subject, body);
        }

        public Task Send(string sender, string sender_displayname, string[] receiver, string subject, string body) {
            return Send(new MailAddress(sender, sender_displayname), receiver.Select(s => new MailAddress(s)).ToArray(), subject, body);
        }

        public Task Send(string sender, string sender_displayname, MailAddress receiver, string subject, string body) {
            return Send(new MailAddress(sender, sender_displayname), new[] {receiver}, subject, body);
        }

        public Task Send(string sender, string sender_displayname, MailAddress[] receiver, string subject, string body) {
            return Send(new MailAddress(sender, sender_displayname), receiver, subject, body);
        }

        public Task Send(Sender sender, string receiver, string subject, string body) {
            return Send(new MailAddress(Settings.DefaultSender, Settings.DefaultSenderDisplayName), new[] {new MailAddress(receiver)}, subject, body);
        }

        public Task Send(Sender sender, string[] receiver, string subject, string body) {
            return Send(new MailAddress(Settings.DefaultSender, Settings.DefaultSenderDisplayName), receiver.Select(s => new MailAddress(s)).ToArray(), subject, body);
        }

        public Task Send(Sender sender, MailAddress receiver, string subject, string body) {
            return Send(new MailAddress(Settings.DefaultSender, Settings.DefaultSenderDisplayName), new[] {receiver}, subject, body);
        }

        public Task Send(Sender sender, MailAddress[] receivers, string subject, string body) {
            return Send(new MailAddress(Settings.DefaultSender, Settings.DefaultSenderDisplayName), receivers, subject, body);
        }
        #endregion

        /// <summary>
        ///     Sends an email.
        /// </summary>
        /// <param name="sender">Which email address sends this email</param>
        /// <param name="receiver">The receiver of the email</param>
        /// <param name="subject">Subject of the email</param>
        /// <param name="body">Content of the email</param>
        public Task Send(MailAddress sender, MailAddress[] receiver, string subject, string body) {
            return SendTemplate(sender, receiver, subject, EmailSource.String, body);
        }

        #endregion

        private async Task _internal_sendasyncmail(MailMessage msg) {


            using (var cli = GetClient) {
                cli.Credentials = new NetworkCredential(Settings.Username, Settings.Password);
                await cli.SendMailAsync(msg);
            }
        }

        /// <summary>
        ///     Tests the connection and authentication
        /// </summary>
        /// <returns></returns>
        public async Task<bool> TestConnection() {
            return await SmtpHelper.ValidateCredentials(Settings.Username, Settings.PasswordToString(), Settings.HostIp, Settings.Port, Settings.EnableSSL);
        }
    }
}