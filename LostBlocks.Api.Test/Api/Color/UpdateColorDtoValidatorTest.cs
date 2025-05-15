using FluentValidation.TestHelper;
using LostBlocks.Api.Api.Color;
using Xunit;

namespace LostBlocks.Api.Test.Controllers.Color;

public class UpdateColorDtoValidatorTest
{
    private readonly UpdateColorDtoValidator validator = new();

    [Fact]
    public void Valid()
    {
        UpdateColorDto dto = CreateValidDto();

        var result = validator.TestValidate(dto);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Name_is_required(string? name)
    {
        UpdateColorDto dto = CreateValidDto() with { Name = name! };

        var result = validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(e => e.Name);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Rgb_is_required(string? rgb)
    {
        UpdateColorDto dto = CreateValidDto() with { Rgb = rgb! };

        var result = validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(e => e.Rgb);
    }

    [Fact]
    public void Rgb_should_use_rgb_validator()
    {
        UpdateColorDto dto = CreateValidDto() with { Rgb = "invalid" };

        var result = validator.TestValidate(dto);

        result
            .ShouldHaveValidationErrorFor(e => e.Rgb)
            .WithErrorMessage("'Rgb' is not a valid 6-byte hex string.");
    }

    private static UpdateColorDto CreateValidDto()
    {
        return new UpdateColorDto
        {
            Name = "name",
            Rgb = "010101",
            IsTransparent = false
        };
    }
}
