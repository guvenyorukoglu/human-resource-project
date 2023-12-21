using humanResourceProject.Application.Services.Abstract.IExpenseServices;
using humanResourceProject.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace humanResourceProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseReadService _expenseReadService;
        private readonly IExpenseWriteService _expenseWriteService;

        public ExpenseController(IExpenseReadService expenseReadService, IExpenseWriteService expenseWriteService)
        {
            _expenseReadService = expenseReadService;
            _expenseWriteService = expenseWriteService;
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

        [HttpGet]
        [Route("GetUpdateAdvanceDTO/{id}")]
        public async Task<IActionResult> GetUpdateExpenseDTO(Guid id)
        {
            return Ok(await _expenseReadService.GetUpdateExpenseDTO(id));
        }


    }
}
