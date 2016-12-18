using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SubMapper.EnumerableMapping
{
    public abstract partial class PartialEnumerableMapping<TSubA, TSubB, TSubIEnum, TSubJ, TSubIItem>
        : BaseMapping<TSubA, TSubB>
        where TSubIEnum : IEnumerable<TSubIItem>
        where TSubJ : new()
        where TSubIItem : new()
    {
        protected SubMap MapVia<TNonI, TNonJ>(
            SubMap prevSubMap,
            Expression<Func<TNonI, IEnumerable<TSubIItem>>> getSubIExpr,
            Expression<Func<TNonJ, TSubJ>> getSubJExpr)
        {
            var subIEnumInfo = getSubIExpr.GetMapPropertyInfo();
            var subJInfo = getSubJExpr.GetMapPropertyInfo();

            var result = new SubMap
            {
                GetSubAFromA = na =>
                {
                    var subIEnum = subIEnumInfo.Getter(na);
                    if (subIEnum == null) return null;
                    // TODO: make WhereMatchesContainer consume objets instead of derived types
                    var subIItem = _whereMatchess.First().GetFirstSubAItemFromSubAEnumWhereMatches((TSubIEnum)subIEnum);
                    if (subIItem == null) return null;
                    return prevSubMap.GetSubAFromA(subIItem);
                },
                GetSubBFromB = nb =>
                {
                    var subJ = subJInfo.Getter(nb);
                    if (subJ == null) return null;
                    return prevSubMap.GetSubBFromB(subJ);
                },

                // TODO
                SubAPropertyName = "TODO", //subAEnumInfo.Setter == null ? prevSubMap.SubAPropertyName : (aInfo.PropertyName + "." + prevSubMap.SubAPropertyName),
                SubBPropertyName = subJInfo.Setter == null ? prevSubMap.SubBPropertyName : (subJInfo.PropertyName + "." + prevSubMap.SubBPropertyName),

                SetSubAFromA = (na, v) =>
                {
                    if (v == null) return;
                    var subIEnum = subIEnumInfo.Getter(na);

                    // TODO: allow multiple
                    var whereMatches = _whereMatchess.First();
                    var subIItem = subIEnum != null ? (object)whereMatches.GetFirstSubAItemFromSubAEnumWhereMatches((TSubIEnum)subIEnum) : null;
                    if (subIItem == null)
                    {
                        subIItem = new TSubIItem();
                        typeof(TSubIItem).GetMapPropertyInfo(whereMatches.ValuePropertyName).Setter(subIItem, whereMatches.ValuePropertyValue);
                        subIEnum = _getTSubAEnumWithAddedTSubAItem((TSubIEnum)subIEnum, (TSubIItem)subIItem);
                        subIEnumInfo.Setter(na, subIEnum);
                    }

                    prevSubMap.SetSubAFromA(subIItem, v);
                },
                SetSubBFromB = (nb, v) =>
                {
                    if (v == null) return;
                    if (subJInfo.Getter(nb) == null) subJInfo.Setter(nb, new TSubJ());
                    prevSubMap.SetSubBFromB(subJInfo.Getter(nb), v);
                }
            };

            return result;
        }
    }
}
