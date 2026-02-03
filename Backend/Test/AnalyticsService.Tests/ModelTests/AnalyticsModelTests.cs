using AnalyticsService.Models;
using FluentAssertions;

namespace AnalyticsService.Tests.ModelTests
{
    public class AnalyticsModelTests
    {
        [Fact]
        public void Analytics_ShouldCalculateNetIncomeCorrectly()
        {
            // Arrange
            var analytics = new Analytics
            {
                TotalIncome = 2000,
                TotalExpenses = 1500
            };

            // Act
            var netIncome = analytics.NetIncome;

            // Assert
            netIncome.Should().Be(500);
        }

        [Fact]
        public void Analytics_ShouldCalculateSavingsRateCorrectly()
        {
            // Arrange
            var analytics = new Analytics
            {
                TotalIncome = 2000,
                TotalSavings = 400
            };

            // Act
            var savingsRate = analytics.SavingsRate;

            // Assert
            savingsRate.Should().Be(20);
        }

        [Fact]
        public void Analytics_ShouldReturnZeroSavingsRate_WhenIncomeIsZero()
        {
            // Arrange
            var analytics = new Analytics
            {
                TotalIncome = 0,
                TotalSavings = 100
            };

            // Act
            var savingsRate = analytics.SavingsRate;

            // Assert
            savingsRate.Should().Be(0);
        }

        [Fact]
        public void Analytics_ShouldIdentifyHighSaver_WhenSavingsRateIsAbove20Percent()
        {
            // Arrange
            var analytics = new Analytics
            {
                TotalIncome = 2000,
                TotalSavings = 500 // 25% savings rate
            };

            // Act
            var isHighSaver = analytics.IsHighSaver;

            // Assert
            isHighSaver.Should().BeTrue();
        }

        [Fact]
        public void Analytics_ShouldIdentifyOverspending_WhenExpensesExceedIncome()
        {
            // Arrange
            var analytics = new Analytics
            {
                TotalIncome = 1000,
                TotalExpenses = 1200
            };

            // Act
            var isOverspending = analytics.IsOverspending;

            // Assert
            isOverspending.Should().BeTrue();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void IsValidUserId_ShouldReturnFalse_ForInvalidUserIds(int userId)
        {
            // Arrange
            var analytics = new Analytics { UserId = userId };

            // Act
            var isValid = analytics.IsValidUserId();

            // Assert
            isValid.Should().BeFalse();
        }

        [Fact]
        public void IsValidUserId_ShouldReturnTrue_ForValidUserId()
        {
            // Arrange
            var analytics = new Analytics { UserId = 1 };

            // Act
            var isValid = analytics.IsValidUserId();

            // Assert
            isValid.Should().BeTrue();
        }

        [Fact]
        public void IsValidAmounts_ShouldReturnTrue_ForNonNegativeAmounts()
        {
            // Arrange
            var analytics = new Analytics
            {
                TotalExpenses = 100,
                TotalIncome = 200,
                TotalSavings = 50
            };

            // Act
            var isValid = analytics.IsValidAmounts();

            // Assert
            isValid.Should().BeTrue();
        }
    }
}