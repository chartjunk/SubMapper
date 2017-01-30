using System;
using System.Linq.Expressions;
using SubMapper.SubMapping;

namespace SubMapper
{
    public static partial class BaseMappingExtensions
    {
        public static BaseMapping<TA, TB> Sub<TA, TB, TSubA, TSubB>(
            this BaseMapping<TA, TB> source,
            Expression<Func<TA, TSubA>> getSubAExpr,
            Expression<Func<TB, TSubB>> getSubBExpr,
            Func<SubMapping<TA, TB, TSubA, TSubB>, BaseMapping<TSubA, TSubB>> getInnerBaseMapping)
            where TSubA : new()
            where TSubB : new()
        {
            var subMapping = new SubMapping<TA, TB, TSubA, TSubB>();
            var innerBaseMapping = getInnerBaseMapping(subMapping);            
            var fullSubMaps = subMapping.GetSubMapsWithAddedPath(getSubAExpr, getSubBExpr);
            source.Extensibility.SubMaps.AddRange(fullSubMaps);
            return source;
        }
    }
}
