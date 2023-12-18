using humanResourceProject.Application.Services.Abstract.ILeaveServices;
using humanResourceProject.Application.Services.Concrete.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.IRepository.BaseRepos;
using humanResourceProject.Models.DTOs;
using humanResourceProject.Models.VMs;

namespace humanResourceProject.Application.Services.Concrete.LeaveServices
{
    public class LeaveReadService : BaseReadService<Leave>, ILeaveReadService
    {
        public LeaveReadService(IBaseReadRepository<Leave> readRepository) : base(readRepository)
        {
        }

        public Task<List<LeaveVM>> GetAllLeaves()
        {
            throw new NotImplementedException();
        }

        public LeaveDTO GetLeaveById(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
