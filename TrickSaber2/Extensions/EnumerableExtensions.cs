using System;
using System.Collections.Generic;

namespace TrickSaber2.Extensions;

internal static class EnumerableExtensions
{
    public static void ForEach<T>(this IEnumerable<T> seq, Action<T> action)
    {
        foreach (var item in seq)
        {
            action(item);
        }
    }
}