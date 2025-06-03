using FluentValidation.TestHelper;
using LostBlocks.Api.Theme;
using Xunit;

namespace LostBlocks.Test.Api.Theme;

public class CreateThemeValidatorTest
{
    private readonly CreateThemeDtoValidator validator = new();

    [Fact]
    public void Valid()
    {
        CreateThemeDto dto = ValidDto();

        var result = validator.TestValidate(dto);

        result.ShouldNotHaveAnyValidationErrors();
    }

    private static CreateThemeDto ValidDto()
    {
        return new CreateThemeDto
        {
            Name = "Name",
            ParentId = null
        };
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Name_is_required(string? name)
    {
        CreateThemeDto dto = ValidDto() with { Name = name! };

        var result = validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(d => d.Name);
    }
}
