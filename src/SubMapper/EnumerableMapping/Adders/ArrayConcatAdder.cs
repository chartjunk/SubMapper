using SubMapper.EnumerableMapping;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SubMapper.EnumerableMapping.Adders
{
    public static partial class PartialEnumerableMappingExtensions
    {
        public static Func<IEnumerable<T>, T, IEnumerable<T>> ArrayConcatAdder<T>()
            where T : new() => (bc, b) => (bc ?? new T[] { }).Concat(new[] { b }).ToArray();

        public static PartialEnumerableMapping<TSubA, TSubB, IEnumerable<TSubIItem>, TSubJ, TSubIItem>
            UsingArrayConcatAdder<TSubA, TSubB, TSubJ, TSubIItem>(
            this PartialEnumerableMapping<TSubA, TSubB, IEnumerable<TSubIItem>, TSubJ, TSubIItem> source)
            where TSubJ : new()
            where TSubIItem : new()
        {
            source.UsingAdder((bc, b) => (bc ?? new TSubIItem[] { }).Concat(new[] { b }).ToArray());
            return source;
        }
    }
}
