using FluentValidation.TestHelper;
using LostBlocks.Api.Theme;
using Xunit;

namespace LostBlocks.Test.Api.Theme;

public class UpdateThemeValidatorTest
{
    private readonly UpdateThemeValidator validator = new();

    [Fact]
    public void Valid()
    {
        UpdateThemeDto dto = ValidDto();

        var result = validator.TestValidate(dto);

        result.ShouldNotHaveAnyValidationErrors();
    }

    private static UpdateThemeDto ValidDto()
    {
        return new UpdateThemeDto
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
        UpdateThemeDto dto = ValidDto() with { Name = name! };

        var result = validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(d => d.Name);
    }
}
