using FluentValidation;

namespace LostBlocks.Api.Controllers;

public class UpdateCategoryDtoValidator : AbstractValidator<UpdateCategoryDto>
{
    public UpdateCategoryDtoValidator()
    {
        RuleFor(c => c.Name).NotEmpty();
    }
}
