using AutoMapper;
using humanResourceProject.Application.Services.Abstract.IJobServices;
using humanResourceProject.Application.Services.Concrete.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.IRepository.BaseRepos;
using humanResourceProject.Domain.IRepository.DepartmentRepo;
using humanResourceProject.Domain.IRepository.JobRepo;
using humanResourceProject.Models.DTOs;
using Microsoft.AspNetCore.Identity;

namespace humanResourceProject.Application.Services.Concrete.JobServices
{
    public class JobWriteService : BaseWriteService<Job>, IJobWriteService
    {
        private readonly IBaseWriteRepository<Job> _baseWriteRepository;
        private readonly IBaseReadRepository<Job> _baseReadRepository;
        private readonly IJobWriteRepository _jobWriteRepository;
        private readonly IMapper _mapper;
        public JobWriteService(IBaseWriteRepository<Job> writeRepository, IBaseReadRepository<Job> readRepository, IJobWriteRepository jobWriteRepository, IMapper mapper) : base(writeRepository, readRepository)
        {
            _baseWriteRepository = writeRepository;
            _baseReadRepository = readRepository;
            _jobWriteRepository = jobWriteRepository;
            _mapper = mapper;
        }

        public async Task<IdentityResult> DeleteJob(Guid id)
        {
            Job job = await _baseReadRepository.GetById(id);
            job.Status = Domain.Enum.Status.Deleted;
            job.DeleteDate = DateTime.Now;
            if(await _baseWriteRepository.Delete(id))
                return IdentityResult.Success;
            else
                return IdentityResult.Failed();
        }

        public async Task<IdentityResult> InsertJob(JobDTO model)
        {
            if (model == null)
                return IdentityResult.Failed();

            Job newJob = _mapper.Map<Job>(model);
            newJob.Status = Domain.Enum.Status.Active;
            newJob.CreateDate = DateTime.Now;
            if (await _baseWriteRepository.Insert(newJob))
                return IdentityResult.Success;
            else
                return IdentityResult.Failed();

        }

        public async Task<IdentityResult> UpdateJob(JobDTO model)
        {
            Job job = await _baseReadRepository.GetSingleDefault(x => x.Id == model.Id);
            if (job == null)
                return IdentityResult.Failed();

            JobDTO jobDTO = _mapper.Map<JobDTO>(job);

            jobDTO.Name = model.Name;
            jobDTO.UpdateDate = DateTime.Now;
            jobDTO.Status = Domain.Enum.Status.Modified;

            if (await _baseWriteRepository.Update(_mapper.Map<Job>(jobDTO)))
                return IdentityResult.Success;
            else
                return IdentityResult.Failed();
        }
    }
}
