using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using MSLServer.Models;

namespace MSLServer.Services.EmailService
{
    public class EmailService : IEmailService
    {
        public void SendEmail(Email request)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("nazmoxmc@gmail.com"));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = request.Body };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate("nazmoxmc@gmail.com", "mgolqfajpcvqoytn");
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
