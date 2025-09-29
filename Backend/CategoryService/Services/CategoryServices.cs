using CategoryService.Dtos;
using CategoryService.Models;
using CategoryService.Repositories;

namespace CategoryService.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;

        public CategoryService(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Category>> GetCategoriesAsync(int userId)
        {
            return await _repository.GetAllAsync(userId);
        }

        public async Task<Category?> GetCategoryAsync(int id, int userId)
        {
            return await _repository.GetByIdAsync(id, userId);
        }

        public async Task<Category> CreateCategoryAsync(int userId, CategoryDto dto)
        {
            // Check if category with same name already exists for this user
            if (await _repository.ExistsAsync(dto.Name, userId))
            {
                throw new InvalidOperationException($"Category '{dto.Name}' already exists");
            }

            var category = new Category
            {
                UserId = userId,
                Name = dto.Name,
                Description = dto.Description,
                Color = dto.Color,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            return await _repository.AddAsync(category);
        }

        public async Task<Category> UpdateCategoryAsync(int id, int userId, CategoryDto dto)
        {
            var category = await _repository.GetByIdAsync(id, userId);
            if (category == null)
            {
                throw new KeyNotFoundException($"Category with ID {id} not found");
            }

            // Check if new name conflicts with existing categories (excluding current one)
            if (await _repository.ExistsAsync(dto.Name, userId, id))
            {
                throw new InvalidOperationException($"Category '{dto.Name}' already exists");
            }

            category.Name = dto.Name;
            category.Description = dto.Description;
            category.Color = dto.Color;

            await _repository.UpdateAsync(category);
            return category;
        }

        public async Task DeleteCategoryAsync(int id, int userId)
        {
            var category = await _repository.GetByIdAsync(id, userId);
            if (category == null)
            {
                throw new KeyNotFoundException($"Category with ID {id} not found");
            }

            await _repository.DeleteAsync(category);
        }
    }
}