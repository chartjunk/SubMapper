using System.Collections.Generic;
using System.Linq;

namespace SubMapper
{
    public partial class BaseMapping<TA, TB>
    {
        public void TranslateAToB(TA A, TB B)
            => TranslateIToJ(A, B, _subMaps.Select(GetAToBIToJFrom).ToList());

        public void TranslateBToA(TA A, TB B)
            => TranslateIToJ(B, A, _subMaps.Select(GetBToAIToJFrom).ToList());

        private void TranslateIToJ<TI, TJ>(TI I, TJ J, List<SubMap> IToJSubMaps)
        {
            foreach (var k in IToJSubMaps)
            {
                k.HalfSubMapPair.JHalfSubMap.SetSubFrom(J, k.HalfSubMapPair.IHalfSubMap.GetSubFrom(I));
            }
        }
    }
}
