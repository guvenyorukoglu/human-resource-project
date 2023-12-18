using humanResourceProject.Application.Services.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Models.DTOs;
using Microsoft.AspNetCore.Identity;
namespace humanResourceProject.Application.Services.Abstract.IAdvanceServices
{
    public interface IAdvanceWriteService : IBaseWriteService<Advance>
    {
        Task<IdentityResult> InsertAdvance(AdvanceDTO model);

        Task<IdentityResult> UpdateAdvance(AdvanceDTO model);

        Task<IdentityResult> DeleteAdvance(Guid id);
    }
}
