using AutoMapper;
using humanResourceProject.Application.Services.Abstract.IPossessionServices;
using humanResourceProject.Application.Services.Concrete.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.IRepository.BaseRepos;
using humanResourceProject.Models.DTOs;
using humanResourceProject.Models.VMs;
using Microsoft.EntityFrameworkCore;

namespace humanResourceProject.Application.Services.Concrete.PossessionServices
{
    public class PossessionReadService : BaseReadService<Possession>, IPossessionReadService
    {
        private readonly IBaseReadRepository<Possession> _readRepository;
        private readonly IMapper _mapper;

        public PossessionReadService(IBaseReadRepository<Possession> readRepository, IMapper mapper) : base(readRepository)
        {
            _readRepository = readRepository;
            _mapper = mapper;
        }

        public async Task<List<PossessionVM>> GetAllPossessions()
        {
            List<PossessionVM>? possessions = await _readRepository.GetFilteredList(
                                              select: x => new PossessionVM
                                              {
                                                  Id = x.Id,
                                                  Brand = x.Brand,
                                                  PossessionModel = x.PossessionModel,
                                                  Barcode = x.Barcode,
                                                  Details = x.Details,
                                                  PossessionType = x.PossessionType
                                              },
                                              where: x => x.Status != Domain.Enum.Status.Deleted && x.Status != Domain.Enum.Status.Inactive,
                                              orderBy: x => x.OrderByDescending(x => x.Brand));
            return possessions;
        }

        public async Task<PossessionDTO> GetPossessionById(Guid id)
        {
            Possession possession = await _readRepository.GetById(id);
            PossessionDTO possessionDTO = _mapper.Map<PossessionDTO>(possession);
            return possessionDTO;
        }

        public async Task<List<PossessionVM>> GetPossessionsByCompanyId(Guid id)
        {
            List<PossessionVM> possessions = await _readRepository.GetFilteredList(
                                                             select: x => new PossessionVM
                                                             {
                                                                 Id = x.Id,
                                                                 Brand = x.Brand,
                                                                 PossessionModel = x.PossessionModel,
                                                                 Barcode = x.Barcode,
                                                                 Details = x.Details,
                                                                 PossessionType = x.PossessionType,
                                                                 ToWhomItBelongs = x.Company.Employees.FirstOrDefault(u => u.PossessionLogs.Any(pl => pl.PossessionId == x.Id && pl.EndDateOfPossession == null)).FullName
                                                             },
                                                             where: x => (x.Status != Domain.Enum.Status.Deleted && x.Status != Domain.Enum.Status.Inactive) && x.CompanyId == id,
                                                             orderBy: x => x.OrderBy(x => x.Brand),
                                                             include: x => x.Include(x => x.Company).ThenInclude(c => c.Employees).ThenInclude(u => u.PossessionLogs));
            return possessions;
        }

        public async Task<UpdatePossessionDTO> GetUpdatePossessionDTO(Guid id)
        {
            Possession possession = await _readRepository.GetDefault(x => x.Id == id);

            if (possession == null)
                return null;

            UpdatePossessionDTO updatePossessionDTO = _mapper.Map<UpdatePossessionDTO>(possession);
            return updatePossessionDTO;
        }
    }
}
