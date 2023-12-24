using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Infrastructure.Context;
using humanResourceProject.Infrastructure.Repositories.BaseRepos;

namespace humanResourceProject.Infrastructure.Repositories.ExpenseRepo
{
    public class ExpenseReadRepository : BaseReadRepository<Expense>
    {
        public ExpenseReadRepository(AppDbContext context) : base(context)
        {
        }
    }
}
