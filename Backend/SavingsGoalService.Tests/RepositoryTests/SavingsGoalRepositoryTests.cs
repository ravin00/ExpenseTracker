using SavingsGoalService.Data;
using SavingsGoalService.Models;
using SavingsGoalService.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace SavingsGoalService.Tests.RepositoryTests
{
    public class SavingsGoalRepositoryTests : IDisposable
    {
        private readonly SavingsGoalDbContext _context;
        private readonly SavingsGoalRepository _repository;

        public SavingsGoalRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<SavingsGoalDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new SavingsGoalDbContext(options);
            _repository = new SavingsGoalRepository(_context);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnUserGoalsOrderedByPriorityAndName()
        {
            // Arrange
            var userId = 1;
            var goal1 = new SavingsGoal { Name = "B Goal", TargetAmount = 1000m, UserId = userId, Priority = SavingsGoalPriority.High };
            var goal2 = new SavingsGoal { Name = "A Goal", TargetAmount = 2000m, UserId = userId, Priority = SavingsGoalPriority.High };
            var goal3 = new SavingsGoal { Name = "C Goal", TargetAmount = 3000m, UserId = 2, Priority = SavingsGoalPriority.Medium }; // Different user
            var goal4 = new SavingsGoal { Name = "D Goal", TargetAmount = 4000m, UserId = userId, Priority = SavingsGoalPriority.Low };

            _context.SavingsGoals.AddRange(goal1, goal2, goal3, goal4);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllAsync(userId);

            // Assert
            result.Should().HaveCount(3);
            result[0].Name.Should().Be("A Goal"); // High priority, A comes before B
            result[1].Name.Should().Be("B Goal"); // High priority
            result[2].Name.Should().Be("D Goal"); // Low priority
        }

        [Fact]
        public async Task GetByStatusAsync_ShouldReturnGoalsWithSpecificStatus()
        {
            // Arrange
            var userId = 1;
            var activeGoal = new SavingsGoal { Name = "Active Goal", TargetAmount = 1000m, UserId = userId, Status = SavingsGoalStatus.Active };
            var completedGoal = new SavingsGoal { Name = "Completed Goal", TargetAmount = 2000m, UserId = userId, Status = SavingsGoalStatus.Completed };
            var pausedGoal = new SavingsGoal { Name = "Paused Goal", TargetAmount = 3000m, UserId = userId, Status = SavingsGoalStatus.Paused };

            _context.SavingsGoals.AddRange(activeGoal, completedGoal, pausedGoal);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByStatusAsync(userId, SavingsGoalStatus.Active);

            // Assert
            result.Should().HaveCount(1);
            result[0].Name.Should().Be("Active Goal");
        }

        [Fact]
        public async Task GetDueSoonAsync_ShouldReturnGoalsDueWithinTimeframe()
        {
            // Arrange
            var userId = 1;
            var dueSoon = new SavingsGoal 
            { 
                Name = "Due Soon", 
                TargetAmount = 1000m, 
                CurrentAmount = 500m,
                UserId = userId, 
                Status = SavingsGoalStatus.Active,
                TargetDate = DateTime.UtcNow.AddDays(15)
            };
            var dueLater = new SavingsGoal 
            { 
                Name = "Due Later", 
                TargetAmount = 2000m, 
                CurrentAmount = 1000m,
                UserId = userId, 
                Status = SavingsGoalStatus.Active,
                TargetDate = DateTime.UtcNow.AddDays(45)
            };
            var completed = new SavingsGoal 
            { 
                Name = "Completed", 
                TargetAmount = 1000m, 
                CurrentAmount = 1000m,
                UserId = userId, 
                Status = SavingsGoalStatus.Active,
                TargetDate = DateTime.UtcNow.AddDays(10)
            };

            _context.SavingsGoals.AddRange(dueSoon, dueLater, completed);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetDueSoonAsync(userId, 30);

            // Assert
            result.Should().HaveCount(1);
            result[0].Name.Should().Be("Due Soon");
        }

        [Fact]
        public async Task AddAsync_ShouldAddGoalToDatabase()
        {
            // Arrange
            var goal = new SavingsGoal
            {
                Name = "New Goal",
                TargetAmount = 1500m,
                UserId = 1
            };

            // Act
            var result = await _repository.AddAsync(goal);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().BeGreaterThan(0);
            
            var savedGoal = await _context.SavingsGoals.FindAsync(result.Id);
            savedGoal.Should().NotBeNull();
            savedGoal!.Name.Should().Be("New Goal");
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateGoalAndMarkAsCompletedWhenTargetReached()
        {
            // Arrange
            var goal = new SavingsGoal
            {
                Name = "Test Goal",
                TargetAmount = 1000m,
                CurrentAmount = 800m,
                UserId = 1,
                Status = SavingsGoalStatus.Active
            };

            _context.SavingsGoals.Add(goal);
            await _context.SaveChangesAsync();

            // Act
            goal.CurrentAmount = 1000m;
            await _repository.UpdateAsync(goal);

            // Assert
            var updatedGoal = await _context.SavingsGoals.FindAsync(goal.Id);
            updatedGoal.Should().NotBeNull();
            updatedGoal!.Status.Should().Be(SavingsGoalStatus.Completed);
            updatedGoal.CompletedAt.Should().NotBeNull();
            updatedGoal.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }

        [Fact]
        public async Task ExistsAsync_ShouldReturnTrueForExistingGoalName()
        {
            // Arrange
            var goal = new SavingsGoal
            {
                Name = "Existing Goal",
                TargetAmount = 1000m,
                UserId = 1
            };

            _context.SavingsGoals.Add(goal);
            await _context.SaveChangesAsync();

            // Act
            var exists = await _repository.ExistsAsync("Existing Goal", 1);
            var notExists = await _repository.ExistsAsync("Non-existing Goal", 1);

            // Assert
            exists.Should().BeTrue();
            notExists.Should().BeFalse();
        }

        [Fact]
        public async Task GetTotalSavingsAsync_ShouldReturnSumOfCurrentAmounts()
        {
            // Arrange
            var userId = 1;
            var goal1 = new SavingsGoal { Name = "Goal 1", TargetAmount = 1000m, CurrentAmount = 500m, UserId = userId };
            var goal2 = new SavingsGoal { Name = "Goal 2", TargetAmount = 2000m, CurrentAmount = 750m, UserId = userId };
            var goal3 = new SavingsGoal { Name = "Goal 3", TargetAmount = 1000m, CurrentAmount = 300m, UserId = 2 }; // Different user

            _context.SavingsGoals.AddRange(goal1, goal2, goal3);
            await _context.SaveChangesAsync();

            // Act
            var total = await _repository.GetTotalSavingsAsync(userId);

            // Assert
            total.Should().Be(1250m); // 500 + 750
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}