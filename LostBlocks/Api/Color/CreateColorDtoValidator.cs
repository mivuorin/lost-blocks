using FluentValidation;
using LostBlocks.Validation;

namespace LostBlocks.Api.Color;

public class CreateColorDtoValidator : AbstractValidator<CreateColorDto>
{
    public CreateColorDtoValidator()
    {
        RuleFor(dto => dto.Name).NotEmpty();
        RuleFor(dto => dto.Rgb)
            .NotNull()
            .Rgb();
    }
}
