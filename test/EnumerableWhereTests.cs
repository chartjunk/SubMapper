using Microsoft.VisualStudio.TestTools.UnitTesting;
using SubMapper.EnumerableMapping.Adders;
using System.Linq;

namespace SubMapper.UnitTest
{
    [TestClass]
    public class EnumerableWhereTests
    {
        [TestMethod]
        public void MapUsingMultipleWhere()
        {
            var mapping = Mapping.FromTo<SourceType, TargetType>()
                .FromEnum(s => s.Enumerable1, t => t, h => Mapping.Using(h)
                    .UsingArrayConcatAdder()
                    .First(s => s.EnumerableInt1 == 1 && (s.EnumerableString1 == "Enumerable1 String1"))
                    .Sub(s => s.EnumerableSub1, t => t, h2 => Mapping.Using(h2)
                        .Map(s => s.EnumerableSubString1, t => t.String1)));

            var si = SourceType.GetTestInstance();
            var ti = new TargetType();
            var si2 = new SourceType();

            mapping.TranslateAToB(si, ti);
            mapping.TranslateBToA(si2, ti);

            Assert.AreEqual(si.Enumerable1.First(i => i.EnumerableInt1 == 1 && i.EnumerableString1 == "Enumerable1 String1").EnumerableSub1.EnumerableSubString1, ti.String1);
            Assert.AreEqual(si2.Enumerable1.First(i => i.EnumerableInt1 == 1 && i.EnumerableString1 == "Enumerable1 String1").EnumerableSub1.EnumerableSubString1, ti.String1);
        }
    }
}
