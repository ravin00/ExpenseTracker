using SavingsGoalService.Models;
using FluentAssertions;
using System.ComponentModel.DataAnnotations;

namespace SavingsGoalService.Tests.ModelTests
{
    public class SavingsGoalModelTests
    {
        [Fact]
        public void SavingsGoal_DefaultValues_ShouldBeCorrect()
        {
            // Arrange & Act
            var savingsGoal = new SavingsGoal
            {
                Name = "Test Goal",
                TargetAmount = 1000m,
                UserId = 1
            };

            // Assert
            savingsGoal.CurrentAmount.Should().Be(0);
            savingsGoal.StartDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
            savingsGoal.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
            savingsGoal.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
            savingsGoal.Status.Should().Be(SavingsGoalStatus.Active);
            savingsGoal.Priority.Should().Be(SavingsGoalPriority.Medium);
            savingsGoal.MonthlyContributionTarget.Should().Be(0);
            savingsGoal.IsAutoContribution.Should().BeFalse();
        }

        [Theory]
        [InlineData(0, 1000, 0)]
        [InlineData(250, 1000, 25)]
        [InlineData(500, 1000, 50)]
        [InlineData(1000, 1000, 100)]
        [InlineData(1500, 1000, 100)] // Should cap at 100%
        public void SavingsGoal_ProgressPercentage_ShouldCalculateCorrectly(decimal current, decimal target, decimal expected)
        {
            // Arrange
            var savingsGoal = new SavingsGoal
            {
                Name = "Test Goal",
                TargetAmount = target,
                CurrentAmount = current,
                UserId = 1
            };

            // Act & Assert
            savingsGoal.ProgressPercentage.Should().Be(expected);
        }

        [Theory]
        [InlineData(0, 1000, 1000)]
        [InlineData(250, 1000, 750)]
        [InlineData(1000, 1000, 0)]
        [InlineData(1500, 1000, 0)] // Should not go negative
        public void SavingsGoal_RemainingAmount_ShouldCalculateCorrectly(decimal current, decimal target, decimal expected)
        {
            // Arrange
            var savingsGoal = new SavingsGoal
            {
                Name = "Test Goal",
                TargetAmount = target,
                CurrentAmount = current,
                UserId = 1
            };

            // Act & Assert
            savingsGoal.RemainingAmount.Should().Be(expected);
        }

        [Theory]
        [InlineData(500, 1000, false)]
        [InlineData(1000, 1000, true)]
        [InlineData(1500, 1000, true)]
        public void SavingsGoal_IsCompleted_ShouldCalculateCorrectly(decimal current, decimal target, bool expected)
        {
            // Arrange
            var savingsGoal = new SavingsGoal
            {
                Name = "Test Goal",
                TargetAmount = target,
                CurrentAmount = current,
                UserId = 1
            };

            // Act & Assert
            savingsGoal.IsCompleted.Should().Be(expected);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void SavingsGoal_InvalidName_ShouldFailValidation(string name)
        {
            // Arrange
            var savingsGoal = new SavingsGoal
            {
                Name = name!,
                TargetAmount = 1000m,
                UserId = 1
            };

            // Act
            var validationResults = ValidateModel(savingsGoal);

            // Assert
            validationResults.Should().HaveCountGreaterThan(0);
            validationResults.Should().Contain(vr => vr.MemberNames.Contains("Name"));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-100)]
        public void SavingsGoal_InvalidTargetAmount_ShouldFailValidation(decimal targetAmount)
        {
            // Arrange
            var savingsGoal = new SavingsGoal
            {
                Name = "Test Goal",
                TargetAmount = targetAmount,
                UserId = 1
            };

            // Act
            var validationResults = ValidateModel(savingsGoal);

            // Assert
            validationResults.Should().HaveCountGreaterThan(0);
            validationResults.Should().Contain(vr => vr.MemberNames.Contains("TargetAmount"));
        }

        private static IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, validationContext, validationResults, true);
            return validationResults;
        }
    }
}