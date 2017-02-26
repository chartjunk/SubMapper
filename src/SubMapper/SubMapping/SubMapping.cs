using SubMapper.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

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

        private static void GetXSetY(object nx, object ny, 
            PropertyInfo xPropertyInfo, 
            PropertyInfo yPropertyInfo, 
            PropertyInfo prevXPropertyInfo, 
            PropertyInfo prevYPropertyInfo,
            Func<object> GetNewY)
        {
            if (nx == null)
                return;

            var x = xPropertyInfo.GetValue(nx);
            if (x == null)
                return;

            var prevX = prevXPropertyInfo.GetValue(x);
            if (prevX == null)
                return;

            var y = yPropertyInfo.GetValue(ny);
            if (y == null)
            {
                y = GetNewY();
                yPropertyInfo.SetValue(ny, y);
            }

            prevYPropertyInfo.SetValue(y, prevX);
        }

        protected static SubMap MapVia<TNonA, TNonB>(
            SubMap prevSubMap,
            Expression<Func<TNonA, TSubA>> getSubAExpr,
            Expression<Func<TNonB, TSubB>> getSubBExpr)
        {
            var aInfo = getSubAExpr.GetMapPropertyInfo();
            var bInfo = getSubBExpr.GetMapPropertyInfo();

            //TODO: refactor this kludge
            var aPropertyInfo = aInfo.PropertyInfo;
            if (aInfo.Setter == null)
                aPropertyInfo = prevSubMap.HalfSubMapPair.AHalfSubMap.PropertyInfo;
            var bPropertyInfo = bInfo.PropertyInfo;
            if (bInfo.Setter == null)
                bPropertyInfo = prevSubMap.HalfSubMapPair.BHalfSubMap.PropertyInfo;

            var result = new SubMap
            {
                GetASetB = (a, b) => GetXSetY(a, b, aPropertyInfo, bPropertyInfo, prevSubMap.APropertyInfo, prevSubMap.BPropertyInfo, () => new TSubB()),
                GetBSetA = (b, a) => GetXSetY(b, a, bPropertyInfo, aPropertyInfo, prevSubMap.BPropertyInfo, prevSubMap.APropertyInfo, () => new TSubA()),

                APropertyInfo = aPropertyInfo,
                BPropertyInfo = bPropertyInfo,

                MetaMap = new Lazy<MetaMap>(() => new MetaMap
                {
                    MetadataType = typeof(SubMappingMetadata),
                    Metadata = new SubMappingMetadata
                    {
                        APropertyInfo = aPropertyInfo,
                        SubAPropertyInfo = prevSubMap.HalfSubMapPair.AHalfSubMap.PropertyInfo,
                        IsAAndSubADifferent = aPropertyInfo.Name != prevSubMap.HalfSubMapPair.AHalfSubMap.PropertyInfo.Name, // TODO

                        BPropertyInfo = bPropertyInfo,
                        SubBPropertyInfo = prevSubMap.HalfSubMapPair.BHalfSubMap.PropertyInfo,
                        IsBAndSubBDifferent = bPropertyInfo.Name != prevSubMap.HalfSubMapPair.BHalfSubMap.PropertyInfo.Name, // TODO
                    },
                    SubMetaMap = prevSubMap.MetaMap.Value
                }),
            };

            return result;
        }
    }
}
