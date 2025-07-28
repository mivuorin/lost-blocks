using FluentValidation;

namespace LostBlocks.Api.Set;

public class UpdateSetDtoValidator : AbstractValidator<UpdateSetDto>
{
    public UpdateSetDtoValidator()
    {
        RuleFor(s => s.Name).NotEmpty();
    }
}
