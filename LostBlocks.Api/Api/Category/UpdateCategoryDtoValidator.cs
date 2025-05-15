using FluentValidation;

namespace LostBlocks.Api.Api.Category;

public class UpdateCategoryDtoValidator : AbstractValidator<UpdateCategoryDto>
{
    public UpdateCategoryDtoValidator()
    {
        RuleFor(c => c.Name).NotEmpty();
    }
}
