using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Infrastructure.Context;
using humanResourceProject.Infrastructure.Repositories.BaseRepos;

namespace humanResourceProject.Infrastructure.Repositories.PossessionRepos
{
    public class PossessionWriteRepository : BaseWriteRepository<Possession>
    {
        public PossessionWriteRepository(AppDbContext context) : base(context)
        {
        }
    }
}
