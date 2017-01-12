using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SubMapper.EnumerableMapping
{
    public partial class FromEnumerableMapping<TA, TB, TSubAEnum, TSubB, TSubAItem>
        : PartialEnumerableMapping<TSubAItem, TSubB, TSubAEnum, TSubB, TSubAItem>
        where TSubB : new()
        where TSubAItem : new()
        where TSubAEnum : IEnumerable<TSubAItem>
    {
        public FromEnumerableMapping()
        {
            IsRotated = false;
            Extensibility.DerivedMapping = this;            
        }

        public List<SubMap> GetSubMapsWithAddedPath(
            Expression<Func<TA, IEnumerable<TSubAItem>>> getSubAExpr,
            Expression<Func<TB, TSubB>> getSubBExpr)
            => _subMaps.Select(s => MapFromEnumerableVia(s, getSubAExpr, getSubBExpr)).ToList();
    }
}
