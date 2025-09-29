using SavingsGoalService.Data;
using SavingsGoalService.Models;
using Microsoft.EntityFrameworkCore;

namespace SavingsGoalService.Repositories
{
    public class SavingsGoalRepository : ISavingsGoalRepository
    {
        private readonly SavingsGoalDbContext _context;

        public SavingsGoalRepository(SavingsGoalDbContext context)
        {
            _context = context;
        }

        public async Task<List<SavingsGoal>> GetAllAsync(int userId)
        {
            return await _context.SavingsGoals
                .Where(sg => sg.UserId == userId)
                .OrderByDescending(sg => sg.Priority)
                .ThenBy(sg => sg.Name)
                .ToListAsync();
        }

        public async Task<List<SavingsGoal>> GetByStatusAsync(int userId, SavingsGoalStatus status)
        {
            return await _context.SavingsGoals
                .Where(sg => sg.UserId == userId && sg.Status == status)
                .OrderByDescending(sg => sg.Priority)
                .ThenBy(sg => sg.Name)
                .ToListAsync();
        }

        public async Task<SavingsGoal?> GetByIdAsync(int id, int userId)
        {
            return await _context.SavingsGoals
                .FirstOrDefaultAsync(sg => sg.Id == id && sg.UserId == userId);
        }

        public async Task<List<SavingsGoal>> GetByCategoryAsync(int categoryId, int userId)
        {
            return await _context.SavingsGoals
                .Where(sg => sg.CategoryId == categoryId && sg.UserId == userId)
                .OrderBy(sg => sg.Priority)
                .ThenBy(sg => sg.Name)
                .ToListAsync();
        }

        public async Task<List<SavingsGoal>> GetByPriorityAsync(int userId, SavingsGoalPriority priority)
        {
            return await _context.SavingsGoals
                .Where(sg => sg.UserId == userId && sg.Priority == priority)
                .OrderBy(sg => sg.Name)
                .ToListAsync();
        }

        public async Task<List<SavingsGoal>> GetDueSoonAsync(int userId, int daysFromNow = 30)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(daysFromNow);
            return await _context.SavingsGoals
                .Where(sg => sg.UserId == userId &&
                           sg.Status == SavingsGoalStatus.Active &&
                           sg.TargetDate.HasValue &&
                           sg.TargetDate.Value <= cutoffDate &&
                           sg.CurrentAmount < sg.TargetAmount)
                .OrderBy(sg => sg.TargetDate)
                .ToListAsync();
        }

        public async Task<SavingsGoal> AddAsync(SavingsGoal savingsGoal)
        {
            _context.SavingsGoals.Add(savingsGoal);
            await _context.SaveChangesAsync();
            return savingsGoal;
        }

        public async Task UpdateAsync(SavingsGoal savingsGoal)
        {
            savingsGoal.UpdatedAt = DateTime.UtcNow;

            // Mark as completed if target reached
            if (savingsGoal.CurrentAmount >= savingsGoal.TargetAmount && savingsGoal.Status == SavingsGoalStatus.Active)
            {
                savingsGoal.Status = SavingsGoalStatus.Completed;
                savingsGoal.CompletedAt = DateTime.UtcNow;
            }

            _context.SavingsGoals.Update(savingsGoal);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(SavingsGoal savingsGoal)
        {
            _context.SavingsGoals.Remove(savingsGoal);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(string name, int userId, int? excludeId = null)
        {
            return await _context.SavingsGoals
                .AnyAsync(sg => sg.Name == name && sg.UserId == userId && (excludeId == null || sg.Id != excludeId));
        }

        public async Task<decimal> GetTotalSavingsAsync(int userId)
        {
            return await _context.SavingsGoals
                .Where(sg => sg.UserId == userId)
                .SumAsync(sg => sg.CurrentAmount);
        }

        public async Task<int> GetCompletedGoalsCountAsync(int userId)
        {
            return await _context.SavingsGoals
                .CountAsync(sg => sg.UserId == userId && sg.Status == SavingsGoalStatus.Completed);
        }
    }
}