using SubMapper.Metadata;
using System.Reflection;

namespace SubMapper.BaseMapping
{
    public class BaseMappingMetadata : IMappingMetadata
    {
        public PropertyInfo SuperAProperty { get; set; }        
        public PropertyInfo SuperBProperty { get; set; }
    }
}
