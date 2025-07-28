using FluentValidation.TestHelper;
using LostBlocks.Api.Set;
using Xunit;

namespace LostBlocks.Test.Api.Set;

public class UpdateSetDtoValidatorTest
{
    private readonly UpdateSetDtoValidator validator = new();

    [Fact]
    public void Valid()
    {
        UpdateSetDto dto = ValidDto();

        var result = validator.TestValidate(dto);

        result.ShouldNotHaveAnyValidationErrors();
    }

    private static UpdateSetDto ValidDto()
    {
        return new UpdateSetDto
        {
            Name = "Name",
            ThemeId = 1,
            Year = 2025
        };
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Name_IsRequired(string? name)
    {
        UpdateSetDto dto = ValidDto() with { Name = name! };

        var result = validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(d => d.Name);
    }
}
