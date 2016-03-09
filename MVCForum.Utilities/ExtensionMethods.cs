using System;
using System.Collections.Generic;
using System.Linq;

namespace MVCForum.Utilities
{
    public static class ExtensionMethods
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static TSource GetRandom<TSource>(this IList<TSource> list)
        {            
            int index = RandUtils.GetRandom(0, list.Count); // note: second param of Next is not inclusive
            return list[index];
        }
    }
}

