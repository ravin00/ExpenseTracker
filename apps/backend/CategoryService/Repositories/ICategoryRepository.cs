using CategoryService.Models;

namespace CategoryService.Repositories
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllAsync(int userId);
        Task<Category?> GetByIdAsync(int id, int userId);
        Task<Category?> GetByNameAsync(string name, int userId);
        Task<Category> AddAsync(Category category);
        Task UpdateAsync(Category category);
        Task DeleteAsync(Category category);
        Task<bool> ExistsAsync(string name, int userId, int? excludeId = null);
    }
}