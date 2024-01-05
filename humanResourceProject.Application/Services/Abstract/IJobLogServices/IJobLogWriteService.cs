using humanResourceProject.Application.Services.Abstract.IBaseServices;
using humanResourceProject.Domain.Entities.Concrete;

namespace humanResourceProject.Application.Services.Abstract.IJobLogServices
{
    public interface IJobLogWriteService : IBaseWriteService<JobLog>
    {
    }
}
