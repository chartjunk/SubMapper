using SubMapper.Metadata;
using System;
using System.Linq.Expressions;

namespace SubMapper
{
    public partial class BaseMapping<TA, TB>
    {
        private static void GetXSetY(object x, object y, Utils.MapPropertyInfo subXInfo, Utils.MapPropertyInfo subYInfo)
        {
            if (x == null)
                return;

            var xv = subXInfo.Getter(x);

            if (xv == null)
                return;

            subYInfo.Setter(y, xv);
        }

        public BaseMapping<TA, TB> Map<TValue>(
            Expression<Func<TA, TValue>> getSubAExpr,
            Expression<Func<TB, TValue>> getSubBExpr)
        { 
            var subAInfo = Utils.GetMapPropertyInfo(getSubAExpr);
            var subBInfo = Utils.GetMapPropertyInfo(getSubBExpr);

            var subMap = new SubMap
            {
                GetASetB = (a, b) => GetXSetY(a, b, subAInfo, subBInfo),
                GetBSetA = (b, a) => GetXSetY(b, a, subBInfo, subAInfo),

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

                APropertyInfo = subAInfo.PropertyInfo,
                BPropertyInfo = subBInfo.PropertyInfo
            };

            _subMaps.Add(subMap);

            return this;
        }
    }
}
