using System.Net.Mail;
using Mailzory;

namespace nucs.Emailing {
    public class OutEmail : Mailzory.Email, IBodyGenerator {
        public OutEmail(string razorTemplate, SmtpClientAdv smtpClient = null) : base(razorTemplate, smtpClient) {}

        public new string GenerateMailBody() {
            return base.GenerateMailBody();
        }
    }


    public class OutEmail<T> : Mailzory.Email<T>, IBodyGenerator where T : class {
        public OutEmail(string razorTemplate, T Model, SmtpClientAdv smtpClient = null)
            : base(razorTemplate, Model, smtpClient) {}

        public new string GenerateMailBody() {
            return base.GenerateMailBody();
        }
    }

    public interface IBodyGenerator {
        string GenerateMailBody();
    }
}