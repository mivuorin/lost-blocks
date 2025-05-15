using FluentValidation;
using LostBlocks.Api.Validation;

namespace LostBlocks.Api.Api.Color;

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
