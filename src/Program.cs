using System;

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
                }
            };

            var testTarget = new TargetType();
            var testSource2 = new SourceType();

            var mapping = Mapping.FromTo<SourceType, TargetType>()
                .Map(a => a.SourceString, b => b.TargetString)
                .WithSubMapping(a => a.SourceSub, b => b, h => Mapping.Using(h).Do()
                    .WithSubMapping(a => a.SourceSubSub, b => b, h2 => Mapping.Using(h2).Do()
                        .Map(a => a.SourceString2, b => b.TargetString2)
                        .Map(a => a.SourceInt2, b => b.TargetInt2)));

            //var mapping2 = GetMapping.FromTo<SourceType, TargetType>()
            //    .Map(s => s.SourceString, t => t.TargetString)
            //    .WithSubMapping(s => s.SourceSub, t => t, h => GetMapping.SubFromHandle(h)
            //        .WithSubMapping(s => s.SourceSubSub, t => t, h2 => GetMapping.SubFromHandle(h2)
            //            .Map(s => s.SourceString2, b => b.TargetString2)));
                    

            mapping.TranslateAToB(testSource, testTarget);
            mapping.TranslateBToA(testSource2, testTarget);
            var docs = mapping.GetDocumentation();

            Console.WriteLine("==== MAPPING TESTS ====");
            Console.WriteLine($"testTarget.TargetString: {testTarget.TargetString}");
            Console.WriteLine($"testTarget.TargetString2: {testTarget.TargetString2}");
            Console.WriteLine("");
            Console.WriteLine($"testSource2.SourceString: {testSource.SourceString}");
            Console.WriteLine($"testSource2.SourceSub.SourceSubSub.SourceString2: {testSource.SourceSub.SourceSubSub.SourceString2}");
            Console.WriteLine("");
            Console.WriteLine("==== DOC TESTS ====");
            Console.WriteLine(docs);

            Console.ReadKey();
        }
    }
}
