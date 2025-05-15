using FluentValidation;

namespace LostBlocks.Api.Controllers;

public class UpdatePartDtoValidator : AbstractValidator<UpdatePartDto>
{
    public UpdatePartDtoValidator()
    {
        RuleFor(p => p.Name).NotEmpty();
    }
}
