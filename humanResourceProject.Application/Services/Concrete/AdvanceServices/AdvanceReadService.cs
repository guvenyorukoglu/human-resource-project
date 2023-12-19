using AutoMapper;
using humanResourceProject.Application.Services.Abstract.IAdvanceServices;
using humanResourceProject.Application.Services.Concrete.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.Enum;
using humanResourceProject.Domain.IRepository.AdvanceRepo;
using humanResourceProject.Domain.IRepository.BaseRepos;
using humanResourceProject.Models.DTOs;
using humanResourceProject.Models.VMs;

namespace humanResourceProject.Application.Services.Concrete.AdvanceServices
{
    public class AdvanceReadService : BaseReadService<Advance>, IAdvanceReadService
    {
        private readonly IAdvanceReadRepository _advanceReadRepository;
        private readonly IBaseReadRepository<Advance> _baseReadRepository;
        private readonly IMapper _mapper;

        public AdvanceReadService(IBaseReadRepository<Advance> baseReadRepository, IAdvanceReadRepository advanceReadRepository, IMapper mapper) : base(baseReadRepository)
        {
            _advanceReadRepository = advanceReadRepository;
            _baseReadRepository = baseReadRepository;
            _mapper = mapper;
        }

        public async Task<AdvanceDTO> GetAdvanceById(Guid id)
        {
            Advance advance = await _baseReadRepository.GetById(id);
            AdvanceDTO advanceDTO = _mapper.Map<AdvanceDTO>(advance);
            return advanceDTO;
        }

        public async Task<List<AdvanceVM>> GetAllAdvances()
        {
            List<AdvanceVM>? advances = await _advanceReadRepository.GetFilteredList(
                                                select: x => new AdvanceVM
                                                {
                                                    AdvanceAmount = x.AmountOfAdvance,
                                                    Description = x.Explanation,
                                                    EmployeeName = x.Employee.FirstName,
                                                    EmployeeSurname = x.Employee.LastName,
                                                    AdvanceDate = x.ExpiryDate,
                                                    Status = x.AdvanceStatus
                                                },
                                                where: x => x.Status != Status.Deleted || x.Status != Status.Inactive,
                                                orderBy: x => x.OrderByDescending(x => x.ExpiryDate));
            return advances;
        }

        public async Task<List<AdvanceVM>> GetAdvancesByDepartmentId(Guid id)
        {
            List<AdvanceVM>? advances = await _advanceReadRepository.GetFilteredList(
                                                select: x => new AdvanceVM
                                                {
                                                    AdvanceAmount = x.AmountOfAdvance,
                                                    Description = x.Explanation,
                                                    EmployeeName = x.Employee.FirstName,
                                                    EmployeeSurname = x.Employee.LastName,
                                                    AdvanceDate = x.ExpiryDate,
                                                    Status = x.AdvanceStatus
                                                },
                                                where: x => (x.Status != Status.Deleted || x.Status != Status.Inactive) && x.Employee.DepartmentId == id,
                                                orderBy: x => x.OrderByDescending(x => x.ExpiryDate));
            return advances;
        }

        public async Task<List<AdvanceVM>> GetAdvancesByEmployeeId(Guid id)
        {
            List<AdvanceVM>? advances = await _advanceReadRepository.GetFilteredList(
                                                select: x => new AdvanceVM
                                                {
                                                    AdvanceAmount = x.AmountOfAdvance,
                                                    Description = x.Explanation,
                                                    EmployeeName = x.Employee.FirstName,
                                                    EmployeeSurname = x.Employee.LastName,
                                                    AdvanceDate = x.ExpiryDate,
                                                    Status = x.AdvanceStatus
                                                },
                                                where: x => (x.Status != Status.Deleted || x.Status != Status.Inactive) && x.Employee.Id == id,
                                                orderBy: x => x.OrderByDescending(x => x.ExpiryDate));
            return advances;
        }

        
    }
}
