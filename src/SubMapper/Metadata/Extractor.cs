using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubMapper.Metadata
{
    public class Extractor
    {
        private readonly List<MetaMap> _rootMetaMaps;

        public Extractor(List<MetaMap> rootMetaMaps)
        {
            _rootMetaMaps = rootMetaMaps;
        }

        public List<string> Extract()
        {
            var result = new List<string>();
            foreach (var map in _rootMetaMaps)
            {
                var currentMap = map;
                var currentAString = "";
                var currentBString = "";

                while (currentMap != null)
                {
                    if (currentMap.MetadataType.Equals(typeof(BaseMapping.BaseMappingMetadata)))
                    {
                        var m = ((BaseMapping.BaseMappingMetadata)currentMap.Metadata);
                        currentAString += "." + m.SuperAProperty.Name;
                        currentBString += "." + m.SuperBProperty.Name;
                    }
                    else if (currentMap.MetadataType.Equals(typeof(SubMapping.SubMappingMetadata)))
                    {
                        var m = ((SubMapping.SubMappingMetadata)currentMap.Metadata);
                        if (m.IsSuperAAndSubADifferent) currentAString += "." + m.SuperAProperty.Name;
                        if (m.IsSuperBAndSubBDifferent) currentBString += "." + m.SuperBProperty.Name;
                    }
                    else if (currentMap.MetadataType.Equals(typeof(EnumerableMapping.PartialEnumerableMappingMetadata)))
                    {
                        var m = ((EnumerableMapping.PartialEnumerableMappingMetadata)currentMap.Metadata);
                        if (m.IsRotated)
                        {
                            currentBString += "." + m.SuperIEnumProperty.Name + "[]." + m.SubIEnumProperty.Name;
                            if (m.IsSuperJAndSubJDifferent)
                                currentAString += "." + m.SuperJProperty.Name;
                        }
                        else
                        {
                            currentAString += "." + m.SuperIEnumProperty.Name + "[]." + m.SubIEnumProperty.Name;
                            if (m.IsSuperJAndSubJDifferent)
                                currentBString += "." + m.SuperJProperty.Name;
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
