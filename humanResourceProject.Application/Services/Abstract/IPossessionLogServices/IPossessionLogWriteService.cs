using humanResourceProject.Application.Services.Abstract.IBaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Models.DTOs;
using Microsoft.AspNetCore.Identity;

namespace humanResourceProject.Application.Services.Abstract.IPossessionLogServices
{
    public interface IPossessionLogWriteService : IBaseWriteService<PossessionLog>
    {
        Task<IdentityResult> AssignPossession(AssignPossessionDTO model);
        Task<IdentityResult> TakeBackPossession(Guid id);
    }
}
