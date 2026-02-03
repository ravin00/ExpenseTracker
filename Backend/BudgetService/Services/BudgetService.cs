using BudgetService.Dtos;
using BudgetService.Models;
using BudgetService.Repositories;

namespace BudgetService.Services
{
    public class BudgetServiceImpl : IBudgetService
    {
        private readonly IBudgetRepository _repository;

        public BudgetServiceImpl(IBudgetRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Budget>> GetBudgetsAsync(int userId, bool activeOnly = false)
        {
            return activeOnly
                ? await _repository.GetActiveAsync(userId)
                : await _repository.GetAllAsync(userId);
        }

        public async Task<Budget?> GetBudgetAsync(int id, int userId)
        {
            return await _repository.GetByIdAsync(id, userId);
        }

        public async Task<List<Budget>> GetBudgetsByCategoryAsync(int categoryId, int userId)
        {
            return await _repository.GetByCategoryAsync(categoryId, userId);
        }

        public async Task<Budget> CreateBudgetAsync(int userId, BudgetDto dto)
        {
            // Check if budget with same name already exists for this user
            if (await _repository.ExistsAsync(dto.Name, userId))
            {
                throw new InvalidOperationException($"Budget '{dto.Name}' already exists");
            }

            // Validate end date if provided
            if (dto.EndDate.HasValue && dto.EndDate.Value <= dto.StartDate)
            {
                throw new ArgumentException("End date must be after start date");
            }

            var budget = new Budget
            {
                UserId = userId,
                Name = dto.Name,
                Description = dto.Description,
                Amount = dto.Amount,
                CategoryId = dto.CategoryId,
                Period = dto.Period,
                StartDate = DateTime.SpecifyKind(dto.StartDate, DateTimeKind.Utc),
                EndDate = dto.EndDate.HasValue ? DateTime.SpecifyKind(dto.EndDate.Value, DateTimeKind.Utc) : null,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            return await _repository.AddAsync(budget);
        }

        public async Task<Budget> UpdateBudgetAsync(int id, int userId, BudgetDto dto)
        {
            var budget = await _repository.GetByIdAsync(id, userId);
            if (budget == null)
            {
                throw new KeyNotFoundException($"Budget with ID {id} not found");
            }

            // Check if new name conflicts with existing budgets (excluding current one)
            if (await _repository.ExistsAsync(dto.Name, userId, id))
            {
                throw new InvalidOperationException($"Budget '{dto.Name}' already exists");
            }

            // Validate end date if provided
            if (dto.EndDate.HasValue && dto.EndDate.Value <= dto.StartDate)
            {
                throw new ArgumentException("End date must be after start date");
            }

            budget.Name = dto.Name;
            budget.Description = dto.Description;
            budget.Amount = dto.Amount;
            budget.CategoryId = dto.CategoryId;
            budget.Period = dto.Period;
            budget.StartDate = DateTime.SpecifyKind(dto.StartDate, DateTimeKind.Utc);
            budget.EndDate = dto.EndDate.HasValue ? DateTime.SpecifyKind(dto.EndDate.Value, DateTimeKind.Utc) : null;
            budget.IsActive = dto.IsActive;

            await _repository.UpdateAsync(budget);
            return budget;
        }

        public async Task DeleteBudgetAsync(int id, int userId)
        {
            var budget = await _repository.GetByIdAsync(id, userId);
            if (budget == null)
            {
                throw new KeyNotFoundException($"Budget with ID {id} not found");
            }

            await _repository.DeleteAsync(budget);
        }

        public async Task UpdateSpentAmountAsync(int budgetId, decimal spentAmount, int userId)
        {
            var budget = await _repository.GetByIdAsync(budgetId, userId);
            if (budget == null)
            {
                throw new KeyNotFoundException($"Budget with ID {budgetId} not found");
            }

            await _repository.UpdateSpentAmountAsync(budgetId, spentAmount, userId);
        }

        public decimal GetBudgetProgress(Budget budget)
        {
            if (budget.Amount <= 0) return 0;
            return Math.Min(100, (budget.SpentAmount / budget.Amount) * 100);
        }

        public decimal GetRemainingAmount(Budget budget)
        {
            return Math.Max(0, budget.Amount - budget.SpentAmount);
        }

        public bool IsBudgetExceeded(Budget budget)
        {
            return budget.SpentAmount > budget.Amount;
        }
    }
}