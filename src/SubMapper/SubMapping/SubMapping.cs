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
            Extensibility.DerivedMapping = this;
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

            var result = new SubMap
            {
                GetSubAFromA = na =>
                {
                    var a = aInfo.Getter(na);
                    if (a == null) return null;
                    return prevSubMap.GetSubAFromA(a);
                },
                GetSubBFromB = nb =>
                {
                    var b = bInfo.Getter(nb);
                    if (b == null) return null;
                    return prevSubMap.GetSubBFromB(b);
                },

                MetaMap = new Lazy<MetaMap>(() => new MetaMap
                {
                    MetadataType = typeof(SubMappingMetadata),
                    Metadata = new SubMappingMetadata
                    {
                        SuperAProperty = aInfo.PropertyInfo,
                        SubAProperty = prevSubMap.SubAPropertyInfo,
                        IsSuperAAndSubADifferent = aInfo.PropertyName != prevSubMap.SubAPropertyInfo.Name, // TODO

                        SuperBProperty = bInfo.PropertyInfo,
                        SubBProperty = prevSubMap.SubBPropertyInfo,
                        IsSuperBAndSubBDifferent = bInfo.PropertyName != prevSubMap.SubBPropertyInfo.Name, // TODO

                        SubMetaMap = prevSubMap.MetaMap.Value
                    }
                }),

                // TODO
                SubAPropertyName = aInfo.Setter == null ? prevSubMap.SubAPropertyName : (aInfo.PropertyName + "." + prevSubMap.SubAPropertyName),
                SubBPropertyName = bInfo.Setter == null ? prevSubMap.SubBPropertyName : (bInfo.PropertyName + "." + prevSubMap.SubBPropertyName),

                SetSubAFromA = (na, v) =>
                {
                    if (v == null) return;
                    if (aInfo.Getter(na) == null) aInfo.Setter(na, new TSubA());
                    prevSubMap.SetSubAFromA(aInfo.Getter(na), v);
                },
                SetSubBFromB = (nb, v) =>
                {
                    if (v == null) return;
                    if (bInfo.Getter(nb) == null) bInfo.Setter(nb, new TSubB());
                    prevSubMap.SetSubBFromB(bInfo.Getter(nb), v);
                }
            };

            return result;
        }
    }
}
