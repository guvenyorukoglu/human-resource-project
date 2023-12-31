using AutoMapper;
using humanResourceProject.Application.Services.Abstract.IJobServices;
using humanResourceProject.Application.Services.Concrete.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.IRepository.BaseRepos;
using humanResourceProject.Models.DTOs;
using Microsoft.AspNetCore.Identity;

namespace humanResourceProject.Application.Services.Concrete.JobServices
{
    public class JobWriteService : BaseWriteService<Job>, IJobWriteService
    {
        private readonly IBaseWriteRepository<Job> _jobWriteRepository;
        private readonly IBaseReadRepository<Job> _jobReadRepository;
        private readonly IMapper _mapper;
        public JobWriteService(IBaseWriteRepository<Job> jobWriteRepository, IBaseReadRepository<Job> jobReadRepository, IMapper mapper) : base(jobWriteRepository, jobReadRepository)
        {
            _jobWriteRepository = jobWriteRepository;
            _jobReadRepository = jobReadRepository;
            _mapper = mapper;
        }

        public async Task<IdentityResult> DeleteJob(Guid id)
        {
            Job job = await _jobReadRepository.GetById(id);
            job.Status = Domain.Enum.Status.Deleted;
            job.DeleteDate = DateTime.Now;
            if (await _jobWriteRepository.Delete(id))
                return IdentityResult.Success;
            else
                return IdentityResult.Failed();
        }

        public async Task<IdentityResult> InsertJob(JobDTO model)
        {
            Job job = await _jobReadRepository.GetSingleDefault(x => x.Title == model.Title && (x.Status == Domain.Enum.Status.Active || x.Status == Domain.Enum.Status.Modified));
            if (model == null || job != null)
                return IdentityResult.Failed();

            Job newJob = _mapper.Map<Job>(model);
            newJob.Status = Domain.Enum.Status.Active;
            newJob.CreateDate = DateTime.Now;
            if (await _jobWriteRepository.Insert(newJob))
                return IdentityResult.Success;
            else
                return IdentityResult.Failed();

        }

        public async Task<IdentityResult> UpdateJob(UpdateJobDTO model)
        {

            Job job = await _jobReadRepository.GetSingleDefault(x => x.Id == model.Id);
            if (job == null)
                return IdentityResult.Failed();

            // Id'si güncellenmek istenen pozisyonun Id'sine eşit olmayan pozisyonlarını getirir.
            var otherJobs = await _jobReadRepository.GetDefaults(x => x.Id != model.Id);

            // Güncellenmek istenen pozisyonun ismi başka bir pozisyonda var mı diye kontrol eder.
            bool jobExistsWithSameName = otherJobs.Any(x => x.Title == model.Title && (x.Status == Domain.Enum.Status.Active || x.Status == Domain.Enum.Status.Modified));

            if (jobExistsWithSameName == true)
                return IdentityResult.Failed();

            job.Title = model.Title;
            job.Description = model.Description;
            job.UpdateDate = DateTime.Now;
            job.Status = Domain.Enum.Status.Modified;

            if (await _jobWriteRepository.Update(job))
                return IdentityResult.Success;
            else
                return IdentityResult.Failed();
        }
    }
}
