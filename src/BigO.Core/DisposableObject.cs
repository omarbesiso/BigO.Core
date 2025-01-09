namespace BigO.Core;

/// <summary>
///     Defines a base class that implements <see cref="IDisposable" /> and <see cref="IAsyncDisposable" />,
///     providing thread-safe checks to ensure cleanup occurs only once.
/// </summary>
[PublicAPI]
public abstract class DisposableObject : IDisposable, IAsyncDisposable
{
    /// <summary>
    ///     Internal flag indicating disposal state.
    ///     0 = not disposed, 1 = disposed.
    /// </summary>
    private int _disposed;

    /// <summary>
    ///     Gets a value indicating whether this instance is disposed.
    /// </summary>
    /// <remarks>
    ///     Checks whether <see cref="_disposed" /> is nonzero.
    /// </remarks>
    public bool IsDisposed => _disposed != 0;

    /// <summary>
    ///     Asynchronously disposes this instance by calling <see cref="Dispose(bool)" /> with <c>true</c>,
    ///     and then suppresses finalization.
    /// </summary>
    /// <remarks>
    ///     If your derived class needs to perform additional asynchronous disposal work,
    ///     override this method or call a protected async method.
    /// </remarks>
    /// <returns>A <see cref="ValueTask" /> that represents the asynchronous dispose operation.</returns>
    public async ValueTask DisposeAsync()
    {
        Dispose(true);
        GC.SuppressFinalize(this);

        // Perform additional async cleanup (if needed) in a derived class or separate method.
        await ValueTask.CompletedTask;
    }

    /// <summary>
    ///     Disposes this instance by calling <see cref="Dispose(bool)" /> with <c>true</c>
    ///     and suppresses finalization for this object.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    ///     Releases the unmanaged resources used by this object, and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing">
    ///     <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.
    /// </param>
    protected virtual void Dispose(bool disposing)
    {
        // Use Interlocked.Exchange to ensure the disposing logic
        // is only executed once, even in multithreaded scenarios.
        if (Interlocked.Exchange(ref _disposed, 1) != 0)
        {
            // Already disposed; return immediately.
            return;
        }

        if (disposing)
        {
            // Dispose or free any managed objects here.
            DisposeManagedResources();
        }

        // Free unmanaged resources here (if any).
    }

    /// <summary>
    ///     Called by <see cref="Dispose(bool)" /> when disposing is <c>true</c>.
    ///     Override this in derived classes to release managed resources.
    /// </summary>
    protected virtual void DisposeManagedResources()
    {
        // Derived classes override this to release their managed resources.
    }

    /// <summary>
    ///     Throws an <see cref="ObjectDisposedException" /> if this object is disposed.
    ///     Call this method in derived class methods to guard against use-after-disposal.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the object has already been disposed.</exception>
    protected void ThrowIfDisposed()
    {
        if (IsDisposed)
        {
            throw new ObjectDisposedException(GetType().FullName);
        }
    }
}