using ExpenseService.Data;
using ExpenseService.Models;
using ExpenseService.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace ExpenseService.Tests.RepositoryTests
{
    public class ExpenseRepositoryTests : IDisposable
    {
        private readonly ExpenseDbContext _context;
        private readonly ExpenseRepository _repository;

        public ExpenseRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ExpenseDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ExpenseDbContext(options);
            _repository = new ExpenseRepository(_context);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnUserExpenses()
        {
            // Arrange
            var userId = 1;
            var expense1 = new Expense { Description = "Expense 1", Amount = 100m, UserId = userId, IsActive = true };
            var expense2 = new Expense { Description = "Expense 2", Amount = 200m, UserId = userId, IsActive = true };
            var expense3 = new Expense { Description = "Expense 3", Amount = 300m, UserId = 2, IsActive = true }; // Different user
            var expense4 = new Expense { Description = "Expense 4", Amount = 400m, UserId = userId, IsActive = false }; // Inactive

            _context.Expenses.AddRange(expense1, expense2, expense3, expense4);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllAsync(userId);

            // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(e => e.Description == "Expense 1");
            result.Should().Contain(e => e.Description == "Expense 2");
            result.Should().NotContain(e => e.Description == "Expense 3"); // Different user
            result.Should().NotContain(e => e.Description == "Expense 4"); // Inactive
        }

        [Fact]
        public async Task GetByIdAsync_WithValidIdAndUserId_ShouldReturnExpense()
        {
            // Arrange
            var userId = 1;
            var expense = new Expense
            {
                Description = "Test Expense",
                Amount = 100m,
                UserId = userId,
                IsActive = true
            };

            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdAsync(expense.Id, userId);

            // Assert
            result.Should().NotBeNull();
            result!.Description.Should().Be("Test Expense");
            result.Amount.Should().Be(100m);
            result.UserId.Should().Be(userId);
        }

        [Fact]
        public async Task GetByIdAsync_WithWrongUserId_ShouldReturnNull()
        {
            // Arrange
            var expense = new Expense
            {
                Description = "Test Expense",
                Amount = 100m,
                UserId = 1
            };

            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdAsync(expense.Id, 2); // Different user

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task AddAsync_ShouldAddExpenseToDatabase()
        {
            // Arrange
            var expense = new Expense
            {
                Description = "New Expense",
                Amount = 150m,
                UserId = 1
            };

            // Act
            await _repository.AddAsync(expense);

            // Assert
            var savedExpense = await _context.Expenses.FirstOrDefaultAsync(e => e.Description == "New Expense");
            savedExpense.Should().NotBeNull();
            savedExpense!.Amount.Should().Be(150m);
            savedExpense.UserId.Should().Be(1);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateExpenseInDatabase()
        {
            // Arrange
            var expense = new Expense
            {
                Description = "Original Description",
                Amount = 100m,
                UserId = 1
            };

            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();

            // Act
            expense.Description = "Updated Description";
            expense.Amount = 200m;
            expense.UpdatedAt = DateTime.UtcNow;
            await _repository.UpdateAsync(expense);

            // Assert
            var updatedExpense = await _context.Expenses.FindAsync(expense.Id);
            updatedExpense.Should().NotBeNull();
            updatedExpense!.Description.Should().Be("Updated Description");
            updatedExpense.Amount.Should().Be(200m);
        }

        [Fact]
        public async Task DeleteAsync_ShouldSoftDeleteExpenseInDatabase()
        {
            // Arrange
            var expense = new Expense
            {
                Description = "To Delete",
                Amount = 100m,
                UserId = 1,
                IsActive = true
            };

            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();

            // Act
            await _repository.DeleteAsync(expense);

            // Assert
            var deletedExpense = await _context.Expenses.FindAsync(expense.Id);
            deletedExpense.Should().NotBeNull();
            deletedExpense!.IsActive.Should().BeFalse();
            deletedExpense.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }

        [Fact]
        public async Task GetAllAsync_WithNoExpenses_ShouldReturnEmptyList()
        {
            // Act
            var result = await _repository.GetAllAsync(1);

            // Assert
            result.Should().BeEmpty();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}