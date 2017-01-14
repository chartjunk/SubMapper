using SubMapper.Metadata;
using System.Collections.Generic;
using System.Linq;

namespace SubMapper
{
    public partial class BaseMapping<TA, TB> : IMetaMapProvider
    {
        public IEnumerable<MetaMap> RootMetaMaps => _subMaps.Select(s => s.MetaMap.Value);

        public BaseMapping()
        {
            Extensibility = new ExtensibilityContainer(this);
        }
    }
}
