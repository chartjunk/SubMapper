using System;
using System.Collections.Generic;

namespace SubMapper.EnumerableMapping
{

    public abstract partial class PartialEnumerableMapping<TSubA, TSubB, TSubIEnum, TSubJ, TSubIItem>
        : BaseMapping<TSubA, TSubB>
        where TSubIEnum : IEnumerable<TSubIItem>
        where TSubJ : new()
        where TSubIItem : new()
    {
        private Func<TSubIEnum, TSubIItem, TSubIEnum> _getSubIEnumWithAddedSubIItem;

        public PartialEnumerableMapping<TSubA, TSubB, TSubIEnum, TSubJ, TSubIItem> UsingAdder(Func<TSubIEnum, TSubIItem, TSubIEnum> getEnumerableWithAddedItem)
        {
            _getSubIEnumWithAddedSubIItem = getEnumerableWithAddedItem;
            return this;
        }
    }
}
