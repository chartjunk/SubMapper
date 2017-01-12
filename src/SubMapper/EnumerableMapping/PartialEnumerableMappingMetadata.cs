using SubMapper.Metadata;
using System.Reflection;

namespace SubMapper.EnumerableMapping
{
    public class PartialEnumerableMappingMetadata : IMappingMetadata
    {
        public PropertyInfo IEnumPropertyInfo { get; set; }
        public PropertyInfo SubIEnumPropertyInfo { get; set; }

        public PropertyInfo JPropertyInfo { get; set; }
        public PropertyInfo SubJPropertyInfo { get; set; }
        public bool IsJAndSubJDifferent { get; set; }

        public bool IsRotated { get; set; }
    }
}
