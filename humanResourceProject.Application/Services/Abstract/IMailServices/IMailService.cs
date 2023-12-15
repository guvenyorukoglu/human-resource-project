using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace humanResourceProject.Application.Services.Abstract.IMailServices
{
    public interface IMailService
    {
        Task SendMessage(string to, string subject, string body, bool isBodyHtml = true);
        Task SendMessageAsync(UserRegisterDTO userRegister, string action);
        // void SendMessageAsync(UserRegisterDTO userRegister);
        //Task Confirmation(Guid id, string token);



    }
}
