using BudgetService.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BudgetService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Require JWT
    public class BudgetController(Services.BudgetService service) : ControllerBase
    {
        private int GetUserId()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return userIdClaim != null ? int.Parse(userIdClaim) : throw new UnauthorizedAccessException("User ID not found in token");
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool activeOnly = false)
        {
            try
            {
                var budgets = await service.GetBudgetsAsync(GetUserId(), activeOnly);
                return Ok(budgets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving budgets", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var budget = await service.GetBudgetAsync(id, GetUserId());
                if (budget == null) return NotFound(new { message = $"Budget with ID {id} not found" });

                var result = new
                {
                    budget,
                    progress = service.GetBudgetProgress(budget),
                    remaining = service.GetRemainingAmount(budget),
                    isExceeded = service.IsBudgetExceeded(budget)
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the budget", error = ex.Message });
            }
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetByCategory(int categoryId)
        {
            try
            {
                var budgets = await service.GetBudgetsByCategoryAsync(categoryId, GetUserId());
                return Ok(budgets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving budgets", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BudgetDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var budget = await service.CreateBudgetAsync(GetUserId(), dto);
                return CreatedAtAction(nameof(GetById), new { id = budget.Id }, budget);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the budget", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BudgetDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var budget = await service.UpdateBudgetAsync(id, GetUserId(), dto);
                return Ok(budget);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the budget", error = ex.Message });
            }
        }

        [HttpPut("{id}/spent")]
        public async Task<IActionResult> UpdateSpentAmount(int id, [FromBody] decimal spentAmount)
        {
            try
            {
                if (spentAmount < 0)
                {
                    return BadRequest(new { message = "Spent amount cannot be negative" });
                }

                await service.UpdateSpentAmountAsync(id, spentAmount, GetUserId());
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating spent amount", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await service.DeleteBudgetAsync(id, GetUserId());
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the budget", error = ex.Message });
            }
        }
    }
}