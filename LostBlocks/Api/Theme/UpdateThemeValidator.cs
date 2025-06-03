using FluentValidation;

namespace LostBlocks.Api.Theme;

public class UpdateThemeValidator : AbstractValidator<UpdateThemeDto>
{
    public UpdateThemeValidator()
    {
        RuleFor(d => d.Name).NotEmpty();
    }
}
