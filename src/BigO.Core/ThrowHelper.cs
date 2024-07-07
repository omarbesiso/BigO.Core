using System.Diagnostics.CodeAnalysis;

namespace BigO.Core;

internal static class ThrowHelper
{
    [DoesNotReturn]
    [ContractAnnotation("=> halt")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowArgumentNullException(string argumentName, string? exceptionMessage = null)
    {
        var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
            ? $"The value of '{argumentName}' cannot be null."
            : exceptionMessage;

        throw new ArgumentNullException(argumentName, errorMessage);
    }

    [DoesNotReturn]
    [ContractAnnotation("=> halt")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowArgumentException(string argumentName, string? exceptionMessage = null)
    {
        var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
            ? $"The value of '{argumentName}' is invalid."
            : exceptionMessage;

        throw new ArgumentException(errorMessage, argumentName);
    }

    [DoesNotReturn]
    [ContractAnnotation("=> halt")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowArgumentOutOfRangeException(string argumentName, string? exceptionMessage = null)
    {
        var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
            ? $"The value of '{argumentName}' is invalid."
            : exceptionMessage;

        throw new ArgumentOutOfRangeException(errorMessage, argumentName);
    }
}