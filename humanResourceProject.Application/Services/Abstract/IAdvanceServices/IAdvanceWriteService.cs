using humanResourceProject.Application.Services.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Models.DTOs;
namespace humanResourceProject.Application.Services.Abstract.IAdvanceServices
{
    public interface IAdvanceWriteService : IBaseWriteService<Advance>
    {
        Task<bool> InsertAdvance(AdvanceDTO model);

        Task<bool> UpdateAdvance(AdvanceDTO model);

        Task DeleteAdvance(Guid id);
    }
}
