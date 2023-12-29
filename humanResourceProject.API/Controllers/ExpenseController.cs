using humanResourceProject.Application.Services.Abstract.IAppUserServices;
using humanResourceProject.Application.Services.Abstract.IExpenseServices;
using humanResourceProject.Application.Services.Abstract.IMailServices;
using humanResourceProject.Application.Services.Concrete.ExpenseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Models.DTOs;
using humanResourceProject.Models.VMs;
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

        [HttpGet]
        [Route("GetExpenseDTO/{employeeId}")]
        public async Task<IActionResult> GetExpenseDTO(Guid employeeId)
        {
            return Ok(await _expenseReadService.GetExpenseDTO(employeeId));
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
                string body = $"<p>Sayın {user.FirstName} {user.LastName},</p><p>{expense.CreateDate.ToShortDateString()} tarihli {expense.AmountOfExpense} {expense.Currency.GetDisplayName()} harcama talebiniz onaylannıştır.</p><p>Güzel günlerde kullanmanız dileğiyle.</p><br><hr><br><h3>Team Monitorease</h3>";
                await _mailService.SendEmailAsync(user, recipientEmail, mailToName, subject, body);
                //_mailService.SendApproveMail(user, action, $"Sayın {user.FirstName} {user.LastName} Avansın onaylandı. Güzel günlerde kullan");
            }
            else if (model.ExpenseStatus == Domain.Enum.RequestStatus.Rejected)
            {
                model.ExpenseStatus = Domain.Enum.RequestStatus.Rejected;
                string subject = "Masraf Reddi!";
                string body = $"<p>Sayın {user.FirstName} {user.LastName},</p><p>{expense.CreateDate.ToShortDateString()} tarihli {expense.AmountOfExpense} {expense.Currency.GetDisplayName()} harcama talebiniz Reddedilmiştir.</p><br><hr><br><h3>Team Monitorease</h3>";
                await _mailService.SendEmailAsync(user, recipientEmail, mailToName, subject, body);
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
        [Route("GetExpensesByManagerId/{id}")]
        public async Task<IActionResult> GetExpensesByManagerId(Guid id)
        {
            return Ok(await _expenseReadService.GetExpensesByManagerId(id));
        }

        [HttpGet]
        [Route("GetExpensesByCompanyId/{id}")]
        public async Task<IActionResult> GetExpensesByCompanyId(Guid id)
        {
            return Ok(await _expenseReadService.GetExpensesByCompanyId(id));
        }

        [HttpPost]
        [Route("CreateExpense")]
        public async Task<IActionResult> CreateExpense([FromForm] ExpenseDTO model)
        {
            var result = await _expenseWriteService.InsertExpense(model);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            AppUser employee = await _appUserReadService.GetSingleDefault(x => x.Id == model.EmployeeId);
            AppUser manager = await _appUserReadService.GetSingleDefault(x => x.Id == employee.ManagerId);
            string recipientEmail = manager.Email;
            string mailToName = $"{manager.FirstName} {manager.LastName}";
            string action = "";
            string subject = "Masraf Talebi!";
            string body = $"<p>Sayın {manager.FirstName} {manager.LastName},</p><p>{employee.FirstName} {employee.LastName} tarafından {DateTime.Now.ToShortDateString()} tarihli {model.AmountOfExpense} {model.Currency.GetDisplayName()} masraf talebi yapılmıştır.</p><p>Uygulama üzerinden onaylama ya da reddetme işlemini yapabilirsiniz.</p><p>İyi çalışmalar dileriz.</p><br><hr><br><h3>Team Monitorease</h3>";
            await _mailService.SendEmailAsync(manager, recipientEmail, mailToName, subject, body);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateExpense([FromBody] UpdateExpenseDTO model)
        {
            return Ok(await _expenseWriteService.UpdateExpense(model));
        }

        [HttpGet]
        [Route("GetUpdateExpenseDTO/{id}")]
        public async Task<IActionResult> GetUpdateExpenseDTO(Guid id)
        {
            return Ok(await _expenseReadService.GetUpdateExpenseDTO(id));
        }

        [HttpDelete]
        [Route("DeleteExpense/{id}")]
        public async Task<IActionResult> DeleteExpense(Guid id)
        {
            return Ok(await _expenseWriteService.DeleteExpense(id));
        }

        [HttpGet]
        [Route("FillDashboardExpenseVM/{id}")]
        public async Task<IActionResult> FillDashboardExpenseVM(Guid id)
        {
            return Ok(await _expenseReadService.FillDashboardExpenseVM(id));
        }
    }
}
