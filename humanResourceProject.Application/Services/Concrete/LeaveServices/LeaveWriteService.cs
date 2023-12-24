using AutoMapper;
using humanResourceProject.Application.Services.Abstract.ILeaveServices;
using humanResourceProject.Application.Services.Concrete.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.IRepository.BaseRepos;
using humanResourceProject.Infrastructure.Repositories.BaseRepos;
using humanResourceProject.Models.DTOs;
using Microsoft.AspNetCore.Identity;

namespace humanResourceProject.Application.Services.Concrete.LeaveServices
{
    public class LeaveWriteService : BaseWriteService<Leave>, ILeaveWriteService
    {
        private readonly IBaseWriteRepository<Leave> _leaveWriteRepository;
        private readonly IBaseReadRepository<Leave> _leaveReadRepository;
        private readonly IMapper _mapper;
        public LeaveWriteService(IBaseWriteRepository<Leave> leaveWriteRepository, IBaseReadRepository<Leave> leaveReadRepository, IMapper mapper) : base(leaveWriteRepository, leaveReadRepository)
        {
            _leaveWriteRepository = leaveWriteRepository;
            _leaveReadRepository = leaveReadRepository;
            _mapper = mapper;
        }

        public async Task<IdentityResult> DeleteLeave(Guid id)
        {
            Leave leave = await _leaveReadRepository.GetById(id);
            leave.Status = Domain.Enum.Status.Deleted;
            leave.DeleteDate = DateTime.Now;

            if (await _leaveWriteRepository.Delete(id))
                return IdentityResult.Success;
            else
                return IdentityResult.Failed();
        }

        public async Task<IdentityResult> InsertLeave(LeaveDTO model)
        {
            if (model == null)
                return IdentityResult.Failed();

            Leave newLeave = _mapper.Map<Leave>(model);
            newLeave.Status = Domain.Enum.Status.Active;
            newLeave.CreateDate = DateTime.Now;

            int currentLeaveCount = await _leaveReadRepository.GetCountAsync();
            newLeave.LeaveNo = Leave.GenerateLeaveNumber(currentLeaveCount);

            if (await _leaveWriteRepository.Insert(newLeave))
                return IdentityResult.Success;
            else
                return IdentityResult.Failed();
        }

        public async Task<IdentityResult> UpdateLeave(UpdateLeaveDTO model)
        {
            Leave leave = await _leaveReadRepository.GetSingleDefault(x => x.Id == model.Id);
            if (leave == null)
                return IdentityResult.Failed();

            _leaveWriteRepository.DetachEntity(leave);

            leave = _mapper.Map<Leave>(model);

            leave.Status = Domain.Enum.Status.Modified;
            leave.UpdateDate = DateTime.Now;

            if (await _leaveWriteRepository.Update(leave))
                return IdentityResult.Success;
            else
                return IdentityResult.Failed();

        }
    }
}
