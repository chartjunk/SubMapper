using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubMapper.EnumerableMapping
{
    public partial class FromEnumerableMapping<TA, TB, TSubAEnum, TSubB, TSubAItem>
        : BaseMapping<TSubAItem, TSubB>
        where TSubB : new()
        where TSubAItem : new()
        where TSubAEnum : IEnumerable<TSubAItem>
    {
        private Func<TSubAEnum, TSubAItem, TSubAEnum> _getTSubAEnumWithAddedTSubAItem;

        public FromEnumerableMapping<TA, TB, TSubAEnum, TSubB, TSubAItem> WithAdder(Func<TSubAEnum, TSubAItem, TSubAEnum> getEnumerableWithAddedItem)
        {
            _getTSubAEnumWithAddedTSubAItem = getEnumerableWithAddedItem;
            return this;
        }
    }
}
