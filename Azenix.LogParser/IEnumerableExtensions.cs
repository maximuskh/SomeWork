using System;
using System.Collections.Generic;
using System.Linq;

namespace Azenix.LogParser
{
    static class EnumerableExtensions
    {
        public static string[] GetTop3ByStringKey<T>(this IEnumerable<T> items,
            Func<T, string> keySelector)
        {
            return items.GroupBy(keySelector)
                .Select(group => new
                {
                    group.Key,
                    Count = group.Count()
                })
                .OrderByDescending(x => x.Count)
                .Take(3)
                .Select(x => x.Key)
                .ToArray();
        }
    }
}
