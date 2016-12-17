using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubMapper
{
    public partial class BaseMapping<TA, TB>
    {
        public string GetDocumentation()
            => string.Join(Environment.NewLine, _subMaps.Select(s => s.SubAPropertyName + " MAPS TO " + s.SubBPropertyName));
    }
}
