using System;
using System.Linq.Expressions;

namespace SubMapper
{
    public partial class SubMap
    {
        public SubMap MapVia<TNonA, TNonB, TA, TB>(
            Expression<Func<TNonA, TA>> getSubAExpr,
            Expression<Func<TNonB, TB>> getSubBExpr)
            where TA : new()
            where TB : new()
        {
            var aInfo = getSubAExpr.GetMapPropertyInfo();
            var bInfo = getSubBExpr.GetMapPropertyInfo();

            var result = new SubMap
            {
                GetSubAFromA = na =>
                {
                    var a = aInfo.Getter(na);
                    if (a == null) return null;
                    return GetSubAFromA(a);
                },
                GetSubBFromB = nb =>
                {
                    var b = bInfo.Getter(nb);
                    if (b == null) return null;
                    return GetSubBFromB(b);
                },

                // TODO
                SubAPropertyName = aInfo.Setter == null ? SubAPropertyName : (aInfo.PropertyName + "." + SubAPropertyName),
                SubBPropertyName = bInfo.Setter == null ? SubBPropertyName : (bInfo.PropertyName + "." + SubBPropertyName),

                SetSubAFromA = (na, v) =>
                {
                    if (v == null) return;
                    if (aInfo.Getter(na) == null) aInfo.Setter(na, new TA());
                    SetSubAFromA(aInfo.Getter(na), v);
                },
                SetSubBFromB = (nb, v) =>
                {
                    if (v == null) return;
                    if (bInfo.Getter(nb) == null) bInfo.Setter(nb, new TB());
                    SetSubBFromB(bInfo.Getter(nb), v);
                }
            };            

            return result;
        }
    }
}
