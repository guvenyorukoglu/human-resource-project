using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Infrastructure.Context;
using humanResourceProject.Infrastructure.Repositories.BaseRepos;

namespace humanResourceProject.Infrastructure.Repositories.PossessionRepos
{
    public class PossessionReadRepository : BaseReadRepository<Possession>
    {
        public PossessionReadRepository(AppDbContext context) : base(context)
        {
        }
    }
}
