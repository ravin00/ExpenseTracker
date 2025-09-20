using ExpenseService.Dtos;
using ExpenseService.Events;
using ExpenseService.Models;
using ExpenseService.Repositories;

namespace ExpenseService.Services
{
    public class ExpenseService
    {
        private readonly ExpenseRepository _repository;

        public ExpenseService(ExpenseRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Expense>> GetExpensesAsync(int userId)
        {
            return await _repository.GetAllAsync(userId);
        }

        public async Task<Expense?> GetExpenseAsync(int id, int userId)
        {
            return await _repository.GetByIdAsync(id, userId);
        }

        public async Task<ExpenseCreatedEvent> AddExpenseAsync(int userId, ExpenseDto dto)
        {
            var expense = new Expense
            {
                UserId = userId,
                Description = dto.Description,
                Amount = dto.Amount,
                Date = dto.Date
            };

            await _repository.AddAsync(expense);

            // Create event for async processing (Kafka/RabbitMQ later)
            return new ExpenseCreatedEvent
            {
                ExpenseId = expense.Id,
                UserId = userId,
                Amount = dto.Amount,
                Date = dto.Date
            };
        }

        public async Task UpdateExpenseAsync(Expense expense)
        {
            await _repository.UpdateAsync(expense);
        }

        public async Task DeleteExpenseAsync(Expense expense)
        {
            await _repository.DeleteAsync(expense);
        }
    }
}
