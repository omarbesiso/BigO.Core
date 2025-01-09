using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace BigO.Core;

[DebuggerStepThrough]
internal static class ThrowHelper
{
    [DoesNotReturn]
    [ContractAnnotation("=> halt")]
    public static void ThrowArgumentNullException(string argumentName, string? exceptionMessage = null)
    {
        var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
            ? $"The value of '{argumentName}' cannot be null."
            : exceptionMessage;

        throw new ArgumentNullException(argumentName, errorMessage);
    }

    [DoesNotReturn]
    [ContractAnnotation("=> halt")]
    public static void ThrowArgumentException(string argumentName, string? exceptionMessage = null)
    {
        var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
            ? $"The value of '{argumentName}' is invalid."
            : exceptionMessage;

        throw new ArgumentException(errorMessage, argumentName);
    }

    [DoesNotReturn]
    [ContractAnnotation("=> halt")]
    public static void ThrowArgumentOutOfRangeException(string argumentName, string? exceptionMessage = null)
    {
        var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
            ? $"The value of '{argumentName}' is invalid."
            : exceptionMessage;

        throw new ArgumentOutOfRangeException(argumentName, errorMessage);
    }
}