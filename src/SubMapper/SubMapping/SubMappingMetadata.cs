using SubMapper.Metadata;
using System.Reflection;

namespace SubMapper.SubMapping
{
    public class SubMappingMetadata : IMappingMetadata
    {
        public PropertyInfo APropertyInfo { get; set; }
        public PropertyInfo SubAPropertyInfo { get; set; }
        public bool IsAAndSubADifferent { get; set; }

        public PropertyInfo BPropertyInfo { get; set; }
        public PropertyInfo SubBPropertyInfo { get; set; }
        public bool IsBAndSubBDifferent { get; set; }
    }
}
