using humanResourceProject.Application.Services.Abstract.IAdvanceServices;
using humanResourceProject.Application.Services.Concrete.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.IRepository.BaseRepos;
using humanResourceProject.Models.DTOs;
using humanResourceProject.Models.VMs;

namespace humanResourceProject.Application.Services.Concrete.AdvanceServices
{
    public class AdvanceReadService : BaseReadService<Advance>, IAdvanceReadService
    {
        public AdvanceReadService(IBaseReadRepository<Advance> readRepository) : base(readRepository)
        {
        }

        public AdvanceDTO GetAdvanceById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<AdvanceVM>> GetAllAdvances()
        {
            throw new NotImplementedException();
        }
    }
}
