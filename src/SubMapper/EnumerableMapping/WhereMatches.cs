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
        private List<WhereMatchesContainer<TSubIItem>> _whereMatchess = new List<WhereMatchesContainer<TSubIItem>>();

        private class WhereMatchesContainer
        {
            public Func<IEnumerable<TSubIItem>, IEnumerable<TSubIItem>> GetSubIItemsFromSubIEnumWhereMatches { get; internal set; }
            public PropertyInfo PropertyInfo { get; set; }
            public object EqualValue { get; set; }
        }

        public PartialEnumerableMapping<TSubA, TSubB, TSubIEnum, TSubJ, TSubIItem> First(
            Expression<Func<TSubIItem, bool>> equalsExpression)
        {
            WhereMatchesContainer<TSubIItem>.BuildContainers(equalsExpression).ForEach(_whereMatchess.Add);
            return this;
        }
    }

    public partial class EnumerablesMapping<TA, TB, TSubAEnum, TSubBEnum, TSubAItem, TSubBItem>
    {
        private List<WhereMatchesContainer<TSubAItem>> _whereAMatchess = new List<WhereMatchesContainer<TSubAItem>>();
        private List<WhereMatchesContainer<TSubBItem>> _whereBMatchess = new List<WhereMatchesContainer<TSubBItem>>();

        public EnumerablesMapping<TA, TB, TSubAEnum, TSubBEnum, TSubAItem, TSubBItem> Where(
            Expression<Func<TSubAItem, bool>> aEqualsExpression,
            Expression<Func<TSubBItem, bool>> bEqualsExpression)
        {
            WhereMatchesContainer<TSubAItem>.BuildContainers(aEqualsExpression).ForEach(_whereAMatchess.Add);
            WhereMatchesContainer<TSubBItem>.BuildContainers(bEqualsExpression).ForEach(_whereBMatchess.Add);
            return this;
        }
    }

    class WhereMatchesContainer<TSubXItem>
        where TSubXItem : new()
    {
        public static List<WhereMatchesContainer<TSubXItem>> BuildContainers(Expression<Func<TSubXItem, bool>> xEqualsExpression)
        {
            var expressionVisitor = new MapWhereExpressionVisitor();
            expressionVisitor.Visit(xEqualsExpression);
            var containers = expressionVisitor.WhereEqualsKeyValues.Select(i => new WhereMatchesContainer<TSubXItem>
            {
                PropertyInfo = i.Item1,
                EqualValue = i.Item2,
                GetSubXItemsFromSubXEnumWhereMatches = subBEnum =>
                    subBEnum != null
                    ? subBEnum.Where(j => i.Item1.GetValue(j).Equals(i.Item2))
                    : default(IEnumerable<TSubXItem>)
            }).ToList();
            return containers;
        }

        public Func<IEnumerable<TSubXItem>, IEnumerable<TSubXItem>> GetSubXItemsFromSubXEnumWhereMatches { get; set; }
        public PropertyInfo PropertyInfo { get; set; }
        public object EqualValue { get; set; }
    }
}
