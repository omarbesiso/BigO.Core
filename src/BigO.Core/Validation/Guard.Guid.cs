﻿namespace BigO.Core.Validation;

public static partial class Guard
{
    /// <summary>
    ///     Ensures that the given Guid is not empty.
    ///     If the Guid is empty, an <see cref="ArgumentException" /> is thrown.
    /// </summary>
    /// <param name="value">The Guid to be checked.</param>
    /// <param name="argumentName">
    ///     The name of the argument being checked, used in the exception message for clarity.
    /// </param>
    /// <param name="exceptionMessage">
    ///     Custom exception message if the Guid is empty.
    ///     If not provided, a default message is used.
    /// </param>
    /// <returns>The non-empty Guid.</returns>
    /// <exception cref="ArgumentException">Thrown if the Guid is empty.</exception>
    /// <remarks>
    ///     This method is useful for validating Guid arguments to ensure they are not empty.
    /// </remarks>
    /// <example>
    ///     <code>
    ///         Guid myGuid = Guid.NewGuid();
    ///         Guard.NotEmpty(myGuid, nameof(myGuid));
    ///     </code>
    /// </example>
    public static Guid NotEmpty(Guid value,
        [CallerArgumentExpression(nameof(value))]
        string argumentName = "",
        string? exceptionMessage = null)
    {
        if (value != Guid.Empty)
        {
            return value;
        }

        var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
            ? $"The Guid '{argumentName}' cannot be empty."
            : exceptionMessage;

        throw new ArgumentException(errorMessage, argumentName);
    }
}