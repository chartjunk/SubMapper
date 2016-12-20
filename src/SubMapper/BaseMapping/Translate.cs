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

        private void TranslateIToJ<TI, TJ>(TI I, TJ J, List<IToJSubMap> IToJSubMaps)            
            => IToJSubMaps.ForEach(k => k.SetSubJFromJ(J, k.GetSubIFromI(I)));        
    }
}
