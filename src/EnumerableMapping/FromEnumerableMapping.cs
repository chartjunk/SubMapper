using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SubMapper.EnumerableMapping
{
    public partial class FromEnumerableMapping<TA, TB, TSubAEnum, TSubB, TSubAItem>
        : BaseMapping<TSubAItem, TSubB>
        where TSubB : new()
        where TSubAItem : new()
        where TSubAEnum : IEnumerable<TSubAItem>
    {
        public List<SubMap> GetSubMapsWithAddedPath(
            Expression<Func<TA, IEnumerable<TSubAItem>>> getSubAExpr,
            Expression<Func<TB, TSubB>> getSubBExpr)
            => _subMaps.Select(s => MapVia(s, getSubAExpr, getSubBExpr)).ToList();

        protected static SubMap MapVia<TNonA, TNonB>(
            SubMap prevSubMap,
            Expression<Func<TNonA, IEnumerable<TSubAItem>>> getSubAExpr,
            Expression<Func<TNonB, TSubB>> getSubBExpr)
        {
            var aInfo = getSubAExpr.GetMapPropertyInfo();
            var bInfo = getSubBExpr.GetMapPropertyInfo();

            var result = new SubMap
            {
                GetSubAFromA = na =>
                {
                    var a = aInfo.Getter(na);
                    if (a == null) return null;
                    return prevSubMap.GetSubAFromA(a);
                },
                GetSubBFromB = nb =>
                {
                    var b = bInfo.Getter(nb);
                    if (b == null) return null;
                    return prevSubMap.GetSubBFromB(b);
                },

                // TODO
                SubAPropertyName = aInfo.Setter == null ? prevSubMap.SubAPropertyName : (aInfo.PropertyName + "." + prevSubMap.SubAPropertyName),
                SubBPropertyName = bInfo.Setter == null ? prevSubMap.SubBPropertyName : (bInfo.PropertyName + "." + prevSubMap.SubBPropertyName),

                SetSubAFromA = (na, v) =>
                {
                    if (v == null) return;
                    if (aInfo.Getter(na) == null) aInfo.Setter(na, new TSubA());
                    prevSubMap.SetSubAFromA(aInfo.Getter(na), v);
                },
                SetSubBFromB = (nb, v) =>
                {
                    if (v == null) return;
                    if (bInfo.Getter(nb) == null) bInfo.Setter(nb, new TSubB());
                    prevSubMap.SetSubBFromB(bInfo.Getter(nb), v);
                }
            };

            return result;
        }
    }
}
