using SubMapper.Metadata;
using System.Reflection;

namespace SubMapper.EnumerableMapping
{
    public class PartialEnumerableMappingMetadata : IMappingMetadata
    {
        public PropertyInfo SuperIEnumProperty { get; set; }
        public PropertyInfo SubIEnumProperty { get; set; }

        public PropertyInfo SuperJProperty { get; set; }
        public PropertyInfo SubJProperty { get; set; }
        public bool IsSuperJAndSubJDifferent { get; set; }

        public MetaMap SubMetaMap { get; set; }
    }
}
