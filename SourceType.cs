using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubMapper
{
    class SourceSubType
    {
        public SourceSubSubType SourceSubSub { get; set; }
        public string NutrientKey { get; set; }
        public int NutrientAmount { get; set; }
    }

    class SourceSubSubType
    {
        public string SourceString2 { get; set; }
        public int SourceInt2 { get; set; }
    }

    class SourceType
    {
        public int SourceInt { get; set; }
        public string SourceString { get; set; }
        public DateTime? SourceDateTime { get; set; }
        public SourceSubType SourceSub { get; set; }
    }
}
