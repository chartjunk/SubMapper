using System;
using System.Collections.Generic;
using SubMapper.Metadata;
using System.Reflection;

namespace SubMapper
{
    public enum MappingViewType { IisAandJisB, JisAandIisB }

    public partial class SubMap
    {
        public Action<object, object> GetASetB { get; set; }
        public Action<object, object> GetBSetA { get; set; }
        public PropertyInfo APropertyInfo { get; set; }
        public PropertyInfo BPropertyInfo { get; set; }
        public PropertyInfo IPropertyInfo
        {
            get { return MappingViewType == MappingViewType.IisAandJisB ? APropertyInfo : BPropertyInfo; }
            set { if (MappingViewType == MappingViewType.IisAandJisB) APropertyInfo = value; else BPropertyInfo = value; }
        }
        public PropertyInfo JPropertyInfo
        {
            get { return MappingViewType == MappingViewType.IisAandJisB ? BPropertyInfo : APropertyInfo; }
            set { if (MappingViewType == MappingViewType.IisAandJisB) BPropertyInfo = value; else APropertyInfo = value; }
        }

        public bool IsBaseSubMap { get; set; } = false;

        public Action<object, object> GetISetJ => (i, j) =>
        {
            if (MappingViewType == MappingViewType.IisAandJisB)
                GetASetB(i, j);
            else
                GetBSetA(i, j);
        };

        public MappingViewType MappingViewType { get; set; }

        public Lazy<MetaMap> MetaMap { get; set; }

        public static SubMap Reverse(SubMap subMap)
        {
            if (subMap.MappingViewType == MappingViewType.IisAandJisB)
                subMap.MappingViewType = MappingViewType.JisAandIisB;
            else subMap.MappingViewType = MappingViewType.IisAandJisB;
            return subMap;
        }
    }

    public partial class BaseMapping<TA, TB>
    {
        protected List<SubMap> _subMaps { get; set; } = new List<SubMap>();

        protected static SubMap GetAToBIToJFrom(SubMap subMap)
        {
            subMap.MappingViewType = MappingViewType.IisAandJisB;
            return subMap;
        }

        protected static SubMap GetBToAIToJFrom(SubMap subMap)
        {
            subMap.MappingViewType = MappingViewType.JisAandIisB;
            return subMap;
        }
    }
}
