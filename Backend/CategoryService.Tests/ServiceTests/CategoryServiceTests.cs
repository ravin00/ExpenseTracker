using CategoryService.Dtos;
using CategoryService.Models;
using CategoryService.Repositories;
using CategoryService.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace CategoryService.Tests.ServiceTests
{
    public class CategoryServiceTests
    {
        private readonly Mock<ICategoryRepository> _mockRepository;
        private readonly CategoryService.Services.CategoryService _service;

        public CategoryServiceTests()
        {
            _mockRepository = new Mock<ICategoryRepository>();
            _service = new CategoryService.Services.CategoryService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetCategoriesAsync_ShouldReturnAllUserCategories()
        {
            // Arrange
            var userId = 1;
            var categories = new List<Category>
            {
                new() { Name = "Category 1", UserId = userId },
                new() { Name = "Category 2", UserId = userId }
            };
            _mockRepository.Setup(r => r.GetAllAsync(userId))
                          .ReturnsAsync(categories);

            // Act
            var result = await _service.GetCategoriesAsync(userId);

            // Assert
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(categories);
            _mockRepository.Verify(r => r.GetAllAsync(userId), Times.Once);
        }

        [Fact]
        public async Task GetCategoryAsync_ShouldReturnCategory()
        {
            // Arrange
            var categoryId = 1;
            var userId = 1;
            var category = new Category { Name = "Test Category", UserId = userId };
            _mockRepository.Setup(r => r.GetByIdAsync(categoryId, userId))
                          .ReturnsAsync(category);

            // Act
            var result = await _service.GetCategoryAsync(categoryId, userId);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(category);
            _mockRepository.Verify(r => r.GetByIdAsync(categoryId, userId), Times.Once);
        }

        [Fact]
        public async Task GetCategoryAsync_ShouldReturnNullWhenNotFound()
        {
            // Arrange
            var categoryId = 1;
            var userId = 1;
            _mockRepository.Setup(r => r.GetByIdAsync(categoryId, userId))
                          .ReturnsAsync((Category?)null);

            // Act
            var result = await _service.GetCategoryAsync(categoryId, userId);

            // Assert
            result.Should().BeNull();
            _mockRepository.Verify(r => r.GetByIdAsync(categoryId, userId), Times.Once);
        }

        [Fact]
        public async Task CreateCategoryAsync_ShouldCreateAndReturnCategory()
        {
            // Arrange
            var userId = 1;
            var dto = new CategoryDto 
            { 
                Name = "New Category",
                Description = "Test description",
                Color = "#FF5733"
            };
            var createdCategory = new Category 
            { 
                Id = 1,
                Name = dto.Name,
                Description = dto.Description,
                Color = dto.Color,
                UserId = userId
            };

            _mockRepository.Setup(r => r.ExistsAsync(dto.Name, userId, null))
                          .ReturnsAsync(false);
            _mockRepository.Setup(r => r.AddAsync(It.IsAny<Category>()))
                          .ReturnsAsync(createdCategory);

            // Act
            var result = await _service.CreateCategoryAsync(userId, dto);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(dto.Name);
            result.Description.Should().Be(dto.Description);
            result.Color.Should().Be(dto.Color);
            result.UserId.Should().Be(userId);

            _mockRepository.Verify(r => r.ExistsAsync(dto.Name, userId, null), Times.Once);
            _mockRepository.Verify(r => r.AddAsync(It.Is<Category>(c => 
                c.Name == dto.Name && 
                c.Description == dto.Description && 
                c.Color == dto.Color && 
                c.UserId == userId)), Times.Once);
        }

        [Fact]
        public async Task CreateCategoryAsync_ShouldThrowWhenCategoryExists()
        {
            // Arrange
            var userId = 1;
            var dto = new CategoryDto { Name = "Existing Category" };

            _mockRepository.Setup(r => r.ExistsAsync(dto.Name, userId, null))
                          .ReturnsAsync(true);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.CreateCategoryAsync(userId, dto));

            exception.Message.Should().Contain("already exists");
            _mockRepository.Verify(r => r.ExistsAsync(dto.Name, userId, null), Times.Once);
            _mockRepository.Verify(r => r.AddAsync(It.IsAny<Category>()), Times.Never);
        }

        [Fact]
        public async Task UpdateCategoryAsync_ShouldUpdateAndReturnCategory()
        {
            // Arrange
            var categoryId = 1;
            var userId = 1;
            var existingCategory = new Category 
            { 
                Id = categoryId,
                Name = "Old Name",
                Description = "Old description",
                Color = "#000000",
                UserId = userId
            };
            var dto = new CategoryDto 
            { 
                Name = "Updated Name",
                Description = "Updated description",
                Color = "#FF5733"
            };

            _mockRepository.Setup(r => r.GetByIdAsync(categoryId, userId))
                          .ReturnsAsync(existingCategory);
            _mockRepository.Setup(r => r.ExistsAsync(dto.Name, userId, categoryId))
                          .ReturnsAsync(false);
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Category>()))
                          .Returns(Task.CompletedTask);

            // Act
            var result = await _service.UpdateCategoryAsync(categoryId, userId, dto);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(dto.Name);
            result.Description.Should().Be(dto.Description);
            result.Color.Should().Be(dto.Color);

            _mockRepository.Verify(r => r.GetByIdAsync(categoryId, userId), Times.Once);
            _mockRepository.Verify(r => r.ExistsAsync(dto.Name, userId, categoryId), Times.Once);
            _mockRepository.Verify(r => r.UpdateAsync(existingCategory), Times.Once);
        }

        [Fact]
        public async Task UpdateCategoryAsync_ShouldThrowWhenCategoryNotFound()
        {
            // Arrange
            var categoryId = 1;
            var userId = 1;
            var dto = new CategoryDto { Name = "Updated Name" };

            _mockRepository.Setup(r => r.GetByIdAsync(categoryId, userId))
                          .ReturnsAsync((Category?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _service.UpdateCategoryAsync(categoryId, userId, dto));

            exception.Message.Should().Contain("not found");
            _mockRepository.Verify(r => r.GetByIdAsync(categoryId, userId), Times.Once);
            _mockRepository.Verify(r => r.ExistsAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Category>()), Times.Never);
        }

        [Fact]
        public async Task UpdateCategoryAsync_ShouldThrowWhenNameConflicts()
        {
            // Arrange
            var categoryId = 1;
            var userId = 1;
            var existingCategory = new Category 
            { 
                Id = categoryId,
                Name = "Old Name",
                UserId = userId
            };
            var dto = new CategoryDto { Name = "Conflicting Name" };

            _mockRepository.Setup(r => r.GetByIdAsync(categoryId, userId))
                          .ReturnsAsync(existingCategory);
            _mockRepository.Setup(r => r.ExistsAsync(dto.Name, userId, categoryId))
                          .ReturnsAsync(true);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.UpdateCategoryAsync(categoryId, userId, dto));

            exception.Message.Should().Contain("already exists");
            _mockRepository.Verify(r => r.GetByIdAsync(categoryId, userId), Times.Once);
            _mockRepository.Verify(r => r.ExistsAsync(dto.Name, userId, categoryId), Times.Once);
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Category>()), Times.Never);
        }

        [Fact]
        public async Task DeleteCategoryAsync_ShouldDeleteCategory()
        {
            // Arrange
            var categoryId = 1;
            var userId = 1;
            var category = new Category 
            { 
                Id = categoryId,
                Name = "To Delete",
                UserId = userId
            };

            _mockRepository.Setup(r => r.GetByIdAsync(categoryId, userId))
                          .ReturnsAsync(category);
            _mockRepository.Setup(r => r.DeleteAsync(category))
                          .Returns(Task.CompletedTask);

            // Act
            await _service.DeleteCategoryAsync(categoryId, userId);

            // Assert
            _mockRepository.Verify(r => r.GetByIdAsync(categoryId, userId), Times.Once);
            _mockRepository.Verify(r => r.DeleteAsync(category), Times.Once);
        }

        [Fact]
        public async Task DeleteCategoryAsync_ShouldThrowWhenCategoryNotFound()
        {
            // Arrange
            var categoryId = 1;
            var userId = 1;

            _mockRepository.Setup(r => r.GetByIdAsync(categoryId, userId))
                          .ReturnsAsync((Category?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _service.DeleteCategoryAsync(categoryId, userId));

            exception.Message.Should().Contain("not found");
            _mockRepository.Verify(r => r.GetByIdAsync(categoryId, userId), Times.Once);
            _mockRepository.Verify(r => r.DeleteAsync(It.IsAny<Category>()), Times.Never);
        }

        [Fact]
        public async Task CreateCategoryAsync_ShouldSetTimestamps()
        {
            // Arrange
            var userId = 1;
            var dto = new CategoryDto { Name = "New Category" };
            var createdCategory = new Category 
            { 
                Id = 1,
                Name = dto.Name,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _mockRepository.Setup(r => r.ExistsAsync(dto.Name, userId, null))
                          .ReturnsAsync(false);
            _mockRepository.Setup(r => r.AddAsync(It.IsAny<Category>()))
                          .ReturnsAsync(createdCategory);

            // Act
            var result = await _service.CreateCategoryAsync(userId, dto);

            // Assert
            _mockRepository.Verify(r => r.AddAsync(It.Is<Category>(c => 
                c.CreatedAt != default && 
                c.UpdatedAt != default)), Times.Once);
        }

        [Fact]
        public async Task CreateCategoryAsync_WithNullOptionalFields_ShouldSucceed()
        {
            // Arrange
            var userId = 1;
            var dto = new CategoryDto { Name = "Category with nulls" };
            var createdCategory = new Category 
            { 
                Id = 1,
                Name = dto.Name,
                Description = null,
                Color = null,
                UserId = userId
            };

            _mockRepository.Setup(r => r.ExistsAsync(dto.Name, userId, null))
                          .ReturnsAsync(false);
            _mockRepository.Setup(r => r.AddAsync(It.IsAny<Category>()))
                          .ReturnsAsync(createdCategory);

            // Act
            var result = await _service.CreateCategoryAsync(userId, dto);

            // Assert
            result.Should().NotBeNull();
            result.Description.Should().BeNull();
            result.Color.Should().BeNull();
        }
    }
}