using FluentValidation;

namespace LostBlocks.Validation;

internal static class ValidatorExtensions
{
    public static IRuleBuilderOptions<T, string?> Rgb<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder.SetValidator(new RgbStringValidator<T>());
    }
}
