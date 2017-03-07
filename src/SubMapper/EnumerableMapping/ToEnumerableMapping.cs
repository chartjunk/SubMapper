using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SubMapper.EnumerableMapping
{
    public partial class ToEnumerableMapping<TA, TB, TSubA, TSubBEnum, TSubBItem>
        : PartialEnumerableMapping<TSubA, TSubBItem, TSubBEnum, TSubA, TSubBItem>
        where TSubA : new()
        where TSubBItem : new()
        where TSubBEnum : IEnumerable<TSubBItem>
    {
        public ToEnumerableMapping()
        {
            IsRotated = true;
        }

        public List<SubMap> GetSubMapsWithAddedPath(
            Expression<Func<TA, TSubA>> getSubAExpr,
            Expression<Func<TB, IEnumerable<TSubBItem>>> getSubBEnumExpr)
        {
            // i is always enum
            // j is always the other one
            _iPropertyInfo = getSubBEnumExpr.GetPropertyInfo();
            _jPropertyInfo = getSubAExpr.GetPropertyInfo();
            return _subMaps.Select(SubMap.Reverse).Select(MapFromEnumerableVia<TB, TA>).Select(SubMap.Reverse).ToList();
        }
    }
}
