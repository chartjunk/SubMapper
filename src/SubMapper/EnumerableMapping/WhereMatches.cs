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
                GetSubIItemsFromSubIEnumWhereMatches = subIEnum =>
                    subIEnum != null
                    ? subIEnum.Where(j => i.Item1.GetValue(j).Equals(i.Item2))
                    : default(IEnumerable<TSubIItem>)
            }));
            return this;
        }
    }

    public partial class EnumerablesMapping<TA, TB, TSubAEnum, TSubBEnum, TSubAItem, TSubBItem>
    {
        private List<WhereMatchesContainer<TSubAItem>> _whereAMatchess = new List<WhereMatchesContainer<TSubAItem>>();
        private List<WhereMatchesContainer<TSubBItem>> _whereBMatchess = new List<WhereMatchesContainer<TSubBItem>>();

        private class WhereMatchesContainer<TSubXItem>
            where TSubXItem : new()
        {
            public Func<IEnumerable<TSubXItem>, IEnumerable<TSubXItem>> GetSubXItemsFromSubXEnumWhereMatches { get; internal set; }
            public PropertyInfo PropertyInfo { get; set; }
            public object EqualValue { get; set; }
        }

        public EnumerablesMapping<TA, TB, TSubAEnum, TSubBEnum, TSubAItem, TSubBItem> Where(
            Expression<Func<TSubAItem, bool>> aEqualsExpression,
            Expression<Func<TSubBItem, bool>> bEqualsExpression)
        {

            // TODO: refactor

            var aExpressionVisitor = new MapWhereExpressionVisitor();
            aExpressionVisitor.Visit(aEqualsExpression);
            aExpressionVisitor.WhereEqualsKeyValues.ForEach(i => _whereAMatchess.Add(new WhereMatchesContainer<TSubAItem>
            {
                PropertyInfo = i.Item1,
                EqualValue = i.Item2,
                GetSubXItemsFromSubXEnumWhereMatches = subAEnum =>
                    subAEnum != null
                    ? subAEnum.Where(j => i.Item1.GetValue(j).Equals(i.Item2))
                    : default(IEnumerable<TSubAItem>)
            }));

            var bExpressionVisitor = new MapWhereExpressionVisitor();
            bExpressionVisitor.Visit(bEqualsExpression);
            bExpressionVisitor.WhereEqualsKeyValues.ForEach(i => _whereBMatchess.Add(new WhereMatchesContainer<TSubBItem>
            {
                PropertyInfo = i.Item1,
                EqualValue = i.Item2,
                GetSubXItemsFromSubXEnumWhereMatches = subBEnum =>
                    subBEnum != null
                    ? subBEnum.Where(j => i.Item1.GetValue(j).Equals(i.Item2))
                    : default(IEnumerable<TSubBItem>)
            }));

            return this;
        }
    }
}
