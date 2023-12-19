using AutoMapper;
using humanResourceProject.Application.Services.Abstract.IExpenseServices;
using humanResourceProject.Application.Services.Concrete.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.IRepository.BaseRepos;
using humanResourceProject.Domain.IRepository.ExpenseRepo;
using humanResourceProject.Models.DTOs;
using Microsoft.AspNetCore.Identity;

namespace humanResourceProject.Application.Services.Concrete.ExpenseServices
{
    public class ExpenseWriteService : BaseWriteService<Expense>, IExpenseWriteService
    {
        private readonly IBaseWriteRepository<Expense> _baseWriteRepository;
        private readonly IBaseReadRepository<Expense> _baseReadRepository;
        private readonly IExpenseWriteRepository _expenseWriteRepository;
        private readonly IMapper _mapper;
        public ExpenseWriteService(IBaseWriteRepository<Expense> writeRepository, IBaseReadRepository<Expense> readRepository, IExpenseWriteRepository expenseWriteRepository, IMapper mapper) : base(writeRepository, readRepository)
        {
            _baseWriteRepository = writeRepository;
            _baseReadRepository = readRepository;
            _expenseWriteRepository = expenseWriteRepository;
            _mapper = mapper;
        }

        public async Task<IdentityResult> DeleteExpense(Guid id)
        {
            Expense expense = await _baseReadRepository.GetById(id);
            expense.Status = Domain.Enum.Status.Deleted;
            expense.DeleteDate = DateTime.Now;
            if(await _baseWriteRepository.Delete(id))
                return IdentityResult.Success;
            else
                return IdentityResult.Failed();
        }

        public async Task<IdentityResult> InsertExpense(ExpenseDTO model)
        {
            if (model == null)
                return IdentityResult.Failed();

            Expense newExpense = _mapper.Map<Expense>(model);
            newExpense.Status = Domain.Enum.Status.Active;
            newExpense.CreateDate = DateTime.Now;
            if (await _baseWriteRepository.Insert(newExpense))
                return IdentityResult.Success;
            else
                return IdentityResult.Failed();
        }

        public async Task<IdentityResult> UpdateExpense(ExpenseDTO model)
        {
            Expense expense = await _baseReadRepository.GetSingleDefault(x => x.Id == model.Id);
            if (expense == null)
                return IdentityResult.Failed();

            ExpenseDTO expenseDTO = _mapper.Map<ExpenseDTO>(expense);

            expenseDTO.Description = model.Description;
            expenseDTO.AmountOfExpense = model.AmountOfExpense;
            expenseDTO.Status = Domain.Enum.Status.Modified;
            expenseDTO.UpdateDate = DateTime.Now;

            if (await _baseWriteRepository.Update(_mapper.Map<Expense>(expenseDTO)))
                return IdentityResult.Success;
            else
                return IdentityResult.Failed();
        }
    }
}
