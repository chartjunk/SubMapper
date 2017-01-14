using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubMapper.Metadata
{
    public static class SimpleStringExtraction
    {        
        public static IEnumerable<string> ExtractFrom(IMetaMapProvider metaMapProvider)
        {
            var result = new List<string>();
            foreach (var map in metaMapProvider.RootMetaMaps)
            {
                var currentMap = map;
                var currentAString = "";
                var currentBString = "";

                while (currentMap != null)
                {
                    if (currentMap.MetadataType.Equals(typeof(BaseMapping.BaseMappingMetadata)))
                    {
                        var m = ((BaseMapping.BaseMappingMetadata)currentMap.Metadata);
                        currentAString += "." + m.APropertyInfo.Name;
                        currentBString += "." + m.BPropertyInfo.Name;
                    }
                    else if (currentMap.MetadataType.Equals(typeof(SubMapping.SubMappingMetadata)))
                    {
                        var m = ((SubMapping.SubMappingMetadata)currentMap.Metadata);
                        if (m.IsAAndSubADifferent) currentAString += "." + m.APropertyInfo.Name;
                        if (m.IsBAndSubBDifferent) currentBString += "." + m.BPropertyInfo.Name;
                    }
                    else if (currentMap.MetadataType.Equals(typeof(EnumerableMapping.PartialEnumerableMappingMetadata)))
                    {
                        var m = ((EnumerableMapping.PartialEnumerableMappingMetadata)currentMap.Metadata);
                        var whereEquals = string.Join(" AND ", m.WhereEquals.Select(i => i.PropertyInfo.Name + "=" + Convert.ToString(i.EqualValue) ?? ""));
                        if (m.MappingViewType == MappingViewType.JisAandIisB)
                        {
                            currentBString += "." + m.IEnumPropertyInfo.Name + $"[{whereEquals}]";
                            if (m.IsJAndSubJDifferent)
                                currentAString += "." + m.JPropertyInfo.Name;
                        }
                        else
                        {
                            currentAString += "." + m.IEnumPropertyInfo.Name + $"[{whereEquals}]";
                            if (m.IsJAndSubJDifferent)
                                currentBString += "." + m.JPropertyInfo.Name;
                        }
                    }

                    currentMap = currentMap.SubMetaMap;
                }

                result.Add(currentAString + " -> " + currentBString);
            }

            return result;
        }
    }
}
