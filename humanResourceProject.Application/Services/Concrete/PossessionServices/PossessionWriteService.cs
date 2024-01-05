using AutoMapper;
using humanResourceProject.Application.Services.Abstract.IPossessionServices;
using humanResourceProject.Application.Services.Concrete.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.Enum;
using humanResourceProject.Domain.IRepository.BaseRepos;
using humanResourceProject.Models.DTOs;
using Microsoft.AspNetCore.Identity;

namespace humanResourceProject.Application.Services.Concrete.PossessionServices
{
    public class PossessionWriteService : BaseWriteService<Possession>, IPossessionWriteService
    {
        private readonly IBaseWriteRepository<Possession> _possessionWriteRepository;
        private readonly IBaseReadRepository<Possession> _possessionReadRepository;
        private readonly IMapper _mapper;
        public PossessionWriteService(IBaseWriteRepository<Possession> writeRepository, IBaseReadRepository<Possession> readRepository, IMapper mapper, IBaseReadRepository<Possession> possessionReadRepository, IBaseWriteRepository<Possession> possessionWriteRepository) : base(writeRepository, readRepository)
        {
            _possessionWriteRepository = possessionWriteRepository;
            _possessionReadRepository = possessionReadRepository;
            _mapper = mapper;
        }

        public async Task<IdentityResult> DeletePossession(Guid id)
        {
            Possession possession = await _possessionReadRepository.GetById(id);
            possession.Status = Status.Deleted;
            possession.DeleteDate = DateTime.Now;
            if (await _possessionWriteRepository.Delete(id))
                return IdentityResult.Success;
            else
                return IdentityResult.Failed();
        }

        public async Task<IdentityResult> InsertPossession(PossessionDTO model)
        {
            Possession possession = await _possessionReadRepository.GetSingleDefault(x => x.Barcode == model.Barcode && (x.Status == Status.Active || x.Status == Status.Modified));
            if (model == null || possession != null)
                return IdentityResult.Failed();

            Possession newPossession = _mapper.Map<Possession>(model);
            newPossession.Status = Status.Active;
            newPossession.CreateDate = DateTime.Now;
            if (await _possessionWriteRepository.Insert(newPossession))
                return IdentityResult.Success;
            else
                return IdentityResult.Failed();
        }

        public async Task<IdentityResult> UpdatePossession(UpdatePossessionDTO model)
        {
            // Id'ye göre zimmeti bulur.
            Possession possession = await _possessionReadRepository.GetSingleDefault(x => x.Id == model.Id);
            if (possession == null)
                return IdentityResult.Failed();

            // Id'si güncellenmek istenen zimmetin Id'sine eşit olmayan zimmetleri getirir.
            var otherPossessions = await _possessionReadRepository.GetDefaults(x => x.Id != model.Id);

            // Güncellenmek istenen zimmetin barkodu başka bir zimmette var mı diye kontrol eder.
            bool possessionExistsWithSameBarcode = otherPossessions.Any(x => x.Barcode == model.Barcode && (x.Status == Status.Active || x.Status == Status.Modified));

            if (possessionExistsWithSameBarcode == true)
                return IdentityResult.Failed();

            possession.Barcode = model.Barcode;
            possession.Brand = model.Brand;
            possession.PossessionModel = model.PossessionModel;
            possession.Details = model.Details;
            possession.PossessionType = model.PossessionType;
            possession.Status = Status.Modified;
            possession.UpdateDate = DateTime.Now;

            if (await _possessionWriteRepository.Update(possession))
                return IdentityResult.Success;
            else
                return IdentityResult.Failed();
        }
    }
}
