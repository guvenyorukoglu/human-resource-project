using humanResourceProject.Application.Services.Abstract.IMailServices;
using humanResourceProject.Domain.Entities.Concrete;
using MimeKit;

namespace humanResourceProject.Application.Services.Concrete.MailServices
{
    public class MailService : IMailService
    {
        public async Task SendEmailAsync(AppUser user, string recipientEmail, string mailToName, string action, string subject, string body)
        {

            var confirmationLink = action;

            MimeMessage mimeMessage = new MimeMessage();
            MailboxAddress mailboxFrom = new MailboxAddress("Monitorease", "monitorease@gmail.com");
            MailboxAddress mailboxTo = new MailboxAddress(mailToName, recipientEmail);

            mimeMessage.From.Add(mailboxFrom);
            mimeMessage.To.Add(mailboxTo);

            var bodybuilder = new BodyBuilder();
            bodybuilder.HtmlBody = $"{body} {confirmationLink}";
            mimeMessage.Body = bodybuilder.ToMessageBody();

            mimeMessage.Subject = subject;

            MailKit.Net.Smtp.SmtpClient smtpClient = new MailKit.Net.Smtp.SmtpClient();

            smtpClient.Connect("smtp.gmail.com", 587, false);
            smtpClient.Authenticate("monitorease@gmail.com", "tvhq axkn vyrb zzmc");
            smtpClient.Send(mimeMessage);
            smtpClient.Disconnect(true);
        }

        //public async Task SendMessageAsync(AppUser user, string action)
        //{
        //    var confirmationLink = action;

        //    MimeMessage mimeMessage = new MimeMessage();
        //    MailboxAddress mailboxFrom = new MailboxAddress("Monitorease", "monitorease@gmail.com");
        //    MailboxAddress mailboxTo = new MailboxAddress($"{user.FirstName} {user.LastName}", user.Email);

        //    mimeMessage.From.Add(mailboxFrom);
        //    mimeMessage.To.Add(mailboxTo);

        //    var bodybuilder = new BodyBuilder();
        //    bodybuilder.TextBody = "Kayıt işlemini gerçekleştirmek için linke tıklayınız: " + confirmationLink;
        //    mimeMessage.Body = bodybuilder.ToMessageBody();

        //    mimeMessage.Subject = "Onay linkiniz";


        //    MailKit.Net.Smtp.SmtpClient smtpClient = new MailKit.Net.Smtp.SmtpClient();

        //    smtpClient.Connect("smtp.gmail.com", 587, false);
        //    smtpClient.Authenticate("monitorease@gmail.com", "tvhq axkn vyrb zzmc");
        //    smtpClient.Send(mimeMessage);
        //}

        //public async Task SendUserRegisteredEmail(AppUser user, string action)
        //{
        //    var confirmationLink = action;

        //    MimeMessage mimeMessage = new MimeMessage();
        //    MailboxAddress mailboxFrom = new MailboxAddress("Monitorease", "monitorease@gmail.com");
        //    MailboxAddress mailboxTo = new MailboxAddress("Admin", "yorukoglu.guven@gmail.com");

        //    mimeMessage.From.Add(mailboxFrom);
        //    mimeMessage.To.Add(mailboxTo);

        //    var bodybuilder = new BodyBuilder();
        //    bodybuilder.HtmlBody = $"<p>Merhaba Admin</p><p>Yeni kullanıcı uygulamaya kayıt olmuştur.</p><p>Kullanıcının statüsünü aktif yapmak için <a href='{confirmationLink}'>linkine</a> tıklayınız.</p><br><hr><br><h3>Team Monitorease</h3>";
        //    mimeMessage.Body = bodybuilder.ToMessageBody();

        //    mimeMessage.Subject = "Yeni Kullanıcı Kayıt Oldu!";

        //    MailKit.Net.Smtp.SmtpClient smtpClient = new MailKit.Net.Smtp.SmtpClient();

        //    smtpClient.Connect("smtp.gmail.com", 587, false);
        //    smtpClient.Authenticate("monitorease@gmail.com", "tvhq axkn vyrb zzmc");
        //    smtpClient.Send(mimeMessage);
        //}
        //   public async Task SendApproveMail(AppUser user, string action, string body)
        //{


        //    MimeMessage mimeMessage = new MimeMessage();
        //    MailboxAddress mailboxFrom = new MailboxAddress("Monitorease", "monitorease@gmail.com");
        //    MailboxAddress mailboxTo = new MailboxAddress("Admin", "efeyzyum@gmail.com");

        //    mimeMessage.From.Add(mailboxFrom);
        //    mimeMessage.To.Add(mailboxTo);

        //    var bodybuilder = new BodyBuilder();
        //    bodybuilder.HtmlBody =body;  
        //    mimeMessage.Body = bodybuilder.ToMessageBody();

        //    mimeMessage.Subject = "Yeni Kullanıcı Kayıt Oldu!";

        //    MailKit.Net.Smtp.SmtpClient smtpClient = new MailKit.Net.Smtp.SmtpClient();

        //    smtpClient.Connect("smtp.gmail.com", 587, false);
        //    smtpClient.Authenticate("monitorease@gmail.com", "tvhq axkn vyrb zzmc");
        //    smtpClient.Send(mimeMessage);
        //}

        //public async Task SendAccountConfirmEmail(AppUser user, string action)
        //{
        //    var confirmationLink = action;

        //    MimeMessage mimeMessage = new MimeMessage();
        //    MailboxAddress mailboxFrom = new MailboxAddress("Monitorease", "monitorease@gmail.com");
        //    MailboxAddress mailboxTo = new MailboxAddress($"{user.FirstName} {user.LastName}", user.Email);

        //    mimeMessage.From.Add(mailboxFrom);
        //    mimeMessage.To.Add(mailboxTo);

        //    var bodybuilder = new BodyBuilder();
        //    bodybuilder.HtmlBody = $"<p>Merhaba {user.FirstName} {user.LastName}</p><p>Monitorease hesabınız başarılı bir şekilde oluşturulmuştur.</p><p>Hesabınızı doğrulamak için lütfen <a href='{confirmationLink}'>buraya</a> tıklayınız.</p><p>Bize her zaman monitorease@gmail.com adresinden ulaşabilirsiniz.</p><br><hr><br><h3>Team Monitorease</h3>";
        //    mimeMessage.Body = bodybuilder.ToMessageBody();

        //    mimeMessage.Subject = "Monitorease Hesabınızı Doğrulayınız!";

        //    MailKit.Net.Smtp.SmtpClient smtpClient = new MailKit.Net.Smtp.SmtpClient();

        //    smtpClient.Connect("smtp.gmail.com", 587, false);
        //    smtpClient.Authenticate("monitorease@gmail.com", "tvhq axkn vyrb zzmc");
        //    smtpClient.Send(mimeMessage);
        //}

        //public async Task SendForgotPasswordEmail(AppUser user, string link)
        //{
        //    var resetLink = link;

        //    MimeMessage mimeMessage = new MimeMessage();
        //    MailboxAddress mailboxFrom = new MailboxAddress("Monitorease", "monitorease@gmail.com");
        //    MailboxAddress mailboxTo = new MailboxAddress($"{user.FirstName} {user.LastName}", user.Email);

        //    mimeMessage.From.Add(mailboxFrom);
        //    mimeMessage.To.Add(mailboxTo);

        //    var bodybuilder = new BodyBuilder();
        //    bodybuilder.HtmlBody = $"<p>Merhaba {user.FirstName} {user.LastName}</p><p>Aşağıdaki linke tıklayarak şifrenizi sıfırlayabilirsiniz. Linkin geçerlilik süresi 24 saattir.</p><p>{resetLink}</p><p>Bize her zaman monitorease@gmail.com adresinden ulaşabilirsiniz.</p><br><hr><br><h3>Team Monitorease</h3>";
        //    mimeMessage.Body = bodybuilder.ToMessageBody();

        //    mimeMessage.Subject = "Monitorease Şifre Sıfırlama!";

        //    MailKit.Net.Smtp.SmtpClient smtpClient = new MailKit.Net.Smtp.SmtpClient();

        //    smtpClient.Connect("smtp.gmail.com", 587, false);
        //    smtpClient.Authenticate("monitorease@gmail.com", "tvhq axkn vyrb zzmc");
        //    smtpClient.Send(mimeMessage);
        //}


    }

}

