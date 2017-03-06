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
            Action<object, object> doPrevSubMapping,
            Func<object> GetNewY)
        {
            if (nx == null)
                return;

            object x = null;
            if (xPropertyInfo != null)
                x = xPropertyInfo.GetValue(nx);
            else
                x = nx;


            var y = yPropertyInfo != null ? yPropertyInfo.GetValue(ny) : ny; // may be k => k
            if (y == null)
            {
                y = GetNewY();
                yPropertyInfo.SetValue(ny, y);
            }

            if (doPrevSubMapping == null)
            {
                x = prevXPropertyInfo.GetValue(x);
                prevYPropertyInfo.SetValue(y, x);
            }
            else
                doPrevSubMapping(x, y);

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
                aPropertyInfo = prevSubMap.IPropertyInfo;
            var bPropertyInfo = bInfo.PropertyInfo;
            if (bInfo.Setter == null)
                bPropertyInfo = prevSubMap.JPropertyInfo;

            var result = new SubMap
            {
                GetASetB = (a, b) => GetXSetY(a, b, aInfo.PropertyInfo, bInfo.PropertyInfo, prevSubMap.IPropertyInfo, prevSubMap.JPropertyInfo, prevSubMap.IsBaseSubMap ? null : prevSubMap.GetASetB, () => new TSubB()),
                GetBSetA = (b, a) => GetXSetY(b, a, bInfo.PropertyInfo, aInfo.PropertyInfo, prevSubMap.JPropertyInfo, prevSubMap.IPropertyInfo, prevSubMap.IsBaseSubMap ? null : prevSubMap.GetBSetA, () => new TSubA()),

                APropertyInfo = aPropertyInfo,
                BPropertyInfo = bPropertyInfo,

                MetaMap = new Lazy<MetaMap>(() => new MetaMap
                {
                    MetadataType = typeof(SubMappingMetadata),
                    Metadata = new SubMappingMetadata
                    {
                        APropertyInfo = aPropertyInfo,
                        //SubAPropertyInfo = prevSubMap.HalfSubMapPair.AHalfSubMap.PropertyInfo,
                        //IsAAndSubADifferent = aPropertyInfo.Name != prevSubMap.HalfSubMapPair.AHalfSubMap.PropertyInfo.Name, // TODO

                        BPropertyInfo = bPropertyInfo,
                        //SubBPropertyInfo = prevSubMap.HalfSubMapPair.BHalfSubMap.PropertyInfo,
                        //IsAndSubBDifferent = bPropertyInfo.Name != prevSubMap.HalfSubMapPair.BHalfSubMap.PropertyInfo.Name, // TODO
                    },
                    SubMetaMap = prevSubMap.MetaMap.Value
                }),
            };

            return result;
        }
    }
}
