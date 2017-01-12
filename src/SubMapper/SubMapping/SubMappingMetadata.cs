using SubMapper.Metadata;
using System.Reflection;

namespace SubMapper.SubMapping
{
    public class SubMappingMetadata : IMappingMetadata
    {
        public PropertyInfo SuperAProperty { get; set; }
        public PropertyInfo SubAProperty { get; set; }
        public bool IsSuperAAndSubADifferent { get; set; }

        public PropertyInfo SuperBProperty { get; set; }
        public PropertyInfo SubBProperty { get; set; }
        public bool IsSuperBAndSubBDifferent { get; set; }
    }
}
