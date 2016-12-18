using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

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

        public PartialEnumerableMapping<TSubA, TSubB, TSubIEnum, TSubJ, TSubIItem> FirstWhereEquals<TValue>(
            Expression<Func<TSubIItem, TValue>> getValue, TValue equalValue)
        {
            var subAItemValuePropertyInfo = getValue.GetMapPropertyInfo();
            var getFirstSubAItemFromSubAEnumWhereMatches =
                new Func<TSubIEnum, TSubIItem>(subIEnum =>
                    subIEnum != null
                    ? subIEnum.FirstOrDefault(i => subAItemValuePropertyInfo.Getter(i).Equals(equalValue))
                    : default(TSubIItem));

            _whereMatchess.Add(new WhereMatchesContainer
            {
                GetFirstSubAItemFromSubAEnumWhereMatches = getFirstSubAItemFromSubAEnumWhereMatches,
                ValuePropertyName = subAItemValuePropertyInfo.PropertyName,
                ValuePropertyValue = equalValue
            });

            return this;
        }
    }
}
