using humanResourceProject.Application.Services.Abstract.IBaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Models.DTOs;
using Microsoft.AspNetCore.Identity;
namespace humanResourceProject.Application.Services.Abstract.IExpenseServices
{
    public interface IExpenseWriteService : IBaseWriteService<Expense>
    {
        Task<IdentityResult> InsertExpense(ExpenseDTO model);

        Task<IdentityResult> UpdateExpense(UpdateExpenseDTO model);

        Task<IdentityResult> DeleteExpense(Guid id);
    }
}
