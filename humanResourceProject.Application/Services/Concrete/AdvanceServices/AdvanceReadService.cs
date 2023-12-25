using AutoMapper;
using humanResourceProject.Application.Services.Abstract.IAdvanceServices;
using humanResourceProject.Application.Services.Concrete.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.Enum;
using humanResourceProject.Domain.IRepository.BaseRepos;
using humanResourceProject.Models.DTOs;
using humanResourceProject.Models.VMs;
using Microsoft.EntityFrameworkCore;

namespace humanResourceProject.Application.Services.Concrete.AdvanceServices
{
    public class AdvanceReadService : BaseReadService<Advance>, IAdvanceReadService
    {
        private readonly IBaseReadRepository<Advance> _advanceReadRepository;
        private readonly IMapper _mapper;

        public AdvanceReadService(IBaseReadRepository<Advance> advanceReadRepository, IMapper mapper) : base(advanceReadRepository)
        {
            _advanceReadRepository = advanceReadRepository;
            _mapper = mapper;
        }

        public async Task<AdvanceDTO> GetAdvanceById(Guid id)
        {
            Advance advance = await _advanceReadRepository.GetById(id);
            AdvanceDTO advanceDTO = _mapper.Map<AdvanceDTO>(advance);
            return advanceDTO;
        }

        public async Task<List<AdvanceVM>> GetAllAdvances()
        {
            List<AdvanceVM>? advances = await _advanceReadRepository.GetFilteredList(
                                                select: x => new AdvanceVM
                                                {
                                                    Id = x.Id,
                                                    AdvanceType = x.AdvanceType,
                                                    CreateDate = x.CreateDate,
                                                    EmployeeId = x.EmployeeId,
                                                    AmountOfAdvance = x.AmountOfAdvance,
                                                    Explanation = x.Explanation,
                                                    FirstName = x.Employee.FirstName,
                                                    LastName = x.Employee.LastName,
                                                    ExpiryDate = x.ExpiryDate,
                                                    AdvanceStatus = x.AdvanceStatus,
                                                    Currency = x.Currency
                                                },
                                                where: x => x.Status != Status.Deleted && x.Status != Status.Inactive,
                                                orderBy: x => x.OrderByDescending(x => x.CreateDate),
                                                include: x => x.Include(x => x.Employee));
            return advances;
        }

        public async Task<List<AdvanceVM>> GetAdvancesByDepartmentId(Guid id)
        {
            List<AdvanceVM>? advances = await _advanceReadRepository.GetFilteredList(
                                                select: x => new AdvanceVM
                                                {
                                                    Id = x.Id,
                                                    AdvanceType = x.AdvanceType,
                                                    CreateDate = x.CreateDate,
                                                    EmployeeId = x.EmployeeId,
                                                    AmountOfAdvance = x.AmountOfAdvance,
                                                    Explanation = x.Explanation,
                                                    FirstName = x.Employee.FirstName,
                                                    LastName = x.Employee.LastName,
                                                    ExpiryDate = x.ExpiryDate,
                                                    AdvanceStatus = x.AdvanceStatus,
                                                    Currency = x.Currency
                                                },
                                                where: x => (x.Status != Status.Deleted && x.Status != Status.Inactive) && x.Employee.DepartmentId == id,
                                                orderBy: x => x.OrderByDescending(x => x.CreateDate),
                                                include: x => x.Include(x => x.Employee));
            return advances;
        }

        public async Task<List<AdvancePersonnelVM>> GetAdvancesByEmployeeId(Guid id)
        {
            List<AdvancePersonnelVM>? advances = await _advanceReadRepository.GetFilteredList(
                                                select: x => new AdvancePersonnelVM
                                                {
                                                    Id = x.Id,
                                                    EmployeeId = x.EmployeeId,
                                                    AmountOfAdvance = x.AmountOfAdvance,
                                                    ExpiryDate = x.ExpiryDate,
                                                    Status = x.Status,
                                                    AdvanceType = x.AdvanceType,
                                                    Explanation = x.Explanation,
                                                    AdvanceStatus = x.AdvanceStatus,
                                                    CreateDate = x.CreateDate,
                                                    Currency = x.Currency
                                                },
                                                where: x => (x.Status != Status.Deleted && x.Status != Status.Inactive) && x.Employee.Id == id,
                                                orderBy: x => x.OrderByDescending(x => x.CreateDate),
                                                include: x => x.Include(x => x.Employee));
            return advances;
        }

        public async Task<UpdateAdvanceDTO> GetUpdateAdvanceDTO(Guid id)
        {
            Advance advance = await _advanceReadRepository.GetById(id);
            UpdateAdvanceDTO updateAdvanceDTO = _mapper.Map<UpdateAdvanceDTO>(advance);
            return updateAdvanceDTO;
        }

        public async Task<List<AdvanceVM>> GetAdvancesByCompanyId(Guid id)
        {
            List<AdvanceVM>? advances = await _advanceReadRepository.GetFilteredList(
                                                select: x => new AdvanceVM
                                                {
                                                    Id = x.Id,
                                                    AdvanceType = x.AdvanceType,
                                                    CreateDate = x.CreateDate,
                                                    EmployeeId = x.EmployeeId,
                                                    AmountOfAdvance = x.AmountOfAdvance,
                                                    Explanation = x.Explanation,
                                                    FirstName = x.Employee.FirstName,
                                                    LastName = x.Employee.LastName,
                                                    ExpiryDate = x.ExpiryDate,
                                                    AdvanceStatus = x.AdvanceStatus,
                                                    Currency = x.Currency
                                                },
                                                where: x => (x.Status != Status.Deleted && x.Status != Status.Inactive) && x.Employee.Department.CompanyId == id,
                                                orderBy: x => x.OrderByDescending(x => x.CreateDate),
                                                include: x => x.Include(x => x.Employee).ThenInclude(e => e.Department));
            return advances;
        }
    }
}
