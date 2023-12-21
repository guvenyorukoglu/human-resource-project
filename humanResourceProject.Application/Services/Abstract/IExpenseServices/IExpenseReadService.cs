using humanResourceProject.Application.Services.Abstract.IBaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Models.DTOs;
using humanResourceProject.Models.VMs;
namespace humanResourceProject.Application.Services.Abstract.IExpenseServices
{
    public interface IExpenseReadService : IBaseReadService<Expense>
    {
        Task<ExpenseDTO> GetExpenseById(Guid id);
        Task<List<ExpenseVM>> GetAllExpenses();
        Task<List<ExpenseVM>> GetExpensesByEmployeeId(Guid id);
        Task<List<ExpenseVM>> GetExpensesByDepartmentId(Guid id);
        Task<object?> GetUpdateExpenseDTO(Guid id);
    }
}
