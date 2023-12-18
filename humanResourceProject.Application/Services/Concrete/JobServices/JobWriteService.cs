using AutoMapper;
using humanResourceProject.Application.Services.Abstract.IJobServices;
using humanResourceProject.Application.Services.Concrete.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.IRepository.BaseRepos;
using humanResourceProject.Domain.IRepository.DepartmentRepo;
using humanResourceProject.Domain.IRepository.JobRepo;
using humanResourceProject.Models.DTOs;

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

        public async Task DeleteJob(Guid id)
        {
            Job job = await _baseReadRepository.GetById(id);
            job.Status = Domain.Enum.Status.Deleted;
            job.DeleteDate = DateTime.Now;
            await _baseWriteRepository.Delete(id);
        }

        public async Task<bool> InsertJob(JobDTO model)
        {
            if (model == null)
                return false;

            Job newJob = _mapper.Map<Job>(model);
            newJob.Status = Domain.Enum.Status.Active;
            newJob.CreateDate = DateTime.Now;
            if (await _baseWriteRepository.Insert(newJob))
                return true;
            else
                return false;

        }

        public async Task<bool> UpdateJob(JobDTO model)
        {
            Job job = await _baseReadRepository.GetSingleDefault(x => x.Id == model.Id);
            if (job == null)
                return false;

            JobDTO jobDTO = _mapper.Map<JobDTO>(job);

            jobDTO.Name = model.Name;
            jobDTO.UpdateDate = DateTime.Now;
            jobDTO.Status = Domain.Enum.Status.Modified;

            if (await _baseWriteRepository.Update(_mapper.Map<Job>(jobDTO)))
                return true;
            else
                return false;
        }
    }
}
