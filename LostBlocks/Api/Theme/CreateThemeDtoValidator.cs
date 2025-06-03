using FluentValidation;

namespace LostBlocks.Api.Theme;

public class CreateThemeDtoValidator : AbstractValidator<CreateThemeDto>
{
    public CreateThemeDtoValidator()
    {
        RuleFor(d => d.Name).NotEmpty();
    }
}
