using humanResourceProject.Application.Services.Abstract.IAppUserServices;
using humanResourceProject.Application.Services.Abstract.IExpenseServices;
using humanResourceProject.Application.Services.Abstract.IMailServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;

namespace humanResourceProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseReadService _expenseReadService;
        private readonly IExpenseWriteService _expenseWriteService;
        private readonly IAppUserReadService _appUserReadService;
        private readonly IMailService _mailService;

        public ExpenseController(IExpenseReadService expenseReadService, IExpenseWriteService expenseWriteService, IAppUserReadService appUserReadService, IMailService mailService)
        {
            _expenseReadService = expenseReadService;
            _expenseWriteService = expenseWriteService;
            _appUserReadService = appUserReadService;
            _mailService = mailService;
        }

        [HttpGet]
        [Route("GetExpensesByEmployeeId/{id}")]
        public async Task<IActionResult> GetExpensesByEmployeeId(Guid id)
        {
            return Ok(await _expenseReadService.GetExpensesByEmployeeId(id));
        }

        [HttpGet]
        [Route("GetExpensesByDepartmentId/{id}")]
        public async Task<IActionResult> GetExpensesByDepartmentId(Guid id)
        {
            return Ok(await _expenseReadService.GetExpensesByDepartmentId(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateExpense([FromBody] ExpenseDTO model)
        {
            AppUser manager = await _appUserReadService.GetSingleDefault(x => x.Id == model.Employee.ManagerId);
            string recipientEmail = manager.Email;
            string mailToName = $"{manager.FirstName} {manager.LastName}";
            string action = "";
            string subject = "Harcama Talebi!";
            string body = $"Sayın {manager.FirstName} {manager.LastName}, {model.CreateDate.ToShortDateString()} tarihli {model.AmountOfExpense} {model.Currency.GetDisplayName()} harcama talebi yapılmıştır. Uygulamaya giriş yapıp onaylamanızı rica ederiz.";
            await _mailService.SendEmailAsync(manager, recipientEmail, mailToName, action, subject, body);
            return Ok(await _expenseWriteService.InsertExpense(model));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateExpense([FromBody] ExpenseDTO model)
        {
            return Ok(await _expenseWriteService.UpdateExpense(model));
        }

        [HttpDelete]
        [Route("DeleteExpense/{id}")]
        public async Task<IActionResult> DeleteExpense(Guid id)
        {
            return Ok(await _expenseWriteService.DeleteExpense(id));
        }

        //[HttpGet]
        //[Route("GetUpdateAdvanceDTO/{id}")]
        //public async Task<IActionResult> GetUpdateExpenseDTO(Guid id)
        //{
        //    return Ok(await _expenseReadService.GetUpdateExpenseDTO(id));
        //}
    }
}
