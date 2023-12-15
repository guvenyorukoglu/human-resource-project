using AutoMapper;
using Azure.Core;
using humanResourceProject.Application.Services.Abstract.IMailServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Models.DTOs;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Policy;

namespace humanResourceProject.Application.Services.Concrete.MailServices
{
    public class MailService : IMailService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;



        public MailService(UserManager<AppUser> userManager, IMapper mapper, IConfiguration configuration)
        {
            _userManager = userManager;
            _mapper = mapper;
            _configuration = configuration;
        }

        //public async Task Confirmation(Guid id, string token)
        //{
        //    var user = await _userManager.FindByIdAsync(id.ToString());
        //    if (user != null )
        //    {

        //        IdentityResult result =  await _userManager.ConfirmEmailAsync(user, token);
        //        if (result.Succeeded)
        //        {
        //            user.Status = Domain.Enum.Status.Active;

        //        }
        //    }

        //}

        public async Task SendMessage(string to, string subject, string body, bool isBodyHtml = true)
        {
       
            MailMessage mailMessage = new MailMessage();
           
            mailMessage.IsBodyHtml = isBodyHtml;
            mailMessage.To.Add(to);
            mailMessage.Subject = subject;
            mailMessage.Body = body;
            mailMessage.From = new MailAddress("monitorease@gmail.com");

            System.Net.Mail.SmtpClient smtpClient = new System.Net.Mail.SmtpClient();
            smtpClient.Credentials = new NetworkCredential("monitorease@gmail.com", "tvhq axkn vyrb zzmc");
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.Host = "smtp.gmail.com";
            smtpClient.Send(mailMessage);

        }

        public async Task SendMessageAsync(UserRegisterDTO model, string action)
        {

            AppUser user = new AppUser()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Address = model.Address,
                PhoneNumber = model.PhoneNumber,
                IdentificationNumber = model.IdentificationNumber,
                BloodGroup = model.BloodGroup,
                Birthdate = model.Birthdate,
                Title = model.Title,
                Job = model.Job,
          
                Email = model.Email,
                CompanyId = model.CompanyId

            };

            //IdentityResult result = await _iAppUser.Register(registerDTO);

            var token = _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = action;

            MimeMessage mimeMessage = new MimeMessage();
            MailboxAddress mailboxFrom = new MailboxAddress("Admin", "monitorease@gmail.com");
            MailboxAddress mailboxTo = new MailboxAddress("User", user.Email);

            mimeMessage.From.Add(mailboxFrom);
            mimeMessage.To.Add(mailboxTo);

            var bodybuilder = new BodyBuilder();
            bodybuilder.TextBody = "Kayıt işlemini gerçekleştirmek için linke tıklayınız: " + confirmationLink;
            mimeMessage.Body = bodybuilder.ToMessageBody();

            mimeMessage.Subject = "Onay linkiniz";


            MailKit.Net.Smtp.SmtpClient smtpClient = new MailKit.Net.Smtp.SmtpClient();

            smtpClient.Connect("smtp.gmail.com", 587, false);
            smtpClient.Authenticate("monitorease@gmail.com", "tvhq axkn vyrb zzmc");
            smtpClient.Send(mimeMessage);
  

        }



        //    AppUser messageUser = _mapper.Map<AppUser>(userRegister);

        //    IdentityResult result = await _userManager.CreateAsync(messageUser, messageUser.PasswordHash);

        //    var token = await _userManager.GenerateEmailConfirmationTokenAsync(messageUser);
        //  var confirmation= new Url.Action("Confirmation", "Account", new { id = user.Id, token }, Request.Scheme);
        //    MailMessage mailMessage = new MailMessage(); 
        //    mailMessage.To.Add(new MailAddress( userRegister.Email,"User"));
        //    mailMessage.From= new MailAddress(  "monitorease@gmail.com", "Admin");
        //    mailMessage.Subject = "onay linkiniz";
        //    var bodyBuilder= new StringBuilder();
        //    mailMessage.Body = "Tamamlamak için tıkla "+ confirmation ;
        //    mailMessage.From = new("monitorease@gmail.com", "Admin", System.Text.Encoding.UTF8);

        //    SmtpClient smtpClient = new SmtpClient();
        //    smtpClient.Credentials = new NetworkCredential("monitorease@gmail.com", "dmvw jqiw elap fbdc");
        //    smtpClient.Port = 587;
        //    smtpClient.EnableSsl = true;
        //    smtpClient.Host = "smtp.gmail.com";
        //    await smtpClient.SendMailAsync(mailMessage);
    }






    }

