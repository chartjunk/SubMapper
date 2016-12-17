using System;
using System.Linq.Expressions;

namespace MappingLibrary2
{
    public partial class SubMap
    {
        public Func<object, object> GetSubAFromA { get; set; }
        public Func<object, object> GetSubBFromB { get; set; }

        public Action<object, object> SetSubAFromA { get; set; }
        public Action<object, object> SetSubBFromB { get; set; }

        public string SubAPropertyName { get; set; }
        public string SubBPropertyName { get; set; }

        public SubMap MapVia<TNonA, TNonB, TA, TB>(
            Expression<Func<TNonA, TA>> getAExpr,
            Expression<Func<TNonB, TB>> getBExpr)
        {

            var aInfo = getAExpr.GetMapPropertyInfo();
            var bInfo = getBExpr.GetMapPropertyInfo();

            var result = new SubMap
            {
                GetSubAFromA = na => GetSubAFromA(aInfo.Getter(na)),
                GetSubBFromB = nb => GetSubBFromB(bInfo.Getter(nb)),

                SetSubAFromA = (na, v) => SetSubAFromA(aInfo.Getter(na), v),
                SetSubBFromB = (nb, v) => SetSubBFromB(bInfo.Getter(nb), v),

                SubAPropertyName = aInfo.PropertyName,
                SubBPropertyName = bInfo.PropertyName
            };

            return result;
        }
    }
}
