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
            Extensibility.DerivedMapping = this;
        }

        public List<SubMap> GetSubMapsWithAddedPath(
            Expression<Func<TA, TSubA>> getSubAExpr,
            Expression<Func<TB, IEnumerable<TSubBItem>>> getSubBEnumExpr)
            => _subMaps.Select(SubMap.Reverse).Select(s => MapFromEnumerableVia(s, getSubBEnumExpr, getSubAExpr)).Select(SubMap.Reverse).ToList();
    }
}
