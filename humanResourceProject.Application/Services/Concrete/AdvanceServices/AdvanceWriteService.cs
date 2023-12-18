using AutoMapper;
using humanResourceProject.Application.Services.Abstract.IAdvanceServices;
using humanResourceProject.Application.Services.Concrete.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.IRepository.AdvanceRepo;
using humanResourceProject.Domain.IRepository.BaseRepos;
using humanResourceProject.Models.DTOs;

namespace humanResourceProject.Application.Services.Concrete.AdvanceServices
{
    public class AdvanceWriteService : BaseWriteService<Advance>, IAdvanceWriteService
    {
        private readonly IBaseWriteRepository<Advance> _baseWriteRepository;
        private readonly IBaseReadRepository<Advance> _baseReadRepository;
        private readonly IAdvanceWriteRepository _advanceWriteRepository;
        private readonly IMapper _mapper;
        public AdvanceWriteService(IBaseWriteRepository<Advance> writeRepository, IBaseReadRepository<Advance> readRepository, IAdvanceWriteRepository advanceWriteRepository, IMapper mapper) : base(writeRepository, readRepository)
        {
            _baseWriteRepository = writeRepository;
            _baseReadRepository = readRepository;
            _advanceWriteRepository = advanceWriteRepository;
            _mapper = mapper;
        }

        public async Task DeleteAdvance(Guid id)
        {
            Advance advance = await _baseReadRepository.GetById(id);
            advance.Status = Domain.Enum.Status.Deleted;
            advance.DeleteDate = DateTime.Now;
            await _baseWriteRepository.Delete(id);
        }

        public async Task<bool> InsertAdvance(AdvanceDTO model)
        {
            if (model == null)
                return false;

            Advance newAdvance = _mapper.Map<Advance>(model);
            newAdvance.Status = Domain.Enum.Status.Active;
            newAdvance.CreateDate = DateTime.Now;
            if (await _baseWriteRepository.Insert(newAdvance))
                return true;
            else
                return false;
        }

        public async Task<bool> UpdateAdvance(AdvanceDTO model)
        {
            Advance advance = await _baseReadRepository.GetSingleDefault(x => x.Id == model.Id);
            if (advance == null)
                return false;
            AdvanceDTO advanceDTO = _mapper.Map<AdvanceDTO>(advance);

            advanceDTO.AdvanceAmount = model.AdvanceAmount;
            advanceDTO.ExpiryDate = model.ExpiryDate;
            advanceDTO.Description = model.Description;
            advanceDTO.AdvanceType = model.AdvanceType;
            advanceDTO.AdvanceStatus = model.AdvanceStatus;
            advanceDTO.Status = Domain.Enum.Status.Modified;
            advanceDTO.UpdateDate = DateTime.Now;


            if (await _baseWriteRepository.Update(advance))
                return true;
            else
                return false;

        }
    }
}
