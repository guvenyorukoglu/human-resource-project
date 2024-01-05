using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Infrastructure.Context;
using humanResourceProject.Infrastructure.Repositories.BaseRepos;

namespace humanResourceProject.Infrastructure.Repositories.PossessionLogRepos
{
    public class PossessionLogReadRepository : BaseReadRepository<PossessionLog>
    {
        public PossessionLogReadRepository(AppDbContext context) : base(context)
        {
        }
    }
}
