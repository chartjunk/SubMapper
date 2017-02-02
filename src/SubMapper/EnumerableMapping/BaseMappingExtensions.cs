using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using SubMapper.EnumerableMapping;

namespace SubMapper
{
    public static partial class BaseMappingExtensions
    {
        public static BaseMapping<TA, TB> FromEnum<TA, TB, TSubB, TSubAItem>(
            this BaseMapping<TA, TB> source,
            Expression<Func<TA, IEnumerable<TSubAItem>>> getSubAEnumExpr,
            Expression<Func<TB, TSubB>> getSubBExpr,
            Func<FromEnumerableMapping<TA, TB, IEnumerable<TSubAItem>, TSubB, TSubAItem>, BaseMapping<TSubAItem, TSubB>> getInnerBaseMapping)            
            where TSubAItem : new()
            where TSubB : new()
        {
            var fromEnumerableMapping = new FromEnumerableMapping<TA, TB, IEnumerable<TSubAItem>, TSubB, TSubAItem>();
            var innerBaseMapping = getInnerBaseMapping(fromEnumerableMapping);
            var fullSubMaps = fromEnumerableMapping.GetSubMapsWithAddedPath(getSubAEnumExpr, getSubBExpr);
            source.Extensibility.SubMaps.AddRange(fullSubMaps);
            return source;
        }

        public static BaseMapping<TA, TB> ToEnum<TA, TB, TSubA, TSubBItem>(
            this BaseMapping<TA, TB> source,
            Expression<Func<TA, TSubA>> getSubAExpr,
            Expression<Func<TB, IEnumerable<TSubBItem>>> getSubBEnumExpr,
            Func<ToEnumerableMapping<TA, TB, TSubA, IEnumerable<TSubBItem>, TSubBItem>, BaseMapping<TSubA, TSubBItem>> getInnerBaseMapping)
            where TSubA : new()
            where TSubBItem : new()
        {
            var toEnumerableMapping = new ToEnumerableMapping<TA, TB, TSubA, IEnumerable<TSubBItem>, TSubBItem>();
            var innerBaseMapping = getInnerBaseMapping(toEnumerableMapping);
            var fullSubMaps = toEnumerableMapping.GetSubMapsWithAddedPath(getSubAExpr, getSubBEnumExpr);
            source.Extensibility.SubMaps.AddRange(fullSubMaps);
            return source;
        }

        public static BaseMapping<TA, TB> Enums<TA, TB, TSubAItem, TSubBItem>(
            this BaseMapping<TA, TB> source,
            Expression<Func<TA, IEnumerable<TSubAItem>>> getSubAEnumExpr,
            Expression<Func<TB, IEnumerable<TSubBItem>>> getSubBEnumExpr,
            Func<EnumerableMapping<TA, TB, IEnumerable<TSubAItem>, IEnumerable<TSubBItem>, TSubAItem, TSubBItem>, BaseMapping<TSubAItem, TSubBItem>> getInnerBaseMapping)
            where TSubAItem : new()
            where TSubBItem : new()
        {
            var enumerablesMapping = new EnumerableMapping<TA, TB, IEnumerable<TSubAItem>, IEnumerable<TSubBItem>, TSubAItem, TSubBItem>();
            var innerBaseMapping = getInnerBaseMapping(enumerablesMapping);
            var fullSubMaps = enumerablesMapping.GetSubMapsWithAddedPath(getSubAEnumExpr, getSubBEnumExpr);
            source.Extensibility.SubMaps.AddRange(fullSubMaps);
            return source;
        }
    }
}
