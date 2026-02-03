using ExpenseService.Dtos;
using FluentAssertions;
using System.ComponentModel.DataAnnotations;

namespace ExpenseService.Tests.DtoTests
{
    public class ExpenseDtoTests
    {
        [Fact]
        public void ExpenseCreateDto_ValidData_ShouldPassValidation()
        {
            // Arrange
            var dto = new ExpenseCreateDto
            {
                Description = "Grocery Shopping",
                Amount = 150.75m,
                Date = DateTime.UtcNow,
                Category = "Food",
                Notes = "Weekly grocery shopping"
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            validationResults.Should().BeEmpty();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void ExpenseCreateDto_InvalidDescription_ShouldFailValidation(string description)
        {
            // Arrange
            var dto = new ExpenseCreateDto
            {
                Description = description!,
                Amount = 150.75m,
                Date = DateTime.UtcNow
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            validationResults.Should().HaveCountGreaterThan(0);
            validationResults.Should().Contain(vr => vr.MemberNames.Contains("Description"));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100.50)]
        public void ExpenseCreateDto_InvalidAmount_ShouldFailValidation(decimal amount)
        {
            // Arrange
            var dto = new ExpenseCreateDto
            {
                Description = "Test Expense",
                Amount = amount,
                Date = DateTime.UtcNow
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            validationResults.Should().HaveCountGreaterThan(0);
            validationResults.Should().Contain(vr => vr.MemberNames.Contains("Amount"));
        }

        [Fact]
        public void ExpenseUpdateDto_ValidData_ShouldPassValidation()
        {
            // Arrange
            var dto = new ExpenseUpdateDto
            {
                Description = "Updated Expense",
                Amount = 200.50m,
                Date = DateTime.UtcNow,
                Category = "Updated Category",
                Notes = "Updated notes"
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            validationResults.Should().BeEmpty();
        }

        [Fact]
        public void ExpenseUpdateDto_DescriptionTooLong_ShouldFailValidation()
        {
            // Arrange
            var longDescription = new string('a', 501);
            var dto = new ExpenseUpdateDto
            {
                Description = longDescription,
                Amount = 100.50m,
                Date = DateTime.UtcNow
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            validationResults.Should().HaveCountGreaterThan(0);
            validationResults.Should().Contain(vr => vr.MemberNames.Contains("Description"));
        }

        [Fact]
        public void ExpenseDto_Properties_ShouldBeSettable()
        {
            // Arrange & Act
            var dto = new ExpenseDto
            {
                Id = 1,
                UserId = 123,
                Description = "Test Expense",
                Amount = 100.50m,
                Date = DateTime.UtcNow,
                Category = "Test Category",
                Notes = "Test Notes",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };

            // Assert
            dto.Id.Should().Be(1);
            dto.UserId.Should().Be(123);
            dto.Description.Should().Be("Test Expense");
            dto.Amount.Should().Be(100.50m);
            dto.Category.Should().Be("Test Category");
            dto.Notes.Should().Be("Test Notes");
            dto.IsActive.Should().BeTrue();
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