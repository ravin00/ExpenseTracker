using ExpenseService.Dtos;
// using ExpenseService.Models;
// using ExpenseService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExpenseService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Require JWT
    public class ExpenseController : ControllerBase
    {
        private readonly Services.ExpenseService _service;

        public ExpenseController(Services.ExpenseService service)
        {
            _service = service;
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return userIdClaim != null ? int.Parse(userIdClaim) : throw new UnauthorizedAccessException("User ID not found in token");
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var expenses = await _service.GetExpensesAsync(GetUserId());
            return Ok(expenses);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var expense = await _service.GetExpenseAsync(id, GetUserId());
            if (expense == null) return NotFound();
            return Ok(expense);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ExpenseDto dto)
        {
            var evt = await _service.AddExpenseAsync(GetUserId(), dto);
            // TODO: publish evt to RabbitMQ/Kafka
            return Ok(evt);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ExpenseDto dto)
        {
            var expense = await _service.GetExpenseAsync(id, GetUserId());
            if (expense == null) return NotFound();

            expense.Description = dto.Description;
            expense.Amount = dto.Amount;
            expense.Date = dto.Date;

            await _service.UpdateExpenseAsync(expense);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var expense = await _service.GetExpenseAsync(id, GetUserId());
            if (expense == null) return NotFound();

            await _service.DeleteExpenseAsync(expense);
            return NoContent();
        }
    }
}
