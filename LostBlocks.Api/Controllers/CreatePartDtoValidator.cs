using FluentValidation;

namespace LostBlocks.Api.Controllers;

public class CreatePartDtoValidator : AbstractValidator<CreatePartDto>
{
    public CreatePartDtoValidator()
    {
        RuleFor(p => p.PartNum).NotEmpty();
        RuleFor(p => p.Name).NotEmpty();
    }
}
