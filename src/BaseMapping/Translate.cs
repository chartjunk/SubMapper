using System.Collections.Generic;
using System.Linq;

namespace SubMapper
{
    public partial class BaseMapping<TA, TB>
    {
        public void TranslateAToB(TA A, TB B)
            => TranslateIToJ(A, B, _subMaps.Select(GetAToBIToJFrom));

        public void TranslateBToA(TA A, TB B)
            => TranslateIToJ(B, A, _subMaps.Select(GetBToAIToJFrom));

        private void TranslateIToJ<TI, TJ>(TI I, TJ J, IEnumerable<IToJSubMap> IToJSubMaps)
        {
            var IToJSubMapsList = IToJSubMaps.ToList();
            IToJSubMapsList.ForEach(k => k.SetSubJFromJ(J, k.GetSubIFromI(I)));
        }
    }
}
