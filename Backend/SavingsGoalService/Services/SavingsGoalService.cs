using SavingsGoalService.Dtos;
using SavingsGoalService.Models;
using SavingsGoalService.Repositories;

namespace SavingsGoalService.Services
{
    public class SavingsGoalService : ISavingsGoalService
    {
        private readonly ISavingsGoalRepository _repository;

        public SavingsGoalService(ISavingsGoalRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<SavingsGoal>> GetSavingsGoalsAsync(int userId, SavingsGoalStatus? status = null)
        {
            return status.HasValue
                ? await _repository.GetByStatusAsync(userId, status.Value)
                : await _repository.GetAllAsync(userId);
        }

        public async Task<SavingsGoal?> GetSavingsGoalAsync(int id, int userId)
        {
            return await _repository.GetByIdAsync(id, userId);
        }

        public async Task<List<SavingsGoal>> GetSavingsGoalsByCategoryAsync(int categoryId, int userId)
        {
            return await _repository.GetByCategoryAsync(categoryId, userId);
        }

        public async Task<List<SavingsGoal>> GetSavingsGoalsByPriorityAsync(int userId, SavingsGoalPriority priority)
        {
            return await _repository.GetByPriorityAsync(userId, priority);
        }

        public async Task<List<SavingsGoal>> GetDueSoonAsync(int userId, int daysFromNow = 30)
        {
            return await _repository.GetDueSoonAsync(userId, daysFromNow);
        }

        public async Task<SavingsGoal> CreateSavingsGoalAsync(int userId, SavingsGoalDto dto)
        {
            // Check if savings goal with same name already exists
            if (await _repository.ExistsAsync(dto.Name, userId))
            {
                throw new InvalidOperationException($"Savings goal '{dto.Name}' already exists");
            }

            // Validate target date if provided
            if (dto.TargetDate.HasValue && dto.TargetDate.Value <= dto.StartDate)
            {
                throw new ArgumentException("Target date must be after start date");
            }

            // Validate current amount doesn't exceed target
            if (dto.CurrentAmount > dto.TargetAmount)
            {
                throw new ArgumentException("Current amount cannot exceed target amount");
            }

            var savingsGoal = new SavingsGoal
            {
                UserId = userId,
                Name = dto.Name,
                Description = dto.Description,
                TargetAmount = dto.TargetAmount,
                CurrentAmount = dto.CurrentAmount,
                StartDate = dto.StartDate,
                TargetDate = dto.TargetDate,
                Status = dto.Status,
                Priority = dto.Priority,
                Color = dto.Color,
                CategoryId = dto.CategoryId,
                MonthlyContributionTarget = dto.MonthlyContributionTarget,
                IsAutoContribution = dto.IsAutoContribution,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            return await _repository.AddAsync(savingsGoal);
        }

        public async Task<SavingsGoal> UpdateSavingsGoalAsync(int id, int userId, SavingsGoalDto dto)
        {
            var savingsGoal = await _repository.GetByIdAsync(id, userId);
            if (savingsGoal == null)
            {
                throw new KeyNotFoundException($"Savings goal with ID {id} not found");
            }

            // Check if new name conflicts with existing goals (excluding current one)
            if (await _repository.ExistsAsync(dto.Name, userId, id))
            {
                throw new InvalidOperationException($"Savings goal '{dto.Name}' already exists");
            }

            // Validate target date if provided
            if (dto.TargetDate.HasValue && dto.TargetDate.Value <= dto.StartDate)
            {
                throw new ArgumentException("Target date must be after start date");
            }

            // Validate current amount doesn't exceed target
            if (dto.CurrentAmount > dto.TargetAmount)
            {
                throw new ArgumentException("Current amount cannot exceed target amount");
            }

            // Update properties
            savingsGoal.Name = dto.Name;
            savingsGoal.Description = dto.Description;
            savingsGoal.TargetAmount = dto.TargetAmount;
            savingsGoal.CurrentAmount = dto.CurrentAmount;
            savingsGoal.StartDate = dto.StartDate;
            savingsGoal.TargetDate = dto.TargetDate;
            savingsGoal.Status = dto.Status;
            savingsGoal.Priority = dto.Priority;
            savingsGoal.Color = dto.Color;
            savingsGoal.CategoryId = dto.CategoryId;
            savingsGoal.MonthlyContributionTarget = dto.MonthlyContributionTarget;
            savingsGoal.IsAutoContribution = dto.IsAutoContribution;

            await _repository.UpdateAsync(savingsGoal);
            return savingsGoal;
        }

        public async Task<SavingsGoal> AddContributionAsync(int id, int userId, SavingsContributionDto dto)
        {
            var savingsGoal = await _repository.GetByIdAsync(id, userId);
            if (savingsGoal == null)
            {
                throw new KeyNotFoundException($"Savings goal with ID {id} not found");
            }

            if (savingsGoal.Status != SavingsGoalStatus.Active)
            {
                throw new InvalidOperationException("Cannot add contribution to inactive savings goal");
            }

            savingsGoal.CurrentAmount += dto.Amount;
            await _repository.UpdateAsync(savingsGoal);
            return savingsGoal;
        }

        public async Task<SavingsGoal> WithdrawAsync(int id, int userId, SavingsWithdrawalDto dto)
        {
            var savingsGoal = await _repository.GetByIdAsync(id, userId);
            if (savingsGoal == null)
            {
                throw new KeyNotFoundException($"Savings goal with ID {id} not found");
            }

            if (dto.Amount > savingsGoal.CurrentAmount)
            {
                throw new InvalidOperationException("Withdrawal amount cannot exceed current savings amount");
            }

            savingsGoal.CurrentAmount -= dto.Amount;

            // If goal was completed but now below target, reactivate it
            if (savingsGoal.Status == SavingsGoalStatus.Completed && savingsGoal.CurrentAmount < savingsGoal.TargetAmount)
            {
                savingsGoal.Status = SavingsGoalStatus.Active;
                savingsGoal.CompletedAt = null;
            }

            await _repository.UpdateAsync(savingsGoal);
            return savingsGoal;
        }

        public async Task DeleteSavingsGoalAsync(int id, int userId)
        {
            var savingsGoal = await _repository.GetByIdAsync(id, userId);
            if (savingsGoal == null)
            {
                throw new KeyNotFoundException($"Savings goal with ID {id} not found");
            }

            await _repository.DeleteAsync(savingsGoal);
        }

        public async Task<object> GetSavingsStatisticsAsync(int userId)
        {
            var allGoals = await _repository.GetAllAsync(userId);

            return new
            {
                TotalGoals = allGoals.Count,
                ActiveGoals = allGoals.Count(g => g.Status == SavingsGoalStatus.Active),
                CompletedGoals = allGoals.Count(g => g.Status == SavingsGoalStatus.Completed),
                TotalTargetAmount = allGoals.Sum(g => g.TargetAmount),
                TotalCurrentAmount = allGoals.Sum(g => g.CurrentAmount),
                TotalSavedAmount = allGoals.Where(g => g.Status == SavingsGoalStatus.Completed).Sum(g => g.CurrentAmount),
                AverageProgress = allGoals.Any() ? allGoals.Average(g => g.ProgressPercentage) : 0,
                GoalsDueSoon = await _repository.GetDueSoonAsync(userId, 30)
            };
        }
    }
}