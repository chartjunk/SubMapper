using SubMapper.EnumerableMapping.Adders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SubMapper
{
    class Program
    {
        static void Main(string[] args)
        {
            var testSource = new SourceType
            {
                SourceString = "joku_source",
                SourceSub = new SourceSubType
                {
                    SourceSubSub = new SourceSubSubType
                    {
                        SourceString2 = "toinen_source"
                    }
                },
                SourceSubs = new SourceSubType[]
                {
                    new SourceSubType
                    {
                        NutrientKey = "NN",
                        NutrientAmount = 69,
                        SourceSubSub = new SourceSubSubType
                        {
                            SourceString2 = "lol?"
                        }
                    }
                },
                SourceInt = 911
            };

            var testTarget = new TargetType();
            var testSource2 = new SourceType();
            var myAdder = new Func<IEnumerable<SourceSubType>, SourceSubType, IEnumerable<SourceSubType>>((ac, a) => new[] { a }.Concat(ac ?? new SourceSubType[] { }));

            var mapping = Mapping.FromTo<SourceType, TargetType>()
                .Map(a => a.SourceString, b => b.TargetString)
                .WithSubMapping(a => a.SourceSub, b => b, h => Mapping.Using(h)
                    .WithSubMapping(a => a.SourceSubSub, b => b, h2 => Mapping.Using(h2)
                        .Map(a => a.SourceString2, b => b.TargetString2)
                        .Map(a => a.SourceInt2, b => b.TargetInt2)))

                .WithFromEnumerableMapping(a => a.SourceSubs, b => b, h => Mapping.Using(h)
                    .WithAdder(myAdder)                    
                    .First(a => a.NutrientKey == "NN")
                    .Map(a => a.NutrientAmount, b => b.TargetInt)
                    .WithSubMapping(a => a.SourceSubSub, b => b, h2 => Mapping.Using(h2)
                        .Map(a => a.SourceString2, b => b.TargetString3)))

                .WithToEnumerableMapping(a => a, b => b.TargetSubs, h => Mapping.Using(h)
                    .WithArrayConcatAdder()
                    .First(b => b.NutrientKey == "JJ")
                    .Map(a => a.SourceInt, b => b.NutrientAmount));

            mapping.TranslateAToB(testSource, testTarget);
            mapping.TranslateBToA(testSource2, testTarget);
            var docs = mapping.GetDocumentation();

            var metaMaps = mapping.MetaMaps.ToList();
            var extract = new Metadata.SimpleStringMappingExtractor(metaMaps).Extract();

            Console.WriteLine("==== MAPPING TESTS ====");
            Console.WriteLine($"testTarget.TargetString: {testTarget?.TargetString}");
            Console.WriteLine($"testTarget.TargetString2: {testTarget?.TargetString2}");
            Console.WriteLine($"testTarget.TargetInt: {testTarget?.TargetInt}");
            Console.WriteLine($"testTarget.TargetString3: {testTarget?.TargetString3}");
            Console.WriteLine($"testTarget.TargetSubs[0].NutrientAmount: {testTarget.TargetSubs.ElementAtOrDefault(0)?.NutrientAmount}");
            Console.WriteLine($"testTarget.TargetSubs[0].NutrientKey: {testTarget.TargetSubs.ElementAtOrDefault(0)?.NutrientKey}");
            Console.WriteLine("");
            Console.WriteLine($"testSource2.SourceString: {testSource2.SourceString}");
            Console.WriteLine($"testSource2.SourceSub.SourceSubSub.SourceString2: {testSource2.SourceSub.SourceSubSub.SourceString2}");
            Console.WriteLine($"testSource2.SourceSubs[0].NutrientKey: {testSource2.SourceSubs.ElementAt(0).NutrientKey}");
            Console.WriteLine($"testSource2.SourceSubs[0].NutrientAmount: {testSource2.SourceSubs.ElementAt(0).NutrientAmount}");
            Console.WriteLine($"testSource2.SourceInt: {testSource2.SourceInt}");
            Console.WriteLine("");
            Console.WriteLine("==== DOC TESTS ====");
            //Console.WriteLine(docs);

            Console.ReadKey();
        }
    }
}
