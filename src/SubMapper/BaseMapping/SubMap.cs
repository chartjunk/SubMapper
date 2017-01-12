using System;
using System.Collections.Generic;
using SubMapper.Metadata;
using System.Reflection;

namespace SubMapper
{
    public class HalfSubMap
    {
        public Func<object, object> GetSubFrom { get; set; }
        public Action<object, object> SetSubFrom { get; set; }
        public PropertyInfo PropertyInfo { get; set; }
    }

    public enum SubMapViewType { IisAandJisB, JisAandIisB }

    public class HalfSubMapPair
    {
        public SubMapViewType SubMapViewType { get; set; } = SubMapViewType.IisAandJisB;

        public HalfSubMap IHalfSubMap
        {
            get { return SubMapViewType == SubMapViewType.IisAandJisB ? AHalfSubMap : BHalfSubMap; }
            set { if (SubMapViewType == SubMapViewType.IisAandJisB) AHalfSubMap = value; else BHalfSubMap = value; }
        }

        public HalfSubMap JHalfSubMap
        {
            get { return SubMapViewType == SubMapViewType.IisAandJisB ? BHalfSubMap : AHalfSubMap; }
            set { if (SubMapViewType == SubMapViewType.IisAandJisB) BHalfSubMap = value; else AHalfSubMap = value; }
        }

        public HalfSubMap AHalfSubMap { get; set; }
        public HalfSubMap BHalfSubMap { get; set; }
    }

    public partial class SubMap
    {
        public HalfSubMapPair HalfSubMapPair { get; set; }
        public Lazy<MetaMap> MetaMap { get; set; }
        public static SubMap Reverse(SubMap subMap)
        {
            if (subMap.HalfSubMapPair.SubMapViewType == SubMapViewType.IisAandJisB)
                subMap.HalfSubMapPair.SubMapViewType = SubMapViewType.JisAandIisB;
            else subMap.HalfSubMapPair.SubMapViewType = SubMapViewType.IisAandJisB;
            return subMap;
        }
    }

    public partial class BaseMapping<TA, TB>
    {
        protected List<SubMap> _subMaps { get; set; } = new List<SubMap>();

        protected static SubMap GetAToBIToJFrom(SubMap subMap)
        {
            subMap.HalfSubMapPair.SubMapViewType = SubMapViewType.IisAandJisB;
            return subMap;
        }

        protected static SubMap GetBToAIToJFrom(SubMap subMap)
        {
            subMap.HalfSubMapPair.SubMapViewType = SubMapViewType.JisAandIisB;
            return subMap;
        }
    }
}
