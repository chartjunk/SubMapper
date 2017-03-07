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
        {
            _iPropertyInfo = getSubAExpr.GetPropertyInfo();
            _jPropertyInfo = getSubBExpr.GetPropertyInfo();
            return _subMaps.Select(MapVia<TA, TB>).ToList();
        }

        private void GetXSetY(object nx, object ny,
            PropertyInfo xPropertyInfo,
            PropertyInfo yPropertyInfo,
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

            doPrevSubMapping(x, y);
        }

        protected SubMap MapVia<TNonA, TNonB>(SubMap prevSubMap)
        {
            var result = new SubMap
            {
                GetASetB = (a, b) => GetXSetY(a, b, _iPropertyInfo, _jPropertyInfo, prevSubMap.GetASetB, () => new TSubB()),
                GetBSetA = (b, a) => GetXSetY(b, a, _jPropertyInfo, _iPropertyInfo, prevSubMap.GetBSetA, () => new TSubA()),

                APropertyInfo = _iPropertyInfo ?? prevSubMap.IPropertyInfo,
                BPropertyInfo = _jPropertyInfo ?? prevSubMap.JPropertyInfo,

                MetaMap = new Lazy<MetaMap>(() => new MetaMap
                {
                    MetadataType = typeof(SubMappingMetadata),
                    Metadata = new SubMappingMetadata
                    {
                        APropertyInfo = _iPropertyInfo,
                        SubAPropertyInfo = prevSubMap.APropertyInfo,
                        IsAAndSubADifferent = _iPropertyInfo != null,

                        BPropertyInfo = _jPropertyInfo,
                        SubBPropertyInfo = prevSubMap.BPropertyInfo,
                        IsBAndSubBDifferent = _jPropertyInfo != null
                    },
                    SubMetaMap = prevSubMap.MetaMap.Value
                }),
            };

            return result;
        }
    }
}
