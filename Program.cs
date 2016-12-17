﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Reflection;

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

            var mapping = GetMapping.FromTo<SourceType, TargetType>()
                .Map(a => a.SourceString, b => b.TargetString)
                .WithSubMapping(a => a.SourceSub, b => b, h => GetMapping.SubFromHandle(h)
                    .WithSubMapping(a => a.SourceSubSub, b => b, h2 => GetMapping.SubFromHandle(h2)
                        .Map(a => a.SourceString2, b => b.TargetString2)));

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
