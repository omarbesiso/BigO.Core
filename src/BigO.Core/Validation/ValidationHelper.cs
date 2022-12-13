using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace BigO.Core.Validation;

[DebuggerStepThrough]
internal static class ValidationHelper
{
    private const string MaxLengthErrorMessage = "The maximum length specified cannot be less than or equal to 0.";
    private const string MinLengthErrorMessage = "The minimum length specified cannot be less than or equal to 0.";

    private const string MinMaxLengthErrorMessage =
        "The minimum length specified cannot be greater than the maximum length specified.";

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ValidateMaxLength(string value, int maxLength, string argumentName = "",
        string? exceptionMessage = null)
    {
        if (maxLength <= 0)
        {
            ThrowHelper.ThrowArgumentException(nameof(maxLength), MaxLengthErrorMessage);
        }

        if (value.Length > maxLength)
        {
            var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
                ? $"The length of string '{argumentName}' cannot exceed {maxLength} characters."
                : exceptionMessage;

            ThrowHelper.ThrowArgumentException(argumentName, errorMessage);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ValidateMinLength(string value, int minLength, string argumentName = "",
        string? exceptionMessage = null)
    {
        if (minLength <= 0)
        {
            ThrowHelper.ThrowArgumentException(nameof(minLength), MinLengthErrorMessage);
        }

        if (value.Length < minLength)
        {
            var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
                ? $"The length of string '{argumentName}' cannot be less than {minLength} characters."
                : exceptionMessage;

            ThrowHelper.ThrowArgumentException(argumentName, errorMessage);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ValidateStrengthLength(string value, int minLength, int maxLength, string argumentName = "",
        string? exceptionMessage = null)
    {
        if (maxLength <= 0)
        {
            ThrowHelper.ThrowArgumentException(nameof(maxLength), MaxLengthErrorMessage);
        }

        if (minLength <= 0)
        {
            ThrowHelper.ThrowArgumentException(nameof(minLength), MinLengthErrorMessage);
        }

        if (maxLength < minLength)
        {
            ThrowHelper.ThrowArgumentException(nameof(minLength), MinMaxLengthErrorMessage);
        }

        if (value.Length > maxLength || value.Length < minLength)
        {
            var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
                ? $"The length of string '{argumentName}' cannot exceed {maxLength} characters and cannot be less than {minLength} characters."
                : exceptionMessage;

            ThrowHelper.ThrowArgumentException(argumentName, errorMessage);
        }
    }
}