using humanResourceProject.Application.Services.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Models.DTOs;
namespace humanResourceProject.Application.Services.Abstract.IExpenseServices
{
    public interface IExpenseWriteService : IBaseWriteService<Expense>
    {
        Task<bool> InsertExpense(ExpenseDTO model);

        Task<bool> UpdateExpense(ExpenseDTO model);

        Task DeleteExpense(Guid id);
    }
}
