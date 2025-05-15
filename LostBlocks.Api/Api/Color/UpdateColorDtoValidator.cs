using FluentValidation;
using LostBlocks.Api.Validation;

namespace LostBlocks.Api.Api.Color;

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
