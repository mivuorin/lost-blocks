using FluentValidation;

namespace LostBlocks.Api.Controllers;

public class CreateCategoryDtoValidator : AbstractValidator<CreateCategoryDto>
{
    public CreateCategoryDtoValidator()
    {
        RuleFor(c => c.Name).NotEmpty();
        // TODO Validate category name length is less than 255 characters
    }
}
