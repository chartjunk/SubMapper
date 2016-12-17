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
    }

    public partial class BaseMapping<TA, TB>
    {
        private class IToJSubMap
        {
            public Func<object, object> GetSubIFromI { get; set; }
            public Action<object, object> SetSubJFromJ { get; set; }
        }

        protected List<SubMap> _subMaps { get; set; } = new List<SubMap>();

        private IToJSubMap GetAToBIToJFrom(SubMap subMap)
        {
            return new IToJSubMap
            {
                GetSubIFromI = subMap.GetSubAFromA,
                SetSubJFromJ = subMap.SetSubBFromB
            };
        }

        private IToJSubMap GetBToAIToJFrom(SubMap subMap)
        {
            return new IToJSubMap
            {
                GetSubIFromI = subMap.GetSubBFromB,
                SetSubJFromJ = subMap.SetSubAFromA
            };
        }
    }
}
