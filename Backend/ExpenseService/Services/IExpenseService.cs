using ExpenseService.Dtos;
using ExpenseService.Events;
using ExpenseService.Models;

namespace ExpenseService.Services
{
    public interface IExpenseService
    {
        Task<List<Expense>> GetExpensesAsync(int userId);
        Task<Expense?> GetExpenseAsync(int id, int userId);
        Task<ExpenseCreatedEvent> AddExpenseAsync(int userId, ExpenseDto dto);
        Task UpdateExpenseAsync(Expense expense);
        Task DeleteExpenseAsync(Expense expense);
    }
}