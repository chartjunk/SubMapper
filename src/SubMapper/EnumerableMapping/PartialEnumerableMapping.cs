using SubMapper.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SubMapper.EnumerableMapping
{
    public abstract partial class PartialEnumerableMapping<TSubA, TSubB, TSubIEnum, TSubJ, TSubIItem>
        : BaseMapping<TSubA, TSubB>
        where TSubIEnum : IEnumerable<TSubIItem>
        where TSubJ : new()
        where TSubIItem : new()
    {
        protected bool IsRotated { get; set; }

        protected SubMap MapFromEnumerableVia<TNonI, TNonJ>(
            SubMap prevSubMap,
            Expression<Func<TNonI, IEnumerable<TSubIItem>>> getSubIExpr,
            Expression<Func<TNonJ, TSubJ>> getSubJExpr)
        {
            var subIEnumInfo = getSubIExpr.GetMapPropertyInfo();
            var subJInfo = getSubJExpr.GetMapPropertyInfo();
            
            var subIEnumPropertyInfo = subIEnumInfo.PropertyInfo;
            var subJPropertyInfo = subJInfo.PropertyInfo ?? prevSubMap.HalfSubMapPair.JHalfSubMap.PropertyInfo;

            var result = new SubMap
            {
                HalfSubMapPair = new HalfSubMapPair
                {
                    SubMapViewType = IsRotated ? SubMapViewType.JisAandIisB : SubMapViewType.IisAandJisB,

                    IHalfSubMap = new HalfSubMap
                    {
                        GetSubFrom = ni =>
                        {
                            var subIEnum = subIEnumInfo.Getter(ni);
                            if (subIEnum == null) return null;
                            // TODO: make WhereMatchesContainer consume objets instead of derived types
                            //var subIItem = _whereMatchess.Select(w => w.GetSubIItemsFromSubIEnumWhereMatches).Aggregate((g1, g2) => e => g1(g2((TSubIEnum)subIEnum))); //.First().GetFirstSubAItemFromSubAEnumWhereMatches((TSubIEnum)subIEnum);
                            var subIItem = _whereMatchess
                                    .Select(w => w.GetSubIItemsFromSubIEnumWhereMatches)
                                    .Aggregate((g1, g2) => e => g1(g2(e)))((TSubIEnum)subIEnum)
                                    .FirstOrDefault();
                            if (subIItem == null) return null;
                            return prevSubMap.HalfSubMapPair.IHalfSubMap.GetSubFrom(subIItem);
                        },
                        SetSubFrom = (ni, v) =>
                        {
                            if (v == null) return;
                            var subIEnum = subIEnumInfo.Getter(ni);

                            // TODO: allow multiple
                            //var whereMatches = _whereMatchess.First();
                            //var subIItem = subIEnum != null ? (object)whereMatches.GetFirstSubAItemFromSubAEnumWhereMatches((TSubIEnum)subIEnum) : null;
                            var subIItem = subIEnum != null
                            ? (object)_whereMatchess
                                    .Select(w => w.GetSubIItemsFromSubIEnumWhereMatches)
                                    .Aggregate((g1, g2) => e => g1(g2(e)))((TSubIEnum)subIEnum)
                                    .FirstOrDefault()
                            : null;

                            if (subIItem == null)
                            {
                                subIItem = new TSubIItem();
                                //typeof(TSubIItem).GetMapPropertyInfo(whereMatches.ValuePropertyName).Setter(subIItem, whereMatches.ValuePropertyValue);
                                _whereMatchess.ForEach(w => typeof(TSubIItem).GetMapPropertyInfo(w.ValuePropertyName).Setter(subIItem, w.ValuePropertyValue));
                                subIEnum = _getSubIEnumWithAddedSubIItem((TSubIEnum)subIEnum, (TSubIItem)subIItem);
                                subIEnumInfo.Setter(ni, subIEnum);
                            }

                            prevSubMap.HalfSubMapPair.IHalfSubMap.SetSubFrom(subIItem, v);
                        },
                        PropertyInfo = subIEnumPropertyInfo,
                    },

                    JHalfSubMap = new HalfSubMap
                    {
                        GetSubFrom = nj =>
                        {
                            var subJ = subJInfo.Getter(nj);
                            if (subJ == null) return null;
                            return prevSubMap.HalfSubMapPair.JHalfSubMap.GetSubFrom(subJ);
                        },
                        SetSubFrom = (nj, v) =>
                        {
                            if (v == null) return;
                            if (subJInfo.Getter(nj) == null) subJInfo.Setter(nj, new TSubJ());
                            prevSubMap.HalfSubMapPair.JHalfSubMap.SetSubFrom(subJInfo.Getter(nj), v);
                        },
                        PropertyInfo = subJPropertyInfo
                    }
                },

                MetaMap = new Lazy<MetaMap>(() => new MetaMap
                {
                    MetadataType = typeof(PartialEnumerableMappingMetadata),
                    Metadata = new PartialEnumerableMappingMetadata
                    {
                        // TODO: WHERE infos

                        IEnumPropertyInfo = subIEnumPropertyInfo,
                        SubIEnumPropertyInfo = prevSubMap.HalfSubMapPair.IHalfSubMap.PropertyInfo,

                        JPropertyInfo = subJPropertyInfo,
                        SubJPropertyInfo = prevSubMap.HalfSubMapPair.JHalfSubMap.PropertyInfo,
                        IsJAndSubJDifferent = subJPropertyInfo.Name != prevSubMap.HalfSubMapPair.JHalfSubMap.PropertyInfo.Name,

                        IsRotated = IsRotated
                    },

                    SubMetaMap = prevSubMap.MetaMap.Value
                })
            };

            return result;
        }
    }
}
