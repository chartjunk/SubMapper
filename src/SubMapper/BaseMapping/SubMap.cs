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

    public enum MappingViewType { IisAandJisB, JisAandIisB }

    public class HalfSubMapPair
    {
        public MappingViewType SubMapViewType { get; set; } = MappingViewType.IisAandJisB;

        public HalfSubMap IHalfSubMap
        {
            get { return SubMapViewType == MappingViewType.IisAandJisB ? AHalfSubMap : BHalfSubMap; }
            set { if (SubMapViewType == MappingViewType.IisAandJisB) AHalfSubMap = value; else BHalfSubMap = value; }
        }

        public HalfSubMap JHalfSubMap
        {
            get { return SubMapViewType == MappingViewType.IisAandJisB ? BHalfSubMap : AHalfSubMap; }
            set { if (SubMapViewType == MappingViewType.IisAandJisB) BHalfSubMap = value; else AHalfSubMap = value; }
        }

        public HalfSubMap AHalfSubMap { get; set; }
        public HalfSubMap BHalfSubMap { get; set; }
    }

    public partial class SubMap
    {
        /// <summary>
        /// TODO: remove
        /// </summary>
        //public HalfSubMapPair HalfSubMapPair { get; set; }
        public Action<object, object> GetASetB { get; set; }
        public Action<object, object> GetBSetA { get; set; }
        public void GetISetJ(object i, object j)
        {
            if (MappingViewType == MappingViewType.IisAandJisB)
                GetASetB(i, j);
            else
                GetBSetA(i, j);
        }
        public MappingViewType MappingViewType { get; set; }

        public Lazy<MetaMap> MetaMap { get; set; }

        public static SubMap Reverse(SubMap subMap)
        {
            if (subMap.MappingViewType == MappingViewType.IisAandJisB)
                subMap.MappingViewType = MappingViewType.JisAandIisB;
            else subMap.MappingViewType = MappingViewType.IisAandJisB;
            return subMap;

            //if (subMap.HalfSubMapPair.SubMapViewType == MappingViewType.IisAandJisB)
            //    subMap.HalfSubMapPair.SubMapViewType = MappingViewType.JisAandIisB;
            //else subMap.HalfSubMapPair.SubMapViewType = MappingViewType.IisAandJisB;
            //return subMap;
        }
    }

    public partial class BaseMapping<TA, TB>
    {
        protected List<SubMap> _subMaps { get; set; } = new List<SubMap>();

        //protected static SubMap GetAToBIToJFrom(SubMap subMap)
        //{
        //    subMap.HalfSubMapPair.SubMapViewType = MappingViewType.IisAandJisB;
        //    return subMap;
        //}

        //protected static SubMap GetBToAIToJFrom(SubMap subMap)
        //{
        //    subMap.HalfSubMapPair.SubMapViewType = MappingViewType.JisAandIisB;
        //    return subMap;
        //}

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
