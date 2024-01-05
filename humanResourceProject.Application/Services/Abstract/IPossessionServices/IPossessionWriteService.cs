using humanResourceProject.Application.Services.Abstract.IBaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Models.DTOs;
using Microsoft.AspNetCore.Identity;

namespace humanResourceProject.Application.Services.Abstract.IPossessionServices
{
    public interface IPossessionWriteService : IBaseWriteService<Possession>
    {
        Task<IdentityResult> InsertPossession(PossessionDTO model);

        Task<IdentityResult> UpdatePossession(UpdatePossessionDTO model);

        Task<IdentityResult> DeletePossession(Guid id);
    }
}
