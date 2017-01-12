using SubMapper.Metadata;
using System;
using System.Linq.Expressions;

namespace SubMapper
{
    public partial class BaseMapping<TA, TB>
    {
        public BaseMapping<TA, TB> Map<TValue>(
            Expression<Func<TA, TValue>> getSubAExpr,
            Expression<Func<TB, TValue>> getSubBExpr)
        { 
            var subAInfo = Utils.GetMapPropertyInfo(getSubAExpr);
            var subBInfo = Utils.GetMapPropertyInfo(getSubBExpr);            

            var subMap = new SubMap
            {
                HalfSubMapPair = new HalfSubMapPair
                {
                    AHalfSubMap = new HalfSubMap
                    {
                        GetSubFrom = subAInfo.Getter,
                        SetSubFrom = (a, v) => { if (v == null) return; subAInfo.Setter(a, v); },
                        PropertyInfo = subAInfo.PropertyInfo
                    },

                    BHalfSubMap = new HalfSubMap
                    {
                        GetSubFrom = subBInfo.Getter,
                        SetSubFrom = (b, v) => { if (v == null) return; subBInfo.Setter(b, v); },
                        PropertyInfo = subBInfo.PropertyInfo
                    }
                },

                MetaMap = new Lazy<MetaMap>(() => new MetaMap
                {
                    MetadataType = typeof(BaseMapping.BaseMappingMetadata),
                    Metadata = new BaseMapping.BaseMappingMetadata
                    {
                        APropertyInfo = subAInfo.PropertyInfo,
                        BPropertyInfo = subBInfo.PropertyInfo
                    },
                    SubMetaMap = null
                }),
            };

            _subMaps.Add(subMap);

            return this;
        }
    }
}
