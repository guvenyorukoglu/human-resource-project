using humanResourceProject.Application.Services.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Models.DTOs;
using Microsoft.AspNetCore.Identity;
namespace humanResourceProject.Application.Services.Abstract.IExpenseServices
{
    public interface IExpenseWriteService : IBaseWriteService<Expense>
    {
        Task<IdentityResult> InsertExpense(ExpenseDTO model);

        Task<IdentityResult> UpdateExpense(ExpenseDTO model);

        Task<IdentityResult> DeleteExpense(Guid id);
    }
}
