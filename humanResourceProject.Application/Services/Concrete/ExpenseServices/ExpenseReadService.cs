using AutoMapper;
using humanResourceProject.Application.Services.Abstract.IExpenseServices;
using humanResourceProject.Application.Services.Concrete.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.Enum;
using humanResourceProject.Domain.IRepository.BaseRepos;
using humanResourceProject.Models.DTOs;
using humanResourceProject.Models.VMs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace humanResourceProject.Application.Services.Concrete.ExpenseServices
{
    public class ExpenseReadService : BaseReadService<Expense>, IExpenseReadService
    {
        private readonly IBaseReadRepository<Expense> _expenseReadRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        public ExpenseReadService(IBaseReadRepository<Expense> expenseReadRepository, IMapper mapper, UserManager<AppUser> userManager) : base(expenseReadRepository)
        {
            _expenseReadRepository = expenseReadRepository;
            _mapper = mapper;
            _userManager = userManager;
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
                                                  DepartmentId = (Guid)x.Employee.DepartmentId,
                                                  ManagerId = (Guid)x.Employee.ManagerId,
                                                  ExpenseStatus = x.ExpenseStatus,
                                                  DateOfExpense = x.DateOfExpense,
                                                  FilePath = x.FilePath,
                                                  UploadPath = x.UploadPath,
                                                  CreateDate = x.CreateDate,
                                                  ExpenseType = x.ExpenseType,
                                                  ExpenseNo = x.ExpenseNo
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
                                                  ExpenseType = x.ExpenseType,
                                                  ManagerFullName = $"{x.Employee.Manager.FirstName} {x.Employee.Manager.LastName}",
                                                  ExpenseNo = x.ExpenseNo
                                              },
                                              where: x => (x.Status != Status.Deleted && x.Status != Status.Inactive) && x.Employee.Id == id,
                                              orderBy: x => x.OrderByDescending(x => x.CreateDate),
                                              include: x => x.Include(x => x.Employee).ThenInclude(x => x.Manager));
            return expenses;
        }

        public async Task<List<ExpenseVM>> GetExpensesByManagerId(Guid id)
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
                                                  DepartmentId = (Guid)x.Employee.DepartmentId,
                                                  ManagerId = (Guid)x.Employee.ManagerId,
                                                  ExpenseStatus = x.ExpenseStatus,
                                                  DateOfExpense = x.DateOfExpense,
                                                  FilePath = x.FilePath,
                                                  UploadPath = x.UploadPath,
                                                  CreateDate = x.CreateDate,
                                                  ExpenseType = x.ExpenseType,
                                                  ExpenseNo = x.ExpenseNo
                                              },
                                              where: x => (x.Status != Status.Deleted && x.Status != Status.Inactive) && x.Employee.ManagerId == id,
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
                                                    DepartmentId = (Guid)x.Employee.DepartmentId,
                                                    ManagerId = (Guid)x.Employee.ManagerId,
                                                    ExpenseStatus = x.ExpenseStatus,
                                                    DateOfExpense = x.DateOfExpense,
                                                    FilePath = x.FilePath,
                                                    UploadPath = x.UploadPath,
                                                    CreateDate = x.CreateDate,
                                                    ExpenseType = x.ExpenseType,
                                                    ExpenseNo = x.ExpenseNo
                                                },
                                                where: x => (x.Status != Status.Deleted && x.Status != Status.Inactive) && x.Employee.CompanyId == id,
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

        public async Task<ExpenseDTO> GetExpenseDTO(Guid employeeId)
        {
            AppUser employee = _userManager.FindByIdAsync(employeeId.ToString()).Result;
            AppUser managerOfEmployee = _userManager.FindByIdAsync(employee.ManagerId.ToString()).Result;
            ExpenseDTO expenseDTO = new ExpenseDTO()
            {
                EmployeeId = employee.Id,
                ManagerFullName = managerOfEmployee.FirstName + " " + managerOfEmployee.LastName,
                ManagerEmail = managerOfEmployee.Email,
                CreateDate = DateTime.Now
            };
            return expenseDTO;
        }

        public async Task<List<DashboardExpenseVM>> FillDashboardExpenseVM(Guid id)
        {
            List<DashboardExpenseVM> dashboardExpenseVMs = await _expenseReadRepository.GetFilteredList(
                                              select: x => new DashboardExpenseVM
                                              {
                                                  ExpenseNo = x.ExpenseNo,
                                                  AmountOfExpense = x.AmountOfExpense,
                                                  CreateDate = x.CreateDate

                                              },
                                               where: x => (x.Status != Status.Deleted && x.Status != Status.Inactive) && x.Employee.Id == id,
                                               orderBy: x => x.OrderByDescending(x => x.CreateDate),
                                               include: x => x.Include(x => x.Employee)); ;
            return dashboardExpenseVMs;
        }
    }
}
