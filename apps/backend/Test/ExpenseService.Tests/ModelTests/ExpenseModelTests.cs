using ExpenseService.Models;
using FluentAssertions;
using System.ComponentModel.DataAnnotations;

namespace ExpenseService.Tests.ModelTests
{
    public class ExpenseModelTests
    {
        [Fact]
        public void Expense_DefaultValues_ShouldBeCorrect()
        {
            // Arrange & Act
            var expense = new Expense
            {
                Description = "Test Expense",
                Amount = 100.50m,
                UserId = 1
            };

            // Assert
            expense.Date.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
            expense.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
            expense.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
            expense.IsActive.Should().BeTrue();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Expense_InvalidDescription_ShouldFailValidation(string description)
        {
            // Arrange
            var expense = new Expense
            {
                Description = description!,
                Amount = 100.50m,
                UserId = 1
            };

            // Act
            var validationResults = ValidateModel(expense);

            // Assert
            validationResults.Should().HaveCountGreaterThan(0);
            validationResults.Should().Contain(vr => vr.MemberNames.Contains("Description"));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100.50)]
        public void Expense_InvalidAmount_ShouldFailValidation(decimal amount)
        {
            // Arrange
            var expense = new Expense
            {
                Description = "Test Expense",
                Amount = amount,
                UserId = 1
            };

            // Act
            var validationResults = ValidateModel(expense);

            // Assert
            validationResults.Should().HaveCountGreaterThan(0);
            validationResults.Should().Contain(vr => vr.MemberNames.Contains("Amount"));
        }

        [Theory]
        [InlineData(0.01)]
        [InlineData(1)]
        [InlineData(100.50)]
        [InlineData(999999.99)]
        public void Expense_ValidAmount_ShouldPassValidation(decimal amount)
        {
            // Arrange
            var expense = new Expense
            {
                Description = "Test Expense",
                Amount = amount,
                UserId = 1
            };

            // Act
            var validationResults = ValidateModel(expense);

            // Assert
            validationResults.Should().BeEmpty();
        }

        [Fact]
        public void Expense_DescriptionTooLong_ShouldFailValidation()
        {
            // Arrange
            var longDescription = new string('a', 501); // 501 characters
            var expense = new Expense
            {
                Description = longDescription,
                Amount = 100.50m,
                UserId = 1
            };

            // Act
            var validationResults = ValidateModel(expense);

            // Assert
            validationResults.Should().HaveCountGreaterThan(0);
            validationResults.Should().Contain(vr => vr.MemberNames.Contains("Description"));
        }

        [Fact]
        public void Expense_CategoryTooLong_ShouldFailValidation()
        {
            // Arrange
            var longCategory = new string('a', 101); // 101 characters
            var expense = new Expense
            {
                Description = "Test Expense",
                Amount = 100.50m,
                UserId = 1,
                Category = longCategory
            };

            // Act
            var validationResults = ValidateModel(expense);

            // Assert
            validationResults.Should().HaveCountGreaterThan(0);
            validationResults.Should().Contain(vr => vr.MemberNames.Contains("Category"));
        }

        [Fact]
        public void Expense_NotesTooLong_ShouldFailValidation()
        {
            // Arrange
            var longNotes = new string('a', 1001); // 1001 characters
            var expense = new Expense
            {
                Description = "Test Expense",
                Amount = 100.50m,
                UserId = 1,
                Notes = longNotes
            };

            // Act
            var validationResults = ValidateModel(expense);

            // Assert
            validationResults.Should().HaveCountGreaterThan(0);
            validationResults.Should().Contain(vr => vr.MemberNames.Contains("Notes"));
        }

        [Fact]
        public void Expense_ValidData_ShouldPassValidation()
        {
            // Arrange
            var expense = new Expense
            {
                Description = "Grocery Shopping",
                Amount = 150.75m,
                UserId = 1,
                Category = "Food",
                Notes = "Weekly grocery shopping at local supermarket"
            };

            // Act
            var validationResults = ValidateModel(expense);

            // Assert
            validationResults.Should().BeEmpty();
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