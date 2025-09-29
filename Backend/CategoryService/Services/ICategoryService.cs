using CategoryService.Dtos;
using CategoryService.Models;

namespace CategoryService.Services
{
    public interface ICategoryService
    {
        Task<List<Category>> GetCategoriesAsync(int userId);
        Task<Category?> GetCategoryAsync(int id, int userId);
        Task<Category> CreateCategoryAsync(int userId, CategoryDto dto);
        Task<Category> UpdateCategoryAsync(int id, int userId, CategoryDto dto);
        Task DeleteCategoryAsync(int id, int userId);
    }
}