using System;
using System.Linq.Expressions;
using SubMapper.SubMapping;

namespace SubMapper
{
    public static partial class BaseMappingExtensions
    {
        public static BaseMapping<TA, TB> WithSubMapping<TA, TB, TSubA, TSubB>(
            this BaseMapping<TA, TB> source,
            Expression<Func<TA, TSubA>> getSubAExpr,
            Expression<Func<TB, TSubB>> getSubBExpr,
            Func<SubMappingHandle<TA, TB, TSubA, TSubB>, BaseMapping<TSubA, TSubB>> getInnerBaseMapping)
            where TSubA : new()
            where TSubB : new()
        {
            var innerBaseMapping = getInnerBaseMapping(new SubMappingHandle<TA, TB, TSubA, TSubB>());
            var subMapping = innerBaseMapping.Extensibility.DerivedMapping as SubMapping<TA, TB, TSubA, TSubB>;
            var fullSubMaps = subMapping.GetSubMapsWithAddedPath(getSubAExpr, getSubBExpr);
            source.Extensibility.SubMaps.AddRange(fullSubMaps);
            return source;
        }
    }
}
