using BudgetService.Models;
using FluentAssertions;
using Xunit;

namespace BudgetService.Tests.ModelTests
{
    public class BudgetTests
    {
        [Fact]
        public void Budget_DefaultValues_ShouldBeSetCorrectly()
        {
            // Arrange & Act
            var budget = new Budget { Name = "Test Budget", Amount = 1000m };

            // Assert
            budget.Period.Should().Be(BudgetPeriod.Monthly);
            budget.SpentAmount.Should().Be(0m);
            budget.IsActive.Should().BeTrue();
            budget.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
            budget.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
            budget.StartDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [Theory]
        [InlineData("Valid Budget Name", true)]
        [InlineData("A", true)]
        [InlineData("", false)]
        [InlineData("   ", false)]
        [InlineData(null, false)]
        public void IsValidName_ShouldReturnCorrectResult(string? name, bool expected)
        {
            // Arrange
            var budget = new Budget { Name = name!, Amount = 1000m };

            // Act
            var result = budget.IsValidName();

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(null, true)]
        [InlineData("", true)]
        [InlineData("Valid description", true)]
        [InlineData("A", true)]
        public void IsValidDescription_ShouldReturnTrueForValidDescriptions(string? description, bool expected)
        {
            // Arrange
            var budget = new Budget { Name = "Test", Amount = 1000m, Description = description };

            // Act
            var result = budget.IsValidDescription();

            // Assert
            result.Should().Be(expected);
        }

        [Fact]
        public void IsValidDescription_ShouldReturnFalseForTooLongDescription()
        {
            // Arrange
            var longDescription = new string('A', 501);
            var budget = new Budget { Name = "Test", Amount = 1000m, Description = longDescription };

            // Act
            var result = budget.IsValidDescription();

            // Assert
            result.Should().BeFalse();
        }

        [Theory]
        [InlineData(0.01, true)]
        [InlineData(1000, true)]
        [InlineData(0, false)]
        [InlineData(-1, false)]
        public void IsValidAmount_ShouldReturnCorrectResult(decimal amount, bool expected)
        {
            // Arrange
            var budget = new Budget { Name = "Test", Amount = amount };

            // Act
            var result = budget.IsValidAmount();

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(0, true)]
        [InlineData(500, true)]
        [InlineData(1000, true)]
        [InlineData(-1, false)]
        public void IsValidSpentAmount_ShouldReturnCorrectResult(decimal spentAmount, bool expected)
        {
            // Arrange
            var budget = new Budget { Name = "Test", Amount = 1000m, SpentAmount = spentAmount };

            // Act
            var result = budget.IsValidSpentAmount();

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(100, true)]
        [InlineData(0, false)]
        [InlineData(-1, false)]
        public void IsValidUserId_ShouldReturnCorrectResult(int userId, bool expected)
        {
            // Arrange
            var budget = new Budget { Name = "Test", Amount = 1000m, UserId = userId };

            // Act
            var result = budget.IsValidUserId();

            // Assert
            result.Should().Be(expected);
        }

        [Fact]
        public void IsValidDateRange_ShouldReturnTrueWhenEndDateIsNull()
        {
            // Arrange
            var budget = new Budget
            {
                Name = "Test",
                Amount = 1000m,
                StartDate = DateTime.UtcNow,
                EndDate = null
            };

            // Act
            var result = budget.IsValidDateRange();

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsValidDateRange_ShouldReturnTrueWhenEndDateAfterStartDate()
        {
            // Arrange
            var startDate = DateTime.UtcNow;
            var budget = new Budget
            {
                Name = "Test",
                Amount = 1000m,
                StartDate = startDate,
                EndDate = startDate.AddDays(30)
            };

            // Act
            var result = budget.IsValidDateRange();

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsValidDateRange_ShouldReturnFalseWhenEndDateBeforeStartDate()
        {
            // Arrange
            var startDate = DateTime.UtcNow;
            var budget = new Budget
            {
                Name = "Test",
                Amount = 1000m,
                StartDate = startDate,
                EndDate = startDate.AddDays(-1)
            };

            // Act
            var result = budget.IsValidDateRange();

            // Assert
            result.Should().BeFalse();
        }

        [Theory]
        [InlineData("  Test Budget  ", "Test Budget")]
        [InlineData("Normal Name", "Normal Name")]
        [InlineData("", "")]
        public void DisplayName_ShouldReturnTrimmedName(string name, string expected)
        {
            // Arrange
            var budget = new Budget { Name = name, Amount = 1000m };

            // Act
            var result = budget.DisplayName;

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(1000, 500, 50)]
        [InlineData(1000, 1000, 100)]
        [InlineData(1000, 1200, 100)] // Capped at 100%
        [InlineData(1000, 0, 0)]
        [InlineData(0, 500, 0)] // Edge case: zero amount
        public void ProgressPercentage_ShouldCalculateCorrectly(decimal amount, decimal spent, decimal expected)
        {
            // Arrange
            var budget = new Budget { Name = "Test", Amount = amount, SpentAmount = spent };

            // Act
            var result = budget.ProgressPercentage;

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(1000, 500, 500)]
        [InlineData(1000, 1000, 0)]
        [InlineData(1000, 1200, 0)] // Can't be negative
        [InlineData(1000, 0, 1000)]
        public void RemainingAmount_ShouldCalculateCorrectly(decimal amount, decimal spent, decimal expected)
        {
            // Arrange
            var budget = new Budget { Name = "Test", Amount = amount, SpentAmount = spent };

            // Act
            var result = budget.RemainingAmount;

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(1000, 500, false)]
        [InlineData(1000, 1000, false)]
        [InlineData(1000, 1200, true)]
        [InlineData(1000, 0, false)]
        public void IsExceeded_ShouldReturnCorrectResult(decimal amount, decimal spent, bool expected)
        {
            // Arrange
            var budget = new Budget { Name = "Test", Amount = amount, SpentAmount = spent };

            // Act
            var result = budget.IsExceeded;

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(1000, 1200, 200)]
        [InlineData(1000, 500, 0)]
        [InlineData(1000, 1000, 0)]
        public void ExceededAmount_ShouldCalculateCorrectly(decimal amount, decimal spent, decimal expected)
        {
            // Arrange
            var budget = new Budget { Name = "Test", Amount = amount, SpentAmount = spent };

            // Act
            var result = budget.ExceededAmount;

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(30, 30)]
        [InlineData(0, 0)]
        [InlineData(-5, 0)] // Past end date
        public void DaysRemaining_ShouldCalculateCorrectly(int daysFromNow, int expected)
        {
            // Arrange
            var budget = new Budget
            {
                Name = "Test",
                Amount = 1000m,
                EndDate = DateTime.UtcNow.AddDays(daysFromNow)
            };

            // Act
            var result = budget.DaysRemaining;

            // Assert
            result.Should().BeCloseTo(expected, 1); // Allow 1 day tolerance for timing
        }

        [Fact]
        public void DaysRemaining_ShouldReturnZeroWhenNoEndDate()
        {
            // Arrange
            var budget = new Budget { Name = "Test", Amount = 1000m, EndDate = null };

            // Act
            var result = budget.DaysRemaining;

            // Assert
            result.Should().Be(0);
        }

        [Theory]
        [InlineData(BudgetHealthStatus.Healthy, 50)]
        [InlineData(BudgetHealthStatus.Caution, 75)]
        [InlineData(BudgetHealthStatus.Warning, 95)]
        [InlineData(BudgetHealthStatus.OverBudget, 120)]
        public void HealthStatus_ShouldReturnCorrectStatus(BudgetHealthStatus expected, decimal spentPercentage)
        {
            // Arrange
            var amount = 1000m;
            var spent = amount * (spentPercentage / 100);
            var budget = new Budget { Name = "Test", Amount = amount, SpentAmount = spent };

            // Act
            var result = budget.HealthStatus;

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(-1, true)]
        [InlineData(-7, true)]
        [InlineData(-8, false)]
        public void IsRecentlyCreated_ShouldReturnCorrectResult(int daysOffset, bool expected)
        {
            // Arrange
            var budget = new Budget
            {
                Name = "Test",
                Amount = 1000m,
                CreatedAt = DateTime.UtcNow.AddDays(daysOffset)
            };

            // Act
            var result = budget.IsRecentlyCreated;

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(5, true)]
        [InlineData(7, true)]
        [InlineData(8, false)]
        [InlineData(0, false)] // No end date
        public void IsNearDeadline_ShouldReturnCorrectResult(int daysFromNow, bool expected)
        {
            // Arrange
            var budget = new Budget
            {
                Name = "Test",
                Amount = 1000m,
                EndDate = daysFromNow > 0 ? DateTime.UtcNow.AddDays(daysFromNow) : null
            };

            // Act
            var result = budget.IsNearDeadline;

            // Assert
            result.Should().Be(expected);
        }
    }
}