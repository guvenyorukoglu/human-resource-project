using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.IRepository.BaseRepos;

namespace humanResourceProject.Domain.IRepository.ExpenseRepo
{
    public interface IExpenseReadRepository : IBaseReadRepository<Expense>
    {
    }
}
