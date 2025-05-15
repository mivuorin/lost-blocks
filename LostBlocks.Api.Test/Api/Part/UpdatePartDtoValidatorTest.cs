using FluentValidation.TestHelper;
using LostBlocks.Api.Api.Part;
using Xunit;

namespace LostBlocks.Api.Test.Controllers.Part;

public class UpdatePartDtoValidatorTest
{
    private readonly UpdatePartDtoValidator validator = new();

    [Fact]
    public void Valid()
    {
        UpdatePartDto dto = ValidDto();

        var result = validator.TestValidate(dto);

        result.ShouldNotHaveAnyValidationErrors();
    }

    private static UpdatePartDto ValidDto()
    {
        return new UpdatePartDto
        {
            Name = "Name",
            CategoryId = 1
        };
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Name_IsRequired(string? name)
    {
        UpdatePartDto dto = ValidDto() with { Name = name! };

        var result = validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(d => d.Name);
    }
}
