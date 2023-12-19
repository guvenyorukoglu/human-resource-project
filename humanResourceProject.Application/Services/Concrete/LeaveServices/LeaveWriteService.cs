using AutoMapper;
using humanResourceProject.Application.Services.Abstract.ILeaveServices;
using humanResourceProject.Application.Services.Concrete.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.IRepository.BaseRepos;
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
            if (await _leaveWriteRepository.Insert(newLeave))
                return IdentityResult.Success;
            else
                return IdentityResult.Failed();
        }

        public async Task<IdentityResult> UpdateLeave(LeaveDTO model)
        {
            Leave leave = await _leaveReadRepository.GetSingleDefault(x => x.Id == model.Id);
            if (leave == null)
                return IdentityResult.Failed();

            LeaveDTO leaveDTO = _mapper.Map<LeaveDTO>(leave);

            leaveDTO.LeaveType = model.LeaveType;
            leaveDTO.StartDate = model.StartDate;
            leaveDTO.EndDate = model.EndDate;
            leaveDTO.Status = Domain.Enum.Status.Modified;
            leaveDTO.UpdateDate = DateTime.Now;

            if (await _leaveWriteRepository.Update(_mapper.Map<Leave>(leaveDTO)))
                return IdentityResult.Success;
            else
                return IdentityResult.Failed();

        }
    }
}
