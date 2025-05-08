using FluentValidation;
using LostBlocks.Api.Validation;

namespace LostBlocks.Api.Controllers;

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
