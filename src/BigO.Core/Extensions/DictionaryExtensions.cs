namespace BigO.Core.Extensions;

/// <summary>
///     Provides a set of useful extension methods for working with <see cref="IDictionary{TKey,TValue}" /> objects.
/// </summary>
[PublicAPI]
public static class DictionaryExtensions
{
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
    [System.Diagnostics.Contracts.Pure]
    public static SortedDictionary<TKey, TValue> ToSortedDictionary<TKey, TValue>(
        this IDictionary<TKey, TValue> dictionary) where TKey : notnull
    {
        ArgumentNullException.ThrowIfNull(dictionary);

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
    /// <exception cref="System.ArgumentNullException">
    ///     Thrown when <paramref name="dictionary" /> or
    ///     <paramref name="comparer" /> is <c>null</c>.
    /// </exception>
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
    [System.Diagnostics.Contracts.Pure]
    public static SortedDictionary<TKey, TValue> ToSortedDictionary<TKey, TValue>(
        this IDictionary<TKey, TValue> dictionary, IComparer<TKey> comparer) where TKey : notnull
    {
        ArgumentNullException.ThrowIfNull(dictionary);
        ArgumentNullException.ThrowIfNull(comparer);

        return new SortedDictionary<TKey, TValue>(dictionary, comparer);
    }

    /// <summary>
    ///     Retrieves the value associated with the specified key or returns a default value if the key is not found.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    /// <param name="dictionary">The dictionary to retrieve the value from.</param>
    /// <param name="key">The key of the value to retrieve.</param>
    /// <param name="defaultValue">The default value to return if the key is not found.</param>
    /// <returns>The value associated with the specified key, or the default value if the key is not found.</returns>
    [System.Diagnostics.Contracts.Pure]
    public static TValue? GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key,
        TValue? defaultValue = default)
    {
        ArgumentNullException.ThrowIfNull(dictionary);

        return dictionary.TryGetValue(key, out var value) ? value : defaultValue;
    }

    /// <summary>
    ///     Removes all elements that match the conditions defined by the specified predicate.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    /// <param name="dictionary">The dictionary to remove elements from.</param>
    /// <param name="predicate">The predicate that defines the conditions of the elements to remove.</param>
    public static void RemoveWhere<TKey, TValue>(
        this IDictionary<TKey, TValue> dictionary, Func<KeyValuePair<TKey, TValue>, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(dictionary);
        ArgumentNullException.ThrowIfNull(predicate);

        var filteredList = dictionary.Keys.Where(k => predicate(new KeyValuePair<TKey, TValue>(k, dictionary[k])))
            .ToList();
        foreach (var key in filteredList)
        {
            dictionary.Remove(key);
        }
    }

    /// <summary>
    ///     Merges another dictionary into the current dictionary. If the same key exists in both dictionaries,
    ///     the value from the other dictionary overwrites the value in the current dictionary.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    /// <param name="dictionary">The current dictionary to merge into.</param>
    /// <param name="otherDictionary">The dictionary to merge from.</param>
    public static void Merge<TKey, TValue>(this IDictionary<TKey, TValue> dictionary,
        IDictionary<TKey, TValue> otherDictionary)
    {
        ArgumentNullException.ThrowIfNull(dictionary);
        ArgumentNullException.ThrowIfNull(dictionary);

        foreach (var kvp in otherDictionary)
        {
            dictionary[kvp.Key] = kvp.Value;
        }
    }
}