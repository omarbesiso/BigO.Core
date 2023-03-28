using System.Transactions;
using JetBrains.Annotations;

namespace BigO.Core;

/// <summary>
///     Factory for creating <see cref="TransactionScope" /> objects.
/// </summary>
[PublicAPI]
public static class TransactionFactory
{
    /// <summary>
    ///     Creates a new <see cref="TransactionScope" /> instance with the specified isolation level, timeout, and
    ///     asynchronous flow options.
    /// </summary>
    /// <param name="isolationLevel">
    ///     The isolation level for the transaction. Default is
    ///     <see cref="IsolationLevel.ReadCommitted" />.
    /// </param>
    /// <param name="transactionScopeOption">
    ///     The transaction scope option. Default is
    ///     <see cref="TransactionScopeOption.Required" />.
    /// </param>
    /// <param name="transactionScopeAsyncFlowOption">
    ///     The transaction scope asynchronous flow option. Default is
    ///     <see cref="TransactionScopeAsyncFlowOption.Enabled" />.
    /// </param>
    /// <param name="timeOut">
    ///     The timeout for the transaction. Default is <see cref="TransactionManager.MaximumTimeout" /> if
    ///     null.
    /// </param>
    /// <returns>A new <see cref="TransactionScope" /> instance with the specified options.</returns>
    /// <remarks>
    ///     This extension method creates a new <see cref="TransactionScope" /> instance with the specified options. The method
    ///     returns a new <see cref="TransactionScope" /> instance
    ///     with the specified <paramref name="isolationLevel" />, <paramref name="transactionScopeOption" />,
    ///     <paramref name="transactionScopeAsyncFlowOption" />, and <paramref name="timeOut" /> options.
    /// </remarks>
    /// <exception cref="System.ArgumentOutOfRangeException">
    ///     Thrown when <paramref name="isolationLevel" /> is not a valid
    ///     <see cref="IsolationLevel" /> value.
    /// </exception>
    /// <example>
    ///     The following code demonstrates how to use the <see cref="CreateTransaction" /> method to create a new
    ///     <see cref="TransactionScope" /> instance.
    ///     <code><![CDATA[
    /// using(var transaction = CreateTransaction())
    /// {
    ///     // Perform transactional work here
    ///     transaction.Complete();
    /// }
    /// ]]></code>
    /// </example>
    public static TransactionScope CreateTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
        TransactionScopeOption transactionScopeOption = TransactionScopeOption.Required,
        TransactionScopeAsyncFlowOption transactionScopeAsyncFlowOption = TransactionScopeAsyncFlowOption.Enabled,
        TimeSpan? timeOut = null)
    {
        var transactionOptions = new TransactionOptions { IsolationLevel = isolationLevel };
        if (!timeOut.HasValue)
        {
            transactionOptions.Timeout = TransactionManager.MaximumTimeout;
        }

        return new TransactionScope(transactionScopeOption, transactionOptions, transactionScopeAsyncFlowOption);
    }
}