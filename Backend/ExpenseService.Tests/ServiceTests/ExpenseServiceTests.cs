using ExpenseService.Dtos;
using ExpenseService.Models;
using ExpenseService.Repositories;
using ExpenseService.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace ExpenseService.Tests.ServiceTests
{
    public class ExpenseServiceTests
    {
        private readonly Mock<IExpenseRepository> _mockRepository;
        private readonly ExpenseService.Services.ExpenseService _service;

        public ExpenseServiceTests()
        {
            _mockRepository = new Mock<IExpenseRepository>();
            _service = new ExpenseService.Services.ExpenseService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetExpensesAsync_ShouldReturnUserExpenses()
        {
            // Arrange
            var userId = 1;
            var expenses = new List<Expense>
            {
                new() { Id = 1, Description = "Expense 1", Amount = 100m, UserId = userId },
                new() { Id = 2, Description = "Expense 2", Amount = 200m, UserId = userId }
            };

            _mockRepository.Setup(r => r.GetAllAsync(userId))
                          .ReturnsAsync(expenses);

            // Act
            var result = await _service.GetExpensesAsync(userId);

            // Assert
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(expenses);
            _mockRepository.Verify(r => r.GetAllAsync(userId), Times.Once);
        }

        [Fact]
        public async Task GetExpenseAsync_WithValidId_ShouldReturnExpense()
        {
            // Arrange
            var userId = 1;
            var expenseId = 1;
            var expense = new Expense 
            { 
                Id = expenseId, 
                Description = "Test Expense", 
                Amount = 100m, 
                UserId = userId 
            };

            _mockRepository.Setup(r => r.GetByIdAsync(expenseId, userId))
                          .ReturnsAsync(expense);

            // Act
            var result = await _service.GetExpenseAsync(expenseId, userId);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expense);
            _mockRepository.Verify(r => r.GetByIdAsync(expenseId, userId), Times.Once);
        }

        [Fact]
        public async Task GetExpenseAsync_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            var userId = 1;
            var expenseId = 999;

            _mockRepository.Setup(r => r.GetByIdAsync(expenseId, userId))
                          .ReturnsAsync((Expense?)null);

            // Act
            var result = await _service.GetExpenseAsync(expenseId, userId);

            // Assert
            result.Should().BeNull();
            _mockRepository.Verify(r => r.GetByIdAsync(expenseId, userId), Times.Once);
        }

        [Fact]
        public async Task AddExpenseAsync_ShouldCreateExpenseAndReturnEvent()
        {
            // Arrange
            var userId = 1;
            var dto = new ExpenseDto
            {
                Id = 0,
                UserId = userId,
                Description = "New Expense",
                Amount = 150m,
                Date = DateTime.UtcNow,
                Category = "Food",
                Notes = "Test notes",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _mockRepository.Setup(r => r.AddAsync(It.IsAny<Expense>()))
                          .Returns(Task.CompletedTask);

            // Act
            var result = await _service.AddExpenseAsync(userId, dto);

            // Assert
            result.Should().NotBeNull();
            result.UserId.Should().Be(userId);
            result.Amount.Should().Be(150m);
            result.Date.Should().BeCloseTo(dto.Date, TimeSpan.FromSeconds(1));
            _mockRepository.Verify(r => r.AddAsync(It.Is<Expense>(e => 
                e.UserId == userId && 
                e.Description == "New Expense" && 
                e.Amount == 150m)), Times.Once);
        }

        [Fact]
        public async Task UpdateExpenseAsync_ShouldCallRepositoryUpdate()
        {
            // Arrange
            var expense = new Expense 
            { 
                Id = 1, 
                Description = "Updated Expense", 
                Amount = 200m, 
                UserId = 1 
            };

            _mockRepository.Setup(r => r.UpdateAsync(expense))
                          .Returns(Task.CompletedTask);

            // Act
            await _service.UpdateExpenseAsync(expense);

            // Assert
            _mockRepository.Verify(r => r.UpdateAsync(expense), Times.Once);
        }

        [Fact]
        public async Task DeleteExpenseAsync_ShouldCallRepositoryDelete()
        {
            // Arrange
            var expense = new Expense 
            { 
                Id = 1, 
                Description = "To Delete", 
                Amount = 100m, 
                UserId = 1 
            };

            _mockRepository.Setup(r => r.DeleteAsync(expense))
                          .Returns(Task.CompletedTask);

            // Act
            await _service.DeleteExpenseAsync(expense);

            // Assert
            _mockRepository.Verify(r => r.DeleteAsync(expense), Times.Once);
        }

        [Fact]
        public async Task AddExpenseAsync_ShouldSetCorrectExpenseProperties()
        {
            // Arrange
            var userId = 1;
            var dto = new ExpenseDto
            {
                Id = 0,
                UserId = userId,
                Description = "Grocery Shopping",
                Amount = 75.50m,
                Date = new DateTime(2024, 1, 15),
                Category = "Food",
                Notes = "Weekly groceries",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };

            Expense? capturedExpense = null;
            _mockRepository.Setup(r => r.AddAsync(It.IsAny<Expense>()))
                          .Callback<Expense>(e => capturedExpense = e)
                          .Returns(Task.CompletedTask);

            // Act
            await _service.AddExpenseAsync(userId, dto);

            // Assert
            capturedExpense.Should().NotBeNull();
            capturedExpense!.UserId.Should().Be(userId);
            capturedExpense.Description.Should().Be("Grocery Shopping");
            capturedExpense.Amount.Should().Be(75.50m);
            capturedExpense.Date.Should().Be(new DateTime(2024, 1, 15));
        }
    }
}