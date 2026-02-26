using SavingsGoalService.Dtos;
using SavingsGoalService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using SavingsGoalService.Services;

namespace SavingsGoalService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Require JWT
    public class SavingsGoalController (ISavingsGoalService service): ControllerBase
    {
        private readonly ISavingsGoalService _service = service;

        private int GetUserId()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return userIdClaim != null ? int.Parse(userIdClaim) : throw new UnauthorizedAccessException("User ID not found in token");
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] SavingsGoalStatus? status = null)
        {
            try
            {
                var goals = await _service.GetSavingsGoalsAsync(GetUserId(), status);
                return Ok(goals);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving savings goals" });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var goal = await _service.GetSavingsGoalAsync(id, GetUserId());
                if (goal == null) return NotFound(new { message = $"Savings goal with ID {id} not found" });
                return Ok(goal);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the savings goal", error = ex.Message });
            }
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetByCategory(int categoryId)
        {
            try
            {
                var goals = await _service.GetSavingsGoalsByCategoryAsync(categoryId, GetUserId());
                return Ok(goals);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving savings goals", error = ex.Message });
            }
        }

        [HttpGet("priority/{priority}")]
        public async Task<IActionResult> GetByPriority(SavingsGoalPriority priority)
        {
            try
            {
                var goals = await _service.GetSavingsGoalsByPriorityAsync(GetUserId(), priority);
                return Ok(goals);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving savings goals", error = ex.Message });
            }
        }

        [HttpGet("due-soon")]
        public async Task<IActionResult> GetDueSoon([FromQuery] int daysFromNow = 30)
        {
            try
            {
                var goals = await _service.GetDueSoonAsync(GetUserId(), daysFromNow);
                return Ok(goals);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving due savings goals", error = ex.Message });
            }
        }

        [HttpGet("statistics")]
        public async Task<IActionResult> GetStatistics()
        {
            try
            {
                var stats = await _service.GetSavingsStatisticsAsync(GetUserId());
                return Ok(stats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving statistics", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SavingsGoalDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var goal = await _service.CreateSavingsGoalAsync(GetUserId(), dto);
                return CreatedAtAction(nameof(GetById), new { id = goal.Id }, goal);
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
                return StatusCode(500, new { message = "An error occurred while creating the savings goal", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] SavingsGoalDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var goal = await _service.UpdateSavingsGoalAsync(id, GetUserId(), dto);
                return Ok(goal);
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
                return StatusCode(500, new { message = "An error occurred while updating the savings goal", error = ex.Message });
            }
        }

        [HttpPost("{id}/contribute")]
        public async Task<IActionResult> AddContribution(int id, [FromBody] SavingsContributionDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var goal = await _service.AddContributionAsync(id, GetUserId(), dto);
                return Ok(new { message = "Contribution added successfully", goal });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while adding contribution", error = ex.Message });
            }
        }

        [HttpPost("{id}/withdraw")]
        public async Task<IActionResult> Withdraw(int id, [FromBody] SavingsWithdrawalDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var goal = await _service.WithdrawAsync(id, GetUserId(), dto);
                return Ok(new { message = "Withdrawal completed successfully", goal });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing withdrawal", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteSavingsGoalAsync(id, GetUserId());
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the savings goal", error = ex.Message });
            }
        }
    }
}