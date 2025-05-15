using FluentValidation.TestHelper;
using LostBlocks.Api.Color;
using Xunit;

namespace LostBlocks.Test.Controllers.Color;

public class CreateColorDtoValidatorTest
{
    private readonly CreateColorDtoValidator validator = new();

    [Fact]
    public void Valid()
    {
        CreateColorDto dto = CreateValidDto();

        var result = validator.TestValidate(dto);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(null!)]
    [InlineData("")]
    public void Name_is_required(string? name)
    {
        CreateColorDto dto = CreateValidDto() with { Name = name! };

        var result = validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(e => e.Name);
    }

    [Theory]
    [InlineData(null!)]
    [InlineData("")]
    public void Rgb_is_required(string? rgb)
    {
        CreateColorDto dto = CreateValidDto() with { Rgb = rgb! };

        var result = validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(e => e.Rgb);
    }

    [Fact]
    public void Rgb_too_short()
    {
        CreateColorDto dto = CreateValidDto() with { Rgb = "12345" };

        var result = validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(e => e.Rgb);
    }

    [Fact]
    public void Rgb_too_long()
    {
        CreateColorDto dto = CreateValidDto() with { Rgb = "1234567" };

        var result = validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(e => e.Rgb);
    }

    [Theory]
    [InlineData("00000g")]
    [InlineData("00000G")]
    public void Rgb_invalid_characters(string rgb)
    {
        CreateColorDto dto = CreateValidDto() with { Rgb = rgb };

        var result = validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(e => e.Rgb);
    }

    [Theory]
    [InlineData("012345")]
    [InlineData("67891a")]
    [InlineData("bcdefA")]
    [InlineData("BCDEF0")]
    public void Rgb_valid_characters(string rgb)
    {
        CreateColorDto dto = CreateValidDto() with { Rgb = rgb };

        var result = validator.TestValidate(dto);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Rgb_should_use_rgb_validator()
    {
        CreateColorDto dto = CreateValidDto() with { Rgb = "invalid" };

        var result = validator.TestValidate(dto);

        result
            .ShouldHaveValidationErrorFor(e => e.Rgb)
            .WithErrorMessage("'Rgb' is not a valid 6-byte hex string.");
    }

    private static CreateColorDto CreateValidDto()
    {
        return new CreateColorDto
        {
            Name = "name",
            Rgb = "0F0F0F",
            IsTransparent = false
        };
    }
}
