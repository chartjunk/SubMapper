using System;
using System.Linq.Expressions;

namespace SubMapper
{
    public partial class BaseMapping<TA, TB>
    {
        public BaseMapping<TA, TB> WithSubMapping<TSubA, TSubB>(
            Expression<Func<TA, TSubA>> getSubAExpr,
            Expression<Func<TB, TSubB>> getSubBExpr,
            Func<SubMappingHandle<TA, TB, TSubA, TSubB>, SubMapping<TA, TB, TSubA, TSubB>> getSubMapping)
            where TSubA : new()
            where TSubB : new()
        {
            var subMapping = getSubMapping(new SubMappingHandle<TA, TB, TSubA, TSubB>());
            var fullSubMaps = subMapping.GetSubMapsWithAddedPath(getSubAExpr, getSubBExpr);
            _subMaps.AddRange(fullSubMaps);
            return this;
        }
    }
}
