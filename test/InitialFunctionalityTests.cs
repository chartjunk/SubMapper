using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace SubMapper.UnitTest
{
    [TestClass]
    public class InitialFunctionalityTests
    {
        [TestMethod]
        public void MapDifferentBaseTypes()
        {
            var mapping = Mapping.FromTo<SourceType, TargetType>()
                .Map(s => s.DateTime1, t => t.DateTime1)
                .Map(s => s.Int1, t => t.Int1)
                .Map(s => s.String1, t => t.String1);

            var si = SourceType.GetTestInstance();
            var ti = new TargetType();
            var si2 = new SourceType();

            mapping.TranslateAToB(si, ti);
            mapping.TranslateBToA(si2, ti);

            Assert.AreEqual(si.DateTime1, ti.DateTime1);
            Assert.AreEqual(si.Int1, ti.Int1);
            Assert.AreEqual(si.String1, ti.String1);

            Assert.AreEqual(si.DateTime1, si2.DateTime1);
            Assert.AreEqual(si.Int1, si2.Int1);
            Assert.AreEqual(si.String1, si2.String1);
        }

        [TestMethod]
        public void MapSubType()
        {
            var mapping = Mapping.FromTo<SourceType, TargetType>()
                .WithSubMapping(s => s.Sub1, t => t.Sub1, h => Mapping.Using(h)
                    .Map(s => s.SubString1, t => t.SubString1));

            var si = SourceType.GetTestInstance();
            var ti = new TargetType();
            var si2 = new SourceType();

            mapping.TranslateAToB(si, ti);
            mapping.TranslateBToA(si2, ti);

            Assert.AreEqual(si.Sub1.SubString1, ti.Sub1.SubString1);
            Assert.AreEqual(si.Sub1.SubString1, si2.Sub1.SubString1);
        }

        [TestMethod]
        public void MapFromEnumerable()
        {
            var mapping = Mapping.FromTo<SourceType, TargetType>()
                .WithFromEnumerableMapping(s => s.Enumerable1, t => t, h => Mapping.Using(h)
                    .WithAdder((c, s) => (c??new SourceType.SourceEnumerableType [] {  }).Concat(new [] {s}))
                    .FirstWhereEquals(s => s.EnumerableInt1, 1)
                    .Map(s => s.EnumerableString1, t => t.String1));

            var si = SourceType.GetTestInstance();
            var ti = new TargetType();
            var si2 = new SourceType();

            mapping.TranslateAToB(si, ti);
            mapping.TranslateBToA(si2, ti);

            Assert.AreEqual(si.Enumerable1.First(i => i.EnumerableInt1 == 1).EnumerableString1, ti.String1);
            Assert.AreEqual(si2.Enumerable1.First(i => i.EnumerableInt1 == 1).EnumerableString1, ti.String1);            
        }

        [TestMethod]
        public void MapToEnumerable()
        {
            var mapping = Mapping.FromTo<SourceType, TargetType>()
                .WithToEnumerableMapping(s => s, t => t.Enumerable1, h => Mapping.Using(h)
                    .WithAdder((c, t) => (c ?? new TargetType.TargetEnumerableType[] { }).Concat(new[] { t }))
                    .FirstWhereEquals(t => t.EnumerableInt1, 1)
                    .Map(s => s.String1, t => t.EnumerableString1));

            var si = SourceType.GetTestInstance();
            var ti = new TargetType();
            var si2 = new SourceType();

            mapping.TranslateAToB(si, ti);
            mapping.TranslateBToA(si2, ti);

            Assert.AreEqual(si.String1, ti.Enumerable1.First(i => i.EnumerableInt1 == 1).EnumerableString1);
            Assert.AreEqual(si2.String1, ti.Enumerable1.First(i => i.EnumerableInt1 == 1).EnumerableString1);
        }

        [TestMethod]
        public void MapFromEnumerableSubType()
        {
            var mapping = Mapping.FromTo<SourceType, TargetType>()
                .WithFromEnumerableMapping(s => s.Enumerable1, t => t, h => Mapping.Using(h)
                    .WithAdder((c, t) => (c ?? new SourceType.SourceEnumerableType[] { }).Concat(new[] { t }))
                    .FirstWhereEquals(t => t.EnumerableInt1, 1)
                    .WithSubMapping(s => s.EnumerableSub1, t => t, h2 => Mapping.Using(h2)
                        .Map(s => s.EnumerableSubString1, t => t.String1)));

            var si = SourceType.GetTestInstance();
            var ti = new TargetType();
            var si2 = new SourceType();

            mapping.TranslateAToB(si, ti);
            mapping.TranslateBToA(si2, ti);

            Assert.AreEqual(si.Enumerable1.First(i => i.EnumerableInt1 == 1).EnumerableSub1.EnumerableSubString1, ti.String1);
            Assert.AreEqual(si2.Enumerable1.First(i => i.EnumerableInt1 == 1).EnumerableSub1.EnumerableSubString1, ti.String1);
        }
    }
}
