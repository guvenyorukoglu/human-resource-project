using humanResourceProject.Application.Services.Abstract.IPossessionLogServices;
using humanResourceProject.Application.Services.Concrete.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.IRepository.BaseRepos;
using humanResourceProject.Models.VMs;

namespace humanResourceProject.Application.Services.Concrete.PossessionLogServices
{
    public class PossessionLogReadService : BaseReadService<PossessionLog>, IPossessionLogReadService
    {
        private readonly IBaseReadRepository<PossessionLog> _readRepository;
        private readonly IBaseReadRepository<AppUser> _appUserRepository;
        private readonly IBaseReadRepository<Possession> _possessionRepository;
        List<PersonelPossessionVM> personelPossessionVMs;
        public PossessionLogReadService(IBaseReadRepository<PossessionLog> readRepository, IBaseReadRepository<AppUser> appUserRepository, IBaseReadRepository<Possession> possessionRepository) : base(readRepository)
        {
            _readRepository = readRepository;
            _appUserRepository = appUserRepository;
            _possessionRepository = possessionRepository;
            personelPossessionVMs = new List<PersonelPossessionVM>();
        }

        public async Task<List<PersonelPossessionVM>> GetPossessionsByEmployeeId(Guid employeeId)
        {
            AppUser appUser = await _appUserRepository.GetSingleDefault(x => x.Id == employeeId);
           
            List<PossessionLog> possessionLogs = await _readRepository.GetDefaults(x => x.EmployeeId == employeeId && x.EndDateOfPossession == null);

            foreach (var item in possessionLogs)
            {
                Possession possession = await _possessionRepository.GetSingleDefault(x => x.Id == item.PossessionId);
                PersonelPossessionVM personelPossessionVM = new PersonelPossessionVM
                {
                    PossessionId = item.PossessionId,
                    Barcode = possession.Barcode,
                    Brand = possession.Brand,
                    PossessionModel = possession.PossessionModel,
                    Details = possession.Details,
                    PossessionType = possession.PossessionType,
                    StartDateOfPossession = item.StartDateOfPossession,
                    EndDateOfPossession = item.EndDateOfPossession
                };
                personelPossessionVMs.Add(personelPossessionVM);
            }

            return personelPossessionVMs;
        }
    }
}
