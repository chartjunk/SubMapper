using SubMapper.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SubMapper.EnumerableMapping
{
    public partial class EnumerablesMapping<TA, TB, TSubAEnum, TSubBEnum, TSubAItem, TSubBItem>
        : BaseMapping<TSubAItem, TSubBItem>
        where TSubAEnum : IEnumerable<TSubAItem>
        where TSubBEnum : IEnumerable<TSubBItem>
        where TSubAItem : new()
        where TSubBItem : new()
    {
        private bool _isAnyMapsYetMapped = false;
        private Dictionary<int, int> _yEnumTypedToXEnumTypedIndexing;
        private Func<TSubAEnum, TSubAItem, TSubAEnum> _getSubAEnumWithAddedSubAItem;
        private Func<TSubBEnum, TSubBItem, TSubBEnum> _getSubBEnumWithAddedSubBItem;

        public EnumerablesMapping(out Action finishMappingAction)
        {
            finishMappingAction = FinishMapping;
        }

        public EnumerablesMapping<TA, TB, TSubAEnum, TSubBEnum, TSubAItem, TSubBItem> UsingAdder(
            Func<TSubAEnum, TSubAItem, TSubAEnum> getAEnumWithAddedAItem,
            Func<TSubBEnum, TSubBItem, TSubBEnum> getBEnumWithAddedBItem)
        {
            _getSubAEnumWithAddedSubAItem = getAEnumWithAddedAItem;
            _getSubBEnumWithAddedSubBItem = getBEnumWithAddedBItem;
            return this;
        }

        private void FinishMapping()
        {
            _isAnyMapsYetMapped = false;
            _yEnumTypedToXEnumTypedIndexing = null;
        }

        private void GetXSetY<TSubXItem, TSubYItem>(
            object nXEnum, object nYEnum,
            IEnumerable<WhereMatchesContainer<TSubXItem>> whereXMatchess,
            IEnumerable<WhereMatchesContainer<TSubYItem>> whereYMatchess,
            PropertyInfo xEnumPropertyInfo,
            PropertyInfo yEnumPropertyInfo,
            Action<object, object> doPrevSubMapping)
            where TSubXItem : new()
            where TSubYItem : new()
        {
            if (nXEnum == null)
                return;

            var xEnum = xEnumPropertyInfo.GetValue(nXEnum);
            if (xEnum == null)
                return;

            var xEnumTyped = (IEnumerable<TSubXItem>)xEnum;            
            if (!xEnumTyped.Any())
                return;

            if (whereXMatchess.Any())
                xEnumTyped = whereXMatchess
                    .Select(w => w.GetSubXItemsFromSubXEnumWhereMatches)
                    .Aggregate((g1, g2) => e => g1(g2(e)))(xEnumTyped);            

            var yEnum = yEnumPropertyInfo.GetValue(nYEnum);
            List<TSubYItem> yEnumTyped = null;

            yEnumTyped = yEnum != null
                ? ((IEnumerable<TSubYItem>)yEnum).ToList()
                : new List<TSubYItem>();

            if (!_isAnyMapsYetMapped)
            {
                // If this is the 

                _isAnyMapsYetMapped = true;

                _yEnumTypedToXEnumTypedIndexing = xEnumTyped
                    .Select((x, i) => new { xIx = i, yIx = i + yEnumTyped.Count })
                    .ToDictionary(i => i.yIx, i => i.xIx);
                yEnumTyped = yEnumTyped.Concat(xEnumTyped.Select(x =>
                {
                    var y = new TSubYItem();
                    whereYMatchess.ToList()
                        .ForEach(w => w.PropertyInfo.SetValue(y, w.EqualValue));
                    return y;
                }).ToList()).ToList();
                yEnumPropertyInfo.SetValue(nYEnum, yEnumTyped);
            }

            var isAnyWasMapped = false;
            yEnumTyped
                .Select((y, ix) => new
                {
                    y,
                    xIx = new Func<int?>(() =>
                    {
                        int value;
                        if (_yEnumTypedToXEnumTypedIndexing.TryGetValue(ix, out value))
                            return (int?)value;
                        return null;
                    })()
                })
                .Where(i => i.xIx != null)
                .ToList()
                .ForEach(y =>
                {
                    var x = xEnumTyped.ElementAt(y.xIx.Value);
                    isAnyWasMapped = true;
                    doPrevSubMapping(x, y.y);
                });

            if (isAnyWasMapped)
                yEnumPropertyInfo.SetValue(nYEnum, yEnumTyped);
        }

        protected SubMap MapEnumerableVia<TNonA, TNonB>(SubMap prevSubMap)
        {            
            var result = new SubMap
            {
                APropertyInfo = _iPropertyInfo,
                BPropertyInfo = _jPropertyInfo,

                GetASetB = (a, b) => GetXSetY(a, b, _whereAMatchess, _whereBMatchess, _iPropertyInfo, _jPropertyInfo, prevSubMap.GetASetB),
                GetBSetA = (b, a) => GetXSetY(b, a, _whereBMatchess, _whereAMatchess, _jPropertyInfo, _iPropertyInfo, prevSubMap.GetBSetA),

                MetaMap = new Lazy<MetaMap>(() => new MetaMap
                {                     
                    Metadata = new EnumerablesMappingMetadata
                    {
                        AEnumPropertyInfo = _iPropertyInfo,
                        SubAEnumPropertyInfo = prevSubMap.IPropertyInfo,

                        BEnumPropertyInfo = _jPropertyInfo,
                        SubBEnumPropertyInfo = prevSubMap.JPropertyInfo,

                        WhereAEquals = _whereAMatchess.Select(m => new WhereEqualsMetadata
                        {
                            PropertyInfo = m.PropertyInfo,
                            EqualValue = m.EqualValue
                        }),

                        WhereBEquals = _whereBMatchess.Select(m => new WhereEqualsMetadata
                        {
                            PropertyInfo = m.PropertyInfo,
                            EqualValue = m.EqualValue
                        })
                    },

                    SubMetaMap = prevSubMap.MetaMap.Value
                }),
            };
            return result;
        }

        internal List<SubMap> GetSubMapsWithAddedPath(
            Expression<Func<TA, IEnumerable<TSubAItem>>> getSubAEnumExpr,
            Expression<Func<TB, IEnumerable<TSubBItem>>> getSubBEnumExpr)
        {
            _iPropertyInfo = getSubAEnumExpr.GetPropertyInfo();
            _jPropertyInfo = getSubBEnumExpr.GetPropertyInfo();
            return _subMaps.Select(MapEnumerableVia<TA, TB>).ToList();
        }
    }
}
