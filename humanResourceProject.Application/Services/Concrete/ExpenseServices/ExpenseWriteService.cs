using humanResourceProject.Application.Services.Abstract.IExpenseServices;
using humanResourceProject.Application.Services.Concrete.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.IRepository.BaseRepos;
using humanResourceProject.Models.DTOs;

namespace humanResourceProject.Application.Services.Concrete.ExpenseServices
{
    public class ExpenseWriteService : BaseWriteService<Expense>, IExpenseWriteService
    {
        public ExpenseWriteService(IBaseWriteRepository<Expense> writeRepository, IBaseReadRepository<Expense> readRepository) : base(writeRepository, readRepository)
        {
        }

        public Task DeleteExpense(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> InsertExpense(ExpenseDTO model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateExpense(ExpenseDTO model)
        {
            throw new NotImplementedException();
        }
    }
}
