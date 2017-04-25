using System;

namespace SubMapper.Metadata
{
    public class MetaMap
    {
        public object Metadata { get; set; }
        public MetaMap SubMetaMap { get; set; }
    }
}
