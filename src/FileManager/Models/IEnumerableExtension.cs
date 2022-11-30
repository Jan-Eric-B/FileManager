using System.Collections.Generic;

namespace FileManager.Models
{
    /// <summary>
    /// Safely access any collection without the need for a null check.
    /// https://stackoverflow.com/a/24390071/12273101
    /// </summary>
    public static class IEnumerableExtension
    {
        public static IEnumerable<T> Safe<T>(this IEnumerable<T> source)
        {
            if (source == null)
            {
                yield break;
            }

            foreach (var item in source)
            {
                yield return item;
            }
        }
    }
}