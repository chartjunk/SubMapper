using System;
using System.Collections.Generic;
using System.Linq;

namespace SubMapper.UnitTest
{
    class SourceType
    {
        public class SourceSubType
        {
            public class SourceSubEnumerableType
            {
                public string SubEnumerableString1 { get; set; }                
            }
                        
            public string SubString1 { get; set; }
            public IEnumerable<SourceSubEnumerableType> SubEnumerable1 { get; set; }
        }

        public class SourceEnumerableType
        {
            public class SourceEnumerableSubType
            {
                public string EnumerableSubString1 { get; set; }
            }

            public string EnumerableString1 { get; set; }
            public int EnumerableInt1 { get; set; }
            public int EnumerableInt2 { get; set; }
            public SourceEnumerableSubType EnumerableSub1 { get; set; }
        }

        public int Int1 { get; set; }
        public string String1 { get; set; }
        public DateTime? DateTime1 { get; set; }
        public SourceSubType Sub1 { get; set; }

        public IEnumerable<SourceEnumerableType> Enumerable1 { get; set; }
        public IEnumerable<SourceEnumerableType> Enumerable2 { get; set; }
        public List<SourceEnumerableType> EnumerableList1 { get; set; }
        public SourceEnumerableType[] EnumerableArray1 { get; set; }

        public static SourceType GetTestInstance()
        {
            var sourceSub = new SourceSubType
            {
                SubString1 = "Sub1 String1",
                SubEnumerable1 = new[]
                {
                    new SourceSubType.SourceSubEnumerableType
                    {
                        SubEnumerableString1 = "Sub1 Enumerable1 String1"
                    },
                    new SourceSubType.SourceSubEnumerableType
                    {
                        SubEnumerableString1 = "Sub1 Enumerable2 String2"
                    }
                }
            };

            var sourceEnumerable1 = new[]
            {
                new SourceEnumerableType
                {
                    EnumerableString1 = "Enumerable1 String1",
                    EnumerableInt1 = 1,
                    EnumerableSub1 = new SourceEnumerableType.SourceEnumerableSubType
                    {
                        EnumerableSubString1 = "Enumerable1 Sub1 String1"
                    }
                },
                new SourceEnumerableType
                {
                    EnumerableString1 = "Enumerable1 String2",
                    EnumerableInt1 = 2,
                    EnumerableSub1 = null
                }
            };

            var sourceEnumerable2 = new[]
            {
                new SourceEnumerableType
                {
                    EnumerableString1 = "A",
                    EnumerableInt1 = 111,
                    EnumerableInt2 = 222,
                },
                new SourceEnumerableType
                {
                    EnumerableString1 = "A",
                    EnumerableInt1 = 333,
                    EnumerableInt2 = 444,
                },
                new SourceEnumerableType
                {
                    EnumerableString1 = "B",
                    EnumerableInt1 = 555,
                    EnumerableInt2 = 666,
                },
                new SourceEnumerableType
                {
                    EnumerableString1 = "C",
                    EnumerableInt1 = 777,
                    EnumerableInt2 = 888,
                }
            };

            var result = new SourceType
            {
                DateTime1 = DateTime.MinValue,
                String1 = "String1",
                Int1 = 1,

                Sub1 = sourceSub,
                Enumerable1 = sourceEnumerable1,
                Enumerable2 = sourceEnumerable2,
                EnumerableArray1 = sourceEnumerable1,
                EnumerableList1 = sourceEnumerable1.ToList()
            };

            return result;
        }
    }
}
