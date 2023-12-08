using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Infrastructure.Context;
using humanResourceProject.Infrastructure.Repositories.BaseRepos;

namespace humanResourceProject.Infrastructure.Repositories.CompanyRepos
{
    public class CompanyWriteRepository : BaseWriteRepository<Company>
    {
        public CompanyWriteRepository(AppDbContext context) : base(context)
        {
        }
    }
}
