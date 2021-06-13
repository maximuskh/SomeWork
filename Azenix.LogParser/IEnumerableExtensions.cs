using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azenix.LogParser
{
    public static class IEnumerableExtensions
    {
        public static string[] GetTop3ByStringKey<T>(this IEnumerable<T> items,
            Func<T, string> keySelector)
        {
            return items.GroupBy(keySelector)
                .Select(group => new
                {
                    Key = group.Key,
                    Count = group.Count()
                })
                .OrderByDescending(x => x.Count)
                .Take(3)
                .Select(x => x.Key)
                .ToArray();
        }
    }
}
