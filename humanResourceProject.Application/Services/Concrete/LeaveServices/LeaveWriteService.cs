using humanResourceProject.Application.Services.Abstract.ILeaveServices;
using humanResourceProject.Application.Services.Concrete.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.IRepository.BaseRepos;
using humanResourceProject.Models.DTOs;

namespace humanResourceProject.Application.Services.Concrete.LeaveServices
{
    public class LeaveWriteService : BaseWriteService<Leave>, ILeaveWriteService
    {
        public LeaveWriteService(IBaseWriteRepository<Leave> writeRepository, IBaseReadRepository<Leave> readRepository) : base(writeRepository, readRepository)
        {
        }

        public Task DeleteLeave(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> InsertLeave(LeaveDTO model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateLeave(LeaveDTO model)
        {
            throw new NotImplementedException();
        }
    }
}
