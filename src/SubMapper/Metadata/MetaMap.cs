using System;

namespace SubMapper.Metadata
{
    public class MetaMap
    {
        public Type MetadataType { get; set; }
        public object Metadata { get; set; }
        public MetaMap SubMetaMap { get; set; }
    }
}
