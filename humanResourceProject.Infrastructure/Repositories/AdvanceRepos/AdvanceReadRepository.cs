using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.IRepository.AdvanceRepo;
using humanResourceProject.Infrastructure.Context;
using humanResourceProject.Infrastructure.Repositories.BaseRepos;

namespace humanResourceProject.Infrastructure.Repositories.AdvanceRepos
{
    public class AdvanceReadRepository : BaseReadRepository<Advance>
    {
        public AdvanceReadRepository(AppDbContext context) : base(context) 
        { 
        }
    }
}
