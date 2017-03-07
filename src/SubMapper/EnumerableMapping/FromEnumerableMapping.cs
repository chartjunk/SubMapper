using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

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
        }

        public List<SubMap> GetSubMapsWithAddedPath(
            Expression<Func<TA, IEnumerable<TSubAItem>>> getSubAExpr,
            Expression<Func<TB, TSubB>> getSubBExpr)
        {
            // i is always enum
            // j is always the other one
            _iPropertyInfo = getSubAExpr.GetPropertyInfo();
            _jPropertyInfo = getSubBExpr.GetPropertyInfo();
            return _subMaps.Select(MapFromEnumerableVia<TA, TB>).ToList();
        }
    }
}
