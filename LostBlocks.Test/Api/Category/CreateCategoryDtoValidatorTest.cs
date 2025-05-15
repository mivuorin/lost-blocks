using FluentValidation.TestHelper;
using LostBlocks.Api.Category;
using Xunit;

namespace LostBlocks.Test.Controllers.Category;

public class CreateCategoryDtoValidatorTest
{
    private readonly CreateCategoryDtoValidator validator = new();

    [Fact]
    public void Valid()
    {
        var dto = new CreateCategoryDto
        {
            Name = "name"
        };

        var result = validator.TestValidate(dto);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Name_IsRequired(string? name)
    {
        var dto = new CreateCategoryDto { Name = name! };

        var result = validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(d => d.Name);
    }
}
