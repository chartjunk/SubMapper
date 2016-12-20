using System;
using System.Collections.Generic;

namespace SubMapper.UnitTest
{
    class TargetType
    {
        public class TargetSubType
        {
            public class TargetSubEnumerableType
            {
                public string SubEnumerableString1 { get; set; }
            }

            public string SubString1 { get; set; }
            public IEnumerable<TargetSubEnumerableType> SubEnumerable1 { get; set; }
        }

        public class TargetEnumerableType
        {
            public class TargetEnumerableSubType
            {
                public string EnumerableSubString1 { get; set; }
            }

            public string EnumerableString1 { get; set; }
            public int EnumerableInt1 { get; set; }
            public TargetEnumerableSubType EnumerableSub1 { get; set; }
        }

        public int Int1 { get; set; }
        public string String1 { get; set; }
        public DateTime? DateTime1 { get; set; }
        public TargetSubType Sub1 { get; set; }

        public IEnumerable<TargetEnumerableType> Enumerable1 { get; set; }
        public List<TargetEnumerableType> EnumerableList1 { get; set; }
        public TargetEnumerableType[] EnumerableArray1 { get; set; }
    }
}