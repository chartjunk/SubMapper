using SubMapper.Metadata;
using System.Collections.Generic;
using System.Reflection;

namespace SubMapper.EnumerableMapping
{
    public class WhereEqualsMetadata
    {
        public PropertyInfo PropertyInfo { get; set; }
        public object EqualValue { get; set; }
    }

    public class PartialEnumerableMappingMetadata : IMappingMetadata
    {
        public PropertyInfo IEnumPropertyInfo { get; set; }
        public PropertyInfo SubIEnumPropertyInfo { get; set; }

        public PropertyInfo JPropertyInfo { get; set; }
        public PropertyInfo SubJPropertyInfo { get; set; }
        public bool IsJAndSubJDifferent { get; set; }

        public IEnumerable<WhereEqualsMetadata> WhereEquals { get; set; }

        public MappingViewType MappingViewType { get; set; }
    }
}
