using FluentValidation.TestHelper;
using LostBlocks.Api.Set;
using Xunit;

namespace LostBlocks.Test.Api.Set;

public class CreateSetDtoValidatorTest
{
    private readonly CreateSetDtoValidator validator = new();

    [Fact]
    public void Valid()
    {
        CreateSetDto dto = ValidDto();

        var result = validator.TestValidate(dto);

        result.ShouldNotHaveAnyValidationErrors();
    }

    private static CreateSetDto ValidDto()
    {
        return new CreateSetDto
        {
            SetNum = "SetNum",
            Name = "Name",
            ThemeId = 1,
            Year = 2025
        };
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void SetNum_IsRequired(string? setNum)
    {
        CreateSetDto dto = ValidDto() with { SetNum = setNum! };

        var result = validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(d => d.SetNum);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Name_IsRequired(string? name)
    {
        CreateSetDto dto = ValidDto() with { Name = name! };

        var result = validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(d => d.Name);
    }
}
