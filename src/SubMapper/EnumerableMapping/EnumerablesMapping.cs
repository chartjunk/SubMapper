using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        private Func<TSubAEnum, TSubAItem, TSubAEnum> _getSubAEnumWithAddedSubAItem;
        private Func<TSubBEnum, TSubBItem, TSubBEnum> _getSubBEnumWithAddedSubBItem;

        public EnumerablesMapping<TA, TB, TSubAEnum, TSubBEnum, TSubAItem, TSubBItem> UsingAdder(
            Func<TSubAEnum, TSubAItem, TSubAEnum> getAEnumWithAddedAItem,
            Func<TSubBEnum, TSubBItem, TSubBEnum> getBEnumWithAddedBItem)
        {
            _getSubAEnumWithAddedSubAItem = getAEnumWithAddedAItem;
            _getSubBEnumWithAddedSubBItem = getBEnumWithAddedBItem;
            return this;
        }

        protected SubMap MapEnumerableVia<TNonA, TNonB>(
            SubMap prevSubMap,
            Expression<Func<TNonA, IEnumerable<TSubAItem>>> getSubAExpr,
            Expression<Func<TNonB, IEnumerable<TSubBItem>>> getSubBExpr)
        {
            var subAEnumInfo = getSubAExpr.GetMapPropertyInfo();
            var subBEnumInfo = getSubBExpr.GetMapPropertyInfo();

            var result = new SubMap
            {
                HalfSubMapPair = new HalfSubMapPair
                {
                    AHalfSubMap = new HalfSubMap
                    {
                        GetSubFrom = na =>
                        {
                            var subAEnum = subAEnumInfo.Getter(na);
                            return subAEnum;
                            //if (subAEnum == null) return null;
                            //var subAItem = ((IEnumerable<TSubAItem>)subAEnum).FirstOrDefault();
                            //if (subAItem == null) return null;

                            //return prevSubMap.HalfSubMapPair.AHalfSubMap.GetSubFrom(subAItem);
                        },
                        SetSubFrom = (na, v) =>
                        {
                            if (v == null) return;
                            var subAEnum = subAEnumInfo.Getter(na);
                            var subAItem = subAEnum != null ? (object)((IEnumerable<TSubAItem>)subAEnum).FirstOrDefault() : null;

                            if (subAItem == null)
                            {
                                subAItem = new TSubAItem();
                                subAEnum = _getSubAEnumWithAddedSubAItem((TSubAEnum)subAEnum, (TSubAItem)subAItem);
                                subAEnumInfo.Setter(na, subAEnum);
                            }

                            prevSubMap.HalfSubMapPair.AHalfSubMap.SetSubFrom(subAItem, v);
                        }
                    },
                    BHalfSubMap = new HalfSubMap
                    {
                        GetSubFrom = nb =>
                        {
                            var subBEnum = subBEnumInfo.Getter(nb);
                            if (subBEnum == null) return null;
                            var subBItem = ((IEnumerable<TSubBItem>)subBEnum).FirstOrDefault();
                            if (subBItem == null) return null;

                            return prevSubMap.HalfSubMapPair.BHalfSubMap.GetSubFrom(subBItem);
                        },
                        SetSubFrom = (nb, v) =>
                        {
                            if (v == null) return;
                            var subAEnum = ((IEnumerable<TSubAItem>)v);
                            var subBEnum = ((IEnumerable<TSubBItem>)subBEnumInfo.Getter(nb));

                            if (subBEnum == null)
                                subBEnum = new List<TSubBItem>();

                            for(int i = 0; i < subAEnum.Count(); ++i)
                            {
                                TSubBItem subBItem;
                                // If equivalent child does not exist, create
                                if (subBEnum.Count() <= i)
                                {
                                    subBItem = new TSubBItem();
                                    subBEnum = _getSubBEnumWithAddedSubBItem((TSubBEnum)subBEnum, subBItem);
                                }
                                else
                                    subBItem = subBEnum.ElementAt(i);

                                // Set value for each child
                                var value = prevSubMap.HalfSubMapPair.AHalfSubMap.GetSubFrom(subAEnum.ElementAt(i));
                                prevSubMap.HalfSubMapPair.BHalfSubMap.SetSubFrom(subBItem, value);
                            }

                            // Connect enumerable to its parent
                            subBEnumInfo.Setter(nb, subBEnum);
                        }
                    }
                }
            };
            return result;
        }

        internal List<SubMap> GetSubMapsWithAddedPath(
            Expression<Func<TA, IEnumerable<TSubAItem>>> getSubAEnumExpr,
            Expression<Func<TB, IEnumerable<TSubBItem>>> getSubBEnumExpr)
            => _subMaps.Select(subMap => MapEnumerableVia(subMap, getSubAEnumExpr, getSubBEnumExpr)).ToList();
    }
}
