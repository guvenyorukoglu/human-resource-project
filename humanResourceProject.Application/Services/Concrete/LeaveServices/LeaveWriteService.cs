using AutoMapper;
using humanResourceProject.Application.Services.Abstract.ILeaveServices;
using humanResourceProject.Application.Services.Concrete.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.IRepository.BaseRepos;
using humanResourceProject.Domain.IRepository.LeaveRepo;
using humanResourceProject.Models.DTOs;
using Microsoft.AspNetCore.Identity;

namespace humanResourceProject.Application.Services.Concrete.LeaveServices
{
    public class LeaveWriteService : BaseWriteService<Leave>, ILeaveWriteService
    {
        private readonly IBaseWriteRepository<Leave> _baseWriteRepository;
        private readonly IBaseReadRepository<Leave> _baseReadRepository;
        private readonly ILeaveWriteRepository _leaveWriteRepository;
        private readonly IMapper _mapper;
        public LeaveWriteService(IBaseWriteRepository<Leave> writeRepository, IBaseReadRepository<Leave> readRepository, ILeaveWriteRepository leaveWriteRepository, IMapper mapper) : base(writeRepository, readRepository)
        {
            _baseWriteRepository = writeRepository;
            _baseReadRepository = readRepository;
            _leaveWriteRepository = leaveWriteRepository;
            _mapper = mapper;
        }

        public async Task<IdentityResult> DeleteLeave(Guid id)
        {
            Leave leave = await _baseReadRepository.GetById(id);
            leave.Status = Domain.Enum.Status.Deleted;
            leave.DeleteDate = DateTime.Now;
            if (await _baseWriteRepository.Delete(id))
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
            if (await _baseWriteRepository.Insert(newLeave))
                return IdentityResult.Success;
            else
                return IdentityResult.Failed();
        }

        public async Task<IdentityResult> UpdateLeave(LeaveDTO model)
        {
            Leave leave = await _baseReadRepository.GetSingleDefault(x => x.Id == model.Id);
            if (leave == null)
                return IdentityResult.Failed();

            LeaveDTO leaveDTO = _mapper.Map<LeaveDTO>(leave);

            leaveDTO.LeaveType = model.LeaveType;
            leaveDTO.StartDate = model.StartDate;
            leaveDTO.EndDate = model.EndDate;
            leaveDTO.Status = Domain.Enum.Status.Modified;
            leaveDTO.UpdateDate = DateTime.Now;

            if (await _baseWriteRepository.Update(_mapper.Map<Leave>(leaveDTO)))
                return IdentityResult.Success;
            else
                return IdentityResult.Failed();

        }
    }
}
