using humanResourceProject.Domain.Entities.Concrete;

namespace humanResourceProject.Application.Services.Abstract.IMailServices
{
    public interface IMailService
    {
        Task SendMessageAsync(AppUser appUser, string action);
        Task SendUserRegisteredEmail(AppUser appUser, string action);
        Task SendAccountConfirmEmail(AppUser user, string action);
        Task SendForgotPasswordEmail(AppUser user, string link);
        Task SendApproveMail(AppUser user, string action, string body);


    }
}
