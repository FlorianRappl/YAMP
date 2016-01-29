namespace YAMP.Core.Tests
{
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using YAMP.Help;

    [TestFixture]
    public class Examples : Base
    {
        [Datapoints]
        public String[] examples = ObtainExamples();

        [Theory]
        public void ExampleShouldBeValidYampCode(String example)
        {
            var parser = Parser.Parse(example);
            parser.Execute();
        }

        static String[] ObtainExamples()
        {
            var doc = Documentation.Create(Parser.PrimaryContext);
            var queries = new List<String>();

            foreach (var section in doc.Sections)
            {
                if (section is HelpFunctionSection)
                {
                    var f = (HelpFunctionSection)section;

                    foreach (var usage in f.Usages)
                    {
                        foreach (var example in usage.Examples)
                        {
                            if (!example.IsFile)
                            {
                                queries.Add(example.Example);
                            }
                        }
                    }
                }
            }

            return queries.ToArray();
        }
    }
}
