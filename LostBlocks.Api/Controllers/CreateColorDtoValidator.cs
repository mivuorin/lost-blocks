using FluentValidation;

namespace LostBlocks.Api.Controllers;

public class CreateColorDtoValidator : AbstractValidator<CreateColorDto>
{
    public CreateColorDtoValidator()
    {
        RuleFor(dto => dto.Name).NotEmpty();

        RuleFor(dto => dto.Rgb)
            .NotEmpty()
            .Length(6)
            .Matches("[0-9a-fA-F]{6}");
    }
}
