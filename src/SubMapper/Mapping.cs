﻿using System;
using System.Collections.Generic;
using SubMapper.SubMapping;
using SubMapper.EnumerableMapping;
using System.Collections;

namespace SubMapper
{
    public static partial class Mapping
    {
        public static BaseMapping<TA, TB> FromTo<TA, TB>()
            => new BaseMapping<TA, TB>();

        public static SubMapping<TA, TB, TSubA, TSubB>
            Using<TA, TB, TSubA, TSubB>(
            SubMappingHandle<TA, TB, TSubA, TSubB> subMappingHandle)
            where TSubA : new()
            where TSubB : new()
            => new SubMapping<TA, TB, TSubA, TSubB>();

        public static FromEnumerableMapping<TA, TB, IEnumerable<TSubAItem>, TSubB, TSubAItem> 
            Using<TA, TB, TSubB, TSubAItem>(
            FromEnumerableMappingHandle<TA, TB, IEnumerable<TSubAItem>, TSubB, TSubAItem> fromEnumerableMappingHandle)
            where TSubAItem : new()
            where TSubB : new()
            => new FromEnumerableMapping<TA, TB, IEnumerable<TSubAItem>, TSubB, TSubAItem>();

        public static ToEnumerableMapping<TA, TB, TSubA, IEnumerable<TSubBItem>, TSubBItem>
            Using<TA, TB, TSubA, TSubBItem>(
            ToEnumerableMappingHandle<TA, TB, TSubA, IEnumerable<TSubBItem>, TSubBItem> toEnumerableMappingHandle)
            where TSubA : new()
            where TSubBItem : new()
            => new ToEnumerableMapping<TA, TB, TSubA, IEnumerable<TSubBItem>, TSubBItem>();
    }
}