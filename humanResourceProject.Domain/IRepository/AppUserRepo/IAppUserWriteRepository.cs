using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.IRepository.BaseRepos;

namespace humanResourceProject.Domain.IRepository.AppUserRepo
{
    public interface IAppUserWriteRepository : IBaseWriteRepository<AppUser>
    {
    }
}
