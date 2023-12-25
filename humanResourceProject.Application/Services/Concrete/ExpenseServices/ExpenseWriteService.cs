using AutoMapper;
using humanResourceProject.Application.Services.Abstract.IExpenseServices;
using humanResourceProject.Application.Services.Abstract.IImageServices;
using humanResourceProject.Application.Services.Concrete.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.IRepository.BaseRepos;
using humanResourceProject.Infrastructure.Repositories.BaseRepos;
using humanResourceProject.Models.DTOs;
using Microsoft.AspNetCore.Identity;

namespace humanResourceProject.Application.Services.Concrete.ExpenseServices
{
    public class ExpenseWriteService : BaseWriteService<Expense>, IExpenseWriteService
    {
        private readonly IBaseWriteRepository<Expense> _expenseWriteRepository;
        private readonly IBaseReadRepository<Expense> _expenseReadRepository;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;
        public ExpenseWriteService(IBaseWriteRepository<Expense> expenseWriteRepository, IBaseReadRepository<Expense> expenseReadRepository, IMapper mapper, IImageService imageService) : base(expenseWriteRepository, expenseReadRepository)
        {
            _expenseWriteRepository = expenseWriteRepository;
            _expenseReadRepository = expenseReadRepository;
            _mapper = mapper;
            _imageService = imageService;
        }

        public async Task<IdentityResult> DeleteExpense(Guid id)
        {
            Expense expense = await _expenseReadRepository.GetById(id);
            expense.Status = Domain.Enum.Status.Deleted;
            expense.DeleteDate = DateTime.Now;
            if (await _expenseWriteRepository.Delete(id))
                return IdentityResult.Success;
            else
                return IdentityResult.Failed();
        }

        public async Task<IdentityResult> InsertExpense(ExpenseDTO model)
        {
            if (model == null)
                return IdentityResult.Failed();

            model = await _imageService.UploadExpenseImageToAzure(model);

            Expense newExpense = _mapper.Map<Expense>(model);
            newExpense.Status = Domain.Enum.Status.Active;
            newExpense.CreateDate = DateTime.Now;
            if (await _expenseWriteRepository.Insert(newExpense))
                return IdentityResult.Success;
            else
                return IdentityResult.Failed();
        }

        public async Task<IdentityResult> UpdateExpense(UpdateExpenseDTO model)
        {
            Expense updateExpense = await _expenseReadRepository.GetSingleDefault(x => x.Id == model.Id);
            if (updateExpense == null)
                return IdentityResult.Failed();

            _expenseWriteRepository.DetachEntity(updateExpense);

            updateExpense = _mapper.Map<Expense>(model);

            updateExpense.Status = Domain.Enum.Status.Modified;
            updateExpense.UpdateDate = DateTime.Now;

            if (await _expenseWriteRepository.Update(updateExpense))
                return IdentityResult.Success;
            else
                return IdentityResult.Failed();
        }
    }
}
