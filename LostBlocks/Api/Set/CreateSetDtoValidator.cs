using FluentValidation;

namespace LostBlocks.Api.Set;

public class CreateSetDtoValidator : AbstractValidator<CreateSetDto>
{
    public CreateSetDtoValidator()
    {
        RuleFor(s => s.SetNum).NotEmpty();
        RuleFor(s => s.Name).NotEmpty();
    }
}
