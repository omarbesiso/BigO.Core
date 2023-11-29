namespace BigO.Core.Extensions;

/// <summary>
///     Provides a set of useful extension methods for working with <see cref="IDictionary{TKey,TValue}" /> objects.
/// </summary>
[PublicAPI]
public static class DictionaryExtensions
{
    /// <summary>
    ///     Removes the specified key and its associated value from the dictionary, if the key exists.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    /// <param name="dictionary">The dictionary to remove the key and value from.</param>
    /// <param name="key">The key to remove from the dictionary.</param>
    /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="dictionary" /> is <c>null</c>.</exception>
    /// <example>
    ///     <code><![CDATA[
    ///     IDictionary<string, int> sampleDictionary = new Dictionary<string, int>
    ///     {
    ///         { "One", 1 },
    ///         { "Two", 2 },
    ///         { "Three", 3 }
    ///     };
    ///     sampleDictionary.RemoveIfContainsKey("Two");
    ///     -- The dictionary now contains only "One" and "Three" keys.
    ///     ]]></code>
    /// </example>
    /// <remarks>
    ///     This method is an extension method and can be used directly on any object implementing
    ///     <see cref="System.Collections.Generic.IDictionary{TKey, TValue}" />. It checks if the specified key exists in the
    ///     dictionary, and if so, it removes the key and its associated value. If the key does not exist in the dictionary, no
    ///     action is taken.
    /// </remarks>
    public static void RemoveIfContainsKey<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
    {
        if (dictionary == null)
        {
            throw new ArgumentNullException(nameof(dictionary), $"The {nameof(dictionary)} cannot be null.");
        }

        if (dictionary.ContainsKey(key))
        {
            dictionary.Remove(key);
        }
    }

    /// <summary>
    ///     Creates a new <see cref="SortedDictionary{TKey, TValue}" /> from the elements of the
    ///     <see cref="IDictionary{TKey, TValue}" />.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary. TKey must be a non-nullable value type.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    /// <param name="dictionary">
    ///     The <see cref="IDictionary{TKey, TValue}" /> to create the
    ///     <see cref="SortedDictionary{TKey, TValue}" /> from.
    /// </param>
    /// <returns>
    ///     A new <see cref="SortedDictionary{TKey, TValue}" /> containing the same key-value pairs as the input
    ///     dictionary.
    /// </returns>
    /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="dictionary" /> is <c>null</c>.</exception>
    /// <example>
    ///     <code><![CDATA[
    /// IDictionary<string, int> sampleDictionary = new Dictionary<string, int>
    /// {
    ///     { "One", 1 },
    ///     { "Two", 2 },
    ///     { "Three", 3 }
    /// };
    /// 
    /// SortedDictionary<string, int> sortedDictionary = sampleDictionary.ToSortedDictionary();
    /// 
    /// // The sortedDictionary now contains the same key-value pairs as sampleDictionary, sorted by key.
    /// ]]></code>
    /// </example>
    /// <remarks>
    ///     This method is an extension method and can be used directly on any object implementing
    ///     <see cref="System.Collections.Generic.IDictionary{TKey, TValue}" />. It creates a new
    ///     <see cref="SortedDictionary{TKey, TValue}" /> containing the same key-value pairs as
    ///     the input dictionary, sorted by key. The original dictionary remains unchanged.
    /// </remarks>
    public static SortedDictionary<TKey, TValue> ToSortedDictionary<TKey, TValue>(
        this IDictionary<TKey, TValue> dictionary) where TKey : notnull
    {
        if (dictionary == null)
        {
            throw new ArgumentNullException(nameof(dictionary), $"The {nameof(dictionary)} cannot be null.");
        }

        return new SortedDictionary<TKey, TValue>(dictionary);
    }

    /// <summary>
    ///     Creates a new <see cref="SortedDictionary{TKey, TValue}" /> from the specified dictionary using the specified key
    ///     comparer.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    /// <param name="dictionary">The dictionary to create a sorted dictionary from.</param>
    /// <param name="comparer">The key comparer used to sort the keys in the resulting sorted dictionary.</param>
    /// <returns>
    ///     A new <see cref="SortedDictionary{TKey, TValue}" /> containing the same key-value pairs as the input
    ///     dictionary, sorted by the provided comparer.
    /// </returns>
    /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="dictionary" /> is <c>null</c>.</exception>
    /// <example>
    ///     <code><![CDATA[
    /// IDictionary<string, int> sampleDictionary = new Dictionary<string, int>
    /// {
    ///     { "One", 1 },
    ///     { "Two", 2 },
    ///     { "Three", 3 }
    /// };
    /// 
    /// IComparer<string> keyComparer = StringComparer.OrdinalIgnoreCase;
    /// SortedDictionary<string, int> sortedDictionary = sampleDictionary.ToSortedDictionary(keyComparer);
    /// 
    /// // The sortedDictionary now contains the same key-value pairs as sampleDictionary, sorted by key using the provided comparer.
    /// ]]></code>
    /// </example>
    /// <remarks>
    ///     This method is an extension method and can be used directly on any object implementing
    ///     <see cref="System.Collections.Generic.IDictionary{TKey, TValue}" />. It creates a new
    ///     <see cref="SortedDictionary{TKey, TValue}" /> containing the same key-value pairs as the input dictionary, sorted
    ///     by the provided comparer. The original dictionary remains unchanged.
    /// </remarks>
    public static SortedDictionary<TKey, TValue> ToSortedDictionary<TKey, TValue>(
        this IDictionary<TKey, TValue> dictionary, IComparer<TKey> comparer) where TKey : notnull
    {
        if (dictionary == null)
        {
            throw new ArgumentNullException(nameof(dictionary), $"The {nameof(dictionary)} cannot be null.");
        }

        return new SortedDictionary<TKey, TValue>(dictionary, comparer);
    }
}