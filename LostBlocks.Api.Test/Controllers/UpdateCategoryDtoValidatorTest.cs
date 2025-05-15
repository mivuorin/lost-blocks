using FluentValidation.TestHelper;
using LostBlocks.Api.Controllers;
using Xunit;

namespace LostBlocks.Api.Test.Controllers;

public class UpdateCategoryDtoValidatorTest
{
    private readonly UpdateCategoryDtoValidator validator = new();

    [Fact]
    public void Valid()
    {
        var dto = new UpdateCategoryDto()
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
        var dto = new UpdateCategoryDto() { Name = name! };

        var result = validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(d => d.Name);
    }
}
