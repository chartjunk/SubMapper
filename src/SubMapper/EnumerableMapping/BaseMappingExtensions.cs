using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using SubMapper.EnumerableMapping;

namespace SubMapper
{
    public static partial class BaseMappingExtensions
    {
        public static BaseMapping<TA, TB> WithFromEnumerableMapping<TA, TB, TSubB, TSubAItem>(
            this BaseMapping<TA, TB> source,
            Expression<Func<TA, IEnumerable<TSubAItem>>> getSubAEnumExpr,
            Expression<Func<TB, TSubB>> getSubBExpr,
            Func<FromEnumerableMappingHandle<TA, TB, IEnumerable<TSubAItem>, TSubB, TSubAItem>, BaseMapping<TSubAItem, TSubB>> getInnerBaseMapping)            
            where TSubAItem : new()
            where TSubB : new()
        {
            var innerBaseMapping = getInnerBaseMapping(new FromEnumerableMappingHandle<TA, TB, IEnumerable<TSubAItem>, TSubB, TSubAItem>());
            var fromEnumerableMapping = innerBaseMapping.Extensibility.DerivedMapping as FromEnumerableMapping<TA, TB, IEnumerable<TSubAItem>, TSubB, TSubAItem>;
            var fullSubMaps = fromEnumerableMapping.GetSubMapsWithAddedPath(getSubAEnumExpr, getSubBExpr);
            source.Extensibility.SubMaps.AddRange(fullSubMaps);
            return source;
        }

        public static BaseMapping<TA, TB> WithToEnumerableMapping<TA, TB, TSubA, TSubBItem>(
            this BaseMapping<TA, TB> source,
            Expression<Func<TA, TSubA>> getSubAExpr,
            Expression<Func<TB, IEnumerable<TSubBItem>>> getSubBEnumExpr,
            Func<ToEnumerableMappingHandle<TA, TB, TSubA, IEnumerable<TSubBItem>, TSubBItem>, BaseMapping<TSubA, TSubBItem>> getInnerBaseMapping)
            where TSubA : new()
            where TSubBItem : new()
        {
            var innerBaseMapping = getInnerBaseMapping(new ToEnumerableMappingHandle<TA, TB, TSubA, IEnumerable<TSubBItem>, TSubBItem>());
            var fromEnumerableMapping = innerBaseMapping.Extensibility.DerivedMapping as ToEnumerableMapping<TA, TB, TSubA, IEnumerable<TSubBItem>, TSubBItem>;
            var fullSubMaps = fromEnumerableMapping.GetSubMapsWithAddedPath(getSubAExpr, getSubBEnumExpr);
            source.Extensibility.SubMaps.AddRange(fullSubMaps);
            return source;
        }
    }
}
