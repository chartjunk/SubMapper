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
        public FromEnumerableMapping()
        {
            Extensibility.DerivedMapping = this;
            _whereMatchess = new List<WhereMatchesContainer>();
        }

        public List<SubMap> GetSubMapsWithAddedPath(
            Expression<Func<TA, IEnumerable<TSubAItem>>> getSubAExpr,
            Expression<Func<TB, TSubB>> getSubBExpr)
            => _subMaps.Select(s => MapVia(s, getSubAExpr, getSubBExpr)).ToList();

        //protected static SubMap MapVia(
        //    Func<object, object> getFinalAFromA,
        //    Func<object, object> getFinalBFromB,
        //    Action<Func<object>, Func<object, object>, object> setFinalAFromA,
        //    Action<Func<object>, Func<object, object>, object> setFinalBFromB,
        //    string finalAPropertyName,
        //    string finalBPropertyName)
        //{
            
        //}

        protected SubMap MapVia<TNonA, TNonB>(
            SubMap prevSubMap,
            Expression<Func<TNonA, IEnumerable<TSubAItem>>> getSubAExpr,
            Expression<Func<TNonB, TSubB>> getSubBExpr)
        {
            var subAEnumInfo = getSubAExpr.GetMapPropertyInfo();
            var subBInfo = getSubBExpr.GetMapPropertyInfo();

            var result = new SubMap
            {
                GetSubAFromA = na =>
                {
                    var subAEnum = subAEnumInfo.Getter(na);
                    if (subAEnum == null) return null;
                    // TODO: make WhereMatchesContainer consume objets instead of derived types
                    var subAItem = _whereMatchess.First().GetFirstSubAItemFromSubAEnumWhereMatches((TSubAEnum)subAEnum);
                    if (subAItem == null) return null;
                    return prevSubMap.GetSubAFromA(subAItem);
                },
                GetSubBFromB = nb =>
                {
                    var subB = subBInfo.Getter(nb);
                    if (subB == null) return null;
                    return prevSubMap.GetSubBFromB(subB);
                },

                // TODO
                SubAPropertyName = "TODO", //subAEnumInfo.Setter == null ? prevSubMap.SubAPropertyName : (aInfo.PropertyName + "." + prevSubMap.SubAPropertyName),
                SubBPropertyName = subBInfo.Setter == null ? prevSubMap.SubBPropertyName : (subBInfo.PropertyName + "." + prevSubMap.SubBPropertyName),

                SetSubAFromA = (na, v) =>
                {
                    if (v == null) return;
                    var subAEnum = subAEnumInfo.Getter(na);

                    // TODO: allow multiple
                    var whereMatches = _whereMatchess.First();
                    var subAItem = subAEnum != null ? (object)whereMatches.GetFirstSubAItemFromSubAEnumWhereMatches((TSubAEnum)subAEnum) : null;
                    if (subAItem == null)
                    {
                        subAItem = new TSubAItem();
                        typeof(TSubAItem).GetMapPropertyInfo(whereMatches.ValuePropertyName).Setter(subAItem, whereMatches.ValuePropertyValue);
                        subAEnum = _getTSubAEnumWithAddedTSubAItem((TSubAEnum)subAEnum, (TSubAItem)subAItem);
                        subAEnumInfo.Setter(na, subAEnum);
                    }
                    else
                    {
                        prevSubMap.SetSubAFromA(subAEnum, v);
                    }
                },
                SetSubBFromB = (nb, v) =>
                {
                    if (v == null) return;
                    if (subBInfo.Getter(nb) == null) subBInfo.Setter(nb, new TSubB());
                    prevSubMap.SetSubBFromB(subBInfo.Getter(nb), v);
                }
            };

            return result;
        }
    }
}
