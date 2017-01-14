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
            public Func<IEnumerable<TSubIItem>, IEnumerable<TSubIItem>> GetSubIItemsFromSubIEnumWhereMatches { get; internal set; }
            public PropertyInfo PropertyInfo { get; set; }
            public object EqualValue { get; set; }
        }

        public PartialEnumerableMapping<TSubA, TSubB, TSubIEnum, TSubJ, TSubIItem> First(
            Expression<Func<TSubIItem, bool>> equalsExpression)
        {
            var expressionVisitor = new MapWhereExpressionVisitor();
            expressionVisitor.Visit(equalsExpression);
            expressionVisitor.WhereEqualsKeyValues.ForEach(i => _whereMatchess.Add(new WhereMatchesContainer
            {
                PropertyInfo = i.Item1,
                EqualValue = i.Item2,
                GetSubIItemsFromSubIEnumWhereMatches = new Func<IEnumerable<TSubIItem>, IEnumerable<TSubIItem>>(subIEnum =>
                    subIEnum != null
                    ? subIEnum.Where(j => i.Item1.GetValue(j).Equals(i.Item2))
                    : default(IEnumerable<TSubIItem>))
            }));
            return this;
        }
    }
}
