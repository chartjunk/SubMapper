using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubMapper
{
    public partial class BaseMapping<TA, TB>
    {
        public BaseMapping()
        {
            Extensibility = new ExtensibilityContainer(this);
        }
    }
}
