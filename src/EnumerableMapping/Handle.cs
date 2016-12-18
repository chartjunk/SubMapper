using System.Collections.Generic;

namespace SubMapper.EnumerableMapping
{
    public class FromEnumerableMappingHandle<TA, TB, TSubAEnum, TSubB, TSubAItem> where TSubAEnum : IEnumerable<TSubAItem> { }
    public class ToEnumerableMappingHandle<TA, TB, TSubA, TSubBEnum, TSubBItem> where TSubBEnum : IEnumerable<TSubBItem> { }
    public class EnumerableMappingHandle<TA, TB, TSubAItem, TSubBItem> { }
}
