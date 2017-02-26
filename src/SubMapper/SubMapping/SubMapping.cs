using SubMapper.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SubMapper.SubMapping
{
    public partial class SubMapping<TA, TB, TSubA, TSubB>
        : BaseMapping<TSubA, TSubB>
        where TSubA : new()
        where TSubB : new()
    {
        public SubMapping()
        {            
        }

        public List<SubMap> GetSubMapsWithAddedPath(
            Expression<Func<TA, TSubA>> getSubAExpr,
            Expression<Func<TB, TSubB>> getSubBExpr) 
            =>  _subMaps.Select(s => MapVia(s, getSubAExpr, getSubBExpr)).ToList();

        protected static SubMap MapVia<TNonA, TNonB>(
            SubMap prevSubMap,
            Expression<Func<TNonA, TSubA>> getSubAExpr,
            Expression<Func<TNonB, TSubB>> getSubBExpr)
        {
            var aInfo = getSubAExpr.GetMapPropertyInfo();
            var bInfo = getSubBExpr.GetMapPropertyInfo();

            throw new NotImplementedException();

            // TODO: refactor this kludge
            //var aPropertyInfo = aInfo.PropertyInfo;
            //if(aInfo.Setter == null)
            //    aPropertyInfo = prevSubMap.HalfSubMapPair.AHalfSubMap.PropertyInfo;
            //var bPropertyInfo = bInfo.PropertyInfo;
            //if(bInfo.Setter == null)
            //    bPropertyInfo = prevSubMap.HalfSubMapPair.BHalfSubMap.PropertyInfo;

            //var result = new SubMap
            //{
            //    HalfSubMapPair = new HalfSubMapPair
            //    {
            //        AHalfSubMap = new HalfSubMap
            //        {
            //            GetSubFrom = na =>
            //            {
            //                var a = aInfo.Getter(na);
            //                if (a == null) return null;
            //                return prevSubMap.HalfSubMapPair.AHalfSubMap.GetSubFrom(a);
            //            },
            //            SetSubFrom = (na, v) =>
            //            {
            //                if (v == null) return;
            //                if (aInfo.Getter(na) == null) aInfo.Setter(na, new TSubA());
            //                prevSubMap.HalfSubMapPair.AHalfSubMap.SetSubFrom(aInfo.Getter(na), v);
            //            },
            //            PropertyInfo = aPropertyInfo
            //        },

            //        BHalfSubMap = new HalfSubMap
            //        {
            //            GetSubFrom = nb =>
            //            {
            //                var b = bInfo.Getter(nb);
            //                if (b == null) return null;
            //                return prevSubMap.HalfSubMapPair.BHalfSubMap.GetSubFrom(b);
            //            },
            //            SetSubFrom = (nb, v) =>
            //            {
            //                if (v == null) return;
            //                if (bInfo.Getter(nb) == null) bInfo.Setter(nb, new TSubB());
            //                prevSubMap.HalfSubMapPair.BHalfSubMap.SetSubFrom(bInfo.Getter(nb), v);
            //            },
            //            PropertyInfo = bPropertyInfo
            //        },
            //    },

            //    MetaMap = new Lazy<MetaMap>(() => new MetaMap
            //    {
            //        MetadataType = typeof(SubMappingMetadata),
            //        Metadata = new SubMappingMetadata
            //        {
            //            APropertyInfo = aPropertyInfo,
            //            SubAPropertyInfo = prevSubMap.HalfSubMapPair.AHalfSubMap.PropertyInfo,
            //            IsAAndSubADifferent = aPropertyInfo.Name != prevSubMap.HalfSubMapPair.AHalfSubMap.PropertyInfo.Name, // TODO

            //            BPropertyInfo = bPropertyInfo,
            //            SubBPropertyInfo = prevSubMap.HalfSubMapPair.BHalfSubMap.PropertyInfo,
            //            IsBAndSubBDifferent = bPropertyInfo.Name != prevSubMap.HalfSubMapPair.BHalfSubMap.PropertyInfo.Name, // TODO
            //        },
            //        SubMetaMap = prevSubMap.MetaMap.Value
            //    }),
            //};

            //return result;
        }
    }
}
