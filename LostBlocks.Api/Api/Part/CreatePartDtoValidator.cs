using FluentValidation;

namespace LostBlocks.Api.Api.Part;

public class CreatePartDtoValidator : AbstractValidator<CreatePartDto>
{
    public CreatePartDtoValidator()
    {
        RuleFor(p => p.PartNum).NotEmpty();
        RuleFor(p => p.Name).NotEmpty();
    }
}
