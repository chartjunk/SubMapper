using SubMapper.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SubMapper.EnumerableMapping
{
    public abstract partial class PartialEnumerableMapping<TSubA, TSubB, TSubIEnum, TSubJ, TSubIItem>
        : BaseMapping<TSubA, TSubB>
        where TSubIEnum : IEnumerable<TSubIItem>
        where TSubJ : new()
        where TSubIItem : new()
    {
        protected bool IsRotated { get; set; }

        private void GetXEnumSetY(object nXEnum, object ny,
            Action<object, object> doPrevSubMapping,
            Func<object> getNewY)
        {
            // Get x
            if (nXEnum == null)
                return;

            var xEnum = _iPropertyInfo.GetValue(nXEnum);
            if (xEnum == null)
                return;

            var x = _whereMatchess
                    .Select(w => w.GetSubIItemsFromSubIEnumWhereMatches)
                    .Aggregate((g1, g2) => e => g1(g2(e)))((TSubIEnum)xEnum)
                    .FirstOrDefault();
            if (x == null)
                return;


            // Set y
            var y = _jPropertyInfo != null ? _jPropertyInfo.GetValue(ny) : ny; // may be j => j
            if (y == null)
            {
                y = getNewY();
                _jPropertyInfo.SetValue(ny, y);
            }

            doPrevSubMapping(x, y);
        }

        private void GetXSetYEnum(object nx, object nYEnum, Action<object, object> doPrevSubMapping)
        {
            // Get x
            if (nx == null)
                return;

            var x = _jPropertyInfo != null ? _jPropertyInfo.GetValue(nx) : nx; // may be i => i
            if (x == null)
                return;

            // Set y
            var yEnum = _iPropertyInfo.GetValue(nYEnum);
            var y = yEnum != null
            ? (object)_whereMatchess
                    .Select(w => w.GetSubIItemsFromSubIEnumWhereMatches)
                    .Aggregate((g1, g2) => e => g1(g2(e)))((TSubIEnum)yEnum)
                    .FirstOrDefault()
            : null;

            if (y == null)
            {
                y = new TSubIItem();
                _whereMatchess.ForEach(w => w.PropertyInfo.SetValue(y, w.EqualValue));
                yEnum = _getSubIEnumWithAddedSubIItem((TSubIEnum)yEnum, (TSubIItem)y);
                _iPropertyInfo.SetValue(nYEnum, yEnum);
            }

            doPrevSubMapping(x, y);
        }

        protected SubMap MapFromEnumerableVia<TNonI, TNonJ>(SubMap prevSubMap)
        {
            var result = new SubMap
            {
                IPropertyInfo = _iPropertyInfo,
                JPropertyInfo = _jPropertyInfo ?? prevSubMap.JPropertyInfo,

                GetASetB = (a, b) =>
                {
                    if (IsRotated) GetXSetYEnum(a, b, prevSubMap.GetASetB);
                    else GetXEnumSetY(a, b, prevSubMap.GetASetB, () => new TSubJ());
                },

                GetBSetA = (a, b) =>
                {
                    if (IsRotated) GetXEnumSetY(a, b, prevSubMap.GetBSetA, () => new TSubJ());
                    else GetXSetYEnum(a, b, prevSubMap.GetBSetA);
                },

                MetaMap = new Lazy<MetaMap>(() => new MetaMap
                {
                    MetadataType = typeof(PartialEnumerableMappingMetadata),
                    Metadata = new PartialEnumerableMappingMetadata
                    {
                        IEnumPropertyInfo = _iPropertyInfo,
                        SubIEnumPropertyInfo = prevSubMap.IPropertyInfo,

                        JPropertyInfo = _jPropertyInfo ?? prevSubMap.JPropertyInfo,
                        SubJPropertyInfo = prevSubMap.JPropertyInfo,
                        IsJAndSubJDifferent = _jPropertyInfo != null,

                        WhereEquals = _whereMatchess.Select(m => new WhereEqualsMetadata
                        {
                            PropertyInfo = m.PropertyInfo,
                            EqualValue = m.EqualValue
                        }),

                        MappingViewType = IsRotated ? MappingViewType.JisAandIisB : MappingViewType.IisAandJisB
                    },

                    SubMetaMap = prevSubMap.MetaMap.Value
                })
            };

            return result;
        }
    }
}
