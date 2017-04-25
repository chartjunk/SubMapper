using SubMapper.Metadata;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace SubMapper
{
    public partial class BaseMapping<TA, TB>
    {
        private static void GetXSetY(object x, object y, PropertyInfo xPropertyInfo, PropertyInfo yPropertyInfo)
        {
            if (x == null)
                return;

            var xv = xPropertyInfo.GetValue(x);

            if (xv == null)
                return;

            yPropertyInfo.SetValue(y, xv);
        }

        public BaseMapping<TA, TB> Map<TValue>(
            Expression<Func<TA, TValue>> getSubAExpr,
            Expression<Func<TB, TValue>> getSubBExpr)
        {
            var aPropertyInfo = getSubAExpr.GetPropertyInfo();
            var bPropertyInfo = getSubBExpr.GetPropertyInfo();

            var subMap = new SubMap
            {
                GetASetB = (a, b) => GetXSetY(a, b, aPropertyInfo, bPropertyInfo),
                GetBSetA = (b, a) => GetXSetY(b, a, bPropertyInfo, aPropertyInfo),

                MetaMap = new Lazy<MetaMap>(() => new MetaMap
                {
                    Metadata = new BaseMapping.BaseMappingMetadata
                    {
                        APropertyInfo = aPropertyInfo,
                        BPropertyInfo = bPropertyInfo
                    },
                    SubMetaMap = null
                }),

                APropertyInfo = aPropertyInfo,
                BPropertyInfo = bPropertyInfo
            };

            _subMaps.Add(subMap);

            return this;
        }
    }
}
