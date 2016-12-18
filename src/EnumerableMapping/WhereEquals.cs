using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        private List<WhereMatchesContainer> _whereMatchess;

        private class WhereMatchesContainer
        {
            public Func<TSubAEnum, TSubAItem> GetFirstSubAItemFromSubAEnumWhereMatches { get; internal set; }
        }

        public FromEnumerableMapping<TA, TB, TSubAEnum, TSubB, TSubAItem> FirstWhereEquals<TValue>(
            Expression<Func<TSubAItem, TValue>> getValue, TValue equalValue)
        {
            var subAItemValuePropertyInfo = getValue.GetMapPropertyInfo();
            var getFirstSubAItemFromSubAEnumWhereMatches =
                new Func<TSubAEnum, TSubAItem>(subAEnum =>
                    subAEnum != null
                    ? subAEnum.FirstOrDefault(i => subAItemValuePropertyInfo.Getter(i).Equals(equalValue))
                    : default(TSubAItem));

            _whereMatchess.Add(new WhereMatchesContainer
            {
                GetFirstSubAItemFromSubAEnumWhereMatches = getFirstSubAItemFromSubAEnumWhereMatches
            });

            return this;
        }
    }
}
