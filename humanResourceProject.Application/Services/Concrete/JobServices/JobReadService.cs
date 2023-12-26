using AutoMapper;
using humanResourceProject.Application.Services.Abstract.IJobServices;
using humanResourceProject.Application.Services.Concrete.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.IRepository.BaseRepos;
using humanResourceProject.Models.DTOs;
using humanResourceProject.Models.VMs;

namespace humanResourceProject.Application.Services.Concrete.JobServices
{
    public class JobReadService : BaseReadService<Job>, IJobReadService
    {
        private readonly IBaseReadRepository<Job> _jobReadRepository;
        private readonly IMapper _mapper;
        public JobReadService(IBaseReadRepository<Job> jobReadRepository, IMapper mapper) : base(jobReadRepository)
        {
            _jobReadRepository = jobReadRepository;
            _mapper = mapper;
        }

        public async Task<List<JobVM>> GetAllJobs()
        {
            List<JobVM>? jobs = await _jobReadRepository.GetFilteredList(
                                              select: x => new JobVM
                                              {
                                                  Id = x.Id,
                                                  Title = x.Title,
                                                  Description = x.Description
                                              },
                                              where: x => x.Status != Domain.Enum.Status.Deleted && x.Status != Domain.Enum.Status.Inactive,
                                              orderBy: x => x.OrderBy(x => x.Title)
                                              );
            return jobs;
        }

        public async Task<JobDTO> GetJobById(Guid id)
        {
            Job job = await _jobReadRepository.GetById(id);
            JobDTO jobDTO = _mapper.Map<JobDTO>(job);
            return jobDTO;
        }
    }
}
