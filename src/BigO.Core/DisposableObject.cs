namespace BigO.Core;

/// <summary>
///     Defines an object base with necessary disposable implementation.
/// </summary>
[PublicAPI]
public abstract class DisposableObject : IDisposable
{
    private readonly object _disposeLock = new(); // lock object for thread safety
    private bool _isDisposed; // backing field for the property

    /// <summary>
    ///     Gets a value indicating whether this instance is disposed.
    /// </summary>
    /// <value><c>true</c> if this instance is disposed; otherwise, <c>false</c>.</value>
    public bool IsDisposed
    {
        get
        {
            lock (_disposeLock)
            {
                return _isDisposed;
            }
        }
        private set
        {
            lock (_disposeLock)
            {
                _isDisposed = value;
            }
        }
    }

    /// <summary>
    ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    ///     Releases unmanaged and - optionally - managed resources.
    /// </summary>
    /// <param name="disposing">
    ///     <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only
    ///     unmanaged resources.
    /// </param>
    protected virtual void Dispose(bool disposing)
    {
        if (IsDisposed)
        {
            return;
        }

        if (disposing)
        {
            // Free managed resources
            Disposing();
        }

        // Free unmanaged resources (if any) here

        IsDisposed = true;
    }

    /// <summary>
    ///     Overridden in implementing objects to perform actual clean-up.
    /// </summary>
    protected virtual void Disposing()
    {
    }
}