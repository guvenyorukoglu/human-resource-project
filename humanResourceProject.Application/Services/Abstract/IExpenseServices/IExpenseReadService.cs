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
        Task<List<ExpensePersonnelVM>> GetExpensesByEmployeeId(Guid id);
        Task<List<ExpenseVM>> GetExpensesByManagerId(Guid id);
        Task<List<ExpenseVM>> GetExpensesByCompanyId(Guid id);
        Task<UpdateExpenseDTO> GetUpdateExpenseDTO(Guid id);
        Task<ExpenseDTO> GetExpenseDTO(Guid employeeId);
    }
}
