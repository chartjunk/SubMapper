using System.Collections.Generic;

namespace SubMapper.Metadata
{
    public interface IMetaMapProvider
    {
        IEnumerable<MetaMap> RootMetaMaps { get; }
    }
}
