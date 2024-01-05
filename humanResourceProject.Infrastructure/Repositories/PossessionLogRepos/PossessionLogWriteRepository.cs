using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Infrastructure.Context;
using humanResourceProject.Infrastructure.Repositories.BaseRepos;

namespace humanResourceProject.Infrastructure.Repositories.PossessionLogRepos
{
    public class PossessionLogWriteRepository : BaseWriteRepository<PossessionLog>
    {
        public PossessionLogWriteRepository(AppDbContext context) : base(context)
        {
        }
    }
}
