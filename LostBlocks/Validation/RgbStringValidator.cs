using System.Text.RegularExpressions;
using FluentValidation;
using FluentValidation.Validators;

namespace LostBlocks.Validation;

internal partial class RgbStringValidator<T> : PropertyValidator<T, string?>
{
    public override string Name => "RgbStringValidator";

    [GeneratedRegex("^[0-9a-fA-F]{6}$", RegexOptions.Singleline)]
    private static partial Regex RgbRegex();

    public override bool IsValid(ValidationContext<T> context, string? value)
    {
        return value == null || RgbRegex().IsMatch(value);
    }

    protected override string GetDefaultMessageTemplate(string errorCode)
    {
        return "'{PropertyName}' is not a valid 6-byte hex string.";
    }
}
