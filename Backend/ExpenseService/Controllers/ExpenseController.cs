using ExpenseService.Dtos;
using ExpenseService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExpenseService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Require JWT
    public class ExpenseController (IExpenseService service): ControllerBase
    {
        private int GetUserId()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return userIdClaim != null ? int.Parse(userIdClaim) : throw new UnauthorizedAccessException("User ID not found in token");
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var expenses = await service.GetExpensesAsync(GetUserId());
            return Ok(expenses);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var expense = await service.GetExpenseAsync(id, GetUserId());
            if (expense == null) return NotFound();
            return Ok(expense);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ExpenseCreateDto dto)
        {
            var evt = await service.AddExpenseAsync(GetUserId(), dto);
            // TODO: publish evt to RabbitMQ/Kafka
            
            // Fetch the full expense obejct to return the complete data
            var expense = await service.GetExpenseAsync(evt.ExpenseId, GetUserId());
            return CreatedAtAction(nameof(GetById), new { id = evt.ExpenseId }, expense);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ExpenseUpdateDto dto)
        {
            var expense = await service.GetExpenseAsync(id, GetUserId());
            if (expense == null) return NotFound();

            expense.Description = dto.Description;
            expense.Amount = dto.Amount;
            expense.Date = dto.Date;
            expense.Category = dto.Category;
            expense.Notes = dto.Notes;

            await service.UpdateExpenseAsync(expense);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var expense = await service.GetExpenseAsync(id, GetUserId());
            if (expense == null) return NotFound();

            await service.DeleteExpenseAsync(expense);
            return NoContent();
        }
    }
}
