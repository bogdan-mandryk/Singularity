﻿using System.Collections.Generic;
using System.Linq;

namespace Singularity
{
	internal static class EnumerableExtensions
	{
        public static bool CollectionsAreEqual<T>(this IReadOnlyList<T> list1, IReadOnlyList<T> list2)
        {
            return list1.Count == list2.Count && CollectionsAreEqualInternal(list1, list2);
        }

        private static bool CollectionsAreEqualInternal<T>(this IEnumerable<T> collection1, IEnumerable<T> collection2)
        {
            return collection1.OrderBy(i => i).SequenceEqual(collection2.OrderBy(i => i));
        }
    }
}
