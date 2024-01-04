using humanResourceProject.Application.Services.Abstract.IMailServices;
using humanResourceProject.Domain.Entities.Concrete;
using MimeKit;

namespace humanResourceProject.Application.Services.Concrete.MailServices
{
    public class MailService : IMailService
    {
        public async Task SendEmailAsync(AppUser user, string recipientEmail, string mailToName, string subject, string body)
        {

            //var confirmationLink = action; //link bodynin içinde olacak

            MimeMessage mimeMessage = new MimeMessage();
            MailboxAddress mailboxFrom = new MailboxAddress("Monitorease", "monitorease@gmail.com");
            MailboxAddress mailboxTo = new MailboxAddress(mailToName, recipientEmail);

            mimeMessage.From.Add(mailboxFrom);
            mimeMessage.To.Add(mailboxTo);

            var bodybuilder = new BodyBuilder();
            bodybuilder.HtmlBody = $"{body}";
            mimeMessage.Body = bodybuilder.ToMessageBody();

            mimeMessage.Subject = subject;

            MailKit.Net.Smtp.SmtpClient smtpClient = new MailKit.Net.Smtp.SmtpClient();

            smtpClient.Connect("smtp.gmail.com", 587, false);
            smtpClient.Authenticate("monitorease@gmail.com", "tvhq axkn vyrb zzmc");
            smtpClient.Send(mimeMessage);
            smtpClient.Disconnect(true);
        }
    }

}

