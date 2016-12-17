using System;
using System.Collections.Generic;
using SubMapper.SubMapping;

namespace SubMapper
{
    public static partial class Mapping
    {
        public static BaseMapping<TA, TB> FromTo<TA, TB>()
            => new BaseMapping<TA, TB>();

        public static SubMapping<TA, TB, TSubA, TSubB> Using<TA, TB, TSubA, TSubB>(SubMappingHandle<TA, TB, TSubA, TSubB> subMappingHandle)
            where TSubA : new()
            where TSubB : new()
            => new SubMapping<TA, TB, TSubA, TSubB>();
    }
}
