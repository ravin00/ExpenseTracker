using SavingsGoalService.Dtos;
using SavingsGoalService.Models;
using FluentAssertions;
using System.ComponentModel.DataAnnotations;

namespace SavingsGoalService.Tests.DtoTests
{
    public class SavingsGoalDtoTests
    {
        [Fact]
        public void SavingsGoalDto_ValidData_ShouldPassValidation()
        {
            // Arrange
            var dto = new SavingsGoalDto
            {
                Name = "Emergency Fund",
                Description = "Emergency savings for unexpected expenses",
                TargetAmount = 5000m,
                CurrentAmount = 1500m,
                StartDate = DateTime.UtcNow,
                TargetDate = DateTime.UtcNow.AddMonths(12),
                Priority = SavingsGoalPriority.High,
                Color = "#FF5733",
                MonthlyContributionTarget = 400m
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            validationResults.Should().BeEmpty();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void SavingsGoalDto_InvalidName_ShouldFailValidation(string name)
        {
            // Arrange
            var dto = new SavingsGoalDto
            {
                Name = name!,
                TargetAmount = 1000m
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            validationResults.Should().HaveCountGreaterThan(0);
            validationResults.Should().Contain(vr => vr.MemberNames.Contains("Name"));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-100)]
        public void SavingsGoalDto_InvalidTargetAmount_ShouldFailValidation(decimal targetAmount)
        {
            // Arrange
            var dto = new SavingsGoalDto
            {
                Name = "Test Goal",
                TargetAmount = targetAmount
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            validationResults.Should().HaveCountGreaterThan(0);
            validationResults.Should().Contain(vr => vr.MemberNames.Contains("TargetAmount"));
        }

        [Theory]
        [InlineData("#invalid")]
        [InlineData("FF5733")]
        [InlineData("#GG5733")]
        public void SavingsGoalDto_InvalidColor_ShouldFailValidation(string color)
        {
            // Arrange
            var dto = new SavingsGoalDto
            {
                Name = "Test Goal",
                TargetAmount = 1000m,
                Color = color
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            validationResults.Should().HaveCountGreaterThan(0);
            validationResults.Should().Contain(vr => vr.MemberNames.Contains("Color"));
        }

        [Fact]
        public void SavingsContributionDto_ValidData_ShouldPassValidation()
        {
            // Arrange
            var dto = new SavingsContributionDto
            {
                Amount = 100m,
                Notes = "Monthly contribution",
                Date = DateTime.UtcNow
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            validationResults.Should().BeEmpty();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-50)]
        public void SavingsContributionDto_InvalidAmount_ShouldFailValidation(decimal amount)
        {
            // Arrange
            var dto = new SavingsContributionDto
            {
                Amount = amount
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            validationResults.Should().HaveCountGreaterThan(0);
            validationResults.Should().Contain(vr => vr.MemberNames.Contains("Amount"));
        }

        [Fact]
        public void SavingsWithdrawalDto_ValidData_ShouldPassValidation()
        {
            // Arrange
            var dto = new SavingsWithdrawalDto
            {
                Amount = 50m,
                Reason = "Emergency expense",
                Date = DateTime.UtcNow
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            validationResults.Should().BeEmpty();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void SavingsWithdrawalDto_InvalidReason_ShouldFailValidation(string reason)
        {
            // Arrange
            var dto = new SavingsWithdrawalDto
            {
                Amount = 50m,
                Reason = reason!
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            validationResults.Should().HaveCountGreaterThan(0);
            validationResults.Should().Contain(vr => vr.MemberNames.Contains("Reason"));
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