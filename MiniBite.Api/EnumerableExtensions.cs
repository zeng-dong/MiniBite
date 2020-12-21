using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MiniBite.Api
{
    public static class EnumerableExtensions
    {
        public static void RemoveAll<T>(this ICollection<T> @this, Func<T, bool> predicate)
        {
            if (@this is List<T> list)
            {
                list.RemoveAll(new Predicate<T>(predicate));
            }
            else
            {
                var itemsToDelete = @this
                    .Where(predicate)
                    .ToList();

                foreach (var item in itemsToDelete) @this.Remove(item);
            }
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            foreach (var element in source)
                if (seenKeys.Add(keySelector(element)))
                    yield return element;
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> act)
        {
            foreach (var element in source) act(element);
        }

        public static void ForEach<T>(this IEnumerable source, Action<T> act)
        {
            source.Cast<T>().ForEach(act);
        }

        public static IEnumerable<RT> ForEach<T, RT>(this IEnumerable<T> array, Func<T, RT> func)
        {
            var list = new List<RT>();
            foreach (var i in array)
            {
                var obj = func(i);
                if (obj != null)
                    list.Add(obj);
            }

            return list;
        }
    }
}
