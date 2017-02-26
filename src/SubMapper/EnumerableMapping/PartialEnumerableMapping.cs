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

            throw new NotImplementedException();

            var subIEnumPropertyInfo = subIEnumInfo.PropertyInfo;
            //var subJPropertyInfo = subJInfo.PropertyInfo ?? prevSubMap.HalfSubMapPair.JHalfSubMap.PropertyInfo;

            //var result = new SubMap
            //{
            //    HalfSubMapPair = new HalfSubMapPair
            //    {
            //        SubMapViewType = IsRotated ? MappingViewType.JisAandIisB : MappingViewType.IisAandJisB,

            //        IHalfSubMap = new HalfSubMap
            //        {
            //            GetSubFrom = ni =>
            //            {
            //                var subIEnum = subIEnumInfo.Getter(ni);
            //                if (subIEnum == null) return null;
            //                var subIItem = _whereMatchess
            //                        .Select(w => w.GetSubIItemsFromSubIEnumWhereMatches)
            //                        .Aggregate((g1, g2) => e => g1(g2(e)))((TSubIEnum)subIEnum)
            //                        .FirstOrDefault();
            //                if (subIItem == null) return null;
            //                return prevSubMap.HalfSubMapPair.IHalfSubMap.GetSubFrom(subIItem);
            //            },
            //            SetSubFrom = (ni, v) =>
            //            {
            //                if (v == null) return;
            //                var subIEnum = subIEnumInfo.Getter(ni);
            //                var subIItem = subIEnum != null
            //                ? (object)_whereMatchess
            //                        .Select(w => w.GetSubIItemsFromSubIEnumWhereMatches)
            //                        .Aggregate((g1, g2) => e => g1(g2(e)))((TSubIEnum)subIEnum)
            //                        .FirstOrDefault()
            //                : null;

            //                if (subIItem == null)
            //                {
            //                    subIItem = new TSubIItem();
            //                    _whereMatchess.ForEach(w => w.PropertyInfo.SetValue(subIItem, w.EqualValue));
            //                    subIEnum = _getSubIEnumWithAddedSubIItem((TSubIEnum)subIEnum, (TSubIItem)subIItem);
            //                    subIEnumInfo.Setter(ni, subIEnum);
            //                }

            //                prevSubMap.HalfSubMapPair.IHalfSubMap.SetSubFrom(subIItem, v);
            //            },
            //            PropertyInfo = subIEnumPropertyInfo,
            //        },

            //        JHalfSubMap = new HalfSubMap
            //        {
            //            GetSubFrom = nj =>
            //            {
            //                var subJ = subJInfo.Getter(nj);
            //                if (subJ == null) return null;
            //                return prevSubMap.HalfSubMapPair.JHalfSubMap.GetSubFrom(subJ);
            //            },
            //            SetSubFrom = (nj, v) =>
            //            {
            //                if (v == null) return;
            //                if (subJInfo.Getter(nj) == null) subJInfo.Setter(nj, new TSubJ());
            //                prevSubMap.HalfSubMapPair.JHalfSubMap.SetSubFrom(subJInfo.Getter(nj), v);
            //            },
            //            PropertyInfo = subJPropertyInfo
            //        }
            //    },

            //    MetaMap = new Lazy<MetaMap>(() => new MetaMap
            //    {
            //        MetadataType = typeof(PartialEnumerableMappingMetadata),
            //        Metadata = new PartialEnumerableMappingMetadata
            //        {
            //            IEnumPropertyInfo = subIEnumPropertyInfo,
            //            SubIEnumPropertyInfo = prevSubMap.HalfSubMapPair.IHalfSubMap.PropertyInfo,

            //            JPropertyInfo = subJPropertyInfo,
            //            SubJPropertyInfo = prevSubMap.HalfSubMapPair.JHalfSubMap.PropertyInfo,
            //            IsJAndSubJDifferent = subJPropertyInfo.Name != prevSubMap.HalfSubMapPair.JHalfSubMap.PropertyInfo.Name,

            //            WhereEquals = _whereMatchess.Select(m => new PartialEnumerableMappingWhereEqualsMetadata
            //            {
            //                PropertyInfo = m.PropertyInfo,
            //                EqualValue = m.EqualValue
            //            }),

            //            MappingViewType = IsRotated ? MappingViewType.JisAandIisB : MappingViewType.IisAandJisB
            //        },

            //        SubMetaMap = prevSubMap.MetaMap.Value
            //    })
            //};
            //return result;
        }
    }
}
