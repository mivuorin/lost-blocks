using FluentValidation.TestHelper;
using LostBlocks.Api.Part;
using Xunit;

namespace LostBlocks.Test.Controllers.Part;

public class CreatePartDtoValidatorTest
{
    private readonly CreatePartDtoValidator validator = new();

    [Fact]
    public void Valid()
    {
        CreatePartDto dto = ValidDto();

        var result = validator.TestValidate(dto);

        result.ShouldNotHaveAnyValidationErrors();
    }

    private static CreatePartDto ValidDto()
    {
        return new CreatePartDto
        {
            PartNum = "PartNum",
            Name = "Name",
            CategoryId = 1
        };
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void PartNum_IsRequired(string? partNum)
    {
        CreatePartDto dto = ValidDto() with { PartNum = partNum! };

        var result = validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(d => d.PartNum);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Name_IsRequired(string? name)
    {
        CreatePartDto dto = ValidDto() with { Name = name! };

        var result = validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(d => d.Name);
    }
}
