using SubMapper.EnumerableMapping.Where;
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
        private List<WhereMatchesContainer> _whereMatchess = new List<WhereMatchesContainer>();

        private class WhereMatchesContainer
        {
            public Func<TSubIEnum, TSubIItem> GetFirstSubAItemFromSubAEnumWhereMatches { get; internal set; }
            public string ValuePropertyName { get; set; }
            public object ValuePropertyValue { get; set; }
        }

        public PartialEnumerableMapping<TSubA, TSubB, TSubIEnum, TSubJ, TSubIItem> First(
            Expression<Func<TSubIItem, bool>> equalsExpression)
        {
            var subIItemValuePropertyInfo = ((((equalsExpression as LambdaExpression).Body as BinaryExpression).Left as MemberExpression).Member as PropertyInfo);
            var getter = new Func<object, object>(p => subIItemValuePropertyInfo.GetValue(p));
            var expressionVisitor = new MapWhereExpressionVisitor();
            expressionVisitor.Visit(equalsExpression);
            expressionVisitor.WhereEqualsKeyValues.ForEach(i => _whereMatchess.Add(new WhereMatchesContainer
            {
                ValuePropertyName = i.Item1,
                ValuePropertyValue = i.Item2,
                GetFirstSubAItemFromSubAEnumWhereMatches = new Func<TSubIEnum, TSubIItem>(subIEnum =>
                    subIEnum != null
                    ? subIEnum.FirstOrDefault(j => getter(j).Equals(i.Item2))
                    : default(TSubIItem))
            }));
            return this;
        }
    }
}
