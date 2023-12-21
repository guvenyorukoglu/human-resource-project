using humanResourceProject.Application.Services.Abstract.IAppUserServices;
using humanResourceProject.Application.Services.Abstract.IExpenseServices;
using humanResourceProject.Application.Services.Abstract.IMailServices;
using humanResourceProject.Application.Services.Concrete.ExpenseServices;
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
        private readonly IMailService _mailService;
        private readonly IAppUserReadService _appUserReadService;

        public ExpenseController(IExpenseReadService expenseReadService, IExpenseWriteService expenseWriteService, IMailService mailService, IAppUserReadService appUserReadService)
        {
            _expenseReadService = expenseReadService;
            _expenseWriteService = expenseWriteService;
            _mailService = mailService;
            _appUserReadService = appUserReadService;
        }

        [HttpGet]
        [Route("GetAllExpenses")]
        public async Task<IActionResult> GetAllExpenses()
        {
            return Ok(await _expenseReadService.GetAllExpenses());
        }


        [HttpPut]
        [Route("UpdateStatus")]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateExpenseDTO model)
        {
            var result = await _expenseWriteService.UpdateExpense(model);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            Expense expense = await _expenseReadService.GetSingleDefault(x => x.Id == model.Id);
            AppUser user = await _appUserReadService.GetSingleDefault(x => x.Id == expense.EmployeeId);
            string action = "";
            string recipientEmail = user.Email;
            string mailToName = $"{user.FirstName} {user.LastName}";
            if (expense.ExpenseStatus == Domain.Enum.RequestStatus.Approved)
            {
                string subject = "Masraf Onayı!";
                string body = $"Sayın {user.FirstName} {user.LastName}, {expense.CreateDate.ToShortDateString()} tarihli  masraf talebiniz onaylannıştır. Güzel günlerde kullanınız.";
                await _mailService.SendEmailAsync(user, recipientEmail, mailToName, action, subject, body);
                //_mailService.SendApproveMail(user, action, $"Sayın {user.FirstName} {user.LastName} Avansın onaylandı. Güzel günlerde kullan");
            }
            else if (model.ExpenseStatus == Domain.Enum.RequestStatus.Rejected)
            {
                model.ExpenseStatus = Domain.Enum.RequestStatus.Rejected;
                string subject = "Masraf Reddi!";
                string body = $"Sayın {user.FirstName} {user.LastName} Masraf talebiniz reddedildi.";
                await _mailService.SendEmailAsync(user, recipientEmail, mailToName, action, subject, body);
                //_mailService.SendApproveMail(user, action, $"Sayın {user.FirstName} {user.LastName} Avansın reddedildi");
            }

            return Ok();
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
        [Route("CreateExpense")]
        public async Task<IActionResult> CreateExpense([FromBody] ExpenseDTO model)
        {
            AppUser manager = await _appUserReadService.GetSingleDefault(x => x.Id == model.Employee.ManagerId);
            string recipientEmail = manager.Email;
            string mailToName = $"{manager.FirstName} {manager.LastName}";
            string action = "";
            string subject = "Masraf Talebi!";
            string body = $"Sayın {manager.FirstName} {manager.LastName}, {model.CreateDate.ToShortDateString()} tarihli {model.AmountOfExpense} {model.Currency.GetDisplayName()} masraf talebi yapılmıştır. Uygulamaya giriş yapıp onaylamanızı rica ederiz.";
            await _mailService.SendEmailAsync(manager, recipientEmail, mailToName, action, subject, body);
            return Ok(await _expenseWriteService.InsertExpense(model));
        }

        [HttpPut]
        [Route("UpdateExpense")]
        public async Task<IActionResult> UpdateExpense([FromBody] UpdateExpenseDTO model)
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
