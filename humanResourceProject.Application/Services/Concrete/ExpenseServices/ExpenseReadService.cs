using AutoMapper;
using humanResourceProject.Application.Services.Abstract.IExpenseServices;
using humanResourceProject.Application.Services.Concrete.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.Enum;
using humanResourceProject.Domain.IRepository.BaseRepos;
using humanResourceProject.Domain.IRepository.ExpenseRepo;
using humanResourceProject.Models.DTOs;
using humanResourceProject.Models.VMs;

namespace humanResourceProject.Application.Services.Concrete.ExpenseServices
{
    public class ExpenseReadService : BaseReadService<Expense>, IExpenseReadService
    {
        private readonly IBaseReadRepository<Expense> _baseReadRepository;
        private readonly IExpenseReadRepository _expenseReadRepository;
        private readonly IMapper _mapper;
        public ExpenseReadService(IBaseReadRepository<Expense> baseReadRepository, IExpenseReadRepository expenseReadRepository, IMapper mapper) : base(baseReadRepository)
        {
            _baseReadRepository = baseReadRepository;
            _expenseReadRepository = expenseReadRepository;
            _mapper = mapper;
        }

        public async Task<List<ExpenseVM>> GetAllExpenses()
        {
            List<ExpenseVM>? expenses = await _expenseReadRepository.GetFilteredList(
                                              select: x => new ExpenseVM
                                              {
                                                Description = x.Explanation,
                                                Amount = x.AmountOfExpense,
                                                EmployeeName = x.Employee.FirstName,
                                                EmployeeSurname = x.Employee.LastName,
                                                Status = x.ExpenseStatus
                                              },
                                              where: x => x.ExpenseStatus != RequestStatus.Rejected,
                                              orderBy: x => x.OrderByDescending(x => x.CreateDate)
                                              );
            return expenses;
        }

        public async Task<ExpenseDTO> GetExpenseById(Guid id)
        {
            Expense expense = await _baseReadRepository.GetById(id);
            ExpenseDTO expenseDTO = _mapper.Map<ExpenseDTO>(expense);
            return expenseDTO;
        }
    }
}
