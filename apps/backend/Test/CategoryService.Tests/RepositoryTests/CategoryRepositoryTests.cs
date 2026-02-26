using CategoryService.Data;
using CategoryService.Models;
using CategoryService.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CategoryService.Tests.RepositoryTests
{
    public class CategoryRepositoryTests : IDisposable
    {
        private readonly CategoryDbContext _context;
        private readonly CategoryRepository _repository;

        public CategoryRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<CategoryDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new CategoryDbContext(options);
            _repository = new CategoryRepository(_context);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnUserCategoriesOrderedByName()
        {
            // Arrange
            var userId = 1;
            var category1 = new Category { Name = "B Category", UserId = userId };
            var category2 = new Category { Name = "A Category", UserId = userId };
            var category3 = new Category { Name = "C Category", UserId = 2 }; // Different user

            _context.Categories.AddRange(category1, category2, category3);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllAsync(userId);

            // Assert
            result.Should().HaveCount(2);
            result[0].Name.Should().Be("A Category");
            result[1].Name.Should().Be("B Category");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCategoryForUser()
        {
            // Arrange
            var userId = 1;
            var category = new Category { Name = "Test Category", UserId = userId };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdAsync(category.Id, userId);

            // Assert
            result.Should().NotBeNull();
            result!.Name.Should().Be("Test Category");
            result.UserId.Should().Be(userId);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNullForDifferentUser()
        {
            // Arrange
            var category = new Category { Name = "Test Category", UserId = 1 };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdAsync(category.Id, 2);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByNameAsync_ShouldReturnCategoryForUser()
        {
            // Arrange
            var userId = 1;
            var category = new Category { Name = "Test Category", UserId = userId };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByNameAsync("Test Category", userId);

            // Assert
            result.Should().NotBeNull();
            result!.Name.Should().Be("Test Category");
            result.UserId.Should().Be(userId);
        }

        [Fact]
        public async Task GetByNameAsync_ShouldReturnNullForDifferentUser()
        {
            // Arrange
            var category = new Category { Name = "Test Category", UserId = 1 };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByNameAsync("Test Category", 2);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task AddAsync_ShouldAddAndReturnCategory()
        {
            // Arrange
            var category = new Category { Name = "New Category", UserId = 1 };

            // Act
            var result = await _repository.AddAsync(category);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().BeGreaterThan(0);
            result.Name.Should().Be("New Category");

            var saved = await _context.Categories.FindAsync(result.Id);
            saved.Should().NotBeNull();
            saved!.Name.Should().Be("New Category");
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateCategory()
        {
            // Arrange
            var category = new Category { Name = "Original", UserId = 1 };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            var originalUpdatedAt = category.UpdatedAt;
            await Task.Delay(1); // Ensure time difference

            category.Name = "Updated";
            category.Description = "Updated description";

            // Act
            await _repository.UpdateAsync(category);

            // Assert
            var updated = await _context.Categories.FindAsync(category.Id);
            updated.Should().NotBeNull();
            updated!.Name.Should().Be("Updated");
            updated.Description.Should().Be("Updated description");
            updated.UpdatedAt.Should().BeAfter(originalUpdatedAt);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveCategory()
        {
            // Arrange
            var category = new Category { Name = "To Delete", UserId = 1 };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            // Act
            await _repository.DeleteAsync(category);

            // Assert
            var deleted = await _context.Categories.FindAsync(category.Id);
            deleted.Should().BeNull();
        }

        [Fact]
        public async Task ExistsAsync_ShouldReturnTrueWhenCategoryExists()
        {
            // Arrange
            var category = new Category { Name = "Test Category", UserId = 1 };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.ExistsAsync("Test Category", 1);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task ExistsAsync_ShouldReturnFalseWhenCategoryDoesNotExist()
        {
            // Act
            var result = await _repository.ExistsAsync("Non-existent", 1);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task ExistsAsync_ShouldReturnFalseForDifferentUser()
        {
            // Arrange
            var category = new Category { Name = "Test Category", UserId = 1 };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.ExistsAsync("Test Category", 2);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task ExistsAsync_WithExcludeId_ShouldExcludeSpecifiedCategory()
        {
            // Arrange
            var category = new Category { Name = "Test Category", UserId = 1 };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.ExistsAsync("Test Category", 1, category.Id);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task ExistsAsync_WithExcludeId_ShouldReturnTrueForOtherCategories()
        {
            // Arrange
            var category1 = new Category { Name = "Test Category", UserId = 1 };
            var category2 = new Category { Name = "Test Category", UserId = 1 };
            _context.Categories.AddRange(category1, category2);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.ExistsAsync("Test Category", 1, category1.Id);

            // Assert
            result.Should().BeTrue();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}