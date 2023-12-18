using humanResourceProject.Application.Services.Abstract.IExpenseServices;
using humanResourceProject.Application.Services.Concrete.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.IRepository.BaseRepos;
using humanResourceProject.Models.DTOs;
using humanResourceProject.Models.VMs;

namespace humanResourceProject.Application.Services.Concrete.ExpenseServices
{
    public class ExpenseReadService : BaseReadService<Expense>, IExpenseReadService
    {
        public ExpenseReadService(IBaseReadRepository<Expense> readRepository) : base(readRepository)
        {
        }

        public Task<List<ExpenseVM>> GetAllExpenses()
        {
            throw new NotImplementedException();
        }

        public ExpenseDTO GetExpenseById(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
