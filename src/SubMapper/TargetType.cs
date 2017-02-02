using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubMapper
{
    class TargetSubType
    {
        public string NutrientKey { get; set; }
        public int NutrientAmount { get; set; }
    }

    class TargetEnumType
    {
        public int TargetEnumInt { get; set; }
        public string TargetEnumString { get; set; }
    }

    class TargetType
    {
        public int TargetInt { get; set; }
        public string TargetString { get; set; }
        public DateTime? TargetDateTime { get; set; }
        public string TargetString2 { get; set; }
        public int TargetInt2 { get; set; }
        public TargetSubType[] TargetSubs { get; set; }
        public string TargetString3 { get; set; }
        public IEnumerable<TargetEnumType> TargetEnum { get; set; }
    }
}
