using CategoryService.Dtos;
using FluentAssertions;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace CategoryService.Tests.DtoTests
{
    public class CategoryDtoTests
    {
        private List<ValidationResult> ValidateDto(object dto)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(dto);
            Validator.TryValidateObject(dto, validationContext, validationResults, true);
            return validationResults;
        }

        [Fact]
        public void CategoryDto_WithValidData_ShouldPassValidation()
        {
            // Arrange
            var dto = new CategoryDto
            {
                Name = "Valid Category",
                Description = "This is a valid description",
                Color = "#FF5733"
            };

            // Act
            var validationResults = ValidateDto(dto);

            // Assert
            validationResults.Should().BeEmpty();
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void CategoryDto_WithInvalidName_ShouldFailValidation(string? name)
        {
            // Arrange
            var dto = new CategoryDto { Name = name! };

            // Act
            var validationResults = ValidateDto(dto);

            // Assert
            validationResults.Should().NotBeEmpty();
            validationResults.Should().Contain(vr => vr.ErrorMessage!.Contains("name"));
        }

        [Fact]
        public void CategoryDto_WithTooLongName_ShouldFailValidation()
        {
            // Arrange
            var longName = new string('A', 101);
            var dto = new CategoryDto { Name = longName };

            // Act
            var validationResults = ValidateDto(dto);

            // Assert
            validationResults.Should().NotBeEmpty();
            validationResults.Should().Contain(vr => vr.ErrorMessage!.Contains("100 characters"));
        }

        [Fact]
        public void CategoryDto_WithTooLongDescription_ShouldFailValidation()
        {
            // Arrange
            var longDescription = new string('A', 501);
            var dto = new CategoryDto 
            { 
                Name = "Valid Name",
                Description = longDescription 
            };

            // Act
            var validationResults = ValidateDto(dto);

            // Assert
            validationResults.Should().NotBeEmpty();
            validationResults.Should().Contain(vr => vr.ErrorMessage!.Contains("500 characters"));
        }

        [Theory]
        [InlineData("#FF5733")]
        [InlineData("#000000")]
        [InlineData("#FFFFFF")]
        [InlineData("#123ABC")]
        [InlineData("#abcdef")]
        [InlineData(null)]
        [InlineData("")]
        public void CategoryDto_WithValidColor_ShouldPassValidation(string? color)
        {
            // Arrange
            var dto = new CategoryDto 
            { 
                Name = "Valid Name",
                Color = color 
            };

            // Act
            var validationResults = ValidateDto(dto);

            // Assert
            validationResults.Should().BeEmpty();
        }

        [Theory]
        [InlineData("FF5733")]      // Missing #
        [InlineData("#GG5733")]     // Invalid character
        [InlineData("#FF573")]      // Too short
        [InlineData("#FF57333")]    // Too long
        [InlineData("invalid")]     // Invalid format
        [InlineData("#")]           // Only #
        [InlineData(" #FF5733")]    // Leading space
        [InlineData("#FF5733 ")]    // Trailing space
        public void CategoryDto_WithInvalidColor_ShouldFailValidation(string color)
        {
            // Arrange
            var dto = new CategoryDto 
            { 
                Name = "Valid Name",
                Color = color 
            };

            // Act
            var validationResults = ValidateDto(dto);

            // Assert
            validationResults.Should().NotBeEmpty();
            validationResults.Should().Contain(vr => vr.ErrorMessage!.Contains("hex color"));
        }

        [Fact]
        public void CategoryDto_WithNullDescription_ShouldPassValidation()
        {
            // Arrange
            var dto = new CategoryDto 
            { 
                Name = "Valid Name",
                Description = null 
            };

            // Act
            var validationResults = ValidateDto(dto);

            // Assert
            validationResults.Should().BeEmpty();
        }

        [Fact]
        public void CategoryDto_WithEmptyDescription_ShouldPassValidation()
        {
            // Arrange
            var dto = new CategoryDto 
            { 
                Name = "Valid Name",
                Description = "" 
            };

            // Act
            var validationResults = ValidateDto(dto);

            // Assert
            validationResults.Should().BeEmpty();
        }

        [Fact]
        public void CategoryDto_WithMaxLengthDescription_ShouldPassValidation()
        {
            // Arrange
            var maxDescription = new string('A', 500);
            var dto = new CategoryDto 
            { 
                Name = "Valid Name",
                Description = maxDescription 
            };

            // Act
            var validationResults = ValidateDto(dto);

            // Assert
            validationResults.Should().BeEmpty();
        }

        [Fact]
        public void CategoryDto_WithMaxLengthName_ShouldPassValidation()
        {
            // Arrange
            var maxName = new string('A', 100);
            var dto = new CategoryDto { Name = maxName };

            // Act
            var validationResults = ValidateDto(dto);

            // Assert
            validationResults.Should().BeEmpty();
        }

        [Fact]
        public void CategoryDto_WithMinLengthName_ShouldPassValidation()
        {
            // Arrange
            var dto = new CategoryDto { Name = "A" };

            // Act
            var validationResults = ValidateDto(dto);

            // Assert
            validationResults.Should().BeEmpty();
        }
    }
}