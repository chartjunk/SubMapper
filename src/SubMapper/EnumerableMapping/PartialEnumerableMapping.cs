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

            // TODO: refactor
            // TODO: does rotated map properly? SubIEnum -> SubA??? Nope, they dont
            var subIEnumPropertyInfo = subIEnumInfo.PropertyInfo; //?? (IsRotated ? prevSubMap.SubBPropertyInfo : prevSubMap.SubAPropertyInfo); this should be useless -> always non null
            var subJPropertyInfo = subJInfo.PropertyInfo ?? prevSubMap.HalfSubMapPair.JHalfSubMap.PropertyInfo;

            // TODO: Rotate prevSubMap if it should be

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
                            var subIItem = _whereMatchess.First().GetFirstSubAItemFromSubAEnumWhereMatches((TSubIEnum)subIEnum);
                            if (subIItem == null) return null;
                            return prevSubMap.HalfSubMapPair.IHalfSubMap.GetSubFrom(subIItem);
                        },
                        SetSubFrom = (ni, v) =>
                        {
                            if (v == null) return;
                            var subIEnum = subIEnumInfo.Getter(ni);

                            // TODO: allow multiple
                            var whereMatches = _whereMatchess.First();
                            var subIItem = subIEnum != null ? (object)whereMatches.GetFirstSubAItemFromSubAEnumWhereMatches((TSubIEnum)subIEnum) : null;
                            if (subIItem == null)
                            {
                                subIItem = new TSubIItem();
                                typeof(TSubIItem).GetMapPropertyInfo(whereMatches.ValuePropertyName).Setter(subIItem, whereMatches.ValuePropertyValue);
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

                //GetSubAFromA = na =>
                //{
                //    var subIEnum = subIEnumInfo.Getter(na);
                //    if (subIEnum == null) return null;
                //    // TODO: make WhereMatchesContainer consume objets instead of derived types
                //    var subIItem = _whereMatchess.First().GetFirstSubAItemFromSubAEnumWhereMatches((TSubIEnum)subIEnum);
                //    if (subIItem == null) return null;
                //    return prevSubMap.GetSubAFromA(subIItem);
                //},
                //GetSubBFromB = nb =>
                //{
                //    var subJ = subJInfo.Getter(nb);
                //    if (subJ == null) return null;
                //    return prevSubMap.GetSubBFromB(subJ);
                //},

                //// TODO
                //SubAPropertyName = "TODO", //subAEnumInfo.Setter == null ? prevSubMap.SubAPropertyName : (aInfo.PropertyName + "." + prevSubMap.SubAPropertyName),
                //SubBPropertyName = subJInfo.Setter == null ? prevSubMap.SubBPropertyName : (subJInfo.PropertyName + "." + prevSubMap.SubBPropertyName),

                //SubAPropertyInfo = subIEnumPropertyInfo,
                //SubBPropertyInfo = subJPropertyInfo,

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

                //MetaMap =
                //IsRotated ?
                //new Lazy<MetaMap>(() => new MetaMap
                //{
                //    MetadataType = typeof(PartialEnumerableMappingMetadata),
                //    Metadata = new PartialEnumerableMappingMetadata
                //    {
                //        // TODO: WHERE infos

                //        IEnumPropertyInfo = subIEnumPropertyInfo,
                //        SubIEnumPropertyInfo = prevSubMap.SubBPropertyInfo,

                //        JPropertyInfo = subJPropertyInfo,
                //        SubJPropertyInfo = prevSubMap.SubAPropertyInfo,
                //        IsJAndSubJDifferent = subJPropertyInfo.Name != prevSubMap.SubAPropertyInfo.Name, // TODO

                //        IsRotated = IsRotated
                //    },
                //    SubMetaMap = prevSubMap.MetaMap.Value
                //}) :
                //new Lazy<MetaMap>(() => new MetaMap
                //{
                //    MetadataType = typeof(PartialEnumerableMappingMetadata),
                //    Metadata = new PartialEnumerableMappingMetadata
                //    {
                //        // TODO: WHERE infos

                //        IEnumPropertyInfo = subIEnumPropertyInfo,
                //        SubIEnumPropertyInfo = prevSubMap.SubAPropertyInfo,

                //        JPropertyInfo = subJPropertyInfo,
                //        SubJPropertyInfo = prevSubMap.SubBPropertyInfo,
                //        IsJAndSubJDifferent = subJPropertyInfo.Name != prevSubMap.SubBPropertyInfo.Name, // TODO

                //        IsRotated = IsRotated
                //    },
                //    SubMetaMap = prevSubMap.MetaMap.Value
                //}),

                //SetSubAFromA = (na, v) =>
                //{
                //    if (v == null) return;
                //    var subIEnum = subIEnumInfo.Getter(na);

                //    // TODO: allow multiple
                //    var whereMatches = _whereMatchess.First();
                //    var subIItem = subIEnum != null ? (object)whereMatches.GetFirstSubAItemFromSubAEnumWhereMatches((TSubIEnum)subIEnum) : null;
                //    if (subIItem == null)
                //    {
                //        subIItem = new TSubIItem();
                //        typeof(TSubIItem).GetMapPropertyInfo(whereMatches.ValuePropertyName).Setter(subIItem, whereMatches.ValuePropertyValue);
                //        subIEnum = _getSubIEnumWithAddedSubIItem((TSubIEnum)subIEnum, (TSubIItem)subIItem);
                //        subIEnumInfo.Setter(na, subIEnum);
                //    }

                //    prevSubMap.SetSubAFromA(subIItem, v);
                //},
                //SetSubBFromB = (nb, v) =>
                //{
                //    if (v == null) return;
                //    if (subJInfo.Getter(nb) == null) subJInfo.Setter(nb, new TSubJ());
                //    prevSubMap.SetSubBFromB(subJInfo.Getter(nb), v);
                //}
            };

            return result;
        }
    }
}
