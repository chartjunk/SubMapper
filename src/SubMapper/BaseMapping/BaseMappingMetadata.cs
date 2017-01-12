using SubMapper.Metadata;
using System.Reflection;

namespace SubMapper.BaseMapping
{
    public class BaseMappingMetadata : IMappingMetadata
    {
        public PropertyInfo APropertyInfo { get; set; }
        public PropertyInfo BPropertyInfo { get; set; }
    }
}
