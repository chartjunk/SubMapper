using SubMapper.Metadata;
using System.Collections.Generic;
using System.Reflection;

namespace SubMapper.EnumerableMapping
{
    public class EnumerablesMappingMetadata : IMappingMetadata
    {
        public PropertyInfo AEnumPropertyInfo { get; set; }
        public PropertyInfo SubAEnumPropertyInfo { get; set; }

        public PropertyInfo BEnumPropertyInfo { get; set; }
        public PropertyInfo SubBEnumPropertyInfo { get; set; }        

        public IEnumerable<WhereEqualsMetadata> WhereAEquals { get; set; }
        public IEnumerable<WhereEqualsMetadata> WhereBEquals { get; set; }

        public MappingViewType MappingViewType { get; set; }
    }
}
