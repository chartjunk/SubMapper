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
                SourceInt = 911,
                SourceEnum = new []
                {
                    new SourceEnumType
                    {
                        SourceEnumInt = 5,
                        SourceEnumString = "viitonen"
                    },
                    new SourceEnumType
                    {
                        SourceEnumInt = 6,
                        SourceEnumString = "kuutonen"
                    }
                }
            };

            var testTarget = new TargetType();
            var testSource2 = new SourceType();

            var mapping = Mapping.FromTo<SourceType, TargetType>()
                .Map(a => a.SourceString, b => b.TargetString)
                .Sub(a => a.SourceSub, b => b, sM => sM
                    .Sub(a => a.SourceSubSub, b => b, sM2 => sM2
                        .Map(a => a.SourceString2, b => b.TargetString2)
                        .Map(a => a.SourceInt2, b => b.TargetInt2)))

                .FromEnum(a => a.SourceSubs, b => b, eM => eM.UsingArrayConcatAdder()
                    .First(a => a.NutrientKey == "NN" && a.NutrientAmount == 69)
                    .Map(a => a.NutrientAmount, b => b.TargetInt)
                    .Sub(a => a.SourceSubSub, b => b, sM => sM
                        .Map(a => a.SourceString2, b => b.TargetString3)))

                .ToEnum(a => a, b => b.TargetSubs, eM => eM.UsingArrayConcatAdder()
                    .First(b => b.NutrientKey == "JJ")
                    .Map(a => a.SourceInt, b => b.NutrientAmount))

                //.Enums(a => a.SourceEnum, b => b.TargetEnum, eM => eM.UsingArrayConcatAdder()
                //    .First(a => a.SourceEnumString == "a", b => b.TargetEnumString == "b")
                //    .Map(a => a.SourceEnumInt, b => b.TargetEnumInt))
                    
                .Enums(a => a.SourceEnum, b => b.TargetEnum, eM => eM.UsingArrayConcatAdder()
                    .Map(a => a.SourceEnumString, b => b.TargetEnumString)
                    .Map(a => a.SourceEnumInt, b => b.TargetEnumInt));

            mapping.TranslateAToB(testSource, testTarget);
            mapping.TranslateBToA(testSource2, testTarget);
            var extract = Metadata.SimpleStringExtraction.ExtractFrom(mapping);

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

            Console.ReadKey();
        }
    }
}
