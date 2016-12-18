using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubMapper.EnumerableMapping
{
    public abstract partial class PartialEnumerableMapping<TSubA, TSubB, TSubIEnum, TSubJ, TSubIItem>
        : BaseMapping<TSubA, TSubB>
        where TSubIEnum : IEnumerable<TSubIItem>
        where TSubJ : new()
        where TSubIItem : new()
    {
        private Func<TSubIEnum, TSubIItem, TSubIEnum> _getTSubAEnumWithAddedTSubAItem;

        public PartialEnumerableMapping<TSubA, TSubB, TSubIEnum, TSubJ, TSubIItem> WithAdder(Func<TSubIEnum, TSubIItem, TSubIEnum> getEnumerableWithAddedItem)
        {
            _getTSubAEnumWithAddedTSubAItem = getEnumerableWithAddedItem;
            return this;
        }
    }
}
