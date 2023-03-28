using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace BigO.Core.Extensions;

/// <summary>
///     Provides a set of useful extension methods for working with <see cref="Array" /> objects.
/// </summary>
[PublicAPI]
public static class ArrayExtensions
{
    /// <summary>
    ///     Shuffles the elements of the specified array using the Fisher-Yates algorithm.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    /// <param name="array">The array to be shuffled.</param>
    /// <param name="preserveOriginal">
    ///     A boolean value indicating whether the original array should be preserved. If set to
    ///     <c>true</c>, a shuffled copy of the array will be returned. If set to <c>false</c>, the array will be shuffled
    ///     in-place. Default is <c>false</c>.
    /// </param>
    /// <returns>
    ///     A shuffled array, either as a new instance or the original array modified in-place, depending on the value of
    ///     the <paramref name="preserveOriginal" /> parameter.
    /// </returns>
    /// <remarks>
    ///     This extension method uses the Fisher-Yates shuffle algorithm, which provides an unbiased permutation of the input
    ///     array.
    ///     Note that this method is not thread-safe. If you need to shuffle an array concurrently, you should use appropriate
    ///     synchronization mechanisms.
    /// </remarks>
    /// <example>
    ///     Here's an example of how to use this extension method:
    ///     <code>
    /// <![CDATA[
    /// using System;
    /// 
    /// public class Program
    /// {
    ///     public static void Main()
    ///     {
    ///         int[] numbers = { 1, 2, 3, 4, 5 };
    ///         int[] shuffledNumbers = numbers.Shuffle(true);
    /// 
    ///         Console.WriteLine("Original array:");
    ///         foreach (int number in numbers)
    ///         {
    ///             Console.Write(number + " ");
    ///         }
    /// 
    ///         Console.WriteLine("\nShuffled array:");
    ///         foreach (int number in shuffledNumbers)
    ///         {
    ///             Console.Write(number + " ");
    ///         }
    ///     }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="array" /> parameter is <c>null</c>.</exception>
    /// <exception cref="OutOfMemoryException">
    ///     Thrown when there is not enough memory available to create a copy of the
    ///     original array when <paramref name="preserveOriginal" /> is set to <c>true</c>.
    /// </exception>
    public static T[] Shuffle<T>(this T[] array, bool preserveOriginal = false)
    {
        if (array == null)
        {
            throw new ArgumentNullException(nameof(array), $"The {nameof(array)} cannot be null.");
        }

        var shuffledArray = preserveOriginal ? (T[])array.Clone() : array;

        for (var i = 0; i < shuffledArray.Length - 1; i++)
        {
            var j = Random.Shared.Next(i, shuffledArray.Length);
            (shuffledArray[i], shuffledArray[j]) = (shuffledArray[j], shuffledArray[i]);
        }

        return shuffledArray;
    }


    /// <summary>
    ///     Sets a range of elements in the specified array to the default value of each element type.
    /// </summary>
    /// <param name="array">The array to be cleared.</param>
    /// <param name="index">The starting index of the range to clear.</param>
    /// <param name="length">The number of elements in the range to clear.</param>
    /// <remarks>
    ///     This extension method is a wrapper for the <see cref="Array.Clear(Array, int, int)" /> method, which sets the
    ///     elements in the specified range to their default values.
    ///     The method uses aggressive inlining, which means that it is optimized for performance.
    /// </remarks>
    /// <example>
    ///     Here's an example of how to use this extension method:
    ///     <code>
    /// <![CDATA[
    /// using System;
    /// 
    /// public class Program
    /// {
    ///     public static void Main()
    ///     {
    ///         int[] numbers = { 1, 2, 3, 4, 5 };
    ///         numbers.Clear(1, 3);
    /// 
    ///         foreach (int number in numbers)
    ///         {
    ///             Console.Write(number + " ");
    ///         }
    ///     }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="array" /> parameter is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown when the <paramref name="index" /> or <paramref name="length" /> parameters are negative, or if the sum of
    ///     <paramref name="index" /> and <paramref name="length" /> is greater than the array length.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Clear(this Array array, int index, int length)
    {
        if (array == null)
        {
            throw new ArgumentNullException(nameof(array), $"The {nameof(array)} cannot be null.");
        }

        Array.Clear(array, index, length);
    }
}