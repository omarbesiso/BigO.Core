using JetBrains.Annotations;

namespace BigO.Core.Extensions;

/// <summary>
///     Contains useful utility/extensions methods for working with <see cref="IDictionary{TKey,TValue}" /> objects.
/// </summary>
[PublicAPI]
public static class DictionaryExtensions
{
    /// <summary>
    ///     An <see cref="IDictionary{TKey,TValue}" /> extension method that adds if not contains key.
    /// </summary>
    /// <typeparam name="TKey">Type of the key.</typeparam>
    /// <typeparam name="TValue">Type of the value.</typeparam>
    /// <param name="dictionary">The dictionary to act on.</param>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    /// <returns>true if it succeeds, false if it fails.</returns>
    public static bool AddIfNotContainsKey<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key,
        TValue value)
    {
        ArgumentNullException.ThrowIfNull(dictionary);

        if (dictionary.ContainsKey(key))
        {
            return false;
        }

        dictionary.Add(key, value);
        return true;
    }

    /// <summary>
    ///     An <see cref="IDictionary{TKey,TValue}" /> extension method that adds if not contains key.
    /// </summary>
    /// <typeparam name="TKey">Type of the key.</typeparam>
    /// <typeparam name="TValue">Type of the value.</typeparam>
    /// <param name="dictionary">The dictionary to act on.</param>
    /// <param name="key">The key.</param>
    /// <param name="valueFactory">The value factory.</param>
    /// <returns>true if it succeeds, false if it fails.</returns>
    public static bool AddIfNotContainsKey<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key,
        Func<TValue> valueFactory)
    {
        ArgumentNullException.ThrowIfNull(dictionary);

        if (dictionary.ContainsKey(key))
        {
            return false;
        }

        dictionary.Add(key, valueFactory());
        return true;
    }

    /// <summary>
    ///     An <see cref="IDictionary{TKey,TValue}" /> extension method that adds if not contains key.
    /// </summary>
    /// <typeparam name="TKey">Type of the key.</typeparam>
    /// <typeparam name="TValue">Type of the value.</typeparam>
    /// <param name="dictionary">The dictionary to act on.</param>
    /// <param name="key">The key.</param>
    /// <param name="valueFactory">The value factory.</param>
    /// <returns>true if it succeeds, false if it fails.</returns>
    public static bool AddIfNotContainsKey<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key,
        Func<TKey, TValue> valueFactory)
    {
        ArgumentNullException.ThrowIfNull(dictionary);

        if (dictionary.ContainsKey(key))
        {
            return false;
        }

        dictionary.Add(key, valueFactory(key));
        return true;
    }

    /// <summary>
    ///     An <see cref="IDictionary{TKey,TValue}" /> extension method that removes if contains key.
    /// </summary>
    /// <typeparam name="TKey">Type of the key.</typeparam>
    /// <typeparam name="TValue">Type of the value.</typeparam>
    /// <param name="dictionary">The dictionary to act on.</param>
    /// <param name="key">The key.</param>
    public static void RemoveIfContainsKey<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
    {
        ArgumentNullException.ThrowIfNull(dictionary);

        if (dictionary.ContainsKey(key))
        {
            dictionary.Remove(key);
        }
    }

    /// <summary>
    ///     An <see cref="IDictionary{TKey,TValue}" /> extension method that converts the dictionary to a sorted dictionary.
    /// </summary>
    /// <typeparam name="TKey">Type of the key.</typeparam>
    /// <typeparam name="TValue">Type of the value.</typeparam>
    /// <param name="dictionary">The dictionary to act on.</param>
    /// <returns>
    ///     The <see cref="SortedDictionary{TKey,TValue}" /> instance with items from the provided
    ///     <paramref name="dictionary" />.
    /// </returns>
    public static SortedDictionary<TKey, TValue> ToSortedDictionary<TKey, TValue>(
        this IDictionary<TKey, TValue> dictionary) where TKey : notnull
    {
        ArgumentNullException.ThrowIfNull(dictionary);
        return new SortedDictionary<TKey, TValue>(dictionary);
    }

    /// <summary>
    ///     An <see cref="IDictionary{TKey,TValue}" /> extension method that converts the dictionary to a sorted dictionary.
    /// </summary>
    /// <typeparam name="TKey">Type of the key.</typeparam>
    /// <typeparam name="TValue">Type of the value.</typeparam>
    /// <param name="dictionary">The dictionary to act on.</param>
    /// <param name="comparer">The comparer.</param>
    /// <returns>
    ///     The <see cref="SortedDictionary{TKey,TValue}" /> instance with items from the provided
    ///     <paramref name="dictionary" />.
    /// </returns>
    public static SortedDictionary<TKey, TValue> ToSortedDictionary<TKey, TValue>(
        this IDictionary<TKey, TValue> dictionary, IComparer<TKey> comparer) where TKey : notnull
    {
        ArgumentNullException.ThrowIfNull(dictionary);
        return new SortedDictionary<TKey, TValue>(dictionary, comparer);
    }
}