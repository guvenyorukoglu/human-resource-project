using AutoMapper;
using humanResourceProject.Application.Services.Abstract.IExpenseServices;
using humanResourceProject.Application.Services.Concrete.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.Enum;
using humanResourceProject.Domain.IRepository.BaseRepos;
using humanResourceProject.Models.DTOs;
using humanResourceProject.Models.VMs;
using Microsoft.EntityFrameworkCore;

namespace humanResourceProject.Application.Services.Concrete.ExpenseServices
{
    public class ExpenseReadService : BaseReadService<Expense>, IExpenseReadService
    {
        private readonly IBaseReadRepository<Expense> _expenseReadRepository;
        private readonly IMapper _mapper;
        public ExpenseReadService(IBaseReadRepository<Expense> expenseReadRepository, IMapper mapper) : base(expenseReadRepository)
        {
            _expenseReadRepository = expenseReadRepository;
            _mapper = mapper;
        }

        public async Task<ExpenseDTO> GetExpenseById(Guid id)
        {
            Expense expense = await _expenseReadRepository.GetById(id);
            ExpenseDTO expenseDTO = _mapper.Map<ExpenseDTO>(expense);
            return expenseDTO;
        }

        public async Task<List<ExpenseVM>> GetAllExpenses()
        {
            List<ExpenseVM>? expenses = await _expenseReadRepository.GetFilteredList(
                                              select: x => new ExpenseVM
                                              {
                                                  Id = x.Id,
                                                  Explanation = x.Explanation,
                                                  AmountOfExpense = x.AmountOfExpense,
                                                  Currency = x.Currency,
                                                  FirstName = x.Employee.FirstName,
                                                  LastName = x.Employee.LastName,
                                                  EmployeeId = x.EmployeeId,
                                                  ExpenseStatus = x.ExpenseStatus,
                                                  DateOfExpense = x.DateOfExpense,
                                                  FilePath = x.FilePath,
                                                  UploadPath = x.UploadPath,
                                                  CreateDate = x.CreateDate,
                                                  ExpenseType = x.ExpenseType
                                              },
                                              where: x => x.Status != Status.Deleted && x.Status != Status.Inactive,
                                              orderBy: x => x.OrderByDescending(x => x.CreateDate),
                                              include: x => x.Include(x => x.Employee).ThenInclude(x => x.Manager));
            return expenses;
        }

        public async Task<List<ExpensePersonnelVM>> GetExpensesByEmployeeId(Guid id)
        {
            List<ExpensePersonnelVM>? expenses = await _expenseReadRepository.GetFilteredList(
                                              select: x => new ExpensePersonnelVM
                                              {
                                                  Id = x.Id,
                                                  Explanation = x.Explanation,
                                                  AmountOfExpense = x.AmountOfExpense,
                                                  Currency = x.Currency,
                                                  EmployeeId = x.EmployeeId,
                                                  ExpenseStatus = x.ExpenseStatus,
                                                  FilePath = x.FilePath,
                                                  DateOfExpense = x.DateOfExpense,
                                                  UploadPath = x.UploadPath,
                                                  CreateDate = x.CreateDate,
                                                  ExpenseType = x.ExpenseType
                                              },
                                              where: x => (x.Status != Status.Deleted && x.Status != Status.Inactive) && x.Employee.Id == id,
                                              orderBy: x => x.OrderByDescending(x => x.CreateDate),
                                              include: x => x.Include(x => x.Employee));
            return expenses;
        }

        public async Task<List<ExpenseVM>> GetExpensesByDepartmentId(Guid id)
        {
            List<ExpenseVM>? expenses = await _expenseReadRepository.GetFilteredList(
                                              select: x => new ExpenseVM
                                              {
                                                  Id = x.Id,
                                                  Explanation = x.Explanation,
                                                  AmountOfExpense = x.AmountOfExpense,
                                                  Currency = x.Currency,
                                                  FirstName = x.Employee.FirstName,
                                                  LastName = x.Employee.LastName,
                                                  EmployeeId = x.EmployeeId,
                                                  ExpenseStatus = x.ExpenseStatus,
                                                  DateOfExpense = x.DateOfExpense,
                                                  FilePath = x.FilePath,
                                                  UploadPath = x.UploadPath,
                                                  CreateDate = x.CreateDate,
                                                  ExpenseType = x.ExpenseType
                                              },
                                              where: x => (x.Status != Status.Deleted && x.Status != Status.Inactive) && x.Employee.DepartmentId == id,
                                              orderBy: x => x.OrderByDescending(x => x.CreateDate),
                                              include: x => x.Include(x => x.Employee).ThenInclude(x => x.Manager));
            return expenses;
        }

        public async Task<List<ExpenseVM>> GetExpensesByCompanyId(Guid id)
        {
            List<ExpenseVM>? expenses = await _expenseReadRepository.GetFilteredList(
                                                             select: x => new ExpenseVM
                                                             {
                                                                 Id = x.Id,
                                                                 Explanation = x.Explanation,
                                                                 AmountOfExpense = x.AmountOfExpense,
                                                                 Currency = x.Currency,
                                                                 FirstName = x.Employee.FirstName,
                                                                 LastName = x.Employee.LastName,
                                                                 EmployeeId = x.EmployeeId,
                                                                 ExpenseStatus = x.ExpenseStatus,
                                                                 DateOfExpense = x.DateOfExpense,
                                                                 FilePath = x.FilePath,
                                                                 UploadPath = x.UploadPath,
                                                                 CreateDate = x.CreateDate,
                                                                 ExpenseType = x.ExpenseType
                                                             },
                                                where: x => (x.Status != Status.Deleted && x.Status != Status.Inactive) && x.Employee.Department.CompanyId == id,
                                                orderBy: x => x.OrderByDescending(x => x.CreateDate),
                                                include: x => x.Include(x => x.Employee).ThenInclude(x => x.Manager));
            return expenses;
        }

        public async Task<UpdateExpenseDTO> GetUpdateExpenseDTO(Guid id)
        {
            Expense expense = await _expenseReadRepository.GetById(id);
            UpdateExpenseDTO updateExpenseDTO = _mapper.Map<UpdateExpenseDTO>(expense);
            return updateExpenseDTO;
        }
    }
}
