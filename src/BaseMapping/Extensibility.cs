using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubMapper
{
    public partial class BaseMapping<TA, TB>
    {
        public ExtensibilityContainer Extensibility { get; private set; }
        public class ExtensibilityContainer
        {
            private BaseMapping<TA, TB> _baseMapping;
            public ExtensibilityContainer(BaseMapping<TA, TB> baseMapping)
            {
                _baseMapping = baseMapping;
            }

            public object DerivedMapping { get; set; }
            public List<SubMap> SubMaps { get { return _baseMapping._subMaps; } set { _baseMapping._subMaps = value; } }
        }

        public BaseMapping<TA, TB> Do() => this;
    }
}
