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
        private readonly IBaseWriteRepository<Leave> _baseWriteRepository;
        private readonly IBaseReadRepository<Leave> _baseReadRepository;
        private readonly IMapper _mapper;
        public LeaveWriteService(IBaseWriteRepository<Leave> writeRepository, IBaseReadRepository<Leave> readRepository, IMapper mapper) : base(writeRepository, readRepository)
        {
            _baseWriteRepository = writeRepository;
            _baseReadRepository = readRepository;
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
            int currentLeaveCount = await _baseReadRepository.GetCountAsync();
            newLeave.LeaveNo = Leave.GenerateLeaveNumber(currentLeaveCount);
            if (await _baseWriteRepository.Insert(newLeave))
                return IdentityResult.Success;
            else
                return IdentityResult.Failed();
        }

        public async Task<IdentityResult> UpdateLeave(UpdateLeaveDTO model)
        {
            Leave updateLeave = await _baseReadRepository.GetSingleDefault(x => x.Id == model.Id);
            if (updateLeave == null)
                return IdentityResult.Failed();

            _baseWriteRepository.DetachEntity(updateLeave);

            updateLeave = _mapper.Map<Leave>(model);

            updateLeave.Status = Domain.Enum.Status.Modified;
            updateLeave.UpdateDate = DateTime.Now;

            if (await _baseWriteRepository.Update(updateLeave))
                return IdentityResult.Success;
            else
                return IdentityResult.Failed();

        }
    }
}
