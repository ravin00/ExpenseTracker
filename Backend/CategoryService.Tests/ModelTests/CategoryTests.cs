using CategoryService.Models;
using FluentAssertions;
using Xunit;

namespace CategoryService.Tests.ModelTests
{
    public class CategoryTests
    {
        [Fact]
        public void Category_DefaultValues_ShouldBeSetCorrectly()
        {
            // Arrange & Act
            var category = new Category { Name = "Test Category" };

            // Assert
            category.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
            category.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [Theory]
        [InlineData("#FF5733", true)]
        [InlineData("#000000", true)]
        [InlineData("#FFFFFF", true)]
        [InlineData("#123ABC", true)]
        [InlineData("FF5733", false)]   // Missing #
        [InlineData("#GG5733", false)] // Invalid character
        [InlineData("#FF573", false)]  // Too short
        [InlineData("#FF57333", false)] // Too long
        [InlineData("", true)]          // Empty is valid (nullable)
        [InlineData(null, true)]        // Null is valid (nullable)
        public void IsValidColorFormat_ShouldReturnCorrectResult(string? color, bool expected)
        {
            // Arrange
            var category = new Category { Name = "Test", Color = color };

            // Act
            var result = category.IsValidColorFormat();

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("Valid Name", true)]
        [InlineData("A", true)]
        [InlineData("", false)]
        [InlineData(" ", false)]
        [InlineData(null, false)]
        public void IsValidName_ShouldReturnCorrectResult(string? name, bool expected)
        {
            // Arrange
            var category = new Category { Name = name! };

            // Act
            var result = category.IsValidName();

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(null, true)]
        [InlineData("", true)]
        [InlineData("Valid description", true)]
        [InlineData("A", true)]
        public void IsValidDescription_ShouldReturnTrueForValidDescriptions(string? description, bool expected)
        {
            // Arrange
            var category = new Category { Name = "Test", Description = description };

            // Act
            var result = category.IsValidDescription();

            // Assert
            result.Should().Be(expected);
        }

        [Fact]
        public void IsValidDescription_ShouldReturnFalseForTooLongDescription()
        {
            // Arrange
            var longDescription = new string('A', 501);
            var category = new Category { Name = "Test", Description = longDescription };

            // Act
            var result = category.IsValidDescription();

            // Assert
            result.Should().BeFalse();
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(100, true)]
        [InlineData(0, false)]
        [InlineData(-1, false)]
        public void IsValidUserId_ShouldReturnCorrectResult(int userId, bool expected)
        {
            // Arrange
            var category = new Category { Name = "Test", UserId = userId };

            // Act
            var result = category.IsValidUserId();

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("  Test Category  ", "Test Category")]
        [InlineData("Normal Name", "Normal Name")]
        [InlineData("", "")]
        public void DisplayName_ShouldReturnTrimmedName(string name, string expected)
        {
            // Arrange
            var category = new Category { Name = name };

            // Act
            var result = category.DisplayName;

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("#FF5733", "#FF5733")]
        [InlineData("#000000", "#000000")]
        [InlineData("invalid", "#6B7280")]
        [InlineData(null, "#6B7280")]
        [InlineData("", "#6B7280")]
        public void SafeColor_ShouldReturnValidColorOrDefault(string? color, string expected)
        {
            // Arrange
            var category = new Category { Name = "Test", Color = color };

            // Act
            var result = category.SafeColor;

            // Assert
            result.Should().Be(expected);
        }

        [Fact]
        public void DaysOld_ShouldCalculateCorrectly()
        {
            // Arrange
            var createdDate = DateTime.UtcNow.AddDays(-5);
            var category = new Category { Name = "Test", CreatedAt = createdDate };

            // Act
            var result = category.DaysOld;

            // Assert
            result.Should().Be(5);
        }

        [Theory]
        [InlineData(-1, true)]
        [InlineData(-7, true)]
        [InlineData(-8, false)]
        [InlineData(-30, false)]
        public void IsRecentlyCreated_ShouldReturnCorrectResult(int daysOffset, bool expected)
        {
            // Arrange
            var category = new Category 
            { 
                Name = "Test", 
                CreatedAt = DateTime.UtcNow.AddDays(daysOffset) 
            };

            // Act
            var result = category.IsRecentlyCreated;

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(-1, true)]
        [InlineData(-23, true)]
        [InlineData(-25, false)]
        [InlineData(-48, false)]
        public void IsRecentlyUpdated_ShouldReturnCorrectResult(int hoursOffset, bool expected)
        {
            // Arrange
            var category = new Category 
            { 
                Name = "Test", 
                UpdatedAt = DateTime.UtcNow.AddHours(hoursOffset) 
            };

            // Act
            var result = category.IsRecentlyUpdated;

            // Assert
            result.Should().Be(expected);
        }
    }
}