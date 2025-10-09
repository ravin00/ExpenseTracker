using AnalyticsService.Dtos;
using AnalyticsService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AnalyticsService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Require JWT
    public class AnalyticsController : ControllerBase
    {
        private readonly Services.IAnalyticsService _service;
        private readonly ILogger<AnalyticsController> _logger;

        public AnalyticsController(Services.IAnalyticsService service, ILogger<AnalyticsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return userIdClaim != null ? int.Parse(userIdClaim) : throw new UnauthorizedAccessException("User ID not found in token");
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] AnalyticsPeriod? period = null)
        {
            try
            {
                var analytics = await _service.GetAnalyticsAsync(GetUserId(), period);
                return Ok(analytics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving analytics for user {UserId}", GetUserId());
                return StatusCode(500, new { message = "An error occurred while retrieving analytics", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var analytics = await _service.GetAnalyticsAsync(id, GetUserId());
                if (analytics == null) 
                    return NotFound(new { message = $"Analytics with ID {id} not found" });
                
                return Ok(analytics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving analytics {AnalyticsId} for user {UserId}", id, GetUserId());
                return StatusCode(500, new { message = "An error occurred while retrieving the analytics", error = ex.Message });
            }
        }

        [HttpGet("range")]
        public async Task<IActionResult> GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                if (startDate > endDate)
                    return BadRequest(new { message = "Start date cannot be after end date" });

                var analytics = await _service.GetAnalyticsByDateRangeAsync(GetUserId(), startDate, endDate);
                return Ok(analytics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving analytics for date range for user {UserId}", GetUserId());
                return StatusCode(500, new { message = "An error occurred while retrieving analytics", error = ex.Message });
            }
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            try
            {
                var dashboard = await _service.GetDashboardDataAsync(GetUserId());
                return Ok(dashboard);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving dashboard data for user {UserId}", GetUserId());
                return StatusCode(500, new { message = "An error occurred while retrieving dashboard data", error = ex.Message });
            }
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetFinancialSummary([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                if (startDate > endDate)
                    return BadRequest(new { message = "Start date cannot be after end date" });

                var summary = await _service.GetFinancialSummaryAsync(GetUserId(), startDate, endDate);
                return Ok(summary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving financial summary for user {UserId}", GetUserId());
                return StatusCode(500, new { message = "An error occurred while retrieving financial summary", error = ex.Message });
            }
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetExpensesByCategory([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                if (startDate > endDate)
                    return BadRequest(new { message = "Start date cannot be after end date" });

                var categories = await _service.GetExpensesByCategoryAsync(GetUserId(), startDate, endDate);
                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving expenses by category for user {UserId}", GetUserId());
                return StatusCode(500, new { message = "An error occurred while retrieving category expenses", error = ex.Message });
            }
        }

        [HttpGet("trends")]
        public async Task<IActionResult> GetSpendingTrends([FromQuery] AnalyticsPeriod period = AnalyticsPeriod.Monthly, [FromQuery] int months = 12)
        {
            try
            {
                if (months <= 0 || months > 60)
                    return BadRequest(new { message = "Months must be between 1 and 60" });

                var trends = await _service.GetSpendingTrendsAsync(GetUserId(), period, months);
                return Ok(trends);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving spending trends for user {UserId}", GetUserId());
                return StatusCode(500, new { message = "An error occurred while retrieving spending trends", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AnalyticsDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var analytics = await _service.CreateAnalyticsAsync(GetUserId(), dto);
                return CreatedAtAction(nameof(GetById), new { id = analytics.Id }, analytics);
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
                _logger.LogError(ex, "Error creating analytics for user {UserId}", GetUserId());
                return StatusCode(500, new { message = "An error occurred while creating analytics", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] AnalyticsDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var analytics = await _service.UpdateAnalyticsAsync(id, GetUserId(), dto);
                return Ok(analytics);
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
                _logger.LogError(ex, "Error updating analytics {AnalyticsId} for user {UserId}", id, GetUserId());
                return StatusCode(500, new { message = "An error occurred while updating analytics", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAnalyticsAsync(id, GetUserId());
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting analytics {AnalyticsId} for user {UserId}", id, GetUserId());
                return StatusCode(500, new { message = "An error occurred while deleting analytics", error = ex.Message });
            }
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateAnalytics([FromBody] AnalyticsRequestDto request)
        {
            try
            {
                var date = request.StartDate ?? DateTime.UtcNow.Date;
                var period = request.Period;

                if (await _service.AnalyticsExistsAsync(GetUserId(), date, period))
                    return Conflict(new { message = $"Analytics already exists for {date:yyyy-MM-dd} with period {period}" });

                var analytics = await _service.GenerateAnalyticsAsync(GetUserId(), date, period);
                return CreatedAtAction(nameof(GetById), new { id = analytics.Id }, analytics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating analytics for user {UserId}", GetUserId());
                return StatusCode(500, new { message = "An error occurred while generating analytics", error = ex.Message });
            }
        }

        [HttpGet("health-status")]
        public async Task<IActionResult> GetHealthStatus()
        {
            try
            {
                var currentMonth = DateTime.UtcNow.Date;
                var startOfMonth = new DateTime(currentMonth.Year, currentMonth.Month, 1);
                var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

                var summary = await _service.GetFinancialSummaryAsync(GetUserId(), startOfMonth, endOfMonth);
                
                var healthStatus = new
                {
                    OverallHealth = summary.FinancialHealth,
                    SavingsRate = summary.SavingsRate,
                    NetWorth = summary.NetWorth,
                    IsOverspending = summary.TotalExpenses > summary.TotalIncome,
                    Recommendation = _service.GenerateRecommendation(summary)
                };

                return Ok(healthStatus);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving health status for user {UserId}", GetUserId());
                return StatusCode(500, new { message = "An error occurred while retrieving health status", error = ex.Message });
            }
        }
    }
}