using FluentValidation;
using LostBlocks.Validation;

namespace LostBlocks.Api.Color;

public class UpdateColorDtoValidator : AbstractValidator<UpdateColorDto>
{
    public UpdateColorDtoValidator()
    {
        RuleFor(dto => dto.Name).NotEmpty();
        RuleFor(dto => dto.Rgb)
            .NotNull()
            .Rgb();
    }
}
