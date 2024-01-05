using humanResourceProject.Application.Services.Abstract.IPossessionLogServices;
using humanResourceProject.Application.Services.Concrete.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.IRepository.BaseRepos;
using humanResourceProject.Models.DTOs;
using Microsoft.AspNetCore.Identity;

namespace humanResourceProject.Application.Services.Concrete.PossessionLogServices
{
    public class PossessionLogWriteService : BaseWriteService<PossessionLog>, IPossessionLogWriteService
    {
        private readonly IBaseWriteRepository<PossessionLog> _writeRepository;
        private readonly IBaseReadRepository<PossessionLog> _readRepository;
        public PossessionLogWriteService(IBaseWriteRepository<PossessionLog> writeRepository, IBaseReadRepository<PossessionLog> readRepository) : base(writeRepository, readRepository)
        {
            _writeRepository = writeRepository;
            _readRepository = readRepository;
        }

        public async Task<IdentityResult> AssignPossession(AssignPossessionDTO model)
        {
            PossessionLog possessionLog = await _readRepository.GetDefault(x => x.EmployeeId == model.EmployeeId && x.PossessionId == model.PossessionId && x.EndDateOfPossession == null);

            if (possessionLog != null)
                return IdentityResult.Failed(new IdentityError { Description = "Bu zimmet zaten bu çalışana atanmış." });

            var result = await _writeRepository.Insert(new PossessionLog
            {
                EmployeeId = model.EmployeeId,
                PossessionId = model.PossessionId,
                StartDateOfPossession = DateTime.Now,
                CreateDate = DateTime.Now,
                Status = Domain.Enum.Status.Active
            });

            if (!result)
                return IdentityResult.Failed(new IdentityError { Description = "Zimmet atama işlemi başarısız." });

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> TakeBackPossession(Guid id)
        {
            PossessionLog possessionLog = await _readRepository.GetDefault(x => x.PossessionId == id && x.EndDateOfPossession == null); // Zimmeti geri alınmamış olanı getir.

            if (possessionLog == null)
                return IdentityResult.Failed(new IdentityError { Description = "Bu zimmet zaten geri alınmış." });

            possessionLog.EndDateOfPossession = DateTime.Now;
            var result = await _writeRepository.Update(possessionLog);

            if (!result)
                return IdentityResult.Failed(new IdentityError { Description = "Zimmet geri alma işlemi başarısız." });

            return IdentityResult.Success;
        }
    }
}
