using SubMapper.Metadata;
using System.Reflection;

namespace SubMapper.BaseMapping
{
    public class BaseMappingMetadata : IMappingMetadata
    {
        public PropertyInfo SuperAProperty { get; set; }
        //public PropertyInfo SubAProperty { get; set; }

        public PropertyInfo SuperBProperty { get; set; }
        //public PropertyInfo SubBProperty { get; set; }

        public MetaMap SubMetaMap { get; set; }
    }
}
