using SavingsGoalService.Dtos;
using SavingsGoalService.Models;
using SavingsGoalService.Repositories;
using SavingsGoalService.Services;
using FluentAssertions;
using Moq;

namespace SavingsGoalService.Tests.ServiceTests
{
    public class SavingsGoalServiceTests
    {
        private readonly Mock<ISavingsGoalRepository> _mockRepository;
        private readonly SavingsGoalService.Services.SavingsGoalService _service;

        public SavingsGoalServiceTests()
        {
            _mockRepository = new Mock<ISavingsGoalRepository>();
            _service = new SavingsGoalService.Services.SavingsGoalService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetSavingsGoalsAsync_WithoutStatus_ShouldReturnAllGoals()
        {
            // Arrange
            var userId = 1;
            var goals = new List<SavingsGoal>
            {
                new() { Id = 1, Name = "Goal 1", TargetAmount = 1000m, UserId = userId },
                new() { Id = 2, Name = "Goal 2", TargetAmount = 2000m, UserId = userId }
            };

            _mockRepository.Setup(r => r.GetAllAsync(userId))
                          .ReturnsAsync(goals);

            // Act
            var result = await _service.GetSavingsGoalsAsync(userId);

            // Assert
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(goals);
            _mockRepository.Verify(r => r.GetAllAsync(userId), Times.Once);
        }

        [Fact]
        public async Task CreateSavingsGoalAsync_WithValidData_ShouldCreateGoal()
        {
            // Arrange
            var userId = 1;
            var dto = new SavingsGoalDto
            {
                Name = "Emergency Fund",
                TargetAmount = 5000m,
                StartDate = DateTime.UtcNow,
                Priority = SavingsGoalPriority.High
            };

            var createdGoal = new SavingsGoal
            {
                Id = 1,
                Name = dto.Name,
                TargetAmount = dto.TargetAmount,
                UserId = userId
            };

            _mockRepository.Setup(r => r.ExistsAsync(dto.Name, userId, null))
                          .ReturnsAsync(false);
            _mockRepository.Setup(r => r.AddAsync(It.IsAny<SavingsGoal>()))
                          .ReturnsAsync(createdGoal);

            // Act
            var result = await _service.CreateSavingsGoalAsync(userId, dto);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(dto.Name);
            result.TargetAmount.Should().Be(dto.TargetAmount);
            _mockRepository.Verify(r => r.AddAsync(It.Is<SavingsGoal>(g => 
                g.Name == dto.Name && 
                g.TargetAmount == dto.TargetAmount &&
                g.UserId == userId)), Times.Once);
        }

        [Fact]
        public async Task CreateSavingsGoalAsync_WithDuplicateName_ShouldThrowException()
        {
            // Arrange
            var userId = 1;
            var dto = new SavingsGoalDto
            {
                Name = "Existing Goal",
                TargetAmount = 1000m
            };

            _mockRepository.Setup(r => r.ExistsAsync(dto.Name, userId, null))
                          .ReturnsAsync(true);

            // Act & Assert
            await _service.Invoking(s => s.CreateSavingsGoalAsync(userId, dto))
                         .Should().ThrowAsync<InvalidOperationException>()
                         .WithMessage("*already exists*");
        }

        [Fact]
        public async Task CreateSavingsGoalAsync_WithInvalidTargetDate_ShouldThrowException()
        {
            // Arrange
            var userId = 1;
            var dto = new SavingsGoalDto
            {
                Name = "Test Goal",
                TargetAmount = 1000m,
                StartDate = DateTime.UtcNow,
                TargetDate = DateTime.UtcNow.AddDays(-1) // Past date
            };

            _mockRepository.Setup(r => r.ExistsAsync(dto.Name, userId, null))
                          .ReturnsAsync(false);

            // Act & Assert
            await _service.Invoking(s => s.CreateSavingsGoalAsync(userId, dto))
                         .Should().ThrowAsync<ArgumentException>()
                         .WithMessage("*Target date must be after start date*");
        }

        [Fact]
        public async Task AddContributionAsync_WithValidData_ShouldAddToCurrentAmount()
        {
            // Arrange
            var userId = 1;
            var goalId = 1;
            var goal = new SavingsGoal
            {
                Id = goalId,
                Name = "Test Goal",
                TargetAmount = 1000m,
                CurrentAmount = 500m,
                UserId = userId,
                Status = SavingsGoalStatus.Active
            };
            var contribution = new SavingsContributionDto
            {
                Amount = 200m,
                Notes = "Monthly contribution"
            };

            _mockRepository.Setup(r => r.GetByIdAsync(goalId, userId))
                          .ReturnsAsync(goal);
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<SavingsGoal>()))
                          .Returns(Task.CompletedTask);

            // Act
            var result = await _service.AddContributionAsync(goalId, userId, contribution);

            // Assert
            result.CurrentAmount.Should().Be(700m); // 500 + 200
            _mockRepository.Verify(r => r.UpdateAsync(goal), Times.Once);
        }

        [Fact]
        public async Task AddContributionAsync_ToInactiveGoal_ShouldThrowException()
        {
            // Arrange
            var userId = 1;
            var goalId = 1;
            var goal = new SavingsGoal
            {
                Id = goalId,
                Name = "Test Goal",
                TargetAmount = 1000m,
                CurrentAmount = 500m,
                UserId = userId,
                Status = SavingsGoalStatus.Paused
            };
            var contribution = new SavingsContributionDto { Amount = 200m };

            _mockRepository.Setup(r => r.GetByIdAsync(goalId, userId))
                          .ReturnsAsync(goal);

            // Act & Assert
            await _service.Invoking(s => s.AddContributionAsync(goalId, userId, contribution))
                         .Should().ThrowAsync<InvalidOperationException>()
                         .WithMessage("*Cannot add contribution to inactive*");
        }

        [Fact]
        public async Task WithdrawAsync_WithValidAmount_ShouldReduceCurrentAmount()
        {
            // Arrange
            var userId = 1;
            var goalId = 1;
            var goal = new SavingsGoal
            {
                Id = goalId,
                Name = "Test Goal",
                TargetAmount = 1000m,
                CurrentAmount = 500m,
                UserId = userId,
                Status = SavingsGoalStatus.Active
            };
            var withdrawal = new SavingsWithdrawalDto
            {
                Amount = 100m,
                Reason = "Emergency expense"
            };

            _mockRepository.Setup(r => r.GetByIdAsync(goalId, userId))
                          .ReturnsAsync(goal);
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<SavingsGoal>()))
                          .Returns(Task.CompletedTask);

            // Act
            var result = await _service.WithdrawAsync(goalId, userId, withdrawal);

            // Assert
            result.CurrentAmount.Should().Be(400m); // 500 - 100
            _mockRepository.Verify(r => r.UpdateAsync(goal), Times.Once);
        }

        [Fact]
        public async Task WithdrawAsync_WithExcessiveAmount_ShouldThrowException()
        {
            // Arrange
            var userId = 1;
            var goalId = 1;
            var goal = new SavingsGoal
            {
                Id = goalId,
                Name = "Test Goal",
                TargetAmount = 1000m,
                CurrentAmount = 500m,
                UserId = userId
            };
            var withdrawal = new SavingsWithdrawalDto
            {
                Amount = 600m, // More than current amount
                Reason = "Emergency expense"
            };

            _mockRepository.Setup(r => r.GetByIdAsync(goalId, userId))
                          .ReturnsAsync(goal);

            // Act & Assert
            await _service.Invoking(s => s.WithdrawAsync(goalId, userId, withdrawal))
                         .Should().ThrowAsync<InvalidOperationException>()
                         .WithMessage("*cannot exceed current savings amount*");
        }

        [Fact]
        public async Task GetSavingsStatisticsAsync_ShouldReturnCorrectStatistics()
        {
            // Arrange
            var userId = 1;
            var goals = new List<SavingsGoal>
            {
                new() { Name = "Emergency Fund", TargetAmount = 1000m, CurrentAmount = 500m, Status = SavingsGoalStatus.Active },
                new() { Name = "Vacation Fund", TargetAmount = 2000m, CurrentAmount = 2000m, Status = SavingsGoalStatus.Completed },
                new() { Name = "Car Fund", TargetAmount = 500m, CurrentAmount = 250m, Status = SavingsGoalStatus.Paused }
            };

            _mockRepository.Setup(r => r.GetAllAsync(userId))
                          .ReturnsAsync(goals);
            _mockRepository.Setup(r => r.GetDueSoonAsync(userId, 30))
                          .ReturnsAsync(new List<SavingsGoal>());

            // Act
            var result = await _service.GetSavingsStatisticsAsync(userId);

            // Assert
            result.Should().NotBeNull();
            // Verify the structure and some key values
            var statsDict = result.GetType().GetProperties().ToDictionary(p => p.Name, p => p.GetValue(result));
            statsDict["TotalGoals"].Should().Be(3);
            statsDict["ActiveGoals"].Should().Be(1);
            statsDict["CompletedGoals"].Should().Be(1);
        }
    }
}