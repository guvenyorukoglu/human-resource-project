using humanResourceProject.Application.Services.Abstract.IAdvanceServices;
using humanResourceProject.Application.Services.Concrete.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.IRepository.BaseRepos;
using humanResourceProject.Models.DTOs;

namespace humanResourceProject.Application.Services.Concrete.AdvanceServices
{
    public class AdvanceWriteService : BaseWriteService<Advance>, IAdvanceWriteService
    {
        public AdvanceWriteService(IBaseWriteRepository<Advance> writeRepository, IBaseReadRepository<Advance> readRepository) : base(writeRepository, readRepository)
        {
        }

        public Task DeleteAdvance(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> InsertAdvance(AdvanceDTO model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAdvance(AdvanceDTO model)
        {
            throw new NotImplementedException();
        }
    }
}
