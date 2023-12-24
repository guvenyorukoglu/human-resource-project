using AutoMapper;
using humanResourceProject.Application.Services.Abstract.IAdvanceServices;
using humanResourceProject.Application.Services.Concrete.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.IRepository.BaseRepos;
using humanResourceProject.Models.DTOs;
using Microsoft.AspNetCore.Identity;

namespace humanResourceProject.Application.Services.Concrete.AdvanceServices
{
    public class AdvanceWriteService : BaseWriteService<Advance>, IAdvanceWriteService
    {
        private readonly IBaseWriteRepository<Advance> _baseWriteRepository;
        private readonly IBaseReadRepository<Advance> _baseReadRepository;
        private readonly IMapper _mapper;
        public AdvanceWriteService(IBaseWriteRepository<Advance> writeRepository, IBaseReadRepository<Advance> readRepository, IMapper mapper) : base(writeRepository, readRepository)
        {
            _baseWriteRepository = writeRepository;
            _baseReadRepository = readRepository;
            _mapper = mapper;
        }

        public async Task<IdentityResult> DeleteAdvance(Guid id)
        {
            Advance advance = await _baseReadRepository.GetById(id);
            advance.Status = Domain.Enum.Status.Deleted;
            advance.DeleteDate = DateTime.Now;
            if (await _baseWriteRepository.Delete(id))
                return IdentityResult.Success;
            else
                return IdentityResult.Failed();
        }

        public async Task<IdentityResult> InsertAdvance(AdvanceDTO model)
        {
            if (model == null)
                return IdentityResult.Failed();

            Advance newAdvance = _mapper.Map<Advance>(model);
            newAdvance.Status = Domain.Enum.Status.Active;
            newAdvance.CreateDate = DateTime.Now;

            int currentAdvanceCount = await _baseReadRepository.GetCountAsync();
            newAdvance.AdvanceNo = Advance.GenerateAdvanceNumber(currentAdvanceCount);

            if (await _baseWriteRepository.Insert(newAdvance))
                return IdentityResult.Success;
            else
                return IdentityResult.Failed();
        }

        public async Task<IdentityResult> UpdateAdvance(UpdateAdvanceDTO model)
        {
            Advance updateAdvance = await _baseReadRepository.GetSingleDefault(x => x.Id == model.Id);
            if (updateAdvance == null)
                return IdentityResult.Failed();

            _baseWriteRepository.DetachEntity(updateAdvance);

            updateAdvance = _mapper.Map<Advance>(model);

            updateAdvance.Status = Domain.Enum.Status.Modified;
            updateAdvance.UpdateDate = DateTime.Now;

            if (await _baseWriteRepository.Update(updateAdvance))
                return IdentityResult.Success;
            else
                return IdentityResult.Failed();

        }
    }
}
