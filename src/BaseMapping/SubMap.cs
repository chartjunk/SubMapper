using System;
using System.Collections.Generic;

namespace SubMapper
{
    public partial class SubMap
    {
        public Func<object, object> GetSubAFromA { get; set; }
        public Func<object, object> GetSubBFromB { get; set; }

        public Action<object, object> SetSubAFromA { get; set; }
        public Action<object, object> SetSubBFromB { get; set; }

        public string SubAPropertyName { get; set; }
        public string SubBPropertyName { get; set; }

        public static SubMap Rotate(SubMap subMap)
        {
            return new SubMap
            {
                GetSubAFromA = subMap.GetSubBFromB,
                GetSubBFromB = subMap.GetSubAFromA,

                SetSubAFromA = subMap.SetSubBFromB,
                SetSubBFromB = subMap.SetSubAFromA,

                SubAPropertyName = subMap.SubBPropertyName,
                SubBPropertyName = subMap.SubAPropertyName
            };
        }
    }

    public partial class BaseMapping<TA, TB>
    {
        protected class IToJSubMap
        {
            public Func<object, object> GetSubIFromI { get; set; }
            public Action<object, object> SetSubJFromJ { get; set; }

            public static SubMap ToPartialAToBSubMap(IToJSubMap iToJSubMap)
            {
                return new SubMap
                {
                    GetSubAFromA = iToJSubMap.GetSubIFromI,
                    SetSubBFromB = iToJSubMap.SetSubJFromJ
                };
            }
        }

        protected List<SubMap> _subMaps { get; set; } = new List<SubMap>();

        protected static IToJSubMap GetAToBIToJFrom(SubMap subMap)
        {
            return new IToJSubMap
            {
                GetSubIFromI = subMap.GetSubAFromA,
                SetSubJFromJ = subMap.SetSubBFromB
            };
        }

        protected static IToJSubMap GetBToAIToJFrom(SubMap subMap)
        {
            return new IToJSubMap
            {
                GetSubIFromI = subMap.GetSubBFromB,
                SetSubJFromJ = subMap.SetSubAFromA
            };
        }
    }
}
