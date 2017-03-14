using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Linq.Expressions;
using System;
using System.Collections.Generic;

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
                .Sub(s => s.Sub1, t => t.Sub1, subMapping => subMapping
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
                .FromEnum(s => s.Enumerable1, t => t, fromEnumMapping => fromEnumMapping
                    .UsingAdder((c, s) => (c??new SourceType.SourceEnumerableType [] {  }).Concat(new [] {s}))
                    .First(s => s.EnumerableInt1 == 1)
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
                .ToEnum(s => s, t => t.Enumerable1, toEnumMapping => toEnumMapping
                    .UsingAdder((c, t) => (c ?? new TargetType.TargetEnumerableType[] { }).Concat(new[] { t }))
                    .First(t => t.EnumerableInt1 == 1)
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
        public void MapEnumerables()
        {
            var mapping = Mapping.FromTo<SourceType, TargetType>()
                .Enums(s => s.Enumerable1, t => t.Enumerable1, enumsMapping => enumsMapping
                    .UsingAdder(
                        (c, s) => (c ?? new SourceType.SourceEnumerableType[] { }).Concat(new[] { s }),
                        (c, t) => (c ?? new TargetType.TargetEnumerableType[] { }).Concat(new[] { t }))
                    .Map(s => s.EnumerableInt1, t => t.EnumerableInt1)
                    .Map(s => s.EnumerableString1, t => t.EnumerableString1)
                    .Sub(s => s.EnumerableSub1, t => t.EnumerableSub1, subMapping => subMapping
                        .Map(s => s.EnumerableSubString1, t => t.EnumerableSubString1)));

            var si = SourceType.GetTestInstance();
            var ti = new TargetType();
            var si2 = new SourceType();

            mapping.TranslateAToB(si, ti);
            mapping.TranslateBToA(si2, ti);

            var siFirst = si.Enumerable1.ElementAt(0);
            var siSecond = si.Enumerable1.ElementAt(1);
            var tiFirst = ti.Enumerable1.ElementAt(0);
            var tiSecond = ti.Enumerable1.ElementAt(1);
            var si2First = si2.Enumerable1.ElementAt(0);
            var si2Second = si2.Enumerable1.ElementAt(1);

            var assertEnumerablesEqual = new Action<SourceType.SourceEnumerableType, TargetType.TargetEnumerableType>((s, t) =>
            {
                Assert.AreEqual(s.EnumerableInt1, t.EnumerableInt1);
                Assert.AreEqual(s.EnumerableString1, t.EnumerableString1);
                var sSub = s.EnumerableSub1;
                var tSub = t.EnumerableSub1;
                if(sSub != null || tSub != null)
                    Assert.AreEqual(sSub.EnumerableSubString1, tSub.EnumerableSubString1);
                // else both of them are null -> are equal
            });

            foreach(var p in new[]
            {
                Tuple.Create(siFirst, tiFirst),
                Tuple.Create(siSecond, tiSecond),
                Tuple.Create(si2First, tiFirst),
                Tuple.Create(si2Second, tiSecond)
            })
                assertEnumerablesEqual(p.Item1, p.Item2);
        }

        [TestMethod]
        public void MapFromEnumerableSubType()
        {
            var mapping = Mapping.FromTo<SourceType, TargetType>()
                .FromEnum(s => s.Enumerable1, t => t, fromEnumMapping => fromEnumMapping
                    .UsingAdder((c, t) => (c ?? new SourceType.SourceEnumerableType[] { }).Concat(new[] { t }))
                    .First(s => s.EnumerableInt1 == 1)
                    .Sub(s => s.EnumerableSub1, t => t, subMapping => subMapping
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
